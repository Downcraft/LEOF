namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AtosF;
    using static AtosF.modAtosF;
    using static AtosF.modAtosF2;
    using PeakCanXcp;

    class Fkt7411_PmTemperature : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT01101", NetName = "PM_NTC_RANGE_MCU_TEMP", Testpoint = "TP6416", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT01102", NetName = "PM1_NTC_ANA_TEMP_MCU", Testpoint = "TP6420", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
            new Test{TestItem = "FCT01103", NetName = "PM2_NTC_ANA_TEMP_MCU", Testpoint = "TP6411", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
            new Test{TestItem = "FCT01104", NetName = "PM3_NTC_ANA_TEMP_MCU", Testpoint = "TP6403", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
            new Test{TestItem = "FCT01105", NetName = "PM_NTC_RANGE_MCU_TEMP", Testpoint = "TP6416", Min = 3.28, Typ = 3.33, Max = 3.38, Unit = "V"},
            new Test{TestItem = "FCT01106", NetName = "PM1_NTC_ANA_TEMP_MCU", Testpoint = "TP6420", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
            new Test{TestItem = "FCT01107", NetName = "PM2_NTC_ANA_TEMP_MCU", Testpoint = "TP6411", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
            new Test{TestItem = "FCT01108", NetName = "PM3_NTC_ANA_TEMP_MCU", Testpoint = "TP6403", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            DutController.DisableSafeState(xcp, a2l);

            // set Range to 1
            DutController.SetFlag(xcp, a2l, "IoInvTemp_cRngOvrrdMode", 0x01, 10);
            Thread.Sleep(40);

            // FCT01101 (Analog)

            // read Temperature and compare to reference Temp.
            var referenceTemp = 23.0;
            var offset = 16.60486554450756;

            TestItems.Where(t => t.TestItem.Last() < '5' && t.Unit == "°C").ToList().ForEach(t =>
            {
                t.Typ = referenceTemp;
                t.Min = referenceTemp - 2;
                t.Max = referenceTemp + 2;
            });

            TestItems.Where(t => t.TestItem.Last() > '5' && t.Unit == "°C").ToList().ForEach(t =>
            {
                t.Typ = referenceTemp - offset;
                t.Min = referenceTemp - 2 - offset;
                t.Max = referenceTemp + 2 - offset;
            });

            // FCT01102
            TestPmTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01102"), 1);
            // FCT01103
            TestPmTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01103"), 2);
            // FCT01104
            TestPmTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01104"), 3);

            // set Range to 2
            DutController.SetFlag(xcp, a2l, "IoInvTemp_cRngOvrrdMode", 0x02, 10);
            Thread.Sleep(40);

            // FCT01105 (Analog)

            // FCT01102
            TestPmTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01106"), 1);
            // FCT01103
            TestPmTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01107"), 2);
            // FCT01104
            TestPmTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01108"), 3);

            // set Range to 0
            DutController.SetFlag(xcp, a2l, "IoInvTemp_cRngOvrrdMode", 0x00, 10);

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }

        private static void TestPmTemperatureMcu(Xcp xcp, A2lParser a2l, Test test, int powerModule)
        {
            test.Measured = xcp.GetConvertedValue(a2l.Measurements[$"IoInvTemp_PwrModule{powerModule}Temp.Val"]).Value;

            test.Measured = NtcConvertToNxv08h350st2(test.Measured);

            if (test.Measured <= test.Max && test.Measured >= test.Min)
            {
                test.Result = PASS;
            }
            else
            {
                test.Result = FAIL;
            }
        }

        private static double NtcConvertToNxv08h350st2(double temperature)
        {
            var tn = ToKelvin(25);
            temperature = ToKelvin(temperature);

            // NTC in NXV08H250DT12
            // MURATA 10k 1% B @25/55 = 3380
            var b = 3380.0;
            var rn = 10000;

            var rt = rn * Math.Exp(b * ((1.0 / (temperature)) - (1.0 / tn)));

            // NTC in NXV08H350ST2
            // MURATA 47k 1% B @25/55 = 4050
            b = 4050.0;
            rn = 47000;

            temperature = (1.0 / ((1.0 / tn) + (1.0 / b) * Math.Log(rt / rn)));

            return ToCelsius(temperature);
        }
    }
}
