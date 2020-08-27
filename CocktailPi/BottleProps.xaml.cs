using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CocktailPi
{
    public sealed partial class BottleProps : UserControl
    {
        public BottleProps()
        {
            this.InitializeComponent();
            //string XMLFilePath = Path.Combine(Package.Current.InstalledLocation.Path, "Ingredients.xml");
            //XDocument loadedData = XDocument.Load(XMLFilePath);
            //var data = from query in loadedData.Descendants("Ingredient")
            //           select new Ingredient
            //           {
            //               Name = (string)query.Attribute("Name")
            //           };
            //cboIngredient.ItemsSource = data;
        }
    }
}
