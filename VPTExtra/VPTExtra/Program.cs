using Interfaces;
using Logic.Services;
using DataAcces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<IEventManagement, EventManagementService>();
string connectionString = "Server=127.0.0.1;Database=vpt;Uid=root;Pwd=;";

builder.Services.AddTransient<IEventRepository>(sp =>
{
    // Create and return an instance of EventRepository with the connection string parameter
    return new EventRepository(connectionString);
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
