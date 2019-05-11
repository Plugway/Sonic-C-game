using System.Drawing;

namespace sonic_c_sharp
{
    public static class HUD
    {
        private static readonly Bitmap ringsYellowBitmap = new Bitmap("graphics/HUDRingsYellow.png");
        private static readonly Bitmap ringsRedBitmap = new Bitmap("graphics/HUDRingsRed.png");
        public static Bitmap CurrentRingsBitmap = ringsYellowBitmap;
        
        public static readonly Bitmap[] NumbersBitmaps =
        {
            new Bitmap("graphics/0.png"),
            new Bitmap("graphics/1.png"),
            new Bitmap("graphics/2.png"),
            new Bitmap("graphics/3.png"),
            new Bitmap("graphics/4.png"),
            new Bitmap("graphics/5.png"),
            new Bitmap("graphics/6.png"),
            new Bitmap("graphics/7.png"),
            new Bitmap("graphics/8.png"),
            new Bitmap("graphics/9.png")
        };

        private static int framesElapsed = 0;
        private static int currentAnimationFrame = 0;
        public static void PerformAnimation()
        {
            if (GameState.LinkToSonicObject.Rings == 0)
            {
                if (framesElapsed > 10)
                {
                    framesElapsed = 0;
                    ++currentAnimationFrame;
                    if (currentAnimationFrame > 1)
                        currentAnimationFrame = 0;
                }
                ++framesElapsed;

                if (currentAnimationFrame == 0)
                    CurrentRingsBitmap = ringsYellowBitmap;
                else if (currentAnimationFrame == 1)
                    CurrentRingsBitmap = ringsRedBitmap;
            }
            else
                CurrentRingsBitmap = ringsYellowBitmap;
        }
    }
}