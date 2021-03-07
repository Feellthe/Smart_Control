using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ambilight.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Loading_screen : Page
    {
        private double GetAspectHeight(double width)
        {
            return width * (1080d / 1920);
        }
         
        public Loading_screen()
        {
            this.InitializeComponent();
            //LoadDefaultProfile();
        }
        public void LoadDefaultProfile()
        {
            page.MaxWidth = 892;
            page.MaxHeight = 530;

        }
        

        

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Image_Background.Width = page.ActualWidth;
            Image_Background.Height = page.ActualHeight;
        }


    }
}
