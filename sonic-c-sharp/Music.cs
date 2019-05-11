using System.Media;

namespace sonic_c_sharp
{
    public static class Music
    {
        public static SoundPlayer ScrapBrainMusic = new SoundPlayer("music/Scrap Brain Zone.wav");
        public static SoundPlayer FinalBossMusic = new SoundPlayer("music/Final Boss.wav");
        public static SoundPlayer EndingMusic = new SoundPlayer("music/Ending.wav");

        public static bool IsPlayingScrapBrainMusic = false;
        
        public static void InitiateMusic()
        {
            ScrapBrainMusic.Load();
            FinalBossMusic.Load();
            EndingMusic.Load();
        }
    }
}