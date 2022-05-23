using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.models.сompany.product
{
    public class Country
    {
        [Key] public int Id { get; set; }
        [Required] public string CountryTitle { get; set; }
        [Required] public string Continent { get; set; }
    }
}
