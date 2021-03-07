using Ambilight.Ampoules;
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
    public sealed partial class Edit : Page
    {
        public event TypedEventHandler<ColorPicker, ColorChangedEventArgs> ColorChanged;

       
        public Edit()
        {
            
            this.InitializeComponent();

            //ColorChanged +=  

            Initialise_Display();
            if (App.Item_Selected is WIZ_A60 ampoule)
            {
                Scene_Selector_View.ItemsSource = WIZ_A60_Scene_List();
            }
           
        }

        private void Initialise_Display()
        {
            // App.ampoule_Selected = 
           // App.ampoule_Selected = new WIZ_A60();
            if (App.Item_Selected is WIZ_A60 ampoule)
            {
                if (ampoule._currentMode ==  Ampoules.mode.RGB)
                {
                    RadioButton_RGB.IsChecked = true;
                    RadioButton_White.IsChecked = false;
                    RadioButton_Scene.IsChecked = false;
                    ColorPicker.Visibility = Visibility.Visible;
                    White_Color_Slider.Visibility = Visibility.Collapsed;
                    White_Intensity_Slider.Visibility = Visibility.Collapsed;
                    Scene_Selector_View.Visibility = Visibility.Collapsed;
                    Scene_Intensity_Slider.Visibility = Visibility.Collapsed;
                    Scene_Speed_Slider.Visibility = Visibility.Collapsed;
                }
                else if (ampoule._currentMode == Ampoules.mode.White)
                {
                    RadioButton_RGB.IsChecked = false;
                    RadioButton_White.IsChecked = true;
                    RadioButton_Scene.IsChecked = false;
                    ColorPicker.Visibility = Visibility.Collapsed;
                    White_Color_Slider.Visibility = Visibility.Visible;
                    White_Intensity_Slider.Visibility = Visibility.Visible;
                    Scene_Selector_View.Visibility = Visibility.Collapsed;
                    Scene_Intensity_Slider.Visibility = Visibility.Collapsed;
                    Scene_Speed_Slider.Visibility = Visibility.Collapsed;
                    White_Color_Slider.Value = ampoule.White_Color;
                    White_Intensity_Slider.Value = ampoule.Intensity;
                }
                else if (ampoule._currentMode == Ampoules.mode.Scene)
                {
                    RadioButton_RGB.IsChecked = false;
                    RadioButton_White.IsChecked = false;
                    RadioButton_Scene.IsChecked = true;
                    ColorPicker.Visibility = Visibility.Collapsed;
                    White_Color_Slider.Visibility = Visibility.Collapsed;
                    White_Intensity_Slider.Visibility = Visibility.Collapsed;
                    Scene_Selector_View.Visibility = Visibility.Visible;
                    Scene_Intensity_Slider.Visibility = Visibility.Visible;
                    Scene_Speed_Slider.Visibility = Visibility.Visible;
                }
            }
            
            


        }




        private void NavigationView_SelectionChanged5(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string choice = (args.SelectedItem as NavigationViewItem).Content.ToString();
            if (args.IsSettingsSelected)
            {
                this.Frame.Navigate(typeof(Settings));

            }
            else if (choice == "Open")
            {
                //Load_File();


            }
            else if (choice == "Save")
            {
                //Save_File();

            }
            else if (choice == "Add")
            {
                this.Frame.Navigate(typeof(Add));
                /*for (int i = 0; i < 10; i++)
                {
                    Ampoule ampoule = new Ampoule();
                    ampoule.name = "Ampoule No " + i;
                    App.Ampoules.Add(ampoule);
                }*/


            }
            else if (choice == "Liste")
            {
                this.Frame.Navigate(typeof(MainPage));

            }

        }

        private void Color_changed_event(object sender,ColorChangedEventArgs e)
        {

            Apply_Color();
        }
        private void Mode_Selector(object sender, RoutedEventArgs e)
        {
            string mode = (e.OriginalSource as RadioButton).Content.ToString();

            if (mode == "RGB")
            {
                if (App.Item_Selected is WIZ_A60 ampoule)
                {
                    ampoule._currentMode = Ampoules.mode.RGB;
                }
                

            }
            else if (mode == "White")
            {
                if (App.Item_Selected is WIZ_A60 ampoule)
                {
                    ampoule._currentMode = Ampoules.mode.White;
                }

            }
            else if (mode == "Scene")
            {
                if (App.Item_Selected is WIZ_A60 ampoule)
                {
                    ampoule._currentMode = Ampoules.mode.Scene;
                }
            }
            Initialise_Display();

        }

            private void Button_Apply_Color(object sender, RoutedEventArgs e)
        {
            Apply_Color();
        }

        private void Apply_Color()
        {
            if (App.Item_Selected is WIZ_A60 ampoule)
            {
                if (ampoule.name != "")
                {
                    if (ampoule._currentMode == Ampoules.mode.RGB)
                    {
                        ampoule.Set_Red(ColorPicker.Color.R);
                        ampoule.Set_Green(ColorPicker.Color.G);
                        ampoule.Set_Blue(ColorPicker.Color.B);

                        int index = -1;
                        for (int i = 0; i < App.Items.Count; i++)
                        {
                            if ((App.Items[i] as WIZ_A60).name == ampoule.name) index = i;
                        }

                        App.Items[index] = ampoule;
                        ampoule.Update_Status();

                    }
                    else if (ampoule._currentMode == Ampoules.mode.White)
                    {
                        //White_Color_Slider.Visibility = Visibility.Collapsed;
                        //White_Intensity_Slider.Visibility = Visibility.Collapsed;
                        if (White_Intensity_Slider != null) ampoule.Set_Intensity((int)White_Intensity_Slider.Value);

                        ampoule.Set_White_Color((int)White_Color_Slider.Value);
                        int index = -1;
                        for (int i = 0; i < App.Items.Count; i++)
                        {
                            if ((App.Items[i] as WIZ_A60).name == ampoule.name) index = i;
                        }

                        App.Items[index] = ampoule;
                        ampoule.Update_Status();


                    }
                    else if (ampoule._currentMode == Ampoules.mode.Scene)
                    {
                        ampoule.Set_Intensity((int)Scene_Intensity_Slider.Value);
                        ampoule.Set_Scene_Speed((int)Scene_Speed_Slider.Value);
                        int index = -1;
                        for (int i = 0; i < App.Items.Count; i++)
                        {
                            if ((App.Items[i] as WIZ_A60).name == ampoule.name) index = i;
                        }

                        App.Items[index] = ampoule;
                        ampoule.Update_Status();

                    }
                    
                    
                }
            }
            
            
           // var index = App.Ampoules.Where(Ampoule => Ampoule.name == App.ampoule_Selected.name).
            //var utilisateur = App.UserViewModel.Users.Where(User => User.Name == LoginBox.Text.ToString()).FirstOrDefault();

        }

        private void White_Intensity_Color_Changed(object sender, RangeBaseValueChangedEventArgs e)
        {
            Apply_Color();

        }

        private List<string> WIZ_A60_Scene_List()
        {
            List<string> result = new List<string>();
            int numberOfScenes = Enum.GetNames(typeof(scene)).Length;
            for (int i = 0; i < numberOfScenes; i++)
            {
                result.Add(Enum.GetNames(typeof(scene))[i]);
                

            }
            return result;
        }

        

        private void Set_Scene(string choice)
        {
            if (App.Item_Selected is WIZ_A60 ampoule)
            {
                scene Selected_scene = (scene)Enum.Parse(typeof(scene), choice, true);
                ampoule.Set_Scene(Selected_scene);
                ampoule.Update_Status();
                
            }
            

        }

       

        private void Scene_Selector_View_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is TextBlock tb)
            {
                Set_Scene(tb.Text);
                Apply_Color();
            }
            else if (e.OriginalSource is ListViewItemPresenter L)
            {
                Set_Scene(L.Content.ToString());
                Apply_Color();
            }
            

            


        }

        private void Scene_Intensity_Color_Changed(object sender, RangeBaseValueChangedEventArgs e)
        {
            Apply_Color();

        }

        private void Bouton_edit_Click(object sender, RoutedEventArgs e)
        {
            App.Edit_item = true;
            this.Frame.Navigate(typeof(Add));
        }
    }
}
