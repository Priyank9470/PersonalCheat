using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

//using Microsoft.OpenApi;
//using Microsoft.OpenApi.Models;
using ServiceManagement.Controllers;
using ServiceManagement.Core.Entity;
using ServiceManagement.MappingProfile;
using ServiceManagement.Repository;
using ServiceManagement.Repository.Classes;
using ServiceManagement.Repository.Interface;
using ServiceManagement.Service.Classes;
using ServiceManagement.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ServiceManagementDBContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection")), ServiceLifetime.Transient);

// Add services to the container.
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(configuration => configuration.AddMaps(typeof(Program).Assembly));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Service Management API",
		Version = "v1"
	});

	// 1. Define the Scheme
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter your JWT token in the format: Bearer {your token}"
	});

	// 2. Add Security Requirement using the new document callback syntax
	options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecuritySchemeReference("Bearer", document),
			new List<string>() // Must be List<string>, not string[]
        }
	});
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	//app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service Management API");
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
