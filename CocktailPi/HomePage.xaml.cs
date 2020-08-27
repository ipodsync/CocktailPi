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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CocktailPi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {

        public Recipes _recipes;
        DrinkDetails _drinkDetails = new DrinkDetails();
        public HomePage()
        {
            this.InitializeComponent();
            _recipes = new Recipes();
            _recipes.Load();

            DrinkList.ItemsSource = _recipes;
        }

        private void listItemClicked(object sender, ItemClickEventArgs e)
        {
            Frame frame = (Frame)Window.Current.Content;
            MainPage page = (MainPage)frame.Content;
            page.ContentFrame.Navigate(typeof(DrinkDetails));

            DrinkDetails detailPage = (DrinkDetails)page.ContentFrame.Content;
            detailPage.Item = (Recipe)e.ClickedItem; 
        }
    }
}
