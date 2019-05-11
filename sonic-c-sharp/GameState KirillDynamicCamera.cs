using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace sonic_c_sharp
{
    public class GameState
    {   
        public static List<GameObject> ObjectList = new List<GameObject>();
        public static List<GameObject> ObjectsToRemove = new List<GameObject>();
        public static List<GameObject> ObjectsToAdd = new List<GameObject>();
        
        public static List<GameObject> MotobugsList = new List<GameObject>();
        public static List<GameObject> MotobugsToRemove = new List<GameObject>();

        public static int CameraX;
        public static int CameraY;

        public static SonicObject LinkToSonicObject;

        private static int framesBeforePlayingMusicElasped = 0;

        public static bool BossStarted = false;
        public static bool LevelIsComplete = false;

        public static void PerformGameStep()                //GAMELOOP STUFF
        {
            ObjectsToRemove = new List<GameObject>();
            ObjectsToAdd = new List<GameObject>();

            if (!Music.IsPlayingScrapBrainMusic && framesBeforePlayingMusicElasped > 6)    //otherwise soundplayer gets fucked up
            {
                Music.ScrapBrainMusic.PlayLooping();
                Music.IsPlayingScrapBrainMusic = true;
                framesBeforePlayingMusicElasped = 0;
            }
            else if (!Music.IsPlayingScrapBrainMusic && framesBeforePlayingMusicElasped <= 6)
                ++framesBeforePlayingMusicElasped;

            MoveObjects();

            CollisionDetection.PerformCollisionDetection();
            RemoveUnneededObjects();

            if (GameForm.XIsPressed)
                DroppingRings.DropRings();
            if (GameForm.RIsPressed)
                GameForm.IsFadingToBlack = true;
            
            AddNeededObjects();

            //if (GameState.LinkToSonicObject.Y > 600)    for bg test; can remove i think
            //    GameState.LinkToSonicObject.Y = 200;
            UpdateCameraPosition();
            Background.UpdateBackgroundsPositions();
            HUD.PerformAnimation();

            if (GameState.LevelIsComplete && GameForm.EnterIsPressed)
                GameForm.IsFadingToBlack = true;
        }
        
        public static void FindSonicObject()
        {
            foreach (var gameObject in GameState.ObjectList)
            {
                if (gameObject is SonicObject)
                    LinkToSonicObject = (SonicObject)gameObject;
            }
        }
        
        public static void UpdateCameraPosition()
        {
            /*
            CameraX = LinkToSonicObject.X - GameForm.WindowWidth/2;
            CameraY = LinkToSonicObject.Y - GameForm.WindowHeight/2;
            */
            
             var CameraXupd = LinkToSonicObject.X - GameForm.WindowWidth / 2 + (int)(LinkToSonicObject.XSpeed * 3);
            var CameraYupd = LinkToSonicObject.Y - GameForm.WindowHeight / 2 + (int)(LinkToSonicObject.YSpeed * 3);
            var d1 = CameraX - CameraXupd;
            var d2 = CameraY - CameraYupd;
            if (Math.Abs(d1) > 10)
                CameraX -= d1 / 3;
            else
                CameraX = CameraXupd;
            if (Math.Abs(d2) > 10)
                CameraY -= d2 / 3;
            else
                CameraY = CameraYupd;

            /*
             FOR 2X:
             
            CameraX = LinkToSonicObject.X - GameForm.WindowWidth/2/2;
            CameraY = LinkToSonicObject.Y - GameForm.WindowHeight/2/2;
             */
        }

        public static void MoveObjects()
        {
            foreach (var gameObject in GameState.ObjectList)
            {
                if (gameObject is SonicObject sonic)        //same as: if (gameObject is SonicObject) { var Sonic = (SonicObject) gameObject; sonic.Move(); }
                    sonic.Move();
                else if (gameObject is BadnikSimpleObject badnik)
                    badnik.Move();
                else if (gameObject is RingObject && !(gameObject is BlueRingObject))
                {
                    var ring = (RingObject) gameObject;        //maybe add Move() as a method to GameObject.cs to simplify this by a lot
                    ring.Move();                            //it didnt. i cant seem to cast objects to their actual type without typing it out explicitly
                }                                            //maybe if i use override Move() would work fine without casting? idk.
                else if (gameObject is RingSparklesObject ringSparkles)
                    ringSparkles.Move();
                else if (gameObject is ExplosionObject explosion)
                    explosion.Move();
                else if (gameObject is RingDroppedObject ringDropped)
                    ringDropped.Move();
                else if (gameObject is LavaObject lava)
                    lava.Move();
                else if (gameObject is LavaTopObject lavaTop)
                    lavaTop.Move();
                else if (gameObject is SpikeBallSmallObject spikeBallSmall)
                    spikeBallSmall.Move();
                else if (gameObject is SpikeBallBigObject spikeBallBig)
                    spikeBallBig.Move();
                else if (gameObject is YellowSpringObject yellowSpring)
                    yellowSpring.Move();
                else if (gameObject is BadnikMotobugObject badnikMotobug)
                    badnikMotobug.Move();
                else if (gameObject is BadnikFishObject badnikFish)
                    badnikFish.Move();
                else if (gameObject is ConveyorBeltObject conveyorBelt)
                    conveyorBelt.Move();
                else if (gameObject is BlueRingObject blueRingObject)
                    blueRingObject.Move();
                else if (gameObject is BlueRingSparklesObject blueRingSparkles)
                    blueRingSparkles.Move();
                else if (gameObject is ClosingDoorObject closingDoor)
                    closingDoor.Move();
                else if (gameObject is MechaSonicObject mechaSonic)
                    mechaSonic.Move();
                else if (gameObject is EnergyBallObject energyBall)
                    energyBall.Move();
                else if (gameObject is ShuttersObject shutters)
                    shutters.Move();
                else if (gameObject is RedExplosionObject redExplosionObject)
                    redExplosionObject.Move();
                else if (gameObject is SmokeObject smokeObject)
                    smokeObject.Move();
            }
        }

        public static void RemoveUnneededObjects()
        {
            foreach (var gameObject in ObjectsToRemove)
                ObjectList.Remove(gameObject);
            foreach (var gameObject in MotobugsToRemove)
                MotobugsList.Remove(gameObject);
        }
        
        public static void AddNeededObjects()
        {
            foreach (var gameObject in ObjectsToAdd)
                ObjectList.Add(gameObject);
        }
    }
}