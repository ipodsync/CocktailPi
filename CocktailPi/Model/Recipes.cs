using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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
            foreach (XmlElement node in doc.SelectNodes("//Recipe"))
            {
                Recipe recipe = new Recipe(node);
                Add(recipe);
            }
            Sort();
        }
    }
}
