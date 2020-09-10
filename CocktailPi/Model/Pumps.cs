using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CocktailPi
{
    public class Pumps : List<Pump>
    {

        public void SaveConfiguration ()
        {
            foreach (Pump p in this)
            {
                p.SaveConfiguration();
            }
        }

        public void LoadConfiguration ()
        {
            foreach (Pump p in this)
            {
                p.LoadConfiguration();
            }
        }
    }
}
