using Microsoft.EntityFrameworkCore;
using BrigadeiroApp.Data;

var builder = WebApplication.CreateBuilder(args);

// EF Core (se você estiver usando)
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

// app.UseHttpsRedirection(); // opcional no dev
app.UseStaticFiles();
app.UseRouting();

// Se tiver auth: app.UseAuthentication();
app.UseAuthorization();

// 🔒 Necessário para endpoints com antiforgery (Blazor, forms, etc.)
app.UseAntiforgery();

app.MapRazorComponents<BrigadeiroApp.Components.App>()
   .AddInteractiveServerRenderMode();

app.Run();
