using StoreAppApi.models.user.history.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAppApi.DTOs.user.history
{
    public class HistoryPostDTO
    {
        [Required] public HistoryType Type { get; set; }

        public int EventId { get; set; }
        public int СompanyId { get; set; }
        public int ProductId { get; set; }
    }
}
