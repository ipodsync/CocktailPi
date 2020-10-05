using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Gpio;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace CocktailPi
{
    public static class Cocktail
    {
        const int STEPS_PER_ROTATION = 200;
        const int ROTATIONS_PER_OUNCE = 130;
        const int STEPS_PER_OUNCE = STEPS_PER_ROTATION * ROTATIONS_PER_OUNCE;

        public const int PIN_ENABLE = 18;
        public const int PIN_DIRECTION = 23;

        static GpioController gpio;
        static GpioPin pinEnable;
        static GpioPin pinDirection;

        static MediaPlayer player;

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
            player.Pause();
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

        public static async Task InitAsync()
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

            AddPump("A1", "", 4);
            AddPump("A2", "", 17);
            AddPump("A3", "", 27);
            AddPump("A4", "", 22);

            AddPump("B1", "", 10);
            AddPump("B2", "", 9);
            AddPump("B3", "", 11);
            AddPump("B4", "", 5);

            AddPump("C1", "", 6);
            AddPump("C2", "", 13);
            AddPump("C3", "", 19);
            AddPump("C4", "", 16);

            Pumps.LoadConfiguration();

            #endregion

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/make.mp3"));
            player = BackgroundMediaPlayer.Current;
            player.AutoPlay = false;
            player.SetFileSource(file);

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
            player.Position = TimeSpan.Zero;
            player.Play();
            LoadRecipeOntoPumps(recipe);
            Pumps.DebugPumpUsage();
            SetPumpDirectionOut();

            ProgressBar.Maximum = Pumps.MaxSteps;
            ProgressBar.Value = 0;

            EnableMotorDrivers();
         
        }

    }
}
