using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string? SupplierName { get; set; }

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();

    override public string ToString()
    {
        return SupplierName;
    }
}
