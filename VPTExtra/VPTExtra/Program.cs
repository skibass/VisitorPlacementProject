using DataAcces;
using Interfaces.Repositories;
using Interfaces.Logic;
using Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Interfaces.DataAcces.Repositories;
using Logic.Services.API;
using System.Net.Http;

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
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://192.168.2.114:5147/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    };
});

string connectionString = "Server=127.0.0.1;Database=vpt;Uid=root;Pwd=;";

builder.Services.AddScoped<IEventGenerationService>(_ =>
{
    var eventRepository = _.GetRequiredService<IEventRepository>();
    var logger = _.GetRequiredService<ILogger<EventGenerationService>>();
    return new EventGenerationService(eventRepository, logger);
});

builder.Services.AddScoped<IEventEditService>(_ =>
{
    var editEventRepository = _.GetRequiredService<IEventEditRepository>();
    var logger = _.GetRequiredService<ILogger<EventEditService>>();
    return new EventEditService(editEventRepository, logger);
});

builder.Services.AddScoped<IEventService>(_ =>
{
    var eventRepository = _.GetRequiredService<IEventRepository>();
    var logger = _.GetRequiredService<ILogger<EventService>>();
    return new EventService(eventRepository, logger);
}); 

builder.Services.AddScoped<IUserService>(_ =>
{
    var userRepository = _.GetRequiredService<IUserRepository>();
    var logger = _.GetRequiredService<ILogger<UserService>>();
    return new UserService(userRepository, logger);
}); 

builder.Services.AddScoped<IVisitorPlacementService>(_ =>
{
    var visPlacementRepository = _.GetRequiredService<IVisitorPlacementRepository>();
    var logger = _.GetRequiredService<ILogger<VisitorPlacementService>>();
    return new VisitorPlacementService(visPlacementRepository, logger);
}); 

builder.Services.AddScoped<IUserProfileService>(_ =>
{
    var userProfileDataRepository = _.GetRequiredService<IUserProfileDataRepository>();
    var logger = _.GetRequiredService<ILogger<UserProfileService>>();
    return new UserProfileService(userProfileDataRepository, logger);
});



builder.Services.AddScoped<TestApiService>();


builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddScoped<IEventEditRepository>(_ => new EventEditRepository(connectionString));
builder.Services.AddScoped<IEventRepository>(_ =>
{
    var userRepository = _.GetRequiredService<IUserRepository>();
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
