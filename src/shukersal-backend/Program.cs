using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add DbContexts
builder.Services.AddDbContext<EventContext>(opt =>
    opt.UseInMemoryDatabase("Event"));

builder.Services.AddDbContext<ManagerContext>(opt =>
    opt.UseInMemoryDatabase("Manager"));

builder.Services.AddDbContext<MemberContext>(opt =>
    opt.UseInMemoryDatabase("Member"));

builder.Services.AddDbContext<NotificationContext>(opt =>
    opt.UseInMemoryDatabase("Notification"));

builder.Services.AddDbContext<PurchaseContext>(opt =>
    opt.UseInMemoryDatabase("Purchase"));

builder.Services.AddDbContext<ReviewContext>(opt =>
    opt.UseInMemoryDatabase("Purchase"));

builder.Services.AddDbContext<ShoppingCartContext>(opt =>
    opt.UseInMemoryDatabase("Purchase"));

builder.Services.AddDbContext<StoreContext>(opt =>
    opt.UseInMemoryDatabase("Store"));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
