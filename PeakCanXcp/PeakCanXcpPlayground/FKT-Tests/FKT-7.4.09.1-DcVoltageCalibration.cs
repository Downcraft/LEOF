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

    class Fkt7491_DcVoltageCalibration : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT09101", NetName = "U_CL40F_ANA_MSMT_MCU", Testpoint = "TP4131", Min = 3.15, Typ = 3.2, Max = 3.25, Unit = "V"},
            new Test{TestItem = "FCT09102", NetName = "U_CL40F_OV_ANA_MSMT_MCU", Testpoint = "TP4129", Min = 2.12, Typ = 2.17, Max = 2.22, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // FCT09101
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT09101"), a2l.Measurements["IoEm_rDcLinkVolt.AdcVolt"]);

            // Calculate calibration data

            // Write calibration data

            // FCT09102

            foreach (var test in Fkt7491_DcVoltageCalibration.TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
