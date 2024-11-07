using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace StoreApi.Entities;

public class User : IdentityUser
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    public Customer? Customer { get; set; }
}