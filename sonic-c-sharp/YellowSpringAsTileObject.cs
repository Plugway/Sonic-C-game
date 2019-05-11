using System.Drawing;
using System.Collections.Generic;

namespace sonic_c_sharp
{
    public class YellowSpringAsTileObject : TileObject
    {
        //this object only used for narrow phase collision detection
        
        public YellowSpringAsTileObject(int x, int y) : base(x, y, true, null, null)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = new Bitmap("ringRotating2.bmp");    //for testing only
            
        }

        public List<Point[]> AABBs;
    }
}