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

    internal class Fct8312_15VMonitoring : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8312_15VMonitoring(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "15V Monitoring";

        protected override string Id { get; set; } = "8.3.1.2";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT012001", Remark = "15V0Fly (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2202"), new TestPoint("M1x9900") }, Minimal = -0.05, Nominal = 0.0, Maximal = 0.5, Unit = "V" },
            new TestItem { Descriptor = "FCT012002", Remark = "UGdbBotm (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("IP1x6003GB"), new TestPoint("M1x9900") }, Minimal = 13.6, Nominal = 14.15, Maximal = 14.8, Unit = "V" },
            new TestItem { Descriptor = "FCT012003", Remark = "UGdbTop (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("IP1x6003GT"), new TestPoint("M1x9900") }, Minimal = 13.6, Nominal = 14.15, Maximal = 14.8, Unit = "V" },
            new TestItem { Descriptor = "FCT012004", Remark = "UGdbBotm_An (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x6003GB"), new TestPoint("M1x9900") }, Minimal = 2, Nominal = 2.2, Maximal = 2.4, Unit = "V" },
            new TestItem { Descriptor = "FCT012005", Remark = "UGdbTop_An (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x6003GT"), new TestPoint("M1x9900") }, Minimal = 2, Nominal = 2.2, Maximal = 2.4, Unit = "V" },
            new TestItem { Descriptor = "FCT012006", Remark = "14V6Top (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x2006T"), new TestPoint("M1x9900") }, Minimal = 14.1, Nominal = 14.6, Maximal = 15.1, Unit = "V" },
            new TestItem { Descriptor = "FCT012007", Remark = "14V6Botm (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP1x2006B"), new TestPoint("M1x9900") }, Minimal = 14.1, Nominal = 14.6, Maximal = 15.1, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment,  TestParameters parameters)
        {

            testEnvironment.Reset();
            Thread.Sleep(3000);



            var test = GetTest("FCT012001");

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(300);

            test = GetTest("FCT012001");
            TestLibrary.Voltage(test, measureTime: 0.05);

            test = GetTest("FCT012002");
            TestLibrary.Voltage(test);

            test = GetTest("FCT012003");         
            TestLibrary.Voltage(test);

            test = GetTest("FCT012004");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT012005");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT012006");
            TestLibrary.Voltage(test);

            test = GetTest("FCT012007");
            TestLibrary.Voltage(test);

            testEnvironment.Reset();

            Thread.Sleep(500);

        }

    }
}
