using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Windows.Forms;

namespace sonic_c_sharp
{
    [TestFixture]
    public class Tests
    {
	    public static bool IsTestingApplication = false;
	    public static int FramesElasped = 0;
	    
	    private void InitiateTestEnvironment(string levelFilePath)
	    {
		    Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
		    
		    GameState.ObjectList = new List<GameObject>();
		    GameState.ObjectsToRemove = new List<GameObject>();
		    GameState.ObjectsToAdd = new List<GameObject>();
            
		    GameState.MotobugsList = new List<GameObject>();
		    GameState.MotobugsToRemove = new List<GameObject>();

		    GameState.LinkToSonicObject = null;
		    Resources.LoadResources(levelFilePath);
		    GameState.LinkToSonicObject.InitializeSonicCollisionBoxes();
		    GameState.LinkToSonicObject.CurrentCollisionBoxesSet = GameState.LinkToSonicObject.StandingCollisionBoxes;
		    Background.InitiateBackground();

		    GameForm.IsFadingToTransperent = false;
			GameForm.IsFadingToBlack = false;
		    IsTestingApplication = true;
		    FramesElasped = 0;

		    GameForm.ZIsPressed = false;
		    GameForm.LeftIsPressed = false;
		    GameForm.DownIsPressed = false;
		    GameForm.UpIsPressed = false;
		    GameForm.RightIsPressed = false;
		    
		    Application.EnableVisualStyles();
	    }
	    
	    //testLevel1
	    [Test]
	    public void DuckingTest()
	    {
		    InitiateTestEnvironment("tests/testLevel1.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    GameForm.LockKeysVariables = true;
		    GameForm.DownIsPressed = true;
		    Application.Run(new GameForm());
			
		    Assert.AreEqual(Sonic.XSpeed, 0);
		    Assert.AreEqual(Sonic.YSpeed, 0);
		    Assert.AreEqual(Sonic.CurrentCollisionBoxesSet, Sonic.DuckingCollisionBoxes);
	    }
	    
	    [Test]
	    public void LookingUpTest()
	    {
		    InitiateTestEnvironment("tests/testLevel1.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    GameForm.LockKeysVariables = true;
		    GameForm.UpIsPressed = true;
		    Application.Run(new GameForm());
			
		    Assert.AreEqual(Sonic.XSpeed, 0);
		    Assert.AreEqual(Sonic.YSpeed, 0);
		    Assert.AreEqual(Sonic.CurrentCollisionBoxesSet, Sonic.StandingCollisionBoxes);
	    }
	    
	    [Test]
	    public void JumpingUpTest()
	    {
		    InitiateTestEnvironment("tests/testLevel1.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    GameForm.LockKeysVariables = true;
		    GameForm.ZIsPressed = true;
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.XSpeed, 0);
		    Assert.AreEqual(Sonic.YSpeed, 0);
		    Assert.AreEqual(Sonic.CurrentCollisionBoxesSet, Sonic.StandingCollisionBoxes);
	    }

	    
	    //testLevel2
	    [Test]
	    public void JumpingLeftTest()
	    {
		    InitiateTestEnvironment("tests/testLevel2.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    GameForm.LockKeysVariables = true;
		    GameForm.ZIsPressed = true;
		    GameForm.LeftIsPressed = true;
		    Application.Run(testForm);
			
		    Assert.Less(Sonic.XSpeed, 0);
		    Assert.Greater(Sonic.YSpeed, 0);
		    Assert.AreEqual(Sonic.CurrentCollisionBoxesSet, Sonic.RollingCollisionBoxes);
	    }
	    
	    [Test]
	    public void JumpingRightTest()
	    {
		    InitiateTestEnvironment("tests/testLevel2.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    GameForm.LockKeysVariables = true;
		    GameForm.ZIsPressed = true;
		    GameForm.RightIsPressed = true;
		    Application.Run(testForm);
			
		    Assert.Greater(Sonic.XSpeed, 0);
		    Assert.Greater(Sonic.YSpeed, 0);
		    Assert.AreEqual(Sonic.CurrentCollisionBoxesSet, Sonic.RollingCollisionBoxes);
	    }
	    
	    [Test]
	    public void WalkingLeftTest()
	    {	
		    InitiateTestEnvironment("tests/testLevel2.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    GameForm.LockKeysVariables = true;
		    GameForm.LeftIsPressed = true;
		    Application.Run(testForm);
			
		    Assert.Less(Sonic.XSpeed, 0);
		    Assert.AreEqual(Sonic.CurrentCollisionBoxesSet, Sonic.StandingCollisionBoxes);
	    }
	    
	    [Test]
	    public void WalkingRightTest()
	    {	
		    InitiateTestEnvironment("tests/testLevel2.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    GameForm.LockKeysVariables = true;
		    GameForm.RightIsPressed = true;
		    Application.Run(testForm);
			
		    Assert.Greater(Sonic.XSpeed, 0);
		    Assert.AreEqual(Sonic.CurrentCollisionBoxesSet, Sonic.StandingCollisionBoxes);
	    }
	    
	    //unique test levels
	    [Test]
	    public void RingSparkleTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelRingSparkle.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);

		    var foundRingSparklesObject = false;
		    foreach (var gameObject in GameState.ObjectList)
		    {
			    if (gameObject is RingSparklesObject)
			    {
				    foundRingSparklesObject = true;
				    break;
			    }
		    }
		    
		    Assert.AreEqual(foundRingSparklesObject, true);
	    }
	    
	    [Test]
	    public void ConveyorBelt1Test()
	    {	
		    InitiateTestEnvironment("tests/testLevelConveyorBelt1.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.Greater(Sonic.X, 200);
	    }
	    
	    [Test]
	    public void ConveyorBelt2Test()
	    {	
		    InitiateTestEnvironment("tests/testLevelConveyorBelt2.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.Less(Sonic.X, 200);
	    }
	    
	    [Test]
	    public void RingsCollectingTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelRingSparkle.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.Greater(Sonic.Rings, 20);
	    }
	    
	    [Test]
	    public void BlueRingsCollectingTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelBlueRings.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.Rings, 160);
	    }
	    
	    [Test]
	    public void SpikesTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelSpikes.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }
	    
	    [Test]
	    public void LavaTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelLava.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }
	    
	    [Test]
	    public void HittingBadnikFishTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelBadnikFish.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }
	    
