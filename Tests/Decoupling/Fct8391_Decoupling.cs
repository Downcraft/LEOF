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

    internal class Fct8391_Decoupling: BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8391_Decoupling(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }
        public override bool IsEnabled { get; set; } = true;
        

        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Decoupling";

        protected override string Id { get; set; } = "8.3.9.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT091003", Remark = "5V0 (Voltage)", TestPoints = { new TestPoint("TP1x5003"), new TestPoint("M1x9900") }, Minimal = 4.8, Nominal = 5.0, Maximal = 5.2, Unit = "V" },
            new TestItem { Descriptor = "FCT091006", Remark = "Terminal30Drvr (Voltage)", TestPoints = { new TestPoint("TP2x5003"), new TestPoint("M1x9900") },Minimal = 12.8, Nominal = 13.0, Maximal = 13.2, Unit = "V" },
            new TestItem { Descriptor = "FCT091007", Remark = "Terminal30Brdg (Voltage)", TestPoints = { new TestPoint("TP4x5003"), new TestPoint("M1x9900") }, Minimal = 13.2, Nominal = 13.4, Maximal = 13.6, Unit = "V" },
            new TestItem { Descriptor = "FCT091008", Remark = "DCU_POS_SIG (Voltage)", TestPoints = { new TestPoint("FP2x8602"), new TestPoint("M1x9900") }, Minimal = 900, Nominal = 1000, Maximal = 1100, Unit = "Hz" },
            new TestItem { Descriptor = "FCT091009", Remark = "DigSnsrPwmSent_Dig (Voltage)", TestPoints = { new TestPoint("FP1x8602"), new TestPoint("M1x9900") }, Minimal = 23.0, Nominal = 25, Maximal = 26, Unit = "%" },
            new TestItem { Descriptor = "FCT091010", Remark = "DigSnsrPwmSent_Dig (Voltage)", TestPoints = { new TestPoint("FP1x8602"), new TestPoint("M1x9900") }, Minimal = 900, Nominal = 1000, Maximal = 1100, Unit = "µs" },
            new TestItem { Descriptor = "FCT091012", Remark = "SolnElecI_An (Voltage)", TestPoints = { new TestPoint("FP3x5003"), new TestPoint("M1x9900") }, Minimal = 2.4, Nominal = 2.64, Maximal = 2.8, Unit = "V" },
            new TestItem { Descriptor = "FCT091013", Remark = "IoActr_ActrCurr (Xcp)", Minimal = 0.5, Nominal = 0.66, Maximal = 0.8, Unit = "A" },
            new TestItem { Descriptor = "FCT091015", Remark = "SolnDiagPhaPosNegElecU_An (Voltage)", TestPoints = { new TestPoint("FP8x5003"), new TestPoint("M1x9900") }, Minimal = 1.1, Nominal = 1.2, Maximal = 1.3, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;
            var test = new TestItem();


            testEnvironment.Reset();
            Thread.Sleep(3000);

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(
                    UserFlagPurpose.CAN,
                    UserFlagPurpose.CAN_Termination, 
                    UserFlagPurpose.Power_Mod_U, 
                    UserFlagPurpose.Power_Mod_V, 
                    UserFlagPurpose.Power_Mod_W, 
                    UserFlagPurpose.Solenoid
                    )
                .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("FP2x8602").Number)
                .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1X9900").Number)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 3.0, useSense: true))
            );

            Thread.Sleep(200);
            xcp.Connect();
            Thread.Sleep(100);


            //xcp.Download(a2l.Characteristics["HwTest_cExtSensSupDisOvrrdEn"], new List<byte> { 0x01 });
            //xcp.Download(a2l.Characteristics["HwTest_cExtSensSupDisOvrrd"], new List<byte> { 0x01 });
            Thread.Sleep(25);

            test = GetTest("FCT091003");
            TestLibrary.Voltage(test);

            xcp.Download(a2l.Characteristics["HwTest_cT30RppSwdEnOvrrdEn"], new List<byte> { 0x01 });
            Thread.Sleep(25);

            xcp.Download(a2l.Characteristics["HwTest_cT30RppSwdEnOvrrd"], new List<byte> { 0x01 });
            Thread.Sleep(25);

            test = GetTest("FCT091006");
            TestLibrary.Voltage(test);

            test = GetTest("FCT091007");
            TestLibrary.Voltage(test);


            LeoF.WfgConnectAbus(Abus.ABUS1,  Abus.ABUS4);

            LeoF.WfgOutputSet(WfgOutputMode.SINGLE_ENDED, WfgAmpRange.R10V, 5, WfgOffsetRange.R1V, 0, WfgImpedance.Z_10_OHM);
            LeoF.WfgSequenceSelect("Pwm25", (1d / 1000d) / 32d, WfgOutputFormat.CONT_ON);
            LeoF.WfgEnable();


            Thread.Sleep(100);

            test = GetTest("FCT091008");
            TestLibrary.Frequency(test, test.Nominal, periodes: 20);


            test = GetTest("FCT091009");
            var param  = TestLibrary.PWM(test, test.Nominal, 0.5, 3.0, periodes: 10);
            test.Measured = param.DutyCycle;

            if(test.Result != TestResult.PASS)
            {                

                param = TestLibrary.PWM(test, test.Nominal, 0.5, 3.0, periodes: 10);
                test.Measured = param.DutyCycle;
            }

            test = GetTest("FCT091010");
            test.Measured = (1/param.Frequency) * 1_000_000d;

            xcp.Download(a2l.Characteristics["Aps_cTestModeEn"], new List<byte> { 0x01 });
            Thread.Sleep(25);

            xcp.Download(a2l.Characteristics["Aps_cCtrlModeTest"], new List<byte> { 0x02 });
            Thread.Sleep(25);

            xcp.Download(a2l.Characteristics["Aps_cSupVoltSeln"], new List<byte> { 0x02 });
            Thread.Sleep(25);

            xcp.Download(a2l.Characteristics["Aps_cDucyReqTest"], BitConverter.GetBytes(2.0f).ToList());
            Thread.Sleep(25);

            Thread.Sleep(500);


            test = GetTest("FCT091012");
            TestLibrary.Voltage(test, range: DvmVRange.R10V, measureTime: 0.05);

            test = GetTest("FCT091013");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoActr_ActrCurr"], numberOfMeasurements: 10);

            xcp.Download(a2l.Characteristics["Aps_cCtrlModeTest"], new List<byte> { 0x00 });
            Thread.Sleep(25);

            test = GetTest("FCT091015");
            TestLibrary.Voltage(test, 0.05);

        
            LeoF.WfgDisable();
            LeoF.WfgDisconnectAbus(Abus.ABUS1, Abus.ABUS4);
        }

    }
}
