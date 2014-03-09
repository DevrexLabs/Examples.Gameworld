using System.IO;
using OrigoDB.Examples.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using OrigoDB.Core;
using OrigoDB.Core.Proxy;

namespace Examples.Proxy.Test
{
    
    
    /// <summary>
    ///This is a test class for GameWorldModelTest and is intended
    ///to contain all GameWorldModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GameWorldModelTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AddPlayer
        ///</summary>
        [TestMethod()]
        public void CanAddPlayer()
        {
            GameWorldModel target = new GameWorldModel();
            DateTime when = DateTime.Now;
            string name = "spanky";
            target.AddPlayer(when, name);
            
        }

        /// <summary>
        ///A test for GetNearbyPlayers. Testing behavior of the domain model by targetting it direct.
        ///</summary>
        [TestMethod()]
        public void GetNearbyPlayersTest()
        {
            var target = new GameWorldModel(); 
            
            
            //both players starting at position 0,0 and no 
            target.AddPlayer(DateTime.Now, "spanky");
            target.AddPlayer(DateTime.Now, "snoop");

            // one starts moving NE with a speed of sqrt(2) or approx. 1.414
            target.Update("robert", new Point(1,1), DateTime.Now);
            Thread.Sleep(1000);

            //after a second should be at 1.414 which is within 1.5
            var result = target.GetPlayersWithinRadius("spanky", 1.5);
            Assert.IsTrue(result.Length == 1);

            //after another second the distance should be 2.828
            Thread.Sleep(1000);
            result = target.GetPlayersWithinRadius("spanky", 1.5);
            Assert.IsTrue(result.Length == 0);
        }

        /// <summary>
        ///A test for RemovePlayer
        ///</summary>
        [TestMethod()]
        public void ProxyTest()
        {
            //Depends on which test runner is used...
            if (Directory.Exists("GameWorldModel"))
            {
                Console.WriteLine("Deleting old model");
                new DirectoryInfo("GameWorldModel").Delete(recursive: true);
            }

            // a full engine test, leaves journal files in the TestResults folder.

            //should create
            var engine = Engine.LoadOrCreate<GameWorldModel>();
            var proxy = engine.GetProxy();
            proxy.AddPlayer(DateTime.Now, "spanky");
            proxy.AddPlayer(DateTime.Now, "snoop");
            engine.Close();

            //should load the previous
            engine = Engine.LoadOrCreate<GameWorldModel>();
            proxy = engine.GetProxy();
            var result = proxy.GetPlayersWithinRadius("spanky", 10);
            Assert.IsTrue(result.Length == 1);
            engine.Close();

        }
    }
}
