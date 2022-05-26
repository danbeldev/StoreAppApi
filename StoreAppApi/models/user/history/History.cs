using StoreAppApi.models.product;
using StoreAppApi.models.user.history.enums;
using StoreAppApi.models.сompany;
using StoreAppApi.models.сompany.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAppApi.models.user.history
{
    public class History
    {
        [Key] public int Id { get; set; }
        [Required] public HistoryType Type { get; set; }
        [Required] public DateTime Date { get; set; }

        public Event Event { get; set; }
        public Сompany Сompany { get; set; }
        public Product Product { get; set; }
    }
}
