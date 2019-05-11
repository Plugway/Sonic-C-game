using System.Drawing;
using System.Collections.Generic;

namespace sonic_c_sharp
{
    public class ClosingDoorObject : TileObject
    {
        public ClosingDoorObject(int x, int y, bool shouldStartBoss) : base(x, y, true, new Bitmap("graphics/door.png"), new List<Point[]> {new [] {new Point(0, 0), new Point(39, 93)} })
        {
            this.X = x;
            this.Y = y;
            this.YAtWhichToStop = y - 93;
            this.IsCollidable = true;
            this.CurrentBitmap = new Bitmap("graphics/door.png");
            this.AABBs = new List<Point[]> {new [] {new Point(0, 0), new Point(39, 93)} };
            this.BigAABB = new [] { new Point(0, 0), new Point(CurrentBitmap.Width, CurrentBitmap.Height) };    //for broad-phase collision detections
            this.shouldStartBoss = shouldStartBoss;    //FALSE - SHOULD STOP SCRAP BRAIN MUSIC, TRUE - SHOULD START FINAL BOSS MUSIC
        }

        public List<Point[]> AABBs;
        public Point[] BigAABB;
        public readonly int YAtWhichToStop;
        private bool shouldStopMovement;
        private readonly bool shouldStartBoss;
        
        public void Move()
        {
            if (!shouldStopMovement)
            {
                if (GameState.LinkToSonicObject.X > X + 40)
                {
                    if (Y > YAtWhichToStop)
                        Y -= 3;
                    else
                    {
                        if (!shouldStartBoss)
                            Music.ScrapBrainMusic.Stop();
                        else
                        {
                            Music.ScrapBrainMusic.Stop();
                            Music.FinalBossMusic.PlayLooping();
                            GameState.BossStarted = true;
                        }

                        Sounds.DoorClosingSound.Play();
                        shouldStopMovement = true;
                    }
                }
            }
        }
    }
}