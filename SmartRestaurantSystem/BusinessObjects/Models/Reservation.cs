using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int? CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Phone { get; set; }

    public int? TableId { get; set; }

    public int NumberOfPeople { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public byte Status { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Table Table { get; set; } = null!;
}
