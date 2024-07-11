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

    class Fkt7416_RpsReceiver : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT01603", NetName = "COS_ANA_RPS_MCU_N", Testpoint = "TP6331", Min = -1.57, Typ = -1.52, Max = -1.47, Unit = "V"},
            new Test{TestItem = "FCT01604", NetName = "COS_ANA_RPS_MCU_P", Testpoint = "TP6332", Min = -1.57, Typ = -1.52, Max = -1.47, Unit = "V"},
            new Test{TestItem = "FCT01607", NetName = "SIN_ANA_RPS_MCU_N", Testpoint = "TP6337", Min = -1.57, Typ = -1.52, Max = -1.47, Unit = "V"},
            new Test{TestItem = "FCT01608", NetName = "SIN_ANA_RPS_MCU_P", Testpoint = "TP6338", Min = -1.57, Typ = -1.52, Max = -1.47, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            DutController.DisableSafeState(xcp, a2l);

            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT01603"), a2l.Measurements["IoEm_rRslvr.CosVolt"]);
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT01604"), a2l.Measurements["IoEm_rRslvr.CosVolt"]);
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT01607"), a2l.Measurements["IoEm_rRslvr.SinVolt"]);
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT01608"), a2l.Measurements["IoEm_rRslvr.SinVolt"]);

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
