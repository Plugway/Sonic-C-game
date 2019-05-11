using System.Drawing;

namespace sonic_c_sharp
{
    public class LavaObject : GameObject
    {
        public LavaObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = this.flowingBitmaps[0];
        }

        public readonly Point[] AABB = { new Point(0, 0),
                                         new Point(31, 31) };

        private readonly Bitmap[] flowingBitmaps = 
        {
            new Bitmap("graphics/lava1.png"),
            new Bitmap("graphics/lava2.png"),
            new Bitmap("graphics/lava3.png")
        };

        public void Move()
        {
            PerformFlowingAnimation();
        }

        private int framesElapsed = 0;
        private int currentAnimationFrame = 0;
        private void PerformFlowingAnimation()
        {
            if (framesElapsed > 3)
            {
                framesElapsed = 0;
                ++currentAnimationFrame;
                if (currentAnimationFrame > 2)
                    currentAnimationFrame = 0;
            }

            CurrentBitmap = flowingBitmaps[currentAnimationFrame];

            ++framesElapsed;
        }
    }
}