using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? TableId { get; set; }

    public int? StaffId { get; set; }

    public int? CustomerId { get; set; }

    public int? ReservationId { get; set; }

    public DateTime OrderTime { get; set; }

    public byte Status { get; set; }

    public decimal Discount { get; set; }

    public string? Note { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Payment? Payment { get; set; }

    public virtual Reservation? Reservation { get; set; }

    public virtual User Staff { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
