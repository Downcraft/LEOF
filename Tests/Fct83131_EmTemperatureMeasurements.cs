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

    internal class Fct83131_EmTemperatureMeasurements : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83131_EmTemperatureMeasurements(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }
        public override bool IsEnabled { get; set; } = true;


        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "EM Temperature Measurement";

        protected override string Id { get; set; } = "8.3.13.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT131007", Remark = "EmTHiPrim_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x7002T1"), new TestPoint("M1x9900") }, Minimal = 4.97, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT131008", Remark = "IoMotTemp_rWndgTemp1AdcVolt (Xcp)", Minimal = 4.89, Nominal = 5, Maximal = 5, Unit = "V" },
            new TestItem { Descriptor = "FCT131009", Remark = "EmTLoPrim_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x7002T1"), new TestPoint("M1x9900") }, Minimal = -0.02, Nominal = 0.0, Maximal = 0.06, Unit = "V" },
            new TestItem { Descriptor = "FCT131010", Remark = "IoMotTemp_rWndgTemp1DiagAdcVolt (Xcp)", Minimal = 0, Nominal = 0, Maximal = 0.02, Unit = "V" },
            new TestItem { Descriptor = "FCT131011", Remark = "nIPhaVOvc_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x7002T2"), new TestPoint("M1x9900") }, Minimal = 4.97, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT131012", Remark = "IoMotTemp_rWndgTemp2AdcVolt (Xcp)", Minimal = 4.89, Nominal = 5, Maximal = 5, Unit = "V" },
            new TestItem { Descriptor = "FCT131013", Remark = "EmTLoSecdy_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x7002T2"), new TestPoint("M1x9900") }, Minimal = -0.02, Nominal = 0.0, Maximal = 0.06, Unit = "V" },
            new TestItem { Descriptor = "FCT131014", Remark = "IoMotTemp_rWndgTemp2DiagAdcVolt (Xcp)", Minimal = 0, Nominal = 0, Maximal = 0.02, Unit = "V" },
             
            new TestItem { Descriptor = "FCT131019", Remark = "EmTHiPrim_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x7002T1"), new TestPoint("M1x9900") }, Minimal = 4.82, Nominal = 4.87, Maximal = 4.92, Unit = "V" },
            new TestItem { Descriptor = "FCT131020", Remark = "IoMotTemp_rWndgTemp1AdcVolt (Xcp)", Minimal = 4.82, Nominal = 4.87, Maximal = 4.92, Unit = "V" },
            new TestItem { Descriptor = "FCT131021", Remark = "IoMotTemp_wWndgTemp1DiagEnPinLvl (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT131022", Remark = "EmTLoPrim_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x7002T1"), new TestPoint("M1x9900") }, Minimal = 3.75, Nominal = 3.8, Maximal = 3.85, Unit = "V" },
            new TestItem { Descriptor = "FCT131023", Remark = "IoMotTemp_rWndgTemp1DiagAdcVolt (Xcp)", Minimal = 3.75, Nominal = 3.8, Maximal = 3.85, Unit = "V" },
            new TestItem { Descriptor = "FCT131024", Remark = "IoMotTemp_wWndgTemp1Rng2EnPinLvl (Xcp)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT131025", Remark = "EmTHiSecdy_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x7002T2"), new TestPoint("M1x9900") }, Minimal = 4.82, Nominal = 4.87, Maximal = 4.92, Unit = "V" },
            new TestItem { Descriptor = "FCT131026", Remark = "IoMotTemp_rWndgTemp2AdcVolt (Xcp)", Minimal = 4.82, Nominal = 4.87, Maximal = 4.92, Unit = "V" },
            new TestItem { Descriptor = "FCT131027", Remark = "IoMotTemp_wWndgTemp2DiagEnPinLvl (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT131028", Remark = "EmTLoSecdy_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x7002T2"), new TestPoint("M1x9900") }, Minimal = 3.75, Nominal = 3.8, Maximal = 3.85, Unit = "V" },
            new TestItem { Descriptor = "FCT131029", Remark = "IoMotTemp_rWndgTemp2DiagAdcVolt (Xcp)", Minimal = 3.75, Nominal = 3.8, Maximal = 3.85, Unit = "V" },
            new TestItem { Descriptor = "FCT131030", Remark = "IoMotTemp_wWndgTemp2Rng2EnPinLvl (Xcp)", Minimal = 0, Nominal = 0, Maximal = 0, Unit = "Boolean" },
            
            new TestItem { Descriptor = "FCT131035", Remark = "EmTHiPrim_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x7002T1"), new TestPoint("M1x9900") }, Minimal = 4.44, Nominal = 4.52, Maximal = 4.6, Unit = "V" },
            new TestItem { Descriptor = "FCT131036", Remark = "IoMotTemp_rWndgTemp1AdcVolt (Xcp)", Minimal = 4.44, Nominal = 4.52, Maximal = 4.6, Unit = "V" },
            new TestItem { Descriptor = "FCT131037", Remark = "IoMotTemp_wWndgTemp1DiagEnPinLvl (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT131038", Remark = "EmTLoPrim_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x7002T1"), new TestPoint("M1x9900") }, Minimal = 0.42, Nominal = 0.47, Maximal = 0.5, Unit = "V" },
            new TestItem { Descriptor = "FCT131039", Remark = "IoMotTemp_rWndgTemp1DiagAdcVolt (Xcp)", Minimal = 0.42, Nominal = 0.47, Maximal = 0.5, Unit = "V" },
            new TestItem { Descriptor = "FCT131040", Remark = "IoMotTemp_wWndgTemp1Rng2EnPinLvl (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT131041", Remark = "EmTHiSecdy_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x7002T2"), new TestPoint("M1x9900") }, Minimal = 4.44, Nominal = 4.52, Maximal = 4.6, Unit = "V" },
            new TestItem { Descriptor = "FCT131042", Remark = "IoMotTemp_rWndgTemp2AdcVolt (Xcp)", Minimal = 4.44, Nominal = 4.52, Maximal = 4.6, Unit = "V" },
            new TestItem { Descriptor = "FCT131043", Remark = "IoMotTemp_wWndgTemp2DiagEnPinLvl (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "FCT131044", Remark = "EmTLoSecdy_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP2x7002T2"), new TestPoint("M1x9900") }, Minimal = 0.42, Nominal = 0.47, Maximal = 0.5, Unit = "V" },
            new TestItem { Descriptor = "FCT131045", Remark = "IoMotTemp_rWndgTemp2DiagAdcVolt (Xcp)", Minimal = 0.42, Nominal = 0.47, Maximal = 0.5, Unit = "V" },
            new TestItem { Descriptor = "FCT131046", Remark = "IoMotTemp_wWndgTemp2Rng2EnPinLvl (Xcp)", Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },

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

            xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State5HoldTime"],BitConverter.GetBytes(1.0f).ToList());
            xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State6HoldTime"],BitConverter.GetBytes(0.01f).ToList());

            Thread.Sleep(100);


            test = GetTest("FCT131007");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131008");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp1AdcVolt"]);

            test = GetTest("FCT131009");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131010");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp1DiagAdcVolt"]);

            test = GetTest("FCT131011");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131012");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp2AdcVolt"]);

            test = GetTest("FCT131013");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131014");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp2DiagAdcVolt"]);

            testEnvironment.Modify(state => state
                .SetUserFlags(UserFlagPurpose.EM_Temp_Prim, UserFlagPurpose.EM_Temp_Sec)
            );

            Thread.Sleep(100);

            xcp.Download(a2l.Characteristics["IoMotTemp_cWndgTemp1Ovrrd.RngOvrrdMode"], new List<byte> { 0x02 });
            xcp.Download(a2l.Characteristics["IoMotTemp_cWndgTemp2Ovrrd.RngOvrrdMode"], new List<byte> { 0x02 });

            Thread.Sleep(100);

            test = GetTest("FCT131019");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131020");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp1AdcVolt"]);
            
            test = GetTest("FCT131021");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp1DiagEnPinLvl"]);

            test = GetTest("FCT131022");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131023");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp1DiagAdcVolt"]);
            
            test = GetTest("FCT131024");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp1Rng2EnPinLvl"]);

            test = GetTest("FCT131025");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131026");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp2AdcVolt"]);

             test = GetTest("FCT131027");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp2DiagEnPinLvl"]);

            test = GetTest("FCT131028");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131029");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp2DiagAdcVolt"]);

            test = GetTest("FCT131030");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp2Rng2EnPinLvl"]);

            //testEnvironment.Modify(state => state
            //    .ResetUserFlags(UserFlagPurpose.EM_Temp_Prim, UserFlagPurpose.EM_Temp_Sec)
            //    .ConnectTps(Abus.ABUS1, 1295, 1293, 1297, 1299)
            //    .ConnectTps(Abus.ABUS4, new TestPoint("M1x9900").Number)
            //    .SetStimuli(new StimulusConfig(StimulusId.DRI1, 5.0, 0.1, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            //);

            //Thread.Sleep(100);

            //xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State5HoldTime"], BitConverter.GetBytes(0.01f).ToList());
            //xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State6HoldTime"], BitConverter.GetBytes(1.0f).ToList());

            //Thread.Sleep(100);



            xcp.Download(a2l.Characteristics["IoMotTemp_cWndgTemp1Ovrrd.RngOvrrdMode"], BitConverter.GetBytes(0).ToList());
            xcp.Download(a2l.Characteristics["IoMotTemp_cWndgTemp2Ovrrd.RngOvrrdMode"], BitConverter.GetBytes(0).ToList());

            Thread.Sleep(100);

            xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State5HoldTime"], BitConverter.GetBytes(0.01f).ToList());
            xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State6HoldTime"], BitConverter.GetBytes(1.0f).ToList());

            Thread.Sleep(100);



            test = GetTest("FCT131035");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131036");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp1AdcVolt"]);

            test = GetTest("FCT131037");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp1DiagEnPinLvl"]);

            test = GetTest("FCT131038");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131039");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp1DiagAdcVolt"]);

            test = GetTest("FCT131040");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp1Rng2EnPinLvl"]);

            test = GetTest("FCT131041");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131042");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp2AdcVolt"]);

            test = GetTest("FCT131043");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp2DiagEnPinLvl"]);

            test = GetTest("FCT131044");
            TestLibrary.Voltage(test);

            test = GetTest("FCT131045");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_rWndgTemp2DiagAdcVolt"]);

            test = GetTest("FCT131046");
            TestLibrary.Xcp(test, xcp, a2l.Measurements["IoMotTemp_wWndgTemp2Rng2EnPinLvl"]);

            xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State5HoldTime"], BitConverter.GetBytes(0.04f).ToList());
            xcp.Download(a2l.Characteristics["IoMotTemp_cSmPar.State6HoldTime"], BitConverter.GetBytes(0.03f).ToList());



        }

       
    }
}
