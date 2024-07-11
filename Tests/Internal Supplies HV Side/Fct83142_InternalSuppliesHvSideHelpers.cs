using PeakCanXcp;
using Program;
using Spea;
using Spea.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

internal static class Fct83142_InternalSuppliesHvSideHelpers
{
    public static void AllPhasesSetToLow(Xcp xcp, A2lParser a2l, Func<string, TestItem> GetTest)
    {
        Thread.Sleep(50);
        xcp.Connect();
        Thread.Sleep(50);


        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x02 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._2_.Hs"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Ls"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Ls"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._2_.Ls"], new List<byte> { 0x00 });

        TestLibrary.Voltage(GetTest("FCT142008"));
        TestLibrary.Voltage(GetTest("FCT142009"));

        TestLibrary.Voltage(GetTest("FCT142010"));
        TestLibrary.Voltage(GetTest("FCT142011"));

        TestLibrary.Voltage(GetTest("FCT142012"));
        TestLibrary.Voltage(GetTest("FCT142013"));

        TestLibrary.Voltage(GetTest("FCT142014"));
        TestLibrary.Voltage(GetTest("FCT142015"));

        TestLibrary.Voltage(GetTest("FCT142016"));
        TestLibrary.Voltage(GetTest("FCT142017"));

        TestLibrary.Voltage(GetTest("FCT142018"));
        TestLibrary.Voltage(GetTest("FCT142019"));

        TestLibrary.Voltage(GetTest("FCT142020"), range: DvmVRange.R100V);
        TestLibrary.Voltage(GetTest("FCT142021"), range: DvmVRange.R100V);
        TestLibrary.Voltage(GetTest("FCT142022"), range: DvmVRange.R100V);
        TestLibrary.Voltage(GetTest("FCT142023"), range: DvmVRange.R100V);
        TestLibrary.Voltage(GetTest("FCT142024"), range: DvmVRange.R100V);
        TestLibrary.Voltage(GetTest("FCT142025"), range: DvmVRange.R100V);
    }

    public static void HighSideSetToHigh(Xcp xcp, A2lParser a2l, Func<string, TestItem> GetTest)
    {
        Thread.Sleep(50);
        xcp.Connect();
        Thread.Sleep(50);

        //xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x02 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs"], new List<byte> { 0x01 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs"], new List<byte> { 0x01 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._2_.Hs"], new List<byte> { 0x01 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Ls"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Ls"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._2_.Ls"], new List<byte> { 0x00 });

        Thread.Sleep(100);

        TestLibrary.Voltage(GetTest("FCT142036"));

        TestLibrary.Voltage(GetTest("FCT142032"));
        
        TestLibrary.Voltage(GetTest("FCT142034"));
      
    }

    public static void LowSideSetToHigh(Xcp xcp, A2lParser a2l, Func<string, TestItem> GetTest)
    {
        Thread.Sleep(50);
        xcp.Connect();
        Thread.Sleep(50);

        //xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x02 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Hs"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Hs"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._2_.Hs"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Ls"], new List<byte> { 0x01 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Ls"], new List<byte> { 0x01 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._2_.Ls"], new List<byte> { 0x01 });



        Thread.Sleep(100);

        TestLibrary.Voltage(GetTest("FCT142044"));
        TestLibrary.Voltage(GetTest("FCT142046"));
        TestLibrary.Voltage(GetTest("FCT142048"));




        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._0_.Ls"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._1_.Ls"], new List<byte> { 0x00 });
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.PhaseOvrrd._2_.Ls"], new List<byte> { 0x00 });

        Thread.Sleep(100);

    }

    public static void PwmSetTo0(Xcp xcp, A2lParser a2l,  Func<string, TestItem> GetTest)
    {
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x00 });
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEn"], new List<byte> { 0x01 });
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEnFrc"], new List<byte> { 0x01 });
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], BitConverter.GetBytes(0f).ToList(), arrayIndex: 0);
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], BitConverter.GetBytes(0f).ToList(), arrayIndex: 1);
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], BitConverter.GetBytes(0f).ToList(), arrayIndex: 2);
        Thread.Sleep(500);

       MeasureDigPoints("0");




        //TestLibrary.Voltage(GetTest("FCT142064"));
        TestLibrary.Voltage(GetTest("FCT142065"));

        //TestLibrary.Voltage(GetTest("FCT142066"));
        TestLibrary.Voltage(GetTest("FCT142067"));

        //TestLibrary.Voltage(GetTest("FCT142068"));
        TestLibrary.Voltage(GetTest("FCT142069"));

        //TestLibrary.Voltage(GetTest("FCT142070"));
        TestLibrary.Voltage(GetTest("FCT142071"));

        //TestLibrary.Voltage(GetTest("FCT142072"));
        TestLibrary.Voltage(GetTest("FCT142073"));

        //TestLibrary.Voltage(GetTest("FCT142074"));
        TestLibrary.Voltage(GetTest("FCT142075"));
    }

    public static void PwmSetTo100(Xcp xcp, A2lParser a2l, Func<string, TestItem> GetTest)
    {
        xcp.Download(a2l.Characteristics["HwTest_cPwrStgPar.Mode"], new List<byte> { 0x00 });
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEn"], new List<byte> { 0x01 });
        Thread.Sleep(100);

        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrdEnFrc"], new List<byte> { 0x01 });
        Thread.Sleep(100);

        Thread.Sleep(100);
        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], BitConverter.GetBytes(100f).ToList(), arrayIndex: 0);
        Thread.Sleep(100);
        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], BitConverter.GetBytes(100f).ToList(), arrayIndex: 1);
        Thread.Sleep(100);
        xcp.Download(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], BitConverter.GetBytes(100f).ToList(), arrayIndex: 2);
        Thread.Sleep(100);

        Thread.Sleep(500);

        MeasureDigPoints("100");

      

        //TestLibrary.Voltage(GetTest("FCT142053"));
        TestLibrary.Voltage(GetTest("FCT142054"));

        //TestLibrary.Voltage(GetTest("FCT142055"));
        TestLibrary.Voltage(GetTest("FCT142056"));

        //TestLibrary.Voltage(GetTest("FCT142057"));
        TestLibrary.Voltage(GetTest("FCT142058"));

        //TestLibrary.Voltage(GetTest("FCT142059"));
        TestLibrary.Voltage(GetTest("FCT142060"));

        //TestLibrary.Voltage(GetTest("FCT142061"));
        TestLibrary.Voltage(GetTest("FCT142062"));

        //TestLibrary.Voltage(GetTest("FCT142051"));
        TestLibrary.Voltage(GetTest("FCT142052"));
    }

    private static void MeasureDigPoints(string percentage)
    {
        LogController.Print($"");
        LogController.Print($"PWM Set To {percentage}%");

        LeoF.TpConnectAbus(new List<int> { new TestPoint("M1x9900").Number }, Abus.ABUS3);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP10x800").Number }, Abus.ABUS2);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out double measured, 0.05);
        LogController.Print($"Measured voltage on PwmTopU_Dig: {measured}V");
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP10x800").Number }, Abus.ABUS2);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP11x800").Number }, Abus.ABUS2);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out measured, 0.05);
        LogController.Print($"Measured voltage on PwmTopV_Dig: {measured}V");
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP11x800").Number }, Abus.ABUS2);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP12x800").Number }, Abus.ABUS2);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out measured, 0.05);
        LogController.Print($"Measured voltage on PwmTopW_Dig: {measured}V");
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP12x800").Number }, Abus.ABUS2);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP7x800").Number }, Abus.ABUS2);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out measured, 0.05);
        LogController.Print($"Measured voltage on PwmBotU_Dig: {measured}V");
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP7x800").Number }, Abus.ABUS2);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP8x800").Number }, Abus.ABUS2);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out measured, 0.05);
        LogController.Print($"Measured voltage on PwmBotV_Dig: {measured}V");
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP8x800").Number }, Abus.ABUS2);

        LeoF.TpConnectAbus(new List<int> { new TestPoint("TP9x800").Number }, Abus.ABUS2);
        Dvm.AutoMeasureDc(DvmHotConnectionPoint.ABUS2, DvmColdConnectionPoint.ABUS3, out measured, 0.05);
        LogController.Print($"Measured voltage on PwmBotW_Dig: {measured}V");
        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("TP9x800").Number }, Abus.ABUS2);

        LeoF.TpDisconnectAbus(new List<int> { new TestPoint("M1x9900").Number }, Abus.ABUS3);
        LogController.Print($"");

    }
}