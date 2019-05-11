using System.Drawing;
using System.Collections.Generic;

namespace sonic_c_sharp
{
    public class BadnikSimpleObject : GameObject
    {
        public BadnikSimpleObject(int x, int y, List<Point[]> AABB)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = new Bitmap("graphics/badnikSimple.png");
            this.AABB = AABB;
        }

        
        public List<Point[]> AABB;    //only Point[0][..] is supposed to be used
        private bool shouldMove = false;

        public void Move()
        {
            if (X - GameState.LinkToSonicObject.X < 445)
                shouldMove = true;
                
            if (shouldMove)
                X -= 3;
        }
    }
}