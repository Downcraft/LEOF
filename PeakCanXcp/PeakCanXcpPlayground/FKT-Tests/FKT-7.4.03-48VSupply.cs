namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Fkt743_48VSupply : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT00301", NetName = "48V", Testpoint = "TP3200", Min = 47.5, Typ = 48, Max = 48.5, Unit = "V"},
            new Test{TestItem = "FCT00302", NetName = "14V7", Testpoint = "TP3214", Min = 14.3, Typ = 14.7, Max = 15.1, Unit = "V"},
            new Test{TestItem = "FCT00304", NetName = "3V3_CPLD", Testpoint = "TP3500", Min = 3.2, Typ = 3.3, Max = 3.4, Unit = "V"},
            new Test{TestItem = "FCT00305", NetName = "Supply current 48V", Testpoint = "External", Min = 0.02, Typ = 0.023, Max = 0.025, Unit = "A"},
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
