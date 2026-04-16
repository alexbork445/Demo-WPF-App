using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
