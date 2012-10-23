namespace Nova.Dnug.UI.Wpf.Utilities
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// Composition of a <see cref="DispatcherTimer"/> ensuring the timer overruns by a number of tick when it is stopped
    /// </summary>
    public class TickOnceDispatcherTimer
    {
        /// <summary>
        /// The composed <see cref="DispatcherTimer"/>
        /// </summary>
        private readonly DispatcherTimer timer;
        
        /// <summary>
        /// Count down to number of ticks before stopping
        /// </summary>
        private int stopIn;

        /// <summary>
        /// Initializes a new instance of the <see cref="TickOnceDispatcherTimer"/> class.
        /// </summary>
        /// <param name="interval">
        /// The interval to tick at
        /// </param>
        public TickOnceDispatcherTimer(TimeSpan interval)
        {
            this.timer = new DispatcherTimer { Interval = interval };

            this.timer.Tick += delegate(object sender, EventArgs eventArgs)
                {
                    if (this.stopIn != 0)
                    {
                        this.stopIn--;

                        if (this.stopIn == 0)
                        {
                            this.timer.Stop();
                        }
                    }

                    this.Tick(sender, eventArgs);
                };
        }

        /// <summary>
        /// Gets or sets the event handler fired when the timer ticks
        /// </summary>
        public EventHandler Tick { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timer is enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.timer.IsEnabled;
            }

            set
            {
                this.timer.IsEnabled = value;
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }

        /// <summary>
        /// Stops the timer in a given number of ticks time
        /// </summary>
        public void StopWhenTicked()
        {
            this.stopIn = 5;
        }
    }
}