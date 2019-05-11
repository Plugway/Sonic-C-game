using System.Collections.Generic;

namespace sonic_c_sharp
{
    public static class Reloading
    {
        public static void ReloadLevel()
        {
            Music.IsPlayingScrapBrainMusic = false;
            Music.ScrapBrainMusic.Stop();
            Music.FinalBossMusic.Stop();
            Music.EndingMusic.Stop();
            
            GameState.ObjectList = new List<GameObject>();
            GameState.ObjectsToRemove = new List<GameObject>();
            GameState.ObjectsToAdd = new List<GameObject>();
            
            GameState.MotobugsList = new List<GameObject>();
            GameState.MotobugsToRemove = new List<GameObject>();
            
            Resources.LoadResources("level1Files.txt");
            GameState.LinkToSonicObject.InitializeSonicCollisionBoxes();
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet = GameState.LinkToSonicObject.StandingCollisionBoxes;

            Background.InitiateBackground();

            GameState.BossStarted = false;
            GameState.LevelIsComplete = false;

            GameForm.IsFadingToTransperent = true;
        }
    }
}