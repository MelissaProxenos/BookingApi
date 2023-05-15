using Booking.DataStore.Interfaces;
using Booking.DataStore.Repositories;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// add logging
builder.Logging.AddJsonConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Booking API", 
        Version = "v1", 
        Description = "An API to manage bookings", 
        Contact = new OpenApiContact
            {
            Name = "Melissa Proxenos",
            Email = "melissa.proxenos@gmail.com"},
        License = new OpenApiLicense
        {
            Name = "Use under MIT",
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
