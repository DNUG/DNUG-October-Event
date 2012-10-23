namespace Nova.Dnug.UI.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Implementation of <see cref="IValueConverter"/> for converting bools to images
    /// </summary>
    public class BoolToImageConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the image to use when the value is true
        /// </summary>
        public ImageSource TrueImage { get; set; }

        /// <summary>
        /// Gets or sets the image to use when the value is false
        /// </summary>
        public ImageSource FalseImage { get; set; }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">
        /// The value produced by the binding source.
        /// </param>
        /// <param name="targetType">
        /// The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value ? this.TrueImage : this.FalseImage;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">
        /// The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        /// The type to convert to.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
