namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using PeakCanXcp;

    class Fkt742_VoltageAndCurrentInNormalMode : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT00201", NetName = "13V5", Testpoint = "TP3154", Min = 13, Typ = 13.5, Max = 14.5, Unit = "V"},
            new Test{TestItem = "FCT00202", NetName = "5V_BN12", Testpoint = "TP3127", Min = 4.7, Typ = 4.9, Max = 5.1, Unit = "V"},
            new Test{TestItem = "FCT00203", NetName = "8V_BN12", Testpoint = "TP3122", Min = 8, Typ = 8.3, Max = 8.6, Unit = "V"},
            new Test{TestItem = "FCT00204", NetName = "Supply current KL30", Testpoint = "External", Min = 0.2, Typ = 0.225, Max = 0.250, Unit = "A"},
        };

        public static void Test(int site)
        {
            // Power on (12V).

            Thread.Sleep(50);

            // Analog measurements based on site beeing tested.

            // FCT00201
            // FCT00202
            // FCT00203
            // FCT00204

            foreach (var item in TestItems)
            {
                FktTest.SetTestResult(item);
            }
        }
    }
}
