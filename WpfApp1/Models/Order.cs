using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int AddressId { get; set; }

    public int UserId { get; set; }

    public int? PickupCode { get; set; }

    public int StatusId { get; set; }

    public virtual PickupPoint Address { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
