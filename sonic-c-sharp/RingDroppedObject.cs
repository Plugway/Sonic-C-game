using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public class RingDroppedObject : GameObject
    {
        public RingDroppedObject(int x, int y, float xSpeed, float ySpeed)
        {
            this.X = x;
            this.Y = y;
            this.XSpeed = xSpeed;
            this.YSpeed = ySpeed;
            this.CurrentBitmap = this.RotatingBitmaps[0];
        }

        private const float Gravity = 0.09375f;
        
        public float XSpeed;
        public float YSpeed;

        public readonly Point[] AABB = { new Point(0, 0),
                                new Point(15, 15) };

        public int DontCheckTileCollisionsTimer = 0;
        private int totalFramesPassed = 0;
        
        public Bitmap[] RotatingBitmaps = 
        {
            new Bitmap("graphics/ringRotating1.png"),
            new Bitmap("graphics/ringRotating2.png"),
            new Bitmap("graphics/ringRotating3.png"),
            new Bitmap("graphics/ringRotating4.png")
        };

        public void Move()
        {
            if (Math.Abs(XSpeed) < 1)
            {
                XSpeed = 1;       //a hack so that rings hopefully don't get stuck in tiles when they get thrown straight up
            }
            
            YSpeed += Gravity*2;

            X += (int)XSpeed;
            Y += (int)YSpeed;
            
            PerformRotatingAnimation();

            ++totalFramesPassed;
            if (totalFramesPassed > 128)
                GameState.ObjectsToRemove.Add(this);
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

            CurrentBitmap = RotatingBitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}