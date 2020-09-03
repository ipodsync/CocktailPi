using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace CocktailPi
{
    public static class Cocktail
    {
        const int STEPS_PER_ROTATION = 200;
        const int ROTATIONS_PER_OUNCE = 130;
        const int PIN_ENABLE = 7;
        const int PIN_DIRECTION = 8;

        static GpioController gpio;
        static GpioPin pinEnable;
        static GpioPin pinDirection;

        internal static void SetPumpDirectionOut()
        {
            pinDirection?.Write(GpioPinValue.High);
        }

        internal static void SetPumpDirectionIn()
        {
            pinDirection?.Write(GpioPinValue.Low);
        }

        internal static void EnableMotorDrivers()
        {
            pinEnable?.Write(GpioPinValue.High);
        }

        internal static void DisableMotorDrivers()
        {
            pinEnable?.Write(GpioPinValue.Low);
        }
        

        public static Recipes Recipes { get; private set; }

        public static Pumps Pumps { get; private set; }

        public static Ingredients Ingredients { get; private set; }



        public static void Init()
        {
            Ingredients = new Ingredients();
            Ingredients.Load();

            Recipes = new Recipes();
            Recipes.Load();

            
            gpio = GpioController.GetDefault();
            if (gpio != null)
            {
                pinEnable = gpio.OpenPin(PIN_ENABLE);
                pinEnable.Write(GpioPinValue.High);
                pinEnable.SetDriveMode(GpioPinDriveMode.InputPullDown);

                pinDirection = gpio.OpenPin(PIN_DIRECTION);
                pinDirection.Write(GpioPinValue.Low);
                pinDirection.SetDriveMode(GpioPinDriveMode.InputPullDown);
            }

            #region Pumps

            Pumps = new Pumps();

            AddPump(1, "Bourbon", 10);
            AddPump(2, "Compari", 11);
            AddPump(3, "Rye", 12);
            AddPump(4, "Gin", 13);
            AddPump(5, "Aperol", 14);
            AddPump(6, "Scotch", 15);
            AddPump(7, "Drambuie", 16);
            AddPump(8, "", 17);
            AddPump(9, "", 18);
            AddPump(10, "", 19);
            AddPump(11, "", 21);
            AddPump(12, "", 22);

            #endregion
        }

        static Pump AddPump(int number, string ingredient, int pinNumber)
        {
            Pump pump = new Pump();
            pump.Number = number;
            pump.Ingredient = ingredient;

            if (gpio != null)
            {
                GpioPin pin = gpio.OpenPin(pinNumber);
                pin.Write(GpioPinValue.High);
                pin.SetDriveMode(GpioPinDriveMode.InputPullDown);
                pump.Pin = pin;
            }

            Pumps.Add(pump);
            return pump;
        }
    }
}
