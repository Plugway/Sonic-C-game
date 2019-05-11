using System.Drawing;

namespace sonic_c_sharp
{
    public class ConveyorBeltObject : GameObject
    {
        public ConveyorBeltObject(int x, int y, bool shouldIncreaseXSpeed)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.shouldIncreaseXSpeed = shouldIncreaseXSpeed;
            
            if (shouldIncreaseXSpeed)
                this.bitmaps = new []
                {
                    new Bitmap("graphics/conveyorBelt1.png"),
                    new Bitmap("graphics/conveyorBelt2.png"),
                    new Bitmap("graphics/conveyorBelt3.png")
                };
            else
                this.bitmaps = new []
                {
                    new Bitmap("graphics/conveyorBelt3.png"),
                    new Bitmap("graphics/conveyorBelt2.png"),
                    new Bitmap("graphics/conveyorBelt1.png")
                };
            
            this.CurrentBitmap = bitmaps[0];
        }

        public Point[] AABB = { new Point(0, 0),
                                new Point(199, 3) };

        private readonly bool shouldIncreaseXSpeed;
        public bool IsCollidingWithSonic;

        private readonly Bitmap[] bitmaps;
        
        public void Move()
        {
            if (shouldIncreaseXSpeed && IsCollidingWithSonic)
            {
                GameState.LinkToSonicObject.X += 3;
            }
            else if (!shouldIncreaseXSpeed && IsCollidingWithSonic)
            {
                GameState.LinkToSonicObject.X -= 3;
            }

            IsCollidingWithSonic = false;

            PerformAnimation();
        }

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformAnimation()
        {
            if (framesElapsed > 0)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 2)
                    currentAnimationFrame = 0;
            }
            ++framesElapsed;

            CurrentBitmap = bitmaps[currentAnimationFrame];
        }
    }
}