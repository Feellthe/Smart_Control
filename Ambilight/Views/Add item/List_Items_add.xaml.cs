using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ambilight
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class List_Items_add : Page
    {
        private Byte[] result;
        private List<string> models = new List<string> { "WIZ_A60-WIZ_A67"};
        private List<Connected_Device> Availble_Items = new List<Connected_Device>();


        public List_Items_add()
        {
            this.InitializeComponent();
            InitialiseList();

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
            
        }

        
        private async void InitialiseList()
        {
            for (int i = 0; i != models.Count; i++)
            {
                Connected_Device Device = new Connected_Device();
                string path = "\\Assets\\ItemsList\\" + models[i] + ".jpg";
                Device.Image = await ImageToByte(path);
                Device.name = models[i];
                Availble_Items.Add(Device);
            }
            Text_for_return.Text = Availble_Items.Count.ToString();
            Refresh_Display();
        }
        

        private async Task<Byte[]> ImageToByte(string path)
        {
            string origin = Directory.GetCurrentDirectory();
            string Absolute_path = origin + path;
            StorageFile file = await StorageFile.GetFileFromPathAsync(Absolute_path);
            
            if (file != null)
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    byte[] content = new byte[stream.Length];
                    await stream.ReadAsync(content, 0, content.Length);
                    return content;
                }
            }
            else
            {
                return null;   
            }
        }

        private void Button_Refresh(object sender, RoutedEventArgs e)
        {
            Refresh_Display();
        }

        private void Refresh_Display()
        {
            Items_list.ItemsSource = null;
            Items_list.ItemsSource = Availble_Items;
        }

        private async void Load_Image(object sender, RoutedEventArgs e)
        {
            Connected_Device WIZ_A60 = new Connected_Device();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpg");

            StorageFile file = await openPicker.PickSingleFileAsync();
            StorageFile testFile = await StorageFile.GetFileFromPathAsync("ms-appx:///Assets/ItemsList/WIZ_A60.jpeg");
            
            
            //testFile.Path = @"C:\\Users\\Pierre\\Pictures\\add_blue.JPG";
            if (file != null)
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    byte[] content = new byte[stream.Length];
                    await stream.ReadAsync(content, 0, content.Length);
                    WIZ_A60.Image = content;
                    //pvm.Product = SelectedProduct;
                    //await pvm.UpdateAsync();
                }
            }
            //pvm.Products[0].Thumbnail = content;
            //pvm.LoadFile(file);
            

        }

        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Connected_Device c)
            App.Item_Selected = (e.ClickedItem as Connected_Device);
            this.Frame.Navigate(typeof(Add));



        }
    }
}
