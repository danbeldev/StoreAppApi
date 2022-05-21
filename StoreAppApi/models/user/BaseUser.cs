using StoreAppApi.models.product;
using StoreAppApi.models.product.review;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAppApi.models.user
{
    public class BaseUser
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(256)] public string Username { get; set; }
        [Required, MaxLength(128)] public string Email { get; set; }
        [Required, MaxLength(128)] public string Password { get; set; }
        public string Photo { get; set; }
        public virtual string Role => "BaseUser";

        [JsonIgnore] public virtual List<Review> Reviews { get; set; } = new List<Review>();
        [JsonIgnore] public virtual List<Product> ProductsDownload { get; set; } = new List<Product>();
    }
}
