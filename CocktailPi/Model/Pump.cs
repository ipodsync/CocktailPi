using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Storage;

namespace CocktailPi
{
    public class Pump
    {
        public string ID { get; set; } = "";

        public string Title { get => $"Pump {ID:00}"; }

        public string Ingredient { get; set; } = "";

        public GpioPin Pin { get; set; } = null;

        public int Steps { get; set; } = 0;

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

        public void SaveConfiguration()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[$"{ID}.Ingredient"] = Ingredient;
        }

        public void LoadConfiguration()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey($"{ID}.Ingredient"))
            {
                Ingredient = localSettings.Values[$"{ID}.Ingredient"] as string;
            }
        }
    }
}
