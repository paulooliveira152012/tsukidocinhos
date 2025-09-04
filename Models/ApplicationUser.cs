using Microsoft.AspNetCore.Identity;

namespace BrigadeiroApp.Models;

public class ApplicationUser : IdentityUser
{
    // Campos adicionais (exemplo)
    public string DisplayName { get; set; } = "";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
