using System.Windows.Forms;

namespace sonic_c_sharp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Resources.LoadResources("genLvl.txt");
            GameState.LinkToSonicObject.InitializeSonicCollisionBoxes();
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet = GameState.LinkToSonicObject.StandingCollisionBoxes;
            Background.InitiateBackground();
            Music.InitiateMusic();
            Sounds.InitiateSounds();
            
            Application.EnableVisualStyles();
            Application.Run(new GameForm());            
        }
    }
}