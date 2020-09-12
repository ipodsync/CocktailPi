using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CocktailPi
{
    public class Pumps : List<Pump>
    {
        #region Persistance 

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

        #endregion

        public int MaxSteps
        {
            get
            {
                int max = 0;
                foreach (Pump p in this)
                {
                    if (p.Steps > max)
                    {
                        max = p.Steps;
                    }
                }
                return max;
            }
        }

        public void DebugPumpUsage ()
        {
            foreach (Pump p in this)
            {
                if (p.Steps > 0)
                {
                    Debug.Print($"Pump {p.ID} - {p.Ingredient} - {p.Steps} steps\r\n");
                }
            }
        }
    }
}
