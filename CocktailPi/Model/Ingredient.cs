using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CocktailPi
{
    public class Ingredient
    {
        public Ingredient(string name            )
        {
            Name = name;
        }

            public Ingredient(XmlElement node)
        {
            Name = node.GetAttribute("Name");
            if (node.HasAttribute("Qnty"))
            {
                Qnty = float.Parse(node.GetAttribute("Qnty"));
            }
        }

        public string Name { get; set; }

        public float Qnty { get; set; } = -1;

        public string Caption
        {
            get
            {
                return $"{Qnty} oz - {Name}";
            }
        }

        public bool IsIngredientAvailable
        {
            get
            {
                return Cocktail.FindIngredientPump(Name) != null;
            }
        }

        public Brush LabelColor
        {
            get
            {
                return IsIngredientAvailable ? new SolidColorBrush(Windows.UI.Colors.Black) : new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }

        public FontWeight CaptionFontWeight
        {
            get
            {
                return IsIngredientAvailable ? FontWeights.Normal : FontWeights.Bold;
            }
        }
    }
}
