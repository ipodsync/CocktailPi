﻿using System;
using System.Collections.Generic;
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
    public class Recipe
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

        public string Name { get; set; } = "";

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
                return $"Bartender!  Mix me a {Name}!";
            }
        }

        public BitmapImage Image { get; set; }

        public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();

        public List<Dash> Dashs { get; private set; } = new List<Dash>();

        public List<Extra> Extras { get; private set; } = new List<Extra>();
    }
}