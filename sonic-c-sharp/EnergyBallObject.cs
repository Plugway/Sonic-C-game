using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public class EnergyBallObject : GameObject
    {
        public EnergyBallObject(int x, int y, bool mechaSonicIsFacingLeft)
        {
            this.X = x;
            this.Y = y;

            double temp1 = GameState.LinkToSonicObject.Y + 10 - y;        //Sonic.Y + 10 because otherwise can just duck to avoid all projectiles
            double temp2 = GameState.LinkToSonicObject.X - x;
            if (!mechaSonicIsFacingLeft)
                temp1 *= -1;                            //hello, i am kostil, i make sure that projectiles are fired correctly in reverse
            double angle = Math.Atan(temp1 / temp2);
            
            var xSpeedTemp = (int)(Math.Cos(angle)*10);
            this.xSpeed = GameState.LinkToSonicObject.X > X ? xSpeedTemp : -xSpeedTemp;
            this.ySpeed = (int)(-Math.Sin(angle)*10);
            this.IsCollidable = true;
            this.CurrentBitmap = this.bitmaps[1];
        }

        public Point[] AABB = { new Point(9, 7),
                                new Point(32, 30) };

        private readonly Bitmap[] bitmaps = 
        {
            new Bitmap("graphics/energyBall4.png"),
            new Bitmap("graphics/energyBall2.png"),
            new Bitmap("graphics/energyBall3.png"),
            new Bitmap("graphics/energyBall4.png"),
            new Bitmap("graphics/energyBall5.png")
        };
        
        private Random random = new Random();
        private int totalFramesPassed = 0;

        private int xSpeed;
        private int ySpeed;

        public void Move()
        {
            ++totalFramesPassed;   
            if (totalFramesPassed > 600)
                GameState.ObjectsToRemove.Add(this);
            
            PerformAnimation();

            X += xSpeed;
            Y += ySpeed;
        }

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformAnimation()
        {
            if (framesElapsed > 1)
            {
                framesElapsed = 0;
                currentAnimationFrame = random.Next(0, 5);
            }

            CurrentBitmap = bitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}