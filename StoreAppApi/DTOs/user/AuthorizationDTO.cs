using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.user
{
    public class AuthorizationDTO
    {
        [Required, MaxLength(128)] public string Email { get; set; }
        [Required, MaxLength(128)] public string Password { get; set; }
    }
}
