using Microsoft.EntityFrameworkCore;
using BrigadeiroApp.Data;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Blazor (template novo)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); // pode comentar no dev, se quiser
app.UseStaticFiles();
app.UseRouting();

// Monta o app Blazor
app.MapRazorComponents<BrigadeiroApp.Components.App>()
   .AddInteractiveServerRenderMode();

// Endpoint opcional de saúde
app.MapGet("/ping", () => new { ok = true });

app.Run();
