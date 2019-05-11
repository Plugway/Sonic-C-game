using System.Drawing;

namespace sonic_c_sharp
{
    public class ShuttersObject : GameObject
    {    
        public ShuttersObject(int x, int y, bool shouldMoveRight)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = false;
            this.shouldMoveRight = shouldMoveRight;
            this.CurrentBitmap = new Bitmap("graphics/shutters.png");
        }

        private bool shouldMoveRight;

        private int pixelsMovedTotal = 0;
        private int framesElapsedAfterStartedMoving = 0;
        
        public void Move()
        {
            if (GameState.BossStarted)
            {
                if (shouldMoveRight && pixelsMovedTotal < 205)        //FIX
                {
                    if (pixelsMovedTotal > 155)
                    {
                        X += 1;
                        pixelsMovedTotal += 1;
                    }
                    else
                    {
                        X += 2;
                        pixelsMovedTotal += 2;
                    }
                }
                else if (!shouldMoveRight && pixelsMovedTotal < 205)    //FIX
                {
                    if (pixelsMovedTotal > 155)
                    {
                        X -= 1;
                        pixelsMovedTotal += 1;
                    }
                    else
                    {
                        X -= 2;
                        pixelsMovedTotal += 2;
                    }
                }

                //++framesElapsedAfterStartedMoving;
                //if (framesElapsedAfterStartedMoving > 400)
                //    GameState.ObjectsToRemove.Add(this);
            }
        }
    }
}