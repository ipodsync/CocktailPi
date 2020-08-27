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
            GpioController gpio = GpioController.GetDefault();
            if (gpio == null)
                return;

            int sleep = 1;

            // Open GPIO 5
            using (GpioPin pinStep = gpio.OpenPin(3)) 
            using (GpioPin pinDir = gpio.OpenPin(4))
            {
                // Latch HIGH value first. This ensures a default value when the pin is set as output
                pinStep.Write(GpioPinValue.High);
                pinStep.SetDriveMode(GpioPinDriveMode.Output);

                pinDir.Write(GpioPinValue.Low);
                pinDir.SetDriveMode(GpioPinDriveMode.Output);
                pinDir.Write(GpioPinValue.High);

                for (int x=0; x<200 * 130; x++)
                {
                    StepperStepOne(pinStep);
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
        }

        void StepperStepOne (GpioPin pinStep)
        {
            double speed = 1;
            //var signal = Task.Run(async delegate { await Task.Delay(TimeSpan.FromMilliseconds(speed)); });
            //var pavza = Task.Run(async delegate { await Task.Delay(TimeSpan.FromMilliseconds(speed)); });
            pinStep.Write(GpioPinValue.High);
            //signal.Wait();
            //Task.Delay(-1).Wait(1);
            udelay(600);
            pinStep.Write(GpioPinValue.Low);
            udelay(600);
            //Task.Delay(-1).Wait(1);
            //pavza.Wait();
        }

        static void udelay(long us)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            long v = (us * System.Diagnostics.Stopwatch.Frequency) / 1000000;
            while (sw.ElapsedTicks < v)
            {
            }
        }
    }
}
