using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CocktailPi
{
    public class Ingredients : List<Ingredient>
    {

        public void Load()
        {
            List<string> ingredients = new List<string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(@"Data\Recipes.xml");

            foreach (XmlElement node in doc.SelectNodes("//Ingredient"))
            {
                string ingredient = node.GetAttribute("Name");
                if (!ingredients.Contains(ingredient))
                    ingredients.Add(ingredient);
            }

            ingredients.Sort();
            foreach (string ingredient in ingredients)
            {
                Ingredient ing = new Ingredient(ingredient);
                Add(ing);

            }
        }
    }
}

