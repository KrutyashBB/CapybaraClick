using System.Drawing;

namespace CapybaraClicker
{
    public class Capybara
    {
        public Image ImgPath { get; set; }
        public int Cost { get; set; }

        public bool IsBuy { get; set; }

        public Capybara(Image imgPath, int cost, bool isBuy)
        {
            ImgPath = imgPath;
            Cost = cost;
            IsBuy = isBuy;
        }
    }
}