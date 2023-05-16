using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using shukersal_backend.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Enable cors
var AllowOrigin = "AllowOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowOrigin,
                      policy =>
                      {
                          policy
                          .WithOrigins("*") // Remove in production
                          //.WithOrigins(
                          //    // Backend (Swagger)
                          //    "https://localhost:7273/",
                          //    "http://localhost:7273/",
                          //    // Frontned
                          //    "https://localhost:3000/",
                          //    "http://localhost:3000/"
                          //)
                          .AllowAnyHeader()
                            .AllowAnyMethod()
                          ;
                      });
});

// Generate lowercase URLs
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<MarketDbContext>(opt =>
    opt.UseInMemoryDatabase("MarketDbContext"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Tocken",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});


// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowOrigin);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
