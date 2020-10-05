using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CocktailPi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PumpsPage : Page
    {
        public PumpsPage()
        {
            this.InitializeComponent();
            Items = Cocktail.Pumps;
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Cocktail.Pumps.SaveConfiguration();
        }

        public Pumps Items { get; set; }

        private void listItemClicked(object sender, ItemClickEventArgs e)
        {
            //Frame frame = (Frame)Window.Current.Content;
            //MainPage page = (MainPage)frame.Content;
            //page.ContentFrame.Navigate(typeof(DrinkDetails));

            //DrinkDetails detailPage = (DrinkDetails)page.ContentFrame.Content;
            //detailPage.Item = (Recipe)e.ClickedItem;
        }


        private void cmdRecover_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Pump pump = button?.DataContext as Pump;
            if (pump != null)
            {
                if (!pump.IsRecovering)
                {
                    pump.StopPrime();
                    pump.StartRecover();
                }
                else pump.StopRecover();
                button.Content = pump.CaptionRecoverButton;

            }
        }

        private void cmdPrime_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Pump pump = button?.DataContext as Pump;
            if (pump != null)
            {
                if (!pump.IsPriming)
                {
                    pump.StopRecover();
                    pump.StartPrime();
                }
                else pump.StopPrime();
                button.Content = pump.CaptionPrimeButton;
            }
        }

    }
}
