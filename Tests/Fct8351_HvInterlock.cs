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

    internal class Fct8351_HvInterlock : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8351_HvInterlock(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }
        public override bool IsEnabled { get; set; } = true;
        

        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Hv Interlock";

        protected override string Id { get; set; } = "8.3.5.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT051001", Remark = "HvIlkMeas_An_P (Voltage)",TestPoints = new List<TestPoint>{new TestPoint("FP1x8902x1"), new TestPoint("M1x9900")} ,Minimal = 3.1, Nominal = 3.3, Maximal = 3.5, Unit = "V" },
            new TestItem { Descriptor = "FCT051002", Remark = "HvIlkMeas_An_N (Voltage)",TestPoints = new List<TestPoint>{new TestPoint("FP2x8902x1"), new TestPoint("M1x9900")}, Minimal = 0.9, Nominal = 1.1, Maximal = 1.3, Unit = "V" },
            new TestItem { Descriptor = "FCT051003", Remark = "IoEcu_rHvil.CurrAdcVolt (Xcp)", Minimal = 3.1, Nominal = 3.3, Maximal = 3.5, Unit = "V" },
            new TestItem { Descriptor = "FCT051004", Remark = "IoEcu_rHvil.VoltAdcVolt (Xcp)", Minimal = 0.9, Nominal = 1.1, Maximal = 1.3, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var test = new TestItem()   ;
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U,UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);
            xcp.Connect();
            Thread.Sleep(300);

            test = GetTest("FCT051001");
            TestLibrary.Voltage(test, measureTime: 0.05, range: DvmVRange.R10V);

            test = GetTest("FCT051002");
            TestLibrary.Voltage(test, measureTime: 0.05, range: DvmVRange.R10V);

            test = GetTest("FCT051003");

            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHvil.CurrAdcVolt"]);

            test = GetTest("FCT051004");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHvil.VoltAdcVolt"]);

        }

    }
}
