namespace PeakCanUds
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Peak.Can.Basic;
    using Peak.Can.IsoTp;
    using Peak.Can.Uds;

    /// <summary>
    /// Class containing the main methodes used for the can communication using Peak-Interfaces.
    /// </summary>
    public class Uds
    {
        private static readonly Dictionary<uds_nrc, string> NrcDescriptionDictionary = new Dictionary<uds_nrc, string>
        {
            { uds_nrc.PUDS_NRC_PR, "Positive Response" },
            { uds_nrc.PUDS_NRC_GR, "General Reject" },
            { uds_nrc.PUDS_NRC_SNS, "Service Not Supported" },
            { uds_nrc.PUDS_NRC_SFNS, "Sub Function Not Supported" },
            { uds_nrc.PUDS_NRC_IMLOIF, "Incorrect Message Length Or Invalid Format" },
            { uds_nrc.PUDS_NRC_RTL, "Response Too Long" },
            { uds_nrc.PUDS_NRC_BRR, "Busy Repeat Request" },
            { uds_nrc.PUDS_NRC_CNC, "Conditions Not Correct" },
            { uds_nrc.PUDS_NRC_RSE, "Request Sequence Error" },
            { uds_nrc.PUDS_NRC_NRFSC, "No Response From Subnet Component" },
            { uds_nrc.PUDS_NRC_FPEORA, "Failure Prevents Execution Of Requested Action" },
            { uds_nrc.PUDS_NRC_ROOR, "Request Out Of Range" },
            { uds_nrc.PUDS_NRC_SAD, "Security Access Denied" },
            { uds_nrc.PUDS_NRC_AR, "Authentication Required" },
            { uds_nrc.PUDS_NRC_IK, "Invalid Key" },
            { uds_nrc.PUDS_NRC_ENOA, "Exceeded Number Of Attempts" },
            { uds_nrc.PUDS_NRC_RTDNE, "Required Time Delay Not Expired" },
            { uds_nrc.PUDS_NRC_SDTR, "Secure Data Transmission Required" },
            { uds_nrc.PUDS_NRC_SDTNA, "Secure Data Transmission Not Allowed" },
            { uds_nrc.PUDS_NRC_SDTF, "Secure Data Verification Failed" },
            { uds_nrc.PUDS_NRC_CVFITP, "Certificate Verification Failed Invalid Time Period" },
            { uds_nrc.PUDS_NRC_CVFISIG, "Certificate Verification Failed Invalid SIGnature" },
            { uds_nrc.PUDS_NRC_CVFICOT, "Certificate Verification Failed Invalid Chain of Trust" },
            { uds_nrc.PUDS_NRC_CVFIT, "Certificate Verification Failed Invalid Type" },
            { uds_nrc.PUDS_NRC_CVFIF, "Certificate Verification Failed Invalid Format" },
            { uds_nrc.PUDS_NRC_CVFIC, "Certificate Verification Failed Invalid Content" },
            { uds_nrc.PUDS_NRC_CVFISCP, "Certificate Verification Failed Invalid ScoPe" },
            { uds_nrc.PUDS_NRC_CVFICERT, "Certificate Verification Failed Invalid CERTificate(revoked)" },
            { uds_nrc.PUDS_NRC_OVF, "Ownership Verification Failed" },
            { uds_nrc.PUDS_NRC_CCF, "Challenge Calculation Failed" },
            { uds_nrc.PUDS_NRC_SARF, "Setting Access Rights Failed" },
            { uds_nrc.PUDS_NRC_SKCDF, "Session Key Creation / Derivation Failed" },
            { uds_nrc.PUDS_NRC_CDUF, "Configuration Data Usage Failed" },
            { uds_nrc.PUDS_NRC_DAF, "DeAuthentication Failed" },
            { uds_nrc.PUDS_NRC_UDNA, "Upload Download Not Accepted" },
            { uds_nrc.PUDS_NRC_TDS, "Transfer Data Suspended" },
            { uds_nrc.PUDS_NRC_GPF, "General Programming Failure" },
            { uds_nrc.PUDS_NRC_WBSC, "Wrong Block Sequence Counter" },
            { uds_nrc.PUDS_NRC_RCRRP, "Request Correctly Received – Response Pending" },
            { uds_nrc.PUDS_NRC_SFNSIAS, "Sub Function Not Supported In Active Session" },
            { uds_nrc.PUDS_NRC_SNSIAS, "Service Not Supported In Active Session" },
            { uds_nrc.PUDS_NRC_RPMTH, "RPM Too High" },
            { uds_nrc.PUDS_NRC_RPMTL, "RPM Too Low" },
            { uds_nrc.PUDS_NRC_EIR, "Engine Is Running" },
            { uds_nrc.PUDS_NRC_EINR, "Engine Is Not Running" },
            { uds_nrc.PUDS_NRC_ERTTL, "Engine Run Time Too Low" },
            { uds_nrc.PUDS_NRC_TEMPTH, "TEMPerature Too High" },
            { uds_nrc.PUDS_NRC_TEMPTL, "TEMPerature Too Low" },
            { uds_nrc.PUDS_NRC_VSTH, "Vehicle Speed Too High" },
            { uds_nrc.PUDS_NRC_VSTL, "Vehicle Speed Too Low" },
            { uds_nrc.PUDS_NRC_TPTH, "Throttle / Pedal Too High" },
            { uds_nrc.PUDS_NRC_TPTL, "Throttle / Pedal Too Low" },
            { uds_nrc.PUDS_NRC_TRNIN, "Transmission Range Not In Neutral" },
            { uds_nrc.PUDS_NRC_TRNIG, "Transmission Range Not In Gear" },
            { uds_nrc.PUDS_NRC_BSNC, "Brake Switch(es) Not Closed(brake pedal not pressed or not applied)" },
            { uds_nrc.PUDS_NRC_SLNIP, "Shifter Lever Not In Park" },
            { uds_nrc.PUDS_NRC_TCCL, "Torque Converter Clutch Locked" },
            { uds_nrc.PUDS_NRC_VTH, "Voltage Too High" },
            { uds_nrc.PUDS_NRC_VTL, "Voltage Too Low" },
            { uds_nrc.PUDS_NRC_RTNA, "Resource Temporarily Not Available" },
        };

        private bool createLogFile = true;
        private int bayNo = 0;
        private uds_msgconfig msgConfig;
        private cantp_handle handle;
        private uds_mapping requestMapping;
        private uds_mapping responseMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="Uds"/> class for a given handle.
        /// </summary>
        /// <param name="handle">Represents currently defined and supported PCANTP handle (a.k.a. channels).</param>
        /// <param name="printHandler">Methode for printing Can activity. type void. parameters: string with text, and bool for deciding if text gets printed in logWindow or reportWindow.  </param>
        public Uds(cantp_handle handle, PrintHandlerDelegate printHandler = null, int bay = 0)
        {
            if (printHandler != null)
            {
                this.PrintHandler = printHandler;
            }

            this.handle = handle;

            this.bayNo = bay;
        }

        /// <summary>
        /// Delegate for a print method.
        /// </summary>
        /// <param name="msg"> text to be printed</param>
        /// <param name="isErrorMessage"> flag for deciding if text gets printed in logWindow or reportWindow. </param>
        public delegate void PrintHandlerDelegate(string msg, bool isErrorMessage = false);

        /// <summary>
        /// Gets or sets Name for the Log File (without ".txt").
        /// </summary>
        public string LogFileName { get; set; } = $"CanLog_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";

        /// <summary>
        /// Gets or sets the directory for the log file.
        /// </summary>
        public string LogFileDirectory { get; set; } = @"c:\CanLogFiles\";

        /// <summary>
        /// Gets or sets a value indicating whether a log file containing the can bus activity is created.
        /// </summary>
        public bool LogToFileEnable { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whetherthe can bus activity gets printed on screen.
        /// </summary>
        public bool PrintOnScreenEnable { get; set; } = false;

        private PrintHandlerDelegate PrintHandler { get; } = (string message, bool isErrorMessage) => { }; // Default: Do nothing.

        /// <summary>
        /// Findes the usbChannel for a given device Id.
        /// Only useful for single channel interfaces!
        /// </summary>
        /// <param name="deviceId"> Id can be configured using PCanView software.</param>
        /// <returns>deviceUsbChannel if found, else 0. </returns>
        public static cantp_handle FindPeakDeviceUsbChannel(uint deviceId)
        {
            StringBuilder peakId = new StringBuilder(sizeof(uint));

            cantp_handle foundHandle = 0;
            try
            {
                if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS1, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS1;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS2, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS2;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS3, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS3;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS4, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS4;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS5, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS5;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS6, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS6;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS7, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS7;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS8, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS8;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS9, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS9;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS10, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS10;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS11, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS11;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS12, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS12;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS13, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS13;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS14, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS14;
                    }
                }
                else if (PCANBasic.GetValue(PCANBasic.PCAN_USBBUS15, TPCANParameter.PCAN_DEVICE_ID, peakId, 2) == 0)
                {
                    uint id = StringBuilderToId(peakId);

                    if (id == deviceId)
                    {
                        foundHandle = cantp_handle.PCANTP_HANDLE_USBBUS15;
                    }
                }

                if (foundHandle == 0)
                {
                    throw new Exception($"No Device was found for the Id: 0x{deviceId:X2}");
                }
            }
            catch (DllNotFoundException)
            {
                throw new Exception($"Unable to find the library: PCANBasic.dll !");
            }

            return foundHandle;
        }

        /// <summary>
        /// Initializes a PUDS channel based on a PCANTP handle (including CAN FD support).
        /// </summary>
        /// <param name="bitrateCanFd">The speed for the communication.</param>
        /// <returns>uds_status.</returns>
        public uds_status InitializeCanFd(string bitrateCanFd, MsgConfiguration _msgConfig)
        {
            var status = UDSApi.InitializeFD_2013(this.handle, bitrateCanFd);

            UDSApi.RemoveMappingByCanId_2013(this.handle, _msgConfig.CanIdPhysycalRequest);
            UDSApi.RemoveMappingByCanId_2013(this.handle, _msgConfig.CanIdPhysycalResponse);

            this.msgConfig.can_id = _msgConfig.CanIdPhysycalRequest;
            this.msgConfig.can_msgtype = _msgConfig.CanMsgType;
            this.msgConfig.nai.protocol = _msgConfig.Protocol;
            this.msgConfig.can_tx_dlc = _msgConfig.CanTxDlc;
            this.msgConfig.nai.source_addr = _msgConfig.SourceAddress;
            this.msgConfig.nai.target_addr = _msgConfig.TargetAddress;
            this.msgConfig.nai.target_type = _msgConfig.TargetType;

            this.requestMapping = default;
            this.requestMapping.can_id = this.msgConfig.can_id;
            this.requestMapping.can_id_flow_ctrl = _msgConfig.CanIdPhysycalResponse;
            this.requestMapping.can_msgtype = this.msgConfig.can_msgtype;
            this.requestMapping.nai.protocol = this.msgConfig.nai.protocol;
            this.requestMapping.can_tx_dlc = this.msgConfig.can_tx_dlc;
            this.requestMapping.nai.target_type = this.msgConfig.nai.target_type;
            this.requestMapping.nai.source_addr = _msgConfig.SourceAddress;
            this.requestMapping.nai.target_addr = _msgConfig.TargetAddress;

            this.responseMapping = this.requestMapping;
            this.responseMapping.can_id = this.requestMapping.can_id_flow_ctrl;
            this.responseMapping.can_id_flow_ctrl = this.requestMapping.can_id;
            this.responseMapping.nai.source_addr = this.requestMapping.nai.target_addr;
            this.responseMapping.nai.target_addr = this.requestMapping.nai.source_addr;

            status = UDSApi.AddMapping_2013(this.handle, ref this.requestMapping);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                throw new Exception($"An Error ocurred while trying to add requestMapping during the initialization! {this.handle}");
            }

            status = UDSApi.AddMapping_2013(this.handle, ref this.responseMapping);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                throw new Exception($"An Error ocurred while trying to add responsetMapping during the initialization! {this.handle}");
            }

            return status;
        }

        /// <summary>
        /// Initializes a PUDS channel based on a PCANTP handle (without CAN FD support).
        /// </summary>
        /// <param name="baudrate">The speed for the communication.</param>
        /// <param name="_msgConfig"> Struct with configuration for the messages.  Must be defined and initialized before instantiating a <see cref="Uds"/> object. </param>
        /// <returns>uds_status.</returns>
        public uds_status InitializeCan(cantp_baudrate baudrate, MsgConfiguration _msgConfig)
        {
            var status = UDSApi.Initialize_2013(this.handle, baudrate);

            if (!UDSApi.StatusIsOk_2013(status))
            {
                string errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An Error ocurred while trying to initialize the can interface {nameof(this.handle)} {this.handle}. Error: {errorTxt}", isErrorMessage: true);
                throw new Exception($"An Error ocurred while trying to initialize the can interface {this.handle} Error: {errorTxt}");
            }

            UDSApi.RemoveMappingByCanId_2013(this.handle, _msgConfig.CanIdPhysycalRequest);
            UDSApi.RemoveMappingByCanId_2013(this.handle, _msgConfig.CanIdPhysycalResponse);

            this.msgConfig.can_id = _msgConfig.CanIdPhysycalRequest;
            this.msgConfig.can_msgtype = _msgConfig.CanMsgType;
            this.msgConfig.nai.protocol = _msgConfig.Protocol;
            this.msgConfig.can_tx_dlc = _msgConfig.CanTxDlc;
            this.msgConfig.nai.source_addr = _msgConfig.SourceAddress;
            this.msgConfig.nai.target_addr = _msgConfig.TargetAddress;
            this.msgConfig.nai.target_type = _msgConfig.TargetType;

            this.requestMapping = default;
            this.requestMapping.can_id = this.msgConfig.can_id;
            this.requestMapping.can_id_flow_ctrl = _msgConfig.CanIdPhysycalResponse;
            this.requestMapping.can_msgtype = this.msgConfig.can_msgtype;
            this.requestMapping.nai.protocol = this.msgConfig.nai.protocol;
            this.requestMapping.can_tx_dlc = this.msgConfig.can_tx_dlc;
            this.requestMapping.nai.target_type = this.msgConfig.nai.target_type;
            this.requestMapping.nai.source_addr = _msgConfig.SourceAddress;
            this.requestMapping.nai.target_addr = _msgConfig.TargetAddress;

            this.responseMapping = this.requestMapping;
            this.responseMapping.can_id = this.requestMapping.can_id_flow_ctrl;
            this.responseMapping.can_id_flow_ctrl = this.requestMapping.can_id;
            this.responseMapping.nai.source_addr = this.requestMapping.nai.target_addr;
            this.responseMapping.nai.target_addr = this.requestMapping.nai.source_addr;

            status = UDSApi.AddMapping_2013(this.handle, ref this.requestMapping);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                throw new Exception($"An Error ocurred while trying to add requestMapping during the initialization! {this.handle}");
            }

            status = UDSApi.AddMapping_2013(this.handle, ref this.responseMapping);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                throw new Exception($"An Error ocurred while trying to add responsetMapping during the initialization! {this.handle}");
            }

            return status;
        }

        /// <summary>
        /// Transmits a message using a connected PUDS channel.
        /// </summary>
        /// <param name="message">list or array with message bytes.</param>
        /// <returns>Returns TRUE if an error occurred.</returns>
        public bool Write(IEnumerable<byte> message)
        {
            var status = UDSApi.Reset_2013(this.handle);

            if (!UDSApi.StatusIsOk_2013(status))
            {
                string errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An Error ocurred while clearing the queue of the channel.", isErrorMessage: true);
                throw new Exception($"An Error ocurred while clearing the queue of the channel. Method: {nameof(this.Write)} Error: {errorTxt}");
            }

            var messageArray = message.ToArray();

            status = UDSApi.MsgAlloc_2013(out var txMsg, this.msgConfig, (uint)messageArray.Length);

            if (!UDSApi.StatusIsOk_2013(status))
            {
                string errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An error ocurred while alocating memory for the message. Error: {errorTxt}", isErrorMessage: true);
                return true;
            }

            if (!CanTpApi.setData_2016(ref txMsg.msg, 0, messageArray, messageArray.Length))
            {
                this.PrintAndLog($"An error ocurred while setting the data.");
                return true;
            }

            status = UDSApi.Write_2013(this.handle, ref txMsg);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                string errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An error ocurred while writting the message. Error: {errorTxt}", isErrorMessage: true);
                return true;
            }

            this.PrintAndLog($"Tx: {BitConverter.ToString(messageArray)}");

            status = UDSApi.MsgFree_2013(ref txMsg);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                string errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An Error ocurred while freeing Memory. Error: {errorTxt}", isErrorMessage: true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Matheus Version.
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="timeoutMilliSeconds"></param>
        /// <returns></returns>
        public uds_status OldRead(out byte[] responseMessage, uint timeoutMilliSeconds = 1000)
        {
            uds_status status;
            responseMessage = new byte[0];
            var sw = System.Diagnostics.Stopwatch.StartNew();

            while (true)
            {
                status = UDSApi.Read_2013(this.handle, out var rx_msg);

                if (!UDSApi.StatusIsOk_2013(status, uds_status.PUDS_STATUS_NO_MESSAGE) && (!UDSApi.StatusIsOk_2013(status)))
                {
                    try
                    {
                        UDSApi.MsgFree_2013(ref rx_msg);
                    }
                    catch (Exception ex)
                    {

                    }

                    break;
                }

                if (rx_msg.msg.type == cantp_msgtype.PCANTP_MSGTYPE_NONE)
                {
                    UDSApi.MsgFree_2013(ref rx_msg);

                    if (sw.ElapsedMilliseconds > timeoutMilliSeconds)
                    {
                        break;
                    }

                    continue;
                }

                // Check message
                bool msgok = false;
                byte[] rx_byte_array = new byte[rx_msg.msg.Msgdata_any_Copy.length];
                if (UDSApi.StatusIsOk_2013(status))
                {
                    msgok = CanTpApi.getData_2016(ref rx_msg.msg, 0, rx_byte_array, (int)rx_msg.msg.Msgdata_any_Copy.length);
                }

                if (msgok && (rx_msg.msg.can_info.can_id != this.msgConfig.can_id) && (rx_msg.msg.Msgdata_isotp_Copy.netaddrinfo.msgtype == cantp_isotp_msgtype.PCANTP_ISOTP_MSGTYPE_DIAGNOSTIC))
                {
                    responseMessage = rx_byte_array;
                    break;
                }
                else
                {
                    // TODO: Handle this!
                }

                UDSApi.MsgFree_2013(ref rx_msg);
            }

            return status;
        }

        /// <summary>
        /// Tries to read a Can message from the buffer. In case the response is not found in the buffer it will retry util timeout is reached.
        /// </summary>
        /// <param name="responseMessage"> ByteArray containing the can response.</param>
        /// <param name="timeoutMilliSeconds"> Time to retry to get the response from the buffer.</param>
        /// <returns>Returns TRUE if an error occurred.</returns>
        public bool Read(out byte[] responseMessage, uint timeoutMilliSeconds = 100)
        {
            bool error = true;
            uds_status status;
            responseMessage = new byte[0];
            var sw = System.Diagnostics.Stopwatch.StartNew();

            while (true)
            {
                status = UDSApi.Read_2013(this.handle, out var rx_msg);

                if (!UDSApi.StatusIsOk_2013(status, uds_status.PUDS_STATUS_NO_MESSAGE) && (!UDSApi.StatusIsOk_2013(status)))
                {
                    try
                    {
                        status = UDSApi.MsgFree_2013(ref rx_msg);
                        if (!UDSApi.StatusIsOk_2013(status))
                        {
                            var errorTxt = this.GetUdsStatusErrorText(status);
                            this.PrintAndLog($"An Error ocurred while freeing Memory. Error: {errorTxt}", isErrorMessage: true);
                            error = true;
                        }
                    }
                    catch
                    {
                    }

                    break;
                }

                if (rx_msg.msg.type == cantp_msgtype.PCANTP_MSGTYPE_NONE)
                {
                    status = UDSApi.MsgFree_2013(ref rx_msg);
                    if (!UDSApi.StatusIsOk_2013(status))
                    {
                        var errorTxt = this.GetUdsStatusErrorText(status);
                        this.PrintAndLog($"An Error ocurred while freeing Memory. Error: {errorTxt}", isErrorMessage: true);
                        error = true;
                    }

                    if (sw.ElapsedMilliseconds > timeoutMilliSeconds)
                    {
                        break;
                    }

                    continue;
                }

                if (rx_msg.type != uds_msgtype.PUDS_MSGTYPE_FLAG_LOOPBACK)
                {
                    if (UDSApi.GetDataNrc_2013(ref rx_msg, out var nrc))
                    {
                        this.PrintAndLog($"Negative Response! Code 0x{nrc:X2}: {GetNrcDescription((uds_nrc)nrc)}", isErrorMessage: true);
                        error = true;
                    }
                    else
                    {
                        error = false;
                    }

                    responseMessage = this.GetAnswer(ref rx_msg);
                    break;
                }
                else
                {
                    // TODO: Handle this?
                }

                status = UDSApi.MsgFree_2013(ref rx_msg);
                if (!UDSApi.StatusIsOk_2013(status))
                {
                    var errorTxt = this.GetUdsStatusErrorText(status);
                    this.PrintAndLog($"An Error ocurred while freeing Memory. Error: {errorTxt}", isErrorMessage: true);
                    error = true;
                }
            }

            return error;
        }

        /// <summary>
        /// Writes a message then waits and gets the response.
        /// </summary>
        /// <param name="message">List or Array containing the bytes to send.</param>
        /// <param name="response">byte array for saving the response.</param>
        /// <param name="timeOut">time to wait for the answer.</param>
        /// <param name="retryTimeOut">time to wait for the answer in case a retry is needed.</param>
        /// <param name="enhancedTimeOut">extra time to wait for the answer in case the ECU asks for more time.</param>
        /// <returns>Returns TRUE if an error occurred.</returns>
        public bool WriteAndGetResponse(IEnumerable<byte> message, out byte[] response, uint timeOut = 50, uint retryTimeOut = 50, uint enhancedTimeOut = 100)
        {
            bool error = true;
            response = null;

            var status = UDSApi.Reset_2013(this.handle);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                var errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An Error ocurred while clearing the queue of the channel.", isErrorMessage: true);
                throw new Exception($"An Error ocurred while clearing the queue of the channel. Method: {nameof(this.Write)}. Error: {errorTxt}");
            }

            var messageArray = message.ToArray();

            status = UDSApi.MsgAlloc_2013(out var txMsg, this.msgConfig, (uint)messageArray.Length);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                var errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An error ocurred while alocating memory for the message. Error: {errorTxt}", isErrorMessage: true);
            }

            if (!CanTpApi.setData_2016(ref txMsg.msg, 0, messageArray, messageArray.Length))
            {
                this.PrintAndLog($"An error ocurred while setting the data.");
            }

            status = UDSApi.Write_2013(this.handle, ref txMsg);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                string errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An error ocurred while writting the message. Error: {errorTxt}", isErrorMessage: true);
            }

            this.PrintAndLog($"Tx: {BitConverter.ToString(messageArray)}");

            status = UDSApi.WaitForSingleMessage_2013(this.handle, ref txMsg, false, timeOut, enhancedTimeOut, out var rxMsg);
            if (UDSApi.StatusIsOk_2013(status))
            {
                if (UDSApi.GetDataNrc_2013(ref rxMsg, out var nrc))
                {
                    this.PrintAndLog($"Negative Response! Code 0x{nrc:X2}: {GetNrcDescription((uds_nrc)nrc)}", isErrorMessage: true);
                }
                else
                {
                    error = false;
                }

                response = this.GetAnswer(ref rxMsg);
            }
            else
            {
                var errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"Error occurred while waiting for answer. Error: {errorTxt}", isErrorMessage: true);

                // ****************************************
                // RETRY ON TIMEOUT
                // ****************************************
                if (UDSApi.StatusIsOk_2013(status, uds_status.PUDS_STATUS_SERVICE_TIMEOUT_RESPONSE))
                {
                    this.PrintAndLog($"Retry...");
                    status = UDSApi.WaitForSingleMessage_2013(this.handle, ref txMsg, false, retryTimeOut, enhancedTimeOut, out var rxMsg_retry);
                    if (UDSApi.StatusIsOk_2013(status))
                    {
                        if (UDSApi.GetDataNrc_2013(ref rxMsg_retry, out var nrc))
                        {
                            this.PrintAndLog($"Negative Response! Code 0x{nrc:X2}: {GetNrcDescription((uds_nrc)nrc)}", isErrorMessage: true);
                        }
                        else
                        {
                            error = false;
                        }

                        response = this.GetAnswer(ref rxMsg_retry);
                    }
                    else
                    {
                        errorTxt = this.GetUdsStatusErrorText(status);
                        this.PrintAndLog($"Error occurred while waiting for answer after retry. Error: {errorTxt}", isErrorMessage: true);
                    }
                }
            }

            status = UDSApi.MsgFree_2013(ref rxMsg);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                var errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An Error ocurred while freeing Memory. Error {errorTxt}", isErrorMessage: true);
                error = true;
            }

            status = UDSApi.MsgFree_2013(ref txMsg);
            if (!UDSApi.StatusIsOk_2013(status))
            {
                var errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An Error ocurred while freeing Memory. Error {errorTxt}", isErrorMessage: true);
                error = true;
            }

            return error;
        }

        /// <summary>
        /// Uninitializes the PUD Channel of the present instance of <see cref="Uds"/> class.
        /// </summary>
        /// <returns>The return value is a uds_status code. PUDS_STATUS_OK is returned on success.</returns>
        public uds_status Uninitialize()
        {
            var status = UDSApi.Uninitialize_2013(this.handle);
            if (UDSApi.StatusIsOk_2013(status))
            {
                // do nothing
            }
            else
            {
                string errorTxt = this.GetUdsStatusErrorText(status);
                this.PrintAndLog($"An Error ocurred while trying to uninitialize the can interface {nameof(this.handle)} {this.handle}. Error: {errorTxt}", isErrorMessage: true);
                throw new Exception($"An Error ocurred while trying to uninitialize the can interface {this.handle} Error: {errorTxt}");
            }

            return status;
        }

        /// <summary>
        /// Helps configure the msgConfig struct with the needed values for the session.
        /// </summary>
        /// <param name="canId">CAN identifier.</param>
        /// <param name="protocol">Represents the protocol being used for communication.</param>
        /// <param name="canMsgType">Optional flags for the CAN layer.</param>
        /// <param name="udsMsgType">Message specific flags.</param>
        /// <param name="targetType">Represents the target address type.</param>
        public void MsgConfig(uint canId, uds_msgprotocol protocol, cantp_can_msgtype canMsgType, uds_msgtype udsMsgType, cantp_isotp_addressing targetType)
        {
            try
            {
                this.msgConfig = default;
                this.msgConfig.can_id = canId;
                this.msgConfig.can_msgtype = canMsgType;         // cantp_can_msgtype.PCANTP_CAN_MSGTYPE_STANDARD;
                this.msgConfig.type = udsMsgType;                // uds_msgtype.PUDS_MSGTYPE_USDT;
                this.msgConfig.nai.protocol = protocol;          // uds_msgprotocol.PUDS_MSGPROTOCOL_ISO_15765_2_11B_EXTENDED;
                this.msgConfig.nai.target_type = targetType;     // cantp_isotp_addressing.PCANTP_ISOTP_ADDRESSING_PHYSICAL;
            }
            catch (Exception ex)
            {
                throw new Exception($"An Exception was thrown while trying to configure the msgConfig struct on {nameof(this.MsgConfig)}. Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Gives back a text message that represents the UDs-Status.
        /// </summary>
        /// <param name="status">uds_status.</param>
        /// <returns>status as string.</returns>
        public string GetUdsStatusErrorText(uds_status status)
        {
            StringBuilder errorTxt = new StringBuilder(256);
            UDSApi.GetErrorText_2013(status, 0x09, errorTxt, 256);
            var text = errorTxt.ToString();
            return text;
        }

        /// <summary>
        /// Gets the text description for a given nrc (negative response code).
        /// </summary>
        /// <param name="nrc">Negative response enum. </param>
        /// <returns>Text description of the nrc. </returns>
        private static string GetNrcDescription(uds_nrc nrc)
        {
            var description = NrcDescriptionDictionary[nrc];
            return description;
        }

        /// <summary>
        /// Gives the text description for a given TPCANStatus error.
        /// </summary>
        /// <param name="error">TPCANStatus. </param>
        /// <returns>string. </returns>
        private static string GetFormattedError(TPCANStatus error)
        {
            // Creates a buffer big enough for a error-text
            var strTemp = new StringBuilder(256);

            // Gets the text using the GetErrorText API function
            // If the function success, the translated error is returned. If it fails, a text describing the current
            // error is returned.
            if (PCANBasic.GetErrorText(error, 0x09, strTemp) != TPCANStatus.PCAN_ERROR_OK)
            {
                return string.Format("An error occurred. Error-code's text ({0:X}) couldn't be retrieved", error);
            }

            return strTemp.ToString();
        }

        /// <summary>
        /// Needed for FindPeakDeviceUsbChannel().
        /// </summary>
        /// <param name="peakId"></param>
        /// <returns></returns>
        private static uint StringBuilderToId(StringBuilder peakId)
        {
            var byteList = peakId.ToString().Select(c => (byte)c).ToList();

            while (byteList.Count < sizeof(uint))
            {
                byteList.Add(0);
            }

            var id = BitConverter.ToUInt32(byteList.ToArray(), 0);
            return id;
        }

        /// <summary>
        /// Gets the data from the Buffer.
        /// </summary>
        /// <param name="rxMsg"></param>
        /// <returns>ByteArray containing answer.</returns>
        private byte[] GetAnswer(ref uds_msg rxMsg)
        {
            byte[] rxByteArray = new byte[rxMsg.msg.Msgdata_any_Copy.length];
            var msgOk = CanTpApi.getData_2016(ref rxMsg.msg, 0, rxByteArray, (int)rxMsg.msg.Msgdata_any_Copy.length);
            if (msgOk == true)
            {
                this.PrintAndLog($"Rx: {BitConverter.ToString(rxByteArray)}");
            }
            else if (msgOk == false)
            {
                this.PrintAndLog($"Error on received data. Method getData_2016 returned false!", isErrorMessage: true);
            }

            return rxByteArray;
        }

        /// <summary>
        /// If LogToFileEnable is true, this method writes the bus activity on a text file.
        /// </summary>
        /// <param name="txt"></param>
        private void LogToTextFile(string txt)
        {
            if (this.LogToFileEnable == true)
            {
                if (this.createLogFile == true)
                {
                    System.IO.Directory.CreateDirectory(this.LogFileDirectory);
                    if ((this.bayNo >= 1) && (this.bayNo <= 4))
                    {
                        this.LogFileName += $"_Bay{this.bayNo}.txt";
                    }
                    else
                    {
                        this.LogFileName += $"_BayNotDefined.txt";
                    }

                    File.AppendAllText(Path.Combine(this.LogFileDirectory, this.LogFileName), $"{DateTime.Now:yyyy_MM_dd HH:mm:ss}" + Environment.NewLine);
                    this.PrintOnScreen("@FG{WHITE}@BG{RED}Can Logfile creation ACTIVATED! " + Path.Combine(this.LogFileDirectory, this.LogFileName), true);
                    this.createLogFile = false;
                }

                File.AppendAllText(Path.Combine(this.LogFileDirectory, this.LogFileName), $"{DateTime.Now:HH:mm:ss:fff} {txt}" + Environment.NewLine);
            }
        }

        /// <summary>
        /// If PrintOnScreenEnable = true, this method prints the bus activity on screen.
        /// </summary>
        /// <param name="txt"></param>
        private void PrintOnScreen(string txt, bool isErrorMessage = false)
        {
            if (this.PrintOnScreenEnable)
            {
                this.PrintHandler(txt, isErrorMessage);
            }
        }

        /// <summary>
        /// This method calls two other methods: LogToTextFile() and PrintOnScreen()
        /// </summary>
        /// <param name="txt"> string to be printed or logged</param>
        /// <param name="isErrorMessage"> Depending on this parameter, the message will be printed on the report or the log Window. </param>
        private void PrintAndLog(string txt, bool isErrorMessage = false)
        {
            this.LogToTextFile(txt);
            this.PrintOnScreen(txt, isErrorMessage);
        }

        /// <summary>
        /// Contains the parameters that are needed for the configuration of the msgConfig.
        /// It has 2 Constructors. One for CAN-Basic and one for CAN-FD.
        /// </summary>
        public struct MsgConfiguration
        {

            /// <summary>
            /// Initializes a new instance of the <see cref="MsgConfiguration"/> struct.
            /// Use for CAN-Basic.
            /// </summary>
            /// <param name="canIdPhysycalRequest"> Physical request CAN ID from external test equipment (Peak) to ECU (DUT). Bsp. 0x7E1. </param>
            /// <param name="canIdPhysycalResponse"> Physical response CAN ID from ECU (DUT) to external test equipment (Peak) .Bsp. 0x7E9. </param>
            /// <param name="protocol"></param>
            /// <param name="canMsgType"></param>
            /// <param name="udsMsgType"></param>
            /// <param name="targetType"></param>
            /// <param name="canTxDlc">Number of Bytes per Telegram. Usually 8.</param>
            public MsgConfiguration(uint canIdPhysycalRequest, uint canIdPhysycalResponse, uds_msgprotocol protocol, cantp_can_msgtype canMsgType, uds_msgtype udsMsgType, cantp_isotp_addressing targetType, byte canTxDlc = 8)
            {
                this.CanIdPhysycalRequest = canIdPhysycalRequest;
                this.CanIdPhysycalResponse = canIdPhysycalResponse;
                this.CanMsgType = canMsgType;
                this.Protocol = protocol;
                this.UdsMsgType = udsMsgType;
                this.TargetType = targetType;
                this.CanTxDlc = canTxDlc;
                this.SourceAddress = 0;
                this.TargetAddress = 1;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MsgConfiguration"/> struct.
            /// USe for CAN-FD.
            /// </summary>
            /// <param name="canIdPhysycalRequest"> Physical request CAN ID from external test equipment (Peak) to ECU (DUT). Bsp. 0x7E1. </param>
            /// <param name="canIdPhysycalResponse"> Physical response CAN ID from ECU (DUT) to external test equipment (Peak) .Bsp. 0x7E9. </param>
            /// <param name="protocol"></param>
            /// <param name="canMsgType"></param>
            /// <param name="udsMsgType"></param>
            /// <param name="targetType"></param>
            /// <param name="sourceAddress"></param>
            /// <param name="targetAddress"></param>
            /// <param name="canTxDlc"></param>
            public MsgConfiguration(uint canIdPhysycalRequest, uint canIdPhysycalResponse, uds_msgprotocol protocol, cantp_can_msgtype canMsgType, uds_msgtype udsMsgType, cantp_isotp_addressing targetType, ushort sourceAddress, ushort targetAddress, byte canTxDlc = 8)
            {
                this.CanIdPhysycalRequest = canIdPhysycalRequest;
                this.CanIdPhysycalResponse = canIdPhysycalResponse;
                this.CanMsgType = canMsgType;
                this.Protocol = protocol;
                this.UdsMsgType = udsMsgType;
                this.TargetType = targetType;
                this.CanTxDlc = canTxDlc;
                this.SourceAddress = sourceAddress;
                this.TargetAddress = targetAddress;
            }

            public uint CanIdPhysycalRequest { get; set; }

            public uint CanIdPhysycalResponse { get; set; }

            public uds_msgprotocol Protocol { get; set; }

            public cantp_can_msgtype CanMsgType { get; set; }

            public uds_msgtype UdsMsgType { get; set; }

            public cantp_isotp_addressing TargetType { get; set; }

            public byte CanTxDlc { get; set; }

            public ushort SourceAddress { get; set; }

            public ushort TargetAddress { get; set; }
        }
    }
}
