namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using PicoTechnology;
    using ScottPlot;
    using CsvHelper;
    using CsvHelper.Configuration.Attributes;
    using CsvHelper.Configuration;
    using AtosF;
    using static AtosF.modAtosF;
    using static AtosF.modAtosF2;
    using PeakCanXcp;
    using System.Threading.Tasks;

    /// <summary>
    /// Class responsible for testing the test item "7.4.8 Gate Driver"
    /// Test Item: Gate Driver<para/>
    /// Functionblock: FB5<para/>
    /// Test description: the Gate-Signals are controlled via XCP and the PM-Signals are read with the FCT. Measure the fall time of the GS signal with a scope.<br/>
    /// Input: PWM_HS, PWM_LS<para/>
    /// XCP Command: Set duty cycle to 30% via XCP Command and perform measurement with scope.<para/>
    /// Test Failure: Fall time of the GS Signal is outside of the envelope in picture 7.10 (see Test specification).<para/>
    /// </summary>
    public class Fkt748_GateDriver : FktTest
    {
        private static double ProbeMultiplier { get; set; } = 10.1;

        private static PicoScope4000a.Range PicoScopeRange { get; set; } = PicoScope4000a.Range.Range_2V;

        private static double TriggerLevel { get; set; } = 0.25;

        private static double TriggerHyteresisLevel { get; set; } = 0.025;

        private static TimeSpan AutoTriggerTimeout { get; set; } = new TimeSpan(0, 0, 0, 0, 30);

        private static string LowSidePicoscopeSN { get; set; } = "IO714/0094";

        private static string HighSidePicoscopeSN { get; set; } = "IO714/0094";

        private static List<ConnectionGateDriverToScope> Connections { get; set; } = new List<ConnectionGateDriverToScope>
        {
           new ConnectionGateDriverToScope { GateDriverNumber = 1, ScopeChannel = PicoScope4000a.Channel.CHANNEL_A },
           new ConnectionGateDriverToScope { GateDriverNumber = 5, ScopeChannel = PicoScope4000a.Channel.CHANNEL_B },
           new ConnectionGateDriverToScope { GateDriverNumber = 2, ScopeChannel = PicoScope4000a.Channel.CHANNEL_C },
           new ConnectionGateDriverToScope { GateDriverNumber = 6, ScopeChannel = PicoScope4000a.Channel.CHANNEL_D },
           new ConnectionGateDriverToScope { GateDriverNumber = 3, ScopeChannel = PicoScope4000a.Channel.CHANNEL_E },
           new ConnectionGateDriverToScope { GateDriverNumber = 4, ScopeChannel = PicoScope4000a.Channel.CHANNEL_F },
        };

        private static Dictionary<RelayFunction, UserFlagRelay> UserFlags { get; set; } = new Dictionary<RelayFunction, UserFlagRelay>
        {
            {RelayFunction.LowSideDrivers,  new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 15 } },
            {RelayFunction.HighSideDriver1, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 9 } },
            {RelayFunction.HighSideDriver2, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 11 } },
            {RelayFunction.HighSideDriver3, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 13 } },
            {RelayFunction.HighSideDriver4, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 14 } },
            {RelayFunction.HighSideDriver5, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 10 } },
            {RelayFunction.HighSideDriver6, new UserFlagRelay { Group = UserFlagGroup.RLYNOA, FlagNumber = 12 } }
        };

        private static Dictionary<PicoScope4000a.Channel, Test> LowsideTestItemResults { get; set; } = new Dictionary<PicoScope4000a.Channel, Test>
        {
            {PicoScope4000a.Channel.CHANNEL_A, new Test { TestItem = "FCT00804", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_B, new Test { TestItem = "FCT00812", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_C, new Test { TestItem = "FCT00806", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_D, new Test { TestItem = "FCT00814", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_E, new Test { TestItem = "FCT00808", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_F, new Test { TestItem = "FCT00810", Result = 0} },
        };

        private static Dictionary<PicoScope4000a.Channel, Test> HighsideTestItemResults { get; set; } = new Dictionary<PicoScope4000a.Channel, Test>
        {
            {PicoScope4000a.Channel.CHANNEL_A, new Test { TestItem = "FCT00803", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_B, new Test { TestItem = "FCT00811", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_C, new Test { TestItem = "FCT00805", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_D, new Test { TestItem = "FCT00813", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_E, new Test { TestItem = "FCT00807", Result = 0} },
            {PicoScope4000a.Channel.CHANNEL_F, new Test { TestItem = "FCT00809", Result = 0} },
        };

        private static List<Envelope> Envelope;

        private static double ScopeRange
        {
            get
            {
                switch (PicoScopeRange)
                {
                    case PicoScope4000a.Range.Range_10MV:
                        return 10_000_000.0;
                    case PicoScope4000a.Range.Range_20MV:
                        return 20_000_000.0;
                    case PicoScope4000a.Range.Range_50MV:
                        return 50_000_000.0;
                    case PicoScope4000a.Range.Range_100MV:
                        return 100_000_000.0;
                    case PicoScope4000a.Range.Range_200MV:
                        return 200_000_000.0;
                    case PicoScope4000a.Range.Range_500MV:
                        return 500_000_000.0;
                    case PicoScope4000a.Range.Range_1V:
                        return 1.0;
                    case PicoScope4000a.Range.Range_2V:
                        return 2.0;
                    case PicoScope4000a.Range.Range_5V:
                        return 5.0;
                    case PicoScope4000a.Range.Range_10V:
                        return 10.0;
                    case PicoScope4000a.Range.Range_20V:
                        return 20.0;
                    case PicoScope4000a.Range.Range_50V:
                        return 50.0;
                    case PicoScope4000a.Range.Range_100V:
                        return 100.0;
                    case PicoScope4000a.Range.Range_200V:
                        return 200.0;
                }

                return 1.0;
            }
        }

        public async static Task<bool> TestAsync(Xcp xcp, A2lParser a2l, float pwmDutyCycle = 30.0f, bool printPlot = false)
        {
            // Set the number of samples to be captured by the PicoScopes.
            // For analysing the characteristic of the gate driver pwm, we need at least 200 samples, but for triggering the scope on the right
            // place it is necessary to get at least 2 periods of the pwm. The pwm has 10 kHz (100µs period) frequency and we are sampling each 25 ns.
            // Therefore we need 2 * 100000 ns / 25ns = 8000.
            var samples = 8000;

            var openScopeLowsideTask = Task.Run(() => OpenScope(Fkt748_GateDriver.LowSidePicoscopeSN));

            Task<short> openScopeHighsideTask = new Task<short>(() => 0);

           var openThreadSafeTask = Task.Run(async () =>
           {
               if (Fkt748_GateDriver.HighSidePicoscopeSN != Fkt748_GateDriver.LowSidePicoscopeSN)
               {
                   await openScopeLowsideTask;
                   openScopeHighsideTask = Task.Run(() => OpenScope(Fkt748_GateDriver.HighSidePicoscopeSN));
               }
           });

            var envelopeTask = Task.Run(() =>InitEnvelope());

            await Task.Run(() => Precondition(xcp, a2l));

            await Task.Run(() => SetPwmForAllConnections(xcp, a2l, pwmDutyCycle));

            var handleLowside = await openScopeLowsideTask;

            var handleHighside = handleLowside;

            (var LowSideFalling, var LowSideRising, var timeIntervalNs) = MeasureLowSide(handleLowside, samples);

            if (Fkt748_GateDriver.HighSidePicoscopeSN != Fkt748_GateDriver.LowSidePicoscopeSN)
            {
                handleHighside = await openScopeHighsideTask;
            }

            (var HighSideFalling, var HighSideRising, _) = MeasureHighSide(handleHighside, samples);

            var stopScopeLowsideTask = Task.Run(() => StopScope(handleLowside));

            Task stopScopeHighsideTask = new Task(() => {}) ;

            if (Fkt748_GateDriver.HighSidePicoscopeSN != Fkt748_GateDriver.LowSidePicoscopeSN)
            {
                stopScopeHighsideTask = Task.Run(() => StopScope(handleHighside));
            }

            await Task.Run(() => SetPwmForAllConnections(xcp, a2l, 0.0f));

            await Task.Run(() => ResetPrecondition(xcp, a2l));

            bool testResult = true;

            await envelopeTask;

            var evaluateLowsideTasks = new Dictionary<(PicoScope4000a.Channel, PicoScope4000a.ThresholdDirection), Task<bool>>();

            var evaluateHighsideTasks = new Dictionary<(PicoScope4000a.Channel, PicoScope4000a.ThresholdDirection), Task<bool>>();

            foreach (var measured in LowSideFalling)
            {
                var direction = PicoScope4000a.ThresholdDirection.Falling;
                var label = "GL" + Connections.Where(c => c.ScopeChannel == measured.Key).Select(c => c.GateDriverNumber).Single();
                evaluateLowsideTasks.Add((measured.Key, direction), Task.Run(() => EvaluateCurve(direction, measured.Value, timeIntervalNs, printPlot, label)));
            }

            foreach (var measured in LowSideRising)
            {
                var direction = PicoScope4000a.ThresholdDirection.Rising;
                var label = "GL" + Connections.Where(c => c.ScopeChannel == measured.Key).Select(c => c.GateDriverNumber).Single();
                evaluateLowsideTasks.Add((measured.Key, direction), Task.Run(() => EvaluateCurve(direction, measured.Value, timeIntervalNs, printPlot, label)));
            }

            foreach (var measured in HighSideFalling)
            {
                var direction = PicoScope4000a.ThresholdDirection.Falling;
                var label = "GH" + Connections.Where(c => c.ScopeChannel == measured.Key).Select(c => c.GateDriverNumber).Single();
                evaluateHighsideTasks.Add((measured.Key, direction), Task.Run(() => EvaluateCurve(direction, measured.Value, timeIntervalNs, printPlot, label)));
            }

            foreach (var measured in HighSideRising)
            {
                var direction = PicoScope4000a.ThresholdDirection.Rising;
                var label = "GH" + Connections.Where(c => c.ScopeChannel == measured.Key).Select(c => c.GateDriverNumber).Single();
                evaluateHighsideTasks.Add((measured.Key, direction), Task.Run(() => EvaluateCurve(direction, measured.Value, timeIntervalNs, printPlot, label)));
            }

            Task.WaitAll(evaluateLowsideTasks.Select(pair => pair.Value).ToArray());
            Task.WaitAll(evaluateHighsideTasks.Select(pair => pair.Value).ToArray());

            foreach (var task in evaluateLowsideTasks)
            {
                if (await task.Value == false)
                {
                    LowsideTestItemResults[task.Key.Item1].Result = 1;
                    testResult = false;
                }
            }

            foreach (var task in evaluateHighsideTasks)
            {
                if (await task.Value == false)
                {
                    LowsideTestItemResults[task.Key.Item1].Result = 1;
                    testResult = false;
                }
            }

            foreach (var result in LowsideTestItemResults.Select(pair => pair.Value).Union(HighsideTestItemResults.Select(pair => pair.Value)))
            {
                SetTestResult(result);
            }

            await stopScopeLowsideTask;

            if (Fkt748_GateDriver.HighSidePicoscopeSN != Fkt748_GateDriver.LowSidePicoscopeSN)
            {
                await stopScopeHighsideTask;
            }

            return testResult;
        }

        private static void Precondition(Xcp xcp, A2lParser a2l)
        {
            DutController.DisableSafeState(xcp, a2l);
            DutController.SetPowerStageMode(xcp, a2l, 0x00);
            DutController.SetFlag(xcp, a2l, "IoEm_cInvPhaseDucySpOvrrdEn", 0x01);
            DutController.SetFlag(xcp, a2l, "IoEm_cInvPhaseDucySpOvrrdEnFrc", 0x01);
        }

        private static void ResetPrecondition(Xcp xcp, A2lParser a2l)
        {
            DutController.SetFlag(xcp, a2l, "IoEm_cInvPhaseDucySpOvrrdEn", 0x00);
            DutController.SetFlag(xcp, a2l, "IoEm_cInvPhaseDucySpOvrrdEnFrc", 0x00);
        }

        private static void SetPwmForAllConnections(Xcp xcp, A2lParser a2l, float dutyCycle)
        {
            foreach (var connection in Connections)
            {
                DutController.SetPwm(xcp, a2l, dutyCycle, connection.GateDriverNumber, 3);
            }
        }

        private static (Dictionary<PicoScope4000a.Channel, List<double>> highSideFalling, Dictionary<PicoScope4000a.Channel, List<double>> highSideRising, int timeIntervalNs) MeasureHighSide(short handle, int samples)
        {
            var highSideFalling = new Dictionary<PicoScope4000a.Channel, List<double>>();
            var highSideRising = new Dictionary<PicoScope4000a.Channel, List<double>>();
            var timeIntervalNs = 0;
            for (var function = RelayFunction.HighSideDriver1; function <= RelayFunction.HighSideDriver6; function = function + 1)
            {
                SetSingleUserFlag(function, Fkt748_GateDriver.UserFlags);

                var channel = Connections.Where(c => c.GateDriverNumber == (int)function).Select(c => c.ScopeChannel).ToList();

                if (channel.Count == 0)
                {
                    continue;
                }

                var direction = PicoScope4000a.ThresholdDirection.Falling;

                timeIntervalNs = SetUpChannels(handle, channel, direction, samples);

                var result = GetValues(handle, channel, samples);

                foreach (var entry in result)
                {
                    highSideFalling.Add(entry.Key, entry.Value);
                }
                direction = PicoScope4000a.ThresholdDirection.Rising;

                SetUpChannels(handle, channel, direction, samples);

                result = GetValues(handle, channel, samples);

                foreach (var entry in result)
                {
                    highSideRising.Add(entry.Key, entry.Value);
                }

                DisableChannels(handle, channel);
            }

            ResetUserFlags(Fkt748_GateDriver.UserFlags);

            return (highSideFalling, highSideRising, timeIntervalNs);
        }

        private static (Dictionary<PicoScope4000a.Channel, List<double>> lowSideFalling, Dictionary<PicoScope4000a.Channel, List<double>> lowSideRising, int timeIntervalNs) MeasureLowSide(short handle, int samples)
        {
            SetSingleUserFlag(RelayFunction.LowSideDrivers, Fkt748_GateDriver.UserFlags);

            var channels = Connections.Select(c => c.ScopeChannel).ToList();

            var direction = PicoScope4000a.ThresholdDirection.Falling;
            SetUpChannels(handle, channels, direction, samples);
            var lowSideFalling = GetValues(handle, channels, samples);

            direction = PicoScope4000a.ThresholdDirection.Rising;
            var timeIntervalNs = SetUpChannels(handle, channels, direction, samples);
            var lowSideRising = GetValues(handle, channels, samples);

            ResetUserFlags(Fkt748_GateDriver.UserFlags);

            return (lowSideFalling, lowSideRising, timeIntervalNs);
        }

        private static void InitEnvelope()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            using (var reader = new StreamReader(@"FKT-Tests\FKT-7.4.08-Envelope.csv"))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    Envelope = csv.GetRecords<Envelope>().ToList();
                }
            }
        }

        private static bool EvaluateCurve(PicoScope4000a.ThresholdDirection direction, List<double> measured, int timeIntervalNs, bool printPlot, string name)
        {
            bool testResult = true;

            if (direction == PicoScope4000a.ThresholdDirection.Falling)
            {
                testResult = EvaluateFalling(measured, timeIntervalNs, printPlot, name);
            }
            else if (direction == PicoScope4000a.ThresholdDirection.Rising)
            {
                testResult = EvaluateRising(measured, timeIntervalNs, printPlot, name);
            }
            else
            {
                throw new ArgumentException("Only PicoScope4000a.ThresholdDirection.Falling and PicoScope4000a.ThresholdDirection.Rising can be used as the direction parameter.");
            }

            return testResult;
        }

        private static bool EvaluateFalling(List<double> measured, int timeIntervalNs, bool printPlot, string name)
        {

            int i = 0;
            bool highDetected = false;
            for (i = 0; i < measured.Count - 1; i++)
            {
                if ((measured[i] >= Envelope.First().FallLow) && (measured[i] <= Envelope.First().FallHigh))
                {
                    highDetected = true;
                }

                if (highDetected && ((measured[i] - measured[i + 1]) > 1))
                {
                    break;
                }
            }

            var lowLimits = Envelope.Select(e => e.FallLow).ToList();
            var highLimits = Envelope.Select(e => e.FallHigh).ToList();

            if ((measured.Count - i) < lowLimits.Count)
            {
                i = 0;
            }

            measured = measured.GetRange(i, lowLimits.Count);


            var testResult = true;
            for (i = 0; i < measured.Count; i++)
            {
                if ((measured[i] < lowLimits[i]) || (measured[i] > highLimits[i]))
                {
                    testResult = false;
                    break;
                }
            }

            if (printPlot)
            {
                var title = $"FKT - Gate Driver {name} (Falling Edge)";
                var fileName = $"\\FKT - Gate Driver {name} Falling Edge.png";
                var path = @"C:\Users\Administrator\Desktop" + fileName;

                Plot(timeIntervalNs, measured, lowLimits, highLimits, title, path);
            }

            return testResult;
        }

        private static bool EvaluateRising(List<double> measured, int timeIntervalNs, bool printPlot, string name)
        {

            int i = 0;
            bool lowDetected = false;
            for (i = 0; i < measured.Count - 1; i++)
            {
                if ((measured[i] <= Envelope.First().RiseHigh) && (measured[i] >= Envelope.First().RiseLow))
                {
                    lowDetected = true;
                }

                if (lowDetected && ((measured[i + 1] - measured[i]) > 1))
                {
                    break;
                }
            }

            var lowLimits = Envelope.Select(e => e.RiseLow).ToList();
            var highLimits = Envelope.Select(e => e.RiseHigh).ToList();

            if ((measured.Count - i) < lowLimits.Count)
            {
                i = 0;
            }

            measured = measured.GetRange(i, lowLimits.Count);

            var testResult = true;
            for (i = 0; i < measured.Count; i++)
            {
                if ((measured[i] < lowLimits[i]) || (measured[i] > highLimits[i]))
                {
                    testResult = false;
                    break;
                }
            }

            if (printPlot)
            {
                var title = $"FKT - Gate Driver {name} (Rising Edge)";
                var fileName = $"\\FKT - Gate Driver {name} Rising Edge.png";
                var path = @"C:\Users\Administrator\Desktop" + fileName;

                Plot(timeIntervalNs, measured, lowLimits, highLimits, title, path);
            }

            return testResult;
        }

        private static void Plot(int timeIntervalNs, List<double> measured, List<double> lowLimits, List<double> highLimits, string title, string path)
        {
            var plt = new Plot();
            var time = Enumerable.Range(0, lowLimits.Count).Select(e => (double)e * timeIntervalNs).ToList();

            plt.AddScatter(time.ToArray(), measured.ToArray(), label: "Signal Measured", markerShape: MarkerShape.none);
            plt.AddScatter(time.ToArray(), lowLimits.ToArray(), label: "Low Limits", markerShape: MarkerShape.none);
            plt.AddScatter(time.ToArray(), highLimits.ToArray(), label: "High Limits", markerShape: MarkerShape.none);

            plt.Title(title);
            plt.XLabel("Time [ns]");
            plt.YLabel("Voltage [V]");

            plt.SaveFig(path);
        }

        private static void StopScope(short handle)
        {
            var ret = PicoScope4000a.Stop(handle);
            CheckPicoStatus(ret);

            ret = PicoScope4000a.CloseUnit(handle);
            CheckPicoStatus(ret);
        }

        private static Dictionary<PicoScope4000a.Channel, List<double>> GetValues(short handle, List<PicoScope4000a.Channel> channels, int samples)
        {
            int timeIndisposedMs = 0;

            var ptr = new IntPtr();

            var isBlockReady = false;

            PicoScope4000a.ps4000aBlockReady callBack = (hndl, status, pVoid) =>
            {
                if (hndl == handle)
                {
                    isBlockReady = true;
                }
            };

            var ret = PicoScope4000a.RunBlock(handle, (int)(0.2 * samples), (int)(0.8 * samples), 1, out timeIndisposedMs, 0, callBack, ptr);
            CheckPicoStatus(ret);

            var buffers = new Dictionary<PicoScope4000a.Channel, short[]>();

            foreach (var channel in channels)
            {
                var buffer = new short[samples];
                ret = PicoScope4000a.SetDataBuffer(handle, channel, buffer, samples, 0, PicoScope4000a.DownSamplingMode.None);
                CheckPicoStatus(ret);
                buffers.Add(channel, buffer);
            }


            while (isBlockReady == false) { };

            uint noOfSamples = (uint)samples;
            short overflow = 0;
            ret = PicoScope4000a.GetValues(handle, 0, ref noOfSamples, 1, PicoScope4000a.DownSamplingMode.None, 0, out overflow);
            CheckPicoStatus(ret);

            var voltages = new Dictionary<PicoScope4000a.Channel, List<double>>();
            var scaleFactor = ScopeRange * ProbeMultiplier / Int16.MaxValue;

            foreach (var channel in channels)
            {
                var voltage = buffers[channel].Select(e => (double)e * scaleFactor).ToList();
                voltages.Add(channel, voltage);
            }

            return voltages;
        }

        private static int SetUpChannel(short handle, PicoScope4000a.Channel channel, PicoScope4000a.ThresholdDirection direction, int samples)
        {
            var ret = PicoScope4000a.SetChannel(handle, channel, 1, PicoScope4000a.Coupling.DC, PicoScopeRange, 0);

            CheckPicoStatus(ret);

            int timeIntervalNs = 0;
            ret = PicoScope4000a.GetTimebase(handle, 1, samples, out timeIntervalNs, out _, 0);

            CheckPicoStatus(ret);

            var triggerCondition = new PicoScope4000a.TriggerConditions() { Condition = PicoScope4000a.TriggerState.True, Source = channel };
            var triggerConditions = new PicoScope4000a.TriggerConditions[] { triggerCondition };
            ret = PicoScope4000a.SetTriggerChannelConditions(handle, triggerConditions, 1, PicoScope4000a.ConditionsInfo.Clear | PicoScope4000a.ConditionsInfo.Add);

            CheckPicoStatus(ret);

            var triggerDirection = new PicoScope4000a.TriggerDirections() { Direction = direction, Source = channel };
            var triggerDirections = new PicoScope4000a.TriggerDirections[] { triggerDirection };
            ret = PicoScope4000a.SetTriggerChannelDirections(handle, triggerDirections, 1);

            CheckPicoStatus(ret);

            var triggerChannelProperty = new PicoScope4000a.TriggerChannelProperties()
            {
                ThresholdMajor = (short)(TriggerLevel * Int16.MaxValue),
                ThresholdMinor = (short)(TriggerLevel * Int16.MaxValue),
                ThresholdMode = PicoScope4000a.ThresholdMode.Level,
                Channel = channel,
                HysteresisMajor = (ushort)(TriggerHyteresisLevel * Int16.MaxValue),
                HysteresisMinor = (ushort)(TriggerHyteresisLevel * Int16.MaxValue),
            };

            var triggerChannelProperties = new PicoScope4000a.TriggerChannelProperties[] { triggerChannelProperty };
            ret = PicoScope4000a.SetTriggerChannelProperties(handle, triggerChannelProperties, 1, 0, AutoTriggerTimeout.Milliseconds);
            CheckPicoStatus(ret);

            ret = PicoScope4000a.SetTriggerDelay(handle, 0);
            CheckPicoStatus(ret);

            return timeIntervalNs;
        }

        private static int SetUpChannels(short handle, List<PicoScope4000a.Channel> channels, PicoScope4000a.ThresholdDirection direction, int samples)
        {
            int ret = 0;

            foreach (var channel in channels)
            {
                ret = SetUpChannel(handle, channel, direction, samples);
            }

            return ret;
        }

        private static void DisableChannel(short handle, PicoScope4000a.Channel channel)
        {
            var ret = PicoScope4000a.SetChannel(handle, channel, 0, PicoScope4000a.Coupling.DC, PicoScope4000a.Range.Range_2V, 0);

            CheckPicoStatus(ret);
        }

        private static void DisableChannels(short handle, List<PicoScope4000a.Channel> channels)
        {
            foreach (var channel in channels)
            {
                DisableChannel(handle, channel);
            }
        }

        private static short OpenScope(string picoScopeSN)
        {
            short handle;
            StringBuilder serial = new StringBuilder(picoScopeSN);
            var ret = PicoScope4000a.OpenUnit(out handle, serial);

            CheckPicoStatus(ret);

            return handle;
        }

        private static void CheckPicoStatus(PicoStatus status)
        {
            if (status != PicoStatus.PICO_OK)
            {
                throw new Exception($"PicoScope4000a.OpenUnit() returned: {status}");
            }
        }
    }

    internal class Envelope
    {
        [Name("Rise High")]
        public double RiseHigh { get; set; }
        [Name("Rise Low")]
        public double RiseLow { get; set; }
        [Name("Fall High")]
        public double FallHigh { get; set; }
        [Name("Fall Low")]
        public double FallLow { get; set; }
    }

    internal class ConnectionGateDriverToScope
    {
        public int GateDriverNumber { get; set; }
        public PicoScope4000a.Channel ScopeChannel { get; set; }
    }
}
