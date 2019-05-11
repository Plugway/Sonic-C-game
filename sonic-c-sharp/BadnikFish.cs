using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public class BadnikMotobugObject : GameObject
    {
        public SpikeBallSmallObject(int x, int y, bool shouldMoveHorizontally)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = new Bitmap("spikeBallSmall.png");    //just for testing
            this.XSpeed = 0;
            this.YSpeed = 0;
            this.ShouldMoveHorizontally = !shouldMoveHorizontally;
        }

        public Point[] AABB = { new Point(5, 5),
                                new Point(26, 26) };

        public float XSpeed;
        public float YSpeed;
        public bool ShouldMoveHorizontally = false;
        public bool IsMovingDown = false;
        public bool IsMovingLeft = false;

        public void Move()
        {
            if (ShouldMoveHorizontally)
            {
                if (IsMovingLeft)
                    XSpeed -= 0.2f;
                else
                    XSpeed += 0.2f;

                if (IsMovingLeft && XSpeed < -4.5)
                    IsMovingLeft = false;
                else if (!IsMovingLeft && XSpeed > 4.5)
                    IsMovingLeft = true;

                X += (int) XSpeed;
            }
            else
            {
                if (IsMovingDown)
                    YSpeed += 0.2f;
                else
                    YSpeed -= 0.2f;

                if (IsMovingDown && YSpeed > 4.5)
                    IsMovingDown = false;
                else if (!IsMovingDown && YSpeed < -4.5)
                    IsMovingDown = true;

                Y += (int) YSpeed;
            }
        }
    }
}