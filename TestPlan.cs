using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using MathNet.Numerics.RootFinding;
using Prorgam;
using Spea;
using Spea.Instruments;
using Spea.TestEnvironment;
using Spea.TestFramework;

namespace Program
{
    public class TestPlan
    {
        private static Thread keepAliveThread;


        /// <summary>
        /// The test point offset between subsequential sites.
        /// </summary>
        private const int TestPointSiteOffset = 0;

        /// <summary>
        /// A list of bays, which are initialized with its number and the the list of sites it is responsible for.
        /// </summary>
        private static List<Bay> _bays = new List<Bay>
        {
            new Bay(number: 1, sites: new List<int> { 1 }),
        };

        /// <summary>
        /// Mapping between the purpose of an user flag and the physical flag. <br/>
        /// It is initialized in the static constructor <see cref="TestPlan()"/>. <br/>
        /// </summary>
        private static UserFlagMap<UserFlagPurpose> _userFlagMap;

        /// <summary>
        /// Mapping between the purpose of an pmx channel and the physical channel. <br/>
        /// It is initialized in the static constructor <see cref="TestPlan()"/>. <br/>
        /// </summary>
        private static PmxChannelMap<PmxPurpose> _pmxChannelMap;


        /// <summary>
        /// The current active variant of the TPGM.
        /// </summary>
        private static Variant _variant;


        public static TestLists testLists = new TestLists();   

        /// <summary>
        /// The list of multi site tests that will be executed for this TPGM.
        /// </summary>
        private static MultiSiteTestList<TestParameters, UserFlagPurpose, PmxPurpose> _multiSiteTestList = testLists.getMultiSiteTestList();       

        /// <summary>
        /// The list of tests that will run for this TPGM. <br/>
        /// The tests will run in the order they are inserted in this list. <br/>
        /// The example test can be find in ./Tests. Further tests can be added to the same folder.
        /// </summary>
        private static TestList<TestParameters, UserFlagPurpose, PmxPurpose> _testList = testLists.getSingleSiteTestList();

        /// <summary>
        /// Static constructor used to initialize <see cref="_userFlagMap"/> and set <see cref="UserFlag.DefaultGroup"/>.
        /// You can modify the file ./TestFramework/UserFlagPurpose.cs to define the purposes of the user flags for your project. <br/>
        /// The phsysical user flag can be passed as an integer (for UserFlag.DefaultGroup) or explicitly. 
        /// </summary>
        static TestPlan()
        {
            _userFlagMap = new UserFlagMaps().getUserFlagMap();
            _pmxChannelMap = new PMXChannelMaps().getPmxChannelMap();
            _variant = new Variant();
        }


        

        /// <summary>
        /// Entry point for your TPGM.
        /// </summary>
        public static void TestPlanStart()
        {
            LeoF.RunAnlTaskLabel("", "DSC_S", "DSC_E");
        

            //LogController.ConfigureLogging(LoggingOption.Both, "C:\\Data\\", "eAts_TestSoftware.log");

            // The site manager knows which sites should be tested based on the current bay and the bay list.
            // It also is used by the TestFramework to automatically update the test points of the test items based on the offset.
            var siteManager = new SiteManager(_bays, TestPointSiteOffset, debug: TestParameters.Debug);

            // The fixture is used by the test environment for setting and resetting user flags and pmx channels based on its purpose.
            var fixture = new Fixture<UserFlagPurpose, PmxPurpose>(_userFlagMap, _pmxChannelMap);

            // The test environment is passed to each one of the tests and should be used to set the test envrionment for each test.
            // (FPS, User Flags, Test Points and Stimuli).
            var testEnvironment = new TestEnv<UserFlagPurpose, PmxPurpose>(fixture);

            var status = TestplanResults.PASS;

            try
            {
                // Test parameters is an object that is passed to each of the tests. Use this to pass custom objects to your tests.
                // You can find this class in ./TestFramework/TestParameter.cs.
                var testParameters = new TestParameters();


                


                // This method contains the main loop, which will go through all sites, executing all the tests entered in the _testList.
                status = Run(testEnvironment, siteManager, _variant, testParameters);
            }
            catch (Exception ex)
            {
                // Log exception to Runpack and CdColl.
                status = LogException(ex, siteManager.Sites.ToList());
            }

            if(TestParameters.Debug)
            {

                var Log = LogController.GetLogHistory();


                File.WriteAllLines(TestParameters.DebugLoggingPath, Log);
            }


            // Set test plan result.
            LeoF.TplanResultSet(status);
        }

