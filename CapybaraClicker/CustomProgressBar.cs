using System.Drawing;
using System.Windows.Forms;

namespace CapybaraClicker
{
    public class CustomProgressBar : ProgressBar
    {
        public CustomProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height -= 4;
            e.Graphics.FillRectangle(Brushes.Brown, 2, 2, Width - 4, Height - 4);
            e.Graphics.FillRectangle(Brushes.Orange, 2, 2, rec.Width, rec.Height);
        }
    }
}