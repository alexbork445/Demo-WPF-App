using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
