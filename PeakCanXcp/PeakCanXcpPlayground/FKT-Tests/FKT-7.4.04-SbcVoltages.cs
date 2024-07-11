namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Fkt744_SbcVoltages : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT00401", NetName = "6V5_VPRE", Testpoint = "TP3420", Min = 6, Typ = 6.5, Max = 7.0, Unit = "V"},
            new Test{TestItem = "FCT00402", NetName = "3V3_MCU", Testpoint = "TP3416", Min = 3.2, Typ = 3.3, Max = 3.4, Unit = "V"},
            new Test{TestItem = "FCT00403", NetName = "3V3_CPLD", Testpoint = "TP3500", Min = 3.2, Typ = 3.3, Max = 3.4, Unit = "V"},
            new Test{TestItem = "FCT00404", NetName = "5V_ANA", Testpoint = "TP3400", Min = 4.9, Typ = 5.0, Max = 5.1, Unit = "V"},
            new Test{TestItem = "FCT00405", NetName = "1V25_MCU", Testpoint = "TP4109", Min = 1.19, Typ = 1.25, Max = 1.31, Unit = "V"},
        };

        public static void Test(int site)
        {
            // Power on (48V).

            Thread.Sleep(100);

            // Analog measurements based on site beeing tested.

            // FCT00301
            // FCT00302
            // FCT00304
            // FCT00305

            foreach (var item in TestItems)
            {
                FktTest.SetTestResult(item);
            }
        }
    }
}
