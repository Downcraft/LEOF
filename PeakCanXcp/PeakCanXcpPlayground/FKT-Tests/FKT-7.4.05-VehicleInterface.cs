namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Fkt745_VehicleInterface : FktTest
    {
        private static Dictionary<RelayFunction, UserFlagRelay> UserFlags { get; set; } = new Dictionary<RelayFunction, UserFlagRelay>
        {
            {RelayFunction.ConnectKl15Site1,  new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 2 } },
            {RelayFunction.ConnectKl15Site2, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 4 } },
        };

        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT00501", NetName = "KL15_CON_IO", Testpoint = "TP2113", Min = 11.95, Typ = 12, Max = 12.05, Unit = "V"},
        };

        public static void Test(int site)
        {
            switch (site)
            {
                case 1:
                    SetSingleUserFlag(RelayFunction.ConnectKl15Site1, Fkt745_VehicleInterface.UserFlags);
                    break;
                case 2:
                    SetSingleUserFlag(RelayFunction.ConnectKl15Site2, Fkt745_VehicleInterface.UserFlags);
                    break;
                default:
                    throw new ArgumentException($"The number {site} can't be passed as the selected site. Panel has two sites.");
            }

            // Analog measurements based on site beeing tested.

            // FCT00501

            foreach (var item in TestItems)
            {
                FktTest.SetTestResult(item);
            }
        }
    }
}
