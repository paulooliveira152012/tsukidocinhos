using Microsoft.EntityFrameworkCore;
using BrigadeiroApp.Data;
using BrigadeiroApp.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Caminho do SQLite (igual você já fez)
var dataDir = Environment.GetEnvironmentVariable("DATA_DIR");
var dbPath = dataDir is not null
    ? Path.Combine(dataDir, "app.db")
    : Path.Combine(builder.Environment.ContentRootPath, "app.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
var cs = $"Data Source={dbPath}";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(cs));

// 🔐 ASP.NET Core Identity (usuários/roles + UI)
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages(); // necessário para Identity UI

// Blazor
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

// ordem correta: Auth -> Authorize -> Antiforgery
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Blazor + Identity UI pages
app.MapRazorComponents<BrigadeiroApp.Components.App>()
   .AddInteractiveServerRenderMode();
app.MapRazorPages(); // expõe /Identity/Account/Login etc.

// Migrações + seed de usuários/roles
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

       // SEED TIPOS (só se estiver vazio)
    if (!db.BrigadeiroTypes.Any())
    {
        db.BrigadeiroTypes.AddRange(
            new BrigadeiroType { Name = "Tradicional", UnitPrice = 2.50m, UnitCost = 1.00m, Active = true },
            new BrigadeiroType { Name = "Beijinho",   UnitPrice = 2.50m, UnitCost = 1.10m, Active = true },
            new BrigadeiroType { Name = "Ninho",      UnitPrice = 3.00m, UnitCost = 1.30m, Active = true },
            new BrigadeiroType { Name = "Nutella",    UnitPrice = 3.50m, UnitCost = 1.80m, Active = true }
        );
        await db.SaveChangesAsync();
    }

    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // cria roles se não existirem
    foreach (var role in new[] { "Marcos", "Gabi" })
        if (!await roleMgr.RoleExistsAsync(role))
            await roleMgr.CreateAsync(new IdentityRole(role));

    // cria usuários iniciais (troque emails/senhas depois)
    async Task EnsureUser(string email, string password, string role)
    {
        var u = await userMgr.FindByEmailAsync(email);
        if (u is null)
        {
            u = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var ok = await userMgr.CreateAsync(u, password);
            if (!ok.Succeeded) throw new Exception(string.Join(';', ok.Errors.Select(e => e.Description)));
        }
        if (!await userMgr.IsInRoleAsync(u, role))
            await userMgr.AddToRoleAsync(u, role);
    }

    await EnsureUser("marcos@example.com", "Marcos123!", "Marcos");
    await EnsureUser("gabi@example.com",   "Gabi123!",   "Gabi");
}

app.Run();