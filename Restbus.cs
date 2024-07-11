using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeakCanUds;
using Peak.Can.Uds;
using static PeakCanUds.Uds;
using Peak.Can.IsoTp;
using System.Threading;

namespace Prorgam
{
    internal class Restbus
    {
        public static bool RestBusStop = false;

        public static void RunRestbus()
        {
            var uds = new Uds((cantp_handle)cantp_handle.PCANTP_HANDLE_USBBUS2);

            uds.InitializeCanFd(TestParameters.XcpRestBus.Bitrate, msgConf);

            //uds.Write(new List<Byte> { 0x10, 0x03 });

            Thread.Sleep(100);

            while (!RestBusStop)
            {
                //uds.Write(new List<Byte> { 0x10, 0x03 });
                //Thread.Sleep(100);

                //uds.Write(new List<Byte> { 0x1B, 0xFE, 0x13, 0x01 });

                uds.Write(new List<Byte> { 0x3E});

                Thread.Sleep(500);

            }

            Thread.Sleep(100);

            uds.Uninitialize();

            Thread.Sleep(1000);

        }



        public static MsgConfiguration msgConf = new MsgConfiguration
          (
            canIdPhysycalRequest: 0x18E06288,
            canIdPhysycalResponse: 0x18E06280,
            protocol: uds_msgprotocol.PUDS_MSGPROTOCOL_ISO_15765_2_29B_NORMAL,
            canMsgType: cantp_can_msgtype.PCANTP_CAN_MSGTYPE_FD | cantp_can_msgtype.PCANTP_CAN_MSGTYPE_BRS,
            udsMsgType: uds_msgtype.PUDS_MSGTYPE_USDT,
            targetType: cantp_isotp_addressing.PCANTP_ISOTP_ADDRESSING_PHYSICAL,
            //extendedSourceAddress: 0x86,
            //extendedTargetAddress: 0x8E,
            canTxDlc: 15
        );
    }
}
