using kck_api.Controller;
using ScottPlot;
using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace kck_projekt2.ViewModels
{
    public class NotesInMonthViewModel : INotifyPropertyChanged
    {
        private readonly NoteController _noteController;
        private readonly MainWindow _mainWindow;
        private Dictionary<(int year, int month), List<NoteModel>> _notesByMonthYear;
        private int? _yearFrom;
        private int? _yearTo;
        private WpfPlot _plotControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<int> Years { get; set; }
        public double[] ChartData { get; private set; } = new double[12];

        public WpfPlot PlotControl
        {
            get => _plotControl;
            private set
            {
                _plotControl = value;
                OnPropertyChanged(nameof(PlotControl));
            }
        }
        public int? YearFrom
        {
            get => _yearFrom;
            set
            {
                _yearFrom = value;
                OnPropertyChanged(nameof(YearFrom));
                UpdateChart();
            }
        }
        public int? YearTo
        {
            get => _yearTo;
            set
            {
                _yearTo = value;
                OnPropertyChanged(nameof(YearTo));
                UpdateChart();
            }
        }

        public NotesInMonthViewModel(MainWindow mainWindow)
        {
            _noteController = NoteController.GetInstance();
            _mainWindow = mainWindow;
            PlotControl = new WpfPlot();
            LoadNotes();
            SetYears();
            UpdateChart();

            App.LanguageChanged += UpdateChart;
        }
        private void LoadNotes()
        {
            _notesByMonthYear = new Dictionary<(int, int), List<NoteModel>>();
            List<NoteModel> userNotes = _noteController.GetNotesByUserId(_mainWindow.loggedUserId);

            foreach (var note in userNotes)
            {
                var key = (note.ModifiedDate.Year, note.ModifiedDate.Month);
                if (!_notesByMonthYear.ContainsKey(key))
                    _notesByMonthYear[key] = new List<NoteModel>();

                _notesByMonthYear[key].Add(note);
            }
        }

        private void UpdateChart()
        {
            ChartData = GetChartData();
            var plt = PlotControl.Plot;
            plt.Clear();

            string[] monthNames = GetLocalizedMonthNames();
            plt.Add.Bars(Enumerable.Range(0, 12).Select(x => (double)x).ToArray(), ChartData);

            plt.Title((string)Application.Current.Resources["NotesInYears"]);
            plt.YLabel((string)Application.Current.Resources["NumberOfNotes"]);
            plt.XLabel((string)Application.Current.Resources["Month"]);

            var tickGen = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < 12; i++)
                tickGen.AddMajor(i, monthNames[i]);

            plt.Axes.Bottom.TickGenerator = tickGen;
            plt.Axes.Margins(bottom: 0);
            PlotControl.Refresh();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChartData)));
        }

        private double[] GetChartData()
        {
            double[] values = new double[12];
            for (int i = 0; i < 12; i++)
            {
                values[i] = _notesByMonthYear.Count(x => x.Key.month == i + 1 && x.Key.year >= (YearFrom ?? 1900) && x.Key.year <= (YearTo ?? 9999));
            }
            return values;
        }
        private void SetYears()
        {
            Years = new ObservableCollection<int>(_notesByMonthYear.Select(x => x.Key.year).Distinct().OrderBy(x => x));
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string[] GetLocalizedMonthNames()
        {
            return new string[]
            {
                (string)Application.Current.Resources["JanuaryStr"],
                (string)Application.Current.Resources["FebruaryStr"],
                (string)Application.Current.Resources["MarchStr"],
                (string)Application.Current.Resources["AprilStr"],
                (string)Application.Current.Resources["MayStr"],
                (string)Application.Current.Resources["JuneStr"],
                (string)Application.Current.Resources["JulyStr"],
                (string)Application.Current.Resources["AugustStr"],
                (string)Application.Current.Resources["SeptemberStr"],
                (string)Application.Current.Resources["OctoberStr"],
                (string)Application.Current.Resources["NovemberStr"],
                (string)Application.Current.Resources["DecemberStr"]
            };
        }
    }
}
