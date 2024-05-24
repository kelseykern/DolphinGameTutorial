using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DolphinGame
{
    internal class Rock : Actor
    {
        public Rock(double height, double width): base(height, width) {

            sprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/slate.png"));
            rectangle.Fill = sprite;
        }
    }
}
