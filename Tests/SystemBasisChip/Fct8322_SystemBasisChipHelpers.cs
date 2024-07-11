using Spea;
using Spea.Instruments;
using Spea.TestEnvironment;
using Spea.TestFramework;
using System.Collections.Generic;
using System.Threading;

namespace Program
{
    internal static class Fct8322_SystemBasisChipHelpers
    {
        private static DvmVRange GetVoltageRangeRegardingTestLimits(TestItem test)
        {
            var range = DvmVRange.R100V;

            if (test.Maximal < 1.0)
            {
                range = DvmVRange.R1V;
            }
            else if (test.Maximal < 10.0)
            {
                range = DvmVRange.R10V;
            }

            return range;
        }
        public static void TestWakeOnCan(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, List<TestItem> tests)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            testEnvironment.Set(state => state
            .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
            .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
            );


            //TestParameters.Xcp.Connect();

            //Thread.Sleep(300);
            DvmVRange range;

            foreach (var test in tests.GetRange(0, 5))
            {
                range = GetVoltageRangeRegardingTestLimits(test);
                TestLibrary.Voltage(test, range: range, measureTime: 0.05);
            }

            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x00 });

            range = GetVoltageRangeRegardingTestLimits(tests[5]);
            TestLibrary.Voltage(tests[5], range: range);

            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x01 });

            range = GetVoltageRangeRegardingTestLimits(tests[6]);
            TestLibrary.Voltage(tests[6], range: range);


            try
            {
                xcp.Disconnect();
            }
            catch
            {

            }
        }

        public static void TestWakePerFlyback(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, List<TestItem> tests)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;
            testEnvironment.Reset();

            Thread.Sleep(1000);


            testEnvironment.Set(state => state
           .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
           .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("TP33x2703").Number)
           .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1x9900").Number)
           .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
           .HasStimuliOn(new StimulusConfig(StimulusId.BSTV1, 15.9, 0.5, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
           );


            Thread.Sleep(300);
            DvmVRange range;

            foreach (var test in tests.GetRange(0, 5))
            {
                range = GetVoltageRangeRegardingTestLimits(test);
                TestLibrary.Voltage(test, range: range);
            }


            xcp.Connect();
            Thread.Sleep(300);


            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x00 });

            range = GetVoltageRangeRegardingTestLimits(tests[5]);
            TestLibrary.Voltage(tests[5], range: range);

            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x03 });

            range = GetVoltageRangeRegardingTestLimits(tests[6]);
            TestLibrary.Voltage(tests[6], range: range);


            try
            {
                xcp.Disconnect();
            }
            catch
            {

            }

            testEnvironment.Reset();

        }

        public static void TestUndervoltageSBC(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, List<TestItem> tests)
        {

            //Thread.Sleep(1000);


            testEnvironment.Set(state => state
           .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
           .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 6.0, 2, useSense: true))
           );


            Thread.Sleep(300);
            DvmVRange range;

            foreach (var test in tests.GetRange(0, 7))
            {
                range = GetVoltageRangeRegardingTestLimits(test);
                TestLibrary.Voltage(test, range: range);
            }            
        }

        public static void TestOvervoltageSBC(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, List<TestItem> tests)
        {
            //Thread.Sleep(300);

            testEnvironment.Modify(state => state
           .SetStimuli(new StimulusConfig(StimulusId.BSTI1, 18.0, 2, useSense: true))
           );


            Thread.Sleep(300);
            DvmVRange range;

            foreach (var test in tests.GetRange(0, 7))
            {
                range = GetVoltageRangeRegardingTestLimits(test);
                TestLibrary.Voltage(test, range: range);
            }

            testEnvironment.Reset();
            Thread.Sleep(1000);
        }

        public static void TestSBCSafepath(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, List<TestItem> tests)
        {
            testEnvironment.Set(state => state
           .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
           .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("FP5x2703").Number)
           .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1x9900").Number)
           //.HasStimuliOn(new StimulusConfig(StimulusId.BSTV1, 5.0, 0.5, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
           .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense:true))
           );

            Thread.Sleep(1000);


            testEnvironment.Modify(state => state
            .SetStimuli(new StimulusConfig(StimulusId.BSTV1, 5.0, 0.5, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            );

            Thread.Sleep(500);

            DvmVRange range;
            
            foreach(var test in tests.GetRange(0, 2))
            {
                range = GetVoltageRangeRegardingTestLimits(test);
                TestLibrary.Voltage(test, range: range);
            }

            testEnvironment.Reset();

            Thread.Sleep(100);        


            testEnvironment.Set(state => state
            .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
            .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("FP1x2703").Number)
            .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1x9900").Number)
            .HasStimuliOn(new StimulusConfig(StimulusId.BSTV1, 5.5, 0.5, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 1.0, StimulusConnectionPoint.EXTERNAL, StimulusConnectionPoint.EXTERNAL))

            );

            foreach (var test in tests.GetRange(2, 2))
            {
                range = GetVoltageRangeRegardingTestLimits(test);
                TestLibrary.Voltage(test, range: range);
            }

            testEnvironment.Reset();
            
        }
    }
}