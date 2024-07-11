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

    internal class Fct8371_ActiveDischarge : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8371_ActiveDischarge(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }
        public override bool IsEnabled { get; set; } = true;


        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Active Discharge";

        protected override string Id { get; set; } = "8.3.7.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT071001", Remark = "PwrHv (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP1x5502x1"), new TestPoint("M1x9900") }, Minimal = 74, Nominal = 75, Maximal = 76, Unit = "V" },
            new TestItem { Descriptor = "FCT071002", Remark = "ActvDchaCtrlMon_Dig (Xcp)", Minimal = 92.5, Nominal = 94.5, Maximal = 95.5, Unit = "%" },
            new TestItem { Descriptor = "FCT071003", Remark = "ActvDchaCurrEst (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x5502x1"), new TestPoint("M1x9900") }, Minimal = 0.01, Nominal = 0.02, Maximal = 0.05, Unit = "V" },
            new TestItem { Descriptor = "FCT071007", Remark = "ActvDchaCtrlMon_Dig (Xcp)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "%" },
            new TestItem { Descriptor = "FCT071008", Remark = "ActvDchaCurrEst (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x5502x1"), new TestPoint("M1x9900") }, Minimal = 0.28, Nominal = 0.3, Maximal = 0.32, Unit = "V" },
            new TestItem { Descriptor = "FCT071009", Remark = "ActvDcha_Iset (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP18x5502x2"), new TestPoint("M1x9900") }, Minimal = 1.73, Nominal = 1.85, Maximal = 1.97, Unit = "V" },
            new TestItem { Descriptor = "FCT071011", Remark = "ActvDchaCurrEst (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x5502x1"), new TestPoint("M1x9900") }, Minimal = 0, Nominal = 0, Maximal = 0.05, Unit = "V" },

        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            var test = new TestItem();

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.HV, UserFlagPurpose.CAP_Cb, UserFlagPurpose.HV_GND_GND, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTV1, 75, 0.5, useSense: true/*, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4*/))
            );

            Thread.Sleep(500);
            xcp.Connect();
            Thread.Sleep(3000);

            test = GetTest("FCT071001");
            TestLibrary.Voltage(test);

            xcp.Connect();
            Thread.Sleep(50);

            xcp.Download(a2l.Characteristics["HwTest_cActvDchrgDisOvrrdEn"], new List<byte> { 0x00 });

            Thread.Sleep(50);


            test = GetTest("FCT071002");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_rActvDchaCtrlMonDucy"]);
            Thread.Sleep(50);


            test = GetTest("FCT071003");
            TestLibrary.Voltage(test, measureTime: 0.05);

            xcp.Download(a2l.Characteristics["HwTest_cActvDchrgDisOvrrdEn"], new List<byte> { 0x01 });
            Thread.Sleep(50);

            xcp.Download(a2l.Characteristics["HwTest_cActvDchrgTstOvrrdEn"], new List<byte> { 0x01 });
            Thread.Sleep(50);

            xcp.Download(a2l.Characteristics["HwTest_cActvDchrgTstOvrrd"], new List<byte> { 0x01 });
            Thread.Sleep(50);

            Thread.Sleep(4000);


            test = GetTest("FCT071007");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_rActvDchaCtrlMonDucy"], numberOfMeasurements: 10);

            
            test = GetTest("FCT071008");

            for (int i = 0; i < 100; i++ )
            {               
                if (TestLibrary.Voltage(test, measureTime: 0.005, range: DvmVRange.R10V).Result == TestResult.PASS)
                {
                    break;
                }
            }


            test = GetTest("FCT071009");
            TestLibrary.Voltage(test);
            
            xcp.Download(a2l.Characteristics["HwTest_cActvDchrgDisOvrrd"], new List<byte> { 0x01 });

            test = GetTest("FCT071011");
            TestLibrary.Voltage(test);

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

        }
    }
}
