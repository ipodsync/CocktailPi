using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace CocktailPi
{
    public class Pump
    {
        public int Number { get; set; } = 0;

        public string Title { get => $"Pump {Number:00}"; }

        public string Ingredient { get; set; } = "";

        public GpioPin Pin { get; set; } = null;

        internal void StartPrime()
        {
            Debug.Print("StartPrime");

            Cocktail.SetPumpDirectionOut();
            Cocktail.EnableMotorDrivers();
        }

        internal void StopPrime()
        {
            Debug.Print("StopPrime");

            Cocktail.DisableMotorDrivers();
        }

        internal void StartRecover()
        {
            Debug.Print("StartRecover");

            Cocktail.SetPumpDirectionIn();
            Cocktail.EnableMotorDrivers();
        }

        internal void StopRecover()
        {
            Debug.Print("StopRecover");

            Cocktail.DisableMotorDrivers();
        }

    }
}
