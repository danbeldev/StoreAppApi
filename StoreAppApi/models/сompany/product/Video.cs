using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.models.product
{
    public class Video
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string VideoUrl { get; set; }
    }
}
