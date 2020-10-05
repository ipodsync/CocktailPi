using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace CocktailPi
{
    public class Recipe : IComparable<Recipe>
    {

        public Recipe(XmlElement node)
        {
            Name = node.GetAttribute("Name");
            Description = node.GetAttribute("Description");

            string imageFileName = node.GetAttribute("Image");
            if (!string.IsNullOrEmpty(imageFileName))
            {
                imageFileName = $"ms-appx:///Assets/{imageFileName}";
            }
            else
            {
                imageFileName = $"ms-appx:///Assets/cocktail-glass.png";
            }

            BitmapImage image = new BitmapImage();
            image.UriSource = new Uri(imageFileName);
            Image = image;

            foreach (XmlElement n in node.SelectNodes("Ingredient"))
            {
                Ingredients.Add(new Ingredient(n));
            }

            foreach (XmlElement n in node.SelectNodes("Dash"))
            {
                Dashs.Add(new Dash(n));
            }

            foreach (XmlElement n in node.SelectNodes("Extra"))
            {
                Extras.Add(new Extra(n));
            }
        }

        #region Properties

        public string Name { get; set; } = "";

        public string Caption
        {
            get
            {
                string caption = Name;
                if (!CanMakeRecipe)
                {
                    caption += " (Missing Ingredient)";
                }
                return caption;
            }
        }

        public string Description { get; set; } = "";

        public bool ShowAddons
        {
            get
            {
                return Dashs.Count > 0 || Extras.Count > 0;
            }
        }

        public string ExecuteCaption
        {
            get
            {
                if (CanMakeRecipe)
                    return $"Bartender!  Mix me a {Name}!";

                return "Unable to make cocktail.   Missing key ingredients.  :(";
            }
        }

        public BitmapImage Image { get; set; }

        public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();

        public List<Dash> Dashs { get; private set; } = new List<Dash>();

        public List<Extra> Extras { get; private set; } = new List<Extra>();

        public bool CanMakeRecipe
        {
            get
            {
                foreach (Ingredient i in Ingredients)
                {
                    if (Cocktail.FindIngredientPump(i.Name) == null)
                        return false;
                }
                return true;
            }
        }

        int _executionProgress = 0;
        public int ExecutionProgress
        {
            get => _executionProgress;
            set
            {
                _executionProgress = value;
                Debug.Print($"Percent={ExecutionProgress}\r\n");
            }
        }

        public int CompareTo(Recipe other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;

            else
                return this.Name.CompareTo(other.Name);
        }

        #endregion
    }
}
