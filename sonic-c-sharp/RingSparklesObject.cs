using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public class RingSparklesObject : GameObject
    {
        public RingSparklesObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = false;
            this.CurrentBitmap = this.sparklesBitmaps[0];
        }

        private readonly Bitmap[] sparklesBitmaps = 
        {
            new Bitmap("graphics/ringSparkles1.png"),
            new Bitmap("graphics/ringSparkles2.png"),
            new Bitmap("graphics/ringSparkles3.png"),
            new Bitmap("graphics/ringSparkles4.png")
        };

        public void Move()
        {
            PerformRotatingAnimation();
            if (shouldRemoveObject)
                GameState.ObjectsToRemove.Add(this);
        }

        private bool shouldRemoveObject;

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformRotatingAnimation()
        {
            if (framesElapsed > 1)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 3)
                    shouldRemoveObject = true;
            }

            if (currentAnimationFrame <= 3)
                CurrentBitmap = sparklesBitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}