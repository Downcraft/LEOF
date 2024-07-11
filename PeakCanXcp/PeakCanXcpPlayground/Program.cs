using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeakCanXcp;
using Test;

namespace PeakCanXcpPlayground
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var path = @"C:\Users\Administrator\Desktop\a.a2l";
            var a2l = new A2lParser(path, A2lParser.FileType.BIN, true);

            var xcp = new Xcp(
                Xcp.NominalBitrate._500k,
                Xcp.DataBitrate._2M,
                a2l.CanParameters.CanIdMaster,
                a2l.CanParameters.CanIdSlave,
                a2l.CanParameters.CanIdBroadcast,
                12);

            List<byte> Bytes;
            var exception = false;
            var exceptionMsg = string.Empty;

            while (true)
            {
                try
                {
                    Bytes = xcp.Connect();
                    break;
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                }
            }

            //Fkt741_VoltageAndCurrentInSleepState.Test(xcp, a2l, 1);
            //Fkt7412_MotorTemperature.Test(xcp, a2l, 1);
            Menu(a2l, xcp, ref Bytes, ref exception, ref exceptionMsg);

            //Fkt7416_RpsReceiver.Test(xcp, a2l, site: 1);

            // await Fkt748_GateDriver.TestAsync(xcp, a2l, printPlot: true);

            Console.Clear();

            try
            {
                Bytes = xcp.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        private static void Menu(A2lParser a2l, Xcp xcp, ref List<byte> Bytes, ref bool exception, ref string exceptionMsg)
        {
            Console.CursorVisible = false;

            Console.WriteLine(string.Format("Resource Mask: 0x{0:X2}", Bytes[1]));
            Console.WriteLine(string.Format("COMM_MODE_BASIC: 0x{0:X2}", Bytes[2]));
            Console.WriteLine(string.Format("MAX_CTO: {0:D2}", Bytes[3]));
            Console.WriteLine(string.Format("MAX_DTO: {0:D2}", ((Bytes[2] & 1) == 1) ? Bytes[5] : Bytes[4]));
            Console.WriteLine(string.Format("XCP Protocol Layer: Version: {0:D2}", Bytes[6]));
            Console.WriteLine(string.Format("XCP transport Layer: Version: {0:D2}", Bytes[7]));

            Console.WriteLine("\nPress ESC to stop\n");

            do
            {
                (var left, var top) = (Console.CursorLeft, Console.CursorTop);

                try
                {
                    while (!Console.KeyAvailable)
                    {
                        Console.CursorVisible = false;
                        Console.WriteLine("Measurements:\n");

                        PrintConvertedValue(xcp, a2l.Measurements["IoEcu_rLvBnet.SupVoltAdcVolt"]);
                        PrintConvertedValue(xcp, a2l.Measurements["IoEcu_LvBnetWrk.SupVolt"]);
                        PrintConvertedValue(xcp, a2l.Measurements["IoEcu_LvBnetWrk.WkupVolt"]);
                        PrintConvertedValue(xcp, a2l.Measurements["IoEcu_EcuVolt2.IntrnlSupVolt1.Val"]);
                        PrintConvertedValue(xcp, a2l.Measurements["IoEcu_rEcuVolt2AdcVolt.IntrnlSup2"]);
                        PrintConvertedValue(xcp, a2l.Measurements["IoEcu_PcbTemp.Val"]);
                        PrintConvertedValue(xcp, a2l.Measurements["IoEcu_McuTemp"]);

                        Console.WriteLine("\nSleep Control:\n");

                        PrintConvertedValue(xcp, a2l.Characteristics["HwTest_cCanTrcvOvrrdEn"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["HwTest_cCanTrcvOpMode"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["HwTest_cCanTrcvSetOpMode"]);

                        Console.WriteLine("\nCPLD Safe State:\n");

                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.OutOvrrd.OvrrdEn"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.EnOvrrdEn"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.EnOvrrd"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.SafeStateSrcEnOvrrd.EnBf"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.SafeStateSrcEnOvrrd.OvrrdEn"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.OutOvrrd.CommErrRset"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.OutOvrrd.SafeStateSrcRset"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEcu_cPld.OutOvrrd.AscRset"]);
                        PrintConvertedValue(xcp, a2l.Measurements["IoEm_rHwSafeStateStsPinLvl"]);

                        Console.WriteLine("\nGate Driver:\n");

                        PrintConvertedValue(xcp, a2l.Characteristics["HwTest_cPwrStgPar.Mode"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEn"]);
                        PrintConvertedValue(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEnFrc"]);
                        for (var index = 0; index < 6; index++)
                        {
                            PrintConvertedValue(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], (int)index);
                        }

                        Console.WriteLine("\nTemperature:\n");
                        PrintConvertedValue(xcp, a2l.Measurements["IoMotTemp_WndgTemp1.Val"]);

                        Console.WriteLine("");

                        if (exception)
                        {
                            ClearCurrentConsoleLine();
                            Console.WriteLine("[Exception]");
                            ClearCurrentConsoleLine();
                            Console.WriteLine(exceptionMsg);

                        }
                        else
                        {
                            ClearCurrentConsoleLine();
                            Console.WriteLine("");
                            ClearCurrentConsoleLine();
                        }

                        Console.SetCursorPosition(left, top);
                    }

                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else if (key == ConsoleKey.S)
                    {
                        xcp.Download(a2l.Characteristics["HwTest_cCanTrcvOvrrdEn"], new List<byte> { 0x01 }, 3);
                        xcp.Download(a2l.Characteristics["HwTest_cCanTrcvOpMode"], new List<byte> { 0x01 }, 3);
                        xcp.Download(a2l.Characteristics["HwTest_cCanTrcvSetOpMode"], new List<byte> { 0x01 }, 3);
                    }
                    else if (key == ConsoleKey.Spacebar)
                    {

                        xcp.Download(a2l.Characteristics["IoEcu_cPld.OutOvrrd.OvrrdEn"], new List<byte> { 0x01 }, 100);
                        xcp.Download(a2l.Characteristics["IoEcu_cPld.EnOvrrdEn"], new List<byte> { 0x01 }, 100);
                        xcp.Download(a2l.Characteristics["IoEcu_cPld.EnOvrrd"], new List<byte> { 0x01 }, 100);
                        xcp.Download(a2l.Characteristics["IoEcu_cPld.SafeStateSrcEnOvrrd.EnBf"], new List<byte> { 0x00, 0x00, 0x00, 0x00 }, 100);

                        while (xcp.GetConvertedValue(a2l.Measurements["IoEm_rHwSafeStateStsPinLvl"]).Value != 1.0)
                        {
                            xcp.Download(a2l.Characteristics["IoEcu_cPld.SafeStateSrcEnOvrrd.OvrrdEn"], new List<byte> { 0x01 });
                            xcp.Download(a2l.Characteristics["IoEcu_cPld.OutOvrrd.SafeStateSrcRset"], new List<byte> { 0x01 });
                            xcp.Download(a2l.Characteristics["IoEcu_cPld.OutOvrrd.AscRset"], new List<byte> { 0x01 });
                        }

                        xcp.Download(a2l.Characteristics["IoEcu_cPld.OutOvrrd.AscRset"], new List<byte> { 0x00 }, 100);
                        xcp.Download(a2l.Characteristics["IoEcu_cPld.SafeStateSrcEnOvrrd.OvrrdEn"], new List<byte> { 0x00 }, 100);
                        xcp.Download(a2l.Characteristics["IoEcu_cPld.OutOvrrd.SafeStateSrcRset"], new List<byte> { 0x00 }, 100);

                    }
                    else if (key == ConsoleKey.R)
                    {
                        exception = false;
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x00 }, 3);
                        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEn"], new List<byte> { 0x01 }, 3);
                        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEnFrc"], new List<byte> { 0x01 }, 3);
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {

                        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEnFrc"], new List<byte> { 0x00 }, 3);
                        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEn"], new List<byte> { 0x00 }, 3);
                        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x00 }, 3);
                    }
                    else if (key == ConsoleKey.Oem5)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 0.0f);
                    }
                    else if (key == ConsoleKey.D1)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 10.0f);
                    }
                    else if (key == ConsoleKey.D2)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 20.0f);
                    }
                    else if (key == ConsoleKey.D3)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 30.0f);
                    }
                    else if (key == ConsoleKey.D4)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 40.0f);
                    }
                    else if (key == ConsoleKey.D5)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 50.0f);
                    }
                    else if (key == ConsoleKey.D6)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 60.0f);
                    }
                    else if (key == ConsoleKey.D7)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 70.0f);
                    }
                    else if (key == ConsoleKey.D8)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 80.0f);
                    }
                    else if (key == ConsoleKey.D9)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 90.0f);
                    }
                    else if (key == ConsoleKey.D0)
                    {
                        SetPwm(xcp, a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], 100.0f);
                    }
                    else if (key == ConsoleKey.F)
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        Fkt748_GateDriver.TestAsync(xcp, a2l, printPlot: true).GetAwaiter().GetResult();
                        stopwatch.Stop();
                    }
                }
                catch
                {
                    exception = true;
                    try
                    {
                        Console.SetCursorPosition(left, top);
                        Bytes = xcp.Connect();
                    }
                    catch (Exception e)
                    {
                        exceptionMsg = e.Message;
                    }
                }
            } while (true);
        }

        public static void SetPwm(Xcp xcp, XcpValue value, float dutyCycle)
        {
            var ByteList = BitConverter.GetBytes(dutyCycle).ToList();
            for (int index = 0; index < 6; index++)
            {
                xcp.Download((uint)(value.EcuAddress + (4 * index)), ByteList, 3);
            }
        }

        public static void PrintConvertedValue(Xcp xcp, XcpValue value, int arrayIndex = -1)
        {
            var converted = xcp.GetConvertedValue(value, arrayIndex);
            ClearCurrentConsoleLine();
            if (value.LongIdentifier != string.Empty)
            {
                Console.WriteLine($"{value.LongIdentifier}: {converted.Value,3:f2} {converted.Unit}");
            }
            else
            {
                Console.WriteLine($"{value.Name}: {converted.Value,3:f2} {converted.Unit}");
            }
            
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
