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
    using static Fct8381_CanTransceiverHelpers;

    internal class Fct8381_CanTransceiver : BaseTest<TestParameters, UserFlagPurpose, PmxPurpose>
    {
        public Fct8381_CanTransceiver(int site, SiteManager siteManager, Variant variant) : base(site, siteManager, variant)
        {
        }
        public override bool IsEnabled { get; set; } = true;


        protected override CdCollLogCongfig CdCollLogCongfig { get; set; } = new CdCollLogCongfig();

        protected override string Name { get; set; } = "Can Transceiver";

        protected override string Id { get; set; } = "8.3.8.1";

        protected override List<TestItem> TestItems { get; set; } = new List<TestItem>
        {
            new TestItem { Descriptor = "FCT081001", Remark = "WakeUpSbc_Dig (Voltage)",TestPoints = new List<TestPoint> {new TestPoint("FP6x3501A"), new TestPoint("M1x9900") }, Minimal = 12.0, Nominal = 12.2, Maximal = 12.4, Unit = "V" },
            new TestItem { Descriptor = "FCT081002", Remark = "nCanEmErr_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP7x3501A"), new TestPoint("M1x9900") }, Minimal = 4.75, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT081003", Remark = "nCanStb_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("TP6x3501A"), new TestPoint("M1x9900") }, Minimal = 4.75, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT081004", Remark = "CanEmTrcvEna_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP10x3501A"), new TestPoint("M1x9900") }, Minimal = 4.75, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT081005", Remark = "CanEmTrcv_P (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP3x3501A"), new TestPoint("M1x9900") }, Minimal = 1.7, Nominal = 1.8, Maximal = 2.9, Unit = "V" },
            new TestItem { Descriptor = "FCT081006", Remark = "CanEmTrcv_N (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP5x3501A"), new TestPoint("M1x9900") }, Minimal = 1.7, Nominal = 1.8, Maximal = 2.55, Unit = "V" },
            new TestItem { Descriptor = "FCT081007", Remark = "CanEmSftyTx (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP7x3502EM"), new TestPoint("M1x9900") }, Minimal = 4.75, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT081008", Remark = "CanEmSftyRx (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP3x3502EM"), new TestPoint("M1x9900") }, Minimal = 4.5, Nominal = 4.85, Maximal = 5.0, Unit = "V" },
            new TestItem { Descriptor = "FCT081009", Remark = "CanEmTrcv_P (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x3502EM"), new TestPoint("M1x9900") }, Minimal = 1.7, Nominal = 1.85, Maximal = 2.9, Unit = "V" },
            new TestItem { Descriptor = "FCT081010", Remark = "CanEmTrcv_N (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP5x3502EM"), new TestPoint("M1x9900") }, Minimal = 1.6, Nominal = 1.75, Maximal = 2.9, Unit = "V" },
            new TestItem { Descriptor = "FCT081011", Remark = "CanPrivtTx_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP7x3502AP"), new TestPoint("M1x9900") }, Minimal = 4.75, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT081012", Remark = "CanPrivtRx_Dig (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP3x3502AP"), new TestPoint("M1x9900") }, Minimal = 4.75, Nominal = 5.0, Maximal = 5.1, Unit = "V" },
            new TestItem { Descriptor = "FCT081013", Remark = "CanPrivtTrcv_P (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP1x3502AP"), new TestPoint("M1x9900") }, Minimal = 2.4, Nominal = 2.5, Maximal = 2.9, Unit = "V" },
            new TestItem { Descriptor = "FCT081014", Remark = "CanPrivtTrcv_N (Voltage)", TestPoints = new List<TestPoint> {new TestPoint("FP5x3502AP"), new TestPoint("M1x9900") }, Minimal = 2.4, Nominal = 2.5, Maximal = 2.9, Unit = "V" },

        };

        protected override void RunTest(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;


            TestStateAndNetworkTransceiver(testEnvironment, TestItems, GetTest);

            TestSafetyTransceiver(testEnvironment, TestItems, GetTest);

            TestPrivateTransceiver(testEnvironment, TestItems, GetTest);

        }
        
    }
}
