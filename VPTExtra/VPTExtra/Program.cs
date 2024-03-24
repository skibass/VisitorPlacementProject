using Logic.Services;
using DataAcces;
using Interfaces.Repositories;
using Interfaces.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<EventGenerationService>();
builder.Services.AddTransient<EventService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<VisitorPlacementService>();
builder.Services.AddTransient<UserProfileService>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

string connectionString = "Server=127.0.0.1;Database=vpt;Uid=root;Pwd=;";

IUserRepository userRepository = new UserRepository(connectionString);
IEventRepository eventRepository = new EventRepository(connectionString, userRepository);
IUserProfileDataRepository profileDataRepo = new UserProfileDataRepository(connectionString);
IVisitorPlacement visitorPlacementRepo = new VisitorPlacementRepository(connectionString);

builder.Services.AddTransient<IEventRepository>(_ =>
{
    return new EventRepository(connectionString, userRepository);
});

builder.Services.AddTransient<IVisitorPlacement>(_ =>
{
    return new VisitorPlacementRepository(connectionString);
});

builder.Services.AddTransient<IUserRepository>(_ =>
{
    return new UserRepository(connectionString);
});

builder.Services.AddTransient<IUserProfileDataRepository>(s_p =>
{
    return new UserProfileDataRepository(connectionString);
});

builder.Services.AddTransient<IEventGenerationService>(_ =>
{
    return new EventGenerationService(eventRepository);
});

builder.Services.AddTransient<IEventService>(_ =>
{
    return new EventService(eventRepository);
});

builder.Services.AddTransient<IUserProfileService>(_ =>
{
    return new UserProfileService(profileDataRepo);
});

builder.Services.AddTransient<IUserService>(_ =>
{
    return new UserService(userRepository);
});

builder.Services.AddTransient<IVisitorPlacementService>(_ =>
{
    return new VisitorPlacementService(visitorPlacementRepo);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
