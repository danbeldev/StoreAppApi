using Microsoft.AspNetCore.Http;
using StoreAppApi.models.product;
using StoreAppApi.models.product.enums;
using StoreAppApi.models.сompany.product.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.product
{
    public class ProductPostDTO
    {
        [Required] public string Title { get; set; }
        [Required] public string ShortDescription { get; set; }
        [Required] public string FullDescription { get; set; }
        [Required] public Boolean Advertising { get; set; }
        [Required] public string Version { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string PrivacyPolicyWebUrl { get; set; }
        public virtual List<SocialNetwork> SocialNetwork { get; set; } = new List<SocialNetwork>();
        [Required] public AgeRating AgeRating { get; set; }
        public int? Price { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public ProductType ProductType { get; set; }
        [Required] public int GenreId { get; set; }
    }
}
