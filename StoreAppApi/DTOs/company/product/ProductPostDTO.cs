using Microsoft.AspNetCore.Http;
using StoreAppApi.models.product;
using StoreAppApi.models.product.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.product
{
    public class ProductPostDTO
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        public int? Price { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public ProductType ProductType { get; set; }
        [Required] public int GenreId { get; set; }
    }
}
