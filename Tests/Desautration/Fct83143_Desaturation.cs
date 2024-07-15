namespace Program
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using PeakCanXcp;
    using Spea;
    using Spea.Instruments;
    using Spea.TestEnvironment;
    using Spea.TestFramework;

    internal class Fct83143_Desaturation : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83143_Desaturation(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Desaturation";

        protected override string Id { get; set; } = "8.3.14.3";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT143003", Remark = "HwTest_rPhaseFeedback_SH4_PinLvl (XCP)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT143004", Remark = "GateSource TU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TU"), new TestPoint("TP19x4503TU") }, Minimal = -0.01, Nominal = 0, Maximal = 0.2, Unit = "V" },

            new TestItem { Descriptor = "FCT143007", Remark = "HwTest_rPhaseFeedback_SH4_PinLvl (XCP)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT143008", Remark = "GateSource TU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TU"), new TestPoint("TP19x4503TU") }, Minimal = 17.8, Nominal = 18, Maximal = 18.2, Unit = "V" },

            new TestItem { Descriptor = "FCT143015", Remark = "HwTest_rPhaseFeedback_SH5_PinLvl (XCP)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT143016", Remark = "GateSource TV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TV"), new TestPoint("TP19x4503TV") }, Minimal = -0.01, Nominal = 0, Maximal = 0.2, Unit = "V" },

            new TestItem { Descriptor = "FCT143019", Remark = "HwTest_rPhaseFeedback_SH5_PinLvl (XCP)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT143020", Remark = "GateSource TV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TV"), new TestPoint("TP19x4503TV") }, Minimal = 17.8, Nominal = 18, Maximal = 18.2, Unit = "V" },

        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {


            var test = new TestItem();
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            //testEnvironment.Set(state => state
            //    .HasFpsOn(FpsId.FPS4)
            //    .HasActiveUserFlags(UserFlagPurpose.CAN,
            //                        UserFlagPurpose.CAN_Termination,
            //                        UserFlagPurpose.Power_Mod_U,
            //                        UserFlagPurpose.Power_Mod_V,
            //                        UserFlagPurpose.Power_Mod_W,
            //                        UserFlagPurpose.Phase_U_Top_ISO)
            //    .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            //);

            //Fct83143_DesaturationHelpers.PhaseUTop(xcp, a2l, GetTest);

            //testEnvironment.Reset();

            //Thread.Sleep(1000);

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN,
                                    UserFlagPurpose.CAN_Termination,
                                    UserFlagPurpose.Power_Mod_U,
                                    UserFlagPurpose.Power_Mod_V,
                                    UserFlagPurpose.Power_Mod_W,
                                    UserFlagPurpose.Phase_V_Top_ISO)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
                );

            Fct83143_DesaturationHelpers.PhaseVTop(xcp, a2l, GetTest);

            testEnvironment.Reset();

            Thread.Sleep(1000);

        }
    }
}
