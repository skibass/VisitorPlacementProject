using DataAcces;
using Interfaces.Repositories;
using Interfaces.Service;
using Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

string connectionString = "Server=127.0.0.1;Database=vpt;Uid=root;Pwd=;";


// Repositories
builder.Services.AddTransient<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddTransient<IEventRepository>(sp =>
{
    var userRepository = sp.GetRequiredService<IUserRepository>();
    return new EventRepository(connectionString, userRepository);
}); 
builder.Services.AddTransient<IUserProfileDataRepository>(_ => new UserProfileDataRepository(connectionString));
builder.Services.AddTransient<IVisitorPlacement>(_ => new VisitorPlacementRepository(connectionString));

// Services
builder.Services.AddTransient<IEventGenerationService, EventGenerationService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IUserProfileService, UserProfileService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IVisitorPlacementService, VisitorPlacementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
