using System.Drawing;
using System.Windows.Forms;

namespace CapybaraClicker
{
    public class Modification
    {
        public Image Image { get; set; }
        public string BigModifLabel { get; set; }
        public string SmallModifLabel { get; set; }
        public int Cost { get; set; }
        public int Bonus { get; set; }
        public int NumberOfPurchase { get; set; }
        public TypesOfModifications Type { get; set; }

        public Modification(Image image, string bigModifLabel, string smallModifLabel, int cost, int bonus,
            int numberOfPurchase,
            TypesOfModifications type)
        {
            Image = image;
            BigModifLabel = bigModifLabel;
            SmallModifLabel = smallModifLabel;
            Cost = cost;
            Bonus = bonus;
            NumberOfPurchase = numberOfPurchase;
            Type = type;
        }
    }
}