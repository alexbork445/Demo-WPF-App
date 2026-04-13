using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Producer
{
    public int ProducerId { get; set; }

    public string ProducerName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
