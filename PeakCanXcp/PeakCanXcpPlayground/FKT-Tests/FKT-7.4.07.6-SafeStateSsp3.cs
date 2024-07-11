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

    class Fkt7476_SafeStateSsp3 : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT07601", NetName = "nFAULT_FS0B_SBC_SSL", Testpoint = "TP3427", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07602", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07603", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07604", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07605", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07606", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07607", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07608", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07609", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07610", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07611", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07612", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07613", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07614", NetName = "nFAULT_FS0B_SBC_SSL", Testpoint = "TP3427", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07615", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07616", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07617", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07618", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07619", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07620", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07621", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07622", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07623", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07624", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07625", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07626", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07627", NetName = "nFAULT_FS0B_SBC_SSL", Testpoint = "TP3427", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07628", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07629", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07630", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07631", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07632", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07633", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07634", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07635", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07636", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07637", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07638", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07639", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // Precondition:
            DutController.DisableSafeState(xcp, a2l);
            DutController.SetPowerStageMode(xcp, a2l, 0x00);

            // Enable FS0b-Test override:
            DutController.SetFlag(xcp, a2l, "HwTest_cMcuSsp2Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cMcuSsp2OvrrdEn", 0x01);

            // Activate HS ASC via manual override:
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

            // FCT07601
            // FCT07602
            // FCT07603
            // FCT07604
            // FCT07605
            // FCT07606
            // FCT07607
            // FCT07608
            // FCT07609
            // FCT07610
            // FCT07611
            // FCT07612
            // FCT07613

            // Set FS0b override active:
            DutController.SetFlag(xcp, a2l, "HwTest_cMcuSsp2Ovrrd", 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07614
            // FCT07615
            // FCT07616
            // FCT07617
            // FCT07618
            // FCT07619
            // FCT07620
            // FCT07621
            // FCT07622
            // FCT07623
            // FCT07624
            // FCT07625
            // FCT07626

            // Set all overrides to 0

            // Set highside gate driver outputs to HIGH.
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._2_.Hs", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._3_.Hs", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._4_.Hs", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._5_.Hs", 0x00);

            // Set highside gate driver outputs to LOW.
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._0_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._1_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._2_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._3_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._4_.Ls", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.PhaseOvrrd._5_.Ls", 0x00);

            // Set
            DutController.SetFlag(xcp, a2l, "HwTest_cSbcFS0bCmdOvrrdEn", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cSbcFS0bCmdAssertOvrrd", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cSbcFS0bCmdReleaseOvrrd", 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07627
            // FCT07628
            // FCT07629
            // FCT07630
            // FCT07631
            // FCT07632
            // FCT07633
            // FCT07634
            // FCT07635
            // FCT07636
            // FCT07637
            // FCT07638
            // FCT07639

            // Clear
            DutController.SetFlag(xcp, a2l, "HwTest_cSbcFS0bCmdOvrrdEn", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cSbcFS0bCmdAssertOvrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cSbcFS0bCmdReleaseOvrrd", 0x00);

            foreach (var test in Fkt7476_SafeStateSsp3.TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
