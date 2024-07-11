using Spea.TestEnvironment;
using Spea.TestFramework;
using System;
using System.Collections.Generic;
using Spea;
using Spea.Instruments;
using System.Threading;

namespace Program
{
    public static class Fct8381_CanTransceiverHelpers
    {
        public static  void TestPrivateTransceiver(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, List<TestItem> testItems, Func<string, TestItem> GetTest)
        {
            TestItem test;
            testEnvironment.Set(state => state
                .HasFpsOn(FpsId.FPS4)
                .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.HV, UserFlagPurpose.CAP_Cb, UserFlagPurpose.HV_GND_GND, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("TP2x3501A").Number/*, new TestPoint("TP6X3501A").Number*/)
                .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1x9900").Number)
                .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
                .HasStimuliOn(new StimulusConfig(StimulusId.DRI1, 5.0, 0.1, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
            );

            Thread.Sleep(100);  

            test = GetTest("FCT081011");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081012");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081013");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081014");
            TestLibrary.Voltage(test);
        }

        public static void TestSafetyTransceiver(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, List<TestItem> testItems, Func<string, TestItem> GetTest)
        {
            TestItem test;
            testEnvironment.Set(state => state
                            .HasFpsOn(FpsId.FPS4)
                            .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.HV, UserFlagPurpose.CAP_Cb, UserFlagPurpose.HV_GND_GND, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                            .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("TP2x3501A").Number/*, new TestPoint("TP2X3502EM").Number*/)
                            .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1x9900").Number)
                            .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
                            .HasStimuliOn(new StimulusConfig(StimulusId.DRI1, 5.0, 0.1, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
                        );

            Thread.Sleep(100);

            test = GetTest("FCT081007");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081008");
            TestLibrary.Voltage(test, measureTime: 0.1);

            test = GetTest("FCT081009");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081010");
            TestLibrary.Voltage(test);
        }

        public static void TestStateAndNetworkTransceiver(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, List<TestItem> testItems, Func<string, TestItem> GetTest)
        {
            TestItem test;
            testEnvironment.Set(state => state
                            .HasFpsOn(FpsId.FPS4)
                            .HasActiveUserFlags(UserFlagPurpose.CAN, UserFlagPurpose.CAN_Termination, UserFlagPurpose.HV, UserFlagPurpose.CAP_Cb, UserFlagPurpose.HV_GND_GND, UserFlagPurpose.Power_Mod_U, UserFlagPurpose.Power_Mod_V, UserFlagPurpose.Power_Mod_W)
                            .HasTpsConnectedToAbus(Abus.ABUS1, new TestPoint("TP2x3501A").Number/*, new TestPoint("TP2x3502AP").Number*/)
                            .HasTpsConnectedToAbus(Abus.ABUS4, new TestPoint("M1x9900").Number)
                            .HasStimuliOn(new StimulusConfig(StimulusId.BSTI1, 13.5, 2, useSense: true))
                            .HasStimuliOn(new StimulusConfig(StimulusId.DRI1, 5.0, 0.1, StimulusConnectionPoint.ABUS1, StimulusConnectionPoint.ABUS4))
                        );

            Thread.Sleep(100);


            test = GetTest("FCT081001");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081002");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081003");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081004");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081005");
            TestLibrary.Voltage(test);

            test = GetTest("FCT081006");
            TestLibrary.Voltage(test);
        }
    }
}
