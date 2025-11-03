using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class Feedback
{
    [NotMapped]
    public List<string> RatingStars { get; set; } = new();


    public int FeedbackId { get; set; }

    public int? CustomerId { get; set; }

    public int? FoodId { get; set; }

    public int? OrderId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? Customer { get; set; }

    public virtual Food? Food { get; set; }

    public virtual Order? Order { get; set; }
}
