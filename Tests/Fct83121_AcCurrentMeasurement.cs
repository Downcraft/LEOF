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

    internal class Fct83121_AcCurrentMeasurement : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83121_AcCurrentMeasurement(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }
        public override bool IsEnabled { get; set; } = true;


        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "AC Current Measurement";

        protected override string Id { get; set; } = "8.3.12.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT121001", Remark = "5V0An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP4x2703"), new TestPoint("M1x9900") }, Minimal = 4.9, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT121002", Remark = "IPhaU (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP1x8006x1U"), new TestPoint("M1x9900") }, Minimal = 0.01, Nominal = 0.5, Maximal = 0.01, Unit = "V" },
            new TestItem { Descriptor = "FCT121003", Remark = "IPhaV (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP1x8006x1V"), new TestPoint("M1x9900") }, Minimal = 0.01, Nominal = 0.5, Maximal = 0.01, Unit = "V" },
            new TestItem { Descriptor = "FCT121004", Remark = "IPhaW (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP1x8006x1W"), new TestPoint("M1x9900") }, Minimal = 0.01, Nominal = 0.5, Maximal = 0.01, Unit = "V" },
            new TestItem { Descriptor = "FCT121005", Remark = "nIPhaUOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x8201x1"), new TestPoint("M1x9900") }, Minimal = 4.5, Nominal = 4.7, Maximal = 5.0, Unit = "V" },
            new TestItem { Descriptor = "FCT121006", Remark = "nIPhaVOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP3x8201x1"), new TestPoint("M1x9900") }, Minimal = 4, Nominal = 4.2, Maximal = 5.0, Unit = "V" },
            new TestItem { Descriptor = "FCT121007", Remark = "nIPhaWOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP4x8201x1"), new TestPoint("M1x9900") }, Minimal = 4, Nominal = 4.2, Maximal = 5.0, Unit = "V" },
            new TestItem { Descriptor = "FCT121008", Remark = "nIPhaWOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP7x8201x1"), new TestPoint("M1x9900") }, Minimal = 4, Nominal = 4.2, Maximal = 5.0, Unit = "V" },
            new TestItem { Descriptor = "FCT121009", Remark = "IoEcu_rHwDiagPinLvl.InvPhase2CurrHigh (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Level" },
            new TestItem { Descriptor = "FCT121010", Remark = "IoEcu_rHwDiagPinLvl.InvPhase3CurrHigh (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Level" },
            new TestItem { Descriptor = "FCT121011", Remark = "IoEcu_rHwDiagPinLvl.InvPhase1CurrHigh (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Level" },
            new TestItem { Descriptor = "FCT121012", Remark = "IoEcu_rHwDiagPinLvl.PhaseCurrHigh (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Level" },
            new TestItem { Descriptor = "FCT121016", Remark = "nIPhaUOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.025, Maximal = 0.0, Unit = "V" },
            new TestItem { Descriptor = "FCT121017", Remark = "nIPhaVOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP3x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.025, Maximal = 0.0, Unit = "V" },
            new TestItem { Descriptor = "FCT121018", Remark = "nIPhaWOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP4x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.025, Maximal = 0.0, Unit = "V" },
            new TestItem { Descriptor = "FCT121019", Remark = "nIOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP7x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.05, Maximal = 0.9, Unit = "V" },
            new TestItem { Descriptor = "FCT121020", Remark = "IoEcu_rHwDiagPinLvl.InvPhase1CurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
            new TestItem { Descriptor = "FCT121021", Remark = "IoEcu_rHwDiagPinLvl.InvPhase2CurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
            new TestItem { Descriptor = "FCT121022", Remark = "IoEcu_rHwDiagPinLvl.InvPhase3CurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
            new TestItem { Descriptor = "FCT121023", Remark = "IoEcu_rHwDiagPinLvl.PhaseCurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
            new TestItem { Descriptor = "FCT121027", Remark = "nIPhaUOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.0, Maximal = 0.115, Unit = "V" },
            new TestItem { Descriptor = "FCT121028", Remark = "nIPhaVOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP3x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.0, Maximal = 0.115, Unit = "V" },
            new TestItem { Descriptor = "FCT121029", Remark = "nIPhaWOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP4x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.0, Maximal = 0.115, Unit = "V" },
            new TestItem { Descriptor = "FCT121030", Remark = "nIOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP7x8201x1"), new TestPoint("M1x9900") }, Minimal = 0.0, Nominal = 0.0, Maximal = 0.738, Unit = "V" },
            new TestItem { Descriptor = "FCT121031", Remark = "IoEcu_rHwDiagPinLvl.InvPhase1CurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
            new TestItem { Descriptor = "FCT121032", Remark = "IoEcu_rHwDiagPinLvl.InvPhase2CurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
            new TestItem { Descriptor = "FCT121033", Remark = "IoEcu_rHwDiagPinLvl.InvPhase3CurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
            new TestItem { Descriptor = "FCT121034", Remark = "IoEcu_rHwDiagPinLvl.PhaseCurrHigh (Voltage)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Level" },
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


            test = GetTest("FCT121001");
            TestLibrary.Voltage(test);

            test = GetTest("FCT121002");
            test.Nominal = test.Nominal* GetTest("FCT121001").Measured;
            test.Minimal = test.Nominal - test.Minimal;
            test.Maximal = test.Nominal + test.Maximal;
            TestLibrary.Voltage(test);

            test = GetTest("FCT121003");
            test.Nominal = test.Nominal * GetTest("FCT121001").Measured;
            test.Minimal = test.Nominal - test.Minimal;
            test.Maximal = test.Nominal + test.Maximal;
            TestLibrary.Voltage(test);

            test = GetTest("FCT121004");
            test.Nominal = test.Nominal * GetTest("FCT121001").Measured;
            test.Minimal = test.Nominal - test.Minimal;
            test.Maximal = test.Nominal + test.Maximal;
            TestLibrary.Voltage(test);

            test = GetTest("FCT121005");
            TestLibrary.Voltage(test);

            test = GetTest("FCT121006");
            TestLibrary.Voltage(test);

            test = GetTest("FCT121007");
            TestLibrary.Voltage(test);

            test = GetTest("FCT121008");
            TestLibrary.Voltage(test);

            test = GetTest("FCT121009");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase1CurrHigh"]);

            test = GetTest("FCT121010");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase2CurrHigh"]);

            test = GetTest("FCT121011");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase3CurrHigh"]);

            test = GetTest("FCT121012");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.PhaseCurrHigh"]);


            //TODO: Implement the following tests if the firmware gets fixed

            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagHighOvrrd"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagHighOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagLowOvrrd"], new List<byte> { 0x00 });
            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagLowOvrrdEn"], new List<byte> { 0x00 });

            Thread.Sleep(500);

            test = GetTest("FCT121016");
            test.Nominal = test.Nominal * GetTest("FCT121001").Measured;
            test.Maximal = test.Nominal + test.Maximal;
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121017");
            test.Nominal = test.Nominal * GetTest("FCT121001").Measured;
            test.Maximal = test.Nominal + test.Maximal;
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121018");
            test.Nominal = test.Nominal * GetTest("FCT121001").Measured;
            test.Maximal = test.Nominal + test.Maximal;
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121019");
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121020");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase1CurrHigh"]);

            test = GetTest("FCT121021");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase2CurrHigh"]);

            test = GetTest("FCT121022");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase3CurrHigh"]);

            test = GetTest("FCT121023");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.PhaseCurrHigh"]);

            testEnvironment.Reset();

            Thread.Sleep(2000);

            testEnvironment.Set(state => state
               .HasFpsOn(FpsId.FPS4)
               .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
               .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
           );

            Thread.Sleep(100);
            xcp.Connect();
            Thread.Sleep(100);


            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagHighOvrrd"], new List<byte> { 0x00 });
            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagHighOvrrdEn"], new List<byte> { 0x00 });
            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagLowOvrrd"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["HwTest_cPhaseOvcDiagLowOvrrdEn"], new List<byte> { 0x01 });

            test = GetTest("FCT121027");
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121028");
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121029");
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121030");
            TestLibrary.Voltage(test, range: DvmVRange.R1V);

            test = GetTest("FCT121031");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase1CurrHigh"]);

            test = GetTest("FCT121032");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase2CurrHigh"]);

            test = GetTest("FCT121033");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.InvPhase3CurrHigh"]);

            test = GetTest("FCT121034");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rHwDiagPinLvl.PhaseCurrHigh"]);

        }
    }
}
