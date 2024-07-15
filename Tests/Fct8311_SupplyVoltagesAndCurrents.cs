namespace Program
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using PeakCanXcp;
    using Spea;
    using Spea.Instruments;
    using Spea.TestEnvironment;
    using Spea.TestFramework;
   
    using System.Linq;
    using Prorgam;

    internal partial class Fct8311_SupplyVoltagesAndCurrents : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {

        public Fct8311_SupplyVoltagesAndCurrents(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {

        }

        public override bool IsEnabled { get; set; } = true;

        protected override CdCollLogCongfig CdCollLogCongfig { get  ; set; } = new CdCollLogCongfig(); 

        protected TestEnv<UserFlagPurpose, PmxPurpose> _testEnvironment;
        protected TestParameters _parameters;

        protected override string Name { get; set; } = "Supply Voltages and Currents";

        protected override string Id { get; set; } = "8.3.1.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT011001", Remark = "KL30T (Current)", Minimal = 0.55, Nominal = 0.65, Maximal = 0.8, Unit = "A" },
            new TestItem { Descriptor = "FCT011002", Remark = "Terminal30Rpp (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP1X2002"), new TestPoint("M1X9900") }, Minimal = 12.8, Nominal = 13.0, Maximal = 13.2, Unit = "V" },
            new TestItem { Descriptor = "FCT011003.1", Remark = "Terminal30RppSepic (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("IP1x2006B"), new TestPoint("TP1x2006B") }, Minimal = 12.8, Nominal = 13.0, Maximal = 13.2, Unit = "V" },
            new TestItem { Descriptor = "FCT011003.2", Remark = "Terminal30RppSepic (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("IP1x2006T"), new TestPoint("TP1x2006T") }, Minimal = 12.8, Nominal = 13.0, Maximal = 13.2, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {

            

            _parameters = parameters;
            _testEnvironment = testEnvironment;

            var xcp = parameters.Xcp;

            var test = new TestItem();
            Thread.Sleep(500);

            _testEnvironment.Set(state => state
                //.HasTpsConnectedToAbus(Abus.ABUS1, 1537, 1540)
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );
            
                        

            Thread.Sleep(500);

            //Restbus.PowerOn = true;

            //Thread.Sleep(500);

            test = GetTest("FCT011001");
            TestLibrary.StimulusCurrent(test, testEnvironment.GetStimulus(StimulusId.BSTI1), timeOnMs: 20, 10);

            test = GetTest("FCT011002");
            TestLibrary.Voltage(test);

            test = GetTest("FCT011003.1");
            TestLibrary.Voltage(test);

            test = GetTest("FCT011003.2");
            TestLibrary.Voltage(test);
        }              
    }
}
