using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.user
{
    public class RegistrationDTO
    {
        [Required, MaxLength(256)] public string Username { get; set; }
        [Required, MaxLength(128)] public string Email { get; set; }
        [Required, MaxLength(128)] public string Password { get; set; }
    }
}