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

    class Fkt7410_DcAndPhaseCurrentMeasurement : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT01001", NetName = "5V_ANA", Testpoint = "TP3400", Min = 4.95, Typ = 5, Max = 5.05, Unit = "V"},
            new Test{TestItem = "FCT01002", NetName = "IAC_a_SENS_MCU_1", Testpoint = "TP6128", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT01003", NetName = "IAC_a_SENS_MCU_2", Testpoint = "TP6116", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT01004", NetName = "IAC_a_SENS_MCU_3", Testpoint = "TP6118", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT01005", NetName = "IAC_a_SENS_MCU_4", Testpoint = "TP6108", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT01006", NetName = "IAC_a_SENS_MCU_5", Testpoint = "TP6123", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT01007", NetName = "IAC_a_SENS_MCU_6", Testpoint = "TP6106", Min = 2.45, Typ = 2.5, Max = 2.55, Unit = "V"},
            new Test{TestItem = "FCT01008", NetName = "nFAULT_IAC_SENS_MCU_1", Testpoint = "TP6152", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT01009", NetName = "nFAULT_IAC_SENS_MCU_2", Testpoint = "TP6149", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT01010", NetName = "nFAULT_IAC_SENS_MCU_3", Testpoint = "TP6150", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT01011", NetName = "nFAULT_IAC_SENS_MCU_4", Testpoint = "TP6148", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT01012", NetName = "nFAULT_IAC_SENS_MCU_5", Testpoint = "TP6151", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT01013", NetName = "nFAULT_IAC_SENS_MCU_6", Testpoint = "TP6132", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // FCT01001 (analog)

            // FCT01002-7
            for (int i = 0; i < 6; i++)
            {
                var testItem = $"FCT010{2 + i:D2}";
                FktTest.TestMcuMeasurement(xcp, TestItems.GetTest(testItem), a2l.Measurements["IoEm_rInvPhaseCurrAdcVolt"], arrayIndex: i);
            }

            // FCT01008-13
            for (int i = 0; i < 6; i++)
            {
                var testItem = $"FCT010{8 + i:D2}";
                var xcpParameter = $"IoEcu_rHwDiagPinLvl.InvPhase{i + 1}CurrHigh";
                FktTest.TestMcuMeasurement(xcp, TestItems.GetTest(testItem), a2l.Measurements[xcpParameter]);
            }

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
