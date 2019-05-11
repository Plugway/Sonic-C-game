using System;
using System.Drawing;

namespace sonic_c_sharp
{
    public class MechaSonicObject : GameObject
    {
        public MechaSonicObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.IsCollidable = true;
            this.CurrentBitmap = this.standingBitmap;
            this.CurrentAABB = standingAABB;
        }

        public Point[] CurrentAABB;

        private readonly Point[] standingAABB = { new Point(6, 6),
                                                  new Point(33, 64) };
        private readonly Point[] ducking2AABB = { new Point(8, 18),
                                                  new Point(35, 64) };
        private readonly Point[] firingAABB = { new Point(6, 3),
                                                new Point(33, 64) };
        private readonly Point[] spinningAABB = { new Point(4, 27),
                                                  new Point(45, 70) };        
        private readonly Point[] landing2AABB = { new Point(11, 14),
                                                  new Point(45, 67) };
        private readonly Point[] landing37AABB = { new Point(8, 5),
                                                   new Point(42, 64) };
        private readonly Point[] landing456AABB = { new Point(13, 5),
                                                    new Point(37, 64) };
        
        private readonly Point[] standingAABBFacingRight = { new Point(18, 6),
                                                             new Point(45, 64) };
        private readonly Point[] ducking2AABBFacingRight = { new Point(16, 18),
                                                             new Point(43, 64) };
        private readonly  Point[] firingAABBFacingRight = { new Point(18, 3),
                                                            new Point(45, 64) };
        private readonly Point[] spinningAABBFacingRight = { new Point(6, 27),
                                                             new Point(47, 70) };        
        private readonly Point[] landing2AABBFacingRight = { new Point(6, 14),
                                                             new Point(40, 67) };
        private readonly Point[] landing37AABBFacingRight = { new Point(9, 5),
                                                              new Point(43, 64) };
        private readonly Point[] landing456AABBFacingRight = { new Point(14, 5),
                                                               new Point(38, 64) };

        private readonly Bitmap standingBitmap = new Bitmap("graphics/mechaSonicStanding.png");
        private readonly Bitmap standingHurtBitmap = new Bitmap("graphics/mechaSonicStandingHurt.png");
        private readonly Bitmap[] spinningBitmaps = 
        {
            new Bitmap("graphics/mechaSonicSpinning1.png"),
            new Bitmap("graphics/mechaSonicSpinning2.png"),
            new Bitmap("graphics/mechaSonicSpinning3.png")
        };
        private readonly Bitmap[] spinningHurtBitmaps = 
        {
            new Bitmap("graphics/mechaSonicSpinning1Hurt.png"),
            new Bitmap("graphics/mechaSonicSpinning2Hurt.png"),
            new Bitmap("graphics/mechaSonicSpinning3Hurt.png")
        };
        private readonly Bitmap[] landingBitmaps = 
        {
            new Bitmap("graphics/mechaSonicLanding1.png"),
            new Bitmap("graphics/mechaSonicLanding2.png"),
            new Bitmap("graphics/mechaSonicLanding3.png"),
            new Bitmap("graphics/mechaSonicLanding4.png"),
            new Bitmap("graphics/mechaSonicLanding5.png"),
            new Bitmap("graphics/mechaSonicLanding6.png"),
            new Bitmap("graphics/mechaSonicLanding7.png")
        };
        private readonly Bitmap[] landingHurtBitmaps = 
        {
            new Bitmap("graphics/mechaSonicLanding1Hurt.png"),
            new Bitmap("graphics/mechaSonicLanding2Hurt.png"),
            new Bitmap("graphics/mechaSonicLanding3Hurt.png"),
            new Bitmap("graphics/mechaSonicLanding4Hurt.png"),
            new Bitmap("graphics/mechaSonicLanding5Hurt.png"),
            new Bitmap("graphics/mechaSonicLanding6Hurt.png"),
            new Bitmap("graphics/mechaSonicLanding7Hurt.png")
        };
        private readonly Bitmap[] duckingBitmaps = 
        {
            new Bitmap("graphics/mechaSonicDucking1.png"),
            new Bitmap("graphics/mechaSonicDucking2.png")
        };
        private readonly Bitmap[] duckingHurtBitmaps = 
        {
            new Bitmap("graphics/mechaSonicDucking1Hurt.png"),
            new Bitmap("graphics/mechaSonicDucking2Hurt.png")
        };
        private readonly Bitmap[] firingBitmaps = 
        {
            new Bitmap("graphics/mechaSonicFiring1.png"),
            new Bitmap("graphics/mechaSonicFiring2.png")
        };
        private readonly Bitmap[] firingHurtBitmaps = 
        {
            new Bitmap("graphics/mechaSonicFiring1Hurt.png"),
            new Bitmap("graphics/mechaSonicFiring2Hurt.png")
        };
        