        /// <summary>
        /// Main loop where tests are executed for each site.
        /// </summary>
        /// <param name="testEnvironment">The test environment object which will be passed to each test.</param>
        /// <param name="siteManager">The site manager object.</param>
        /// <param name="testParameters">Custom parameters which will be passed to each test.</param>
        /// <returns></returns>
        private static TestplanResults Run(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, SiteManager siteManager, Variant variant, TestParameters testParameters, bool logWholeTestTime = true)
        {
            var status = TestplanResults.PASS;
            var sitesUnderTest = siteManager.SitesToTest.ToList();
            var toBeTestedSites = siteManager.SitesToTest.ToList();
            Stopwatch testTime = Stopwatch.StartNew();

            // This try-catch will catch any exception which is thrown from inside a test.
            // An exception will end the tests for the bay and will be logged to the screen and CdColl as a Fail.
            // After all tests are completed, the test envrionment is reseted, ensuring a power down.
            try
            {
                MultiSiteTestLoop(testEnvironment, siteManager, testParameters, variant, out status, out sitesUnderTest, out var stop);

                if (!stop && siteManager.SitesToTest.Any())
                {
                    SingleSiteTestLoop(testEnvironment, siteManager, testParameters, variant, out status, out sitesUnderTest);
                }
                if (logWholeTestTime && toBeTestedSites.Any())
                {
                    var plural = toBeTestedSites.Count > 1 ? "s" : string.Empty;
                    LogController.Print(string.Empty);
                    LogController.PrintWithTag($"Complete Test duration: {testTime.ElapsedMilliseconds} ms", $"Site{plural} {string.Join(",", toBeTestedSites)}", LogColor.White);
                }
            }
            catch (Exception ex)
            {
                foreach (var site in sitesUnderTest)
                {
                    siteManager.SetTestResult(site, SiteResults.RESULT_FAIL);
                }

                // Log exception to Runpack and CdColl.
                status = LogException(ex, sitesUnderTest);
            }
            finally
            {
                // Catch and log any exception caused by the test environment itself.


                try
                {
                    // Reset test environment, ensuring power down.
                    testEnvironment.Reset();
                }
                catch (Exception ex)
                {
                    foreach (var site in sitesUnderTest)
                    {
                        siteManager.SetTestResult(site, SiteResults.RESULT_FAIL);
                    }

                    status = LogException(ex, sitesUnderTest);
                }
                try
                {
                    Restbus.RestBusStop = true;
                    keepAliveThread.Join();
                }
                catch
                {

                }
            }

            return status;
        }

        private static void SingleSiteTestLoop(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, SiteManager siteManager, TestParameters testParameters, Variant variant, out TestplanResults status, out List<int> sitesUnderTest)
        {
            status = TestplanResults.PASS;
            sitesUnderTest = siteManager.SitesToTest.ToList();
            var @break = false;
            var sequentialTestId = TestItem.NextAvailableId;

            // Loop through all sites.
            foreach (var site in siteManager.SitesToTest)
            {

                testParameters.Xcp.Initialize();

                keepAliveThread = new System.Threading.Thread(() =>
                {
                    Restbus.RunRestbus();
                   
                });

               keepAliveThread.Start();

                // Store site being tested, so that we can log any exceptions to the correct site.
                sitesUnderTest = new List<int> { site };

                // All IDs are reseted before a site is tested, so that the same test have the same ID.
                TestItem.ResetId(sequentialTestId);

                // Tests are generated new for each site. This ensure that test configuration changed during test is restored.
                var tests = _testList.Select(factory => factory(site, siteManager, variant)).ToList();

                // Each site is set to PASS, at test start.
                siteManager.SetTestResult(site, SiteResults.RESULT_PASS);

                // Loop through all tests.
                foreach (var test in tests)
                {
                    // Execute test and handle FAIL case.
                    if (test.Test(testEnvironment, testParameters, isLogTimeEnabled: true).Single().Value == TestResult.FAILBOTH)
                    {
                        status = TestplanResults.FAIL;
                        siteManager.SetTestResult(site, SiteResults.RESULT_FAIL);
                        if(TestParameters.DontBreakOnFail)
                        {

                        }
                        else
                        {
                            break;
                        }
                    }

                    // Check for a Stop in Runpack.
                    if (LeoF.CheckStop() != 0)
                    {
                        @break = true;
                        break;
                    }
                }

                // Reset test environment, ensuring power down of site.
                testEnvironment.Reset();

                // If test was stopped from Runpack, break.
                if (@break)
                {
                    break;
                }
            }
        }

        private static void MultiSiteTestLoop(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, SiteManager siteManager, TestParameters testParameters, Variant variant, out TestplanResults status, out List<int> sitesUnderTest, out bool stop)
        {
            status = TestplanResults.PASS;
            sitesUnderTest = siteManager.SitesToTest.ToList();
            stop = false;

            // Loop through all multi site tests.
            foreach (var testFactory in _multiSiteTestList)
            {
                var test = testFactory.Create(siteManager, variant);

                // Exclude any tests which don't have sites assigned to it.
                if (testFactory.Sites.Count == 0)
                {
                    continue;
                }

                // Update the sitesUnderTest variable (used to log exceptions).
                sitesUnderTest = testFactory.Sites;

                // Execute test and handle FAIL case.
                var results = test.Test(testEnvironment, testParameters, isLogTimeEnabled: true);
                foreach (var result in results)
                {
                    if (result.Value == TestResult.FAILBOTH)
                    {
                        status = TestplanResults.FAIL;
                        var site = result.Key;
                        siteManager.SetTestResult(site, SiteResults.RESULT_FAIL);
                    }
                }

                // Check for a Stop in Runpack.
                if (LeoF.CheckStop() != 0)
                {
                    stop = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Logs exception to Runpack and CdColl.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="sites">Sites being tested, when exception happened.</param>
        /// <param name="logToCdColl">Log exception to CdColl.</param>
        /// <returns><see cref="TestplanResults.FAIL"/></returns>
        private static TestplanResults LogException(Exception ex, List<int> sites, bool logToCdColl = true)
        {
            while (ex != null)
            {
                if (logToCdColl)
                {
                    foreach (var site in sites)
                    {
                        LogController.LogFail(new LogInfo { Remark = $"Exception: {ex.Message}", MeasUnit = "boolean", ThrHigh = 1, ThrLow = 1, MeasuredValue = 0, Site = site }, testReport: false);
                        LeoF.SiteResultWrite(site, SiteResults.RESULT_FAIL);
                    }                    
                }

                LogController.PrintWithTagAndFail(ex.Message, "Exception", LogColor.Red);

                ex = ex.InnerException;
            }           

            return TestplanResults.FAIL;
        }
    }
}
