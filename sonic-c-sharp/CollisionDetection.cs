using System;
using System.Collections.Generic;
using System.Drawing;

namespace sonic_c_sharp
{
    public static class CollisionDetection
    {
        public static void PerformCollisionDetection()
        {
            var collisionDetected = false;
            
            ResetSonicCollisionBoxes();
            GameState.LinkToSonicObject.EnemyCollisionDetected = false;
            GameState.LinkToSonicObject.SpikesCollisionDetected = false;

            ResetMotobugCollisionBoxes();
            
            foreach (var gameObject1 in GameState.ObjectList)
                foreach (var gameObject2 in GameState.ObjectList)
                {
                    if (!(Math.Abs(gameObject1.X - gameObject2.X) < 450 && Math.Abs(gameObject1.Y - gameObject2.Y) < 450)) //reducing amount of checks
                        continue;
                    //if (gameObject1.IsCollidable && gameObject2.IsCollidable)    //slightly(?) reducing amount of checks        BROKEN BECAUSE NOT ALL OBJECTS HAVE ISCOLLIDABLE SET PROPERLY
                    {
                        if (gameObject1 is SonicObject && gameObject2 is TileObject && !(gameObject2 is YellowSpringObject) && !GameForm.ZeroIsPressed)
                        {
                            var Sonic = (SonicObject) gameObject1;
                            var Tile = (TileObject) gameObject2;
                            
                            if (Tile.IsCollidable)
                            {
                                var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                           new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                                SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                                SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                                SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                                SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                                
                                //var TileBigAABB = Tile.BigAABB;
                                var TileBigAABB = new [] {new Point (Tile.BigAABB[0].X, Tile.BigAABB[0].Y),
                                                          new Point (Tile.BigAABB[1].X, Tile.BigAABB[1].Y)};
                                TileBigAABB[0].X = TileBigAABB[0].X + Tile.X;
                                TileBigAABB[1].X = TileBigAABB[1].X + Tile.X;
                                TileBigAABB[0].Y = TileBigAABB[0].Y + Tile.Y;
                                TileBigAABB[1].Y = TileBigAABB[1].Y + Tile.Y;
    
                                if (CheckBroadPhase(SonicBigAABB, TileBigAABB) && !Sonic.IsLosingLife)
                                {
                                    CheckNarrowPhase(Sonic, Tile);
                                }
                            }
                        }
                        if (gameObject1 is SonicObject && gameObject2 is BadnikSimpleObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var BadnikSimple = (BadnikSimpleObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            //only BadnikSimple.AABB[0][..] is supposed to be used;
                            var BadnikSimpleHitboxAABB = new [] {new Point (BadnikSimple.AABB[0][0].X, BadnikSimple.AABB[0][0].Y),
                                                                 new Point (BadnikSimple.AABB[0][1].X, BadnikSimple.AABB[0][1].Y)};
                            BadnikSimpleHitboxAABB[0].X = BadnikSimpleHitboxAABB[0].X + BadnikSimple.X;
                            BadnikSimpleHitboxAABB[1].X = BadnikSimpleHitboxAABB[1].X + BadnikSimple.X;
                            BadnikSimpleHitboxAABB[0].Y = BadnikSimpleHitboxAABB[0].Y + BadnikSimple.Y;
                            BadnikSimpleHitboxAABB[1].Y = BadnikSimpleHitboxAABB[1].Y + BadnikSimple.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, BadnikSimpleHitboxAABB) && !Sonic.IsLosingLife)
                            {
                                if ((Sonic.IsJumping || Sonic.isRolling) && !Sonic.IsBouncingOffSpring)
                                {
                                    GameState.ObjectsToRemove.Add(gameObject2);
                                    GameState.ObjectsToAdd.Add(new ExplosionObject(BadnikSimple.X + BadnikSimple.CurrentBitmap.Width/3, BadnikSimple.Y));
                                    if (Sonic.Y < BadnikSimple.Y && Sonic.YSpeed > 0)
                                        Sonic.ShouldBounceOffEnemy = true;
                                    
                                    Sounds.ExplosionSound.Play();
                                }
                                else if (!Sonic.IsInvincible)
                                {
                                    Sonic.EnemyCollisionDetected = true;
                                    if (Sonic.X + 21 <= BadnikSimple.X + BadnikSimple.CurrentBitmap.Width / 2)
                                        Sonic.HitFromRight = true;
                                    else
                                        Sonic.HitFromRight = false;
                                }
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is BadnikFishObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var BadnikFish = (BadnikFishObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            //only BadnikFish.AABB[0][..] is supposed to be used;
                            var BadnikFishHitboxAABB = new [] {new Point (BadnikFish.AABB[0].X, BadnikFish.AABB[0].Y),
                                                               new Point (BadnikFish.AABB[1].X, BadnikFish.AABB[1].Y)};
                            BadnikFishHitboxAABB[0].X = BadnikFishHitboxAABB[0].X + BadnikFish.X;
                            BadnikFishHitboxAABB[1].X = BadnikFishHitboxAABB[1].X + BadnikFish.X;
                            BadnikFishHitboxAABB[0].Y = BadnikFishHitboxAABB[0].Y + BadnikFish.Y;
                            BadnikFishHitboxAABB[1].Y = BadnikFishHitboxAABB[1].Y + BadnikFish.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, BadnikFishHitboxAABB) && !Sonic.IsLosingLife)
                            {
                                if ((Sonic.IsJumping || Sonic.isRolling) && !Sonic.IsBouncingOffSpring)
                                {
                                    GameState.ObjectsToRemove.Add(gameObject2);
                                    GameState.ObjectsToAdd.Add(new ExplosionObject(BadnikFish.X + BadnikFish.CurrentBitmap.Width/3, BadnikFish.Y));
                                    if (Sonic.Y < BadnikFish.Y && Sonic.YSpeed > 0)
                                        Sonic.ShouldBounceOffEnemy = true;
                                    
                                    Sounds.ExplosionSound.Play();
                                }
                                else if (!Sonic.IsInvincible)
                                {
                                    Sonic.EnemyCollisionDetected = true;
                                    if (Sonic.X + 21 <= BadnikFish.X + BadnikFish.CurrentBitmap.Width / 2)
                                        Sonic.HitFromRight = true;
                                    else
                                        Sonic.HitFromRight = false;
                                }
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is BadnikMotobugObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var BadnikMotobug = (BadnikMotobugObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            //only BadnikSimple.AABB[0][..] is supposed to be used;
                            var BadnikMotobugHitboxAABB = new [] {new Point (BadnikMotobug.AABB[0][0].X, BadnikMotobug.AABB[0][0].Y),
                                                                  new Point (BadnikMotobug.AABB[0][1].X, BadnikMotobug.AABB[0][1].Y)};
                            BadnikMotobugHitboxAABB[0].X = BadnikMotobugHitboxAABB[0].X + BadnikMotobug.X;
                            BadnikMotobugHitboxAABB[1].X = BadnikMotobugHitboxAABB[1].X + BadnikMotobug.X;
                            BadnikMotobugHitboxAABB[0].Y = BadnikMotobugHitboxAABB[0].Y + BadnikMotobug.Y;
                            BadnikMotobugHitboxAABB[1].Y = BadnikMotobugHitboxAABB[1].Y + BadnikMotobug.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, BadnikMotobugHitboxAABB) && !Sonic.IsLosingLife)
                            {
                                if ((Sonic.IsJumping || Sonic.isRolling) && !Sonic.IsBouncingOffSpring)
                                {
                                    GameState.ObjectsToRemove.Add(gameObject2);
                                    GameState.MotobugsToRemove.Add(gameObject2);
                                    GameState.ObjectsToAdd.Add(new ExplosionObject(BadnikMotobug.X + BadnikMotobug.CurrentBitmap.Width/3, BadnikMotobug.Y));
                                    if (Sonic.Y < BadnikMotobug.Y && Sonic.YSpeed > 0)
                                        Sonic.ShouldBounceOffEnemy = true;
                                    
                                    Sounds.ExplosionSound.Play();
                                }
                                else if (!Sonic.IsInvincible)
                                {
                                    Sonic.EnemyCollisionDetected = true;
                                    if (Sonic.X + 21 <= BadnikMotobug.X + BadnikMotobug.CurrentBitmap.Width / 2)
                                        Sonic.HitFromRight = true;
                                    else
                                        Sonic.HitFromRight = false;
                                }
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is RingObject)
                        {
                            var Sonic = (SonicObject) gameObject1;
                            var Ring = (RingObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var RingAABB = new [] { new Point (Ring.AABB[0].X, Ring.AABB[0].Y),
                                                    new Point (Ring.AABB[1].X, Ring.AABB[1].Y) };
                            RingAABB[0].X = RingAABB[0].X + Ring.X;
                            RingAABB[1].X = RingAABB[1].X + Ring.X;
                            RingAABB[0].Y = RingAABB[0].Y + Ring.Y;
                            RingAABB[1].Y = RingAABB[1].Y + Ring.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, RingAABB) && !Sonic.IsLosingLife && Sonic.InvincibilityFramesLeft < 40)//30)
                            {
                                GameState.ObjectsToRemove.Add(gameObject2);

                                if (gameObject2 is BlueRingObject)
                                {
                                    GameState.ObjectsToAdd.Add(new BlueRingSparklesObject(Ring.X, Ring.Y));
                                    Sonic.Rings += 10;
                                    
                                    Sounds.PlayRingCollectingSound();
                                }
                                else
                                {
                                    GameState.ObjectsToAdd.Add(new RingSparklesObject(Ring.X, Ring.Y));
                                    ++Sonic.Rings;
                                    
                                    Sounds.PlayRingCollectingSound();
                                }
                            }
                        }
                        if (gameObject1 is SonicObject && gameObject2 is RingDroppedObject)
                        {
                            var Sonic = (SonicObject) gameObject1;
                            var RingDropped = (RingDroppedObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var RingDroppedAABB = new [] { new Point (RingDropped.AABB[0].X, RingDropped.AABB[0].Y),
                                new Point (RingDropped.AABB[1].X, RingDropped.AABB[1].Y) };
                            RingDroppedAABB[0].X = RingDroppedAABB[0].X + RingDropped.X;
                            RingDroppedAABB[1].X = RingDroppedAABB[1].X + RingDropped.X;
                            RingDroppedAABB[0].Y = RingDroppedAABB[0].Y + RingDropped.Y;
                            RingDroppedAABB[1].Y = RingDroppedAABB[1].Y + RingDropped.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, RingDroppedAABB) && !Sonic.IsLosingLife && Sonic.InvincibilityFramesLeft < 40)//30)
                            {
                                ++Sonic.Rings;
                                GameState.ObjectsToRemove.Add(gameObject2);
                                GameState.ObjectsToAdd.Add(new RingSparklesObject(RingDropped.X, RingDropped.Y));

                                Sounds.PlayRingCollectingSound();
                            }
                        }
                        
                        if (gameObject1 is RingDroppedObject && gameObject2 is TileObject)
                        {
                            //this would be a disaster with slopes, but there are no slopes in this demo so whatever
                            
                            var RingDropped = (RingDroppedObject) gameObject1;
                            var Tile = (TileObject) gameObject2;

                            if (RingDropped.DontCheckTileCollisionsTimer > 0)        //an ugly hack, see comment below for more info
                                --RingDropped.DontCheckTileCollisionsTimer;
                            else if (Tile.IsCollidable)
                            {
                                var RingDroppedAABB = new [] { new Point (RingDropped.AABB[0].X, RingDropped.AABB[0].Y),
                                    new Point (RingDropped.AABB[1].X, RingDropped.AABB[1].Y) };
                                RingDroppedAABB[0].X = RingDroppedAABB[0].X + RingDropped.X;
                                RingDroppedAABB[1].X = RingDroppedAABB[1].X + RingDropped.X;
                                RingDroppedAABB[0].Y = RingDroppedAABB[0].Y + RingDropped.Y;
                                RingDroppedAABB[1].Y = RingDroppedAABB[1].Y + RingDropped.Y;
                                
                                //var TileBigAABB = Tile.BigAABB;
                                var TileBigAABB = new [] {new Point (Tile.BigAABB[0].X, Tile.BigAABB[0].Y),
                                                          new Point (Tile.BigAABB[1].X, Tile.BigAABB[1].Y)};
                                TileBigAABB[0].X = TileBigAABB[0].X + Tile.X;
                                TileBigAABB[1].X = TileBigAABB[1].X + Tile.X;
                                TileBigAABB[0].Y = TileBigAABB[0].Y + Tile.Y;
                                TileBigAABB[1].Y = TileBigAABB[1].Y + Tile.Y;
    
                                //broad phase check
                                if (CheckBroadPhase(RingDroppedAABB, TileBigAABB))
                                {
                                    RingDropped.YSpeed *= -0.75f;
                                    RingDropped.DontCheckTileCollisionsTimer = 500;        //an ugly(?) attempt at not having rings stuck inside tiles
                                }
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is InvisibleDamagingObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var InvisibleDamaging = (InvisibleDamagingObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            //only InvisibleDamaging.AABB[0][..] is supposed to be used;
                            var InvisibleDamagingAABB = new [] {new Point (InvisibleDamaging.AABB[0][0].X, InvisibleDamaging.AABB[0][0].Y),
                                                                      new Point (InvisibleDamaging.AABB[0][1].X, InvisibleDamaging.AABB[0][1].Y)};
                            InvisibleDamagingAABB[0].X = InvisibleDamagingAABB[0].X + InvisibleDamaging.X;
                            InvisibleDamagingAABB[1].X = InvisibleDamagingAABB[1].X + InvisibleDamaging.X;
                            InvisibleDamagingAABB[0].Y = InvisibleDamagingAABB[0].Y + InvisibleDamaging.Y;
                            InvisibleDamagingAABB[1].Y = InvisibleDamagingAABB[1].Y + InvisibleDamaging.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, InvisibleDamagingAABB) && !Sonic.IsLosingLife && !Sonic.IsInvincible)
                            {
                                Sonic.EnemyCollisionDetected = true;
                                Sonic.SpikesCollisionDetected = true;
                                if (Sonic.X + 21 <= InvisibleDamaging.X + InvisibleDamaging.CurrentBitmap.Width / 2)
                                    Sonic.HitFromRight = true;
                                else
                                    Sonic.HitFromRight = false;
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is LavaObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var Lava = (LavaObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            //only InvisibleDamaging.AABB[0][..] is supposed to be used;
                            var LavaAABB = new [] {new Point (Lava.AABB[0].X, Lava.AABB[0].Y),
                                                   new Point (Lava.AABB[1].X, Lava.AABB[1].Y)};
                            LavaAABB[0].X = LavaAABB[0].X + Lava.X;
                            LavaAABB[1].X = LavaAABB[1].X + Lava.X;
                            LavaAABB[0].Y = LavaAABB[0].Y + Lava.Y;
                            LavaAABB[1].Y = LavaAABB[1].Y + Lava.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, LavaAABB) && !Sonic.IsLosingLife && !Sonic.IsInvincible)
                            {
                                Sonic.EnemyCollisionDetected = true;
                                if (Sonic.X + 21 <= Lava.X + Lava.CurrentBitmap.Width / 2)
                                    Sonic.HitFromRight = true;
                                else
                                    Sonic.HitFromRight = false;
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is SpikeBallSmallObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var SpikeBallSmall = (SpikeBallSmallObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var SpikeBallSmallAABB = new [] {new Point (SpikeBallSmall.AABB[0].X, SpikeBallSmall.AABB[0].Y),
                                                   new Point (SpikeBallSmall.AABB[1].X, SpikeBallSmall.AABB[1].Y)};
                            SpikeBallSmallAABB[0].X = SpikeBallSmallAABB[0].X + SpikeBallSmall.X;
                            SpikeBallSmallAABB[1].X = SpikeBallSmallAABB[1].X + SpikeBallSmall.X;
                            SpikeBallSmallAABB[0].Y = SpikeBallSmallAABB[0].Y + SpikeBallSmall.Y;
                            SpikeBallSmallAABB[1].Y = SpikeBallSmallAABB[1].Y + SpikeBallSmall.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, SpikeBallSmallAABB) && !Sonic.IsLosingLife && !Sonic.IsInvincible)
                            {
                                Sonic.EnemyCollisionDetected = true;
                                if (Sonic.X + 21 <= SpikeBallSmall.X + SpikeBallSmall.CurrentBitmap.Width / 2)
                                    Sonic.HitFromRight = true;
                                else
                                    Sonic.HitFromRight = false;
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is SpikeBallBigObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var SpikeBallBig = (SpikeBallBigObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var SpikeBallBigAABB = new [] {new Point (SpikeBallBig.AABB[0].X, SpikeBallBig.AABB[0].Y),
                                                   new Point (SpikeBallBig.AABB[1].X, SpikeBallBig.AABB[1].Y)};
                            SpikeBallBigAABB[0].X = SpikeBallBigAABB[0].X + SpikeBallBig.X;
                            SpikeBallBigAABB[1].X = SpikeBallBigAABB[1].X + SpikeBallBig.X;
                            SpikeBallBigAABB[0].Y = SpikeBallBigAABB[0].Y + SpikeBallBig.Y;
                            SpikeBallBigAABB[1].Y = SpikeBallBigAABB[1].Y + SpikeBallBig.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, SpikeBallBigAABB) && !Sonic.IsLosingLife && !Sonic.IsInvincible)
                            {
                                Sonic.EnemyCollisionDetected = true;
                                if (Sonic.X + 21 <= SpikeBallBig.X + SpikeBallBig.CurrentBitmap.Width / 2)
                                    Sonic.HitFromRight = true;
                                else
                                    Sonic.HitFromRight = false;
                            }
                        }
                        if (gameObject1 is SonicObject && gameObject2 is YellowSpringObject)
                        {
                            
                            var Sonic = (SonicObject) gameObject1;
                            var YellowSpring = (YellowSpringObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var YellowSpringSolidAABB = new [] {new Point (YellowSpring.SolidAABB[0].X, YellowSpring.SolidAABB[0].Y),
                                                                new Point (YellowSpring.SolidAABB[1].X, YellowSpring.SolidAABB[1].Y)};
                            YellowSpringSolidAABB[0].X = YellowSpringSolidAABB[0].X + YellowSpring.X;
                            YellowSpringSolidAABB[1].X = YellowSpringSolidAABB[1].X + YellowSpring.X;
                            YellowSpringSolidAABB[0].Y = YellowSpringSolidAABB[0].Y + YellowSpring.Y;
                            YellowSpringSolidAABB[1].Y = YellowSpringSolidAABB[1].Y + YellowSpring.Y;
                                
                            if (CheckBroadPhase(SonicBigAABB, YellowSpringSolidAABB) && !Sonic.IsLosingLife)
                            {
                                CheckNarrowPhase(Sonic, (TileObject)YellowSpring);
                            }
                            
                            var YellowSpringBouncingAABB = new [] {new Point (YellowSpring.BouncingAABB[0].X, YellowSpring.BouncingAABB[0].Y),
                                                           new Point (YellowSpring.BouncingAABB[1].X, YellowSpring.BouncingAABB[1].Y)};
                            YellowSpringBouncingAABB[0].X = YellowSpringBouncingAABB[0].X + YellowSpring.X;
                            YellowSpringBouncingAABB[1].X = YellowSpringBouncingAABB[1].X + YellowSpring.X;
                            YellowSpringBouncingAABB[0].Y = YellowSpringBouncingAABB[0].Y + YellowSpring.Y;
                            YellowSpringBouncingAABB[1].Y = YellowSpringBouncingAABB[1].Y + YellowSpring.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, YellowSpringBouncingAABB) && !Sonic.IsLosingLife)
                            {
                                Sonic.IsBouncingOffSpring = true;
                                Sonic.YSpeed = -10*2;
                                
                                YellowSpring.IsActivated = true;
                                
                                Sounds.SpringSound.Play();
                            }
                        }
                        
