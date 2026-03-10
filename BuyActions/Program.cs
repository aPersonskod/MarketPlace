using System.Reflection;
using System.Text;
using BuyActions;
using BuyActions.Services;
using BuyActions.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors();

if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("Grpc:UsersDev"));
    builder.Services.Configure<ShoppingCartSettings>(builder.Configuration.GetSection("Grpc:ShoppingCartsDev"));
    builder.Services.Configure<ProductCatalogSettings>(builder.Configuration.GetSection("Grpc:ProductCatalogsDev"));
    builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Auth:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Auth:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:Key"]!)),
            ValidateIssuerSigningKey = true
        });
    builder.Services.AddAuthorization();
    builder.Services.AddDbContext<DataContext>(o 
        => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnectionDev")));
}
else
{
    builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("Grpc:Users"));
    builder.Services.Configure<ShoppingCartSettings>(builder.Configuration.GetSection("Grpc:ShoppingCarts"));
    builder.Services.Configure<ProductCatalogSettings>(builder.Configuration.GetSection("Grpc:ProductCatalogs"));
    builder.Services.AddDbContext<DataContext>(o 
        => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
}

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));  

//builder.Services.AddSingleton<ShoppingCartClientService>();
//builder.Services.AddSingleton<UserClientService>();
//builder.Services.AddTransient<IProductCatalog, ProductCatalogClientService>();
//builder.Services.AddTransient<IBuyService, BuyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();