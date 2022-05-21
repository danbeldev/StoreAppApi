using StoreAppApi.models.product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAppApi.models.сompany
{
    public class Сompany
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        public string Icon { get; set; }
        [Required] public DateTime DateCreating { get; set; }

        [JsonIgnore] public List<Product> Products { get; set; } = new List<Product>();
    }
}
