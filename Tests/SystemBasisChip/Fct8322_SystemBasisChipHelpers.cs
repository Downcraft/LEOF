using PeakCanXcp;
using Spea;
using Spea.Instruments;
using Spea.TestEnvironment;
using Spea.TestFramework;
using System;
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
        public static bool TestWakeOnCan(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, Func<string, TestItem> GetTest)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;

            xcp.Connect();

            Thread.Sleep(300);

            DvmVRange range;

             var test = GetTest("FCT022002");
             range = GetVoltageRangeRegardingTestLimits(test);
             TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022003");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022004");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022005");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022006");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestVoltageWithRetest(test, range, measureTime: 0.1, retest: 10);

            if(test.Result != TestResult.PASS)
            {
                return false;
            }


            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x00 });

            test = GetTest("FCT022008");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.1);


            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x01 });

            test = GetTest("FCT022010");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.1);


            //try
            //{
            //    xcp.Disconnect();
            //}
            //catch
            //{

            //}

            return true;
        }

        public static void TestWakePerFlyback(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, Func<string, TestItem> GetTest)
        {
            var xcp = parameters.Xcp;
            var a2l = TestParameters.A2l;
            testEnvironment.Reset();

            Thread.Sleep(3000);


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

            var test = GetTest("FCT022012");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);


            test = GetTest("FCT022013");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022014");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022015");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022016");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);




            xcp.Connect();
            Thread.Sleep(300);


            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x00 });

            test = GetTest("FCT022018");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrdEn"], new List<byte> { 0x01 });
            xcp.Download(a2l.Characteristics["IoEcu_cSbcMux.CmdMuxValueOvrrd"], new List<byte> { 0x03 });

            test = GetTest("FCT022020");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);


            //testEnvironment.Reset();

        }

        public static void TestUndervoltageSBC(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, Func<string, TestItem> GetTest)
        {

            //Thread.Sleep(1000);


            testEnvironment.Set(state => state
           .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
           .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 6.0, 2, useSense: true))
           );


            Thread.Sleep(300);
            DvmVRange range;

            var test = GetTest("FCT022021");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022022");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022023");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022024");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022025");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022026");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022027");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestVoltageWithRetest(test, range, measureTime: 0.01, retest: 5);

        }

        public static void TestOvervoltageSBC(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters,  Func<string, TestItem> GetTest)
        {
            //Thread.Sleep(300);

            testEnvironment.Modify(state => state
           .SetStimuli(new StimulusConfig(StimulusId.BSTI1, 18.0, 2, useSense: true))
           );


            Thread.Sleep(300);
            DvmVRange range;

            var test = GetTest("FCT022028");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022029");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022030");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022031");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022032");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022033");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022034");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestVoltageWithRetest(test, range, measureTime: 0.01, retest: 5);

            //testEnvironment.Reset();
            //Thread.Sleep(1000);
        }

        public static void TestSBCSafepath(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, TestParameters parameters, Func<string, TestItem> GetTest)
        {
            testEnvironment.Set(state => state
           .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
           .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("FP5x2703").Number)
           .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1x9900").Number)
           //.HasStimuliOn(new StimulusConfig(StimulusId.BSTV1, 5.0, 0.5, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
           .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense:true))
           );

            Thread.Sleep(100);


            testEnvironment.Modify(state => state
            .SetStimuli(new StimulusConfig(StimulusId.BSTV1, 5.0, 0.5, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            );

            Thread.Sleep(500);

            DvmVRange range;
            
            var test = GetTest("FCT022035");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022036");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestVoltageWithRetest(test, range, measureTime: 0.01, retest: 5);

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

            test = GetTest("FCT022037");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestLibrary.Voltage(test, range: range, measureTime: 0.05);

            test = GetTest("FCT022038");
            range = GetVoltageRangeRegardingTestLimits(test);
            TestVoltageWithRetest(test, range, measureTime: 0.01, retest: 5);

            testEnvironment.Reset();

            Thread.Sleep(1000);

        }

        private static void TestVoltageWithRetest(TestItem test, DvmVRange dvmVRange, double measureTime = 0.001, int retest = 10)
        {
            for (int i = 0; i < retest; i++)
            {
                TestLibrary.Voltage(test, range: dvmVRange, measureTime: measureTime);
                Thread.Sleep(50);


                if (test.Result == TestResult.PASS)
                {
                    break;
                }
            }

        }
    }
}