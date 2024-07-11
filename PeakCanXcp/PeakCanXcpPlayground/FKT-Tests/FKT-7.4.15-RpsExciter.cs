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

    class Fkt7415_RpsExciter : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT01501", NetName = "EXCDRV_DIG_MCU_RPS_N", Testpoint = "TP6327", Min = 2.7, Typ = 3, Max = 3.3, Unit = "V"},
            new Test{TestItem = "FCT01502", NetName = "EXCDRV_DIG_MCU_RPS_P", Testpoint = "TP6328", Min = 2.7, Typ = 3, Max = 3.3, Unit = "V"},
            new Test{TestItem = "FCT01503", NetName = "EXC_b_ANA_RPS_EM_N", Testpoint = "TP6335", Min = 1.42, Typ = 1.47, Max = 1.52, Unit = "V"},
            new Test{TestItem = "FCT01504", NetName = "EXC_b_ANA_RPS_EM_P", Testpoint = "TP6336", Min = 1.42, Typ = 1.47, Max = 1.52, Unit = "V"},
            new Test{TestItem = "FCT01505", NetName = "EXC_DIG_RPS_MCU", Testpoint = "TP6316", Min = 9761, Typ = 9766, Max = 9771, Unit = "Hz"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            DutController.DisableSafeState(xcp, a2l);

            DutController.SetFlag(xcp, a2l, "HwTest_cRslvExcDigOutOvrrdEn", 0x01);

            DutController.SetFlag(xcp, a2l, "HwTest_cRslvExcDigOutNegOvrrd", 0x01);

            DutController.SetFlag(xcp, a2l, "HwTest_cRslvExcDigOutPosOvrrd", 0x01);

            Thread.Sleep(20);

            // FCT01501 (Analog)

            // FCT01502 (Analog)

            // FCT01503 (XCP)
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT01503"), a2l.Measurements["HwTest_rRpsExcNegAdcVolt"]);

            // FCT01503 (Analog)

            // Analog measurements should be multiplied by this factor to be able to compare with the ADC measurements through XCP.
            var voltageDividerFactor = 1.3 / 6.0;

            // FCT01504 (XCP)
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT01504"), a2l.Measurements["HwTest_rRpsExcPosAdcVolt"]);

            // FCT01504 (Analog)

            DutController.SetFlag(xcp, a2l, "HwTest_cRslvExcDigOutPosOvrrd", 0x00);

            DutController.SetFlag(xcp, a2l, "HwTest_cRslvExcDigOutNegOvrrd", 0x00);

            DutController.SetFlag(xcp, a2l, "HwTest_cRslvExcDigOutOvrrdEn", 0x00);

            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT01505"), a2l.Measurements["HwTest_rRslvExcFreq"]);

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
