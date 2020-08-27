using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CocktailPi
{
    public class Ingredient
    {
        public Ingredient(XmlElement node)
        {
            Name = node.GetAttribute("Name");
            Qnty = float.Parse(node.GetAttribute("Qnty"));
        }

        public string Name { get; set; }

        public float Qnty { get; set; } = 0;

        public string Caption
        {
            get
            {
                return $"{Qnty} oz - {Name}";
            }
        }
    }
}
