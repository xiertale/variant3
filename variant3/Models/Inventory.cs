using System;
using System.Collections.Generic;

namespace variant3.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public string InventoryNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly PublicationDate { get; set; }

    public string State { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
