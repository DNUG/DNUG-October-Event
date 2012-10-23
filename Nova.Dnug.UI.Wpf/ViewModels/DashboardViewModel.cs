namespace Nova.Dnug.UI.Wpf.ViewModels
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Media;
    using GalaSoft.MvvmLight.Command;
    using Nova.Dnug.Data.Repository;
    using Nova.Dnug.Data.Repository.Ado;
    using Nova.Dnug.Data.Repository.EntityFramework;
    using Nova.Dnug.Data.Repository.MongoDb;
    using Nova.Dnug.Data.Repository.Redis;
    using Nova.Dnug.Domain.Model.Builders;
    using Nova.Dnug.UI.Wpf.Commands;
    using Nova.Dnug.UI.Wpf.Models;
    using Nova.Dnug.UI.Wpf.Utilities;
    using Nova.Dnug.UI.Wpf.Views;

    /// <summary>
    /// View model context for the <see cref="DashboardView"/> view
    /// </summary>
    public class DashboardViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardViewModel"/> class.
        /// </summary>
        public DashboardViewModel()
        {
            var universities = new UniversityBuilder().Build(2).ToList();

            this.DataPoints = new ObservableCollection<DataPoint>();

            var mongoDbRepository = new MongoDbRepository(ConfigurationManager.AppSettings["MongoDbConnectionString"]);
            var entityFrameworkRepository = new EntityFrameworkRepository(ConfigurationManager.AppSettings["EntityFrameworkConnectionString"]);
            var adoRepository = new AdoRepository(ConfigurationManager.AppSettings["AdoConnectionString"]);
            var redisRepository = new RedisRepository(ConfigurationManager.AppSettings["RedisConnectionString"]);
            
            var repositories = new List<IRepository> { mongoDbRepository, entityFrameworkRepository, adoRepository, redisRepository };
            var progress = new ConcurrentDictionary<IRepository, int>(repositories.Select(r => new KeyValuePair<IRepository, int>(r, 0)));

            this.AdoNetRepository = new ChartableRepository
                {
                    Title = "ADO.NET",
                    Key = "Ado",
                    Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EC098C")),
                    Repository = adoRepository
                };

            this.Ef5Repository = new ChartableRepository
                {
                    Title = "EF5",
                    Key = "EntityFramework",
                    Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B159")),
                    Repository = entityFrameworkRepository
                };

            this.MongoDbRepository = new ChartableRepository
                {
                    Title = "MongoDB",
                    Key = "MongoDB",
                    Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C4199")),
                    Repository = mongoDbRepository
                };

            this.RedisRepository = new ChartableRepository
                {
                    Title = "Redis",
                    Key = "Redis",
                    Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F37735")),
                    Repository = redisRepository
                };

            this.ChartableRepositories = new List<ChartableRepository>{ this.AdoNetRepository, this.Ef5Repository, this.MongoDbRepository, this.RedisRepository };

            var timer = new TickOnceDispatcherTimer(new TimeSpan(0, 0, 0, 0, 100));
            timer.Tick += delegate
                {
                    var datapoint = new DataPoint
                            {
                                Ticks = (this.DataPoints.Count + 1) / 10.0,
                                MongoDB = progress[mongoDbRepository],
                                EntityFramework = progress[entityFrameworkRepository],
                                Ado = progress[adoRepository],
                                Redis = progress[redisRepository]
                            };
                    this.DataPoints.Add(datapoint);

                    if (this.AdoNetRepository.LastDuration == default(TimeSpan) && datapoint.Ado == universities.Count)
                    {
                        this.AdoNetRepository.LastDuration = Stopwatch.Elapsed;
                    }

                    if (this.Ef5Repository.LastDuration == default(TimeSpan) && datapoint.EntityFramework == universities.Count)
                    {
                        this.Ef5Repository.LastDuration = Stopwatch.Elapsed;
                    }

                    if (this.MongoDbRepository.LastDuration == default(TimeSpan) && datapoint.MongoDB == universities.Count)
                    {
                        this.MongoDbRepository.LastDuration = Stopwatch.Elapsed;
                    }

                    if (this.RedisRepository.LastDuration == default(TimeSpan) && datapoint.Redis == universities.Count)
                    {
                        this.RedisRepository.LastDuration = Stopwatch.Elapsed;
                    }

                    this.OnPropertyChanged("Stopwatch");
                };

            Action<IRepository> notifyProgress = delegate(IRepository repository)
                {
                    if (!timer.IsEnabled)
                    {
                        timer.Start();
                    }

                    progress[repository]++;
                };

            EventHandler commandCompleteEventHandler = delegate
                {
                    Task.Factory.StartNew(timer.StopWhenTicked);
                    this.Stopwatch.Stop();
                };

            this.ResetCommand = new RelayCommand(delegate
                {
                    this.DataPoints.Clear();
                    this.DataPoints.Add(new DataPoint { Ticks = 0, MongoDB = 0, EntityFramework = 0, Ado = 0 });

                    foreach (IRepository repository in progress.Select(x => x.Key))
                    {
                        progress[repository] = 0;
                    }

                    this.Ef5Repository.LastDuration = default(TimeSpan);
                    this.AdoNetRepository.LastDuration = default(TimeSpan);
                    this.MongoDbRepository.LastDuration = default(TimeSpan);
                    this.RedisRepository.LastDuration = default(TimeSpan);

                    this.Stopwatch.Restart();
                });

            this.Stopwatch = new Stopwatch();
            this.ToggleCommand = new ToggleCommand();
            this.BuildCommand = new BuildCommand(repositories);

            var createCommand = new CreateCommand(this.ChartableRepositories.Where(x => x.Selected).Select(x => x.Repository), universities, notifyProgress);
            createCommand.CommandComplete += commandCompleteEventHandler;
            this.CreateCommand = new CompositeCommand(this.ResetCommand, createCommand);

            var retrieveCommand = new RetrieveCommand(this.ChartableRepositories.Where(x => x.Selected).Select(x => x.Repository), universities, notifyProgress);
            retrieveCommand.CommandComplete += commandCompleteEventHandler;
            this.RetrieveCommand = new CompositeCommand(this.ResetCommand, retrieveCommand);

            var updateCommand = new UpdateCommand(this.ChartableRepositories.Where(x => x.Selected).Select(x => x.Repository), universities, notifyProgress);
            updateCommand.CommandComplete += commandCompleteEventHandler;
            this.UpdateCommand = new CompositeCommand(this.ResetCommand, updateCommand);

            var queryCommand = new QueryCommand(this.ChartableRepositories.Where(x => x.Selected && x != this.RedisRepository).Select(x => x.Repository), new StudentBuilder().GetForenames(1000), notifyProgress);
            queryCommand.CommandComplete += commandCompleteEventHandler;
            this.QueryCommand = new CompositeCommand(this.ResetCommand, queryCommand);

            var deleteCommand = new DeleteCommand(this.ChartableRepositories.Where(x => x.Selected).Select(x => x.Repository), universities, notifyProgress);
            deleteCommand.CommandComplete += commandCompleteEventHandler;
            this.DeleteCommand = new CompositeCommand(this.ResetCommand, deleteCommand);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the stopwatch used for timing commands
        /// </summary>
        public Stopwatch Stopwatch { get; private set; }

        /// <summary>
        /// Gets the observable collection of <see cref="DataPoint"/> instances which will be rendered by the graph
        /// </summary>
        public ObservableCollection<DataPoint> DataPoints { get; private set; }

        /// <summary>
        /// Gets a collection of <see cref="ChartableRepository"/> classes
        /// </summary>
        public IEnumerable<ChartableRepository> ChartableRepositories { get; private set; }

        /// <summary>
        /// Gets the command which will reset the graph.
        /// </summary>
        public ICommand ResetCommand { get; private set; }

        /// <summary>
        /// Gets the command which will request each repository clear itself of data
        /// </summary>
        public ICommand BuildCommand { get; private set; }

        /// <summary>
        /// Gets the command which will send data to each repository to create persisted instances of the domain model
        /// </summary>
        public ICommand CreateCommand { get; private set; }

        /// <summary>
        /// Gets the command which will retrieve data from each repository
        /// </summary>
        public ICommand RetrieveCommand { get; private set; }

        /// <summary>
        /// Gets the command which will update data in each repository
        /// </summary>
        public ICommand UpdateCommand { get; private set; }

        /// <summary>
        /// Gets the command which will delete data from each repository
        /// </summary>
        public ICommand QueryCommand { get; private set; }

        /// <summary>
        /// Gets the command which will delete data from each repository
        /// </summary>
        public ICommand DeleteCommand { get; private set; }
        
        /// <summary>
        /// Gets the command which will toggle a repository's selected state
        /// </summary>
        public ICommand ToggleCommand { get; private set; }

        /// <summary>
        /// Gets the ado net repository wrapper
        /// </summary>
        public ChartableRepository AdoNetRepository { get; private set; }

        /// <summary>
        /// Gets the EF5 repository wrapper
        /// </summary>
        public ChartableRepository Ef5Repository { get; private set; }

        /// <summary>
        /// Gets the MongoDB repository wrapper
        /// </summary>
        public ChartableRepository MongoDbRepository { get; private set; }

        /// <summary>
        /// Gets the Redis repository wrapper
        /// </summary>
        public ChartableRepository RedisRepository { get; private set; }

        /// <summary>
        /// Custom event invocator for <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">
        /// The property name to notify change of
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
