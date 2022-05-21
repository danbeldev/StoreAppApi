using StoreAppApi.models.product.enums;
using StoreAppApi.models.product.review;
using StoreAppApi.models.user;
using StoreAppApi.models.сompany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAppApi.models.product
{
    public class Product
    {
        [Key] public int Id { get; set; }
        public string Icon { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        public int? Price { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public ProductType ProductType { get; set; }
        [Required] public Genre Genre { get; set; }
        public virtual List<Image> Images { get; set; } = new List<Image>();
        public Video Video { get; set; }
        [JsonIgnore, Required] public Сompany Company { get; set; }
        [JsonIgnore] public virtual List<Review> Reviews { get; set; } = new List<Review>();
        [JsonIgnore] public virtual List<BaseUser> UserDownload { get; set; } = new List<BaseUser>();
    }
}
