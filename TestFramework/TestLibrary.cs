// <copyright file="TestLibrary.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace Program
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading.Tasks;
    using Spea;
    using Spea.Instruments;
    using Spea.TestEnvironment;
    using PeakCanXcp;
    using Spea.TestFramework;

    /// <summary>
    /// A collection of test methods that can be applied to a <see cref="TestItem"/> instance.
    /// </summary>
    public static class TestLibrary
    {
        /* =======================================================
         * PUBLIC STATIC METHODS
         * ======================================================= */

        /// <summary>
        /// Tests the DC voltage between the two test points of the given <see cref="TestItem"/> using the DVM.
        /// The measured value is automatically sotred in the test item and the test points are automatically connected to ABUS.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with exactly 2 TPs.</param>
        /// <param name="testEnvironment">The test environment.</param>
        /// <param name="measureTime">The measure time in seconds.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance does not contain exactly two test points.</exception>
        public static (double Measured, TestResult Result) Voltage(TestItem testItem, double measureTime = 0.01, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(Voltage));

            var voltage = MeasureDc(testItem, measureTime, range, hot, cold);

            // Round measurement to relevant decimal digits based on DVM accuracy.
            voltage = RoundDc(range, voltage);

            testItem.Measured = voltage;

            return (testItem.Measured, testItem.Result);
        }

        /// <summary>
        /// Tests the RMS voltage between the two test points of the given <see cref="TestItem"/> using the DVM.
        /// The the measured value is automatically sotred in the test item and the test points are automatically connected to ABUS.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with exactly 2 TPs.</param>
        /// <param name="testEnvironment">The test environment.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance does not contain exactly two test points.</exception>
        public static (double Measured, TestResult Result) VoltageRms(TestItem testItem, double expectedFrequency, int periodes = 10, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(VoltageRms));

            var parameters = MeasureAc(testItem, expectedFrequency, periodes, range, hot, cold);

            // Round measurement to relevant decimal digits based on DVM accuracy.
            var rms = RoundAc(range, parameters.Rms);

            testItem.Measured = rms;

            return (testItem.Measured, testItem.Result);
        }

        /// <summary>
        /// Tests the current on the selected stimulus' output.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with no TPs.</param>
        /// <param name="stimulus">The <see cref="Stimulus"/> instance.</param>
        /// <param name="timeOnMs">The measure time in seconds.</param>
        /// <param name="numberOfDvmCycles">The number of times the measurement should be performed. The average of these are the final result.</param>
        /// <param name="digits">The number of decimal digits of the result. It controls how the measurement is rounded.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance contains any test points.</exception>
        public static (double Measured, TestResult Result) StimulusCurrent(TestItem testItem, Stimulus stimulus, double timeOnMs = 10.0, int numberOfDvmCycles = 1, int digits = 4)
        {
            // Ensure test item has exactly 0 TPs.
            GuardTpCount(testItem, 0, nameof(StimulusCurrent));

            stimulus.MeasureStimulusCurrent(testItem.Minimal, testItem.Maximal, out var current, timeOnMs, numberOfDvmCycles);

            // Round measurement to relevant decimal digits based on user input.
            current = Math.Round(current, digits);

            testItem.Measured = current;

            return (testItem.Measured, testItem.Result);
        }

        /// <summary>
        /// Tests the frequency of the signal measured between the two test points of the given <see cref="TestItem"/> using the DVM.
        /// The the measured value is automatically sotred in the test item and the test points are automatically connected to ABUS.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with exactly 2 TPs.</param>
        /// <param name="testEnvironment">The test environment.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance does not contain exactly two test points.</exception>
        public static (double Measured, TestResult Result) Frequency(TestItem testItem, double expectedFrequency, int periodes = 10, int digits = 3, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(Frequency));

            var parameters = MeasureAc(testItem, expectedFrequency, periodes, range, hot, cold);

            testItem.Measured = Math.Round(parameters.Frequency, digits);

            return (testItem.Measured, testItem.Result);
        }

      


        /// <summary>
        /// Tests the frequency of the signal measured between the two test points of the given <see cref="TestItem"/> using the DVM.
        /// The the measured value is automatically sotred in the test item and the test points are automatically connected to ABUS.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with exactly 2 TPs.</param>
        /// <param name="testEnvironment">The test environment.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance does not contain exactly two test points.</exception>
        public static PwmProperties PWM(TestItem testItem, double expectedFrequency, double thresholdLow, double thresholdHigh, int periodes = 10, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(PWM));

            var parameters = MeasureAc(testItem, expectedFrequency,thresholdLow, thresholdHigh, periodes, range, hot, cold);


            return parameters;
        }


        /// <summary>
        /// Tests the Peak-to-Peak voltage between the two test points of the given <see cref="TestItem"/> using the DVM.
        /// The the measured value is automatically sotred in the test item and the test points are automatically connected to ABUS.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with exactly 2 TPs.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance does not contain exactly two test points.</exception>
        public static (double Measured, TestResult Result) VoltagePeakToPeak(TestItem testItem, double expectedFrequency, int periodes = 10, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(VoltagePeakToPeak));

            var parameters = MeasureAc(testItem, expectedFrequency, periodes, range, hot, cold);

            double max = parameters.Max;
            double min = parameters.Min;

            if ((range == DvmVRange.R100V) && (expectedFrequency > 9500) && (expectedFrequency < 11500))
            {
                max = ((max - parameters.DcOffset) * 1.41253754462) + parameters.DcOffset;
                min = ((min - parameters.DcOffset) * 1.41253754462) + parameters.DcOffset;
            }

            // Round measurement to relevant decimal digits based on DVM accuracy.
            max = RoundAc(range, max);
            min = RoundAc(range, min);

            testItem.Measured = max - min;

            return (testItem.Measured, testItem.Result);
        }

        /// <summary>
        /// Tests the positive peak voltage between the two test points of the given <see cref="TestItem"/> using the DVM.
        /// The the measured value is automatically sotred in the test item and the test points are automatically connected to ABUS.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with exactly 2 TPs.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance does not contain exactly two test points.</exception>
        public static (double Measured, TestResult Result) VoltagePositivePeak(TestItem testItem, double expectedFrequency, int periodes = 10, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(VoltagePositivePeak));

            var parameters = MeasureAc(testItem, expectedFrequency, periodes, range, hot, cold);

            var max = parameters.Max;

            if ((range == DvmVRange.R100V) && (expectedFrequency > 9500) && (expectedFrequency < 11500))
            {
                max = ((max - parameters.DcOffset) * 1.41253754462) + parameters.DcOffset;
            }

            // Round measurement to relevant decimal digits based on DVM accuracy.
            max = RoundAc(range, max);

            testItem.Measured = max;

            return (testItem.Measured, testItem.Result);
        }

        public static (double Measured, TestResult Result) VoltageAveragePositiveAmplitude(TestItem testItem, double expectedFrequency, int periodes = 10, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(VoltagePositivePeak));

            var parameters = MeasureAc(testItem, expectedFrequency, periodes, range, hot, cold);

            var averageAmplitude = parameters.MeasuredValues.Where(x => x > 0).Average();

            if ((range == DvmVRange.R100V) && (expectedFrequency > 9500) && (expectedFrequency < 11500))
            {
                averageAmplitude = ((averageAmplitude - parameters.DcOffset) * 1.41253754462) + parameters.DcOffset;
            }

            // Round measurement to relevant decimal digits based on DVM accuracy.
            averageAmplitude = RoundAc(range, averageAmplitude);

            testItem.Measured = averageAmplitude;

            return (testItem.Measured, testItem.Result);
        }

        /// <summary>
        /// Tests the negative peak voltage between the two test points of the given <see cref="TestItem"/> using the DVM.
        /// The the measured value is automatically sotred in the test item and the test points are automatically connected to ABUS.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with exactly 2 TPs.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance does not contain exactly two test points.</exception>
        public static (double Measured, TestResult Result) VoltageNegativePeak(TestItem testItem, double expectedFrequency, int periodes = 10, DvmVRange range = DvmVRange.R100V, Abus hot = Abus.ABUS2, Abus cold = Abus.ABUS3)
        {
            // Ensure test item has exactly 2 TPs.
            GuardTpCount(testItem, 2, nameof(VoltageNegativePeak));

            var parameters = MeasureAc(testItem, expectedFrequency, periodes, range, hot, cold);

            var min = parameters.Min;

            if ((range == DvmVRange.R100V) && (expectedFrequency > 9500) && (expectedFrequency < 11500))
            {
                min = ((min - parameters.DcOffset) * 1.41253754462) + parameters.DcOffset;
            }

            // Round measurement to relevant decimal digits based on DVM accuracy.
            min = RoundAc(range, min);

            testItem.Measured = min;

            return (testItem.Measured, testItem.Result);
        }

        /* =======================================================
         * PRIVATE STATIC METHODS
         * ======================================================= */



        /// <summary>
        /// Rounds DC voltage to a number of decimal digits based on DVM accuracy.
        /// </summary>
        /// <param name="range">The DVM range.</param>
        /// <param name="voltage">The measured voltage.</param>
        /// <returns>The rounded value.</returns>
        private static double RoundDc(DvmVRange range, double voltage)
        {
            // m.v. accuracy for DC measurements based on DVM data sheet.
            double measuredValueAccuracy = 0.00005;
            return RoundVoltage(range, ref voltage, measuredValueAccuracy);
        }

        /// <summary>
        /// Rounds AC voltage to a number of decimal digits based on DVM accuracy.
        /// </summary>
        /// <param name="range">The DVM range.</param>
        /// <param name="voltage">The measured voltage.</param>
        /// <returns>The rounded value.</returns>
        private static double RoundAc(DvmVRange range, double voltage)
        {
            // m.v. accuracy for AC measurements based on DVM data sheet.
            double measuredValueAccuracy = 0.0005;
            return RoundVoltage(range, ref voltage, measuredValueAccuracy);
        }

        /// <summary>
        /// Rounds voltage to a number of decimal digits based on F.S. accuracy and m.v. accuracy which depends on kind of measurement (DC/AC).
        /// </summary>
        /// <param name="range">The DVM range.</param>
        /// <param name="voltage">The measured voltage.</param>
        /// <returns>The rounded value.</returns>
        private static double RoundVoltage(DvmVRange range, ref double voltage, double measuredValueAccuracy)
        {
            // Determine F.S. value.
            double fullScale = 100;
            switch (range)
            {
                case DvmVRange.R100mV:
                    fullScale = 0.1;
                    break;
                case DvmVRange.R1V:
                    fullScale = 1;
                    break;
                case DvmVRange.R10V:
                    fullScale = 10;
                    break;
                case DvmVRange.R100V:
                    fullScale = 100;
                    break;
            }

            // Calculate accuracy based on DVM data sheet.
            var accuracy = (measuredValueAccuracy * Math.Abs(voltage)) + (0.0001 * fullScale);

            // Round to number of accurate digits plus one.
            var digits = (int)Math.Floor(Math.Abs(Math.Log10(accuracy))) + 1;

            voltage = Math.Round(voltage, digits);

            return voltage;
        }

        /// <summary>
        /// Connects TPs for DVM measurement. Connects 1st test point to selected hot ABUS and 2nd test point to cold ABUS.
        /// </summary>
        /// <param name="testItem">A test item containing two test points.</param>
        /// <param name="hot">The hot ABUS.</param>
        /// <param name="cold">The cold ABUS.</param>
        private static void ConnectTps(TestItem testItem, Abus hot, Abus cold)
        {
            LeoF.TpConnectAbus(new List<int>{ testItem.TestPoints[0].Number }, hot);
            LeoF.TpConnectAbus(new List<int> { testItem.TestPoints[1].Number }, cold);           
        }

        /// <summary>
        /// Connects TPs for DVM measurement. Connects 1st test point to selected hot ABUS and 2nd test point to cold ABUS.
        /// </summary>
        /// <param name="testItem">A test item containing two test points.</param>
        /// <param name="hot">The hot ABUS.</param>
        /// <param name="cold">The cold ABUS.</param>
        private static void DisconnectTps(TestItem testItem, Abus hot, Abus cold)
        {
            LeoF.TpDisconnectAbus(new List<int> { testItem.TestPoints[0].Number }, hot);
            LeoF.TpDisconnectAbus(new List<int> { testItem.TestPoints[1].Number }, cold);
        }

        /// <summary>
        /// Disconnects TPs after DVM measurement. Disconnects 1st test point from selected hot ABUS and 2nd test point from cold ABUS.
        /// </summary>
        /// <param name="testEnvironment">The test environment.</param>
        /// <param name="previousState">The state to be restored.</param>
        private static void RestoreState(TestEnv<UserFlagPurpose, PmxPurpose> testEnvironment, ReadOnlyState<UserFlagPurpose, PmxPurpose> previousState)
        {
            testEnvironment.Set(state => 
            {
                state
                .HasActiveUserFlags(previousState.ActiveUserFlags)
                .HasStimuliOn(previousState.StimuliOn.Values)
                .HasFpsOn(previousState.FpsOn);

                foreach (var abus in previousState.TpsConnectedToAbus.Keys)
                {
                    state.HasTpsConnectedToAbus(abus, previousState.TpsConnectedToAbus[abus]);
                }

                return state;
            });
        }

        /// <summary>
        /// Measures DC voltage. It automatically connects and disconnects the test points entered in the <see cref="TestItem"/> instance to the ABUS.
        /// </summary>
        /// <param name="testItem">A test item containing two test points.</param>
        /// <param name="measureTime">The measure time in seconds.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>The measured DC voltage.</returns>
        private static double MeasureDc(TestItem testItem, double measureTime, DvmVRange range, Abus hot, Abus cold)
        {
            ConnectTps(testItem, hot, cold);

            Dvm.AutoMeasureDc((DvmHotConnectionPoint)hot, (DvmColdConnectionPoint)cold, out var voltage, measureTime, range);

            DisconnectTps(testItem, hot, cold);
            return voltage;
        }

        /// <summary>
        /// Measures AC voltage. It automatically connects and disconnects the test points entered in the <see cref="TestItem"/> instance to the ABUS.
        /// </summary>
        /// <param name="testItem">A test item containing two test points.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>The measured AC voltage.</returns>
        private static SinusProperties MeasureAc(TestItem testItem, double expectedFrequency, int periodes, DvmVRange range, Abus hot, Abus cold)
        {
            ConnectTps(testItem, hot, cold);

            Dvm.AutomaticMeasureSinusLike((DvmHotConnectionPoint)hot, (DvmColdConnectionPoint)cold, expectedFrequency, periodes, out var parameters, range);

            DisconnectTps(testItem, hot, cold);
            return parameters;
        }

        /// <summary>
        /// Measures AC voltage. It automatically connects and disconnects the test points entered in the <see cref="TestItem"/> instance to the ABUS.
        /// </summary>
        /// <param name="testItem">A test item containing two test points.</param>
        /// <param name="expectedFrequency">The expected frequency of the signal being measured.</param>
        /// <param name="periodes">The number of periodes that should be sampled during the measurement.</param>
        /// <param name="range">The DVM range.</param>
        /// <param name="hot">The ABUS connection for the hot wire of the DVM. The first test point will be connected to it as well.</param>
        /// <param name="cold">The ABUS connection for the cold wire of the DVM. The second test point will be connected to it as well.</param>
        /// <returns>The measured AC voltage.</returns>
        private static PwmProperties MeasureAc(TestItem testItem, double expectedFrequency, double thresholdLow, double thresholdHigh, int periodes, DvmVRange range, Abus hot, Abus cold)
        {
            ConnectTps(testItem, hot, cold);

            Dvm.AutomaticMeasurePWM((DvmHotConnectionPoint)hot, (DvmColdConnectionPoint)cold, expectedFrequency, periodes, thresholdLow, thresholdHigh, out var parameters, range);

            DisconnectTps(testItem, hot, cold);
            return parameters;
        }

        /// <summary>
        /// Throws an exception if <see cref="TestItem"/> instance does not contain the exactly amout of test points.
        /// </summary>
        /// <param name="testItem">The test item instance.</param>
        /// <param name="count">The number of test points to ensure.</param>
        /// <param name="measurementName">The name of the measurement being performed.</param>
        /// <exception cref="ArgumentException">When the number of test points is different than expected.</exception>
        private static void GuardTpCount(TestItem testItem, int count, string measurementName)
        {
            if (testItem.TestPoints.Count != count)
            {
                throw new ArgumentException($"Exact {count} test points are needed for a {measurementName} measurement.");
            }
        }

        /// <summary>
        /// Tests the value retrieved via XCP.
        /// </summary>
        /// <param name="testItem">A <see cref="TestItem"/> instance with no TPs.</param>
        /// <param name="xcp">A <see cref="Xcp"/> instance which was already connected.</param>
        /// <param name="value">The <see cref="XcpValue"/> which should be retrieved from the ECU and tested.</param>
        /// <returns>A tuple containing the measured value and the result of the test.</returns>
        /// <exception cref="ArgumentException">When the <see cref="TestItem"/> instance contains any test points.</exception>
        public static (double Measured, TestResult Result) Xcp(TestItem testItem, Xcp xcp, XcpValue value, int numberOfMeasurements = 1, int digits = 3)
        {
            // Ensure test item has exactly 0 TPs.
            GuardTpCount(testItem, 0, nameof(Xcp));

            var measurements = new List<double>();
            for (int i = 0; i < numberOfMeasurements; i++)
            {
                measurements.Add(xcp.GetConvertedValue(value).Value);
            }

            testItem.Measured = Math.Round(measurements.Average(), digits);

            return (testItem.Measured, testItem.Result);
        }


        public static void doMeasurements (Xcp xcp,  string measure, A2lParser parser)
        {
            var measurements = parser.Measurements.Keys.ToList().Where(k=>k.StartsWith(measure)).ToList();

            foreach (var measurement in measurements)
            {
                var value = parser.Measurements[measurement];
                var val = xcp.GetConvertedValue(value).Value;

                LogController.Print($"Measurement: {measurement} = {val}");
            }
        }

    }
}
