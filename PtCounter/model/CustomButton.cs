using System.Windows.Forms;

namespace PtCounter.model
{
    public class CustomButton : Button
    {
        public readonly string Moniker;
        public readonly string CamName;

        public CustomButton(string Moniker, string CamName) : base()
        {
            this.Moniker = Moniker;

            this.CamName = CamName;
        }
    }
}
