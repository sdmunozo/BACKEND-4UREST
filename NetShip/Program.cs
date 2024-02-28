using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetShip;
using NetShip.Endpoints;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;
using NetShip.Utilities;

var builder = WebApplication.CreateBuilder(args);
// inicio de area de los servicios 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql("name = DefaultConnection"));


builder.Services.AddIdentityCore<ApplicationUser>(options => {
    options.Password.RequiredLength = 1;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();

builder.Services.AddCors(options => options.AddDefaultPolicy(
    configuration => {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }));

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

builder.Services.AddScoped<IBrandsRepository, BrandsRepository>();
builder.Services.AddScoped<IBranchesRepository, BranchesRepository>();

builder.Services.AddScoped<IFileStorage, FileLocalStorage>();
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAuthentication().AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = Keys.GetKey(builder.Configuration).First(),
    //IssuerSigningKeys = Keys.GetAllKey(builder.Configuration),
    ClockSkew = TimeSpan.Zero
});
builder.Services.AddAuthorization();

// fin de area de los servicios 
var app = builder.Build();

// inicio de area de los middleware

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseCors();
app.UseOutputCache();

app.UseAuthorization();

app.MapGroup("/category").MapCategories();
app.MapGroup("/product").MapProducts();
app.MapGroup("/users").MapUsers();



// fin de area de los middleware
app.Run();