                        if (gameObject1 is BadnikMotobugObject && gameObject2 is TileObject)    // && !(gameObject2 is YellowSpringObject))
                        {
                            //this would've been a bad solution if we had slopes, but there are no slopes in this demo so whatever
                            var BadnikMotobug = (BadnikMotobugObject) gameObject1;
                            var Tile = (TileObject) gameObject2;
                            
                            if (Tile.IsCollidable)
                            {
                                var BadnikMotobugBigAABB = new [] {new Point (BadnikMotobug.AABB[0][0].X, BadnikMotobug.AABB[0][0].Y),
                                                                   new Point (BadnikMotobug.AABB[0][1].X, BadnikMotobug.AABB[0][1].Y)};
                                BadnikMotobugBigAABB[0].X = BadnikMotobugBigAABB[0].X + BadnikMotobug.X;
                                BadnikMotobugBigAABB[1].X = BadnikMotobugBigAABB[1].X + BadnikMotobug.X;
                                BadnikMotobugBigAABB[0].Y = BadnikMotobugBigAABB[0].Y + BadnikMotobug.Y;
                                BadnikMotobugBigAABB[1].Y = BadnikMotobugBigAABB[1].Y + BadnikMotobug.Y;
                                
                                //var TileBigAABB = Tile.BigAABB;
                                var TileBigAABB = new [] {new Point (Tile.BigAABB[0].X, Tile.BigAABB[0].Y),
                                                          new Point (Tile.BigAABB[1].X, Tile.BigAABB[1].Y)};
                                TileBigAABB[0].X = TileBigAABB[0].X + Tile.X;
                                TileBigAABB[1].X = TileBigAABB[1].X + Tile.X;
                                TileBigAABB[0].Y = TileBigAABB[0].Y + Tile.Y;
                                TileBigAABB[1].Y = TileBigAABB[1].Y + Tile.Y;
    
                                if (CheckBroadPhase(BadnikMotobugBigAABB, TileBigAABB))
                                {
                                    var GroundLeftAABB = new [] {new Point (BadnikMotobug.GroundLeftCollisionBox[0].X, BadnikMotobug.GroundLeftCollisionBox[0].Y),
                                                                 new Point (BadnikMotobug.GroundLeftCollisionBox[1].X, BadnikMotobug.GroundLeftCollisionBox[1].Y)};
                                    GroundLeftAABB[0].X = GroundLeftAABB[0].X + BadnikMotobug.X;
                                    GroundLeftAABB[1].X = GroundLeftAABB[1].X + BadnikMotobug.X;
                                    GroundLeftAABB[0].Y = GroundLeftAABB[0].Y + BadnikMotobug.Y;
                                    GroundLeftAABB[1].Y = GroundLeftAABB[1].Y + BadnikMotobug.Y;
                                    
                                    var GroundRightAABB = new [] {new Point (BadnikMotobug.GroundRightCollisionBox[0].X, BadnikMotobug.GroundRightCollisionBox[0].Y),
                                                                  new Point (BadnikMotobug.GroundRightCollisionBox[1].X, BadnikMotobug.GroundRightCollisionBox[1].Y)};
                                    GroundRightAABB[0].X = GroundRightAABB[0].X + BadnikMotobug.X;
                                    GroundRightAABB[1].X = GroundRightAABB[1].X + BadnikMotobug.X;
                                    GroundRightAABB[0].Y = GroundRightAABB[0].Y + BadnikMotobug.Y;
                                    GroundRightAABB[1].Y = GroundRightAABB[1].Y + BadnikMotobug.Y;
                                    
                                    var WallLeftAABB = new [] {new Point (BadnikMotobug.WallLeftCollisionBox[0].X, BadnikMotobug.WallLeftCollisionBox[0].Y),
                                                               new Point (BadnikMotobug.WallLeftCollisionBox[1].X, BadnikMotobug.WallLeftCollisionBox[1].Y)};
                                    WallLeftAABB[0].X = WallLeftAABB[0].X + BadnikMotobug.X;
                                    WallLeftAABB[1].X = WallLeftAABB[1].X + BadnikMotobug.X;
                                    WallLeftAABB[0].Y = WallLeftAABB[0].Y + BadnikMotobug.Y;
                                    WallLeftAABB[1].Y = WallLeftAABB[1].Y + BadnikMotobug.Y;
                                    
                                    var WallRightAABB = new [] {new Point (BadnikMotobug.WallRightCollisionBox[0].X, BadnikMotobug.WallRightCollisionBox[0].Y),
                                                                new Point (BadnikMotobug.WallRightCollisionBox[1].X, BadnikMotobug.WallRightCollisionBox[1].Y)};
                                    WallRightAABB[0].X = WallRightAABB[0].X + BadnikMotobug.X;
                                    WallRightAABB[1].X = WallRightAABB[1].X + BadnikMotobug.X;
                                    WallRightAABB[0].Y = WallRightAABB[0].Y + BadnikMotobug.Y;
                                    WallRightAABB[1].Y = WallRightAABB[1].Y + BadnikMotobug.Y;
                                    
                                    if (CheckBroadPhase(GroundLeftAABB, TileBigAABB))
                                        BadnikMotobug.GroundLeftIsActive = true;
                                    if (CheckBroadPhase(GroundRightAABB, TileBigAABB))
                                        BadnikMotobug.GroundRightIsActive = true;
                                    if (CheckBroadPhase(WallLeftAABB, TileBigAABB))
                                        BadnikMotobug.WallLeftIsActive = true;
                                    if (CheckBroadPhase(WallRightAABB, TileBigAABB))
                                        BadnikMotobug.WallRightIsActive = true;
                                }
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is ConveyorBeltObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var ConveyorBelt = (ConveyorBeltObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var ConveyorBeltAABB = new [] {new Point (ConveyorBelt.AABB[0].X, ConveyorBelt.AABB[0].Y),
                                                           new Point (ConveyorBelt.AABB[1].X, ConveyorBelt.AABB[1].Y)};
                            ConveyorBeltAABB[0].X = ConveyorBeltAABB[0].X + ConveyorBelt.X;
                            ConveyorBeltAABB[1].X = ConveyorBeltAABB[1].X + ConveyorBelt.X;
                            ConveyorBeltAABB[0].Y = ConveyorBeltAABB[0].Y + ConveyorBelt.Y;
                            ConveyorBeltAABB[1].Y = ConveyorBeltAABB[1].Y + ConveyorBelt.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, ConveyorBeltAABB) && !Sonic.IsLosingLife)
                            {
                                ConveyorBelt.IsCollidingWithSonic = true;
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is ClosingDoorObject)
                        {
                            
                            var Sonic = (SonicObject) gameObject1;
                            var ClosingDoor = (ClosingDoorObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var ClosingDoorAABB = new [] {new Point (ClosingDoor.AABBs[0][0].X, ClosingDoor.AABBs[0][0].Y),
                                                          new Point (ClosingDoor.AABBs[0][1].X, ClosingDoor.AABBs[0][1].Y)};
                            ClosingDoorAABB[0].X = ClosingDoorAABB[0].X + ClosingDoor.X;
                            ClosingDoorAABB[1].X = ClosingDoorAABB[1].X + ClosingDoor.X;
                            ClosingDoorAABB[0].Y = ClosingDoorAABB[0].Y + ClosingDoor.Y;
                            ClosingDoorAABB[1].Y = ClosingDoorAABB[1].Y + ClosingDoor.Y;
                                
                            if (CheckBroadPhase(SonicBigAABB, ClosingDoorAABB) && !Sonic.IsLosingLife)
                            {
                                CheckNarrowPhase(Sonic, (TileObject)ClosingDoor);
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is MechaSonicObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var MechaSonic = (MechaSonicObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            var MechaSonicHitboxAABB = new [] {new Point (MechaSonic.CurrentAABB[0].X, MechaSonic.CurrentAABB[0].Y),
                                                               new Point (MechaSonic.CurrentAABB[1].X, MechaSonic.CurrentAABB[1].Y)};
                            MechaSonicHitboxAABB[0].X = MechaSonicHitboxAABB[0].X + MechaSonic.X;
                            MechaSonicHitboxAABB[1].X = MechaSonicHitboxAABB[1].X + MechaSonic.X;
                            MechaSonicHitboxAABB[0].Y = MechaSonicHitboxAABB[0].Y + MechaSonic.Y;
                            MechaSonicHitboxAABB[1].Y = MechaSonicHitboxAABB[1].Y + MechaSonic.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, MechaSonicHitboxAABB) && !Sonic.IsLosingLife)
                            {
                                if (!MechaSonic.IsInvincible)
                                {
                                    if ((Sonic.IsJumping || Sonic.isRolling) && !Sonic.IsBouncingOffSpring && !MechaSonic.IsSpinning)
                                    {
                                        --MechaSonic.HitPoints;
                                        MechaSonic.InvincibilityFramesLeft = 15;
                                        
                                        //bouncing off
                                        Sonic.GroundSpeed *= -1;
                                        Sonic.XSpeed *= -1;
                                        Sonic.YSpeed *= -1;
                                        
                                        Sounds.BossHitSound.Play();
                                    }
                                    else if (!Sonic.IsInvincible)
                                    {
                                        Sonic.EnemyCollisionDetected = true;
                                        if (Sonic.X + 21 <= MechaSonic.X + MechaSonic.CurrentBitmap.Width / 2)
                                            Sonic.HitFromRight = true;
                                        else
                                            Sonic.HitFromRight = false;
                                    }
                                }
                            }
                        }
                        
                        if (gameObject1 is SonicObject && gameObject2 is EnergyBallObject)
                        {   
                            var Sonic = (SonicObject) gameObject1;
                            var EnergyBall = (EnergyBallObject) gameObject2;
                            var SonicBigAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[0].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[0].Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.LargeAABB[1].X, Sonic.CurrentCollisionBoxesSet.LargeAABB[1].Y)};
                            SonicBigAABB[0].X = SonicBigAABB[0].X + Sonic.X;
                            SonicBigAABB[1].X = SonicBigAABB[1].X + Sonic.X;
                            SonicBigAABB[0].Y = SonicBigAABB[0].Y + Sonic.Y;
                            SonicBigAABB[1].Y = SonicBigAABB[1].Y + Sonic.Y;
                            
                            //only EnergyBall.AABB[0][..] is supposed to be used;
                            var EnergyBallAABB = new [] {new Point (EnergyBall.AABB[0].X, EnergyBall.AABB[0].Y),
                                                         new Point (EnergyBall.AABB[1].X, EnergyBall.AABB[1].Y)};
                            EnergyBallAABB[0].X = EnergyBallAABB[0].X + EnergyBall.X;
                            EnergyBallAABB[1].X = EnergyBallAABB[1].X + EnergyBall.X;
                            EnergyBallAABB[0].Y = EnergyBallAABB[0].Y + EnergyBall.Y;
                            EnergyBallAABB[1].Y = EnergyBallAABB[1].Y + EnergyBall.Y;

                            //broad phase check
                            if (CheckBroadPhase(SonicBigAABB, EnergyBallAABB) && !Sonic.IsLosingLife && !Sonic.IsInvincible)
                            {
                                Sonic.EnemyCollisionDetected = true;
                                if (Sonic.X + 21 <= EnergyBall.X + EnergyBall.CurrentBitmap.Width / 2)
                                    Sonic.HitFromRight = true;
                                else
                                    Sonic.HitFromRight = false;
                            }
                        }
                        
