using Cinema.DataAccess;
using Cinema.Identity;
using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredLength = 4;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;


    }
    
    )
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IMovieRepo, MovieRepository>();
builder.Services.AddScoped<IActorRepo, ActorRepository>();
builder.Services.AddScoped<ICategoryrepo, CategoryRepo>();
builder.Services.AddScoped<IEcinemaRepo, EcinemaRepo>();
builder.Services.AddScoped<IActorMovieRepo, ActorMovieRepo>();
builder.Services.AddScoped<IcartRepo, CartRepo>();
builder.Services.AddScoped<ICartItemsRepo, CartItemRepo>();
builder.Services.AddScoped<IorderRepo, OrderRepo>();
builder.Services.AddScoped<IorderItemsRepo, OrderItemRepo>();
builder.Services.AddScoped<StripeService>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
