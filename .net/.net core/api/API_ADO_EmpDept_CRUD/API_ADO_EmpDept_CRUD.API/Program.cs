using API_ADO_EmpDept_CRUD.Repository.Data;
using API_ADO_EmpDept_CRUD.Repository.Repositories;
using API_ADO_EmpDept_CRUD.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register ADO.NET Connection Factory
builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

// Register Repositories from Class Library
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Register Services from Class Library
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
