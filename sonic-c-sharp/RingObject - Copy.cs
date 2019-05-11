using System.Drawing;

namespace sonic_c_sharp
{
    public class RingObject : GameObject
    {
        public RingObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = this.rotatingBitmaps[0];
        }

        public Point[] AABB = { new Point(0, 0),
                                new Point(15, 15) };

        private readonly Bitmap[] rotatingBitmaps = 
        {
            new Bitmap("ringRotating1.png"),
            new Bitmap("ringRotating2.png"),
            new Bitmap("ringRotating3.png"),
            new Bitmap("ringRotating4.png")
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