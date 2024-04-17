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

builder.Services.AddScoped<IEventGenerationService>(sp =>
{
    var eventRepository = sp.GetRequiredService<IEventRepository>();
    var logger = sp.GetRequiredService<ILogger<EventGenerationService>>();
    return new EventGenerationService(eventRepository, logger);
}); 

builder.Services.AddScoped<IEventService>(sp =>
{
    var eventRepository = sp.GetRequiredService<IEventRepository>();
    var logger = sp.GetRequiredService<ILogger<EventService>>();
    return new EventService(eventRepository, logger);
}); 

builder.Services.AddScoped<IUserService>(sp =>
{
    var userRepository = sp.GetRequiredService<IUserRepository>();
    var logger = sp.GetRequiredService<ILogger<UserService>>();
    return new UserService(userRepository, logger);
}); 

builder.Services.AddScoped<IVisitorPlacementService>(sp =>
{
    var visPlacementRepository = sp.GetRequiredService<IVisitorPlacementRepository>();
    var logger = sp.GetRequiredService<ILogger<VisitorPlacementService>>();
    return new VisitorPlacementService(visPlacementRepository, logger);
}); 

builder.Services.AddScoped<IUserProfileService>(sp =>
{
    var userProfileDataRepository = sp.GetRequiredService<IUserProfileDataRepository>();
    var logger = sp.GetRequiredService<ILogger<UserProfileService>>();
    return new UserProfileService(userProfileDataRepository, logger);
});


builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddScoped<IEventRepository>(sp =>
{
    var userRepository = sp.GetRequiredService<IUserRepository>();
    return new EventRepository(connectionString, userRepository);
}); 
builder.Services.AddScoped<IUserProfileDataRepository>(_ => new UserProfileDataRepository(connectionString));
builder.Services.AddScoped<IVisitorPlacementRepository>(_ => new VisitorPlacementRepository(connectionString));


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
