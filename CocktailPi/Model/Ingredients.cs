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
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Data\Ingredients.xml");
            //ResourceLoader.GetForCurrentView()
            //ResourceMap resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            //string XML = resourceMap.GetValue("Recipes").ValueAsString;
            //var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources.resw");
            //doc.LoadXml(resources.GetString("recipes"));
            foreach (XmlElement node in doc.SelectNodes("//Ingredient"))
            {
                Ingredient ingredient = new Ingredient(node);
                Add(ingredient);
            }
        }
    }
}

