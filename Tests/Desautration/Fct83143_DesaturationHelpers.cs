using PeakCanXcp;
using Program;
using Spea;
using Spea.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

internal static class Fct83143_DesaturationHelpers
{
    public static void PhaseUTop(Xcp xcp, A2lParser a2l, Func<string, TestItem> GetTest)
    {
        Thread.Sleep(50);
        xcp.Connect();
        Thread.Sleep(50);

        LeoF.PpsuOn(PPS.PPS4, 15.0, 1.0);
        Thread.Sleep(200);


        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP28x4503TU").Number }, Abus.ABUS2);
        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP19x4503TU").Number }, Abus.ABUS3);


        Thread.Sleep(200);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out double voltage, 0.05, DvmVRange.R100V);

        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP28x4503TU").Number }, Abus.ABUS2);
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP19x4503TU").Number }, Abus.ABUS3);


        LogController.Print("Pahse U 15V: " + voltage);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x02 });
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs"], new List<byte> { 0x01 });

        Thread.Sleep(200);

        var test = GetTest("FCT143003");
        TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_rPhaseFeedback_SH4_PinLvl"]);

        test = GetTest("FCT143004");
        TestLibrary.Voltage(test);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs"], new List<byte> { 0x00 });

        Thread.Sleep(100);

        LeoF.PpsuSourceSet(PPS.PPS4, 8.0, 1.0);

        Thread.Sleep(1000);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP28x4503TU").Number }, Abus.ABUS2);
        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP19x4503TU").Number }, Abus.ABUS3);
       
        Thread.Sleep(200);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out voltage, 0.05);

        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP28x4503TU").Number }, Abus.ABUS2);
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP19x4503TU").Number }, Abus.ABUS3);
     

        LogController.Print("Pahse U 8V: " + voltage);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs"], new List<byte> { 0x01 });

        Thread.Sleep(200);

        test = GetTest("FCT143007");
        TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_rPhaseFeedback_SH4_PinLvl"]);

        test = GetTest("FCT143008");
        TestLibrary.Voltage(test);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs"], new List<byte> { 0x00 });
        Thread.Sleep(200);

        LeoF.PpsuOff(PPS.PPS4);

    }

    public static void PhaseVTop(Xcp xcp, A2lParser a2l, Func<string, TestItem> GetTest)
    {
        Thread.Sleep(50);
        xcp.Connect();
        Thread.Sleep(50);

        LeoF.PpsuOn(PPS.PPS4, 15.0, 1.0);
        Thread.Sleep(200);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP28x4503TV").Number }, Abus.ABUS2);
        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP19x4503TV").Number }, Abus.ABUS3);

        Thread.Sleep(200);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out double voltage, 0.05, DvmVRange.R100V);

        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP28x4503TV").Number }, Abus.ABUS2);
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP19x4503TV").Number }, Abus.ABUS3);

        LogController.Print("Pahse V 15V: " + voltage);


        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x02 });
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs"], new List<byte> { 0x01 });

        Thread.Sleep(200);

        var test = GetTest("FCT143015");
        TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_rPhaseFeedback_SH5_PinLvl"]);

        test = GetTest("FCT143016");
        TestLibrary.Voltage(test);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs"], new List<byte> { 0x00 });

        Thread.Sleep(100);

        LeoF.PpsuSourceSet(PPS.PPS4, 8.0, 1.0);

        Thread.Sleep(1000);


        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP28x4503TV").Number }, Abus.ABUS2);
        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP19x4503TV").Number }, Abus.ABUS3);

        Thread.Sleep(200);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out voltage, 0.05);

        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP28x4503TV").Number }, Abus.ABUS2);
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP19x4503TV").Number }, Abus.ABUS3);

        LogController.Print("Pahse V 8V: " + voltage);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs"], new List<byte> { 0x01 });

        Thread.Sleep(200);

        test = GetTest("FCT143019");
        TestLibrary.Xcp(test, xcp, a2l.Measurements["HwTest_rPhaseFeedback_SH5_PinLvl"]);

        test = GetTest("FCT143020");
        TestLibrary.Voltage(test);

        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs"], new List<byte> { 0x00 });
        Thread.Sleep(200);

        LeoF.PpsuOff(PPS.PPS4);

    }
}