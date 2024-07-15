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

    internal class Fct8313_Terminal30_Monitoring : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8313_Terminal30_Monitoring(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Terminal30 Monitoring";

        protected override string Id { get; set; } = "8.3.1.3";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT013001", Remark = "Terminal30Fild (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1X9507"), new TestPoint("M1X9900") }, Minimal = 13.4, Nominal = 13.5, Maximal = 13.6, Unit = "V" },
            new TestItem { Descriptor = "FCT013002", Remark = "Terminal30Fild_An (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP2X6001LV"), new TestPoint("M1X9900") }, Minimal = 1.8, Nominal = 2.0, Maximal = 2.2, Unit = "V" },
            new TestItem { Descriptor = "FCT013003", Remark = "Terminal30Brdg (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP6X1104"), new TestPoint("M1X9900") }, Minimal = 13.3, Nominal = 13.5, Maximal = 13.6, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {

            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;
            var test = GetTest("FCT013001");

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 3, useSense: true))
            );

            Thread.Sleep(50);

            xcp.Connect();

            test = GetTest("FCT013001");
            TestLibrary.Voltage(test);

            test = GetTest("FCT013002");
            TestLibrary.Voltage(test);
                       
            xcp.Connect();

            xcp.Download(a2l.Characteristics["HwTest_cT30RppSwdEnOvrrdEn"], new List<byte> { 0x01 });
            Thread.Sleep(25);

            xcp.Download(a2l.Characteristics["HwTest_cT30RppSwdEnOvrrd"], new List<byte> { 0x01 });
            Thread.Sleep(25);

            Thread.Sleep(100);

            test = GetTest("FCT013003");         
            TestLibrary.Voltage(test);

            
        }

    }
}
