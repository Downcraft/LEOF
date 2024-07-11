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

    class Fkt7473_SafeStateSsp2b : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT07301", NetName = "DIAG_nFAULT_OVP_MCU_SSL", Testpoint = "TP6206", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT07302", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07303", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07304", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07305", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07306", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07307", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07308", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07309", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07310", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07311", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07312", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07313", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07314", NetName = "DIAG_nFAULT_OVP_MCU_SSL", Testpoint = "TP6206", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT07315", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07316", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07317", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07318", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07319", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07320", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07321", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07322", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07323", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07324", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07325", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07326", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // CPLD in normal operation
            DutController.DisableSafeState(xcp, a2l);

            // Power Stage Mode = 0 "normal"
            DutController.SetPowerStageMode(xcp, a2l, 0x00);

            // Enable Override of 48V Voltage Diagnostic.
            DutController.Set_48V_VoltageDianosticOverride(xcp, a2l, 0x01);

            // Clear any voltage override faults.
            DutController.Set_48V_OvervoltageFault(xcp, a2l, 0x00);
            DutController.Set_48V_UndervoltageFault(xcp, a2l, 0x00);

            // Power Stage mode = 2 "manual override"
            DutController.SetPowerStageMode(xcp, a2l, 0x02);

            // Set highside gate driver outputs to HIGH.

            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._2_.Hs", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._3_.Hs", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._4_.Hs", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._5_.Hs", 0x01);

            // Set highside gate driver outputs to LOW.

            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._0_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._1_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._2_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._3_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._4_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._5_.Ls", 0x00);

            // Analog measurements based on site beeing tested.

            // FCT07301
            // FCT07302
            // FCT07303
            // FCT07304
            // FCT07305
            // FCT07306
            // FCT07307
            // FCT07308
            // FCT07309
            // FCT07310
            // FCT07311
            // FCT07312
            // FCT07313

            // Simulate an overvoltage fault.
            DutController.Set_48V_OvervoltageFault(xcp, a2l, 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07314
            // FCT07315
            // FCT07316
            // FCT07317
            // FCT07318
            // FCT07319
            // FCT07320
            // FCT07321
            // FCT07322
            // FCT07323
            // FCT07324
            // FCT07325
            // FCT07326

            // Clear the overvoltage fault.
            DutController.Set_48V_OvervoltageFault(xcp, a2l, 0x00);

            // HwTest_cPwrStgPar.Mode = 0 "normal"
            DutController.SetPowerStageMode(xcp, a2l, 0x00);

            foreach (var test in Fkt7473_SafeStateSsp2b.TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
