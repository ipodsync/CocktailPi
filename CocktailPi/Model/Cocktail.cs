using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Gpio;
using Windows.UI.Xaml.Controls;

namespace CocktailPi
{
    public static class Cocktail
    {
        const int STEPS_PER_ROTATION = 200;
        const int ROTATIONS_PER_OUNCE = 130;
        const int STEPS_PER_OUNCE = STEPS_PER_ROTATION * ROTATIONS_PER_OUNCE;

        public const int PIN_ENABLE = 19;
        public const int PIN_DIRECTION = 26;

        static GpioController gpio;
        static GpioPin pinEnable;
        static GpioPin pinDirection;

        #region Hardware

        internal static void SetPumpDirectionOut()
        {
            pinDirection?.Write(GpioPinValue.High);
        }

        internal static void SetPumpDirectionIn()
        {
            pinDirection?.Write(GpioPinValue.Low);
        }

        internal static bool IsDriverEnabled { get; private set; } = false;

        internal static void EnableMotorDrivers()
        {
            Debug.Print("EnableMotorDrivers\r\n");
            pinEnable?.Write(GpioPinValue.Low);
            IsDriverEnabled = true;
        }

        internal static void DisableMotorDrivers()
        {
            Debug.Print("DisableMotorDrivers\r\n");
            pinEnable?.Write(GpioPinValue.High);
            IsDriverEnabled = false;
            foreach (Pump p in Pumps)
            {
                p.Stop();
            }
        }



        static void StepDelay(long us = 600)
        {
            var sw = Stopwatch.StartNew();
            long v = (us * Stopwatch.Frequency) / 1000000;
            while (sw.ElapsedTicks < v)
            {
            }
        }

        #endregion

        public static Recipes Recipes { get; private set; }

        public static Pumps Pumps { get; private set; }

        public static Ingredients Ingredients { get; private set; }

        public static ProgressBar ProgressBar { get; set; }

        public static void Init()
        {
            Pumps = new Pumps();

            Ingredients = new Ingredients();
            Ingredients.Load();

            Recipes = new Recipes();
            Recipes.Load();


            gpio = GpioController.GetDefault();
            if (gpio != null)
            {
                pinEnable = gpio.OpenPin(PIN_ENABLE);
                pinEnable.Write(GpioPinValue.High);
                pinEnable.SetDriveMode(GpioPinDriveMode.Output);

                pinDirection = gpio.OpenPin(PIN_DIRECTION);
                pinDirection.Write(GpioPinValue.Low);
                pinDirection.SetDriveMode(GpioPinDriveMode.Output);

                SetPumpDirectionIn();
                SetPumpDirectionOut();
            }

            #region Pumps

            AddPump("A1", "", 17);
            AddPump("A2", "", 27);
            AddPump("A3", "", 22);
            AddPump("A4", "", 0);
            AddPump("B1", "", 0);
            AddPump("B2", "", 0);
            AddPump("B3", "", 0);
            AddPump("B4", "", 0);
            AddPump("C1", "", 0);
            AddPump("C2", "", 0);
            AddPump("C3", "", 0);
            AddPump("C4", "", 0);

            Pumps.LoadConfiguration();

            #endregion

            _ = Windows.System.Threading.ThreadPool.RunAsync(StepTicThread, Windows.System.Threading.WorkItemPriority.Normal);

        }

        static Pump AddPump(string ID, string ingredientName, int pinNumber)
        {
            Pump pump = new Pump();
            pump.ID = ID;
            pump.Ingredient = ingredientName;

            if (gpio != null && pinNumber > 0)
            {
                GpioPin pin = gpio.OpenPin(pinNumber);
                pin.Write(GpioPinValue.High);
                pin.SetDriveMode(GpioPinDriveMode.Output);
                pump.Pin = pin;
            }

            Pumps.Add(pump);
            return pump;
        }

        static public void LoadRecipeOntoPumps(Recipe recipe)
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

        static public Pump FindIngredientPump(string ingredient)
        {
            foreach (Pump p in Pumps)
            {
                if (p.Ingredient.Equals(ingredient))
                    return p;
            }
            return null;
        }


        static private void StepTicThread(Windows.Foundation.IAsyncAction action)
        {
            //This thread runs on a high priority task and loops forever
            while (true)
            {
                bool workPerformed = false;
                if (IsDriverEnabled)
                {
                    foreach (Pump p in Pumps)
                    {
                        if (p.DoStep)
                        {
                            p.PinHigh();
                        }
                    }
                    StepDelay();

                    foreach (Pump p in Pumps)
                    {
                        if (p.DoStep)
                        {
                            p.PinLow();
                            p.Steps--;
                            workPerformed = true;
                        }
                    }
                    StepDelay();

                    //if (ProgressBar != null && ProgressBar.Value < ProgressBar.Maximum)
                    //{
                    //    ProgressBar.Value++;
                    //}

                    if (!workPerformed)
                    {
                        DisableMotorDrivers();
                    }
                }
            }
        }

        public static async Task ExecuteRecipe(Recipe recipe)
        {
            LoadRecipeOntoPumps(recipe);
            Pumps.DebugPumpUsage();
            SetPumpDirectionOut();

            ProgressBar.Maximum = Pumps.MaxSteps;
            ProgressBar.Value = 0;

            EnableMotorDrivers();
            //_ = Windows.System.Threading.ThreadPool.RunAsync(StepTicThread, Windows.System.Threading.WorkItemPriority.Normal);


            //bool recipeComplete = false;
            //int step = 0;
            //int totalSteps = Pumps.MaxSteps;
            //int percent = 0;
            //int newPercent = 0;
            //recipe.ExecutionProgress = 0;

            //while (!recipeComplete)
            //{
            //    recipeComplete = true;
            //    foreach (Pump p in Pumps)
            //    {
            //        if (p.Steps > 0)
            //        {
            //            p.PinHigh();

            //        }
            //    }
            //    StepDelay();
            //    foreach (Pump p in Pumps)
            //    {
            //        if (p.Steps > 0)
            //        {
            //            //Debug.Print($"{p.ID}-{p.Steps}\t");

            //            p.PinLow();
            //            p.Steps--;
            //            if (p.Steps > 0)
            //            {
            //                recipeComplete = false;
            //            }
            //        }
            //    }
            //    StepDelay();
            //    step++;

            //    newPercent = (int)(((float)step / (float)totalSteps) * 100);
            //    if (newPercent != percent)
            //    {
            //        percent = newPercent;
            //        ProgressBar.Value = percent;
            //    }

            //    //Debug.Print($" - {recipe.ExecutionProgress} percent\r\n");

            //}
            //ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //DisableMotorDrivers();
        }

    }
}
