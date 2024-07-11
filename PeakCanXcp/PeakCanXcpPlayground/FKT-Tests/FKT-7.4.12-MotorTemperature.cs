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

    class Fkt7412_MotorTemperature : FktTest
    {
        private static List<Test> TestItems { get; set; } = new List<Test>
        {
            new Test{TestItem = "FCT01201", NetName = "T_MOT1_RANGE_MCU_TEMP_MSMT", Testpoint = "TP4114", Min = 0, Typ = 0, Max = 0.33, Unit = "V"},
            new Test{TestItem = "FCT01202", NetName = "T_MOT1_ANA_TEMP_MCU", Testpoint = "TP6430", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
            new Test{TestItem = "FCT01203", NetName = "T_MOT1_RANGE_MCU_TEMP_MSMT", Testpoint = "TP4114", Min = 3.28, Typ = 3.33, Max = 3.38, Unit = "V"},
            new Test{TestItem = "FCT01204", NetName = "T_MOT1_ANA_TEMP_MCU", Testpoint = "TP6430", Min = 0, Typ = 0, Max = 0, Unit = "°C"},
        };

        public static void Test(Xcp xcp, A2lParser a2l, int site)
        {
            DutController.DisableSafeState(xcp, a2l);

            // set Range to 1
            DutController.SetFlag(xcp, a2l, "IoMotTemp_cWndgTemp1Ovrrd.RngOvrrdMode", 0x01, 10);
            Thread.Sleep(40);

            // FCT01201 (Analog)

            // read Temperature and compare to reference Temp.
            var referenceTemp = GetMotorReferenceTemperature();
            var offset = 40.76340485;

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

            // FCT01202
            TestMotorTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01202"));

            // set Range to 2
            DutController.SetFlag(xcp, a2l, "IoMotTemp_cWndgTemp1Ovrrd.RngOvrrdMode", 0x02, 10);
            Thread.Sleep(40);

            // FCT01203 (Analog)

            // FCT01204
            TestMotorTemperatureMcu(xcp, a2l, TestItems.GetTest("FCT01204"));

            // set Range to 0
            DutController.SetFlag(xcp, a2l, "IoMotTemp_cWndgTemp1Ovrrd.RngOvrrdMode", 0x00, 10);

            foreach (var test in TestItems)
            {
                FktTest.SetTestResult(test);
            }
        }

        private static double GetMotorReferenceTemperature()
        {
            // Measure resistor (analog)
            var measuredResistor = 17420.0;

            var b = 2305.0;
            var rn = 17100.0;
            var expectedTemperature = 1.0 / ((1.0 / ToKelvin(25)) + ((1.0 / b) * Math.Log(measuredResistor / rn)));

            return ToCelsius(expectedTemperature);
        }

        private static void TestMotorTemperatureMcu(Xcp xcp, A2lParser a2l, Test test)
        {
            test.Measured = xcp.GetConvertedValue(a2l.Measurements[$"IoMotTemp_WndgTemp1.Val"]).Value;

            if (test.Measured <= test.Max && test.Measured >= test.Min)
            {
                test.Result = PASS;
            }
            else
            {
                test.Result = FAIL;
            }
        }
    }
}
