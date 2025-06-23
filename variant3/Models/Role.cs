using System;
using System.Collections.Generic;

namespace variant3.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? AccessRights { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
