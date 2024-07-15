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

    internal class Fct8361_CrashDetection : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8361_CrashDetection(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }
        public override bool IsEnabled { get; set; } = true;


        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Crash Detection";

        protected override string Id { get; set; } = "8.3.6.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT061001", Remark = "KL30C (Voltage)",TestPoints = new List<TestPoint>{new TestPoint(1537), new TestPoint("M1X9900") } ,Minimal = 13.4, Nominal = 13.5, Maximal = 13.6, Unit = "V" },
            new TestItem { Descriptor = "FCT061002", Remark = "CrashDetn_An (Voltage)",TestPoints = new List<TestPoint>{new TestPoint("FP2x6203x1"), new TestPoint("M1X9900") } , Minimal = 1.3, Nominal = 1.5, Maximal = 1.7, Unit = "V" },
            new TestItem { Descriptor = "FCT061003", Remark = "IoEcu_rCrash.CrashAdcVolt (Xcp)", Minimal = 1.3, Nominal = 1.5, Maximal = 1.7, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var test = new TestItem();
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W, UserFlagPurpose.KL30C_KL30T)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);
            xcp.Connect();
            Thread.Sleep(100);

            //testEnvironment.Modify(state => state
            //    .ResetUserFlags(UserFlagPurpose.KL30C_KL30T)
            //);

            Thread.Sleep(1000);

            test = GetTest("FCT061001");
            TestLibrary.Voltage(test);

            test = GetTest("FCT061002");
            TestLibrary.Voltage(test);


            test = GetTest("FCT061003");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rCrash.CrashAdcVolt"]);

            testEnvironment.Reset();
            Thread.Sleep(1000);
        }
    }
}
