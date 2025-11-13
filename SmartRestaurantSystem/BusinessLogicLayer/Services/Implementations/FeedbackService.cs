using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo = new FeedbackRepository();

        public List<Feedback> GetAll() => _repo.GetAll();
        public void Add(Feedback f) => _repo.Add(f);

        public List<Feedback> GetAllWithIncludes()
        {
            using var context = new SmartRestaurantDbContext();
            return context.Feedbacks
                .Include(f => f.Food)
                .Include(f => f.Customer)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();
        }

    }
}
