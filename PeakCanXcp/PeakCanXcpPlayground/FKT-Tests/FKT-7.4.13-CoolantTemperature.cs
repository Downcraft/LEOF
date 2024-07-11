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

    class Fkt7413_CoolantTemperature : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT01301", NetName = "COOL_NTC_RANGE_MCU_TEMP", Testpoint = "TP6400", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT01302", NetName = "COOL_NTC_ANA_TEMP_MCU", Testpoint = "TP6442", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
            new Test{TestItem = "FCT01303", NetName = "COOL_NTC_RANGE_MCU_TEMP", Testpoint = "TP6400", Min = 3.28, Typ = 3.33, Max = 3.38, Unit = "V"},
            new Test{TestItem = "FCT01304", NetName = "COOL_NTC_ANA_TEMP_MCU", Testpoint = "TP6442", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            DutController.DisableSafeState(xcp, a2l);

            // set Range to 1
            DutController.SetFlag(xcp, a2l, "IoEcu_cCltTempOvrrd.RngOvrrdMode", 0x01, 10);
            Thread.Sleep(40);

            // FCT01301 (Analog)

            // read Temperature and compare to reference Temp.

            // Get reference temperature from Coolant NTC by measuring resistance with FKT.
            var referenceTemp = GetCoolantReferenceTemperature();
            var offset = 19.84092331;

            TestItems.Where(t => t.TestItem.Last() < '3' && t.Unit == "°C").ToList().ForEach(t =>
            {
                t.Typ = referenceTemp;
                t.Min = referenceTemp - 2;
                t.Max = referenceTemp + 2;
            });

            TestItems.Where(t => t.TestItem.Last() > '3' && t.Unit == "°C").ToList().ForEach(t =>
            {
                t.Typ = referenceTemp - offset;
                t.Min = referenceTemp - 2 - offset;
                t.Max = referenceTemp + 2 - offset;
            });

            // FCT01302
            TestCoolantTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01302"));

            // set Range to 2
            DutController.SetFlag(xcp, a2l, "IoEcu_cCltTempOvrrd.RngOvrrdMode", 0x02, 10);
            Thread.Sleep(40);

            // FCT01303 (Analog)

            // FCT01304
            TestCoolantTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01304"));

            // set Range to 0
            DutController.SetFlag(xcp, a2l, "IoEcu_cCltTempOvrrd.RngOvrrdMode", 0x00, 10);

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }

        private static void TestCoolantTemperatureMcu(Xcp xcp, A2lParser a2l, Test test)
        {
             test.Measured = xcp.GetConvertedValue(a2l.Measurements[$"IoEcu_CltTemp.Val"]).Value;

            if (test.Measured <= test.Max && test.Measured >= test.Min)
            {
                test.Result = PASS;
            }
            else
            {
                test.Result = FAIL;
            }
        }

        private static double GetCoolantReferenceTemperature()
        {
            // Measure voltage between 5V_ANA and GND
            var v1 = 5.0;
            // Measure voltage between TP6443 abd GND
            var v2 = 3.8;

            var r2 = 24000.0;

            var rt = ((v1 - v2) / v2) * r2;
            var b = 3380;
            var rn = 10000.0;

            var temperature = 1.0 / ((1.0 / ToKelvin(25)) + ((1.0 / b) * Math.Log(rt / rn)));

            return ToCelsius(temperature);
        }
    }
}
