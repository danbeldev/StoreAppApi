using StoreAppApi.models.product;
using StoreAppApi.models.product.review;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.user
{
    public class BaseUserDTO
    {
        [Required, MaxLength(256)] public string Username { get; set; }
        [Required, MaxLength(128)] public string Email { get; set; }
        public string Photo { get; set; }
        public virtual string Role => "BaseUser";
    }
}
