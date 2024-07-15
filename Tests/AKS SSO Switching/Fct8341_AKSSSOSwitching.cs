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
    using static Fct8341_AKSSSOSwitchingHelpers;

    internal class Fct8341_AksSsoSwitching : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8341_AksSsoSwitching(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "AKS/SSO Switching";

        protected override string Id { get; set; } = "8.3.4.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT041001", Remark = "AsoAsc (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP12x3604"), new TestPoint("M1x9900") }, Minimal = -0.1, Nominal = 0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT041002", Remark = "IoEcu_rAsoAsc.AsoAscPinLvl (XCP)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Boolean", IsNoError  = true },
            new TestItem { Descriptor = "FCT041003", Remark = "PwrHv (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP10X6304X1"), new TestPoint("M2") }, Minimal = 45, Nominal = 48, Maximal = 49.3, Unit = "V" },
            new TestItem { Descriptor = "FCT041004", Remark = "AsoAsc (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP12x3604"), new TestPoint("M1x9900") }, Minimal = 4.4, Nominal = 4.5, Maximal = 4.7, Unit = "V" },
            new TestItem { Descriptor = "FCT041005", Remark = "IoEcu_rAsoAsc.AsoAscPinLvl (XCP)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" , IsNoError  = true},
            new TestItem { Descriptor = "FCT041006", Remark = "PwrHv (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP10X6304X1"), new TestPoint("M2") }, Minimal = 49.3, Nominal = 50, Maximal = 53, Unit = "V" },
            new TestItem { Descriptor = "FCT041007", Remark = "AsoAsc (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("FP12x3604"), new TestPoint("M1x9900") }, Minimal = -0.01, Nominal = 0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT041008", Remark = "IoEcu_rAsoAsc.AsoAscPinLvl (XCP)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Boolean", IsNoError  = true },

        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            double factor = 357.7817531305903;

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

            var test = new TestItem();


            //testEnvironment.Modify(state => state
            //.SetUserFlags(UserFlagPurpose.HV, UserFlagPurpose.HV_GND_GND)
            //.SetStimuli(new StimulusConfig(StimulusId.BSTV1, 55.0, 0.5, StimulusConnectionPoint.EXTERNAL, StimulusConnectionPoint.EXTERNAL))
            //);


            testEnvironment.Modify(state => state
                .ConnectTps(Abus.ABUS1, new TestPoint("TP10X6304X1").Number )
                .ConnectTps(Abus.ABUS4, new TestPoint("M2").Number )
                .SetStimuli(new StimulusConfig(StimulusId.DRI1, (55.0/factor), 0.1, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            );          

            HvStimulus = testEnvironment.GetStimulus(StimulusId.DRI1);

            Thread.Sleep(100);

            //LeoF.TpConnectAbus(new List<int> { new TestPoint("M1").Number }, Abus.ABUS2);
            //LeoF.TpConnectAbus(new List<int> { new TestPoint("M2").Number }, Abus.ABUS3);

            //Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out double volt, 0.05, DvmVRange.R1V);

            //LeoF.TpDisconnectAbus(new List<int> { new TestPoint("M1").Number }, Abus.ABUS2);
            //LeoF.TpDisconnectAbus(new List<int> { new TestPoint("M2").Number }, Abus.ABUS3);

            test = GetTest("FCT041001");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            test = GetTest("FCT041002");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rAsoAsc.AsoAscPinLvl"]);
            //TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_AsoAscState"]);

           

            SetTests(LevelTest: GetTest("FCT041004"), triggerTest: GetTest("FCT041003"));
            RampDown(startVoltage: (55.0/factor), endVoltage: (45.0/factor));

            Thread.Sleep(100);

            test = GetTest("FCT041005");

            for (int retry = 0; retry < 1; retry++)
            {
                TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rAsoAsc.AsoAscPinLvl"]/*, numberOfMeasurements: 10*/);
                Thread.Sleep(10);

                if (test.Result == TestResult.PASS)
                {
                    break;
                }
            }

            Thread.Sleep(500);

            xcp.Connect();
            SetTests(LevelTest: GetTest("FCT041007"), triggerTest: GetTest("FCT041006"));
            RampUp(startVoltage: (48/factor), endVoltage: (55 / factor) );
            test = GetTest("FCT041008");
            //TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_AsoAscState"]);
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoEcu_rAsoAsc.AsoAscPinLvl"]);

            testEnvironment.Modify(state => state
            .DisconnectTps(Abus.ABUS1, new TestPoint("TP10X6304X1").Number)
            .DisconnectTps(Abus.ABUS4, new TestPoint("M2").Number)
                .ResetStimuli(StimulusId.BSTV1)
            );


        }

    }
}
