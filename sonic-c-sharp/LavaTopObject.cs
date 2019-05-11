using System.Drawing;

namespace sonic_c_sharp
{
    public class LavaTopObject : GameObject
    {
        public LavaTopObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = false;
            this.CurrentBitmap = this.flowingBitmaps[0];
        }

        private readonly Bitmap[] flowingBitmaps = 
        {
            new Bitmap("graphics/lavaTop1.png"),
            new Bitmap("graphics/lavaTop2.png"),
            new Bitmap("graphics/lavaTop3.png")
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