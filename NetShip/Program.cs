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
builder.Services.AddScoped<DigitalMenuService>();


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
builder.Services.AddScoped<ICatalogsRepository, CatalogsRepository>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();

builder.Services.AddScoped<IPlatformsRepository, PlatformsRepository>();
builder.Services.AddScoped<IModifiersGroupsRepository, ModifiersGroupsRepository>();
builder.Services.AddScoped<IModifiersRepository, ModifiersRepository>();




builder.Services.AddScoped<QrCodeService>();

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

builder.Services.AddCors(options => options.AddDefaultPolicy(
    builder =>
    {
        builder.AllowAnyOrigin()//WithOrigins("http://localhost:64048", "http://localhost:52476")
               .AllowAnyHeader()
               .AllowAnyMethod();
    }));


// fin de area de los servicios 
var app = builder.Build();

// inicio de area de los middleware

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseCors();
app.UseOutputCache();

app.UseAuthorization();

app.MapGroup("/api/category").MapCategories();
app.MapGroup("/api/product").MapProducts();
app.MapGroup("/api/user").MapUsers();
app.MapGroup("/api/catalog").MapCatalogs();
app.MapGroup("/api/brand").MapBrands();
app.MapGroup("/api/branch").MapBranches();
app.MapGroup("/api/item").MapItems();
app.MapGroup("/api/platform").MapPlatforms();
app.MapGroup("/api/modifiersGroup").MapModifiersGroups();
app.MapGroup("/api/modifier").MapModifiers();
app.MapGroup("/api/digital-menu").MapDigitalMenu();



// fin de area de los middleware
app.Run();
