using System.Drawing;

namespace sonic_c_sharp
{
    public static class Background
    {
        public static void InitiateBackground()
        {
            PreviousX = -256 - BackgroundBitmap.Width;
            CurrentX = -256;
            NextX = -256 + BackgroundBitmap.Width;
        }

        public static int PreviousX;
        public static int CurrentX;
        public static int NextX;
        
        public static readonly Bitmap EmptyBackgroundBitmap = new Bitmap("graphics/bgEmpty.png");
        public static readonly Bitmap BackgroundBitmap = new Bitmap("graphics/bgEnormous.png");
        public static Bitmap CurrentBackgroundBitmap = BackgroundBitmap;

        public static void UpdateBackgroundsPositions()
        {
            if (GameState.LinkToSonicObject.X + 256 >= CurrentX + BackgroundBitmap.Width)	//if about to visibly see edge of bg while going to the right
            {
                PreviousX = CurrentX;

                CurrentX = CurrentX + BackgroundBitmap.Width;
                NextX = CurrentX + BackgroundBitmap.Width;
            }

            if (GameState.LinkToSonicObject.X - 256 <= PreviousX)	//if about to visibly see edge of bg while going to the left
            {
                NextX = CurrentX;
                
                CurrentX = CurrentX - BackgroundBitmap.Width;
                PreviousX = CurrentX - BackgroundBitmap.Width;
            }
        }
    }
}