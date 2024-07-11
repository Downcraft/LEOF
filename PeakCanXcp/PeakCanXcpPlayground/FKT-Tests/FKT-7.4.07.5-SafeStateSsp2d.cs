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

    class Fkt7475_SafeStateSsp2d : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT07501", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07502", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07503", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07504", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07505", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07506", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07507", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07508", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07509", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07510", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07511", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07512", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07513", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07514", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07515", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07516", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07517", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07518", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07519", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07520", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07521", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07522", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07523", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07524", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07525", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07526", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07527", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07528", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07529", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07530", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07531", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07532", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07533", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07534", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07535", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07536", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07537", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07538", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07539", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07540", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07541", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07542", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07543", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07544", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07545", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07546", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07547", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07548", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07549", NetName = "DISABLE_PWM_HS_MCU_SSL", Testpoint = "", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT07550", NetName = "FORCE_HS123_ASC_MCU_SSL", Testpoint = "TP4193", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT07551", NetName = "FORCE_HS456_ASC_MCU_SSL", Testpoint = "TP4194", Min = 3.25, Typ = 3.3, Max = 3.35, Unit = "V"},
            new Test{TestItem = "FCT07552", NetName = "FORCE_LS123_ASC_MCU_SSL", Testpoint = "TP4195", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07553", NetName = "FORCE_LS456_ASC_MCU_SSL", Testpoint = "TP4196", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07554", NetName = "GH_GDRV_PM_1", Testpoint = "TP5321.1", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07555", NetName = "GH_GDRV_PM_2", Testpoint = "TP5321.2", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07556", NetName = "GH_GDRV_PM_3", Testpoint = "TP5321.3", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07557", NetName = "GH_GDRV_PM_4", Testpoint = "TP5321.4", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07558", NetName = "GH_GDRV_PM_5", Testpoint = "TP5321.5", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07559", NetName = "GH_GDRV_PM_6", Testpoint = "TP5321.6", Min = 11.16, Typ = 13, Max = 16.39, Unit = "V"},
            new Test{TestItem = "FCT07560", NetName = "GL_GDRV_PM_1", Testpoint = "TP5309.1", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07561", NetName = "GL_GDRV_PM_2", Testpoint = "TP5309.2", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07562", NetName = "GL_GDRV_PM_3", Testpoint = "TP5309.3", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07563", NetName = "GL_GDRV_PM_4", Testpoint = "TP5309.4", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07564", NetName = "GL_GDRV_PM_5", Testpoint = "TP5309.5", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT07565", NetName = "GL_GDRV_PM_6", Testpoint = "TP5309.6", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            // Pre-Condition:
            DutController.DisableSafeState(xcp, a2l);
            DutController.SetPowerStageMode(xcp, a2l, 0x00);

            // Enable FORCE output override:
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHs123Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHs456Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscLs123Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscLs456Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHsLsOvrrdEn", 0x01);

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

            // FCT07501
            // FCT07502
            // FCT07503
            // FCT07504
            // FCT07505
            // FCT07506
            // FCT07507
            // FCT07508
            // FCT07509
            // FCT07510
            // FCT07511
            // FCT07512

            // Disable CPLD (sleep mode):
            DutController.SetFlag(xcp, a2l, "IoEcu_cPld.EnOvrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "IoEcu_cPld.EnOvrrdEn", 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07513
            // FCT07514
            // FCT07515
            // FCT07516
            // FCT07517
            // FCT07518
            // FCT07519
            // FCT07520
            // FCT07521
            // FCT07522
            // FCT07523
            // FCT07524

            // Activate LS ASC via FORCE outputs:
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscLs123Ovrrd", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscLs456Ovrrd", 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07525
            // FCT07526
            // FCT07527
            // FCT07528
            // FCT07529
            // FCT07530
            // FCT07531
            // FCT07532
            // FCT07533
            // FCT07534
            // FCT07535
            // FCT07536

            // Activate HS ASC via FORCE outputs:
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscLs123Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscLs456Ovrrd", 0x00);
            FktTest.DelayUs(3);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHs123Ovrrd", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHs456Ovrrd", 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07537
            // FCT07538
            // FCT07539
            // FCT07540
            // FCT07541
            // FCT07542
            // FCT07543
            // FCT07544
            // FCT07545
            // FCT07546
            // FCT07547
            // FCT07548

            // Analog measurements based on site beeing tested.

            // FCT07549

            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHsLsOvrrdEn", 0x01);

            // Analog measurements based on site beeing tested.

            // FCT07550
            // FCT07551
            // FCT07552
            // FCT07553

            // Analog measurements based on site beeing tested.

            // FCT07554
            // FCT07555
            // FCT07556
            // FCT07557
            // FCT07558
            // FCT07559
            // FCT07560
            // FCT07561
            // FCT07562
            // FCT07563
            // FCT07564
            // FCT07565

            // Clear:
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHs123Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHs456Ovrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "HwTest_cDrvForceAscHsLsOvrrdEn", 0x00);
            DutController.SetFlag(xcp, a2l, "IoEcu_cPld.EnOvrrd", 0x00);
            DutController.SetFlag(xcp, a2l, "IoEcu_cPld.EnOvrrdEn", 0x00);

            foreach (var test in Fkt7475_SafeStateSsp2d.TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }
    }
}
