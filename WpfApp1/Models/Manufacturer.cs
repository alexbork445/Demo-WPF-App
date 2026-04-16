using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Manufacturer
{
    public int Id { get; set; }

    public string? ManufacturerName { get; set; }

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
