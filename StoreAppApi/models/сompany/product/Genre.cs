using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.models.product
{
    public class Genre
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
    }
}
