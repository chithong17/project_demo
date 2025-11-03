using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public byte Method { get; set; }

    public decimal PaidAmount { get; set; }

    public DateTime PaidAt { get; set; }

    public string? Note { get; set; }

    public virtual Order Order { get; set; } = null!;
}
