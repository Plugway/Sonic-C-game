using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public class BadnikFishObject : GameObject
    {
        public BadnikFishObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = bitmaps[0];
            this.ySpeed = 0;
        }

        public readonly Point[] AABB = { new Point(0, 0),
                                         new Point(19, 31) };

        private float ySpeed;
        private bool isMovingDown = false;

        private readonly Bitmap[] bitmaps = 
        {
            new Bitmap("graphics/badnikFish1.png"),
            new Bitmap("graphics/badnikFish2.png")
        };
        
        public void Move()
        {
            if (isMovingDown)
                ySpeed += 0.9f;
            else
                ySpeed -= 0.9f;
    
            if (isMovingDown && ySpeed > 15)
                isMovingDown = false;
            else if (!isMovingDown && ySpeed < -15)
                isMovingDown = true;
    
            Y += (int) ySpeed;

            PerformMovementAnimation();
        }

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformMovementAnimation()
        {
            if (framesElapsed > 1)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 1)
                    currentAnimationFrame = 0;
            }
            ++framesElapsed;

            CurrentBitmap = bitmaps[currentAnimationFrame];
        }
    }
}