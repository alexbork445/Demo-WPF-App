using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class OrderDetails
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Amount { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
