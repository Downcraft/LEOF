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

    internal class Fct83112_ResolverInterface : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83112_ResolverInterface(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }
        public override bool IsEnabled { get; set; } = true;
        

        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Resolver Interface";

        protected override string Id { get; set; } = "8.3.11.2";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {

            new TestItem { Descriptor = "FCT112003.1", Remark = "RslvPosnCosSig_P_An PP (Voltage)", TestPoints = { new TestPoint("FP2x7601C"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.67, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112003.2", Remark = "RslvPosnCosSig_P_An NP (Voltage)", TestPoints = { new TestPoint("FP2x7601C"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.38, Maximal = 1.65, Unit = "V" },
            new TestItem { Descriptor = "FCT112004.1", Remark = "RslvPosnCosSig_N_An PP (Voltage)", TestPoints = { new TestPoint("FP3X7601C"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.62, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112004.2", Remark = "RslvPosnCosSig_N_An NP (Voltage)", TestPoints = { new TestPoint("FP3X7601C"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.32, Maximal = 1.65, Unit = "V" },
            new TestItem { Descriptor = "FCT112005.1", Remark = "RslvPosnCosSigRdnt_P_An PP (Voltage)", TestPoints = { new TestPoint("FP1x7601C"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.62, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112005.2", Remark = "RslvPosnCosSigRdnt_P_An NP (Voltage)", TestPoints = { new TestPoint("FP1x7601C"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.38, Maximal = 1.65, Unit = "V" },
            new TestItem { Descriptor = "FCT112006.1", Remark = "RslvPosnCosSigRdnt_N_An PP (Voltage)", TestPoints = { new TestPoint("FP4x7601C"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.63, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112006.2", Remark = "RslvPosnCosSigRdnt_N_An NP (Voltage)", TestPoints = { new TestPoint("FP4x7601C"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.32, Maximal = 1.65, Unit = "V" },
            new TestItem { Descriptor = "FCT112007.1", Remark = "RslvPosnSinSig_P_An PP (Voltage)", TestPoints = { new TestPoint("FP2x7601S"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.67, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112007.2", Remark = "RslvPosnSinSig_P_An NP (Voltage)", TestPoints = { new TestPoint("FP2x7601S"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.38, Maximal = 1.65, Unit = "V" },
            new TestItem { Descriptor = "FCT112008.1", Remark = "RslvPosnSinSig_N_An PP (Voltage)", TestPoints = { new TestPoint("FP3x7601S"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.62, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112008.2", Remark = "RslvPosnSinSig_N_An NP (Voltage)", TestPoints = { new TestPoint("FP3x7601S"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.32, Maximal = 1.65, Unit = "V" },
            new TestItem { Descriptor = "FCT112009.1", Remark = "RslvPosnSinSigRdnt_P_An PP (Voltage)", TestPoints = { new TestPoint("FP1x7601S"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.67, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112009.2", Remark = "RslvPosnSinSigRdnt_P_An NP (Voltage)", TestPoints = { new TestPoint("FP1x7601S"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.38, Maximal = 1.65, Unit = "V" },
            new TestItem { Descriptor = "FCT112010.1", Remark = "RslvPosnSinSigRdnt_N_An PP (Voltage)", TestPoints = { new TestPoint("FP4x7601S"), new TestPoint("M1x9900") }, Minimal = 3.3, Nominal = 3.63, Maximal = 3.8, Unit = "V" },
            new TestItem { Descriptor = "FCT112010.2", Remark = "RslvPosnSinSigRdnt_N_An NP (Voltage)", TestPoints = { new TestPoint("FP4x7601S"), new TestPoint("M1x9900") }, Minimal = 1.2, Nominal = 1.32, Maximal = 1.65, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;
            var test = new TestItem();

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(
                    UserFlagPurpose.CAN,
                    UserFlagPurpose.CAN_Termination, 
                    UserFlagPurpose.Power_Mod_U, 
                    UserFlagPurpose.Power_Mod_V, 
                    UserFlagPurpose.Power_Mod_W,
                    UserFlagPurpose.Reslv)
                    .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);
            xcp.Connect();
            Thread.Sleep(100);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112003.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112003.2"), 9_765);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112004.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112004.2"), 9_765);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112005.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112005.2"), 9_765);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112006.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112006.2"), 9_765);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112007.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112007.2"), 9_765);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112008.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112008.2"), 9_765);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112009.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112009.2"), 9_765);

            TestLibrary.VoltagePositivePeak(GetTest("FCT112010.1"), 9_765);
            TestLibrary.VoltageNegativePeak(GetTest("FCT112010.2"), 9_765);

        }

    }
}
