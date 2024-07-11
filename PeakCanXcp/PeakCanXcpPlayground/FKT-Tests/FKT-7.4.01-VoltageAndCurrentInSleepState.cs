namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using PeakCanXcp;

    class Fkt741_VoltageAndCurrentInSleepState : FktTest
    {
        private static Dictionary<RelayFunction, UserFlagRelay> UserFlags { get; set; } = new Dictionary<RelayFunction, UserFlagRelay>
        {
            {RelayFunction.ConnectKl30CSite1,  new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 1 } },
            {RelayFunction.ConnectKl30CSite2, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 3 } },
        };

        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT00101", NetName = "KL30_FILT_MSMT_COM", Testpoint = "TP3106", Min = 11.95, Typ = 12, Max = 12.05, Unit = "V"},
            new Test{TestItem = "FCT00102", NetName = "KL30_RPP", Testpoint = "TP3108", Min = 11, Typ = 12, Max = 12.05, Unit = "V"},
            new Test{TestItem = "FCT00103", NetName = "13V5", Testpoint = "TP3154", Min = 13, Typ = 13.5, Max = 14.5, Unit = "V"},
            new Test{TestItem = "FCT00104", NetName = "5V_BN12", Testpoint = "TP3127", Min = 4.7, Typ = 4.9, Max = 5.1, Unit = "V"},
            new Test{TestItem = "FCT00105", NetName = "8V_BN12", Testpoint = "TP3122", Min = 8.1, Typ = 8.5, Max = 8.9, Unit = "V"},
            new Test{TestItem = "FCT00106", NetName = "Supply current KL30", Testpoint = "External", Min = 0, Typ = 0.00005, Max = 0.0001, Unit = "A"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // Power on (12V).

            switch(site)
            {
                case 1:
                    SetSingleUserFlag(RelayFunction.ConnectKl30CSite1, Fkt741_VoltageAndCurrentInSleepState.UserFlags);
                    break;
                case 2:
                    SetSingleUserFlag(RelayFunction.ConnectKl30CSite2, Fkt741_VoltageAndCurrentInSleepState.UserFlags);
                    break;
                default:
                    throw new ArgumentException($"The number {site} can't be passed as the selected site. Panel has two sites.");
            }

            // Activate Sleep Mode.
            DutController.Sleep(xcp, a2l);
            Thread.Sleep(50);

            // Analog measurements based on site beeing tested.

            // FCT00101
            // FCT00102
            // FCT00103
            // FCT00104
            // FCT00105
            // FCT00106

            foreach(var item in TestItems)
            {
                FktTest.SetTestResult(item);
            }

            Console.WriteLine("This is sleep mode");
            Console.WriteLine("Reset the power and press enter to proceed");
            Console.ReadKey(true);

            // Power off (12V).

            Thread.Sleep(500);

            xcp.Connect();
        }
    }
}
