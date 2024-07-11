namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AtosF;
    using static AtosF.modAtosF;
    using static AtosF.modAtosF2;
    using PeakCanXcp;

    public class FktTest
    {
        protected static void SetTestResult(Test test)
        {
            return;
        }

        protected static void SetFlagAndTest(Xcp xcp, A2lParser a2l, Test test, string xcpParameter)
        {
            test.Measured = DutController.SetFlag(xcp, a2l, xcpParameter, (byte)test.Typ);

            if (test.Measured == test.Typ)
            {
                test.Result = PASS;
            }
            else
            {
                test.Result = FAIL;
            }
        }

        protected static void TestMcuMeasurement(Xcp xcp, Test test, XcpValue value, int arrayIndex = -1)
        {
            var convertedValue = xcp.GetConvertedValue(value, arrayIndex);
            test.Measured = convertedValue.Value;
            if ((test.Measured < test.Min) || (test.Measured > test.Max))
            {
                test.Result = FAIL;
            }
            else
            {
                test.Result = PASS;
            }
        }

        protected static void SetUserFlag(RelayFunction function, Dictionary<RelayFunction, UserFlagRelay> userFlags)
        {
            var flag = userFlags[function];
            var flagArray = new int[] { flag.FlagNumber, 0 };
            UserFlagsSet((int)flag.Group, ref flagArray[0]);
        }

        protected static void ResetUserFlag(RelayFunction function, Dictionary<RelayFunction, UserFlagRelay> userFlags)
        {
            var flag = userFlags[function];
            var flagArray = new int[] { flag.FlagNumber, 0 };
            UserFlagsReset((int)flag.Group, ref flagArray[0]);
        }

        protected static void SetSingleUserFlag(RelayFunction function, Dictionary<RelayFunction, UserFlagRelay> userFlags)
        {
            return;

            ResetUserFlags(userFlags);

            SetUserFlag(function, userFlags);
        }

        protected static void ResetUserFlags(Dictionary<RelayFunction, UserFlagRelay> userFlags)
        {
            return;

            foreach (var flag in userFlags)
            {
                var flagArray = new int[] { flag.Value.FlagNumber, 0 };
                UserFlagsReset((int)flag.Value.Group, ref flagArray[0]);
            }
        }

        protected static void DelayUs(int us)
        {
            var currentTime = Stopwatch.GetTimestamp();
            while ((double)(Stopwatch.GetTimestamp() - currentTime) / (double)(Stopwatch.Frequency) < (0.000001 * us)) { }
        }

        protected static double ToKelvin(double celsius)
        {
            return celsius + 273.15;
        }

        protected static double ToCelsius(double kelvin)
        {
            return kelvin - 273.15;
        }
    }

    public class Test
    {
        public string TestItem { get; set; } = "";

        public string NetName { get; set; } = "";

        public string Testpoint { get; set; } = "";

        public double Min { get; set; } = 0.0; 

        public double Typ { get; set; } = 0.0;

        public double Max { get; set; } = 0.0;

        public double Measured { get; set; } = 0.0;

        public string Unit { get; set; } = "";

        public int Result { get; set; } = PASS;
    }

    public class UserFlagRelay
    {
        public UserFlagGroup Group { get; set; }
        public int FlagNumber { get; set; }
    }

    public enum RelayFunction
    {
        LowSideDrivers = 0,
        HighSideDriver1 = 1,
        HighSideDriver2 = 2,
        HighSideDriver3 = 3,
        HighSideDriver4 = 4,
        HighSideDriver5 = 5,
        HighSideDriver6 = 6,
        ConnectKl30CSite1 = 7,
        ConnectKl15Site1 = 8,
        ConnectKl30CSite2 = 9,
        ConnectKl15Site2 = 10,
        Toggle12VBetweenSites = 11,
        ConnectCanSite1 = 12,
        ConnectCanSite2 = 13,
        Toggle48VBetweenSites = 14,
        TogglePicoScopesBetweenSites = 15,
    }

    public enum UserFlagGroup
    {
        RLYNOCPU = modAtosF.RLYNOCPU,
        RLYNOA = modAtosF.RLYNOA,
        RLYNOB = modAtosF.RLYNOB
    }

    public static class TestExtensions
    {
        public static Test GetTest(this List<Test> list, string testItem)
        {
            return list.Where(t => t.TestItem == testItem).Single();
        }
    }
}
