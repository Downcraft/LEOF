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

    internal class Fct8314_HvFlybackConverter : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8314_HvFlybackConverter(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "HV Flyback Converter";

        protected override string Id { get; set; } = "8.3.1.4";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT014001", Remark = "16V5Flyback (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2202"), new TestPoint("M1x9900") }, Minimal = 16, Nominal = 16.5, Maximal = 17, Unit = "V"},
            new TestItem { Descriptor = "FCT014002", Remark = "12V0FlyHv (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP25x2202"), new TestPoint("M2") }, Minimal = 13, Nominal = 14.4, Maximal = 18.0, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var test = GetTest("FCT014001");
                                   

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination,UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("M1").Number)
                .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M2").Number)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTV1, 40, 1.0, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            );

            Thread.Sleep(500);

            test = GetTest("FCT014001");
            TestLibrary.Voltage(test);

            test = GetTest("FCT014002");
            TestLibrary.Voltage(test);

            testEnvironment.Modify(state => state
                .ResetStimuli(StimulusId.BSTV1)
            );
        }

    }
}
