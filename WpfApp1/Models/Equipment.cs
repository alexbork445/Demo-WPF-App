using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string Article { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string RentalUnit { get; set; } = null!;

    public decimal RentalCost { get; set; }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public int TypeId { get; set; }

    public decimal? Discount { get; set; }

    public int AvailableQuantity { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual EquipmentType Type { get; set; } = null!;
}
