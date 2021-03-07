using Ambilight.Ampoules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public sealed partial class Add : Page
    {
        private Connected_Device item;
        public Add()
        {
            this.InitializeComponent();
            item = App.Item_Selected;
            if (App.Edit_item) FillAtributes();

        }
        private void FillAtributes()
        {
            TextBox_IP.Text = item.ip;
            TextBox_Name.Text = item.name;
            if (item.mac != null)TextBox_Mac.Text = item.mac;
            TextBox_Port.Text = item.port.ToString();
            Button_add.Content = "Edit";
        }

        private void NavigationView_SelectionChanged5(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string choice = (args.SelectedItem as NavigationViewItem).Content.ToString();
            if (args.IsSettingsSelected)
            {
                this.Frame.Navigate(typeof(Settings));

            }
            

            else if (choice == "Liste")
            {
                this.Frame.Navigate(typeof(MainPage));

            }
            else if (choice == "Edit")
            {
                this.Frame.Navigate(typeof(Edit));

            }
        }

        private bool CheckForInt(TextBox tb)
        {
            return tb.Text.All(char.IsDigit);
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            if (true)
            {
                WIZ_A60 ampoule = new WIZ_A60();
                if (TextBox_IP.Text != "")
                {
                    ampoule.ip = TextBox_IP.Text; //TextBox_IP.Text;
                }
                else
                {
                    ampoule.ip = "1.1.1.1"; //TextBox_IP.Text;
                }


                if (TextBox_Name.Text != "")
                {
                    ampoule.name = TextBox_Name.Text;
                }
                else
                {
                    string name = "Ampoule No " + App.Items.Count;
                    ampoule.name = name;
                }

                if (TextBox_Mac.Text != "") ampoule.mac = TextBox_Mac.Text;

                if (TextBox_Port.Text != "")
                {
                    if (CheckForInt(TextBox_Port))
                    {
                        ampoule.port = Convert.ToInt32(TextBox_Port.Text);
                    }

                }
                else
                {
                    ampoule.port = 38899;
                }

                if (TextBox_Mac.Text != "")
                {
                    ampoule.mac = TextBox_Mac.Text;
                }



                if (!App.Edit_item)
                {
                    App.Items.Add(ampoule);
                    Textblock_Button.Text = "Ampoule '" + ampoule.name + "' added !";
                }
                else
                {
                    App.Items.Remove(item);
                    App.Items.Add(ampoule);
                }

                this.Frame.Navigate(typeof(MainPage));

            }
           

        }

        

    }
}
