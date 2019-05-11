using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public static class FadingToBlack
    {
        /*
        public LevelCard(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = false;
            this.CurrentBitmap = this.SparklesBitmaps[0];
        }
        */

        public static int X = 0;
        public static int Y = 0;

        public static Bitmap[] LevelCardBitmaps = 
        {
            new Bitmap("levelCard1.png"),
            new Bitmap("levelCard2.png"),
            new Bitmap("levelCard3.png"),
            new Bitmap("levelCard4.png"),
            new Bitmap("levelCard5.png"),
            new Bitmap("levelCard6.png"),
            new Bitmap("levelCard7.png"),
            new Bitmap("levelCard8.png"),
            new Bitmap("levelCard9.png"),
            new Bitmap("levelCard10.png"),
            new Bitmap("levelCard11.png"),
            new Bitmap("levelCard12.png"),
            new Bitmap("levelCard13.png"),
            new Bitmap("levelCard14.png"),
            new Bitmap("levelCard15.png"),
            new Bitmap("levelCard16.png"),
            new Bitmap("levelCard17.png"),
            new Bitmap("levelCard18.png"),
            new Bitmap("levelCard19.png"),
            new Bitmap("levelCard20.png")
        };

        /*
        public void Move()
        {
            PerformRotatingAnimation();
            if (shouldRemoveObject)
                GameState.ObjectsToRemove.Add(this);
        }

        private bool shouldRemoveObject;

        public int framesElapsed = 0;
        public int currentAnimationFrame = 0;
        public void PerformRotatingAnimation()
        {
            if (framesElapsed > 1)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 3)
                    shouldRemoveObject = true;
            }

            if (currentAnimationFrame <= 3)
                CurrentBitmap = SparklesBitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
        */
    }
}