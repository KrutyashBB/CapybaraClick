using System.Drawing;

namespace CapybaraClicker
{
    public class Capybara
    {
        public Image Image { get; set; }
        public int Cost { get; set; }

        public Capybara(Image image, int cost)
        {
            Image = image;
            Cost = cost;
        }
    }
}