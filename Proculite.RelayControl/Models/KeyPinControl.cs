namespace Proculite.RelayControl.Models
{
    public class KeyPinControl
    {
        public string Key { get; set; } = "";
        public Pin[] Pins { get; set; } = Array.Empty<Pin>();
    }
}
