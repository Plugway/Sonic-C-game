using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace sonic_c_sharp
{
    public static class GameState
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

            Cheats.UpdateAndPerformCheats();
            
            AddNeededObjects();
            
            if (!Cheats.DynamicCameraIsActive)
                UpdateCameraPosition();
            
            Background.UpdateBackgroundsPositions();
            HUD.PerformAnimation();

            if (LevelIsComplete && GameForm.EnterIsPressed)
                GameForm.IsFadingToBlack = true;
            
            if (Tests.IsTestingApplication)
            {
                ++Tests.FramesElasped;
                if (Tests.FramesElasped > 60)
                    Application.Exit();
            }
        }

        private static void UpdateCameraPosition()
        {
            CameraX = LinkToSonicObject.X - GameForm.WindowWidth/2;
            CameraY = LinkToSonicObject.Y - GameForm.WindowHeight/2;
            
            /*
             FOR 2X:
             
            CameraX = LinkToSonicObject.X - GameForm.WindowWidth/2/2;
            CameraY = LinkToSonicObject.Y - GameForm.WindowHeight/2/2;
             */
        }

        private static void MoveObjects()
        {
            foreach (var gameObject in ObjectList)
            {
                //if (Math.Abs(gameObject.X - CameraX) > 1000 && Math.Abs(gameObject.Y - CameraY) > 1000)    //doesnt really improve fps at all it seems
                //    continue;

                switch (gameObject)
                {
                    case SonicObject sonic:
                        sonic.Move();
                        break;
                    case BadnikSimpleObject badnik:
                        badnik.Move();
                        break;
                    case RingObject _ when !(gameObject is BlueRingObject):        //huh
                    {
                        var ring = (RingObject) gameObject;        //maybe add Move() as a method to GameObject.cs to simplify this by a lot
                        ring.Move();                            //it didnt. i cant seem to cast objects to their actual type without typing it out explicitly
                                                                //maybe if i use override Move() would work fine without casting? idk.
                        break;
                    }
                    case RingSparklesObject ringSparkles:
                        ringSparkles.Move();
                        break;
                    case ExplosionObject explosion:
                        explosion.Move();
                        break;
                    case RingDroppedObject ringDropped:
                        ringDropped.Move();
                        break;
                    case LavaObject lava:
                        lava.Move();
                        break;
                    case LavaTopObject lavaTop:
                        lavaTop.Move();
                        break;
                    case SpikeBallSmallObject spikeBallSmall:
                        spikeBallSmall.Move();
                        break;
                    case SpikeBallBigObject spikeBallBig:
                        spikeBallBig.Move();
                        break;
                    case YellowSpringObject yellowSpring:
                        yellowSpring.Move();
                        break;
                    case BadnikMotobugObject badnikMotobug:
                        badnikMotobug.Move();
                        break;
                    case BadnikFishObject badnikFish:
                        badnikFish.Move();
                        break;
                    case ConveyorBeltObject conveyorBelt:
                        conveyorBelt.Move();
                        break;
                    case BlueRingObject blueRingObject:
                        blueRingObject.Move();
                        break;
                    case BlueRingSparklesObject blueRingSparkles:
                        blueRingSparkles.Move();
                        break;
                    case ClosingDoorObject closingDoor:
                        closingDoor.Move();
                        break;
                    case MechaSonicObject mechaSonic:
                        mechaSonic.Move();
                        break;
                    case EnergyBallObject energyBall:
                        energyBall.Move();
                        break;
                    case ShuttersObject shutters:
                        shutters.Move();
                        break;
                    case RedExplosionObject redExplosionObject:
                        redExplosionObject.Move();
                        break;
                    case SmokeObject smokeObject:
                        smokeObject.Move();
                        break;
                }
            }
        }

        private static void RemoveUnneededObjects()
        {
            foreach (var gameObject in ObjectsToRemove)
                ObjectList.Remove(gameObject);
            foreach (var gameObject in MotobugsToRemove)
                MotobugsList.Remove(gameObject);
        }

        private static void AddNeededObjects()
        {
            foreach (var gameObject in ObjectsToAdd)
                ObjectList.Add(gameObject);
        }
    }
}