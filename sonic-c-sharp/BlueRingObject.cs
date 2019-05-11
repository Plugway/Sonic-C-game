using System.Drawing;

namespace sonic_c_sharp
{
    public class BlueRingObject : RingObject
    {
        public BlueRingObject(int x, int y) : base(x, y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = this.rotatingBitmaps[0];
        }

        public Point[] AABB = { new Point(0, 0),
                                new Point(15, 15) };

        private Bitmap[] rotatingBitmaps = 
        {
            new Bitmap("graphics/blueRingRotating1.png"),
            new Bitmap("graphics/blueRingRotating2.png"),
            new Bitmap("graphics/blueRingRotating3.png"),
            new Bitmap("graphics/blueRingRotating4.png")
        };

        public void Move()
        {
            PerformRotatingAnimation();
        }

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformRotatingAnimation()
        {
            if (framesElapsed > 3)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 3)
                    currentAnimationFrame = 0;
            }

            CurrentBitmap = rotatingBitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}