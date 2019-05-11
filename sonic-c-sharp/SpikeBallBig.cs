using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public class SpikeBallBigObject : GameObject
    {
        public SpikeBallBigObject(int x, int y, bool shouldMoveHorizontally)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = new Bitmap("graphics/spikeBallBig.png");
            this.xSpeed = 0;
            this.ySpeed = 0;
            this.shouldMoveHorizontally = shouldMoveHorizontally;
        }

        public readonly Point[] AABB = { new Point(8, 8),
                                         new Point(39, 39) };

        private float xSpeed;
        private float ySpeed;
        private readonly bool shouldMoveHorizontally;
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