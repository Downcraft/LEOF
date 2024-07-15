using Spea;
using Spea.Instruments;
using Spea.TestFramework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Program
{
    internal static class Fct8341_AKSSSOSwitchingHelpers
    {

       
        public static Stimulus HvStimulus;
        private static double StepSize = 0.1/ 357.7817531305903;
        private static int delayInMs = 10;

        private static List<TestItem> testItems = new List<TestItem>();
                

        public static void SetTests(TestItem LevelTest, TestItem triggerTest)
        {
            testItems = new List<TestItem>();
            testItems.Add(LevelTest);
            testItems.Add(triggerTest);
        }

        public static void RampDown(double startVoltage, double endVoltage)
        {
            int factor = -1;
            Ramp(startVoltage, endVoltage, factor);
        }

        public static void RampUp(double startVoltage, double endVoltage)
        {
            int factor = 1;
            Ramp(startVoltage, endVoltage, factor);
        }

        private static void Ramp(double startVoltage, double endVoltage, int factor)
        {
            var levelTest = testItems[0];
            var triggerTest = testItems[1];

            var newStep = factor * StepSize;

            for (double voltage = startVoltage; factor < 0 ? voltage >= endVoltage : voltage <= endVoltage; voltage += newStep)
            {
                HvStimulus.SetStimulus(voltage, 0.1);

                Thread.Sleep(delayInMs);

                if (!((triggerTest.Measured != double.NaN) && (triggerTest.Measured != 0)))
                {

                    TestLibrary.Voltage(levelTest, range: DvmVRange.R10V);

                    if (levelTest.Result == TestResult.PASS)
                    {
                        triggerTest.Measured = Math.Round(TestLibrary.Voltage(triggerTest, range: DvmVRange.R1V).Measured * 357.7817531305903, 3);                        
                    }
                }

                //LogController.Print($"{Math.Round(voltage* 357.7817531305903)} V");
            }            

            if(levelTest.Result != TestResult.PASS)
            {
                triggerTest.Measured = double.NaN;
            }
        }    
    }
}