using System;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.product.review
{
    public class ReviewPostDTO
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public float Rating { get; set; }
        [Required] public DateTime DatePublication { get; set; }
    }
}
