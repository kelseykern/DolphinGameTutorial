using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DolphinGame
{
    internal class Actor
    {
        public Rectangle rectangle;

        protected ImageBrush sprite;
        public Actor(double height, double width)
        {
            rectangle = new Rectangle();
            rectangle.Height = height;
            rectangle.Width = width;

            sprite = new ImageBrush();
        }
    }
}
