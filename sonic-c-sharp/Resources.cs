using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace sonic_c_sharp
{
    public static class Resources
    {   
        public static void LoadResources(string levelFilePath)        //also finds sonic object and assigns to gamestate,linktosonicobject
        {
            int linesCounterForDebugging = 0;
            string line;

            var file = new StreamReader(levelFilePath);
            var gottenObjectType = "";
            var gottenX = 0;
            var gottenY = 0;
            var gottenCollidability = false;          //only for tiles
            Bitmap gottenTileBitmap = null;           //only for tiles
            var gottenAABBs = new List<Point[]>();    //only for tiles
            while ((line = file.ReadLine()) != null)
            {
                if (line == "")
                    continue;
                
                Console.WriteLine(line);
                if (line.Substring(0, 1) == "}")      //symbol designating the end of declaration of an object
                {
                    if (gottenObjectType == "Sonic")
                    {
                        var sonic = new SonicObject(gottenX, gottenY);
                        GameState.ObjectList.Add(sonic);
                        GameState.LinkToSonicObject = sonic;
                    }
                    else if (gottenObjectType == "BadnikSimple")
                        GameState.ObjectList.Add(new BadnikSimpleObject(gottenX, gottenY, gottenAABBs));
                    else if (gottenObjectType == "BadnikFish")
                        GameState.ObjectList.Add(new BadnikFishObject(gottenX, gottenY));
                    else if (gottenObjectType == "BadnikMotobug")
                    {
                        var newMotobug = new BadnikMotobugObject(gottenX, gottenY, gottenAABBs);
                        GameState.ObjectList.Add(newMotobug);
                        GameState.MotobugsList.Add(newMotobug);
                    }
                    else if (gottenObjectType == "InvisibleDamaging")
                        GameState.ObjectList.Add(new InvisibleDamagingObject(gottenX, gottenY, gottenAABBs));
                    else if (gottenObjectType == "Lava")
                        GameState.ObjectList.Add(new LavaObject(gottenX, gottenY));
                    else if (gottenObjectType == "LavaTop")
                        GameState.ObjectList.Add(new LavaTopObject(gottenX, gottenY));
                    else if (gottenObjectType == "SpikeBallSmall")
                        GameState.ObjectList.Add(new SpikeBallSmallObject(gottenX, gottenY, gottenCollidability));        //gottenCollidability used for determining movement direction
                    else if (gottenObjectType == "SpikeBallBig")
                        GameState.ObjectList.Add(new SpikeBallBigObject(gottenX, gottenY, gottenCollidability));        //gottenCollidability used for determining movement direction
                    else if (gottenObjectType == "YellowSpring")
                        GameState.ObjectList.Add(new YellowSpringObject(gottenX, gottenY));
                    else if (gottenObjectType == "Ring")
                        GameState.ObjectList.Add(new RingObject(gottenX, gottenY));
                    else if (gottenObjectType == "Tile")
                        GameState.ObjectList.Add(new TileObject(gottenX, gottenY, gottenCollidability, gottenTileBitmap, gottenAABBs));
                    else if (gottenObjectType == "ConveyorBelt")
                        GameState.ObjectList.Add(new ConveyorBeltObject(gottenX, gottenY, gottenCollidability));
                    else if (gottenObjectType == "BlueRing")
                        GameState.ObjectList.Add(new BlueRingObject(gottenX, gottenY));
                    else if (gottenObjectType == "ClosingDoor")
                        GameState.ObjectList.Add(new ClosingDoorObject(gottenX, gottenY, gottenCollidability));
                    else if (gottenObjectType == "MechaSonic")
                        GameState.ObjectList.Add(new MechaSonicObject(gottenX, gottenY));
                    else if (gottenObjectType == "Shutters")
                        GameState.ObjectList.Add(new ShuttersObject(gottenX, gottenY, gottenCollidability));
                    else if (gottenObjectType == "Smoke")
                        GameState.ObjectList.Add(new SmokeObject(gottenX, gottenY));
                    
                    //into separate method: clearing variables (do i really need to do this though?)
                    gottenObjectType = "";
                    gottenX = 0;
                    gottenY = 0;
                    gottenCollidability = false;          //only for tiles
                    gottenTileBitmap = null;              //only for tiles
                    gottenAABBs = new List<Point[]>();    //only for tiles
                }
                else
                {
                    if (line.Substring(0, 1) == ":")      //case of line with an AABB
                    {
                        var startIndex = 1;
                        var length = 0;
                        var totalCoordinatesEncountered = 0;

                        var TopLeftX = 0;
                        var TopLeftY = 0;
                        var BottomRightX = 0;
                        var BottomRightY = 0;
                        for (var i = 1; i < line.Length; ++i)
                        {
                            if (line[i] == ';')
                            {
                                ++totalCoordinatesEncountered;
                                
                                if (totalCoordinatesEncountered == 1)
                                    TopLeftX = int.Parse(line.Substring(startIndex, length));
                                else if (totalCoordinatesEncountered == 2)
                                    TopLeftY = int.Parse(line.Substring(startIndex, length));
                                else if (totalCoordinatesEncountered == 3)
                                    BottomRightX = int.Parse(line.Substring(startIndex, length));
                                else if (totalCoordinatesEncountered == 4)
                                {
                                    BottomRightY = int.Parse(line.Substring(startIndex, length));
                                    gottenAABBs.Add(new[] {new Point(TopLeftX, TopLeftY), new Point(BottomRightX, BottomRightY)});
                                }

                                startIndex = i + 1;
                                length = 0;
                            }
                            else
                            {
                                ++length;
                            }
                            
                        }
                    }
                    else                                  //case of line with object declaration
                    {
                        var startIndex = 0;
                        var length = 0;
                        var totalParametersEncountered = 0;
                        for (var i = 0; i < line.Length; ++i)
                        {
                            if (line[i] == ';')
                            {
                                ++totalParametersEncountered;

                                if (totalParametersEncountered == 1)
                                {
                                    gottenObjectType = line.Substring(startIndex, length);
                                }
                                else if (totalParametersEncountered == 2)
                                {
                                    gottenX = int.Parse(line.Substring(startIndex, length));     //not the best way to convert to int
                                }
                                else if (totalParametersEncountered == 3)
                                {
                                    gottenY = int.Parse(line.Substring(startIndex, length));
                                    
                                    if (gottenObjectType == "Sonic" || gottenObjectType == "BadnikSimple" || gottenObjectType == "Ring"
                                        || gottenObjectType == "InvisibleDamaging" || gottenObjectType == "Lava"
                                        || gottenObjectType == "YellowSpring" || gottenObjectType == "BadnikMotobug"
                                        || gottenObjectType == "BadnikFish" || gottenObjectType == "BlueRing" || gottenObjectType == "MechaSonic")
                                        break;
                                }
                                else if (totalParametersEncountered == 4)
                                {
                                    var temp = line.Substring(startIndex, length);
                                    if (temp == "true")
                                        gottenCollidability = true;        //maybe rename to gottenAdditionalParameter?
                                    else if (temp == "false")
                                        gottenCollidability = false;
                                    
                                    if (gottenObjectType == "SpikeBallSmall" || gottenObjectType == "SpikeBallBig" 
                                        || gottenObjectType == "ConveyorBelt" || gottenObjectType == "ClosingDoor"
                                        || gottenObjectType == "Shutters")
                                        break;
                                }
                                else if (totalParametersEncountered == 5)
                                {
                                    gottenTileBitmap = new Bitmap(line.Substring(startIndex, length));
                                    
                                    if (gottenObjectType == "Tile")    //unnecessary, but may stay for readability
                                        break;
                                }

                                startIndex = i + 1;
                                length = 0;
                            }
                            else
                                ++length;
                        }
                    }
                }
                linesCounterForDebugging++;
            }
            
            file.Close();
            Console.WriteLine("btw, there were {0} lines.", linesCounterForDebugging);
            Console.WriteLine("Done reading files");
        }
    }
}