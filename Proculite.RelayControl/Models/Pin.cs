namespace Proculite.RelayControl.Models
{
    public class Pin
    {
        public string Name { get; set; } = "";
        public int Number { get; set; }
        public string Active { get; set; } = "low";
        public bool ActiveHigh => Active.ToLower() == "high";
    }
}
