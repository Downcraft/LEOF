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

    internal class Fct8342_HvDcMeasurementAndOvDetection : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8342_HvDcMeasurementAndOvDetection(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "HVDC voltage measurement and OV Detection";

        protected override string Id { get; set; } = "8.3.4.2";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT042001", Remark = "Udc_P (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x6304x1"), new TestPoint("TP6X6304X2") }, Minimal = -0.1, Nominal = 0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT042002", Remark = "Udc_N (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x6304x1"), new TestPoint("TP6X6304X2") }, Minimal = -0.1, Nominal = 0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT042003", Remark = "Udc_P (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x6304x1"), new TestPoint("TP6X6304X2") }, Minimal = 1.7, Nominal = 1.75, Maximal = 1.8, Unit = "V" },
            new TestItem { Descriptor = "FCT042004", Remark = "Udc_N (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x6304x1"), new TestPoint("TP6X6304X2") }, Minimal = 0.7, Nominal = 0.74, Maximal = 0.78, Unit = "V" },
            new TestItem { Descriptor = "FCT042005", Remark = "UdcMeas (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP10x6304x1"), new TestPoint("TP6X6304X2") }, Minimal = 2.44, Nominal = 2.45, Maximal = 2.482, Unit = "V" },
            new TestItem { Descriptor = "FCT042006", Remark = "nOvvDetd_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP6x3604"), new TestPoint("M1X9900") }, Minimal = 4.1, Nominal = 4.2, Maximal = 4.3, Unit = "V" },
            new TestItem { Descriptor = "FCT042007", Remark = "UdcMeas (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP10x6304x1"), new TestPoint("TP6X6304X2") }, Minimal = 2.475, Nominal = 2.5, Maximal = 2.55, Unit = "V" },
            new TestItem { Descriptor = "FCT042008", Remark = "nOvvDetd_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP6x3604"), new TestPoint("M1X9900") }, Minimal = 0, Nominal = 0, Maximal = 0.25, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var test = new TestItem();
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(100);

            xcp.Connect();

            Thread.Sleep(100);

            test = GetTest("FCT042001");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT042002");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            testEnvironment.Modify(state => state
                .ConnectTps(Abus.ABUS1, new TestPoint("TP15x6304x1").Number)
                .ConnectTps(Abus.ABUS4, new TestPoint("TP6X6304X2").Number)
                .SetStimuli(new StimulusConfig(StimulusId.DRI1, 4.5, 0.01, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            );

            Thread.Sleep(100);

            test = GetTest("FCT042003");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT042004");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);


            testEnvironment.Modify(state => state
               .DisconnectTps(Abus.ABUS1, new TestPoint("TP15x6304x1").Number)
               .DisconnectTps(Abus.ABUS4, new TestPoint("TP6X6304X2").Number)
               .ResetStimuli(StimulusId.DRI1)
           );

            testEnvironment.Modify(state => state
              .ConnectTps(Abus.ABUS1, new TestPoint("TP10x6304x1").Number)
              .ConnectTps(Abus.ABUS4, new TestPoint("TP6X6304X2").Number)
              .SetStimuli(new StimulusConfig(StimulusId.DRI1, 2.45, 0.1, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
          );

            TestLibrary.Voltage(GetTest("FCT042005"), range: DvmVRange.R10V);

            test = GetTest("FCT042006");
            TestLibrary.Voltage(test, range: DvmVRange.R10V, measureTime: 0.05);


            var stimulus = testEnvironment.GetStimulus(StimulusId.DRI1);                    
           
            for (decimal voltage = 2.45M; voltage < 2.55M; voltage += 0.001M)
            {
                stimulus.SetStimulus((double)voltage, 0.1);

                test = GetTest("FCT042008");
                TestLibrary.Voltage(test, range: DvmVRange.R10V);



                if (test.Result == TestResult.PASS)
                {
                    TestLibrary.Voltage(GetTest("FCT042007"), range: DvmVRange.R10V);
                    break;
                }
            }

            testEnvironment.Modify(state => state
               .DisconnectTps(Abus.ABUS1, new TestPoint("TP10x6304x1").Number)
               .DisconnectTps(Abus.ABUS4, new TestPoint("TP6X6304X2").Number)
               .ResetStimuli(StimulusId.DRI1)
           );

        }

    }
}
