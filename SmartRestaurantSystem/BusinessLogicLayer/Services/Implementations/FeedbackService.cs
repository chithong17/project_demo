using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo = new FeedbackRepository();

        public List<Feedback> GetAll() => _repo.GetAll();
        public void Add(Feedback f) => _repo.Add(f);
    }
}
