using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
