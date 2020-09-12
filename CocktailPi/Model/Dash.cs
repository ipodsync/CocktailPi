using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CocktailPi
{
    public class Dash
    {
        public Dash(XmlElement node)
        {
            Name = node.GetAttribute("Name");
            Qnty = int.Parse(node.GetAttribute("Qnty"));
        }

        #region Properties 

        public string Name { get; set; } = "";

        public int Qnty { get; set; } = 1;

        public string Caption
        {
            get
            {
                if (Qnty > 1)
                    return $"{Qnty} dashes of {Name}";

                return $"{Qnty} dash of {Name}";
            }
        }

        #endregion
    }
}
