using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public class InvisibleDamagingObject : GameObject
    {
        public InvisibleDamagingObject(int x, int y, List<Point[]> AABB)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.AABB = AABB;

            this.CurrentBitmap = new Bitmap("graphics/bgEmpty.png");
        }

        public List<Point[]> AABB;        //only Point[0][..] is supposed to be used
    }
}