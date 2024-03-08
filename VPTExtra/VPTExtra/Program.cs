using Interfaces;
using Logic.Services;
using DataAcces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<EventGenerationService>();
builder.Services.AddTransient<IEventManagement, EventManagementService>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

string connectionString = "Server=127.0.0.1;Database=vpt;Uid=root;Pwd=;";

IVisitorRepository visitorRepository = new VisitorRepository(connectionString);
IEventRepository eventRepository = new EventRepository(connectionString, visitorRepository);

builder.Services.AddTransient<IEventRepository>(sp =>
{
    return new EventRepository(connectionString, visitorRepository);
});

builder.Services.AddTransient<IVisitorPlacement>(sp =>
{
    return new VisitorPlacementRepository(connectionString);
});

builder.Services.AddTransient<IVisitorRepository>(sp =>
{
    return new VisitorRepository(connectionString);
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


app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
