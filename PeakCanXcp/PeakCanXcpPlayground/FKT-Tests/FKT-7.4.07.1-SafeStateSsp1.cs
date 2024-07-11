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

    class Fkt7471_SafeStateSsp1 : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT07101", NetName = "PWM_HS_b_SSL_GDRV_1", Testpoint = "TP4319.1", Min = 0, Typ = 0, Max = 0, Unit = "boolean"},
            new Test{TestItem = "FCT07102", NetName = "PWM_HS_b_SSL_GDRV_2", Testpoint = "TP4319.2", Min = 0, Typ = 0, Max = 0, Unit = "boolean"},
            new Test{TestItem = "FCT07103", NetName = "PWM_HS_b_SSL_GDRV_3", Testpoint = "TP4319.3", Min = 0, Typ = 0, Max = 0, Unit = "boolean"},
            new Test{TestItem = "FCT07104", NetName = "PWM_HS_b_SSL_GDRV_4", Testpoint = "TP4319.4", Min = 0, Typ = 0, Max = 0, Unit = "boolean"},
            new Test{TestItem = "FCT07105", NetName = "PWM_HS_b_SSL_GDRV_5", Testpoint = "TP4319.5", Min = 0, Typ = 0, Max = 0, Unit = "boolean"},
            new Test{TestItem = "FCT07106", NetName = "PWM_HS_b_SSL_GDRV_6", Testpoint = "TP4319.6", Min = 0, Typ = 0, Max = 0, Unit = "boolean"},
            new Test{TestItem = "FCT07107", NetName = "PWM_LS_b_SSL_GDRV_1", Testpoint = "TP5307.1", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT07108", NetName = "PWM_LS_b_SSL_GDRV_2", Testpoint = "TP5307.2", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT07109", NetName = "PWM_LS_b_SSL_GDRV_3", Testpoint = "TP5307.3", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT07110", NetName = "PWM_LS_b_SSL_GDRV_4", Testpoint = "TP5307.4", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT07111", NetName = "PWM_LS_b_SSL_GDRV_5", Testpoint = "TP5307.5", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT07112", NetName = "PWM_LS_b_SSL_GDRV_6", Testpoint = "TP5307.6", Min = 1, Typ = 1, Max = 1, Unit = "boolean"},
            new Test{TestItem = "FCT07113", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07114", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07115", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07116", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07117", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07118", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07119", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07120", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07121", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07122", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07123", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07124", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // CPLD in normal operation
            DutController.DisableSafeState(xcp, a2l);

            // HwTest_cPwrStgPar.Mode = 0 "normal"
            DutController.SetPowerStageMode(xcp, a2l, 0x00);

            // FCT07101
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07101"), "HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs");

            // FCT07102
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07102"), "HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs");

            // FCT07103
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07103"), "HwTest_cPwrStgPar.PhaseOvrrd._2_.Hs");

            // FCT07104
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07104"), "HwTest_cPwrStgPar.PhaseOvrrd._3_.Hs");

            // FCT07105
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07105"), "HwTest_cPwrStgPar.PhaseOvrrd._4_.Hs");

            // FCT07106
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07106"), "HwTest_cPwrStgPar.PhaseOvrrd._5_.Hs");

            // FCT07107
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07107"), "HwTest_cPwrStgPar.PhaseOvrrd._0_.Ls");

            // FCT07108
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07108"), "HwTest_cPwrStgPar.PhaseOvrrd._1_.Ls");

            // FCT07109
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07109"), "HwTest_cPwrStgPar.PhaseOvrrd._2_.Ls");

            // FCT07110
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07110"), "HwTest_cPwrStgPar.PhaseOvrrd._3_.Ls");

            // FCT07111
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07111"), "HwTest_cPwrStgPar.PhaseOvrrd._4_.Ls");

            // FCT07112
            SetFlagAndTest(xcp, a2l, TestItems.GetTest("FCT07112"), "HwTest_cPwrStgPar.PhaseOvrrd._5_.Ls");

            // Analog measurements based on site beeing tested.

            // FCT07113
            // FCT07114
            // FCT07115
            // FCT07116
            // FCT07117
            // FCT07118
            // FCT07119
            // FCT07120
            // FCT07121
            // FCT07122
            // FCT07123
            // FCT07124

            // HwTest_cPwrStgPar.Mode = 0 "normal"
            DutController.SetPowerStageMode(xcp, a2l, 0x00);

            foreach (var test in Fkt7471_SafeStateSsp1.TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
