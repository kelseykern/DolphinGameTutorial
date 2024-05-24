using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Controls;

namespace DolphinGame
{
    internal class Player : Actor
    {
        private int image_index;
        private string[] images_array;

        public int moveRight = 0;
        public int moveLeft = 0;
        public int moveDown = 0;
        public int moveUp = 0;

        public Player(double height, double width) : base(height, width)
        {
            rectangle.Stroke = Brushes.Red;
            images_array = Directory.GetFiles("../../images/dolphin/", "*", SearchOption.TopDirectoryOnly).OrderBy(f => f).Select(x => System.IO.Path.GetFileName(x)).ToArray();
        }

        public void animate()
        {

            if (image_index >= images_array.Length)
            {
                image_index = 0;
            }

            sprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dolphin/" + images_array[(int)(image_index)]));
            rectangle.Fill = sprite;

            image_index++;


        }
    }
}
