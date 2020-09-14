using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CocktailPi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrinkDetails : Page
    {
        public Recipe Item { get; set; }

        public DrinkDetails()
        {
            this.InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cocktail.ExecuteRecipe(Item);
            return;

            GpioController gpio = GpioController.GetDefault();
            if (gpio == null)
                return;

            int sleep = 1;

            // Open GPIO 5
            Cocktail.EnableMotorDrivers();
            Cocktail.SetPumpDirectionOut();

            using (GpioPin pinStep1 = gpio.OpenPin(17))
            using (GpioPin pinStep2 = gpio.OpenPin(27))
            using (GpioPin pinStep3 = gpio.OpenPin(22)) 
            {
                // Latch HIGH value first. This ensures a default value when the pin is set as output
                pinStep1.Write(GpioPinValue.High);
                pinStep1.SetDriveMode(GpioPinDriveMode.Output);

                pinStep2.Write(GpioPinValue.High);
                pinStep2.SetDriveMode(GpioPinDriveMode.Output);

                pinStep3.Write(GpioPinValue.High);
                pinStep3.SetDriveMode(GpioPinDriveMode.Output);


                for (int x=0; x<200 * 130; x++)
                {
                    //StepperStepOne(pinStep1);
                    //StepperStepOne(pinStep2);
                    //StepperStepOne(pinStep3);

                    pinStep1.Write(GpioPinValue.High);
                    pinStep2.Write(GpioPinValue.High);
                    pinStep3.Write(GpioPinValue.High);
                    udelay();

                    pinStep1.Write(GpioPinValue.Low);
                    pinStep2.Write(GpioPinValue.Low);
                    pinStep3.Write(GpioPinValue.Low);
                    udelay();
                    //pinStep.Write(GpioPinValue.High);
                    //Thread.Sleep(sleep);
                    //pinStep.Write(GpioPinValue.Low);
                    //Thread.Sleep(sleep);
                }

                //pinDir.Write(GpioPinValue.Low);
                //for (int x = 0; x < 200; x++)
                //{
                //    StepperStepOne(pinStep);
                //    //pinStep.Write(GpioPinValue.High);
                //    //Thread.Sleep(sleep);
                //    //pinStep.Write(GpioPinValue.Low);
                //    //Thread.Sleep(sleep);
                //}

            }
            Cocktail.DisableMotorDrivers();
        }

        void StepperStepOne (GpioPin pinStep)
        {
            pinStep.Write(GpioPinValue.High);
            udelay();

            pinStep.Write(GpioPinValue.Low);
            udelay();
        }

        static void udelay(long us = 700)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            long v = (us * System.Diagnostics.Stopwatch.Frequency) / 1000000;
            while (sw.ElapsedTicks < v)
            {
            }
        }
    }
}
