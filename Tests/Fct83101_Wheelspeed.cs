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

    internal class Fct83101_WheelSpeed: BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83101_WheelSpeed(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }
        public override bool IsEnabled { get; set; } = true;
        

        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Axle Speed Interface";

        protected override string Id { get; set; } = "8.3.10.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT101001", Remark = "SPD_SIG (Voltage)", TestPoints = { new TestPoint("FP2x8502"), new TestPoint("M1x9900") }, Minimal = 13.5, Nominal = 13.7, Maximal = 13.9, Unit = "V" },
            new TestItem { Descriptor = "FCT101002", Remark = "SpdSnsrSigA_Dig (Voltage)", TestPoints = { new TestPoint("FP1x8502"), new TestPoint("M1x9900") },Minimal = 0.25, Nominal = 0.5, Maximal = 0.7, Unit = "V" },
            new TestItem { Descriptor = "FCT101003", Remark = "HwTest_VADC_Result_G3_Ch5 (Xcp)", Minimal = 1.8, Nominal = 1.85, Maximal = 1.9, Unit = "V" },
            new TestItem { Descriptor = "FCT101004", Remark = "SPD_SIG (Voltage)", TestPoints = { new TestPoint("FP2x8502"), new TestPoint("M1x9900") }, Minimal = 6.5, Nominal = 6.7, Maximal = 6.9, Unit = "V" },
            new TestItem { Descriptor = "FCT101005", Remark = "SpdSnsrSigA_Dig (Voltage)", TestPoints = { new TestPoint("FP1x8502"), new TestPoint("M1x9900") },Minimal = 3, Nominal = 3.2, Maximal = 3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT101006", Remark = "HwTest_VADC_Result_G3_Ch5 (Xcp)", Minimal = 0.27, Nominal = 1.00, Maximal = 1.50, Unit = "V" },
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
                    UserFlagPurpose.Power_Mod_W )
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);
            xcp.Connect();
            Thread.Sleep(100);

            test = GetTest("FCT101001");
            TestLibrary.Voltage(test);

            test = GetTest("FCT101002");
            TestLibrary.Voltage(test);

            test = GetTest("FCT101003");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_VADC_Result_G3_Ch5"]);
            
            testEnvironment.Modify(state => state
                .FpsOn(FpsId.FPS1)
                .SetUserFlags(UserFlagPurpose.Power_Wheel_Speed, UserFlagPurpose.Wheel_Speed_Signal)
            );

            LeoF.WfgConnectInterf();
            LeoF.WfgOutputSet(WfgOutputMode.SINGLE_ENDED, WfgAmpRange.R10V, 5, WfgOffsetRange.R1V, 0, WfgImpedance.Z_10_OHM);
            LeoF.WfgWaveformSelect(WfgWaveformType.SQUARE, 2000.0, WfgOutputFormat.CONT_ON);
            LeoF.WfgEnable();

            Thread.Sleep(100);


            test = GetTest("FCT101004");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT101005");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT101006");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_VADC_Result_G3_Ch5"]);

            LeoF.WfgDisable();
            LeoF.WfgDisconnectInterf();
        }

    }
}
