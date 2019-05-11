using System;

namespace sonic_c_sharp
{
    public static class DroppingRings
    {
        public static void DropRings()
        {
            var angleInDegrees = 101.25f;                             //assuming 0=right, 90=up, 180=left, 270=down
            var speed = 4;
            var shouldMakeSpeedNegative = false;

            var currentRing = 0;

            while (currentRing < GameState.LinkToSonicObject.Rings && currentRing < 32)
            {
                var angleInRadians = Math.PI * angleInDegrees / 180.0;
                
                var newRingXSpeed = Math.Cos(angleInRadians)*speed;
                var newRingYSpeed = -Math.Sin(angleInRadians)*speed;
                
                if (shouldMakeSpeedNegative)
                {
                    newRingXSpeed *= -1;
                    angleInDegrees += 22.5f;
                }

                GameState.ObjectsToAdd.Add(new RingDroppedObject(GameState.LinkToSonicObject.X, GameState.LinkToSonicObject.Y, (float)newRingXSpeed, (float)newRingYSpeed));                
                
                shouldMakeSpeedNegative = !shouldMakeSpeedNegative;

                ++currentRing;
                    
                if (currentRing == 16)  //checking whether the rings are being thrown in the second circle
                {
                    speed = 2;          //we're on the second circle now, so decrease the speed
                    angleInDegrees = 101.25f;    //resetting the angle for the second circle
                }
            }
        }
    }
}