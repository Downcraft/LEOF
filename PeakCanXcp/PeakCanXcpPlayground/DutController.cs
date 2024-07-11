using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeakCanXcp;

namespace Test
{
    class DutController
    {
        public static void DisableSafeState(Xcp xcp, A2lParser a2l)
        {
            if (xcp.GetConvertedValue(a2l.Measurements["IoEm_rHwSafeStateStsPinLvl"]).Value == 1.0)
            {
                return;
            }

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

        public static void SetPowerStageMode(Xcp xcp, A2lParser a2l, byte value, int retry = 5)
        {
            DutController.SetFlag(xcp, a2l, "HwTest_cPwrStgPar.Mode", value, retry);
        }

        public static void Set_48V_VoltageDianosticOverride(Xcp xcp, A2lParser a2l, byte value, int retry = 5)
        {
            DutController.SetFlag(xcp, a2l, "IoEcu_cHwDiag.DcLinkVoltDiagOvrrdEn", value, retry);
        }

        public static void Set_48V_OvervoltageFault(Xcp xcp, A2lParser a2l, byte value, int retry = 5)
        {
            DutController.SetFlag(xcp, a2l, "IoEcu_cHwDiag.DcLinkVoltHighDiagOvrrd", value, retry);
        }

        public static void Set_48V_UndervoltageFault(Xcp xcp, A2lParser a2l, byte value, int retry = 5)
        {
            DutController.SetFlag(xcp, a2l, "IoEcu_cHwDiag.DcLinkVoltLowDiagOvrrd", value, retry);
        }

        public static void SetCpldAscOverrideEnable(Xcp xcp, A2lParser a2l, byte value, int retry = 5)
        {
            DutController.SetFlag(xcp, a2l, "IoHwAb_cSafeStateSetOvrrdEn", value, retry);
        }

        public static void SetCpldAscOverride(Xcp xcp, A2lParser a2l, byte value, int retry = 5)
        {
            DutController.SetFlag(xcp, a2l, "IoHwAb_cSafeStateSetOvrrd", value, retry);
        }

        public static double SetFlag(Xcp xcp, A2lParser a2l, string xcpParameter, byte value, int retry = 5)
        {
            var xcpValue = a2l.Characteristics[xcpParameter];
            xcp.Download(xcpValue.EcuAddress, xcpValue.EcuAddressExtension, new List<byte> { value }, retry);
            var readValue = xcp.GetConvertedValue(xcpValue).Value;

            if (readValue != value)
            {
                throw new Exception($"Couldn't set {xcpParameter} to {value}");
            }

            return readValue;
        }

        public static void SetPwm(Xcp xcp, A2lParser a2l, float dutyCycle, int phase, int retry = 5)
        {
            if (phase < 1 && phase > 6)
            {
                throw new ArgumentException($"{nameof(phase)} must be between 1 and 6.");
            }

            var value = a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"];

            var ByteList = BitConverter.GetBytes(dutyCycle).ToList();

            xcp.Download(value, ByteList, retry: retry, arrayIndex: phase - 1);
        }

        public static double GetPwm(Xcp xcp, A2lParser a2l, int phase)
        {
            var convertedValue = xcp.GetConvertedValue(a2l.Characteristics["IoEm_cInvPhaseDucySpOvrrd"], arrayIndex: phase - 1);

            return convertedValue.Value;
        }

        public static void Sleep(Xcp xcp, A2lParser a2l)
        {
            DutController.SetFlag(xcp, a2l, "HwTest_cCanTrcvOvrrdEn", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cCanTrcvOpMode", 0x01);
            DutController.SetFlag(xcp, a2l, "HwTest_cCanTrcvSetOpMode", 0x01);
        }
    }
}
