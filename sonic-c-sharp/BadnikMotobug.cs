using System;
using System.Drawing;
using System.Collections.Generic;

namespace sonic_c_sharp
{
    public class BadnikMotobugObject : GameObject
    {
        public BadnikMotobugObject(int x, int y, List<Point[]> AABB)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = Bitmaps[0];
            this.AABB = AABB;
        }

        public List<Point[]> AABB;    //only Point[0][..] is supposed to be used

        private Bitmap[] Bitmaps = 
        {
            new Bitmap("graphics/badnikMotobug1.png"),
            new Bitmap("graphics/badnikMotobug2.png"),
            new Bitmap("graphics/badnikMotobug3.png"),
            new Bitmap("graphics/badnikMotobug4.png")
        };
        private bool[] bitmapIsFlipped = new bool[4];

        public Point[] GroundLeftCollisionBox = new [] { new Point(-1, 28), new Point(1, 30) };
        public Point[] GroundRightCollisionBox = new [] { new Point(34, 28), new Point(36, 30) };
        public Point[] WallLeftCollisionBox = new [] { new Point(0, 0), new Point(0, 27) };
        public Point[] WallRightCollisionBox = new [] { new Point(35, 0), new Point(35, 27) };
        
        public bool GroundLeftIsActive;
        public bool GroundRightIsActive;
        public bool WallLeftIsActive;
        public bool WallRightIsActive;

        private bool isMovingLeft;

        private int pauseTimer;
        
        public void Move()
        {
            if (isMovingLeft)
            {
                if (WallLeftIsActive || !GroundLeftIsActive)
                {
                    isMovingLeft = false;
                    pauseTimer = 15;
                }
            }
            else if (!isMovingLeft)
            {
                if (WallRightIsActive || !GroundRightIsActive)
                {
                    isMovingLeft = true;
                    pauseTimer = 15;
                }
            }

            if (pauseTimer > 0)
                --pauseTimer;
            else
            {
                if (isMovingLeft)
                    X -= 2;
                if (!isMovingLeft)
                    X += 2;
                
                PerformMovementAnimation();
            }
        }

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformMovementAnimation()
        {
            if (framesElapsed > 6)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 3)
                    currentAnimationFrame = 0;
            }

            ++framesElapsed;
            
            
            //checking conditions for flipping animations
            if (bitmapIsFlipped[currentAnimationFrame] && isMovingLeft)
            {
                Bitmaps[currentAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                bitmapIsFlipped[currentAnimationFrame] = false;
            }
            else if (!bitmapIsFlipped[currentAnimationFrame] && !isMovingLeft)
            {
                Bitmaps[currentAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                bitmapIsFlipped[currentAnimationFrame] = true;
            }

            CurrentBitmap = Bitmaps[currentAnimationFrame];
        }
    }
}