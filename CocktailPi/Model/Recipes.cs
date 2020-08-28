using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;

namespace CocktailPi
{
    public class Recipes : List<Recipe>
    {

        public void Load()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Data\Recipes.xml");
            //ResourceLoader.GetForCurrentView()
            //ResourceMap resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            //string XML = resourceMap.GetValue("Recipes").ValueAsString;
            //var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources.resw");
            //doc.LoadXml(resources.GetString("recipes"));
            foreach (XmlElement node in doc.SelectNodes("//Recipe"))
            {
                Recipe recipe = new Recipe(node);
                Add(recipe);
            }
        }
    }


}
