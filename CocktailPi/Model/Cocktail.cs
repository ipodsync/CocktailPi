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
        const int STEPS_PER_OUNCE = STEPS_PER_ROTATION * ROTATIONS_PER_OUNCE;

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

            AddPump("A1", "Bourbon", 10);
            AddPump("A2", "Campari", 11);
            AddPump("A3", "Rye", 12);
            AddPump("A4", "Gin", 13);
            AddPump("B1", "Aperol", 14);
            AddPump("B2", "Scotch", 15);
            AddPump("B3", "Drambuie", 16);
            AddPump("B4", "", 17);
            AddPump("C1", "", 18);
            AddPump("C2", "", 19);
            AddPump("C3", "", 21);
            AddPump("C4", "", 22);

            Pumps.LoadConfiguration();

            #endregion
        }

        static Pump AddPump(string ID, string ingredientName, int pinNumber)
        {
            Pump pump = new Pump();
            pump.ID = ID;
            pump.Ingredient = ingredientName;

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

        static public void LoadRecipeOntoPumps (Recipe recipe)
        {
            foreach (Pump p in Pumps)
            {
                p.Steps = 0;
            }

            foreach (Ingredient i in recipe.Ingredients)
            {
                Pump p = FindIngredientPump(i.Name);
                p.Steps = (int)((float)STEPS_PER_OUNCE * i.Qnty);
            }
        }

        static public Pump FindIngredientPump (string ingredient)
        {
            foreach (Pump p in Pumps)
            {
                if (p.Ingredient.Equals(ingredient))
                    return p;
            }
            return null;
        }
    }
}
