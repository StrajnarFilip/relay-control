using Proculite.RelayControl.Models;
using System.Device.Gpio;

namespace Proculite.RelayControl.Services
{
    public class PinService
    {
        private readonly IConfiguration _configuration;
        private readonly KeyPinControl[] _keyPinControl;
        private readonly Dictionary<string, Pin[]> _pinControlMap;
        private readonly int[] _allPinNumbers;
        private readonly Dictionary<int, GpioController> _gpioControllerMap;

        public PinService(IConfiguration configuration)
        {
            _configuration = configuration;
            _keyPinControl = configuration
                .GetSection("KeyPinControl")
                .GetChildren()
                .Select(keyPinControl => new KeyPinControl
                {
                    Key = keyPinControl.GetSection("Key").Get<string>() ?? "",
                    Pins = keyPinControl
                        .GetSection("Pins")
                        .GetChildren()
                        .Select(pin => new Pin
                        {
                            Name = pin.GetSection("Name").Get<string>() ?? "",
                            Number = pin.GetSection("Number").Get<int>()
                        })
                        .ToArray()
                })
                .ToArray();
            _pinControlMap = _keyPinControl.ToDictionary(
                pinControl => pinControl.Key,
                pinControl => pinControl.Pins
            );

            _allPinNumbers = _keyPinControl
                .SelectMany(pinController => pinController.Pins.Select(pin => pin.Number))
                .ToArray();

            _gpioControllerMap = new();

            foreach(int pin in _allPinNumbers)
            {
                _gpioControllerMap[pin] = new GpioController();
                _gpioControllerMap[pin].OpenPin(pin);
            }
        }

        public Pin[] AccessiblePins(HttpRequest httpRequest)
        {
            string? keyCookie = httpRequest.Cookies["key"];
            bool keyExists = keyCookie is not null && _pinControlMap.ContainsKey(keyCookie);
            Pin[] controlledPins = keyExists ? _pinControlMap[keyCookie] : Array.Empty<Pin>();
            return controlledPins;
        }
    }
}
