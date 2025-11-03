using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        List<Feedback> GetAll();
        void Add(Feedback feedback);
    }
}
