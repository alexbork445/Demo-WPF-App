using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Article { get; set; } = null!;

    public decimal Price { get; set; }

    public int SupplierId { get; set; }

    public int ProducerId { get; set; }

    public int CategoryId { get; set; }

    public decimal? CurrentDiscount { get; set; }

    public int StockQuantity { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Producer Producer { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
