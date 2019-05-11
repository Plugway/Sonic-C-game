using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public class SpikeBallSmallObject : GameObject
    {
        public SpikeBallSmallObject(int x, int y, bool shouldMoveHorizontally)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = new Bitmap("graphics/spikeBallSmall.png");
            this.xSpeed = 0;
            this.ySpeed = 0;
            this.shouldMoveHorizontally = !shouldMoveHorizontally;
        }

        public readonly Point[] AABB = { new Point(5, 5),
                                         new Point(26, 26) };

        private float xSpeed;
        private float ySpeed;
        private readonly bool shouldMoveHorizontally = false;
        private bool isMovingDown = false;
        private bool isMovingLeft = false;

        public void Move()
        {
            if (shouldMoveHorizontally)
            {
                if (isMovingLeft)
                    xSpeed -= 0.2f;
                else
                    xSpeed += 0.2f;

                if (isMovingLeft && xSpeed < -4.5)
                    isMovingLeft = false;
                else if (!isMovingLeft && xSpeed > 4.5)
                    isMovingLeft = true;

                X += (int) xSpeed;
            }
            else
            {
                if (isMovingDown)
                    ySpeed += 0.2f;
                else
                    ySpeed -= 0.2f;

                if (isMovingDown && ySpeed > 4.5)
                    isMovingDown = false;
                else if (!isMovingDown && ySpeed < -4.5)
                    isMovingDown = true;

                Y += (int) ySpeed;
            }
        }
    }
}