                        /*
                        if (gameObject1 is SonicObject && gameObject2 is TileJumpableThruObject)
                        {
                        }
                        */
                    }
                }
        }

        public static bool CheckBroadPhase(Point[] box1, Point[] box2)
        {
            var box1Width = box1[1].X - box1[0].X;
            var box2Width = box2[1].X - box2[0].X;
            var box1Height = box1[1].Y - box1[0].Y;
            var box2Height = box2[1].Y - box2[0].Y;
            
            if (box1[0].X < box2[0].X + box2Width &&
                box1[0].X + box1Width > box2[0].X &&
                box1[0].Y < box2[0].Y + box2Height &&
                box1[0].Y + box1Height > box2[0].Y)
                {
                    return true;
                }
    
            return false;
        }
        
        public static void CheckNarrowPhase(SonicObject Sonic, TileObject Tile)
        {
            //perhaps a way to make it look less ugly:
            //foreach aabb in sonic.currentcollisionboxesset add to list a new [] {new Point (bla bla + Sonic.X, etc) etc}
            
            var SonicHeadLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].Y + Sonic.Y),
                                                  new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].Y + Sonic.Y)};
            var SonicHeadRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].Y + Sonic.Y),
                                                   new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].Y + Sonic.Y)};
            
            var SonicWallTopLeftAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallTopLeft[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallTopLeft[0].Y + Sonic.Y),
                                               new Point (Sonic.CurrentCollisionBoxesSet.WallTopLeft[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallTopLeft[1].Y + Sonic.Y)};
            var SonicWallBottomLeftAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallBottomLeft[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallBottomLeft[0].Y + Sonic.Y),
                                                  new Point (Sonic.CurrentCollisionBoxesSet.WallBottomLeft[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallBottomLeft[1].Y + Sonic.Y)};            
            var SonicWallLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].Y + Sonic.Y),
                                                  new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].Y + Sonic.Y)};
            
            var SonicWallTopRightAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallTopRight[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallTopRight[0].Y + Sonic.Y),
                                                new Point (Sonic.CurrentCollisionBoxesSet.WallTopRight[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallTopRight[1].Y + Sonic.Y)};
            var SonicWallBottomRightAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallBottomRight[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallBottomRight[0].Y + Sonic.Y),
                                                   new Point (Sonic.CurrentCollisionBoxesSet.WallBottomRight[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallBottomRight[1].Y + Sonic.Y)};
            var SonicWallRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].Y + Sonic.Y),
                                                   new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].Y + Sonic.Y)};
            
            var SonicFootLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].Y + Sonic.Y),
                                                  new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].Y + Sonic.Y)};
            var SonicFootLeft_OuterAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Outer[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Outer[0].Y + Sonic.Y),
                                                  new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Outer[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Outer[1].Y + Sonic.Y)};
            var SonicFootLeftBalancing_OuterAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootLeftBalancing_Outer[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeftBalancing_Outer[0].Y + Sonic.Y),
                                                           new Point (Sonic.CurrentCollisionBoxesSet.FootLeftBalancing_Outer[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeftBalancing_Outer[1].Y + Sonic.Y)};
            
            var SonicFootRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].Y + Sonic.Y),
                                                   new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].Y + Sonic.Y)};
            var SonicFootRight_OuterAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Outer[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Outer[0].Y + Sonic.Y),
                                                   new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Outer[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Outer[1].Y + Sonic.Y)};
            var SonicFootRightBalancing_OuterAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootRightBalancing_Outer[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRightBalancing_Outer[0].Y + Sonic.Y),
                                                            new Point (Sonic.CurrentCollisionBoxesSet.FootRightBalancing_Outer[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRightBalancing_Outer[1].Y + Sonic.Y)};

            var deltaX = Sonic.X - Sonic.PrevX;
            //Console.WriteLine("deltaX:" + deltaX + " X:" + Sonic.X + " - PrevX:" + Sonic.PrevX);
            var deltaY = -(Sonic.Y - Sonic.PrevY);    //screwed up the math a little and thus have to do this
            //Console.WriteLine("deltaY:" + deltaY + " Y:" + Sonic.Y + " - PrevY:" + Sonic.PrevY);
            
            foreach (var TileAABB in Tile.AABBs)
            {
                //Console.WriteLine("Initial: " + Sonic.X + "," + Sonic.Y);
                
                var CurrentTileAABB = new [] {new Point (TileAABB[0].X + Tile.X, TileAABB[0].Y + Tile.Y),
                                              new Point (TileAABB[1].X + Tile.X, TileAABB[1].Y + Tile.Y)};


                //for fixing sonic's position when stuck in a corner of a tile 
                var IWasActivated = false;
                var IIWasActivated = false;
                var IIIWasActivated = false;
                var IVWasActivated = false;
                
                while (CheckBroadPhase(SonicHeadLeft_InnerAABB, CurrentTileAABB) && CheckBroadPhase(SonicWallLeft_InnerAABB, CurrentTileAABB) && !CheckBroadPhase(SonicHeadRight_InnerAABB, CurrentTileAABB))
                {
                    IWasActivated = true;
                    
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                        ++Sonic.X;    //move right;
                    if (Math.Abs(deltaX) < Math.Abs(deltaY))
                        ++Sonic.Y;    //move down;
                    if (Math.Abs(deltaX) == Math.Abs(deltaY))
                    {
                        //move right and down;
                        ++Sonic.X;
                        ++Sonic.Y;
                    }

                    SonicHeadLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].Y + Sonic.Y)};
                    SonicWallLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].Y + Sonic.Y)};
                    SonicHeadRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("I");
                }
                while (CheckBroadPhase(SonicHeadRight_InnerAABB, CurrentTileAABB) && CheckBroadPhase(SonicWallRight_InnerAABB, CurrentTileAABB) && !CheckBroadPhase(SonicHeadLeft_InnerAABB, CurrentTileAABB))
                {
                    IIWasActivated = true;
                    
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                        --Sonic.X;    //move left;
                    if (Math.Abs(deltaX) < Math.Abs(deltaY))
                        ++Sonic.Y;    //move down;
                    if (Math.Abs(deltaX) == Math.Abs(deltaY))
                    {
                        //move left and down;
                        --Sonic.X;
                        ++Sonic.Y;
                    }

                    SonicHeadRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].Y + Sonic.Y)};
                    SonicWallRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].Y + Sonic.Y)};
                    SonicHeadLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("II: " + Sonic.X + "," + Sonic.Y);
                }
                while (CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB) && CheckBroadPhase(SonicWallLeft_InnerAABB, CurrentTileAABB) && !CheckBroadPhase(SonicFootRight_InnerAABB, CurrentTileAABB))
                {
                    IIIWasActivated = true;
                    
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                        ++Sonic.X;    //move right;
                    if (Math.Abs(deltaX) < Math.Abs(deltaY))
                        --Sonic.Y;    //move up;
                    if (Math.Abs(deltaX) == Math.Abs(deltaY))
                    {
                        //move right and up;
                        ++Sonic.X;
                        --Sonic.Y;
                    }
                    
                    SonicFootLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].Y + Sonic.Y)};
                    SonicWallLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].Y + Sonic.Y)};
                    SonicFootRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("deltaX " + deltaX);
                    //Console.WriteLine("deltaY " + deltaY);
                    //Console.WriteLine("III: " + Sonic.X + "," + Sonic.Y + " footLeftInner:" + CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB) + " wallleft_inner:" + CheckBroadPhase(SonicWallLeft_InnerAABB, CurrentTileAABB) + " footright_inner:" + CheckBroadPhase(SonicFootRight_InnerAABB, CurrentTileAABB));
                }
                while (CheckBroadPhase(SonicFootRight_InnerAABB, CurrentTileAABB) && CheckBroadPhase(SonicWallRight_InnerAABB, CurrentTileAABB) && !CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB))
                {
                    IVWasActivated = true;
                    
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                        --Sonic.X;    //move left;
                    if (Math.Abs(deltaX) < Math.Abs(deltaY))
                        --Sonic.Y;    //move up;
                    if (Math.Abs(deltaX) == Math.Abs(deltaY))
                    {
                        //move left and up;
                        --Sonic.X;
                        --Sonic.Y;
                    }
                    
                    SonicFootRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].Y + Sonic.Y)};                    
                    SonicWallRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].Y + Sonic.Y)};
                    SonicFootLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("deltaX " + deltaX);
                    //Console.WriteLine("deltaY " + deltaY);
                    //Console.WriteLine("IV: " + Sonic.X + "," + Sonic.Y + " footRightInner:" + CheckBroadPhase(SonicFootRight_InnerAABB, CurrentTileAABB) + " wallright_inner:" + CheckBroadPhase(SonicWallRight_InnerAABB, CurrentTileAABB) + " footleft_inner:" + CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB));
                    //Console.WriteLine("");
                }
                
                /*
                //IF I WAS ACTIVATED, CHECK HEADLEFTINNER AND MOVE UP IF COLLISION
                if (IWasActivated)
                {
                    while (CheckBroadPhase(SonicHeadLeft_InnerAABB, CurrentTileAABB))
                    {
                        --Sonic.Y;
                        SonicHeadLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].Y + Sonic.Y),
                            new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].Y + Sonic.Y)};
                    
                        Console.WriteLine("Ia: " + Sonic.X + "," + Sonic.Y);    // + " " + " footLeftInner:" + CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB), CurrentTileAABB);
                    }
                }
                
                //IF II WAS ACTIVATED, CHECK HEADRIGHTINNER AND MOVE UP IF COLLISION
                if (IIWasActivated)
                {
                    while (CheckBroadPhase(SonicHeadRight_InnerAABB, CurrentTileAABB))
                    {
                        --Sonic.Y;
                        SonicHeadRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].Y + Sonic.Y),
                            new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].Y + Sonic.Y)};
                        
                        Console.WriteLine("IIa: " + Sonic.X + "," + Sonic.Y);
                    }
                } 
                
                //IF III WAS ACTIVATED, CHECK FOOTLEFTINNER AND MOVE UP IF COLLISION
                if (IIIWasActivated)
                {
                    while (CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB))
                    {
                        --Sonic.Y;
                        SonicFootLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].Y + Sonic.Y),
                            new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].Y + Sonic.Y)};
                    
                        Console.WriteLine("IIIa: " + Sonic.X + "," + Sonic.Y + " " + " footLeftInner:" + CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB), CurrentTileAABB);
                    }
                }
                
                //IF IV WAS ACTIVATED, CHECK FOOTRIGHTINNER AND MOVE UP IF COLLISION
                if (IVWasActivated)
                {
                    while (CheckBroadPhase(SonicFootRight_InnerAABB, CurrentTileAABB))
                    {
                        --Sonic.Y;
                        SonicFootRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].Y + Sonic.Y),
                            new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].Y + Sonic.Y)};
                    
                        Console.WriteLine("IVa: " + Sonic.X + "," + Sonic.Y);
                    }
                }                
                //                         IF IV WAS ACTIVATED, CHECK FOOTRIGHTINNER AND MOVE UP IF COLLISION
                //ANYTHING I SHOULD DO WITH I AND II?
                */
                
                
                //for fixing sonic's position in cases other than when stuck in a corner
                while (CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB) && CheckBroadPhase(SonicFootRight_InnerAABB, CurrentTileAABB))
                {
                    --Sonic.Y;
                    SonicFootLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootLeft_Inner[1].Y + Sonic.Y)};
                    SonicFootRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.FootRight_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("7 " + Sonic.X + "," + Sonic.Y);
                    //Console.WriteLine("V");
                }
                
                while (CheckBroadPhase(SonicHeadLeft_InnerAABB, CurrentTileAABB) && CheckBroadPhase(SonicHeadRight_InnerAABB, CurrentTileAABB))
                {
                    ++Sonic.Y;        //trying to move sonic to the outside of the tile
                    SonicHeadLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadLeft_Inner[1].Y + Sonic.Y)};
                    SonicHeadRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.HeadRight_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("1 " + Sonic.X + "," + Sonic.Y);
                    //Console.WriteLine("VI");
                }
                
                while (CheckBroadPhase(SonicWallLeft_InnerAABB, CurrentTileAABB))
                {
                    Sonic.CurrentCollisionBoxesSet.WallLeft_InnerIsActive = true;
                    
                    ++Sonic.X;
                    SonicWallLeft_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[0].Y + Sonic.Y),
                                                      new Point (Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallLeft_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("3 " + Sonic.X + "," + Sonic.Y);
                    //Console.WriteLine("VII");
                }
                
                while (CheckBroadPhase(SonicWallRight_InnerAABB, CurrentTileAABB))
                {
                    Sonic.CurrentCollisionBoxesSet.WallRight_InnerIsActive = true;
                    
                    --Sonic.X;
                    SonicWallRight_InnerAABB = new [] {new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[0].Y + Sonic.Y),
                                                       new Point (Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].X + Sonic.X, Sonic.CurrentCollisionBoxesSet.WallRight_Inner[1].Y + Sonic.Y)};
                    
                    //Console.WriteLine("4 " + Sonic.X + "," + Sonic.Y);
                    //Console.WriteLine("VIII");
                }
                
                //Console.WriteLine("Fin: " + Sonic.X + "," + Sonic.Y);
                
                
                //foot
                if (CheckBroadPhase(SonicFootLeft_InnerAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.FootLeft_InnerIsActive = true;
                if (CheckBroadPhase(SonicFootLeft_OuterAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.FootLeft_OuterIsActive = true;
                if (CheckBroadPhase(SonicFootLeftBalancing_OuterAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.FootLeftBalancing_OuterIsActive = true;
                
                if (CheckBroadPhase(SonicFootRight_InnerAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.FootRight_InnerIsActive = true;
                if (CheckBroadPhase(SonicFootRight_OuterAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.FootRight_OuterIsActive = true;
                if (CheckBroadPhase(SonicFootRightBalancing_OuterAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.FootRightBalancing_OuterIsActive = true;
                
                //head
                if (CheckBroadPhase(SonicHeadLeft_InnerAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.HeadLeft_InnerIsActive = true;
                if (CheckBroadPhase(SonicHeadRight_InnerAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.HeadRight_InnerIsActive = true;
                
                //wall
                if (CheckBroadPhase(SonicWallTopLeftAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.WallTopLeftIsActive = true;
                if (CheckBroadPhase(SonicWallBottomLeftAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.WallBottomLeftIsActive = true;
                if (CheckBroadPhase(SonicWallLeft_InnerAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.WallLeft_InnerIsActive = true;
                
                if (CheckBroadPhase(SonicWallTopRightAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.WallTopRightIsActive = true;
                if (CheckBroadPhase(SonicWallBottomRightAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.WallBottomRightIsActive = true;
                if (CheckBroadPhase(SonicWallRight_InnerAABB, CurrentTileAABB))
                    Sonic.CurrentCollisionBoxesSet.WallRight_InnerIsActive = true;
            }
        }

        public static void ResetSonicCollisionBoxes()
        {
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.HeadLeft_InnerIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.HeadRight_InnerIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallTopLeftIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallBottomLeftIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallLeft_InnerIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallTopRightIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallBottomRightIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.WallRight_InnerIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootLeft_InnerIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootLeft_OuterIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootLeftBalancing_OuterIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootRight_InnerIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootRight_OuterIsActive = false;
            GameState.LinkToSonicObject.CurrentCollisionBoxesSet.FootRightBalancing_OuterIsActive = false;
        }

        public static void ResetMotobugCollisionBoxes()
        {
            foreach (var motobug in GameState.MotobugsList)
            {
                var motobugAsMotobug = (BadnikMotobugObject) motobug;

                motobugAsMotobug.GroundLeftIsActive = false;
                motobugAsMotobug.GroundRightIsActive = false;
                motobugAsMotobug.WallLeftIsActive = false;
                motobugAsMotobug.WallRightIsActive = false;
            }
        }
    }
}