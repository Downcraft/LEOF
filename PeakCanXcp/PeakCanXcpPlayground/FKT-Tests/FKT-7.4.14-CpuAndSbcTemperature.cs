namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AtosF;
    using static AtosF.modAtosF;
    using static AtosF.modAtosF2;
    using PeakCanXcp;

    class Fkt7414_CpuAndSbcTemperature : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT01401", NetName = "", Testpoint = "", Min = 0, Typ = 0, Max =0, Unit = "°C"},
            new Test{TestItem = "FCT01402", NetName = "", Testpoint = "", Min = 23, Typ = 36, Max = 60, Unit = "°C"},
            new Test{TestItem = "FCT01403", NetName = "", Testpoint = "", Min = 23, Typ = 36, Max = 60, Unit = "°C"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            DutController.DisableSafeState(xcp, a2l);

            // FCT01401

            // FCT01402
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT01402"), a2l.Measurements["V_DTS_MCU_CoreTemperature"]);

            // FCT01403
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT01403"), a2l.Measurements["V_DTS_MCU_PMS_Temperature"]);

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
