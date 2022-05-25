using StoreAppApi.models.сompany.Event.enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.company.Event
{
    public class EventPostDTO
    {
        [Required] public string Title { get; set; }
        [Required] public string ShortDescription { get; set; }
        [Required] public string FullDescription { get; set; }
        [Required] public int ProductId { get; set; }
        public string WebUrl { get; set; }
    }
}
