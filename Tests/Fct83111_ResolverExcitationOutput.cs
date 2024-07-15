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

    internal class Fct83111_ResolverExcitationOutput : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83111_ResolverExcitationOutput(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }
        public override bool IsEnabled { get; set; } = true;
        

        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Resolver Excitation Output";

        protected override string Id { get; set; } = "8.3.11.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT111001", Remark = "RslvExcFrq_Dig (Voltage)", TestPoints = { new TestPoint("FP1x9101"), new TestPoint("M1x9900") }, Minimal = 9745, Nominal = 9765, Maximal = 9885, Unit = "Hz" },
            new TestItem { Descriptor = "FCT111002", Remark = "HwTest_rRslvExcFreq (Xcp)", Minimal = 9745, Nominal = 9765, Maximal = 9885, Unit = "Hz" },
            new TestItem { Descriptor = "FCT111003.1", Remark = "RSLV_EXC_P VRMS (Voltage)", TestPoints = { new TestPoint("FP1x7501x1"), new TestPoint("M1x9900") }, Minimal = 7.4, Nominal = 7.6, Maximal = 7.8, Unit = "V" },
            new TestItem { Descriptor = "FCT111003.2", Remark = "RSLV_EXC_N VRMS (Voltage)", TestPoints = { new TestPoint("TP2x8003"), new TestPoint("M1x9900") }, Minimal = 7.4, Nominal = 7.6, Maximal = 7.8, Unit = "V" },
            new TestItem { Descriptor = "FCT111004.1", Remark = "RSLV_EXC_P Vmax_pp (Voltage)", TestPoints = { new TestPoint("FP1x7501x1"), new TestPoint("M1x9900") }, Minimal = 9.47, Nominal = 9.75, Maximal = 10.04, Unit = "V" },
            new TestItem { Descriptor = "FCT111004.2", Remark = "RSLV_EXC_N Vmax_pp (Voltage)", TestPoints = { new TestPoint("TP2x8003"), new TestPoint("M1x9900") }, Minimal = 9.47, Nominal = 9.75, Maximal = 10.04, Unit = "V" }

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

            test = GetTest("FCT111001");
            TestLibrary.Frequency(test, test.Nominal);


            test = GetTest("FCT111002");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_rRslvExcFreq"], 10);

            test = GetTest("FCT111003.1");
            TestLibrary.VoltageRms(test, GetTest("FCT111001").Measured);

            test = GetTest("FCT111003.2");
            TestLibrary.VoltageRms(test, GetTest("FCT111001").Measured);

            test = GetTest("FCT111004.1");
            TestLibrary.VoltagePeakToPeak(test, GetTest("FCT111001").Measured);

            test = GetTest("FCT111004.2");
            TestLibrary.VoltagePeakToPeak(test, GetTest("FCT111001").Measured);



        }

    }
}
