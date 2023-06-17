using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.DomainLayer.notifications;
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
// Old db
//builder.Services.AddDbContext<MarketDbContext>(opt =>
//    opt.UseInMemoryDatabase("MarketDbContext"));
var connectionString = builder.Configuration.GetConnectionString("DockerConnection_Shoval");
builder.Services.AddDbContext<MarketDbContext>(opt =>
    opt.UseSqlServer(connectionString));

try
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Connected successfully!");
        connection.Close();
    }
}
catch (SqlException ex)
{
    Console.WriteLine("Connection failed. Error: " + ex.Message);
}

// Add database migrations
builder.Services.AddDbContext<MarketDbContext>(opt =>
    opt.UseSqlServer(connectionString).EnableSensitiveDataLogging());
// --------------------------- DB -

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
//for the notifiactions
builder.Services.AddSignalR();
builder.Services.AddTransient<NotificationController>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// To automatically migrate the database
/*using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetService<MarketDbContext>();
    dbContext?.Database.Migrate();
    if (dbContext != null)
        await BootFileRunner.Run(dbContext);
}*/



app.MapControllers();

app.UseRouting();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseCors(AllowOrigin);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/chatHub");
});

app.Run();


