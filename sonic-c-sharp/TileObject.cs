using System.Drawing;
using System.Collections.Generic;

namespace sonic_c_sharp
{
    public class TileObject : GameObject
    {
        public TileObject(int x, int y, bool isCollidable, Bitmap bitmap, List<Point[]> AABBs)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = isCollidable;
            this.CurrentBitmap = bitmap;
            this.AABBs = AABBs;
            this.BigAABB = new [] { new Point(0, 0), new Point(bitmap.Width, bitmap.Height) };    //for broad-phase collision detections
        }

        public List<Point[]> AABBs;
        public readonly Point[] BigAABB;
    }
}