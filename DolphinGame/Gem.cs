using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DolphinGame
{
    internal class Gem : Actor
    {
        public Gem(double height, double width) : base(height, width)
        {

            sprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/ruby.png"));
            rectangle.Fill = sprite;
        }
    }
}
