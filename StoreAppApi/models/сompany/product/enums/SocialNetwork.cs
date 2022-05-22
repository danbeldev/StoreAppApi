using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.models.сompany.product.enums
{
    public class SocialNetwork
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string WebUrl { get; set; }
    }
}
