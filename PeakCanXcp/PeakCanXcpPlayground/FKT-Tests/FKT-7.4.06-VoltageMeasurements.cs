namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AtosF;
    using static AtosF.modAtosF;
    using static AtosF.modAtosF2;
    using PeakCanXcp;

    class Fkt746_VoltageMeasurements : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT00601", NetName = "U_KL_MSMT", Testpoint = "TP2112", Min = 1.28, Typ = 1.33, Max = 1.38, Unit = "V"},
            new Test{TestItem = "FCT00602", NetName = "U_KL30_MSMT", Testpoint = "TP1502", Min = 1.04, Typ = 1.09, Max = 1.14, Unit = "V"},
            new Test{TestItem = "FCT00603", NetName = "U_KL30C_MSMT", Testpoint = "TP1401", Min = 1.04, Typ = 1.09, Max = 1.14, Unit = "V"},
            new Test{TestItem = "FCT00604", NetName = "U_14V7", Testpoint = "TP3208", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT00605", NetName = "U_13V5", Testpoint = "TP3130", Min = 2.25, Typ = 2.3, Max = 2.35, Unit = "V"},
            new Test{TestItem = "FCT00606", NetName = "U_14V7_COMB_SUP_MCU", Testpoint = "TP3600", Min = 2.47, Typ = 2.52, Max = 2.57, Unit = "V"},
            new Test{TestItem = "FCT00607", NetName = "MUX_SBC_MCU", Testpoint = "TP3425", Min = 0, Typ = 0, Max = 0, Unit = "V"},
            new Test{TestItem = "FCT00608", NetName = "U_CL40F_ANA_MSMT´_MCU", Testpoint = "TP4132", Min = 3.15, Typ = 3.2, Max = 3.25, Unit = "V"},
            new Test{TestItem = "FCT00609", NetName = "U_CL40F_OV_ANA_MSMT_MCU", Testpoint = "TP4129", Min = 2.12, Typ = 2.17, Max = 2.22, Unit = "V"},
            new Test{TestItem = "FCT00610", NetName = "5V_ANA", Testpoint = "TP3400", Min = 4.9, Typ = 5, Max = 5.1, Unit = "V"},
            new Test{TestItem = "FCT00611", NetName = "U_5V_BN12_MSMT", Testpoint = "TP1505", Min = 4.7, Typ = 4.9, Max = 5.1, Unit = "V"},
            new Test{TestItem = "FCT00612", NetName = "3V3_MCU", Testpoint = "TP3416", Min = 3.2, Typ = 3.3, Max = 3.4, Unit = "V"},
            new Test{TestItem = "FCT00613", NetName = "3V3_CPLD", Testpoint = "TP3500", Min = 3.2, Typ = 3.3, Max = 3.4, Unit = "V"},
            new Test{TestItem = "FCT00614", NetName = "2V5_MCU_REF", Testpoint = "TP4135", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT00615", NetName = "1V25_MCU", Testpoint = "TP4109", Min = 1.19, Typ = 1.25, Max = 1.31, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // FCT00601
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00601"), a2l.Measurements["IoEcu_rLvBnet.WkupAdcVolt"]);

            // FCT00602
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00602"), a2l.Measurements["IoEcu_rLvBnet.SupVoltAdcVolt"]);

            // FCT00603
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00603"), a2l.Measurements["IoEcu_rCrash.CrashAdcVolt"]);

            // FCT00604
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00604"), a2l.Measurements["IoEcu_rEcuVolt2AdcVolt.IntrnlSup3"]);

            // FCT00605
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00605"), a2l.Measurements["IoEcu_rEcuVolt2AdcVolt.IntrnlSup4"]);

            // FCT00606
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00606"), a2l.Measurements["HwTest_rPwrStgHsSupAdcVolt"]);

            // FCT00607

            // FCT00608
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00608"), a2l.Measurements["IoEm_rDcLinkVolt.AdcVolt"]);

            // FCT00609

            // FCT00610
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00610"), a2l.Measurements["HwTest_rPmsVDDM"]);

            // FCT00611
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00611"), a2l.Measurements["IoEcu_rEcuVolt2AdcVolt.IntrnlSup7"]);

            // FCT00612
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00612"), a2l.Measurements["IoEcu_rEcuVolt2AdcVolt.IntrnlSup2"]);

            // FCT00613
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00613"), a2l.Measurements["IoEcu_rEcuVolt2AdcVolt.IntrnlSup1"]);

            // FCT00614
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00614"), a2l.Measurements["IoEcu_rEcuVolt2AdcVolt.Ref"]);

            // FCT00615
            TestMcuMeasurement(xcp, TestItems.GetTest("FCT00615"), a2l.Measurements["HwTest_rPmsVDD"]);

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
