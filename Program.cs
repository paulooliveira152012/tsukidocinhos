using Microsoft.EntityFrameworkCore;
using BrigadeiroApp.Data;

var builder = WebApplication.CreateBuilder(args);

// ➜ Cria caminho ABSOLUTO para o arquivo app.db dentro da pasta do app
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "app.db");

// ➜ Se existir ConnectionStrings:Default, usa; senão, cai no fallback com dbPath
var connString = builder.Configuration.GetConnectionString("Default")
                ?? $"Data Source={dbPath}";

// EF Core
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(connString));

// Blazor (template novo)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
// app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<BrigadeiroApp.Components.App>()
   .AddInteractiveServerRenderMode();

// ➜ APLICA MIGRATIONS AUTOMATICAMENTE AO SUBIR
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    app.Logger.LogInformation("SQLite DB em: {path}", dbPath);
    db.Database.Migrate(); // cria/atualiza tabelas
}

app.Run();
