using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Food
{
    public int FoodId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public bool IsAvailable { get; set; }

    public int PopularityScore { get; set; }

    public string? ImageUrl { get; set; }


    public DateTime CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
