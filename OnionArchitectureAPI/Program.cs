using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.DbContextLayer;
using ServiceLayer.Service.Implementation;
using ServiceLayer.Service.Interface; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(con => con.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<ICity, CityService>();
builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddScoped<Ilogin, LoginService>();
builder.Services.AddScoped<ICredential, CredentialServices>();
builder.Services.AddScoped<ITicketBooking, TicketBookingServices>();
builder.Services.AddScoped<Itb_Booking, tb_BookingServices>();
builder.Services.AddScoped<IGSTDetails, GSTDetailsServices>();
builder.Services.BuildServiceProvider();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AWS Lambda support.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);


var app = builder.Build();




if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
