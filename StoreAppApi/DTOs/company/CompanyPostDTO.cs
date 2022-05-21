using System;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.company
{
    public class CompanyPostDTO
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
    }
}
