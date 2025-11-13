using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Admin
{
    public class RatingGroup
    {
        public int Rating { get; set; }
        public int Count { get; set; }
    }

    public class FeedbackStatisticsViewModel : ObservableObject
    {
        private readonly IFeedbackService _feedbackService;

        private List<Feedback> _allFeedbacks;

        public ObservableCollection<Feedback> FilteredFeedbacks { get; set; }
        public ObservableCollection<RatingGroup> RatingGroups { get; set; }

        public int MaxFeedbackCount => RatingGroups.Any()
            ? RatingGroups.Max(r => r.Count)
            : 1;

        // FILTER FIELDS
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        private int _selectedRating = 0;
        public int SelectedRating
        {
            get => _selectedRating;
            set { _selectedRating = value; OnPropertyChanged(); }
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // SUMMARY
        public double AvgRating => _allFeedbacks.Any()
            ? _allFeedbacks.Average(f => (double)f.Rating)
            : 0;

        public int TotalFeedbacks => _allFeedbacks.Count;

        // COMMANDS
        public ICommand ApplyFilterCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public FeedbackStatisticsViewModel()
        {
            _feedbackService = new FeedbackService();

            _allFeedbacks = new List<Feedback>();
            FilteredFeedbacks = new ObservableCollection<Feedback>();
            RatingGroups = new ObservableCollection<RatingGroup>();

            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());

            LoadData();
        }

        private void LoadData()
        {
            _allFeedbacks = _feedbackService.GetAllWithIncludes();

            BuildRatingGroups();

            ApplyFilter();

            OnPropertyChanged(nameof(AvgRating));
            OnPropertyChanged(nameof(TotalFeedbacks));
        }

        private void BuildRatingGroups()
        {
            RatingGroups.Clear();

            var groups = _allFeedbacks
                .GroupBy(f => f.Rating ?? 0)
                .OrderBy(g => g.Key)
                .Select(g => new RatingGroup
                {
                    Rating = g.Key,
                    Count = g.Count()
                })
                .ToList();

            foreach (var g in groups)
                RatingGroups.Add(g);

            OnPropertyChanged(nameof(MaxFeedbackCount));
        }

        private void ApplyFilter()
        {
            var query = _allFeedbacks.AsEnumerable();

            // Search
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string kw = SearchText.ToLower();
                query = query.Where(f =>
                    (f.Food?.Name ?? "").ToLower().Contains(kw)
                    || (f.Customer?.FullName ?? "").ToLower().Contains(kw)
                    || (f.Comment ?? "").ToLower().Contains(kw)
                );
            }

            // Rating
            if (SelectedRating > 0)
                query = query.Where(f => f.Rating == SelectedRating);

            // Date
            if (FromDate.HasValue)
                query = query.Where(f => f.CreatedAt >= FromDate.Value);

            if (ToDate.HasValue)
                query = query.Where(f => f.CreatedAt <= ToDate.Value.AddDays(1).AddSeconds(-1));

            // Output
            FilteredFeedbacks.Clear();
            foreach (var f in query)
                FilteredFeedbacks.Add(f);
        }

        private void ClearFilter()
        {
            SearchText = "";
            SelectedRating = 0;
            FromDate = null;
            ToDate = null;

            OnPropertyChanged(nameof(SearchText));
            OnPropertyChanged(nameof(SelectedRating));
            OnPropertyChanged(nameof(FromDate));
            OnPropertyChanged(nameof(ToDate));

            ApplyFilter();
        }
    }
}
