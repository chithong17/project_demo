using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class OrderTotal
{
    public int OrderId { get; set; }

    public DateTime OrderTime { get; set; }

    public int TableId { get; set; }

    public int StaffId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal Discount { get; set; }

    public decimal? TotalBeforeTax { get; set; }
}
