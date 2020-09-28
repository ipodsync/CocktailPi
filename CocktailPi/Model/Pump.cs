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
        #region Properties 

        public string ID { get; set; } = "";

        public string Title { get => $"Pump {ID:00}"; }

        public string Ingredient { get; set; } = "";

        public GpioPin Pin { get; set; } = null;

        public int Steps { get; set; } = 0;

        public bool IsPriming { get; set; } = false;

        public bool CanPrime { get => !IsRecovering; }

        public bool IsRecovering { get; set;  } = false;

        public bool DoStep { get => IsPriming || IsRecovering || Steps > 0; }

        public bool CanRecover { get => !IsPriming; }

        public string CaptionPrimeButton { get => IsPriming ? "Stop Prime" : "Start Prime"; }

        public string CaptionRecoverButton { get => IsRecovering ? "Stop Recover" : "Start Recover"; }

        #endregion

        #region Hardware

        internal void StartPrime()
        {
            Debug.Print("StartPrime\r\n");

            Cocktail.SetPumpDirectionOut();
            Cocktail.EnableMotorDrivers();
            IsPriming = true;
        }

        internal void StopPrime()
        {
            Debug.Print("StopPrime\r\n");

            Cocktail.DisableMotorDrivers();
        }

        internal void StartRecover()
        {
            Debug.Print("StartRecover\r\n");

            Cocktail.SetPumpDirectionIn();
            Cocktail.EnableMotorDrivers();
            IsRecovering = true;
        }

        internal void StopRecover()
        {
            Debug.Print("StopRecover\r\n");
            Cocktail.DisableMotorDrivers();
        }

        public void PinHigh()
        {
            Pin?.Write(GpioPinValue.High);
        }

        public void PinLow()
        {
            Pin?.Write(GpioPinValue.Low);
        }

        public void Stop ()
        {
            IsPriming = false;
            IsRecovering = false;
            Steps = 0;
        }

        #endregion

        #region Persistance 

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

        #endregion
    }
}
