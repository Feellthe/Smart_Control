using Ambilight.Ampoules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
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
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Newtonsoft.Json;
// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Ambilight
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            RefreshList();
            Init();
        }
        [DllImport("iphlpapi.dll", ExactSpelling = true, EntryPoint = "SendARP")]

        public static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

        private async void Init()
        {

            // we need to ge the file at :
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                Windows.Storage.StorageFile file = await storageFolder.GetFileAsync("defaut.xml");
                if (file != null)
                {
                    Open_File(file);
                    Return_Text.Text = "J'ai ouvert le fichier de base";
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                Return_Text.Text = "le fichier n'existe pas";
            }
            
            
        }

        private void RefreshList()
        {
            Device_List.ItemsSource = null;
            Device_List.ItemsSource = App.Items;
        }

        private async void Choose_File()
        {
            
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("XML", new List<string>() { ".xml" });
            savePicker.SuggestedFileName = "New Profil";
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {

                    //Return_Text.Text = "File " + file.Name + " was saved.";
                    Save_File(file.Name, file); // save where the user want to be saved
                }
                else
                {
                    Return_Text.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                Return_Text.Text = "Operation cancelled.";
            }
        }

        private async void Open_File(StorageFile file)
        {
            if (file != null)
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    byte[] content = new byte[stream.Length];
                    await stream.ReadAsync(content, 0, content.Length);
                    string result = System.Text.Encoding.UTF8.GetString(content);
                    string calculation = result;

                    IList<String> lines = await Windows.Storage.FileIO.ReadLinesAsync(file);
                    int nombre = lines.Count;
                    if (lines.Count == 1)
                    {
                        List<string> JsonObjects = GetJsonObjects(lines);
                        JsonToGlobal(JsonObjects);

                    }
                    else if (lines.Count > 1)
                    {
                        List<string> JsonObjects = GetJsonObjects(lines);
                        JsonToGlobal(JsonObjects);


                    }


                    //WIZ_A60 ampoule =  JsonConvert.DeserializeObject<WIZ_A60>(result);
                    //WIZ_A60 o =  JsonConvert.DeserializeObject(result);
                }
            }

        }

        private async void Load_File()
        {

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".xml");


            StorageFile file = await openPicker.PickSingleFileAsync();
            Open_File(file);


        }

        private List<string> GetJsonObjects(IList<String> lignes)
        {
            List<string> result = new List<string>();
            int end = 0;
            for (int i = 0; i < lignes.Count; i++)
            {
                end = lignes[i].IndexOf("}") + 1;
                string command = lignes[i].Substring(0, end);
                result.Add(command);
            }

            return result;
        }

        private void JsonToGlobal(List<string> JsonObjects)
        {
            //We need to have in App.Items all the items in JsonObjects but in accordance to the type of each JsonObjects...
            App.Items.Clear();
            for (int i = 0; i < JsonObjects.Count; i++)
            {
                int indexOfType = JsonObjects[i].IndexOf("type") + 7;
                int end_indexOfType = JsonObjects[i].IndexOf("port") - 3;
                int lenght = end_indexOfType - indexOfType;
                string type = JsonObjects[i].Substring(indexOfType, lenght);
                if (type == "WIZ_A60")
                {
                    WIZ_A60 item = JsonConvert.DeserializeObject<WIZ_A60>(JsonObjects[i]);
                    App.Items.Add(item);
                    RefreshList();

                    //if (item.mac != "") CheckCorrect_IP_address(App.Items);
                }


                // JsonConvert.DeserializeObject<WIZ_A60>(result);

            }

        }

        private void CheckCorrect_IP_address(List<Connected_Device> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ip != GetIP(list[i].mac))
                {
                    list[i].ip = GetIP(list[i].mac);
                }
            }
        }

        private bool Ping(string ip_adress)
        {
            Ping p = new Ping();
            PingReply PR = p.Send(ip_adress);
            if (PR.Status.ToString() == "Success")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private async void Save_File(string name, StorageFile fichier)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile file = await storageFolder.CreateFileAsync("defaut.xml", Windows.Storage.CreationCollisionOption.OpenIfExists);


            var stream = await fichier.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            var default_stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            string ToSave = "";
            for (int i = 0; i < App.Items.Count; i++)
            {
                if (App.Items[i] is WIZ_A60 a)
                {

                    a.type = "WIZ_A60";
                    string output;
                    if (i == 0)
                    {
                        output = JsonConvert.SerializeObject(a);
                    }
                    else
                    {
                        output = "," + "\n" + JsonConvert.SerializeObject(a);
                    }
                    ToSave = ToSave + output;
                }





            }

            
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                {
                    dataWriter.WriteString(ToSave);
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose();


            using (var outputStream2 = default_stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream2))
                {
                    dataWriter.WriteString(ToSave);
                    await dataWriter.StoreAsync();
                    await outputStream2.FlushAsync();
                }
            }
            default_stream.Dispose();




        }

        private static byte[] StringToBytesMAC(string mac)
        {
            string[] b = new string[6];
            byte[] res = new byte[6];
            mac = string.Join("", mac.Split('-'));
            mac = string.Join("", mac.Split(':'));
            mac = string.Join("", mac.Split('.'));
            if (mac.Length != 12)
                throw new FormatException("MAC address should be delimited by ':' or '-'.\n" +
                                          "Alternatively it should have exactly 12 hexadecimal characters.");
            else
                for (int i = 0; i < 12; i += 2)
                    b[i / 2] = mac.Substring(i, 2);
            for (int i = 0; i < 6; i++)
                res[i] = Convert.ToByte(b[i], 16);
            return res;
        }

        private static IEnumerable<int> GetNeighbourIP(IPAddress hostIP, IPAddress netmask)
        {
            byte[] ipBytes = hostIP.GetAddressBytes();
            byte[] maskBytes = netmask.GetAddressBytes();
            uint iIP = (uint)IPAddress.HostToNetworkOrder((int)hostIP.Address);
            uint iMask = (uint)IPAddress.HostToNetworkOrder((int)netmask.Address);

            if (ipBytes.Length != maskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] netBytes = new byte[ipBytes.Length];
            for (int i = 0; i < netBytes.Length; i++)
                netBytes[i] = (byte)(ipBytes[i] & (maskBytes[i]));
            Array.Reverse(netBytes);
            uint iNet = BitConverter.ToUInt32(netBytes, 0);

            for (uint tempIP = iNet + 1; tempIP < iNet + ~iMask; tempIP++)
                if (tempIP != iIP)
                    yield return IPAddress.NetworkToHostOrder((int)tempIP);
        }

        public static string GetIP(string mac)
        {
            byte[] pMac = StringToBytesMAC(mac);
            byte[] tempMac = new byte[6];

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                foreach (UnicastIPAddressInformation ipInfo in adapter.GetIPProperties().UnicastAddresses)
                    if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        foreach (int ip in GetNeighbourIP(ipInfo.Address, ipInfo.IPv4Mask))
                        {
                            int len = 6;
                            if (SendARP(ip, 0, tempMac, ref len) == 0
                                    && compareMac(pMac, tempMac) == true)
                                return new IPAddress((long)((ulong)ip & 0x00000000fffffffful)).ToString();
                        }

            return "";
        }

        private static bool compareMac(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
                return false;
            for (int i = 0; i < first.Length; i++)
                if (first[i] != second[i])
                    return false;
            return true;
        }

        private void Scan_network(string subnet)
        {

            //progressBar.Maximum = 254;
            //progressBar.Value = 0;
            //lvResult.Items.Clear();

            Task.Factory.StartNew(new Action(() =>
            {
                for (int i = 0; i < 255; i++)
                {
                    //string ip = $"{subnet}.{i}";
                    string ip = subnet + i.ToString();
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(ip, 100);
                    if (reply.Status == IPStatus.Success)
                    {

                    }
                    else
                    {

                    }
                }
            }));
        }

        private void Ampoule_item_click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is WIZ_A60 a)
            {
                App.Item_Selected = a;

            }

            //Thread.Sleep(500);

            this.Frame.Navigate(typeof(Edit));


        }

        private void Button_delete_Item(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button b && b.DataContext is WIZ_A60 ampoule)
            {
                App.Items.Remove(ampoule);
                RefreshList();
            }


        }

        private void Navigation_1_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem.ToString() == "Save")
            {
                Choose_File();
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
                Load_File();
            }
            else if (args.InvokedItem is NavigationViewItem nvi)
            {
                this.Frame.Navigate(typeof(Settings));
                //need new data model to later save 
            }





        }
    }
}
