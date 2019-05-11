using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public class SmokeObject : GameObject
    {
        public SmokeObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = false;
            this.CurrentBitmap = this.bitmaps[0];
        }
        
        private readonly Bitmap[] bitmaps = 
        {
            new Bitmap("graphics/bgSmoke1.png"),
            new Bitmap("graphics/bgSmoke2.png"),
            new Bitmap("graphics/bgSmoke3.png"),
            new Bitmap("graphics/bgSmoke4.png"),
            new Bitmap("graphics/bgSmoke5.png"),
            new Bitmap("graphics/bgSmoke6.png"),
            new Bitmap("graphics/bgSmoke7.png"),
            new Bitmap("graphics/bgSmoke8.png")
        };

        private static Random random = new Random();
        private int withoutMovingframesElapsed = random.Next(0,50);
        public void Move()
        {
            if (withoutMovingframesElapsed > 50)
            {
                PerformAnimation();
            }

            ++withoutMovingframesElapsed;
        }

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformAnimation()
        {
            if (framesElapsed > 3)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 7)
                {
                    currentAnimationFrame = 0;
                    withoutMovingframesElapsed = 0;
                }
            }

            CurrentBitmap = bitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}