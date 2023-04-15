﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsListed { get; set; } // Indicates if the product is available for normal purchase
        [JsonIgnore]
        [ForeignKey("Store")]
        public long StoreId { get; set; }
        [JsonIgnore]
        public Store? Store { get; set; }
    }
}
