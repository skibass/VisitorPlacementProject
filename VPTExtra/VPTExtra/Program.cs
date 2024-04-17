using DataAcces;
using Interfaces.Repositories;
using Interfaces.Logic;
using Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

string connectionString = "Server=127.0.0.1;Database=vpt;Uid=root;Pwd=;";

builder.Services.AddTransient<IEventGenerationService>(sp =>
{
    var eventRepository = sp.GetRequiredService<IEventRepository>();
    return new EventGenerationService(eventRepository);
}); 

builder.Services.AddTransient<IEventService>(sp =>
{
    var eventRepository = sp.GetRequiredService<IEventRepository>();
    return new EventService(eventRepository);
}); 

builder.Services.AddTransient<IUserService>(sp =>
{
    var userRepository = sp.GetRequiredService<IUserRepository>();
    return new UserService(userRepository);
}); 

builder.Services.AddTransient<IVisitorPlacementService>(sp =>
{
    var visPlacementRepository = sp.GetRequiredService<IVisitorPlacementRepository>();
    return new VisitorPlacementService(visPlacementRepository);
}); 

builder.Services.AddTransient<IUserProfileService>(sp =>
{
    var userProfileDataRepository = sp.GetRequiredService<IUserProfileDataRepository>();
    return new UserProfileService(userProfileDataRepository);
});


builder.Services.AddTransient<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddTransient<IEventRepository>(sp =>
{
    var userRepository = sp.GetRequiredService<IUserRepository>();
    return new EventRepository(connectionString, userRepository);
}); 
builder.Services.AddTransient<IUserProfileDataRepository>(_ => new UserProfileDataRepository(connectionString));
builder.Services.AddTransient<IVisitorPlacementRepository>(_ => new VisitorPlacementRepository(connectionString));


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
