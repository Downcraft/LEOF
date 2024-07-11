namespace Program
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using PeakCanXcp;
    using Spea;
    using Spea.Instruments;
    using Spea.TestEnvironment;
    using Spea.TestFramework;

    internal class Fct83141_GateDriverMagneto : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83141_GateDriverMagneto(int site, SiteManager siteManager) : base(site, siteManager)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Gate Driver Magneto";

        protected override string Id { get; set; } = "8.3.14.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT141001", Remark = "Vpos_4503BU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403BU"), new TestPoint("TP19X4503BU") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT141002", Remark = "Vneg_4503BU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403BU"), new TestPoint("TP19X4503BU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141003", Remark = "FBVDD_2403BU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2403BU"), new TestPoint("TP19X4503BU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141004", Remark = "Vpos_4503BV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403BV"), new TestPoint("TP19X4503BV") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT141005", Remark = "Vneg_4503BV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403BV"), new TestPoint("TP19X4503BV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141006", Remark = "FBVDD_2403BV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2403BV"), new TestPoint("TP19X4503BV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141007", Remark = "Vpos_4503BW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403BW"), new TestPoint("TP19X4503BW") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT141008", Remark = "Vneg_4503BW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403BW"), new TestPoint("TP19X4503BW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141009", Remark = "FBVDD_2403BW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2403BW"), new TestPoint("TP19X4503BW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141010", Remark = "Vpos_4503TU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403TU"), new TestPoint("TP19X4503TU") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT141011", Remark = "Vneg_4503TU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403TU"), new TestPoint("TP19X4503TU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141012", Remark = "FBVDD_2403TU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2403TU"), new TestPoint("TP19X4503TU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141013", Remark = "Vpos_4503TV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403TV"), new TestPoint("TP19X4503TV") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT141014", Remark = "Vneg_4503TV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403TV"), new TestPoint("TP19X4503TV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141015", Remark = "FBVDD_2403TV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2403TV"), new TestPoint("TP19X4503TV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141016", Remark = "Vpos_4503TW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403TW"), new TestPoint("TP19X4503TW") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT141017", Remark = "Vneg_4503TW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403TW"), new TestPoint("TP19X4503TW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT141018", Remark = "FBVDD_2403TW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP8x2403TW"), new TestPoint("TP19X4503TW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment,  TestParameters parameters)
        {


            var test = new TestItem();
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );

            Thread.Sleep(50);
            xcp.Connect();
            Thread.Sleep(50);


            TestLibrary.Voltage(GetTest("FCT141004"));
            TestLibrary.Voltage(GetTest("FCT141005"));
            //TestLibrary.Voltage(GetTest("FCT141006"));

            xcp.Connect();
            Thread.Sleep(1000);

         

            TestLibrary.Voltage(GetTest("FCT141001"));
            TestLibrary.Voltage(GetTest("FCT141002"));
            //TestLibrary.Voltage(GetTest("FCT141003"));

            xcp.Connect();
            Thread.Sleep(1000);

            TestLibrary.Voltage(GetTest("FCT141007"));
            TestLibrary.Voltage(GetTest("FCT141008"));
            //TestLibrary.Voltage(GetTest("FCT141009"));

            xcp.Connect();
            Thread.Sleep(300);

            TestLibrary.Voltage(GetTest("FCT141010"));
            TestLibrary.Voltage(GetTest("FCT141011"));
            //TestLibrary.Voltage(GetTest("FCT141012"));

            xcp.Connect();
            Thread.Sleep(300);

            TestLibrary.Voltage(GetTest("FCT141013"));
            TestLibrary.Voltage(GetTest("FCT141014"));
            //TestLibrary.Voltage(GetTest("FCT141015"));

            xcp.Connect();
            Thread.Sleep(300);

            TestLibrary.Voltage(GetTest("FCT141016"));
            TestLibrary.Voltage(GetTest("FCT141017"));
            //TestLibrary.Voltage(GetTest("FCT141018"));
        }

    }
}
