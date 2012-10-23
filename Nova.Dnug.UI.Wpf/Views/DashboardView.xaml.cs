namespace Nova.Dnug.UI.Wpf.Views
{
    using System.Windows.Input;
    using AmCharts.Windows.QuickCharts;
    using Nova.Dnug.UI.Wpf.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DashboardView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardView"/> class.
        /// </summary>
        public DashboardView()
        {
            InitializeComponent();

            var viewModel = this.DataContext as DashboardViewModel;

            if (viewModel != null)
            {
                foreach (var repository in viewModel.ChartableRepositories)
                {
                    this.Chart.Graphs.Add(new LineGraph
                            { 
                                Brush = repository.Brush,
                                Width = 5,
                                Title = repository.Title,
                                ValueMemberPath = repository.Key
                            });
                }
            }
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Exit(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
