using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using NetShip;
using NetShip.Endpoints;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;

var builder = WebApplication.CreateBuilder(args);
// inicio de area de los servicios 

builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseNpgsql("name = DefaultConnection"));

builder.Services.AddCors(options => options.AddDefaultPolicy(
    configuration => {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }));

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

builder.Services.AddScoped<IFileStorage, FileLocalStorage>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

// fin de area de los servicios 
var app = builder.Build();

// inicio de area de los middleware

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseCors();
app.UseOutputCache();

app.MapGroup("/category").MapCategories();
app.MapGroup("/product").MapProducts();



// fin de area de los middleware
app.Run();
