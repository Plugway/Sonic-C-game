using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public class RedExplosionObject : GameObject
    {
        public RedExplosionObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = false;
            this.CurrentBitmap = this.explosionBitmaps[0];
        }

        private readonly Bitmap[] explosionBitmaps = 
        {
            new Bitmap("graphics/redExplosion1.png"),
            new Bitmap("graphics/redExplosion2.png"),
            new Bitmap("graphics/redExplosion3.png"),
            new Bitmap("graphics/redExplosion4.png"),
            new Bitmap("graphics/redExplosion5.png"),
            new Bitmap("graphics/redExplosion6.png"),
            new Bitmap("graphics/redExplosion7.png")
        };

        public void Move()
        {
            PerformExplodingAnimation();
            if (shouldRemoveObject)
                GameState.ObjectsToRemove.Add(this);
        }

        private bool shouldRemoveObject;

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformExplodingAnimation()
        {
            if (framesElapsed > 2)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 6)
                    shouldRemoveObject = true;
            }

            if (currentAnimationFrame <= 6)
                CurrentBitmap = explosionBitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}