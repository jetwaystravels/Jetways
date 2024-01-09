using Microsoft.CodeAnalysis.Options;

var builder = WebApplication.CreateBuilder(args);

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
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
name: "default",
pattern: "{controller=FlightSearchIndex}/{action=Index}/{id?}");

app.Run();
