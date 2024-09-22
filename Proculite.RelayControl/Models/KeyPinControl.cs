namespace Proculite.RelayControl.Models
{
    public class KeyPinControl
    {
        public string Key { get; set; } = "";
        public int[] Pins { get; set; } = Array.Empty<int>();
    }
}