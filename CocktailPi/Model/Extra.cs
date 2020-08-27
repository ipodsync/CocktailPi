using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CocktailPi
{
    public class Extra
    {
        public Extra(XmlElement node)
        {
            Name = node.GetAttribute("Name");
        }

        public string Name { get; set; } = "";
    }
}
