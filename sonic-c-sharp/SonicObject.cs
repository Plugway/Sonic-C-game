using System;
using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public class SonicCollisionBoxes
    {
        public Point[] LargeAABB;        //for checking collision with tiles
        public Point[] HitBoxAABB;        //for checking collision with hazards
        
        public Point[] HeadLeft_Inner;
        public Point[] HeadRight_Inner;
        
        public Point[] WallTopLeft;
        public Point[] WallBottomLeft;
        public Point[] WallTopRight;
        public Point[] WallBottomRight;
        public Point[] WallLeft_Inner;
        public Point[] WallRight_Inner;

        public Point[] FootLeft_Inner;
        public Point[] FootRight_Inner;
        public Point[] FootLeft_Outer;
        public Point[] FootRight_Outer;
        public Point[] FootLeftBalancing_Outer;
        public Point[] FootRightBalancing_Outer;
        
        public bool HeadLeft_InnerIsActive;
        public bool HeadRight_InnerIsActive;
        
        public bool WallTopLeftIsActive;
        public bool WallBottomLeftIsActive;
        public bool WallTopRightIsActive;
        public bool WallBottomRightIsActive;
        public bool WallLeft_InnerIsActive;
        public bool WallRight_InnerIsActive;

        public bool FootLeft_InnerIsActive;
        public bool FootRight_InnerIsActive;
        public bool FootLeft_OuterIsActive;
        public bool FootRight_OuterIsActive;
        public bool FootLeftBalancing_OuterIsActive;
        public bool FootRightBalancing_OuterIsActive;
    }
    
    //public static class SonicObject : GameObject    that's disallowed. shit.
    public class SonicObject : GameObject
    {
        public SonicObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.PrevX = X;
            this.PrevY = Y;
            this.IsCollidable = true;
            this.CurrentBitmap = this.StandingBitmap;
        }
        
        //multiplication by 2 is to account for the 30fps
        public float XSpeed = 0;
        public float YSpeed = 0;
        public float GroundSpeed = 0;
        private const float Accelaration = 0.046875f*2.5f;
        private const float Decelaration = 0.5f*2;
        private const float RollingDecelaration = 0.125f*2;
        private const float RollingFriction = 0.0234375f*4;
        private const float Friction = 0.046875f*2;
        private const int XTopSpeed = 12;
        private const int YTopSpeed = 16;

        public const float NormalGravity = 0.21875f*4;
        public const float MoonGravity = 0.21875f*2;
        public float GravityCurrent = NormalGravity;    //feels more natural than 0.21875f*2 for some reason idk
        private const float Air = 0.09375f*2;
        private const float Jump = 6.5f*2;    //make less? 12f?

        //public const float GravityWhenHit = 0.1875f*2;
        
        public bool IsJumping = false;
        public bool ApplyJumpAtNextStep = false;
        public bool AlreadyJumpedSinceZWasPressedLastTime = false;

        public bool isRolling = false;
        private bool wasRolling = false;

        private bool GoingToLookUp = false;

        public bool EnemyCollisionDetected = false;
        public bool SpikesCollisionDetected = false;
        private bool CollidedWithEnemyAndNotLandedOnTile = false;
        public bool LandedOnTile = false;
        public bool HitFromRight;

        public bool ShouldBounceOffEnemy = false;

        public bool IsInvincible = false;
        public int InvincibilityFramesLeft = 0;
        public bool IsBlinking = false;
        public int IsBlinkingFramesElapsed = 0;

        public bool IsBouncingOffSpring = false;
        
        public bool IsLosingLife;

        public int PrevX;
        public int PrevY;
        public int Rings = 0;
        
        public SonicCollisionBoxes StandingCollisionBoxes = new SonicCollisionBoxes();
        public SonicCollisionBoxes RollingCollisionBoxes = new SonicCollisionBoxes();
        public SonicCollisionBoxes DuckingCollisionBoxes = new SonicCollisionBoxes();
        public SonicCollisionBoxes CurrentCollisionBoxesSet { get; set; }

        public void Move()            //rename into something like "PerformStepActions"?
        {    
            //cheats
            if (GameForm.WIsPressed)
                Y -= 20;
            if (GameForm.AIsPressed)
                X -= 20;
            if (GameForm.SIsPressed)
                Y += 20;
            if (GameForm.DIsPressed)
                X += 20;
            
            if (!Cheats.MoonGravityIsActive)
                GravityCurrent = GameForm.ZeroIsPressed ? 0 : NormalGravity;
            else
                GravityCurrent = GameForm.ZeroIsPressed ? 0 : MoonGravity;
            
            PrevX = X;
            PrevY = Y;

            if (InvincibilityFramesLeft > 0)
            {
                IsInvincible = true;
                --InvincibilityFramesLeft;

                if (IsBlinkingFramesElapsed > 2)
                {
                    if (IsBlinking)
                        IsBlinking = false;
                    else
                        IsBlinking = true;

                    IsBlinkingFramesElapsed = 0;
                }

                ++IsBlinkingFramesElapsed;
            }
            else
            {
                IsInvincible = false;
                IsBlinking = false;
            }
            
            if (ShouldBounceOffEnemy)
            {
                ShouldBounceOffEnemy = false;
                YSpeed = -YSpeed*2;
                IsJumping = true;
            }
            
            if (CollidedWithEnemyAndNotLandedOnTile && (CurrentCollisionBoxesSet.FootLeft_OuterIsActive || CurrentCollisionBoxesSet.FootRight_OuterIsActive))
            {
                LandedOnTile = true;
            }

            //this if statement is placed after the previous one so that at least one frame always passes after hit and sonic's y coordinate increases
            //so that the tile is not detected immediately after being hit
            if (EnemyCollisionDetected && Rings > 0)
            {
                InvincibilityFramesLeft = 120/2 + 20;
                IsInvincible = true;

                DroppingRings.DropRings();
                
                Rings = 0;
                
                YSpeed = -4*2;

                if (HitFromRight)
                {
                    XSpeed = -2*2;
                    GroundSpeed = -2*2;
                }
                else
                {
                    XSpeed = 2*2;
                    GroundSpeed = 2*2;
                }
                
                CollidedWithEnemyAndNotLandedOnTile = true;
                
                Sounds.RingLossSound.Play();
            }
            else if (EnemyCollisionDetected && Rings == 0 && !IsLosingLife)
            {
                CurrentBitmap = LosingLifeBitmap;
                IsLosingLife = true;
                XSpeed = 0;
                YSpeed = -7*2;
                GroundSpeed = 0;
                GameForm.IsFadingToBlack = true;
                
                if (SpikesCollisionDetected)
                    Sounds.SpikesSound.Play();
                else
                    Sounds.LosingLifeSound.Play();
            }
            
            if (CollidedWithEnemyAndNotLandedOnTile && LandedOnTile)
            {
                CollidedWithEnemyAndNotLandedOnTile = false;

                XSpeed = 0;
                GroundSpeed = 0;
                
                LandedOnTile = false;
            }

            if (!isRolling && !IsJumping && !wasRolling)
                CurrentCollisionBoxesSet = StandingCollisionBoxes;
            else
                CurrentCollisionBoxesSet = RollingCollisionBoxes;
            //ducking collision boxes get set as long as down is pressed when at low speed and not airborne
            
            //Console.WriteLine("FootLeft_OuterIsActive:" + CurrentCollisionBoxesSet.FootLeft_OuterIsActive + " FootRight_OuterIsActive:" + CurrentCollisionBoxesSet.FootRight_OuterIsActive);

            if (IsLosingLife)
            {
                YSpeed += GravityCurrent;
            }
            
            //if hit by enemy previously and not landed yet:
            else if (CollidedWithEnemyAndNotLandedOnTile)
            {
                YSpeed += GravityCurrent;
                
                PerformHitAnimation();
            }
            
            //if not airborne:
            else if ((CurrentCollisionBoxesSet.FootLeft_OuterIsActive || CurrentCollisionBoxesSet.FootRight_OuterIsActive)) //&& 
                //!(CurrentCollisionBoxesSet.FootLeft_OuterIsActive && !CurrentCollisionBoxesSet.FootRight_OuterIsActive && (CurrentCollisionBoxesSet.WallBottomLeftIsActive || CurrentCollisionBoxesSet.WallTopLeftIsActive)) &&    //2 cases of being stuck in a tile from the side
                //!(!CurrentCollisionBoxesSet.FootLeft_OuterIsActive && CurrentCollisionBoxesSet.FootRight_OuterIsActive && (CurrentCollisionBoxesSet.WallBottomRightIsActive || CurrentCollisionBoxesSet.WallTopRightIsActive)))
            {
                if (!IsBouncingOffSpring)
                    YSpeed = 0;
                
                IsJumping = false;
                GoingToLookUp = false;
                wasRolling = false;
                
                //Console.WriteLine("not airborne: " + CurrentCollisionBoxesSet.FootLeft_OuterIsActive + " " + CurrentCollisionBoxesSet.FootRight_OuterIsActive);
                
                if (GameForm.UpIsPressed && XSpeed == 0)
                {
                    GoingToLookUp = true;
                }
                
                if (GameForm.DownIsPressed)
                {

                    if (Math.Abs(XSpeed) > 0.53125) //criteria for rolling
                    {
                        if (!isRolling)
                            Sounds.SpinSound.Play();
                        
                        isRolling = true;
                    }
                    else
                    {
                        if (XSpeed == 0)
                        {
                            CurrentCollisionBoxesSet = DuckingCollisionBoxes;
                        }
                    }
                }
                
                if (GameForm.LeftIsPressed && CurrentCollisionBoxesSet != DuckingCollisionBoxes && !GoingToLookUp)
                {
                    if (!isRolling)
                    {
                        if (GroundSpeed == 0 || GroundSpeed < 0)
                            GroundSpeed -= Accelaration;
                        if (GroundSpeed > 0)
                            GroundSpeed -= Decelaration;
                        
                        CurrentCollisionBoxesSet = StandingCollisionBoxes;
                    }
                    else
                    {
                        //cannot accelarate from player input in this state
                        if (GroundSpeed > 0)
                            GroundSpeed -= RollingDecelaration;
                    }
                }

                if (GameForm.RightIsPressed && CurrentCollisionBoxesSet != DuckingCollisionBoxes && !GoingToLookUp)
                {
                    if (!isRolling)
                    {
                        if (GroundSpeed == 0 || GroundSpeed > 0)
                            GroundSpeed += Accelaration;
                        if (GroundSpeed < 0)
                            GroundSpeed += Decelaration;
                        
                        CurrentCollisionBoxesSet = StandingCollisionBoxes;
                    }
                    else
                    {
                        //cannot accelarate from player input in this state
                        if (GroundSpeed < 0)
                            GroundSpeed += RollingDecelaration;
                    }
                }

                //applying friction when rolling
                if (isRolling)
                {
                    CurrentCollisionBoxesSet = RollingCollisionBoxes;
                    if (Math.Abs(GroundSpeed) < RollingFriction)
                    {
                        GroundSpeed = 0;
                        isRolling = false;
                    }
                    else if (GroundSpeed > 0)
                        GroundSpeed -= RollingFriction;
                    else if (GroundSpeed < 0)
                        GroundSpeed += RollingFriction;
                }
                else if (!GameForm.LeftIsPressed && !GameForm.RightIsPressed)    //not rolling
                {
                    if (Math.Abs(GroundSpeed) < Friction)
                        GroundSpeed = 0;
                    else if (GroundSpeed > 0)
                        GroundSpeed -= Friction;
                    else if (GroundSpeed < 0)
                        GroundSpeed += Friction;
                }
                

                if (GameForm.ZIsPressed && !AlreadyJumpedSinceZWasPressedLastTime)
                {
                    CurrentCollisionBoxesSet = RollingCollisionBoxes;

                    ApplyJumpAtNextStep = true;
                }
                if (!GameForm.ZIsPressed)
                {
                    AlreadyJumpedSinceZWasPressedLastTime = false;   //getting reset when Z is not pressed
                }
                
                if (ApplyJumpAtNextStep)
                {
                    if (GameForm.ZIsPressed)
                        YSpeed = -Jump;
                    else
                        YSpeed = -4;
                    ApplyJumpAtNextStep = false;
                    IsJumping = true;

                    AlreadyJumpedSinceZWasPressedLastTime = true;    //will get reset when Z is not pressed
                    
                    Sounds.JumpSoundCurrent.Play();
                }
                
                
                //animations stuff
                if (IsBouncingOffSpring && YSpeed >= 0)
                {
                    IsBouncingOffSpring = false;
                }
                
                if (IsBouncingOffSpring)
                {
                    PerformBouncingOffSpringAnimation();
                    
                    ResetRunningAnimationVariables();
                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (Math.Abs(XSpeed) > 6 && !isRolling)
                {
                    PerformRunningAnimation();

                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (Math.Abs(XSpeed) <= 6 && Math.Abs(XSpeed) > 0 && !isRolling)
                {
                    PerformWalkingAnimation();

                    ResetLookingUpAnimationVariables();
                    ResetRunningAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (XSpeed == 0 && !isRolling && CurrentCollisionBoxesSet != DuckingCollisionBoxes && GameForm.UpIsPressed)
                {
                    PerformLookingUpAnimation();
                    
                    ResetRunningAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (XSpeed == 0 && CurrentCollisionBoxesSet == DuckingCollisionBoxes)
                {
                    PerformDuckingAnimation();
                    
                    ResetRunningAnimationVariables();
                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetRollingAnimationVariables();
                }
                else if (XSpeed == 0 && !isRolling && CurrentCollisionBoxesSet != DuckingCollisionBoxes && !GameForm.UpIsPressed)
                {
                    PerformStandingAnimation();
                    
                    ResetRunningAnimationVariables();
                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (isRolling)
                {
                    PerformRollingAnimation();
                    
                    ResetRunningAnimationVariables();
                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                //if (XSpeed == 0)
                //{
                 //   CurrentCollisionBoxesSet = DuckingCollisionBoxes;
                 //   PerformDuckingAnimation();
                //}
            }
            
            //if airborne:
            else
            {
                //Console.WriteLine("is airborne");
                
                if (isRolling)
                    wasRolling = true;        //if sonic spins, then jumps, we want him to walk when he lands, not spin; but 
                isRolling = false;            //we do want to him to continue to spin while airborne if he spins off a surface
                
                if (GameForm.LeftIsPressed)
                {
                    XSpeed -= Air;

                    //CurrentCollisionBoxesSet = StandingCollisionBoxes;
                }

                if (GameForm.RightIsPressed)
                {
                    XSpeed += Air;

                    //CurrentCollisionBoxesSet = StandingCollisionBoxes;
                }
                

                //cutting the jump short
                if (IsJumping && !GameForm.ZIsPressed)
                {
                    if (YSpeed < -4*2)
                        YSpeed = -4*2;
                }
                
                if (!GameForm.ZIsPressed)
                    AlreadyJumpedSinceZWasPressedLastTime = false;    //getting reset when Z is not pressed
                
                //air drag
                if (YSpeed < 0 && YSpeed > -4)
                {
                    if (Math.Abs(XSpeed) >= 0.125f)
                        XSpeed *= 0.96875f;
                }
                
                YSpeed += GravityCurrent;
                GroundSpeed = XSpeed;
                
                if (CurrentCollisionBoxesSet.WallTopLeftIsActive)    //using TopLeft here is an ugly hack. now if there's a small
                    if (XSpeed < 0)                             //item like a spring, Sonic might not stop. BUT, if BottomLeft
                        XSpeed = 0;                             //is used instead, then upon landing from a height the feet AND BottomLeft sensors
                if (CurrentCollisionBoxesSet.WallTopRightIsActive)   //get activated, i guess, and Sonic loses any running momentum he had after landing from a jump
                    if (XSpeed > 0)
                        XSpeed = 0;                             //maybe if the BottomLeft sensor wasn't as long/close to the foot sensor, none of these would've been an issue
								//this crutch might be unnecessary if sonic doesn't sink into the ground on occasion anymore
                
                
                //animations stuff
                if (IsBouncingOffSpring && YSpeed >= 0)
                {
                    IsBouncingOffSpring = false;
                }

                if (IsBouncingOffSpring)
                {
                    PerformBouncingOffSpringAnimation();
                    
                    ResetRunningAnimationVariables();
                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (IsJumping || wasRolling)
                {
                    PerformRollingAnimation();

                    ResetRunningAnimationVariables();
                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (!IsJumping && !wasRolling && Math.Abs(XSpeed) > 6)
                {
                    PerformRunningAnimation();

                    ResetLookingUpAnimationVariables();
                    ResetWalkingAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
                else if (!IsJumping && !wasRolling && Math.Abs(XSpeed) <= 6 && Math.Abs(XSpeed) > 0)
                {
                    PerformWalkingAnimation();

                    ResetLookingUpAnimationVariables();
                    ResetRunningAnimationVariables();
                    ResetRollingAnimationVariables();
                    ResetDuckingAnimationVariables();
                }
            }

            if (CurrentCollisionBoxesSet.WallTopLeftIsActive)    //using TopLeft here is an ugly hack. now if there's a small
                if (GroundSpeed < 0)                             //item like a spring, Sonic might not stop. BUT, if BottomLeft
                    GroundSpeed = 0;                             //is used instead, then upon landing from a height the feet AND BottomLeft sensors
            if (CurrentCollisionBoxesSet.WallTopRightIsActive)   //get activated, i guess, and Sonic loses any running momentum he had after landing from a jump
                if (GroundSpeed > 0)
                    GroundSpeed = 0;                             //maybe if the BottomLeft sensor wasn't as long/close to the foot sensor, none of these would've been an issue

            if (CurrentCollisionBoxesSet.FootLeft_OuterIsActive || CurrentCollisionBoxesSet.FootRight_OuterIsActive)
                XSpeed = GroundSpeed;    //=gsp*cos(angle);    if have angles
                //YSpeed = 0;            //=gsp*-sin(angle);   if have angles

            
            //checking whether exceeds top speed
            if (GroundSpeed > XTopSpeed)
                GroundSpeed = XTopSpeed;
            if (GroundSpeed < -XTopSpeed)
                GroundSpeed = -XTopSpeed;
            
            if (XSpeed > YTopSpeed)
                XSpeed = YTopSpeed;
            if (XSpeed < -YTopSpeed)
                XSpeed = -YTopSpeed;

            if (YSpeed > YTopSpeed)
                YSpeed = YTopSpeed;
            if (YSpeed < -YTopSpeed)
                YSpeed = -YTopSpeed;
            
            X += (int)XSpeed;
            Y += (int)YSpeed;
            
            /*
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);
            Console.WriteLine(IsJumping + " " + ApplyJumpAtNextStep + " " + XSpeed + " " + YSpeed + " " + Y);*/
        }
        
        public void InitializeSonicCollisionBoxes()
        {
            //standing
            StandingCollisionBoxes.LargeAABB = new [] { new Point(14, 9), new Point(31, 48) };
            StandingCollisionBoxes.HitBoxAABB = new [] { new Point(16, 11), new Point(29, 47) };
            
            StandingCollisionBoxes.HeadLeft_Inner = new [] { new Point(16, 9), new Point(16, 9) };    //fix attempt: 14 -> 16
            StandingCollisionBoxes.HeadRight_Inner = new [] { new Point(29, 9), new Point(29, 9) };    //fix attempt: 31 -> 29
            
            StandingCollisionBoxes.WallTopLeft = new [] { new Point(14, 10), new Point(14, 27) };
            StandingCollisionBoxes.WallBottomLeft = new [] { new Point(14, 28), new Point(14, 46) };    //46 -> 40 fix attempt
            StandingCollisionBoxes.WallLeft_Inner = new [] { new Point(15, 10), new Point(15, 46) };    //46 -> 40 fix attempt
            
            StandingCollisionBoxes.WallTopRight = new [] { new Point(31, 10), new Point(31, 27) };
            StandingCollisionBoxes.WallBottomRight = new [] { new Point(31, 28), new Point(31, 46) };    //46 -> 40 fix attempt
            StandingCollisionBoxes.WallRight_Inner = new [] { new Point(30, 10), new Point(30, 46) };    //46 -> 40 fix attempt
    
            StandingCollisionBoxes.FootLeft_Inner = new [] { new Point(16, 47), new Point(16, 47) };    //fix attempt: 14 -> 16
            StandingCollisionBoxes.FootLeft_Outer = new [] { new Point(16, 48), new Point(16, 48) };    //fix attempt: 14 -> 16
            StandingCollisionBoxes.FootLeftBalancing_Outer = new [] { new Point(15, 48), new Point(17, 48) };
            
            StandingCollisionBoxes.FootRight_Inner = new [] { new Point(29, 47), new Point(29, 47) };    //fix attempt: 31 -> 29
            StandingCollisionBoxes.FootRight_Outer = new [] { new Point(29, 48), new Point(29, 48) };    //fix attempt: 31 -> 29
            StandingCollisionBoxes.FootRightBalancing_Outer = new [] { new Point(28, 48), new Point(30, 48) };
            
            
            //rolling
            RollingCollisionBoxes.LargeAABB = new [] { new Point(14, 18), new Point(31, 48) };
            RollingCollisionBoxes.HitBoxAABB = new [] { new Point(16, 19), new Point(29, 46) };
            
            RollingCollisionBoxes.HeadLeft_Inner = new [] { new Point(16, 18), new Point(16, 18) };    //fix attempt: 14 -> 16
            RollingCollisionBoxes.HeadRight_Inner = new [] { new Point(29, 18), new Point(29, 18) };    //fix attempt: 31 -> 29
            
            RollingCollisionBoxes.WallTopLeft = new [] { new Point(14, 19), new Point(14, 32) };
            RollingCollisionBoxes.WallBottomLeft = new [] { new Point(14, 33), new Point(14, 46) };    //46 -> 40 fix attempt
            RollingCollisionBoxes.WallLeft_Inner = new [] { new Point(15, 19), new Point(15, 46) };    //46 -> 40 fix attempt
            
            RollingCollisionBoxes.WallTopRight = new [] { new Point(31, 19), new Point(31, 32) };
            RollingCollisionBoxes.WallBottomRight = new [] { new Point(31, 33), new Point(31, 46) };    //46 -> 40 fix attempt
            RollingCollisionBoxes.WallRight_Inner = new [] { new Point(30, 19), new Point(30, 46) };    //46 -> 40 fix attempt
    
            RollingCollisionBoxes.FootLeft_Inner = new [] { new Point(16, 47), new Point(16, 47) };    //fix attempt: 14 -> 16
            RollingCollisionBoxes.FootLeft_Outer = new [] { new Point(16, 48), new Point(16, 48) };    //fix attempt: 14 -> 16
            RollingCollisionBoxes.FootLeftBalancing_Outer = new [] { new Point(15, 48), new Point(17, 48) };
            
            RollingCollisionBoxes.FootRight_Inner = new [] { new Point(29, 47), new Point(29, 47) };    //fix attempt: 31 -> 29
            RollingCollisionBoxes.FootRight_Outer = new [] { new Point(29, 48), new Point(29, 48) };    //fix attempt: 31 -> 29
            RollingCollisionBoxes.FootRightBalancing_Outer = new [] { new Point(28, 48), new Point(30, 48) };
            
            
            //ducking
            DuckingCollisionBoxes.LargeAABB = new [] { new Point(14, 22), new Point(31, 48) };
            DuckingCollisionBoxes.HitBoxAABB = new [] { new Point(16, 23), new Point(29, 47) };
            
            DuckingCollisionBoxes.HeadLeft_Inner = new [] { new Point(16, 22), new Point(16, 22) };    //fix attempt: 14 -> 16
            DuckingCollisionBoxes.HeadRight_Inner = new [] { new Point(29, 22), new Point(29, 22) };    //fix attempt: 31 -> 29
            
            DuckingCollisionBoxes.WallTopLeft = new [] { new Point(14, 23), new Point(14, 34) };
            DuckingCollisionBoxes.WallBottomLeft = new [] { new Point(14, 35), new Point(14, 46) };    //46 -> 40 fix attempt
            DuckingCollisionBoxes.WallLeft_Inner = new [] { new Point(15, 23), new Point(15, 46) };    //46 -> 40 fix attempt
            
            DuckingCollisionBoxes.WallTopRight = new [] { new Point(31, 23), new Point(31, 34) };
            DuckingCollisionBoxes.WallBottomRight = new [] { new Point(31, 35), new Point(31, 46) };    //46 -> 40 fix attempt
            DuckingCollisionBoxes.WallRight_Inner = new [] { new Point(30, 23), new Point(30, 46) };    //46 -> 40 fix attempt
        
            DuckingCollisionBoxes.FootLeft_Inner = new [] { new Point(16, 47), new Point(16, 47) };    //fix attempt: 14 -> 16
            DuckingCollisionBoxes.FootLeft_Outer = new [] { new Point(16, 48), new Point(16, 48) };    //fix attempt: 14 -> 16
            DuckingCollisionBoxes.FootLeftBalancing_Outer = new [] { new Point(15, 48), new Point(17, 48) };    //unused
            
            DuckingCollisionBoxes.FootRight_Inner = new [] { new Point(29, 47), new Point(29, 47) };    //fix attempt: 31 -> 29
            DuckingCollisionBoxes.FootRight_Outer = new [] { new Point(29, 48), new Point(29, 48) };    //fix attempt: 31 -> 29
            DuckingCollisionBoxes.FootRightBalancing_Outer = new [] { new Point(28, 48), new Point(30, 48) };    //unused
            
        }

        private Bitmap StandingBitmap = new Bitmap("graphics/sonicStanding.png");
        private Bitmap HitBitmap = new Bitmap("graphics/sonicHit.png");
        private Bitmap BouncingOffSpringBitmap = new Bitmap("graphics/sonicJumpingOffSpring.png");
        private Bitmap LosingLifeBitmap = new Bitmap("graphics/sonicLosingLife.png");
        private Bitmap[] WalkingBitmaps = 
        {
            new Bitmap("graphics/sonicWalking1.png"),
            new Bitmap("graphics/sonicWalking2.png"),
            new Bitmap("graphics/sonicWalking3.png"),
            new Bitmap("graphics/sonicWalking4.png"),
            new Bitmap("graphics/sonicWalking5.png"),
            new Bitmap("graphics/sonicWalking6.png"),
            new Bitmap("graphics/sonicWalking7.png"),
            new Bitmap("graphics/sonicWalking8.png")
        };
        private Bitmap[] RunningBitmaps = 
        {
            new Bitmap("graphics/sonicRunning1.png"),
            new Bitmap("graphics/sonicRunning2.png"),
            new Bitmap("graphics/sonicRunning3.png"),
            new Bitmap("graphics/sonicRunning4.png")
        };
        private Bitmap[] LookingUpBitmaps = 
        {
            new Bitmap("graphics/sonicLookingUp1.png"),
            new Bitmap("graphics/sonicLookingUp2.png")
        };
        private Bitmap[] DuckingBitmaps = 
        {
            new Bitmap("graphics/sonicDucking1.png"),
            new Bitmap("graphics/sonicDucking2.png")
        };
        private Bitmap[] RollingBitmaps = 
        {
            new Bitmap("graphics/sonicRolling1.png"),
            new Bitmap("graphics/sonicRolling2.png"),
            new Bitmap("graphics/sonicRolling3.png"),
            new Bitmap("graphics/sonicRolling4.png"),
            new Bitmap("graphics/sonicRolling5.png")
        };
        /*
        private Bitmap[] SkiddingBitmaps = 
        {
            new Bitmap("graphics/sonicSkidding1.png"),
            new Bitmap("graphics/sonicSkidding2.png"),
            new Bitmap("graphics/sonicSkidding3.png")
        };
        private Bitmap[] PushingBitmaps = 
        {
            new Bitmap("graphics/sonicPushing1.png"),
            new Bitmap("graphics/sonicPushing2.png"),
            new Bitmap("graphics/sonicPushing3.png"),
            new Bitmap("graphics/sonicPushing4.png")
        };
        */

        public bool IsFacingLeft = false;

        private bool StandingBitmapIsFlipped;
        private bool HitBitmapIsFlipped;
        private bool BouncingOffSpringBitmapIsFlipped;
        private bool[] WalkingBitmapsIsFlipped = new bool[8];
        private bool[] RunningBitmapsIsFlipped = new bool[4];
        private bool[] LookingUpBitmapsIsFlipped = new bool[2];
        private bool[] DuckingBitmapsIsFlipped = new bool[2];
        private bool[] RollingBitmapsIsFlipped = new bool[5];
        //private bool[] SkiddingBitmapsIsFlipped = new bool[3];
        //private bool[] PushingBitmapsIsFlipped = new bool[4];
        
        
        private int framesElapsedWhileWalking = 0;
        private int framesElapsedWhileRunning = 0;
        private int framesElapsedWhileRolling = 0;
        private int framesElapsedWhileLookingUp = 0;
        private int framesElapsedWhileDucking = 0;

        private int currentWalkingAnimationFrame = 0;
        private int currentRunningAnimationFrame = 0;
        private int currentRollingAnimationFrame = 0;
        private int currentLookingUpAnimationFrame = 0;
        private int currentDuckingAnimationFrame = 0;
        
        private void PerformWalkingAnimation()
        {   
            int frameDuration;
            if (8 - Math.Abs(GroundSpeed) > 1)
                frameDuration = (int)(8 - Math.Abs(GroundSpeed));
            else
                frameDuration = 1;
            
            if (framesElapsedWhileWalking > frameDuration)
            {
                framesElapsedWhileWalking = 0;
                ++currentWalkingAnimationFrame;
                if (currentWalkingAnimationFrame > 7)
                {
                    currentWalkingAnimationFrame = 0;
                }
            }

            //checking whether IsFacingLeft needs to be changed
            
            //if (GameForm.RightIsPressed && GroundSpeed > 0)        before, as described in SPG
            if (GroundSpeed > 0)
                IsFacingLeft = false;
            //else if (GameForm.LeftIsPressed && GroundSpeed < 0)        before, as described in SPG
            else if (GroundSpeed < 0)
                IsFacingLeft = true;
            
            //checking conditions for flipping animations
            //if (WalkingBitmapsIsFlipped[currentWalkingAnimationFrame] && GameForm.RightIsPressed && GroundSpeed > 0)        before, as described in SPG
            if (WalkingBitmapsIsFlipped[currentWalkingAnimationFrame] && GroundSpeed > 0)
            {
                WalkingBitmaps[currentWalkingAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                WalkingBitmapsIsFlipped[currentWalkingAnimationFrame] = false;
                //IsFacingLeft = false;
            }
            //else if (!WalkingBitmapsIsFlipped[currentWalkingAnimationFrame] && GameForm.LeftIsPressed && GroundSpeed < 0)        before, as described in SPG
            else if (!WalkingBitmapsIsFlipped[currentWalkingAnimationFrame] && GroundSpeed < 0)
            {
                WalkingBitmaps[currentWalkingAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                WalkingBitmapsIsFlipped[currentWalkingAnimationFrame] = true;
                //IsFacingLeft = true;
            }
            
            CurrentBitmap = WalkingBitmaps[currentWalkingAnimationFrame];
                
            ++framesElapsedWhileWalking;
        }
        private void PerformRunningAnimation()
        {
            int frameDuration;
            if (8 - Math.Abs(GroundSpeed) > 1)
                frameDuration = (int)(8 - Math.Abs(GroundSpeed));
            else
                frameDuration = 1;
            
            if (framesElapsedWhileRunning > frameDuration)
            {
                framesElapsedWhileRunning = 0;
                ++currentRunningAnimationFrame;
                if (currentRunningAnimationFrame > 2)
                    currentRunningAnimationFrame = 0;
            }
            
            //checking whether IsFacingLeft needs to be changed
            //if (GameForm.RightIsPressed && GroundSpeed > 0)        before, as described in SPG
            if (GroundSpeed > 0)
                IsFacingLeft = false;
            //if (GameForm.LeftIsPressed && GroundSpeed < 0)        before, as described in SPG
            else if (GroundSpeed < 0)
                IsFacingLeft = true;

            //checking conditions for flipping animations
            //if (RunningBitmapsIsFlipped[currentRunningAnimationFrame] && GameForm.RightIsPressed && GroundSpeed > 0)        before, as described in SPG
            if (RunningBitmapsIsFlipped[currentRunningAnimationFrame] && GroundSpeed > 0)
            {
                RunningBitmaps[currentRunningAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                RunningBitmapsIsFlipped[currentRunningAnimationFrame] = false;
                //IsFacingLeft = false;                
            }
            //else if (!RunningBitmapsIsFlipped[currentRunningAnimationFrame] && GameForm.LeftIsPressed && GroundSpeed < 0)        before, as described in SPG
            else if (!RunningBitmapsIsFlipped[currentRunningAnimationFrame] && GroundSpeed < 0)
            {
                RunningBitmaps[currentRunningAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                RunningBitmapsIsFlipped[currentRunningAnimationFrame] = true;
                //IsFacingLeft = true;
            }
            
            CurrentBitmap = RunningBitmaps[currentRunningAnimationFrame];
            
            ++framesElapsedWhileRunning;
        }
        private void PerformRollingAnimation()
        {
            int frameDuration;
            if (5 - Math.Abs(GroundSpeed) > 1)
                frameDuration = (int)(5 - Math.Abs(GroundSpeed));
            else
                frameDuration = 1;
            
            if (framesElapsedWhileRolling > frameDuration)
            {
                framesElapsedWhileRolling = 0;
                ++currentRollingAnimationFrame;
                if (currentRollingAnimationFrame > 4)
                    currentRollingAnimationFrame = 0;
            }
           
            //checking whether IsFacingLeft needs to be changed
            if (GameForm.RightIsPressed)
                IsFacingLeft = false;
            else if (GameForm.LeftIsPressed)
                IsFacingLeft = true;

            //checking conditions for flipping animations
            //if (RollingBitmapsIsFlipped[currentRollingAnimationFrame] && GameForm.RightIsPressed)        before, as described in SPG
            if (RollingBitmapsIsFlipped[currentRollingAnimationFrame] && !IsFacingLeft)
            {
                RollingBitmaps[currentRollingAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                RollingBitmapsIsFlipped[currentRollingAnimationFrame] = false;
                //IsFacingLeft = false;
            }
            //else if (!RollingBitmapsIsFlipped[currentRollingAnimationFrame] && GameForm.LeftIsPressed)        before, as described in SPG
            else if (!RollingBitmapsIsFlipped[currentRollingAnimationFrame] && IsFacingLeft)
            {
                //Console.WriteLine("excuse me, dafuq");
                RollingBitmaps[currentRollingAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                RollingBitmapsIsFlipped[currentRollingAnimationFrame] = true;
                //IsFacingLeft = true;
            }
            else
            {
                //Console.WriteLine("what's wrong?? IsFlipped:" + RollingBitmapsIsFlipped[currentRollingAnimationFrame] + " GameForm.LeftIsPressed:" + GameForm.LeftIsPressed);
            }

            CurrentBitmap = RollingBitmaps[currentRollingAnimationFrame];
            
            ++framesElapsedWhileRolling;
        }
        private void PerformDuckingAnimation()
        {
            if (framesElapsedWhileDucking > 2)
            {
                framesElapsedWhileDucking = 0;
                ++currentDuckingAnimationFrame;
                if (currentDuckingAnimationFrame > 1)
                    currentDuckingAnimationFrame = 1;
            }
            
            if (DuckingBitmapsIsFlipped[currentDuckingAnimationFrame] && !IsFacingLeft)
            {
                DuckingBitmaps[currentDuckingAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                DuckingBitmapsIsFlipped[currentDuckingAnimationFrame] = false;
            }
            else if (!DuckingBitmapsIsFlipped[currentDuckingAnimationFrame] && IsFacingLeft)
            {
                DuckingBitmaps[currentDuckingAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                DuckingBitmapsIsFlipped[currentDuckingAnimationFrame] = true;
            }

            CurrentBitmap = DuckingBitmaps[currentDuckingAnimationFrame];
            
            ++framesElapsedWhileDucking;            
        }
        private void PerformLookingUpAnimation()
        {
            if (framesElapsedWhileLookingUp > 2)
            {
                framesElapsedWhileLookingUp = 0;
                ++currentLookingUpAnimationFrame;
                if (currentLookingUpAnimationFrame > 1)
                    currentLookingUpAnimationFrame = 1;
            }
            
            if (LookingUpBitmapsIsFlipped[currentLookingUpAnimationFrame] && !IsFacingLeft)
            {
                LookingUpBitmaps[currentLookingUpAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                LookingUpBitmapsIsFlipped[currentLookingUpAnimationFrame] = false;
            }
            else if (!LookingUpBitmapsIsFlipped[currentLookingUpAnimationFrame] && IsFacingLeft)
            {
                LookingUpBitmaps[currentLookingUpAnimationFrame].RotateFlip(RotateFlipType.RotateNoneFlipX);
                LookingUpBitmapsIsFlipped[currentLookingUpAnimationFrame] = true;
            }

            CurrentBitmap = LookingUpBitmaps[currentLookingUpAnimationFrame];
            
            ++framesElapsedWhileLookingUp;
        }
        private void PerformStandingAnimation()
        {
            if (StandingBitmapIsFlipped && !IsFacingLeft)
            {
                StandingBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                StandingBitmapIsFlipped = false;
            }
            else if (!StandingBitmapIsFlipped && IsFacingLeft)
            {
                StandingBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                StandingBitmapIsFlipped = true;
            }

            CurrentBitmap = StandingBitmap;
        }
        private void PerformHitAnimation()
        {
            if (HitBitmapIsFlipped && !IsFacingLeft)
            {
                HitBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                HitBitmapIsFlipped = false;
            }
            else if (!HitBitmapIsFlipped && IsFacingLeft)
            {
                HitBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                HitBitmapIsFlipped = true;
            }

            CurrentBitmap = HitBitmap;
        }
        private void PerformBouncingOffSpringAnimation()
        {
            if (GameForm.RightIsPressed)
                IsFacingLeft = false;
            else if (GameForm.LeftIsPressed)
                IsFacingLeft = true;
            
            if (BouncingOffSpringBitmapIsFlipped && !IsFacingLeft)
            {
                BouncingOffSpringBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                BouncingOffSpringBitmapIsFlipped = false;
            }
            else if (!BouncingOffSpringBitmapIsFlipped && IsFacingLeft)
            {
                BouncingOffSpringBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                BouncingOffSpringBitmapIsFlipped = true;
            }
            CurrentBitmap = BouncingOffSpringBitmap;
        }

        private void ResetWalkingAnimationVariables()        //the following ones can be made into a single function
        {
            currentWalkingAnimationFrame = 0;
            framesElapsedWhileWalking = 0;
        }
        private void ResetRunningAnimationVariables()
        {
            currentRunningAnimationFrame = 0;
            framesElapsedWhileRunning = 0;
        }
        private void ResetRollingAnimationVariables()
        {
            currentRollingAnimationFrame = 0;
            framesElapsedWhileRolling = 0;
        }
        private void ResetDuckingAnimationVariables()
        {
            currentDuckingAnimationFrame = 0;
            framesElapsedWhileDucking = 0;
        }
        private void ResetLookingUpAnimationVariables()
        {
            currentLookingUpAnimationFrame = 0;
            framesElapsedWhileLookingUp = 0;
        }
    }
}