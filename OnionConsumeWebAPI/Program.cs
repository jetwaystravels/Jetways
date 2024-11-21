using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnionConsumeWebAPI.ErrorHandling;
using OnionConsumeWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);
// Add configuration files
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    if (string.IsNullOrEmpty(settings.ConnectionString))
        throw new InvalidOperationException("MongoDB connection string is not set.");

    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});
builder.Services.AddScoped<MongoDbService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.BuildServiceProvider();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.Environment.EnvironmentName = "Production";
builder.Services.AddSession(Option =>
{
    Option.IdleTimeout = TimeSpan.FromMinutes(15);
    Option.Cookie.HttpOnly = true;
    Option.Cookie.IsEssential = true;

});

builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = "localhost:6379";
});


builder.Services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
////if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{

//    app.UseExceptionHandler("/Home/Error");
//}
app.UseStaticFiles();
//app.UseMiddleware<ExceptionHandling>();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
//app.UseMiddleware<RedirectToLogin>();
app.MapControllerRoute(
name: "default",
pattern: "{controller=FlightSearchIndex}/{action=Index}/{id?}");

app.Run();
