using Interfaces;
using Logic.Services;
using DataAcces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<EventGenerationService>();
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

builder.Services.AddTransient<IEventRepository>(sp =>
{
    return new EventRepository(connectionString, userRepository);
});

builder.Services.AddTransient<IVisitorPlacement>(sp =>
{
    return new VisitorPlacementRepository(connectionString);
});

builder.Services.AddTransient<IUserRepository>(sp =>
{
    return new UserRepository(connectionString);
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