	    [Test]
	    public void HittingBadnikMotobugTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelBadnikMotobug.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }

	    [Test]
	    public void HittingBadnikSimpleTest()
	    {
		    InitiateTestEnvironment("tests/testLevelBadnikSimple.txt");
		    var Sonic = GameState.LinkToSonicObject;

		    var testForm = new GameForm();
		    Application.Run(testForm);

		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }

	    [Test]
	    public void YellowSpringTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelYellowSpring.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.Greater(Sonic.YSpeed, 0);
	    }
	    
	    [Test]
	    public void ClosingDoorTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelDoor.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    GameForm.RightIsPressed = true;
		    Application.Run(testForm);
		    
		    var doorIsClosed = false;
		    foreach (var gameObject in GameState.ObjectList)
		    {
			    if (gameObject is ClosingDoorObject)
			    {
				    var ClosingDoor = (ClosingDoorObject) gameObject;
				    if (ClosingDoor.Y <= ClosingDoor.YAtWhichToStop);
						doorIsClosed = true;
				    
				    break;
			    }
		    }
			
		    Assert.AreEqual(doorIsClosed, true);
	    }
	    
	    [Test]
	    public void SpikeBallSmallTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelSpikeBallSmall.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }
	    
	    [Test]
	    public void SpikeBallBigTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelSpikeBallBig.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }
	    
	    [Test]
	    public void HittingMechaSonicTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelMechaSonic.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }
	    
	    [Test]
	    public void NotCollidingWithMechaSonicTest()
	    {	
		    InitiateTestEnvironment("tests/testLevelMechaSonic.txt");
		    var Sonic = GameState.LinkToSonicObject;
			
		    var testForm = new GameForm();
		    Application.Run(testForm);
			
		    Assert.AreEqual(Sonic.IsLosingLife, true);
	    }
	}
}