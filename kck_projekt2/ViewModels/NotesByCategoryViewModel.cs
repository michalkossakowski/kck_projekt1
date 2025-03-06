using kck_api.Controller;
using kck_api.Model;
using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace kck_projekt2.ViewModels
{
    class NotesByCategoryViewModel : INotifyPropertyChanged
    {
        private readonly NoteController _noteController;
        private readonly CategoryController _categoryController;

        private readonly MainWindow _mainWindow;
        private List<NoteModel> userNotes;
        public List<CategoryViewModel> CategoriesView { get; set; }
        public List<int> SelectedCategories { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private WpfPlot _plotControl;
        public WpfPlot PlotControl
        {
            get => _plotControl;
            private set
            {
                _plotControl = value;
                OnPropertyChanged(nameof(PlotControl));
            }
        }
        private bool _emptyChart;
        public bool EmptyChart
        {
            get => SelectedCategories.Count == 0;
            set
            {
                if (_emptyChart != value)
                {
                    _emptyChart = value;
                    OnPropertyChanged(nameof(EmptyChart));
                }
            }
        }
        public NotesByCategoryViewModel(MainWindow mainWindow)
        {
            _noteController = NoteController.GetInstance();
            _categoryController = CategoryController.GetInstance();

            _mainWindow = mainWindow;
            PlotControl = new WpfPlot();

            SelectedCategories = new List<int>();

            LoadNotes();
            LoadCategories();
            UpdateChart();
        }
        private void LoadNotes()
        {
            userNotes = _noteController.GetNotesByUserId(_mainWindow.loggedUserId);
        }
        private async Task LoadCategories()
        {
            var listOfCategories = await _categoryController.GetAllCategoriesAsync();
            CategoriesView = new List<CategoryViewModel>(listOfCategories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }));
            CategoriesView.ForEach(c => c.PropertyChanged += Category_PropertyChanged);
            OnPropertyChanged(nameof(CategoriesView)); // Powiadomienie o zmianie
        }
        private void UpdateChart()
        {
            var ChartData = GetChartData();
            var ChartDataCount = ChartData.Count();
            var plt = PlotControl.Plot;

            plt.Clear();
            var pie = plt.Add.Pie(ChartData.Values.ToArray());

            if (ChartData.Count == 0)
                EmptyChart = true;
            else 
            {
                EmptyChart = false;
                pie.ExplodeFraction = 0.1;
                pie.SliceLabelDistance = 0.5;

                double total = pie.Slices.Select(x => x.Value).Sum();
                for (int i = 0; i < pie.Slices.Count; i++)
                {
                    pie.Slices[i].LabelFontSize = 20;
                    pie.Slices[i].Label = $"{pie.Slices[i].Value}";
                    pie.Slices[i].LegendText = $"{CategoriesView.First(CV => CV.Id == ChartData.ElementAt(i).Key).Name} " +
                        $"({pie.Slices[i].Value / total:p1})";
                }
            }
            plt.Axes.Frameless();
            plt.HideGrid();
            PlotControl.Refresh();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChartData)));
        }
        private Dictionary<int, double> GetChartData()
        {
            if (SelectedCategories == null)
            {
                return new Dictionary<int, double>();
            }

            Dictionary<int, double> categories = new Dictionary<int, double>();

            foreach (NoteModel note in userNotes)
            {
                if (SelectedCategories.Contains(note.CategoryId)) 
                {
                    if (!categories.ContainsKey(note.CategoryId))
                    {
                        categories[note.CategoryId] = 0; 
                    }
                    categories[note.CategoryId]++;
                }
            }

            return categories;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CategoryViewModel.IsSelected))
            {
                var category = sender as CategoryViewModel;
                if (category != null)
                {
                    if (category.IsSelected)
                    {
                        if (!SelectedCategories.Contains(category.Id))
                        {
                            SelectedCategories.Add(category.Id);
                        }
                    }
                    else
                    {
                        SelectedCategories.Remove(category.Id);
                    }
                    UpdateChart();
                }
            }
        }
    }
}
