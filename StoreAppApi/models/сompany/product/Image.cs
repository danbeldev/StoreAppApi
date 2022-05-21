using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.models.product
{
    public class Image
    {
        [Key] public int Id { get; set; }
        [Required] public string ImageUrl { get; set; }
    }
}
