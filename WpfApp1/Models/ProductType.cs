using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
