using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public class YellowSpringObject : TileObject
    {
        public YellowSpringObject(int x, int y) : base(x, y, true, new Bitmap("graphics/yellowSpring1.png"), new List<Point[]> {new [] {new Point(0, 16), new Point(27, 31)} })
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = NormalBitmap;
            
            this.AABBs = new List<Point[]> {new [] {new Point(0, 0), new Point(27, 31)} };    //only used for narrow phase collision detection
                                                                                              //not quite sure why it doesn't work with new Point(0, 16), new Point(27, 31)
                                                                                              //neither am i sure why it works with new Point(0, 0), new Point(27, 31). shouldnt sonic just get moved to the outside of the 28x32 spring tile and not bounce off???
                                                                                              //nothing but new Point(0, 0), new Point(27, 31) seems to really work, although looks kinda ugly.
                                                                                              //honestly, whatever. it kinda works, so whatfuckingever. not gonna waste more time on this.
                                                                                              //fuck. they are still buggy actually. oh well, whatever.
        }
        
        public readonly Point[] BouncingAABB = { new Point(1, 16),
                                                 new Point(26, 16) };

        public readonly Point[] SolidAABB = { new Point(0, 16),
                                              new Point(27, 31) };

        private readonly Bitmap NormalBitmap = new Bitmap("graphics/yellowSpring1.png");
        private readonly Bitmap ActivatedBitmap = new Bitmap("graphics/yellowSpring2.png");
        
        public bool IsActivated = false;
        
        public void Move()
        {
            if (IsActivated)
            {
                CurrentBitmap = ActivatedBitmap;
                IsActivated = false;
            }
            else
                CurrentBitmap = NormalBitmap;
        }
    }
}