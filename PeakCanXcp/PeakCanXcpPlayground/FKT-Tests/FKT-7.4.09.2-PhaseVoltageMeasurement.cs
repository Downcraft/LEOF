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

    class Fkt7492_PhaseVoltageMeasurement : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT09201", NetName = "FAULT_UDC_UV_MSMT_MCU", Testpoint = "TP6201", Min = 2.12, Typ = 2.17, Max = 2.22, Unit = "V"},
            new Test{TestItem = "FCT09202", NetName = "FAULT_UDC_UV_MSMT_SSL", Testpoint = "TP4252", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT09203", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09204", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09205", NetName = "PHASE_ANA_MSMT_MCU_1", Testpoint = "TP6264.1", Min = 3.85, Typ = 3.9, Max = 3.95, Unit = "V"},
            new Test{TestItem = "FCT09206", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09207", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09208", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09209", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09210", NetName = "PHASE_ANA_MSMT_MCU_2", Testpoint = "TP6264.2", Min = 3.85, Typ = 3.9, Max = 3.95, Unit = "V"},
            new Test{TestItem = "FCT09211", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09212", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09213", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09214", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09215", NetName = "PHASE_ANA_MSMT_MCU_3", Testpoint = "TP6264.3", Min = 3.85, Typ = 3.9, Max = 3.95, Unit = "V"},
            new Test{TestItem = "FCT09216", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09217", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09218", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09219", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09220", NetName = "PHASE_ANA_MSMT_MCU_4", Testpoint = "TP6252.1", Min = 3.85, Typ = 3.9, Max = 3.95, Unit = "V"},
            new Test{TestItem = "FCT09221", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09222", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09223", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09224", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09225", NetName = "PHASE_ANA_MSMT_MCU_5", Testpoint = "TP6252.2", Min = 3.85, Typ = 3.9, Max = 3.95, Unit = "V"},
            new Test{TestItem = "FCT09226", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09227", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09228", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09229", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 100, Typ = 100, Max = 100, Unit = "%"},
            new Test{TestItem = "FCT09230", NetName = "PHASE_ANA_MSMT_MCU_6", Testpoint = "TP6252.3", Min = 3.85, Typ = 3.9, Max = 3.95, Unit = "V"},
            new Test{TestItem = "FCT09231", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09232", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 0, Typ = 0, Max = 0, Unit = "%"},
            new Test{TestItem = "FCT09233", NetName = "DIAG_FAULT_UVP_MCU_SSL", Testpoint = "TP6240", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT09234", NetName = "DIAG_nFAULT_OVP_MCU_SSL", Testpoint = "TP206", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT09235", NetName = "nFAULT_UDC_OV_MSMT_MCU_SSL", Testpoint = "TP4264", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT09236", NetName = "UDC_UV_ANA_MSMT_MCU", Testpoint = "TP6242", Min = 0.814, Typ = 0.864, Max = 0.914, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // FCT09201
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT09201"), a2l.Measurements["IoEcu_rHwDiagPinLvl.DcLinkVoltLow"]);

            // FCT09202 (Analog)

            // Precondition:
            DutController.DisableSafeState(xcp, a2l);
            DutController.SetPowerStageMode(xcp, a2l, 0x00);
            DutController.SetFlag(xcp, a2l, "IoEm_cInvPhaseDucySpOvrrdEn", 0x01);
            DutController.SetFlag(xcp, a2l, "IoEm_cInvPhaseDucySpOvrrdEnFrc", 0x01);

            for (int i = 1; i <= 6; i++)
            {
                TestPhaseVoltage(xcp, a2l, i, site);
            }

            // FCT09233
            DutController.SetFlag(xcp, a2l, "IoEcu_cHwDiag.DcLinkVoltDiagOvrrdEn", 0x01);
            DutController.SetFlag(xcp, a2l, "IoEcu_cHwDiag.DcLinkVoltHighDiagOvrrd", 0x01);
            // FCT09233 (Analog)

            // FCT09234
            DutController.SetFlag(xcp, a2l, "IoEcu_cHwDiag.DcLinkVoltLowDiagOvrrd", 0x01);
            // FCT09234 (Analog)

            // FCT09235
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest("FCT09235"), a2l.Measurements["IoEcu_rHwDiagPinLvl.DcLinkVoltHigh"]);

            // FCT09236
            // tbd

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }

        private static void TestPhaseVoltage(Xcp xcp, A2lParser a2l, int phase, int site)
        {
            var testItemOffset = 5 * (phase - 1);

            // FCT092(03 + testItemOffset) 
            // FCT092(04 + testItemOffset)
            DutController.SetPwm(xcp, a2l, dutyCycle: 100, phase: phase);
            var pwm = DutController.GetPwm(xcp, a2l, phase: 1);
            TestItems.GetTest($"FCT092{3 + testItemOffset:D2}").Result = (pwm == 100) ? PASS : FAIL;
            TestItems.GetTest($"FCT092{4 + testItemOffset:D2}").Result = (pwm == 100) ? PASS : FAIL;

            Thread.Sleep(50);

            // FCT092(05 + testItemOffset) 
            FktTest.TestMcuMeasurement(xcp, TestItems.GetTest($"FCT092{5 + testItemOffset:D2}"), a2l.Measurements["IoEm_rInvPhaseVoltAdcVolt"], 0);

            // FCT092(05 + testItemOffset) (Analog)

            // FCT092(06 + testItemOffset)
            // FCT092(07 + testItemOffset)
            DutController.SetPwm(xcp, a2l, dutyCycle: 0, phase: phase);
            pwm = DutController.GetPwm(xcp, a2l, phase: 1);
            TestItems.GetTest($"FCT092{6 + testItemOffset:D2}").Result = (pwm == 0) ? PASS : FAIL;
            TestItems.GetTest($"FCT092{7 + testItemOffset:D2}").Result = (pwm == 0) ? PASS : FAIL;
        }
    }
}
