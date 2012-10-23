namespace Nova.Dnug.UI.Wpf.Models
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;

    using Nova.Dnug.Data.Repository;

    /// <summary>
    /// Model wrapping a <see cref="IRepository"/> with some additional display information
    /// </summary>
    public class ChartableRepository : INotifyPropertyChanged
    {
        /// <summary>
        /// Backing field for <see cref="Selected"/>
        /// </summary>
        private bool selected;

        /// <summary>
        /// Backing field for <see cref="LastDuration"/>
        /// </summary>
        private TimeSpan lastDuration;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the repository to wrap
        /// </summary>
        public IRepository Repository { get; set; }

        /// <summary>
        /// Gets or sets the brush to render the repository's progress with on the graph
        /// </summary>
        public Brush Brush { get; set; }

        /// <summary>
        /// Gets or sets the title for the repository on the GUI
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the key for the repository
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the duration the repository ran the previous command in
        /// </summary>
        public TimeSpan LastDuration
        {
            get
            {
                return this.lastDuration;
            }

            set
            {
                this.lastDuration = value;
                this.OnPropertyChanged("LastDuration");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the repository is selectedi in the view
        /// </summary>
        public bool Selected
        {
            get
            {
                return this.selected;
            }

            set
            {
                this.selected = value;
                this.OnPropertyChanged("Selected");
            }
        }

        /// <summary>
        /// Custom event invocator for <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to raise the notification event for
        /// </param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
