namespace Program
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Spea;
    using Spea.Instruments;
    using Spea.TestEnvironment;
    using Spea.TestFramework;

    internal class Fct8322_SystemBasisChip : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8322_SystemBasisChip(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "System Basis Chip";

        protected override string Id { get; set; } = "8.3.2.2";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT022002", Remark = "6V5Int (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP2x2703"), new TestPoint("M1x9900") }, Minimal = 6.25, Nominal = 6.5, Maximal = 6.75, Unit = "V" },
            new TestItem { Descriptor = "FCT022003", Remark = "NetR4x2702_1 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP7x2703"), new TestPoint("M1x9900") }, Minimal = 0.784, Nominal = 0.8, Maximal = 2.0, Unit = "V" },
            new TestItem { Descriptor = "FCT022004", Remark = "NetC8x2702_1 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2703"), new TestPoint("M1x9900") }, Minimal = 0.784, Nominal = 0.8, Maximal = 0.816, Unit = "V" },
            new TestItem { Descriptor = "FCT022005", Remark = "nSbcMcuRst_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP8x2703"), new TestPoint("M1x9900") }, Minimal = 3.66, Nominal = 4.675, Maximal = 5.69, Unit = "V" },
            new TestItem { Descriptor = "FCT022006", Remark = "nSbcSSp3 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP13x2703"), new TestPoint("M1x9900") }, Minimal = 3.81, Nominal = 4.64, Maximal = 5.47, Unit = "V" },
            new TestItem { Descriptor = "FCT022008", Remark = "SbcIntURef2V5_Phy_U (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP6x2703"), new TestPoint("M1x9900") }, Minimal = 2.31, Nominal = 2.5, Maximal = 2.61, Unit = "V" },
            new TestItem { Descriptor = "FCT022010", Remark = "Terminal30_Phy_U (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP6x2703"), new TestPoint("M1x9900") }, Minimal = 2.6, Nominal = 2.7, Maximal = 2.8, Unit = "V" },
            new TestItem { Descriptor = "FCT022012", Remark = "5V0 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x2703"), new TestPoint("M1x9900") }, Minimal = 4.87, Nominal = 5, Maximal = 5.25, Unit = "V" },
            new TestItem { Descriptor = "FCT022013", Remark = "6V5Int (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP2x2703"), new TestPoint("M1x9900") }, Minimal = 6.25, Nominal = 6.5, Maximal = 6.75, Unit = "V" },
            new TestItem { Descriptor = "FCT022014", Remark = "5V0COM (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP3x2703"), new TestPoint("M1x9900") }, Minimal = 4.8, Nominal = 5, Maximal = 5.2, Unit = "V" },
            new TestItem { Descriptor = "FCT022015", Remark = "5V0An (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP4x2703"), new TestPoint("M1x9900") }, Minimal = 4.95, Nominal = 5, Maximal = 5.05, Unit = "V" },
            new TestItem { Descriptor = "FCT022016", Remark = "5V0Aux (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP5x2703"), new TestPoint("M1x9900") }, Minimal = 4.85, Nominal = 5, Maximal = 5.25, Unit = "V" },
            new TestItem { Descriptor = "FCT022018", Remark = "SbcIntURef2V5_Phy_U (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP6x2703"), new TestPoint("M1x9900") }, Minimal = 2.31, Nominal = 2.5, Maximal = 2.61, Unit = "V" },
            new TestItem { Descriptor = "FCT022020", Remark = "WakeUpIvtrSply (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP6x2703"), new TestPoint("M1x9900") }, Minimal = 0.88, Nominal = 1, Maximal = 1.15, Unit = "V" },
            new TestItem { Descriptor = "FCT022021", Remark = "5V0 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x2703"), new TestPoint("M1x9900") }, Minimal = 4.87, Nominal = 5, Maximal = 5.13, Unit = "V" },
            new TestItem { Descriptor = "FCT022022", Remark = "6V5Int (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP2x2703"), new TestPoint("M1x9900") }, Minimal = 6, Nominal = 6.5, Maximal = 7, Unit = "V" },
            new TestItem { Descriptor = "FCT022023", Remark = "5V0COM (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP3x2703"), new TestPoint("M1x9900") }, Minimal = 4.8, Nominal = 5, Maximal = 5.2, Unit = "V" },
            new TestItem { Descriptor = "FCT022024", Remark = "5V0An (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP4x2703"), new TestPoint("M1x9900") }, Minimal = 4.95, Nominal = 5, Maximal = 5.05, Unit = "V" },
            new TestItem { Descriptor = "FCT022025", Remark = "5V0Aux (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP5x2703"), new TestPoint("M1x9900") }, Minimal = 4.85, Nominal = 5, Maximal = 5.25, Unit = "V" },
            new TestItem { Descriptor = "FCT022026", Remark = "nSbcMcuRst_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP8x2703"), new TestPoint("M1x9900") }, Minimal = 3.66, Nominal = 4.675, Maximal = 5.69, Unit = "V" },
            new TestItem { Descriptor = "FCT022027", Remark = "nSbcSSp3 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP13x2703"), new TestPoint("M1x9900") }, Minimal = 3.81, Nominal = 4.64, Maximal = 5.47, Unit = "V" },
            new TestItem { Descriptor = "FCT022028", Remark = "5V0 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x2703"), new TestPoint("M1x9900") }, Minimal = 4.87, Nominal = 5, Maximal = 5.13, Unit = "V" },
            new TestItem { Descriptor = "FCT022029", Remark = "6V5Int (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP2x2703"), new TestPoint("M1x9900") }, Minimal = 6.25, Nominal = 6.5, Maximal = 6.75, Unit = "V" },
            new TestItem { Descriptor = "FCT022030", Remark = "5V0COM (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP3x2703"), new TestPoint("M1x9900") }, Minimal = 4.8, Nominal = 5, Maximal = 5.2, Unit = "V" },
            new TestItem { Descriptor = "FCT022031", Remark = "5V0An (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP4x2703"), new TestPoint("M1x9900") }, Minimal = 4.95, Nominal = 5, Maximal = 5.05, Unit = "V" },
            new TestItem { Descriptor = "FCT022032", Remark = "5V0Aux (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP5x2703"), new TestPoint("M1x9900") }, Minimal = 4.85, Nominal = 5, Maximal = 5.25, Unit = "V" },
            new TestItem { Descriptor = "FCT022033", Remark = "nSbcMcuRst_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP8x2703"), new TestPoint("M1x9900") }, Minimal = 3.66, Nominal = 4.675, Maximal = 5.69, Unit = "V" },
            new TestItem { Descriptor = "FCT022034", Remark = "nSbcSSp3 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP13x2703"), new TestPoint("M1x9900") }, Minimal = 3.81, Nominal = 4.64, Maximal = 5.47, Unit = "V" },
            new TestItem { Descriptor = "FCT022035", Remark = "nSbcMcuRst_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP8x2703"), new TestPoint("M1x9900") }, Minimal = 3.66, Nominal = 4.675, Maximal = 5.69, Unit = "V" },
            new TestItem { Descriptor = "FCT022036", Remark = "nSbcSSp3 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP13x2703"), new TestPoint("M1x9900") }, Minimal = 0, Nominal = 0, Maximal = 2, Unit = "V" },
            new TestItem { Descriptor = "FCT022037", Remark = "nSbcMcuRst_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP8x2703"), new TestPoint("M1x9900") }, Minimal = 0, Nominal = 0, Maximal = 1.5, Unit = "V" },
            new TestItem { Descriptor = "FCT022038", Remark = "nSbcSSp3 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP13x2703"), new TestPoint("M1x9900") }, Minimal = 0, Nominal = 0, Maximal = 0.5, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {

            testEnvironment.Reset();
            Thread.Sleep(1000);

            var tests = new List<TestItem> 
            { 
                GetTest("FCT022002"),
                GetTest("FCT022003"),
                GetTest("FCT022004"), 
                GetTest("FCT022005"),
                GetTest("FCT022006"), 
                GetTest("FCT022008"), 
                GetTest("FCT022010") 
            };
            
            Fct8322_SystemBasisChipHelpers.TestWakeOnCan(testEnvironment, parameters, tests);

            tests = new List<TestItem>
            {
                GetTest("FCT022012"),
                GetTest("FCT022013"),
                GetTest("FCT022014"),
                GetTest("FCT022015"),
                GetTest("FCT022016"),
                GetTest("FCT022018"),
                GetTest("FCT022020"),
            };

            Fct8322_SystemBasisChipHelpers.TestWakePerFlyback(testEnvironment, parameters, tests);

            tests = new List<TestItem>
            {
                GetTest("FCT022021"),
                GetTest("FCT022022"),
                GetTest("FCT022023"),
                GetTest("FCT022024"),
                GetTest("FCT022025"),
                GetTest("FCT022026"),
                GetTest("FCT022027"),
            };

            Fct8322_SystemBasisChipHelpers.TestUndervoltageSBC(testEnvironment, parameters, tests);

            tests = new List<TestItem>
            {
                GetTest("FCT022028"),
                GetTest("FCT022029"),
                GetTest("FCT022030"),
                GetTest("FCT022031"),
                GetTest("FCT022032"),
                GetTest("FCT022033"),
                GetTest("FCT022034"),
            };

            Fct8322_SystemBasisChipHelpers.TestOvervoltageSBC(testEnvironment, parameters, tests);

            tests = new List<TestItem>
            {
                GetTest("FCT022035"),
                GetTest("FCT022036"),
                GetTest("FCT022037"),
                GetTest("FCT022038"),              
            };

            Fct8322_SystemBasisChipHelpers.TestSBCSafepath(testEnvironment, parameters, tests);

        }
    }
}
