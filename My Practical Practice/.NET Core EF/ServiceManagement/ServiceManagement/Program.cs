using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

//using Microsoft.OpenApi;
//using Microsoft.OpenApi.Models;
using ServiceManagement.Controllers;
using ServiceManagement.Core.Entity;
using ServiceManagement.MappingProfile;
using ServiceManagement.Repository;
using ServiceManagement.Repository.Classes;
using ServiceManagement.Repository.Interface;
using ServiceManagement.Service.Authentication;
using ServiceManagement.Service.Classes;
using ServiceManagement.Service.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ServiceManagementDBContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection")), ServiceLifetime.Transient);

// Add services to the container.
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtTokenGeneration>();

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(configuration => configuration.AddMaps(typeof(Program).Assembly));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	 .AddJwtBearer(options =>
	 {
		 options.RequireHttpsMetadata = false;
		 options.SaveToken = true;

		 options.TokenValidationParameters = new TokenValidationParameters
		 {
			 ValidateIssuer = true,
			 ValidateAudience = true,
			 ValidateLifetime = true,
			 ValidateIssuerSigningKey = true,

			 ValidIssuer = builder.Configuration["Jwt:Issuer"],
			 ValidAudience = builder.Configuration["Jwt:Audience"],
			 IssuerSigningKey = new SymmetricSecurityKey(
				 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
		 };
	 });

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
