using DataAcces;
using Interfaces.Repositories;
using Logic.Services;
using Logic.Services.EventGeneration;
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

builder.Services.AddTransient<EventGenerationService>();
builder.Services.AddTransient<PartGenerationService>();
builder.Services.AddTransient<RowGenerationService>();
builder.Services.AddTransient<ChairGenerationService>();

builder.Services.AddTransient<EventService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<VisitorPlacementService>();
builder.Services.AddTransient<UserProfileService>();

builder.Services.AddTransient<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddTransient<IEventRepository>(sp =>
{
    var userRepository = sp.GetRequiredService<IUserRepository>();
    return new EventRepository(connectionString, userRepository);
}); 
builder.Services.AddTransient<IUserProfileDataRepository>(_ => new UserProfileDataRepository(connectionString));
builder.Services.AddTransient<IVisitorPlacement>(_ => new VisitorPlacementRepository(connectionString));


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
