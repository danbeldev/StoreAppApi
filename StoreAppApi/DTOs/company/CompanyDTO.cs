using StoreAppApi.models.product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAppApi.DTOs.company
{
    public class CompanyDTO
    {

        public decimal Total
        {
            get
            {
                return Items.Count;
            }
        }
        public virtual List<CompanyItemDTO> Items { get; set; } = new List<CompanyItemDTO>();
    }
}
