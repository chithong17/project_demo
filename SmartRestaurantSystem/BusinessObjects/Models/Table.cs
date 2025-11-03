using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Table
{
    public int TableId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public byte Status { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
