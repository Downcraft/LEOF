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

    internal class Fct8331_SafeStateLogic : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8331_SafeStateLogic(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Safe State Logic";

        protected override string Id { get; set; } = "8.3.3.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT031001", Remark = "DrvrEna_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP2x3404"), new TestPoint("M1x9900") }, Minimal = 4.7, Nominal = 4.9, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT031002", Remark = "CanStbSsp3 (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP3x3404"), new TestPoint("M1x9900") }, Minimal = 0, Nominal = 0, Maximal = 0.3, Unit = "V" },
            new TestItem { Descriptor = "FCT031003", Remark = "nSafeSt (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP18x3404"), new TestPoint("M1x9900") }, Minimal = 4.0, Nominal = 4.5, Maximal = 5.0, Unit = "V" },
            new TestItem { Descriptor = "FCT031004", Remark = "IoEm_rHwSafeStateStsPinLv (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var xcp = parameters.Xcp;

            var test = GetTest("FCT031001");

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W, UserFlagPurpose.KL30C_KL30T)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);
            xcp.Connect();
            Thread.Sleep(100);

            test = GetTest("FCT031001");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT031002");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT031003");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            //test = GetTest("FCT031004");
            //TestLibrary.Xcp(test, TestParameters.Xcp, parameters.A2l.Measurements["IoEm_rHwSafeStateStsPinLvl"]);

        }

    }
}
