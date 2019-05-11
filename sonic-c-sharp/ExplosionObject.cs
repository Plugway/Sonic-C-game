using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public class ExplosionObject : GameObject
    {
        public ExplosionObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = false;
            this.CurrentBitmap = this.explosionBitmaps[0];
        }

       // public Point[] AABB = { new Point(0, 0),
       //                         new Point(15, 15) };

        private readonly Bitmap[] explosionBitmaps = 
        {
            new Bitmap("graphics/explosion1.png"),
            new Bitmap("graphics/explosion2.png"),
            new Bitmap("graphics/explosion3.png"),
            new Bitmap("graphics/explosion4.png"),
            new Bitmap("graphics/explosion5.png")
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
                if (currentAnimationFrame > 4)
                    shouldRemoveObject = true;
            }

            if (currentAnimationFrame <= 4)
                CurrentBitmap = explosionBitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}