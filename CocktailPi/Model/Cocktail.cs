using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace CocktailPi
{
    public class Cocktail
    {
        const int STEPS_PER_ROTATION = 200;
        const int ROTATIONS_PER_OUNCE = 130;
        const int PIN_ENABLE = 7;
        const int PIN_DIRECTION = 8;

        GpioController gpio;
        GpioPin pinEnable;
        GpioPin pinDirection;
        

        public Recipes Recipes { get; private set; }

        public Pumps Pumps { get; private set; }


        public void Init()
        {
            gpio = GpioController.GetDefault();
            if (gpio == null)
                return;

            pinEnable = gpio.OpenPin(PIN_ENABLE);
            pinEnable.Write(GpioPinValue.High);
            pinEnable.SetDriveMode(GpioPinDriveMode.InputPullDown);

            pinDirection = gpio.OpenPin(PIN_DIRECTION);
            pinDirection.Write(GpioPinValue.Low);
            pinDirection.SetDriveMode(GpioPinDriveMode.InputPullDown);


        }
    }
}
