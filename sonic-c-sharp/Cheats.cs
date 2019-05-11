using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public static class Cheats
    {
        public static bool AllCheatsAreUnlocked;
        
        public static bool GrayscaleCheatIsActive;
        public static bool DynamicCameraIsActive;
        public static bool DisableRingsCollectingSoundIsActive;
        public static bool SonicCDJumpSoundIsActive;
        public static bool DisableBackgroundIsActive;
        public static bool MoonGravityIsActive;
        public static bool ViewComplexCollisionBoxesIsActive;
        public static bool UnlockFPSIsActive;
        
        public static Bitmap CheatsBitmap = new Bitmap("graphics/cheats.png");

        private static int IgnoringInputFramesElapsed = 0;        //so that "one" button press doesnt register as multiple
        
        public static void UpdateAndPerformCheats()
        {
            if (GameForm.OneIsPressed)
                AllCheatsAreUnlocked = true;

            if (IgnoringInputFramesElapsed == 0)
            {
                if (GameForm.XIsPressed && AllCheatsAreUnlocked)
                    DroppingRings.DropRings();

                if (GameForm.KIsPressed && AllCheatsAreUnlocked)
                {
                    var sonic = GameState.LinkToSonicObject;
                    if (!sonic.IsFacingLeft)
                        GameState.ObjectsToAdd.Add(new MechaSonicObject(sonic.X - 52, sonic.Y - 17));
                    else
                        GameState.ObjectsToAdd.Add(new MechaSonicObject(sonic.X + 42, sonic.Y - 17));
                    IgnoringInputFramesElapsed = 3;
                }

                if (GameForm.PIsPressed && AllCheatsAreUnlocked)
                {
                    GrayscaleCheatIsActive = !GrayscaleCheatIsActive;
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.CIsPressed)
                {
                    DynamicCameraIsActive = !DynamicCameraIsActive;
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.RIsPressed)
                {
                    GameForm.IsFadingToBlack = true;
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.MIsPressed && AllCheatsAreUnlocked)
                {
                    DisableRingsCollectingSoundIsActive = !DisableRingsCollectingSoundIsActive;
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.JIsPressed && AllCheatsAreUnlocked)
                {
                    SonicCDJumpSoundIsActive = !SonicCDJumpSoundIsActive;
                    if (SonicCDJumpSoundIsActive)
                        Sounds.JumpSoundCurrent = Sounds.JumpSonicCDSound;
                    else
                        Sounds.JumpSoundCurrent = Sounds.JumpSound;
                    
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.BIsPressed && AllCheatsAreUnlocked)
                {
                    DisableBackgroundIsActive = !DisableBackgroundIsActive;
                    if (DisableBackgroundIsActive)
                        Background.CurrentBackgroundBitmap = Background.EmptyBackgroundBitmap;
                    else
                        Background.CurrentBackgroundBitmap = Background.BackgroundBitmap;
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.GIsPressed && AllCheatsAreUnlocked)
                {
                    MoonGravityIsActive = !MoonGravityIsActive;
                    if (MoonGravityIsActive)
                        GameState.LinkToSonicObject.GravityCurrent = SonicObject.MoonGravity;
                    else
                        GameState.LinkToSonicObject.GravityCurrent = SonicObject.NormalGravity;
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.VIsPressed && AllCheatsAreUnlocked)
                {
                    ViewComplexCollisionBoxesIsActive = !ViewComplexCollisionBoxesIsActive;
                    IgnoringInputFramesElapsed = 20;
                }
                
                if (GameForm.UIsPressed && AllCheatsAreUnlocked)
                {
                    UnlockFPSIsActive = !UnlockFPSIsActive;
                    GameForm.fpsCappingTimer.Interval = UnlockFPSIsActive ? 1000/33 : 1;
                    IgnoringInputFramesElapsed = 20;
                }
            }
            else
                --IgnoringInputFramesElapsed;

            
            //performing cheats
            if (GrayscaleCheatIsActive)
                ConvertCurrentBitmapsToGrayscale();
            if (DynamicCameraIsActive)
                UpdateDynamicCamera();
        }

        private static void ConvertCurrentBitmapsToGrayscale()
        {
            foreach (var gameObject in GameState.ObjectList)
            {
                if (gameObject is InvisibleDamagingObject)
                    continue;
                if (gameObject is TileObject)        //fps drops too badly
                    continue;
                if (Math.Abs(gameObject.X - GameState.CameraX) > 500 || Math.Abs(gameObject.Y - GameState.CameraY) > 300)
                    continue;

                var bitmap = new Bitmap(gameObject.CurrentBitmap);     //hard copy

                for (var x = 0; x < bitmap.Width; ++x)
                    for (var y = 0; y < bitmap.Height; ++y)
                    {
                        var currentPixel = bitmap.GetPixel(x, y);
    
                        var gray = (int) (currentPixel.R * 0.299 + currentPixel.G * 0.587 + currentPixel.B * 0.114);
    
                        //var newRed = Convert.ToInt32(currentPixel.R * 0.299);
                        //var newGreen = Convert.ToInt32(currentPixel.G * 0.587);
                        //var newBlue = Convert.ToInt32(currentPixel.B * 0.114);
    
                        var newColor = Color.FromArgb(currentPixel.A, gray, gray, gray);
    
                        bitmap.SetPixel(x, y, newColor);
                    }

                gameObject.CurrentBitmap = bitmap;
            }
        }

        private static void UpdateDynamicCamera()
        {
            var CameraXupd = GameState.LinkToSonicObject.X - GameForm.WindowWidth / 2 + (int)(GameState.LinkToSonicObject.XSpeed * 3);
            var CameraYupd = GameState.LinkToSonicObject.Y - GameForm.WindowHeight / 2 + (int)(GameState.LinkToSonicObject.YSpeed * 3);
            var d1 = GameState.CameraX - CameraXupd;
            var d2 = GameState.CameraY - CameraYupd;
            if (Math.Abs(d1) > 100)
                GameState.CameraX -= d1 / 3;
            else
                GameState.CameraX = CameraXupd;
            if (Math.Abs(d2) > 10)
                GameState.CameraY -= d2 / 3;
            else
                GameState.CameraY = CameraYupd;
        }
    }
}