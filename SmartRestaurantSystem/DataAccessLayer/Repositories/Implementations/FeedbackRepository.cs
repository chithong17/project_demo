using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly SmartRestaurantDbContext _context = new();

        public List<Feedback> GetAll()
        {
            return _context.Feedbacks
                .Include(f => f.Food) // thêm dòng này
                .AsNoTracking()
                .ToList();
        }

        public void Add(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
        }
    }
}
