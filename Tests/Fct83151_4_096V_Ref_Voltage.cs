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

    internal class Fct83151_4_096V_Ref_Voltage : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83151_4_096V_Ref_Voltage(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }
        public override bool IsEnabled { get; set; } = true;


        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "4.096V Ref. Voltage";

        protected override string Id { get; set; } = "8.3.15.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT151002", Remark = "4V096Ref_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP1x2604"), new TestPoint("M1x9900") }, Minimal = 4.046, Nominal = 4.096, Maximal = 4.146, Unit = "V" },
            new TestItem { Descriptor = "FCT151004", Remark = "4V096Ref_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP1x2604"), new TestPoint("M1x9900") }, Minimal = 4.064, Nominal = 4.096, Maximal = 4.146, Unit = "V" },
            new TestItem { Descriptor = "FCT151006", Remark = "4V096Ref_An (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP1x2604"), new TestPoint("M1x9900") }, Minimal = 4.064, Nominal = 4.096, Maximal = 4.146, Unit = "V" },
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

            xcp.Download(a2l.Characteristics["HwTest_cDrvSupTopEnOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["HwTest_cDrvSupTopEnOvrrd"], new List<byte> { 0x01 });

            Thread.Sleep(100);

            test = GetTest("FCT151002");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            testEnvironment.Modify(state => state
                .SetStimuli(new StimulusConfig(StimulusId.BSTI1, 6.0, 2, useSense: true))
            );

            Thread.Sleep(100);

            xcp.Download(a2l.Characteristics["HwTest_cDrvSupTopEnOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["HwTest_cDrvSupTopEnOvrrd"], new List<byte> { 0x01 });

            Thread.Sleep(100);

            test = GetTest("FCT151004");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

            testEnvironment.Modify(state => state
                           .SetStimuli(new StimulusConfig(StimulusId.BSTI1, 18.0, 2, useSense: true))
                                      );


            Thread.Sleep(100);

            xcp.Download(a2l.Characteristics["HwTest_cDrvSupTopEnOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["HwTest_cDrvSupTopEnOvrrd"], new List<byte> { 0x01 });

            Thread.Sleep(100);

            test = GetTest("FCT151006");
            TestLibrary.Voltage(test, range: DvmVRange.R10V);

        }


    }
}
