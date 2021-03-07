using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;

namespace Ambilight.Converters
{
    public class CouleurToHex : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !(value is Couleur))
                return null;

            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    //writer.WriteBytes((Couleur)value);
                    //writer.
                    //writer.StoreAsync().GetResults();
                }

                //var couleur = new Couleur();
                Color myColor = Color.FromArgb((value as Couleur).R, (value as Couleur).G, (value as Couleur).B);
                string hex = "#"+myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
                return hex;
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !(value is Couleur))
                return (null);
            return (null);
        }
    }
}
