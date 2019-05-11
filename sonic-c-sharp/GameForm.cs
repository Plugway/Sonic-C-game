using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace sonic_c_sharp
{   
    public class GameForm : Form
    {
        public static int WindowWidth = 445; //445;        //320        800
        public static int WindowHeight = 256; //256;        //180        600
        
        public static bool LeftIsPressed;
        public static bool RightIsPressed;
        public static bool DownIsPressed;
        public static bool UpIsPressed;
        public static bool ZIsPressed;
        public static bool EnterIsPressed;
        public static bool OneIsPressed;
        
        //unlocked cheats keys:
        public static bool RIsPressed;
        public static bool WIsPressed;
        public static bool AIsPressed;
        public static bool SIsPressed;
        public static bool DIsPressed;
        public static bool ZeroIsPressed;
        public static bool CIsPressed;
        
        //unlockable cheats keys:
        public static bool PIsPressed;
        public static bool JIsPressed;
        public static bool XIsPressed;
        public static bool MIsPressed;
        public static bool BIsPressed;
        public static bool GIsPressed;
        public static bool VIsPressed;
        public static bool KIsPressed;
        public static bool UIsPressed;
        
        private Stopwatch fpsMonitoringStopwatch;
        public static Timer fpsCappingTimer;
        
        public GameForm()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(WindowWidth, WindowHeight);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Sonic C#";
            
            
            //timer for capping fps
            fpsCappingTimer = new Timer();
            fpsCappingTimer.Interval = 1000/33;
            fpsCappingTimer.Tick += fpsCappingTimer_Tick;
            fpsCappingTimer.Start();
            
            //timer for monitoring fps
            fpsMonitoringStopwatch = Stopwatch.StartNew();
            
            
            //for input handling
            this.KeyPreview = true;
        }

        private int animationFrameNumber = 0;
        private int framesElapsed = 0;
        private void fpsCappingTimer_Tick(object sender, EventArgs e)
        {
            GameState.PerformGameStep();
            Invalidate();
        }
        
        
        //handling input
        protected override void OnKeyDown(KeyEventArgs e)
        {
            HandleKey(e.KeyCode, true);
        }

        public static bool LockKeysVariables = false;
        
        private void HandleKey(Keys e, bool down)
        {
            if (LockKeysVariables)
                return;
            if (IsFadingToTransperent && (!IsFadingToTransperent || currentFadingToTransperentFrame <= 5))
                return;
            
            switch (e)
            {
                case Keys.Left:
                    LeftIsPressed = down;
                    break;
                case Keys.Right:
                    RightIsPressed = down;
                    break;
                case Keys.Down:
                    DownIsPressed = down;
                    break;
                case Keys.Up:
                    UpIsPressed = down;
                    break;
                case Keys.Z:
                    ZIsPressed = down;
                    break;
                case Keys.X:
                    XIsPressed = down;
                    break;
                case Keys.Enter:
                    EnterIsPressed = down;
                    break;
                case Keys.R:
                    RIsPressed = down;
                    break;
                case Keys.W:
                    WIsPressed = down;
                    break;
                case Keys.A:
                    AIsPressed = down;
                    break;
                case Keys.S:
                    SIsPressed = down;
                    break;
                case Keys.D:
                    DIsPressed = down;
                    break;
                case Keys.D0:
                    ZeroIsPressed = down;
                    break;
                case Keys.P:
                    PIsPressed = down;
                    break;
                case Keys.C:
                    CIsPressed = down;
                    break;
                case Keys.M:
                    MIsPressed = down;
                    break;
                case Keys.J:
                    JIsPressed = down;
                    break;
                case Keys.B:
                    BIsPressed = down;
                    break;
                case Keys.G:
                    GIsPressed = down;
                    break;
                case Keys.V:
                    VIsPressed = down;
                    break;
                case Keys.K:
                    KIsPressed = down;
                    break;
                case Keys.D1:
                    OneIsPressed = down;
                    break;
                case Keys.U:
                    UIsPressed = down;
                    break;
            }
        }
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            HandleKey(e.KeyCode, false);
        }
        
        
        //drawing stuff
        private int timesOnPaintWasCalled;
        private int prevTimes;
        
        public static bool IsFadingToTransperent = true;    //true
        public static bool IsFadingToBlack = false;
        private static int currentFadingToTransperentFrame = 0;
        private static int currentFadingToBlackFrame = 0;
        private static int fadingToTransperentFramesElapsed = 0;
        private static int fadingToBlackFramesElapsed = 0;
        private static int waitingOnFirstFrameWhilrFadingToTransperentFramesElapsed = 0;
        private static int waitingBeforeFadingToBlackFramesElapsed = 0;
        private static int holdingOnBlackFrameFramesElapsed = 0;
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            //graphics.FillRectangle(Brushes.Navy, ClientRectangle);
            
            var cameraOffsetX = -GameState.CameraX - 21;
            var cameraOffsetY = -GameState.CameraY - 24;
            
            graphics.DrawImage(Background.CurrentBackgroundBitmap, Background.PreviousX + cameraOffsetX, 0 + cameraOffsetY);
            graphics.DrawImage(Background.CurrentBackgroundBitmap, Background.CurrentX + cameraOffsetX, 0 + cameraOffsetY);
            graphics.DrawImage(Background.CurrentBackgroundBitmap, Background.NextX + cameraOffsetX, 0 + cameraOffsetY);
            
            foreach (var gameObject in GameState.ObjectList)
            {
                //if (Math.Abs(gameObject.X - GameState.CameraX) > 500 && Math.Abs(gameObject.Y - GameState.CameraY) > 500)    //doesnt really improve fps at all it seems
                //    continue;
                
                if (gameObject is SonicObject)
                {
                    var Sonic = (SonicObject) gameObject;
                    if (!Sonic.IsBlinking)
                        graphics.DrawImage(gameObject.CurrentBitmap, gameObject.X - GameState.CameraX - 21, gameObject.Y - GameState.CameraY - 24);
                        //graphics.DrawImage(gameObject.CurrentBitmap, (gameObject.X - GameState.CameraX - 21)*2, (gameObject.Y - GameState.CameraY - 24)*2);    for 2X
                }
                else if (!(gameObject is InvisibleDamagingObject))    //comment when testing
                    graphics.DrawImage(gameObject.CurrentBitmap, gameObject.X - GameState.CameraX - 21, gameObject.Y - GameState.CameraY - 24);
                //    graphics.DrawImage(gameObject.CurrentBitmap, (gameObject.X - GameState.CameraX - 21)*2, (gameObject.Y - GameState.CameraY - 24)*2);    for 2X
                
            }
            
            graphics.DrawString("fps " + prevTimes, new Font("Times New Roman", 12), Brushes.Yellow, new Point(380, 0));

            if (Cheats.ViewComplexCollisionBoxesIsActive)
            {
                DrawSonicCollisionBoxes(graphics, GameState.LinkToSonicObject.CurrentCollisionBoxesSet);

                foreach (var motobug in GameState.MotobugsList)
                    DrawMotobugCollisionBoxes(graphics, motobug);
            }
            
            if (Cheats.AllCheatsAreUnlocked)
                graphics.DrawImage(Cheats.CheatsBitmap, 0, 0);

            DrawHUD(graphics);

            if (GameState.LevelIsComplete)
                graphics.DrawImage(LevelCompleteOverlay.bitmap, LevelCompleteOverlay.X, LevelCompleteOverlay.Y);
            
            DrawFadingToTransperent(graphics);
            DrawFadingToBlack(graphics);
            
            if (fpsMonitoringStopwatch.ElapsedMilliseconds > 1000)
            {
                //Console.WriteLine("Elapsed.Milliseconds " + fpsMonitoringStopwatch.ElapsedMilliseconds + ", " + "times " + timesOnPaintWasCalled);
                prevTimes = timesOnPaintWasCalled;
                timesOnPaintWasCalled = 0;
                fpsMonitoringStopwatch.Restart();
            }
            ++timesOnPaintWasCalled;
        }

        private void DrawHUD(Graphics graphics)
        {
            graphics.DrawImage(HUD.CurrentRingsBitmap, 20, 10);

            var ringsAsString = GameState.LinkToSonicObject.Rings.ToString();
            var currentX = 70;
            foreach (var number in ringsAsString)
            {
                graphics.DrawImage(HUD.NumbersBitmaps[number - '0'], currentX, 10);
                currentX += 9;
            }
        }

        private void DrawFadingToTransperent(Graphics graphics)
        {
            if (!IsFadingToTransperent)
                return;
            
            if (currentFadingToTransperentFrame == 0 && waitingOnFirstFrameWhilrFadingToTransperentFramesElapsed < 64)
            {
                ++waitingOnFirstFrameWhilrFadingToTransperentFramesElapsed;
                graphics.DrawImage(LevelCard.LevelCardBitmaps[currentFadingToTransperentFrame], LevelCard.X, LevelCard.Y);
            }
            else if (currentFadingToTransperentFrame == 20)
            {
                currentFadingToTransperentFrame = 0;
                fadingToTransperentFramesElapsed = 0;
                waitingOnFirstFrameWhilrFadingToTransperentFramesElapsed = 0;
                IsFadingToTransperent = false;
            }
            else
            {
                graphics.DrawImage(LevelCard.LevelCardBitmaps[currentFadingToTransperentFrame], LevelCard.X, LevelCard.Y);
                    
                ++fadingToTransperentFramesElapsed;
                if (fadingToTransperentFramesElapsed > 0)
                {
                    ++currentFadingToTransperentFrame;
                    fadingToTransperentFramesElapsed = 0;
                }
            }
        }

        private void DrawFadingToBlack(Graphics graphics)
        {
            if (!IsFadingToBlack)
                return;
            
            if (waitingBeforeFadingToBlackFramesElapsed < 20)
            {
                ++waitingBeforeFadingToBlackFramesElapsed;
            }
            else if (currentFadingToBlackFrame == 19 && holdingOnBlackFrameFramesElapsed < 15)
            {
                graphics.DrawImage(FadeToBlack.FadeToBlackBitmaps[currentFadingToBlackFrame], FadeToBlack.X, FadeToBlack.Y);
                ++holdingOnBlackFrameFramesElapsed;
            }
            else if (currentFadingToBlackFrame == 20)
            {
                graphics.DrawImage(FadeToBlack.FadeToBlackBitmaps[currentFadingToBlackFrame - 1], FadeToBlack.X, FadeToBlack.Y);
                    
                currentFadingToBlackFrame = 0;
                fadingToBlackFramesElapsed = 0;
                waitingBeforeFadingToBlackFramesElapsed = 0;
                holdingOnBlackFrameFramesElapsed = 0;
                IsFadingToBlack = false;
                    
                Reloading.ReloadLevel();
            }
            else
            {
                graphics.DrawImage(FadeToBlack.FadeToBlackBitmaps[currentFadingToBlackFrame], FadeToBlack.X, FadeToBlack.Y);
                    
                ++fadingToBlackFramesElapsed;
                if (fadingToBlackFramesElapsed > 0)
                {
                    ++currentFadingToBlackFrame;
                    fadingToBlackFramesElapsed = 0;
                }
            }
        }

        private void DrawSonicCollisionBoxes(Graphics graphics, SonicCollisionBoxes CurrentBoxes)
        {
            //testing sonic's collision boxes' positions
            Brush brush;
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootLeft_InnerIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;

            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.FootLeft_Inner[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.FootLeft_Inner[0].Y,
                CurrentBoxes.FootLeft_Inner[1].X - CurrentBoxes.FootLeft_Inner[0].X + 1,
                CurrentBoxes.FootLeft_Inner[1].Y - CurrentBoxes.FootLeft_Inner[0].Y + 1);

            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootLeft_OuterIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.FootLeft_Outer[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.FootLeft_Outer[0].Y,
                CurrentBoxes.FootLeft_Outer[1].X - CurrentBoxes.FootLeft_Outer[0].X + 1,
                CurrentBoxes.FootLeft_Outer[1].Y - CurrentBoxes.FootLeft_Outer[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootRight_InnerIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.FootRight_Inner[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.FootRight_Inner[0].Y,
                CurrentBoxes.FootRight_Inner[1].X - CurrentBoxes.FootRight_Inner[0].X + 1,
                CurrentBoxes.FootRight_Inner[1].Y - CurrentBoxes.FootRight_Inner[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootRight_OuterIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.FootRight_Outer[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.FootRight_Outer[0].Y,
                CurrentBoxes.FootRight_Outer[1].X - CurrentBoxes.FootRight_Outer[0].X + 1,
                CurrentBoxes.FootRight_Outer[1].Y - CurrentBoxes.FootRight_Outer[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.HeadLeft_InnerIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.HeadLeft_Inner[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.HeadLeft_Inner[0].Y,
                CurrentBoxes.HeadLeft_Inner[1].X - CurrentBoxes.HeadLeft_Inner[0].X + 1,
                CurrentBoxes.HeadLeft_Inner[1].Y - CurrentBoxes.HeadLeft_Inner[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.HeadRight_InnerIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.HeadRight_Inner[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.HeadRight_Inner[0].Y,
                CurrentBoxes.HeadRight_Inner[1].X - CurrentBoxes.HeadRight_Inner[0].X + 1,
                CurrentBoxes.HeadRight_Inner[1].Y - CurrentBoxes.HeadRight_Inner[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootLeftBalancing_OuterIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.FootLeftBalancing_Outer[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.FootLeftBalancing_Outer[0].Y,
                CurrentBoxes.FootLeftBalancing_Outer[1].X - CurrentBoxes.FootLeftBalancing_Outer[0].X + 1,
                CurrentBoxes.FootLeftBalancing_Outer[1].Y - CurrentBoxes.FootLeftBalancing_Outer[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootRightBalancing_OuterIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.FootRightBalancing_Outer[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.FootRightBalancing_Outer[0].Y,
                CurrentBoxes.FootRightBalancing_Outer[1].X - CurrentBoxes.FootRightBalancing_Outer[0].X + 1,
                CurrentBoxes.FootRightBalancing_Outer[1].Y - CurrentBoxes.FootRightBalancing_Outer[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallBottomLeftIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.WallBottomLeft[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.WallBottomLeft[0].Y,
                CurrentBoxes.WallBottomLeft[1].X - CurrentBoxes.WallBottomLeft[0].X + 1,
                CurrentBoxes.WallBottomLeft[1].Y - CurrentBoxes.WallBottomLeft[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallBottomRightIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.WallBottomRight[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.WallBottomRight[0].Y,
                CurrentBoxes.WallBottomRight[1].X - CurrentBoxes.WallBottomRight[0].X + 1,
                CurrentBoxes.WallBottomRight[1].Y - CurrentBoxes.WallBottomRight[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallLeft_InnerIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.WallLeft_Inner[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.WallLeft_Inner[0].Y,
                CurrentBoxes.WallLeft_Inner[1].X - CurrentBoxes.WallLeft_Inner[0].X + 1,
                CurrentBoxes.WallLeft_Inner[1].Y - CurrentBoxes.WallLeft_Inner[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallRight_InnerIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.WallRight_Inner[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.WallRight_Inner[0].Y,
                CurrentBoxes.WallRight_Inner[1].X - CurrentBoxes.WallRight_Inner[0].X + 1,
                CurrentBoxes.WallRight_Inner[1].Y - CurrentBoxes.WallRight_Inner[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallTopLeftIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.WallTopLeft[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.WallTopLeft[0].Y,
                CurrentBoxes.WallTopLeft[1].X - CurrentBoxes.WallTopLeft[0].X + 1,
                CurrentBoxes.WallTopLeft[1].Y - CurrentBoxes.WallTopLeft[0].Y + 1);
            
            if (GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallTopRightIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;
            
            graphics.FillRectangle(brush,
                GameState.LinkToSonicObject.X - GameState.CameraX - 21 + CurrentBoxes.WallTopRight[0].X,
                GameState.LinkToSonicObject.Y - GameState.CameraY - 24 + CurrentBoxes.WallTopRight[0].Y,
                CurrentBoxes.WallTopRight[1].X - CurrentBoxes.WallTopRight[0].X + 1,
                CurrentBoxes.WallTopRight[1].Y - CurrentBoxes.WallTopRight[0].Y + 1);
        }

        private void DrawMotobugCollisionBoxes(Graphics graphics, GameObject motobug)
        {
            var motobugAsMotobug = (BadnikMotobugObject) motobug;
            
            Brush brush;
            if (motobugAsMotobug.GroundLeftIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;

            graphics.FillRectangle(brush,
                motobugAsMotobug.X - GameState.CameraX - 21 + motobugAsMotobug.GroundLeftCollisionBox[0].X,
                motobugAsMotobug.Y - GameState.CameraY - 24 + motobugAsMotobug.GroundLeftCollisionBox[0].Y,
                motobugAsMotobug.GroundLeftCollisionBox[1].X - motobugAsMotobug.GroundLeftCollisionBox[0].X + 1,
                motobugAsMotobug.GroundLeftCollisionBox[1].Y - motobugAsMotobug.GroundLeftCollisionBox[0].Y + 1);

            
            if (motobugAsMotobug.GroundRightIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;

            graphics.FillRectangle(brush,
                motobugAsMotobug.X - GameState.CameraX - 21 + motobugAsMotobug.GroundRightCollisionBox[0].X,
                motobugAsMotobug.Y - GameState.CameraY - 24 + motobugAsMotobug.GroundRightCollisionBox[0].Y,
                motobugAsMotobug.GroundRightCollisionBox[1].X - motobugAsMotobug.GroundRightCollisionBox[0].X + 1,
                motobugAsMotobug.GroundRightCollisionBox[1].Y - motobugAsMotobug.GroundRightCollisionBox[0].Y + 1);
            
            
            if (motobugAsMotobug.WallLeftIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;

            graphics.FillRectangle(brush,
                motobugAsMotobug.X - GameState.CameraX - 21 + motobugAsMotobug.WallLeftCollisionBox[0].X,
                motobugAsMotobug.Y - GameState.CameraY - 24 + motobugAsMotobug.WallLeftCollisionBox[0].Y,
                motobugAsMotobug.WallLeftCollisionBox[1].X - motobugAsMotobug.WallLeftCollisionBox[0].X + 1,
                motobugAsMotobug.WallLeftCollisionBox[1].Y - motobugAsMotobug.WallLeftCollisionBox[0].Y + 1);
            
            
            if (motobugAsMotobug.WallRightIsActive)
                brush = Brushes.Red;
            else
                brush = Brushes.Green;

            graphics.FillRectangle(brush,
                motobugAsMotobug.X - GameState.CameraX - 21 + motobugAsMotobug.WallRightCollisionBox[0].X,
                motobugAsMotobug.Y - GameState.CameraY - 24 + motobugAsMotobug.WallRightCollisionBox[0].Y,
                motobugAsMotobug.WallRightCollisionBox[1].X - motobugAsMotobug.WallRightCollisionBox[0].X + 1,
                motobugAsMotobug.WallRightCollisionBox[1].Y - motobugAsMotobug.WallRightCollisionBox[0].Y + 1);
        }
    }
}