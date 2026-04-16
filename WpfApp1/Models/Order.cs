using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly? OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int? PickupPointId { get; set; }

    public int? UserId { get; set; }

    public string? Code { get; set; }

    public int? OrderStatusId { get; set; }

    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

    public virtual OrderStatus? OrderStatus { get; set; }

    public virtual PickupPoint? PickupPoint { get; set; }

    public virtual User? User { get; set; }
}
