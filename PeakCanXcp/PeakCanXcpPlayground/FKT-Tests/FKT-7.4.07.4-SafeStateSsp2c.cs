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

    class Fkt7474_SafeStateSsp2c : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT07401", NetName = "nFORCE_ASC_MCU_SSL", Testpoint = "TP4248", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT07402", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07403", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07404", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07405", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07406", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07407", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07408", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07409", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07410", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07411", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07412", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07413", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07414", NetName = "nFORCE_ASC_MCU_SSL", Testpoint = "TP4248", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT07415", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07416", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07417", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07418", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07419", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07420", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07421", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07422", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07423", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07424", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07425", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07426", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // CPLD in normal operation
            DutController.DisableSafeState(xcp, a2l);

            // Power Stage Mode = 0 "normal"
            DutController.SetPowerStageMode(xcp, a2l, 0x00);

            // nFORCE_ASC_MCU_SSL output inactive
            DutController.SetCpldAscOverride(xcp, a2l, 0x00);
            DutController.SetCpldAscOverrideEnable(xcp, a2l, 0x01);

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

            // FCT07401
            // FCT07402
            // FCT07403
            // FCT07404
            // FCT07405
            // FCT07406
            // FCT07407
            // FCT07408
            // FCT07409
            // FCT07410
            // FCT07411
            // FCT07412
            // FCT07413

            // Perform a CPLD ASC.
            DutController.SetCpldAscOverride(xcp, a2l, 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07414
            // FCT07415
            // FCT07416
            // FCT07417
            // FCT07418
            // FCT07419
            // FCT07420
            // FCT07421
            // FCT07422
            // FCT07423
            // FCT07424
            // FCT07425
            // FCT07426

            // Clear value.
            DutController.SetCpldAscOverride(xcp, a2l, 0x01);

            foreach (var test in Fkt7474_SafeStateSsp2c.TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
