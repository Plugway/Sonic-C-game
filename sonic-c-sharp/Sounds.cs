using System.Media;

namespace sonic_c_sharp
{
    public static class Sounds
    {
        public static readonly SoundPlayer BossHitSound = new SoundPlayer("sounds/bossHitSound.wav");
        public static readonly SoundPlayer ExplosionSound = new SoundPlayer("sounds/explosionSound.wav");    //done
        public static readonly SoundPlayer JumpSonicCDSound = new SoundPlayer("sounds/jumpSonicCDSound.wav");
        public static readonly SoundPlayer JumpSound = new SoundPlayer("sounds/jumpSound.wav");        //done
        public static SoundPlayer JumpSoundCurrent = JumpSound;        //done
        public static readonly SoundPlayer LosingLifeSound = new SoundPlayer("sounds/losingLifeSound.wav");        //done
        public static readonly SoundPlayer[] MechaSonicFiringSounds =
        {
            new SoundPlayer("sounds/mechaSonicFiringSound.wav"),
            new SoundPlayer("sounds/mechaSonicFiringSound.wav")
        };
        public static readonly SoundPlayer MechaSonicLandingSound = new SoundPlayer("sounds/mechaSonicLandingSound.wav");
        public static readonly SoundPlayer RingLossSound = new SoundPlayer("sounds/ringLossSound.wav");        //done
        public static readonly SoundPlayer[] RingSounds =
        {
            new SoundPlayer("sounds/ringSound.wav"),                 //a crutch because if a ring sound effect starts, it must
            new SoundPlayer("sounds/ringSound.wav"),                 //finish before the next one plays. soundplayer sux.
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav"),
            new SoundPlayer("sounds/ringSound.wav")
        };
        public static readonly SoundPlayer SpikesSound = new SoundPlayer("sounds/spikesSound.wav");        //done
        public static readonly SoundPlayer SpinSound = new SoundPlayer("sounds/spinSound.wav");            //done
        public static readonly SoundPlayer SpringSound = new SoundPlayer("sounds/springSound.wav");        //done
        public static readonly SoundPlayer DoorClosingSound = new SoundPlayer("sounds/doorClosingSound.wav");        //done
        public static readonly SoundPlayer[] RedExplosionSounds =
        {
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav"),
            new SoundPlayer("sounds/redExplosionSound.wav")
        };
        public static readonly SoundPlayer MechaSonicSpinLandingSound = new SoundPlayer("sounds/mechaSonicSpinLandingSound.wav");        //done
        public static readonly SoundPlayer MechaSonicSpinningChargingSound = new SoundPlayer("sounds/mechaSonicSpinningChargingSound.wav");        //done
        public static readonly SoundPlayer MechaSonicSpinningGoSound = new SoundPlayer("sounds/mechaSonicSpinningGoSound.wav");        //done

        public static int CurrentRingSound = 0;
        public static int CurrentFiringSound = 0;
        public static int CurrentRedExplosionSound = 0;

        public static bool IsPlayingScrapBrainMusic = false;
        
        public static void InitiateSounds()
        {
            BossHitSound.Load();
            ExplosionSound.Load();
            JumpSonicCDSound.Load();
            JumpSound.Load();
            LosingLifeSound.Load();

            foreach (var mechaSonicFiringSound in MechaSonicFiringSounds)
                mechaSonicFiringSound.Load();
            
            MechaSonicLandingSound.Load();
            RingLossSound.Load();
            SpikesSound.Load();
            SpinSound.Load();
            SpringSound.Load();

            foreach (var ringSound in RingSounds)
                ringSound.Load();
        }

        public static void SwitchCurrentRingSound()
        {
            if (CurrentRingSound >= 29)
                CurrentRingSound = 0;
            else
                CurrentRingSound++;
        }
        public static void SwitchCurrentFiringSound()
        {
            if (CurrentFiringSound >= 1)
                CurrentFiringSound = 0;
            else
                CurrentFiringSound++;
        }
        public static void SwitchCurrentRedExplosionSound()
        {
            if (CurrentRedExplosionSound >= 14)
                CurrentRedExplosionSound = 0;
            else
                CurrentRedExplosionSound++;
        }

        public static void PlayRingCollectingSound()
        {
            if (!Cheats.DisableRingsCollectingSoundIsActive)
            {
                RingSounds[CurrentRingSound].Play();
                SwitchCurrentRingSound();
            }
        }
    }
}