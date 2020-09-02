using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailPi
{
    public class Pump
    {
        public int Number { get; set; } = 0;

        public string Title { get => $"Pump {Number}"; }

        public string Ingredient { get; set; } = "";
    }
}
