using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ambilight
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
        }
        private void Navigation_1_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem.ToString() == "Save")
            {
                //Choose_File();
            }
            else if (args.InvokedItem.ToString() == "Add")
            {
                this.Frame.Navigate(typeof(List_Items_add));
            }
            else if (args.InvokedItem.ToString() == "Edit")
            {
                this.Frame.Navigate(typeof(Edit));
            }
            else if (args.InvokedItem.ToString() == "Open")
            {
               // Load_File();
            }
            else if (args.InvokedItem is NavigationViewItem nvi)
            {
                this.Frame.Navigate(typeof(Settings));
                //need new data model to later save 
            }





        }
    }
}
