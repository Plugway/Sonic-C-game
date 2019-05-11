using System.Collections.Generic;

namespace sonic_c_sharp
{
    public static class ReloadLevel
    {
        public static void ReloadLevel()
        {
            GameState.ObjectList = new List<GameObject>();
            GameState.ObjectsToRemove = new List<GameObject>();
            GameState.ObjectsToAdd = new List<GameObject>();
            
            
        }


        
        
        
        public static int CameraX;
        public static int CameraY;

        public static SonicObject LinkToSonicObject;    //perhaps should be private
        public static void FindSonicObject()
        {
            foreach (var gameObject in GameState.ObjectList)        //костыыыыыль        maybe just make SonicObject static?
            {
                if (gameObject is SonicObject)
                    LinkToSonicObject = (SonicObject)gameObject;    //reaaally, just make that class static already
            }
        }
        
        public static void UpdateCameraPosition()
        {
            CameraX = LinkToSonicObject.X - GameForm.WindowWidth/2;
            CameraY = LinkToSonicObject.Y - GameForm.WindowHeight/2;
        }

        public static void MoveObjects()
        {
            foreach (var gameObject in GameState.ObjectList)
            {
                if (gameObject is SonicObject)
                {
                    var Sonic = (SonicObject) gameObject;
                    GameState.LinkToSonicObject.Move();
                }
                if (gameObject is BadnikSimpleObject)
                {
                    var Badnik = (BadnikSimpleObject) gameObject;
                    Badnik.Move();
                }
                if (gameObject is RingObject)
                {
                    var Ring = (RingObject) gameObject;        //maybe add Move() as a method to GameObject.cs to simplify this by a lot
                    Ring.Move();                            //it didnt. i cant seem to cast objects to their actual type without typing it out explicitly
                }
                if (gameObject is RingSparklesObject)
                {
                    var RingSparkles = (RingSparklesObject) gameObject;        //maybe add Move() as a method to GameObject.cs to simplify this by a lot
                    RingSparkles.Move();
                }
                if (gameObject is ExplosionObject)
                {
                    var Explosion = (ExplosionObject) gameObject;        //maybe add Move() as a method to GameObject.cs to simplify this by a lot
                    Explosion.Move();
                }
                /*
                if (gameObject.X - GameState.CameraX > -100 && gameObject.Y - GameState.CameraY > -100 &&
                    gameObject.X + GameState.CameraX > )
                    */
                //graphics.DrawImage(gameObject.CurrentBitmap, gameObject.X - GameState.CameraX - 13, gameObject.Y - GameState.CameraY - 19);
            }
        }

        public static void RemoveUnneededObjects()
        {
            foreach (var gameObject in ObjectsToRemove)
                ObjectList.Remove(gameObject);
        }
        
        public static void AddNeededObjects()
        {
            foreach (var gameObject in ObjectsToAdd)
                ObjectList.Add(gameObject);
        }
    }
}