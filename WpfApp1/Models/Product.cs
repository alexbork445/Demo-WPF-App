using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Article { get; set; }

    public decimal? Price { get; set; }

    public int? SupplierId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? ProductTypeId { get; set; }

    public int? Discount { get; set; }

    public string? UnitOfMeasure { get; set; }

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

    public virtual ProductType? ProductType { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
