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

    internal class Fct83142_InternalSuppliesHvSide : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct83142_InternalSuppliesHvSide(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }

        public override bool IsEnabled { get; set; } = true;
        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Internal Supplies HV Side";

        protected override string Id { get; set; } = "8.3.14.2";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            // All Values set to Zero
           
            new TestItem { Descriptor = "FCT142020", Remark = "SwtStTopPhaU_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP5x4503TU"), new TestPoint("M1X9900") }, Minimal = -1, Nominal = 0, Maximal = 1, Unit = "V" },
            new TestItem { Descriptor = "FCT142021", Remark = "SwtStTopPhaV_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP5x4503TV"), new TestPoint("M1X9900") }, Minimal = -1, Nominal = 0, Maximal = 1, Unit = "V" },
            new TestItem { Descriptor = "FCT142022", Remark = "SwtStTopPhaW_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP5x4503TW"), new TestPoint("M1X9900") }, Minimal = -1, Nominal = 0, Maximal = 1, Unit = "V" },
            new TestItem { Descriptor = "FCT142023", Remark = "SwtStBotmPhaU_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP5x4503BU"), new TestPoint("M1X9900") }, Minimal = -1, Nominal = 0, Maximal = 1, Unit = "V" },
            new TestItem { Descriptor = "FCT142024", Remark = "SwtStBotmPhaV_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP5x4503BV"), new TestPoint("M1X9900") }, Minimal = -1, Nominal = 0, Maximal = 1, Unit = "V" },
            new TestItem { Descriptor = "FCT142025", Remark = "SwtStBotmPhaW_Dig (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP5x4503BW"), new TestPoint("M1X9900") }, Minimal = -1, Nominal = 0, Maximal = 1, Unit = "V" },

            // HS Set To 1 LS Set to 0
            new TestItem { Descriptor = "FCT142032", Remark = "GateTopPhaU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TU"), new TestPoint("TP3X2403TU") }, Minimal = -0.5, Nominal = -0.0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT142034", Remark = "GateTopPhaV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TV"), new TestPoint("TP3X2403TV") }, Minimal = -0.5, Nominal = -0.0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT142036", Remark = "GateTopPhaW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TW"), new TestPoint("TP3X2403TW") }, Minimal = -0.5, Nominal = -0.0, Maximal = 0.1, Unit = "V" },

            // HS Set To 0 LS Set to 1
            new TestItem { Descriptor = "FCT142044", Remark = "GateBotmPhaU (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BU"), new TestPoint("TP3X2403BU") }, Minimal = -0.5, Nominal = -0.0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT142046", Remark = "GateBotmPhaV (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BV"), new TestPoint("TP3X2403BV") }, Minimal = -0.5, Nominal = -0.0, Maximal = 0.1, Unit = "V" },
            new TestItem { Descriptor = "FCT142048", Remark = "GateBotmPhaW (Voltage)", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BW"), new TestPoint("TP3X2403BW") }, Minimal = -0.5, Nominal = -0.0, Maximal = 0.1, Unit = "V" },

            // PWM Signals set to 100%
            new TestItem { Descriptor = "FCT142051", Remark = "Vcc_4503TU (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403TU"), new TestPoint("TP19X4503TU")  }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT142052", Remark = "GateTopPhaU (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TU"), new TestPoint("TP19X4503TU") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },

            new TestItem { Descriptor = "FCT142053", Remark = "Vcc_4503TV (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403TV"), new TestPoint("TP19X4503TV") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT142054", Remark = "GateTopPhaV (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TV"), new TestPoint("TP19X4503TV") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },

            new TestItem { Descriptor = "FCT142055", Remark = "Vcc_4503TW (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403TW"), new TestPoint("TP19X4503TW") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT142056", Remark = "GateTopPhaW (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TW"), new TestPoint("TP19X4503TW") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },

            new TestItem { Descriptor = "FCT142057", Remark = "Vcc_4503BU (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403BU"), new TestPoint("TP19X4503BU") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT142058", Remark = "GateBotmPhaU (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BU"), new TestPoint("TP19X4503BU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },

            new TestItem { Descriptor = "FCT142059", Remark = "Vcc_4503BV (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403BV"), new TestPoint("TP19X4503BV") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT142060", Remark = "GateBotmPhaV (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BV"), new TestPoint("TP19X4503BV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },

            new TestItem { Descriptor = "FCT142061", Remark = "Vcc_4503BW (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP3x2403BW"), new TestPoint("TP19X4503BW") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            new TestItem { Descriptor = "FCT142062", Remark = "GateBotmPhaW (Voltage) 100%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BW"), new TestPoint("TP19X4503BW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },

            // PWM Signals set to 0%
            new TestItem { Descriptor = "FCT142064", Remark = "Vcc_4503TU (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403TU"), new TestPoint("TP19X4503TU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT142065", Remark = "GateTopPhaU (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TU"), new TestPoint("TP19X4503TU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },

            new TestItem { Descriptor = "FCT142066", Remark = "Vcc_4503TV (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403TV"), new TestPoint("TP19X4503TV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT142067", Remark = "GateTopPhaV (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TV"), new TestPoint("TP19X4503TV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },

            new TestItem { Descriptor = "FCT142068", Remark = "Vcc_4503TW (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403TW"), new TestPoint("TP19X4503TW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT142069", Remark = "GateTopPhaW (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503TW"), new TestPoint("TP19X4503TW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },

            new TestItem { Descriptor = "FCT142070", Remark = "Vcc_4503BU (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403BU"), new TestPoint("TP19X4503BU") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT142071", Remark = "GateBotmPhaU (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BU"), new TestPoint("TP19X4503BU") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },

            new TestItem { Descriptor = "FCT142072", Remark = "Vcc_4503BV (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403BV"), new TestPoint("TP19X4503BV") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT142073", Remark = "GateBotmPhaV (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BV"), new TestPoint("TP19X4503BV") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },

            new TestItem { Descriptor = "FCT142074", Remark = "Vcc_4503BW (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP12x2403BW"), new TestPoint("TP19X4503BW") }, Minimal = -3.55, Nominal = -3.5, Maximal = -3.4, Unit = "V" },
            new TestItem { Descriptor = "FCT142075", Remark = "GateBotmPhaW (Voltage) 0%", TestPoints = new List<TestPoint> { new TestPoint("TP14x4503BW"), new TestPoint("TP19X4503BW") }, Minimal = 17.3, Nominal = 18, Maximal = 18.7, Unit = "V" },
            
            
            new TestItem { Descriptor = "SafeStateCheck1", Remark = "SafeStateLevel",  Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },
            new TestItem { Descriptor = "SafeStateCheck2", Remark = "SafeStateLevel",  Minimal = 1, Nominal = 1, Maximal = 1, Unit = "Boolean" },

            
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

            Thread.Sleep(300);

         


            Fct83142_InternalSuppliesHvSideHelpers.AllPhasesSetToLow(xcp, a2l, GetTest);
           
            Fct83142_InternalSuppliesHvSideHelpers.HighSideSetToHigh(xcp, a2l, GetTest);

            Fct83142_InternalSuppliesHvSideHelpers.LowSideSetToHigh(xcp, a2l, GetTest);


            testEnvironment.Reset();

            Thread.Sleep(2000);

            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );
            Thread.Sleep(2000);


            Fct83142_InternalSuppliesHvSideHelpers.PwmSetTo100(xcp, a2l, GetTest);

            Thread.Sleep(2000);

            Fct83142_InternalSuppliesHvSideHelpers.PwmSetTo0(xcp, a2l, GetTest);

        }
    }
}
