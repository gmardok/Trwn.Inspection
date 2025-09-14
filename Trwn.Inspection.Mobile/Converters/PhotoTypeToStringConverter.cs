using System.Globalization;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.Converters
{
    public class PhotoTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PhotoType photoType)
            {
                return photoType switch
                {
                    PhotoType.Major => "Major defects",
                    PhotoType.Minor => "Minor defects",
                    PhotoType.ShippingMark => "Shipping mark",
                    PhotoType.Packaging => "Packaging",
                    PhotoType.PackageWithDeffects => "Sealing of Samples of Defects(remain in the factory)",
                    _ => "Unknown Photo Type"
                };
            }

            return "Invalid Type";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not supported.");
        }
    }
}
