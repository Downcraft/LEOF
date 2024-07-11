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

    internal class Fct83161_InverterSyncOut : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83161_InverterSyncOut(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }
        public override bool IsEnabled { get; set; } = true;


        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Inverter Sync Out";

        protected override string Id { get; set; } = "8.3.16.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT161001", Remark = "SyncOutp (Voltage)", TestPoints = new List<TestPoint> {new TestPoint(1543), new TestPoint("M1x9900") }, Minimal = 13, Nominal = 13.5, Maximal = 14, Unit = "V" },
            new TestItem { Descriptor = "FCT161003", Remark = "SyncOutp (Voltage)", TestPoints = new List<TestPoint> {new TestPoint(1543), new TestPoint("M1x9900") }, Minimal = 0.3, Nominal = 0.5, Maximal = 0.7, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {           

            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            var test = new TestItem();

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);
            xcp.Connect();
            Thread.Sleep(100);

            test = GetTest("FCT161001");
            TestLibrary.Voltage(test, range: DvmVRange.R100V);

            //xcp.Download(a2l.Characteristics["ToBeDefined"], new List<byte> { 0x01 });

            //Thread.Sleep(100);

            //test = GetTest("FCT161003");
            //TestLibrary.Voltage(test, range: DvmVRange.R10V);

        }


    }
}
