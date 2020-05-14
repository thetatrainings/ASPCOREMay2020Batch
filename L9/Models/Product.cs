using System;
using System.Collections.Generic;

namespace L9.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Status { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Images { get; set; }
        public double? CurrentStock { get; set; }
        public double? Price { get; set; }
        public string Sku { get; set; }
        public bool? OnSale { get; set; }

        public string ExtraFeatures { get; set; }
    }
}
