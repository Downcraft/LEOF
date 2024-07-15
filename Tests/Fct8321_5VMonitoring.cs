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

    internal class Fct8321_5VMonitoring : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8321_5VMonitoring(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "5V Monitoring";

        protected override string Id { get; set; } = "8.3.2.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT021001", Remark = "5V0 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x2703"), new TestPoint("M1x9900") }, Minimal = 4.87, Nominal = 5, Maximal = 5.13, Unit = "V" },
            new TestItem { Descriptor = "FCT021002", Remark = "5V0Com (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP3x2703"), new TestPoint("M1x9900") }, Minimal = 4.8, Nominal = 5, Maximal = 5.2, Unit = "V" },
            new TestItem { Descriptor = "FCT021003", Remark = "5V0An (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP4x2703"), new TestPoint("M1x9900") }, Minimal = 4.95, Nominal = 5, Maximal = 5.05, Unit = "V" },
            new TestItem { Descriptor = "FCT021004", Remark = "5V0Aux (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP5x2703"), new TestPoint("M1x9900") }, Minimal = 4.85, Nominal = 5, Maximal = 5.25, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            var test = GetTest("FCT021001");

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);

            xcp.Connect();

            Thread.Sleep(100);

            test = GetTest("FCT021001");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT021002");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT021003");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT021004");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            testEnvironment.Modify(state => state
                .ResetStimuli(StimulusId.BSTI1)
            );

            try
            {
                xcp.Disconnect();
            }
            catch
            {

            }

        }

    }
}
