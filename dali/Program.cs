using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Threading;

namespace dali
{
    class Program
    {
        static void Main(string[] args)
        {
            Heat(PinValue.High);

            var pwm = PwmChannel.Create(0, 0, 10, 1);
            pwm.Start();

            Thread.Sleep(5000);

            pwm.Stop();
            Heat(PinValue.Low);
        }

        private static void Heat(PinValue pinValue)
        {
            int pin = 18;
            var controller = new GpioController();
            controller.OpenPin(pin, PinMode.Output);
            controller.Write(pin, pinValue);
            controller.ClosePin(pin);
        }

    }
}