        private readonly Bitmap standingBitmapFacingRight = new Bitmap("graphics/mechaSonicStandingFacingRight.png");
        private readonly Bitmap standingHurtBitmapFacingRight = new Bitmap("graphics/mechaSonicStandingHurtFacingRight.png");
        private readonly Bitmap[] spinningBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicSpinning1FacingRight.png"),
            new Bitmap("graphics/mechaSonicSpinning2FacingRight.png"),
            new Bitmap("graphics/mechaSonicSpinning3FacingRight.png")
        };
        private readonly Bitmap[] spinningHurtBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicSpinning1HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicSpinning2HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicSpinning3HurtFacingRight.png")
        };
        private readonly Bitmap[] landingBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicLanding1FacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding2FacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding3FacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding4FacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding5FacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding6FacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding7FacingRight.png")
        };
        private readonly Bitmap[] landingHurtBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicLanding1HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding2HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding3HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding4HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding5HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding6HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicLanding7HurtFacingRight.png")
        };
        private readonly Bitmap[] duckingBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicDucking1FacingRight.png"),
            new Bitmap("graphics/mechaSonicDucking2FacingRight.png")
        };
        private readonly Bitmap[] duckingHurtBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicDucking1HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicDucking2HurtFacingRight.png")
        };
        private readonly Bitmap[] firingBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicFiring1FacingRight.png"),
            new Bitmap("graphics/mechaSonicFiring2FacingRight.png")
        };
        private readonly Bitmap[] firingHurtBitmapsFacingRight = 
        {
            new Bitmap("graphics/mechaSonicFiring1HurtFacingRight.png"),
            new Bitmap("graphics/mechaSonicFiring2HurtFacingRight.png")
        };

        public int HitPoints = 8;
        
        public bool IsSpinning = false;
        public bool IsInvincible = false;
        public int InvincibilityFramesLeft = 0;
        
        private bool actionIsUndecided = true;
        private int actionInt;
        private Random random = new Random();

        private bool finishedStanding;
        private bool finishedDucking;
        private bool finishedStartSpinningInPlace;
        private bool finishedSpinningHorizonatally;
        private bool finishedJumpingFromSpinningAndLanding;
        private bool finishedDiagonalAndDownwardsSpinningJump;
        private bool finishedFiring;

        private bool isFacingLeft = true;
        
        private int hitPointsAtStartOfStep;

        private bool startMovement = false;
        private int redExplosionsFramesPassed = 0;
        //private bool stopExploding = false;
        
        public void Move()
        {
            if (GameState.LinkToSonicObject.X > X - 250)
                startMovement = true;            
            if (HitPoints <= 0) //&& !stopExploding)
            {
                IsInvincible = true;
                //IsSpinning = ???;
                startMovement = false;
                if (redExplosionsFramesPassed % 6 == 0)
                {
                    if (isFacingLeft)
                        GameState.ObjectsToAdd.Add(new RedExplosionObject(X + random.Next(-10, 25), Y + random.Next(0, 40)));
                    else
                        GameState.ObjectsToAdd.Add(new RedExplosionObject(X + random.Next(25, 60), Y + random.Next(0, 40)));
                    Sounds.RedExplosionSounds[Sounds.CurrentRedExplosionSound].Play();
                    Sounds.SwitchCurrentRedExplosionSound();
                }

                ++redExplosionsFramesPassed;

                if (redExplosionsFramesPassed > 110)
                {
                    //stopExploding = true;
                    Music.ScrapBrainMusic.Stop();
                    Music.FinalBossMusic.Stop();
                    
                    if (redExplosionsFramesPassed > 120)
                    {
                        Music.EndingMusic.PlayLooping();
                        GameState.LevelIsComplete = true;
                        GameState.ObjectsToRemove.Add(this);
                    }
                }
            }
            
            if (!startMovement)
                return;

            hitPointsAtStartOfStep = HitPoints;
            
            if (InvincibilityFramesLeft > 0)
            {
                IsInvincible = true;
                --InvincibilityFramesLeft;
            }
            else
                IsInvincible = false;

            if (actionIsUndecided)
            {
                actionInt = random.Next(0, 3);
                actionIsUndecided = false;

                //to separate method:
                finishedStanding = false;
                finishedDucking = false;
                finishedStartSpinningInPlace = false;
                finishedSpinningHorizonatally = false;
                finishedJumpingFromSpinningAndLanding = false;
                finishedDiagonalAndDownwardsSpinningJump = false;
                finishedFiring = false;

                standingFramesElapsed = 0;
                duckingFramesElapsed = 0;
                startSpinningInPlaceFramesElapsed = 0;
                spinningHorizontallyFramesElapsed = 0;
                jumpingFromSpinningAndLandingFramesElapsed = 0;
                diagonalAndDownwardsSpinningJumpFramesElapsed = 0;
                firingFramesElapsed = 0;
            }

            if (actionInt == 0)        //horizontal spin attack
            {
                if (!finishedStanding)
                    PerformStanding();
                else if (!finishedDucking)
                    PerformDucking();
                else if (!finishedStartSpinningInPlace)
                    PerformStartSpinningInPlace();
                else if (!finishedSpinningHorizonatally)
                    PerformSpinningHorizontally();
                else if (!finishedJumpingFromSpinningAndLanding)
                    PerformJumpingFromSpinningAndLanding();
                else
                    actionIsUndecided = true;
            }
            
            else if (actionInt == 1)        //diagonal spin attack
            {
                if (!finishedStanding)
                    PerformStanding();
                else if (!finishedDucking)
                    PerformDucking();
                else if (!finishedStartSpinningInPlace)
                    PerformStartSpinningInPlace();
                else if (!finishedDiagonalAndDownwardsSpinningJump)
                    PerformDiagonalAndDownwardsSpinningJump();
                else if (!finishedJumpingFromSpinningAndLanding)
                    PerformJumpingFromSpinningAndLanding();
                else
                    actionIsUndecided = true;
            }
            
            else if (actionInt == 2)        //firing attack
            {
                if (!finishedStanding)
                    PerformStanding();
                else if (!finishedFiring)
                    PerformFiring();
                else
                    actionIsUndecided = true;
            }
        }

        private int standingFramesElapsed = 0;
        private void PerformStanding()
        {
            IsSpinning = false;
            ++standingFramesElapsed;

            if (isFacingLeft)
                CurrentBitmap = !IsInvincible ? standingBitmap : standingHurtBitmap;
            else
                CurrentBitmap = !IsInvincible ? standingBitmapFacingRight : standingHurtBitmapFacingRight;
            CurrentAABB = isFacingLeft ? standingAABB : standingAABBFacingRight;
            
            if (standingFramesElapsed > 40)
                finishedStanding = true;
        }

        private int duckingFramesElapsed = 0;
        private void PerformDucking()
        {
            IsSpinning = false;
            ++duckingFramesElapsed;

            if (duckingFramesElapsed < 4)
            {
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? duckingBitmaps[0] : duckingHurtBitmaps[0];
                else
                    CurrentBitmap = !IsInvincible ? duckingBitmapsFacingRight[0] : duckingHurtBitmapsFacingRight[0];                
                CurrentAABB = isFacingLeft ? standingAABB : standingAABBFacingRight;
            }
            else if (duckingFramesElapsed < 8)
            {
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? duckingBitmaps[1] : duckingHurtBitmaps[1];
                else
                    CurrentBitmap = !IsInvincible ? duckingBitmapsFacingRight[1] : duckingHurtBitmapsFacingRight[1];
                CurrentAABB = isFacingLeft ? ducking2AABB : ducking2AABBFacingRight;
            }
            else
                finishedDucking = true;
        }
        
        private int startSpinningInPlaceFramesElapsed = 0;
        private void PerformStartSpinningInPlace()
        {
            IsSpinning = true;
            ++startSpinningInPlaceFramesElapsed;
            
            if (startSpinningInPlaceFramesElapsed == 1)
                Sounds.MechaSonicSpinningChargingSound.Play();

            if (startSpinningInPlaceFramesElapsed % 6 == 1 || startSpinningInPlaceFramesElapsed % 6 == 2)
            {
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                else
                    CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                CurrentAABB = isFacingLeft ? spinningAABB : spinningAABBFacingRight;
            }
            else if (startSpinningInPlaceFramesElapsed % 6 == 3 || startSpinningInPlaceFramesElapsed % 6 == 4)
            {
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                else
                    CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                CurrentAABB = isFacingLeft ? spinningAABB : spinningAABBFacingRight;
            }
            else if (startSpinningInPlaceFramesElapsed % 6 == 5 || startSpinningInPlaceFramesElapsed % 6 == 0)
            {
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                else
                    CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                CurrentAABB = isFacingLeft ? spinningAABB : spinningAABBFacingRight;
            }

            if (startSpinningInPlaceFramesElapsed > 45)
                finishedStartSpinningInPlace = true;
        }

        private int diagonalAndDownwardsSpinningJumpFramesElapsed = 0;
        private void PerformDiagonalAndDownwardsSpinningJump()
        {
            IsSpinning = true;
            ++diagonalAndDownwardsSpinningJumpFramesElapsed;
            
            if (diagonalAndDownwardsSpinningJumpFramesElapsed == 1)
                Sounds.MechaSonicSpinningGoSound.Play();

            CurrentAABB = isFacingLeft ? spinningAABB : spinningAABBFacingRight;
            
            if (hitPointsAtStartOfStep > 4)
            {
                if (diagonalAndDownwardsSpinningJumpFramesElapsed < 16)
                {
                    if (isFacingLeft)
                    {
                        X -= 26;
                        Y -= 10;
                        
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        X += 26;
                        Y -= 10;
                        
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else if (diagonalAndDownwardsSpinningJumpFramesElapsed < 22)
                {
                    Y += 25;
    
                    if (isFacingLeft)
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else
                {
                    finishedDiagonalAndDownwardsSpinningJump = true;
                    Sounds.MechaSonicSpinLandingSound.Play();
                }
            }
            else if (hitPointsAtStartOfStep <= 4)
            {
                if (diagonalAndDownwardsSpinningJumpFramesElapsed < 16)
                {
                    if (isFacingLeft)
                    {
                        X -= 26;
                        Y -= 10;
                        
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        X += 26;
                        Y -= 10;
                        
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else if (diagonalAndDownwardsSpinningJumpFramesElapsed < 22)
                {
                    Y += 25;
    
                    if (isFacingLeft)
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else if (diagonalAndDownwardsSpinningJumpFramesElapsed < 52)
                {
                    if (diagonalAndDownwardsSpinningJumpFramesElapsed == 22)
                        Sounds.MechaSonicSpinLandingSound.Play();
                    
                    //spinning in place for a bit
                    if (isFacingLeft)
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else if (diagonalAndDownwardsSpinningJumpFramesElapsed < 67)
                {
                    if (diagonalAndDownwardsSpinningJumpFramesElapsed == 52)
                        Sounds.MechaSonicSpinningGoSound.Play();
                    
                    if (isFacingLeft)
                    {
                        X -= 26;
                        Y -= 10;
                        
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        X += 26;
                        Y -= 10;
                        
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else if (diagonalAndDownwardsSpinningJumpFramesElapsed < 73)
                {
                    Y += 25;
    
                    if (isFacingLeft)
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (diagonalAndDownwardsSpinningJumpFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else
                {
                    finishedDiagonalAndDownwardsSpinningJump = true;

                    Sounds.MechaSonicSpinLandingSound.Play();
                }

                if (diagonalAndDownwardsSpinningJumpFramesElapsed == 27)
                    isFacingLeft = !isFacingLeft;
            }
        }
        
        private int spinningHorizontallyFramesElapsed = 0;
        private void PerformSpinningHorizontally()
        {
            IsSpinning = true;
            ++spinningHorizontallyFramesElapsed;

            if (spinningHorizontallyFramesElapsed == 1)
                Sounds.MechaSonicSpinningGoSound.Play();
            
            CurrentAABB = isFacingLeft ? spinningAABB : spinningAABBFacingRight;
            
            if (hitPointsAtStartOfStep > 4)
            {
                if (spinningHorizontallyFramesElapsed < 31)
                {
                    if (isFacingLeft)
                        X -= 13;
                    else
                        X += 13;
    
                    if (isFacingLeft)
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else
                    finishedSpinningHorizonatally = true;
            }
            else if (hitPointsAtStartOfStep <= 4)
            {
                if (spinningHorizontallyFramesElapsed < 31)
                {
                    if (isFacingLeft)
                        X -= 13;
                    else
                        X += 13;
    
                    if (isFacingLeft)
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else if (spinningHorizontallyFramesElapsed < 51)
                {
                    //spinning in place for a bit
                    if (isFacingLeft)
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else if (spinningHorizontallyFramesElapsed < 81)
                {
                    if (spinningHorizontallyFramesElapsed == 51)
                        Sounds.MechaSonicSpinningGoSound.Play();
                    
                    if (isFacingLeft)
                        X -= 13;
                    else
                        X += 13;
    
                    if (isFacingLeft)
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                    }
                    else
                    {
                        if (spinningHorizontallyFramesElapsed % 3 == 1)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                        else if (spinningHorizontallyFramesElapsed % 3 == 2)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                        else if (spinningHorizontallyFramesElapsed % 3 == 0)
                            CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                    }
                }
                else
                    finishedSpinningHorizonatally = true;
                
                if (spinningHorizontallyFramesElapsed == 40)
                    isFacingLeft = !isFacingLeft;
            }
        }

        private int jumpingFromSpinningAndLandingFramesElapsed = 0;
        private void PerformJumpingFromSpinningAndLanding()
        {
            ++jumpingFromSpinningAndLandingFramesElapsed;

            CurrentAABB = isFacingLeft ? spinningAABB : spinningAABBFacingRight;
            
            if (jumpingFromSpinningAndLandingFramesElapsed < 10)
            {
                IsSpinning = true;
                if (isFacingLeft)
                {
                    if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 1)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 2)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 0)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                }
                else
                {
                    if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 1)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 2)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 0)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                }
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 20)
            {
                Y -= 5;

                if (isFacingLeft)
                {
                    if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 1)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 2)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 0)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                }
                else
                {
                    if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 1)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 2)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 0)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                }
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 50)
            {
                if (isFacingLeft)
                {
                    if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 1)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[0] : spinningHurtBitmaps[0];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 2)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[1] : spinningHurtBitmaps[1];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 0)
                        CurrentBitmap = !IsInvincible ? spinningBitmaps[2] : spinningHurtBitmaps[2];
                }
                else
                {
                    if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 1)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[0] : spinningHurtBitmapsFacingRight[0];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 2)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[1] : spinningHurtBitmapsFacingRight[1];
                    else if (jumpingFromSpinningAndLandingFramesElapsed % 3 == 0)
                        CurrentBitmap = !IsInvincible ? spinningBitmapsFacingRight[2] : spinningHurtBitmapsFacingRight[2];
                }
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 52)
            {
                IsSpinning = false;
                
                Y += 3;
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? landingBitmaps[0] : landingHurtBitmaps[0];
                else
                    CurrentBitmap = !IsInvincible ? landingBitmapsFacingRight[0] : landingHurtBitmapsFacingRight[0];
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 54)
            {
                CurrentAABB = isFacingLeft ? landing2AABB : landing2AABBFacingRight;
                
                Y += 3;
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? landingBitmaps[1] : landingHurtBitmaps[1];
                else
                    CurrentBitmap = !IsInvincible ? landingBitmapsFacingRight[1] : landingHurtBitmapsFacingRight[1];
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 56)
            {
                CurrentAABB = isFacingLeft ? landing37AABB : landing37AABBFacingRight;
                
                Y += 3;
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? landingBitmaps[2] : landingHurtBitmaps[2];
                else
                    CurrentBitmap = !IsInvincible ? landingBitmapsFacingRight[2] : landingHurtBitmapsFacingRight[2];
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 58)
            {
                CurrentAABB = isFacingLeft ? landing456AABB : landing456AABBFacingRight;
                
                Y += 3;
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? landingBitmaps[3] : landingHurtBitmaps[3];
                else
                    CurrentBitmap = !IsInvincible ? landingBitmapsFacingRight[3] : landingHurtBitmapsFacingRight[3];
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 60)
            {
                CurrentAABB = isFacingLeft ? landing456AABB : landing456AABBFacingRight;
                
                Y += 3;
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? landingBitmaps[4] : landingHurtBitmaps[4];
                else
                    CurrentBitmap = !IsInvincible ? landingBitmapsFacingRight[4] : landingHurtBitmapsFacingRight[4];
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 62)
            {
                CurrentAABB = isFacingLeft ? landing456AABB : landing456AABBFacingRight;
                
                Y += 3;
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? landingBitmaps[5] : landingHurtBitmaps[5];
                else
                    CurrentBitmap = !IsInvincible ? landingBitmapsFacingRight[5] : landingHurtBitmapsFacingRight[5];
            }
            else if (jumpingFromSpinningAndLandingFramesElapsed < 64)
            {
                CurrentAABB = isFacingLeft ? landing37AABB : landing37AABBFacingRight;
                
                Y += 7;
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? landingBitmaps[6] : landingHurtBitmaps[6];
                else
                    CurrentBitmap = !IsInvincible ? landingBitmapsFacingRight[6] : landingHurtBitmapsFacingRight[6];
            }
            else
            {
                finishedJumpingFromSpinningAndLanding = true;
                
                Sounds.MechaSonicLandingSound.Play();
                isFacingLeft = !isFacingLeft;
            }
        }

        private int firingFramesElapsed = 0;
        private void PerformFiring()
        {
            IsSpinning = false;
            ++firingFramesElapsed;

            CurrentAABB = isFacingLeft ? firingAABB : firingAABBFacingRight;
            
            if (firingFramesElapsed < 4)
            {
                if (isFacingLeft)
                    CurrentBitmap = !IsInvincible ? firingBitmaps[0] : firingHurtBitmaps[0];
                else
                    CurrentBitmap = !IsInvincible ? firingBitmapsFacingRight[0] : firingHurtBitmapsFacingRight[0];
            }
            else
            {
                if (hitPointsAtStartOfStep > 4 && firingFramesElapsed < 90)
                {
                    if (firingFramesElapsed == 30 || firingFramesElapsed == 55 || firingFramesElapsed == 80)
                    {
                        if (isFacingLeft)
                            GameState.ObjectsToAdd.Add(new EnergyBallObject(X - 12, Y + 1, true));
                        else
                            GameState.ObjectsToAdd.Add(new EnergyBallObject(X + 25, Y + 1, false));
                        Sounds.MechaSonicFiringSounds[Sounds.CurrentFiringSound].Play();
                        Sounds.SwitchCurrentFiringSound();
                    }
                    
                    if (isFacingLeft)
                        CurrentBitmap = !IsInvincible ? firingBitmaps[1] : firingHurtBitmaps[1];
                    else
                        CurrentBitmap = !IsInvincible ? firingBitmapsFacingRight[1] : firingHurtBitmapsFacingRight[1];

                }
                else if (hitPointsAtStartOfStep <= 4 && firingFramesElapsed < 120)
                {
                    if (firingFramesElapsed == 30 || firingFramesElapsed == 50 || firingFramesElapsed == 70 ||
                        firingFramesElapsed == 90 || firingFramesElapsed == 110)
                    {
                        if (isFacingLeft)
                            GameState.ObjectsToAdd.Add(new EnergyBallObject(X - 12, Y + 1, true));
                        else
                            GameState.ObjectsToAdd.Add(new EnergyBallObject(X + 25, Y + 1, false));
                        Sounds.MechaSonicFiringSounds[Sounds.CurrentFiringSound].Play();
                        Sounds.SwitchCurrentFiringSound();
                    }
                    
                    if (isFacingLeft)
                        CurrentBitmap = !IsInvincible ? firingBitmaps[1] : firingHurtBitmaps[1];
                    else
                        CurrentBitmap = !IsInvincible ? firingBitmapsFacingRight[1] : firingHurtBitmapsFacingRight[1];

                }
                
                if (hitPointsAtStartOfStep > 4 && firingFramesElapsed >= 90 && firingFramesElapsed < 92)
                {
                    if (isFacingLeft)
                        CurrentBitmap = !IsInvincible ? firingBitmaps[0] : firingHurtBitmaps[0];
                    else
                        CurrentBitmap = !IsInvincible ? firingBitmapsFacingRight[0] : firingHurtBitmapsFacingRight[0];
                }
                else if (hitPointsAtStartOfStep > 4 && firingFramesElapsed >= 94)
                    finishedFiring = true;
                
                if (hitPointsAtStartOfStep <= 4 && firingFramesElapsed >= 120 && firingFramesElapsed < 122)
                {
                    if (isFacingLeft)
                        CurrentBitmap = !IsInvincible ? firingBitmaps[0] : firingHurtBitmaps[0];
                    else
                        CurrentBitmap = !IsInvincible ? firingBitmapsFacingRight[0] : firingHurtBitmapsFacingRight[0];
                }
                else if (hitPointsAtStartOfStep <= 4 && firingFramesElapsed >= 124)
                    finishedFiring = true;
            }
        }
    }
}