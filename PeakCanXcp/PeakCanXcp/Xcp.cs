// <copyright file="Xcp.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Peak.Can.Basic;
    using Peak.Can.Xcp;

    /// <summary>
    /// Provides methods for communicating with an ECU using the XCP protocoll on CAN.
    /// </summary>
    public class Xcp
    {
        /* =========================================================================================
        // CONSTANT FIELDS
        // ====================================================================================== */

        /// <summary>
        /// The number of times that a method should try to reconnect to the ECU, after it has been disconnected.
        /// </summary>
        private const uint MaxNumberOfRetryTokens = 3;

        private TXCPProtocolLayerConfig protocolLayerConfig;

        private TXCPTransportLayerCAN transportLayerCAN;

        /* =========================================================================================
        // CONSTRUCTORS
        // ====================================================================================== */

        /// <summary>
        /// Initializes a new instance of the <see cref="Xcp"/> class using CAN-FD as the transport protocol. <br/>
        /// <br/>
        /// If the device ID isn't passed it is assumed that the PEAK-CAN device is connected to the <see cref="PCANBasic.PCAN_USBBUS1"/>.<br/>
        /// <br/>
        /// [WARNING] This library assumes a clock frequency of 24MHz for CAN-FD. If the target ECU uses another frequency, changes to
        /// this library are required.
        /// </summary>
        /// <param name="canFdNominalBitrate">The CAN-FD nominal bitrate.</param>
        /// <param name="canFdDataBitrate">The CAN-FD data bitrate.</param>
        /// <param name="masterId">The CAN-ID for the XCP-Master.</param>
        /// <param name="slaveId">The CAN-ID for the XCP-Slave.</param>
        /// <param name="broadcastId">The CAN-ID for XCP-Broadcast (default = 0).</param>
        /// <param name="deviceId">The ID of the PEAK-CAN-BUS device (default = 0).</param>
        public Xcp(NominalBitrate canFdNominalBitrate, DataBitrate canFdDataBitrate, uint masterId, uint slaveId, uint broadcastId = 0, uint? deviceId = null)
        {
            // Initializes the Bitrate property.
            this.InitBitrate(canFdNominalBitrate, canFdDataBitrate);

            // Initialize the XCP Protocol Layer Configuration.
            this.protocolLayerConfig = default(TXCPProtocolLayerConfig);
            ushort timeout = 100;
            this.protocolLayerConfig.T1 = timeout;
            this.protocolLayerConfig.T2 = timeout;
            this.protocolLayerConfig.T3 = timeout;
            this.protocolLayerConfig.T4 = timeout;
            this.protocolLayerConfig.T5 = timeout;
            this.protocolLayerConfig.T6 = timeout;
            this.protocolLayerConfig.T7 = timeout;

            // Initialize the XCP Transport Layer CAN.
            this.transportLayerCAN = default(TXCPTransportLayerCAN);
            this.transportLayerCAN.IncrementalIdUsed = false;
            this.transportLayerCAN.MasterID = masterId | 0x40000000;  // & 0x0FFFFFFF; //
            this.transportLayerCAN.SlaveID = slaveId | 0x40000000; // & 0x0FFFFFFF; //
            this.transportLayerCAN.BroadcastID = broadcastId;

            this.CanChannel = (ushort)this.GetCanChannelByDeviceId(deviceId);

            //// Initialize XCP on CAN-FD.
            var result = XCPApi.InitializeCanChannelFD(out ushort channel, this.CanChannel, this.Bitrate);
            //var result = XCPApi.InitializeCanChannel(out ushort channel, this.CanChannel, TPCANBaudrate.PCAN_BAUD_500K);
            this.XcpChannel = channel;

            if (result == TXCPResult.XCP_ERR_OK)
            {
                // Add the target ECU as slave.
                result = XCPApi.AddSlaveOnCAN(this.XcpChannel, transportLayerCAN, protocolLayerConfig, out uint session);
                this.XcpSession = session;

                // If it fails, uninitialize CAN channel and handle error.
                if (result != TXCPResult.XCP_ERR_OK)
                {
                    XCPApi.UninitializeChannel(this.XcpChannel);
                    this.XcpChannel = 0;
                    throw new XcpException(result);
                }
            }
            else
            {
                throw new XcpException(result);
            }
        }

        /* =========================================================================================
        // FINALIZERS
        // ====================================================================================== */

        /// <summary>
        /// Finalizes an instance of the <see cref="Xcp"/> class.
        /// </summary>
        ~Xcp()
        {
            // Uninitialize CAN channel.
            XCPApi.UninitializeChannel(this.XcpChannel);
        }

        /* =========================================================================================
        // ENUMS
        // ====================================================================================== */

        /// <summary>
        /// Available CAN FD nominal bitrates for a clock frequency of 24MHz.
        /// </summary>
        public enum NominalBitrate
        {
            /// <summary>
            /// 1 MBit/s.
            /// </summary>
            _1M,

            /// <summary>
            /// 800 kBit/s.
            /// </summary>
            _800k,

            /// <summary>
            /// 500 kBit/s.
            /// </summary>
            _500k,

            /// <summary>
            /// 250 kBits/s.
            /// </summary>
            _250k,

            /// <summary>
            /// 125 kBit/s.
            /// </summary>
            _125k,

            /// <summary>
            /// 100 kBit/s.
            /// </summary>
            _100k,

            /// <summary>
            /// 95.238 kBit/s.
            /// </summary>
            _95k238,

            /// <summary>
            /// 83.333 kBit/s.
            /// </summary>
            _83k333,

            /// <summary>
            /// 50 kBit/s.
            /// </summary>
            _50k,

            /// <summary>
            /// 47.619 kBit/s.
            /// </summary>
            _47k619,

            /// <summary>
            /// 33.333 kBit/s.
            /// </summary>
            _33k333,

            /// <summary>
            /// 20 kBit/s.
            /// </summary>
            _20k,

            /// <summary>
            /// 10 kBit/s.
            /// </summary>
            _10k,

            /// <summary>
            /// 5 kBit/s.
            /// </summary>
            _5k,
        }

        /// <summary>
        /// Available CAN FD data bitrates for a clock frequency of 24MHz.
        /// </summary>
        public enum DataBitrate
        {
            /// <summary>
            /// 6 MBit/s.
            /// </summary>
            _6M,

            /// <summary>
            /// 2 MBit/s.
            /// </summary>
            _2M,
        }

        /// <summary>
        /// Availble interfaces and channels for a PCAN bus.
        /// </summary>
        private enum PeakCanBusInterfaceChannels : ushort
        {
            /// <summary>
            /// Undefined/default value for a PCAN bus.
            /// </summary>
            NONEBUS = PCANBasic.PCAN_NONEBUS,

            /// <summary>
            /// PCAN-ISA interface, channel 1.
            /// </summary>
            ISABUS1 = PCANBasic.PCAN_ISABUS1,

            /// <summary>
            /// PCAN-ISA interface, channel 2.
            /// </summary>
            ISABUS2 = PCANBasic.PCAN_ISABUS2,

            /// <summary>
            /// PCAN-ISA interface, channel 3.
            /// </summary>
            ISABUS3 = PCANBasic.PCAN_ISABUS3,

            /// <summary>
            /// PCAN-ISA interface, channel 4.
            /// </summary>
            ISABUS4 = PCANBasic.PCAN_ISABUS4,

            /// <summary>
            /// PCAN-ISA interface, channel 5.
            /// </summary>
            ISABUS5 = PCANBasic.PCAN_ISABUS5,

            /// <summary>
            /// PCAN-ISA interface, channel 6.
            /// </summary>
            ISABUS6 = PCANBasic.PCAN_ISABUS6,

            /// <summary>
            /// PCAN-ISA interface, channel 7.
            /// </summary>
            ISABUS7 = PCANBasic.PCAN_ISABUS7,

            /// <summary>
            /// PCAN-ISA interface, channel 8.
            /// </summary>
            ISABUS8 = PCANBasic.PCAN_ISABUS8,

            /// <summary>
            /// PPCAN-Dongle/LPT interface, channel 1.
            /// </summary>
            DNGBUS1 = PCANBasic.PCAN_DNGBUS1,

            /// <summary>
            /// PCAN-PCI interface, channel 1.
            /// </summary>
            PCIBUS1 = PCANBasic.PCAN_PCIBUS1,

            /// <summary>
            /// PCAN-PCI interface, channel 2.
            /// </summary>
            PCIBUS2 = PCANBasic.PCAN_PCIBUS2,

            /// <summary>
            /// PCAN-PCI interface, channel 3.
            /// </summary>
            PCIBUS3 = PCANBasic.PCAN_PCIBUS3,

            /// <summary>
            /// PCAN-PCI interface, channel 4.
            /// </summary>
            PCIBUS4 = PCANBasic.PCAN_PCIBUS4,

            /// <summary>
            /// PCAN-PCI interface, channel 5.
            /// </summary>
            PCIBUS5 = PCANBasic.PCAN_PCIBUS5,

            /// <summary>
            /// PCAN-PCI interface, channel 6.
            /// </summary>
            PCIBUS6 = PCANBasic.PCAN_PCIBUS6,

            /// <summary>
            /// PCAN-PCI interface, channel 7.
            /// </summary>
            PCIBUS7 = PCANBasic.PCAN_PCIBUS7,

            /// <summary>
            /// PCAN-PCI interface, channel 8.
            /// </summary>
            PCIBUS8 = PCANBasic.PCAN_PCIBUS8,

            /// <summary>
            /// PCAN-PCI interface, channel 9.
            /// </summary>
            PCIBUS9 = PCANBasic.PCAN_PCIBUS9,

            /// <summary>
            /// PCAN-PCI interface, channel 10.
            /// </summary>
            PCIBUS10 = PCANBasic.PCAN_PCIBUS10,

            /// <summary>
            /// PCAN-PCI interface, channel 11.
            /// </summary>
            PCIBUS11 = PCANBasic.PCAN_PCIBUS11,

            /// <summary>
            /// PCAN-PCI interface, channel 12.
            /// </summary>
            PCIBUS12 = PCANBasic.PCAN_PCIBUS12,

            /// <summary>
            /// PCAN-PCI interface, channel 13.
            /// </summary>
            PCIBUS13 = PCANBasic.PCAN_PCIBUS13,

            /// <summary>
            /// PCAN-PCI interface, channel 14.
            /// </summary>
            PCIBUS14 = PCANBasic.PCAN_PCIBUS14,

            /// <summary>
            /// PCAN-PCI interface, channel 15.
            /// </summary>
            PCIBUS15 = PCANBasic.PCAN_PCIBUS15,

            /// <summary>
            /// PCAN-PCI interface, channel 16.
            /// </summary>
            PCIBUS16 = PCANBasic.PCAN_PCIBUS16,

            /// <summary>
            /// PCAN-USB interface, channel 1.
            /// </summary>
            USBBUS1 = PCANBasic.PCAN_USBBUS1,

            /// <summary>
            /// PCAN-USB interface, channel 2.
            /// </summary>
            USBBUS2 = PCANBasic.PCAN_USBBUS2,

            /// <summary>
            /// PCAN-USB interface, channel 3.
            /// </summary>
            USBBUS3 = PCANBasic.PCAN_USBBUS3,

            /// <summary>
            /// PCAN-USB interface, channel 4.
            /// </summary>
            USBBUS4 = PCANBasic.PCAN_USBBUS4,

            /// <summary>
            /// PCAN-USB interface, channel 5.
            /// </summary>
            USBBUS5 = PCANBasic.PCAN_USBBUS5,

            /// <summary>
            /// PCAN-USB interface, channel 6.
            /// </summary>
            USBBUS6 = PCANBasic.PCAN_USBBUS6,

            /// <summary>
            /// PCAN-USB interface, channel 7.
            /// </summary>
            USBBUS7 = PCANBasic.PCAN_USBBUS7,

            /// <summary>
            /// PCAN-USB interface, channel 8.
            /// </summary>
            USBBUS8 = PCANBasic.PCAN_USBBUS8,

            /// <summary>
            /// PCAN-USB interface, channel 9.
            /// </summary>
            USBBUS9 = PCANBasic.PCAN_USBBUS9,

            /// <summary>
            /// PCAN-USB interface, channel 10.
            /// </summary>
            USBBUS10 = PCANBasic.PCAN_USBBUS10,

            /// <summary>
            /// PCAN-USB interface, channel 11.
            /// </summary>
            USBBUS11 = PCANBasic.PCAN_USBBUS11,

            /// <summary>
            /// PCAN-USB interface, channel 12.
            /// </summary>
            USBBUS12 = PCANBasic.PCAN_USBBUS12,

            /// <summary>
            /// PCAN-USB interface, channel 13.
            /// </summary>
            USBBUS13 = PCANBasic.PCAN_USBBUS13,

            /// <summary>
            /// PCAN-USB interface, channel 14.
            /// </summary>
            USBBUS14 = PCANBasic.PCAN_USBBUS14,

            /// <summary>
            /// PCAN-USB interface, channel 15.
            /// </summary>
            USBBUS15 = PCANBasic.PCAN_USBBUS15,

            /// <summary>
            /// PCAN-USB interface, channel 16.
            /// </summary>
            USBBUS16 = PCANBasic.PCAN_USBBUS16,

            /// <summary>
            /// PCAN-PC Card interface, channel 1.
            /// </summary>
            PCCBUS1 = PCANBasic.PCAN_PCCBUS1,

            /// <summary>
            /// PCAN-PC Card interface, channel 2.
            /// </summary>
            PCCBUS2 = PCANBasic.PCAN_PCCBUS2,

            /// <summary>
            /// PCAN-LAN interface, channel 1.
            /// </summary>
            LANBUS1 = PCANBasic.PCAN_LANBUS1,

            /// <summary>
            /// PCAN-LAN interface, channel 2.
            /// </summary>
            LANBUS2 = PCANBasic.PCAN_LANBUS2,

            /// <summary>
            /// PCAN-LAN interface, channel 3.
            /// </summary>
            LANBUS3 = PCANBasic.PCAN_LANBUS3,

            /// <summary>
            /// PCAN-LAN interface, channel 4.
            /// </summary>
            LANBUS4 = PCANBasic.PCAN_LANBUS4,

            /// <summary>
            /// PCAN-LAN interface, channel 5.
            /// </summary>
            LANBUS5 = PCANBasic.PCAN_LANBUS5,

            /// <summary>
            /// PCAN-LAN interface, channel 6.
            /// </summary>
            LANBUS6 = PCANBasic.PCAN_LANBUS6,

            /// <summary>
            /// PCAN-LAN interface, channel 7.
            /// </summary>
            LANBUS7 = PCANBasic.PCAN_LANBUS7,

            /// <summary>
            /// PCAN-LAN interface, channel 8.
            /// </summary>
            LANBUS8 = PCANBasic.PCAN_LANBUS8,

            /// <summary>
            /// PCAN-LAN interface, channel 9.
            /// </summary>
            LANBUS9 = PCANBasic.PCAN_LANBUS9,

            /// <summary>
            /// PCAN-LAN interface, channel 10.
            /// </summary>
            LANBUS10 = PCANBasic.PCAN_LANBUS10,

            /// <summary>
            /// PCAN-LAN interface, channel 11.
            /// </summary>
            LANBUS11 = PCANBasic.PCAN_LANBUS11,

            /// <summary>
            /// PCAN-LAN interface, channel 12.
            /// </summary>
            LANBUS12 = PCANBasic.PCAN_LANBUS12,

            /// <summary>
            /// PCAN-LAN interface, channel 13.
            /// </summary>
            LANBUS13 = PCANBasic.PCAN_LANBUS13,

            /// <summary>
            /// PCAN-LAN interface, channel 14.
            /// </summary>
            LANBUS14 = PCANBasic.PCAN_LANBUS14,

            /// <summary>
            /// PCAN-LAN interface, channel 15.
            /// </summary>
            LANBUS15 = PCANBasic.PCAN_LANBUS15,

            /// <summary>
            /// PCAN-LAN interface, channel 16.
            /// </summary>
            LANBUS16 = PCANBasic.PCAN_LANBUS16,
        }

        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets the descriptor for the CAN-Channel used by the XCP library.
        /// </summary>
        public ushort CanChannel { get; }

        /// <summary>
        /// Gets or sets a string containing the bitrate information, which is necessary for using the
        /// <see cref="XCPApi.InitializeCanChannelFD"/> PEAK-API method. This string is initialized by
        /// the <see cref="InitBitrate(NominalBitrate, DataBitrate)"/> method.
        /// </summary>
        public string Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the XCP-Channel provided by the <see cref="XCPApi.InitializeCanChannelFD"/> PEAK-API method on the constructor.<br/>
        /// This property is used by the <see cref="XCPApi.AddSlaveOnCAN"/> and <see cref="XCPApi.UninitializeChannel"/> PEAK-API methods.
        /// </summary>
        private ushort XcpChannel { get; set; } = 0;

        /// <summary>
        /// Gets or sets the XCP-Session provided by the <see cref="XCPApi.AddSlaveOnCAN"/> PEAK-API method on the constructor.<br/>
        /// It is used on later calls to methods of the <see cref="XCPApi"/>.
        /// </summary>
        private uint XcpSession { get; set; } = 0;

        /// <summary>
        /// Gets or sets the current number of available retry-tokens.<br/>
        /// If the retry-tokens get to zero, a method will stop trying to reconnect to the XCP session.
        /// </summary>
        private uint RetryTokens { get; set; } = MaxNumberOfRetryTokens;

        /* =========================================================================================
        // METHODS
        // ====================================================================================== */

        public void Initialize()
        {
            var result = XCPApi.InitializeCanChannelFD(out ushort channel, this.CanChannel, this.Bitrate);
            //var result = XCPApi.InitializeCanChannel(out ushort channel, this.CanChannel, TPCANBaudrate.PCAN_BAUD_500K);
            this.XcpChannel = channel;

            if (result == TXCPResult.XCP_ERR_OK)
            {
                // Add the target ECU as slave.
                result = XCPApi.AddSlaveOnCAN(this.XcpChannel, this.transportLayerCAN, this.protocolLayerConfig, out uint session);
                this.XcpSession = session;

                // If it fails, uninitialize CAN channel and handle error.
                if (result != TXCPResult.XCP_ERR_OK)
                {
                    XCPApi.UninitializeChannel(this.XcpChannel);
                    this.XcpChannel = 0;
                    throw new XcpException(result);
                }
            }
            else
            {
                throw new XcpException(result);
            }
        }

        public void Uninitialize()
        {
            XCPApi.UninitializeChannel(this.XcpChannel);
        }

        /// <summary>
        /// The “Connect” command is used for setting up the connection with slave. The slave can
        /// either give a positive response or a negative response.
        /// </summary>
        /// <param name="retry">How many times we should try to establish a xcp connection.b</param>
        /// <returns>
        /// The slave response as a byte list.<br/>
        /// <br/>
        /// Positive response:<br/>
        /// B0: 0xFF | B1: RESOURCE | B2: RESOURCE | B3: COMM_MODE_BASIC | B4: MAX_CTO | B5: MAX_DTO | B6: XCP Version | B7: Transport Layer Version.<br/>
        /// <br/>
        /// Negative response:<br/>
        /// B0: 0xFE | B1: Error Code.
        /// </returns>
        public List<byte> Connect(int retry = 5)
        {
            // Create a new byte array for the slave response.
            // The length of the array is the maximal CTO length for XCP on CAN-FD.
            var response = new byte[XCPApi.CAN_MAX_LEN_FD];

            // Execute the Connect command through the PEAK-API.
            TXCPResult? result = null;

            for (var i = retry; (i > 0) && result != TXCPResult.XCP_ERR_OK; i--)
            {
                result = XCPApi.Connect(this.XcpSession, 0x00, response, (ushort)response.Length);
                Thread.Sleep(10);
            }

            // Handler error.
            if (result != TXCPResult.XCP_ERR_OK)
            {
                throw new XcpException(result.Value);
            }

            return response.ToList();
        }

        /// <summary>
        /// Terminate the connection with slave.
        /// </summary>
        /// <returns>The slave response as a byte list.</returns>
        public List<byte> Disconnect()
        {
            // Create a new byte array for the slave response.
            // The length of the array is the maximal CTO length for XCP on CAN-FD.
            var response = new byte[XCPApi.CAN_MAX_LEN_FD];

            // Execute the Disconnect command through the PEAK-API.
            var result = XCPApi.Disconnect(this.XcpSession, response, (ushort)response.Length);

            // Handler error.
            if (result != TXCPResult.XCP_ERR_OK)
            {
                throw new XcpException(result);
            }

            return response.ToList();
        }

        /// <summary>
        /// Requests the value of a measurement or a characteristic parameter from the Slave.
        /// </summary>
        /// <param name="value">The <see cref="Measurement"/> or <see cref="Characteristic"/> representing the parameter.</param>
        /// <returns>The requested value as a byte list (Little Endian).</returns>
        public List<byte> ShortUpload(XcpValue value)
        {
            return this.ShortUpload(value.EcuAddress, value.EcuAddressExtension, value.ByteSize);
        }

        /// <summary>
        /// Requests the value of a measurement or a characteristic parameter from the Slave.
        /// </summary>
        /// <param name="address">The ECU address for the parameter (Address Extension is defaulted to 0).</param>
        /// <param name="byteSize">The byte size of the parameter.</param>
        /// <returns>The requested value as a byte list (Little Endian).</returns>
        public List<byte> ShortUpload(uint address, uint byteSize)
        {
            return this.ShortUpload(address, 0x00, byteSize);
        }

        /// <summary>
        /// Requests the value of a measurement or a characteristic parameter from the Slave.
        /// </summary>
        /// <param name="address">The ECU address for the parameter.</param>
        /// <param name="addressExtension">The ECU address extension for the parameter.</param>
        /// <param name="byteSize">The byte size of the parameter.</param>
        /// <returns>The requested value as a byte list (Little Endian).</returns>
        public List<byte> ShortUpload(uint address, byte addressExtension, uint byteSize)
        {
            try
            {
                // Response array is one byte longer than the parameter.
                var response = new byte[byteSize + 1];

                var result = TXCPResult.XCP_ERR_OK;

                var responseByteList = new List<byte>();

                if (byteSize > 63)
                {
                    XCPApi.SetMemoryTransferAddress(this.XcpSession, addressExtension, address);

                    Thread.Sleep(100);

                    while (byteSize > 0)
                    {
                        ushort length;
                        if (byteSize > 63)
                        {
                            length = 63 + 1;
                            response = new byte[length];
                            result = XCPApi.Upload(this.XcpSession, (byte)63, response, length);
                            byteSize = byteSize - 63;
                        }
                        else
                        {
                            length = (ushort)(byteSize + 1);
                            result = XCPApi.Upload(this.XcpSession, (byte)byteSize, response, length);
                            byteSize = byteSize - byteSize;
                        }

                        var partialResponseByteList = response.ToList();

                        // Remove Packet Identifier.
                        partialResponseByteList.RemoveAt(0);

                        responseByteList = responseByteList.Concat(partialResponseByteList).ToList();

                        response = new byte[byteSize + 1];
                    }
                }
                else
                {
                    result = XCPApi.ShortUpload(this.XcpSession, (byte)byteSize, addressExtension, address, response, (ushort)response.Length);

                    responseByteList = response.ToList();

                    // Remove Packet Identifier.
                    responseByteList.RemoveAt(0);
                }

                // Handle error.
                if (result != TXCPResult.XCP_ERR_OK)
                {
                    throw new XcpException(result);
                }

                return responseByteList;
            }
            catch (Exception exception)
            {
                // This will try to reconnect the Xcp session and recursively call this function as long as there are RetryTokens available.
                // Once RetryTokens reach zero the original exception is thrown.
                return this.ReconnectAndRetry(
                    exception,
                    (args) => this.ShortUpload((uint)args[0], (byte)args[1], (uint)args[2]),
                    address,
                    addressExtension,
                    byteSize);
            }
        }

        /// <summary>
        /// This command will download data to the ECU. The length of data bytes to be downloaded will be specified in the command.<br/>
        /// <br/>
        /// [WARNING] This function assumes that each array element is stored in 32-bit slots in the ECU memory. If this isn't the case,
        /// the array functionality won't work. As a workaround you can pass the exactly <paramref name="address"/> for the array element
        /// you want to access and use the default <paramref name="arrayIndex"/> of -1.
        /// </summary>
        /// <param name="value">The <see cref="XcpValue"/> representing the parameter.</param>
        /// <param name="bytes">The new value for the parameter as a list of bytes (Little Endian).</param>
        /// <param name="retry">The number of times the download operation should be retried, if it fails.</param>
        /// <param name="arrayIndex">
        /// If this characteristic represents an array, the index of the element being updated.
        /// Use -1 (default) for non-array parameters.
        /// </param>
        /// <returns>The slave response as a byte list.</returns>
        public List<byte> Download(XcpValue value, List<byte> bytes, int retry = 0, int arrayIndex = -1, int millisencondsDelay = 10)
        {
            return this.Download(value.EcuAddress, value.EcuAddressExtension, bytes, retry, arrayIndex, millisencondsDelay);
        }

        /// <summary>
        /// This command will download data to the ECU. The length of data bytes to be downloaded will be specified in the command.<br/>
        /// <br/>
        /// [WARNING] This function assumes that each array element is stored in 32-bit slots in the ECU memory. If this isn't the case,
        /// the array functionality won't work. As a workaround you can pass the exactly <paramref name="address"/> for the array element
        /// you want to access and use the default <paramref name="arrayIndex"/> of -1.
        /// </summary>
        /// <param name="address">The ECU address for the parameter (Address Extension is defaulted to 0).</param>
        /// <param name="bytes">The new value for the parameter as a list of bytes (Little Endian).</param>
        /// <param name="retry">The number of times the download operation should be retried, if it fails.</param>
        /// <param name="arrayIndex">
        /// If this characteristic represents an array, the index of the element being updated.
        /// Use -1 (default) for non-array parameters.
        /// </param>
        /// <returns>The slave response as a byte list.</returns>
        public List<byte> Download(uint address, List<byte> bytes, int retry = 0, int arrayIndex = -1, int millisencondsDelay = 10)
        {
            return this.Download(address, 0, bytes, retry, arrayIndex, millisencondsDelay);
        }

        /// <summary>
        /// This command will download data to the ECU. The length of data bytes to be downloaded will be specified in the command.<br/>
        /// <br/>
        /// [WARNING] This function assumes that each array element is stored in 32-bit slots in the ECU memory. If this isn't the case,
        /// the array functionality won't work. As a workaround you can pass the exactly <paramref name="address"/> for the array element
        /// you want to access and use the default <paramref name="arrayIndex"/> of -1.
        /// </summary>
        /// <param name="address">The ECU address for the parameter.</param>
        /// <param name="addressExtension">The ECU address extension for the parameter.</param>
        /// <param name="bytes">The new value for the parameter as a list of bytes (Little Endian).</param>
        /// <param name="retry">The number of times the download operation should be retried, if it fails.</param>
        /// <param name="arrayIndex">
        /// If this characteristic represents an array, the index of the element being updated.
        /// Use -1 (default) for non-array parameters.
        /// </param>
        /// <returns>The slave response as a byte list.</returns>
        public List<byte> Download(uint address, byte addressExtension, List<byte> bytes, int retry = 0, int arrayIndex = -1, int millisencondsDelay = 10)
        {
            // This variable prevents this function to enter an infinite recursive loop by marking that an exception caused by
            // ShortUplaod() was thrown.
            var shortUploadExceptionDetected = false;

            try
            {
                // Adjust the address according to the array index.
                // Each element is stored in 32 Bits (offset of 4 bytes).
                if (arrayIndex >= 0)
                {
                    address = address + (4 * (uint)arrayIndex);
                }

                var result = TXCPResult.XCP_ERR_GENERIC;

                var response = new byte[bytes.Count + 1];

                var shortUploadResult = new List<byte>();

                var operationFailed = true;

                // Download the data and check if it was stored in the ECU. Retry operation as required, as long as the operation fails.
                for (int i = 0; (i <= retry) && operationFailed; i++)
                {
                    // Set the memory address to which we want to download the data.
                    XCPApi.SetMemoryTransferAddress(this.XcpSession, addressExtension, address);

                    // Execute Download command via XCP-API.
                    result = XCPApi.Download(this.XcpSession, (byte)bytes.Count, bytes.ToArray(), (byte)bytes.Count, response, (ushort)response.Length);

                    try
                    {
                        // Read the value back, to check if it was stored in the ECU.
                        shortUploadResult = this.ShortUpload(address, (byte)bytes.Count);
                    }
                    catch (Exception exception)
                    {
                        shortUploadExceptionDetected = true;
                        throw exception;
                    }

                    // The operation has failed if the result code isn't XCP_ERR_OK or if the uploaded bytes aren't equal to the downloaded bytes.
                    operationFailed = (result != TXCPResult.XCP_ERR_OK) || !Enumerable.SequenceEqual(shortUploadResult, bytes);
                }

                // Handle error.
                if (result != TXCPResult.XCP_ERR_OK)
                {
                    throw new XcpException(result);
                }

                Thread.Sleep(millisencondsDelay);

                return response.ToList();
            }
            catch (Exception exception)
            {
                if (shortUploadExceptionDetected)
                {
                    // Prevent a recursive loop, caused by short upload restoring the retry tokens.
                    throw exception;
                }
                else
                {
                    // This will try to reconnect the Xcp session and recursively call this function as long as there are RetryTokens available.
                    // Once RetryTokens reach zero the exception is thrown.
                    return this.ReconnectAndRetry(
                        exception,
                        (args) => this.Download((uint)args[0], (byte)args[1], (List<byte>)args[2], (int)args[3]),
                        address,
                        addressExtension,
                        bytes,
                        retry);
                }
            }
        }

        /// <summary>
        /// Gets the value of a XCP parameter of type <see cref="XcpValue.DataTypes.SLONG"/>.
        /// </summary>
        /// <param name="address">The ECU address for the parameter.</param>
        /// <param name="addressExtension">The ECU address extension for the parameter.</param>
        /// <returns>The value of the XCP parameter.</returns>
        public int GetSLong(uint address, byte addressExtension = 0)
        {
            var bytes = this.ShortUpload(address, addressExtension, 4);

            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }

        /// <summary>
        /// Gets the value of a XCP parameter of type <see cref="XcpValue.DataTypes.ULONG"/>.
        /// </summary>
        /// <param name="address">The ECU address for the parameter.</param>
        /// <param name="addressExtension">The ECU address extension for the parameter.</param>
        /// <returns>The value of the XCP parameter.</returns>
        public uint GetULong(uint address, byte addressExtension = 0)
        {
            var bytes = this.ShortUpload(address, addressExtension, 4);

            return BitConverter.ToUInt32(bytes.ToArray(), 0);
        }

        /// <summary>
        /// Gets the value of a XCP parameter of type <see cref="XcpValue.DataTypes.UWORD"/>.
        /// </summary>
        /// <param name="address">The ECU address for the parameter.</param>
        /// <param name="addressExtension">The ECU address extension for the parameter.</param>
        /// <returns>The value of the XCP parameter.</returns>
        public uint GetUWord(uint address, byte addressExtension = 0)
        {
            var bytes = this.ShortUpload(address, addressExtension, 2);

            return BitConverter.ToUInt16(bytes.ToArray(), 0);
        }

        /// <summary>
        /// Gets the value of a XCP parameter of type <see cref="XcpValue.DataTypes.FLOAT32_IEEE"/>.
        /// </summary>
        /// <param name="address">The ECU address for the parameter.</param>
        /// <param name="addressExtension">The ECU address extension for the parameter.</param>
        /// <returns>The value of the XCP parameter.</returns>
        public float GetFloat32(uint address, byte addressExtension = 0)
        {
            var bytes = this.ShortUpload(address, addressExtension, 4);

            return BitConverter.ToSingle(bytes.ToArray(), 0);
        }

        /// <summary>
        /// Gets the converted (physical) value of a XCP parameter using its COMPU_METHOD.<br/>
        /// </summary>
        /// <param name="value">The <see cref="Measurement"/> or <see cref="Characteristic"/> representing the parameter.</param>
        /// <param name="arrayIndex">
        /// If this characteristic represents an array, the index of the element being updated.
        /// Use -1 (default) for non-array parameters.
        /// </param>
        /// <returns>The converted value (value and unit).</returns>
        public ConvertedValue GetConvertedValue(XcpValue value, int arrayIndex = -1)
        {
            var converted = new ConvertedValue();

            var address = value.EcuAddress;

            // Update address accordingly to the array index.
            if (((int)value.MatrixDim[0] >= arrayIndex) && (arrayIndex >= 0))
            {
                address = value.EcuAddress + (value.ByteSize * (uint)arrayIndex);
            }

            // Retrieve value from the ECU with the corresponding functions, depending on the data type.
            switch (value.DataType)
            {
                case XcpValue.DataTypes.SLONG:
                    converted.Value = this.GetSLong(address, value.EcuAddressExtension);
                    break;
                case XcpValue.DataTypes.ULONG:
                    converted.Value = this.GetULong(address, value.EcuAddressExtension);
                    break;
                case XcpValue.DataTypes.FLOAT32_IEEE:
                    converted.Value = this.GetFloat32(address, value.EcuAddressExtension);
                    break;
                case XcpValue.DataTypes.UBYTE:
                    converted.Value = this.ShortUpload(value)[0];
                    break;
                case XcpValue.DataTypes.UWORD:
                    converted.Value = this.GetUWord(address, value.EcuAddressExtension);
                    break;
                default:
                    throw new Exception($"Data type {value.DataType} not supported.");
            }

            // Compute the conversion using the corresponding COMPU_METHOD.
            if (value.Conversion != "NO_COMPU_METHOD")
            {
                converted.Unit = value.CompuMethod.Unit;

                switch (value.CompuMethod.Type)
                {
                    case CompuMethod.ConversionType.LINEAR:
                        {
                            var a = value.CompuMethod.CoeffsLinear[0];
                            var b = value.CompuMethod.CoeffsLinear[1];
                            converted.Value = (a * converted.Value) + b;
                            break;
                        }

                    case CompuMethod.ConversionType.RAT_FUNC:
                        {
                            var a = value.CompuMethod.Coeffs[0];
                            var b = value.CompuMethod.Coeffs[1];
                            var c = value.CompuMethod.Coeffs[2];
                            var d = value.CompuMethod.Coeffs[3];
                            var e = value.CompuMethod.Coeffs[4];
                            var f = value.CompuMethod.Coeffs[5];

                            converted.Value = ((a * Math.Pow(converted.Value, 2)) + (b * converted.Value) + c) / ((d * Math.Pow(converted.Value, 2)) + (e * converted.Value) + f);
                            break;
                        }
                }
            }

            return converted;
        }

        public string GetAscii(Characteristic characteristic, int retry = 5, int delayMs = 100)
        {
            var ascii = string.Empty;
            var length = (uint)characteristic.Number;

            for (int i = 0; (i < retry) && (ascii == string.Empty); i++)
            {
                var bytes = this.ShortUpload(characteristic.EcuAddress, length).ToArray();
                ascii = Encoding.ASCII.GetString(bytes).Split('\0')[0].Trim();
                Thread.Sleep(delayMs);
            }

            return ascii;
        }

        /// <summary>
        /// It tries to connect to the XCP session and then it calls a function that has failed before with an exception.
        /// By each try it decrements <see cref="RetryTokens"/>, if it gets to zero, the original exception will be thrown.
        /// </summary>
        /// <typeparam name="TResult">The return type of the function being retried.</typeparam>
        /// <param name="exception">The original exception, which will be thrown if <see cref="RetryTokens"/> get to zero.</param>
        /// <param name="f">The original function we are trying to call after a new connection has been established.</param>
        /// <param name="args">The arguments for the original function.</param>
        /// <returns>The result of the original function after being called successfully.</returns>
        private TResult ReconnectAndRetry<TResult>(Exception exception, Func<object[], TResult> f, params object[] args)
        {
            if (this.RetryTokens > 0)
            {
                this.RetryTokens--;

                try
                {
                    this.Connect();
                }
                catch
                {
                    // Ignore exception because we will throw the original exception if the retry-tokens get to zero.
                }

                TResult result;

                try
                {
                    // Call the original function.
                    result = f(args);
                }
                catch
                {
                    // Function failed, throw original exception.
                    throw exception;
                }

                // If we ever get back here, the function has worked, restore the tokens.
                this.RetryTokens = MaxNumberOfRetryTokens;

                return result;
            }
            else
            {
                // Tokens got to zero. Restore the tokens and throw the original excepetion.
                this.RetryTokens = MaxNumberOfRetryTokens;

                throw exception;
            }
        }

        /// <summary>
        /// Initializes the <see cref="Bitrate"/> property with the correct text, given the desired nominal
        /// and data bitrates for CAN-FD. The configurations for each bitrate option was extracted from the
        /// PCAN-View software.
        /// </summary>
        /// <param name="canFdNominalBitrate">Nominal Bitrate for CAN-FD.</param>
        /// <param name="canFdDataBitrate">Data Bitrate for CAN-FD.</param>
        private void InitBitrate(NominalBitrate canFdNominalBitrate, DataBitrate canFdDataBitrate)
        {
            // Clock frequency will be added to the Bitrate string.
            int clockFrequencyMhz = 80;

            int nomBrp = default;
            int nomTseg1 = default;
            int nomTseg2 = default;
            int nomSjw = default;

            // Select the right parameters for the nominal bitrate.
            switch (canFdNominalBitrate)
            {
                case NominalBitrate._1M:
                    nomBrp = 3;
                    nomTseg1 = 5;
                    nomTseg2 = 2;
                    nomSjw = 1;
                    break;
                case NominalBitrate._800k:
                    nomBrp = 3;
                    nomTseg1 = 7;
                    nomTseg2 = 2;
                    nomSjw = 1;
                    break;
                case NominalBitrate._500k:
                    nomBrp = 2;
                    nomTseg1 = 63;
                    nomTseg2 = 16;
                    nomSjw = 16;
                    break;
                case NominalBitrate._250k:
                    nomBrp = 6;
                    nomTseg1 = 13;
                    nomTseg2 = 2;
                    nomSjw = 1;
                    break;
                case NominalBitrate._125k:
                    nomBrp = 12;
                    nomTseg1 = 13;
                    nomTseg2 = 2;
                    nomSjw = 1;
                    break;
                case NominalBitrate._100k:
                    nomBrp = 12;
                    nomTseg1 = 16;
                    nomTseg2 = 3;
                    nomSjw = 2;
                    break;
                case NominalBitrate._95k238:
                    nomBrp = 12;
                    nomTseg1 = 15;
                    nomTseg2 = 5;
                    nomSjw = 4;
                    break;
                case NominalBitrate._83k333:
                    nomBrp = 18;
                    nomTseg1 = 12;
                    nomTseg2 = 3;
                    nomSjw = 3;
                    break;
                case NominalBitrate._50k:
                    nomBrp = 24;
                    nomTseg1 = 16;
                    nomTseg2 = 3;
                    nomSjw = 2;
                    break;
                case NominalBitrate._47k619:
                    nomBrp = 63;
                    nomTseg1 = 5;
                    nomTseg2 = 2;
                    nomSjw = 1;
                    break;
                case NominalBitrate._33k333:
                    nomBrp = 36;
                    nomTseg1 = 16;
                    nomTseg2 = 3;
                    nomSjw = 3;
                    break;
                case NominalBitrate._20k:
                    nomBrp = 60;
                    nomTseg1 = 16;
                    nomTseg2 = 3;
                    nomSjw = 2;
                    break;
                case NominalBitrate._10k:
                    nomBrp = 120;
                    nomTseg1 = 16;
                    nomTseg2 = 3;
                    nomSjw = 2;
                    break;
                case NominalBitrate._5k:
                    nomBrp = 192;
                    nomTseg1 = 16;
                    nomTseg2 = 8;
                    nomSjw = 2;
                    break;
            }

            int dataBrp = default;
            int dataTseg1 = default;
            int dataTseg2 = default;
            int dataSjw = default;

            // Select the right parameters for the data bitrate.
            switch (canFdDataBitrate)
            {
                case DataBitrate._6M:
                    dataBrp = 1;
                    dataTseg1 = 1;
                    dataTseg2 = 2;
                    dataSjw = 1;
                    break;
                case DataBitrate._2M:
                    dataBrp = 2;
                    dataTseg1 = 15;
                    dataTseg2 = 4;
                    dataSjw = 4;
                    break;
            }

            // Create configuration string for both bitrates.
            string nominalConfig = $"nom_brp={nomBrp}, nom_tseg1={nomTseg1}, nom_tseg2={nomTseg2}, nom_sjw={nomSjw}, ";
            string dataConfig = $"data_brp={dataBrp}, data_tseg1={dataTseg1}, data_tseg2={dataTseg2}, data_sjw={dataSjw}";

            // Build the bitrate configuration string used to initialize CAN-FD for XCP.
            this.Bitrate = $"f_clock_mhz={clockFrequencyMhz}, " + nominalConfig + dataConfig;
        }

        /// <summary>
        /// Gets the interface channel to which a PEAK-CAN device is connected, given the device id.
        /// </summary>
        /// <param name="deviceId">Device Id set with the software PEAK-Hardware.</param>
        /// <returns>The interface channel as an enum.</returns>
        private PeakCanBusInterfaceChannels GetCanChannelByDeviceId(uint? deviceId)
        {
            // If no device id is given, assume USBBUS1 as the interface channel.
            if (deviceId is null)
            {
                return PeakCanBusInterfaceChannels.USBBUS1;
            }

            // Create a list of all possible PeakCanBusInterfaceChannels enum values.
            var channelsList = Enum.GetValues(typeof(PeakCanBusInterfaceChannels)).Cast<PeakCanBusInterfaceChannels>().ToList();

            // The API function for retrieving the value needs a StringBuilder, although it won't return a string.
            // It uses the StringBuilder as a byte array to output the required value.
            var stringBuffer = new StringBuilder(sizeof(uint));

            var canChannel = PeakCanBusInterfaceChannels.USBBUS1;

            // This will store device ids that were found in any interface channels.
            var foundIds = new List<uint>();

            // Go through all channels.
            foreach (var channel in channelsList)
            {
                // Try to get the device id for the selected channel.
                var res = PCANBasic.GetValue((ushort)channel, TPCANParameter.PCAN_DEVICE_ID, stringBuffer, (uint)stringBuffer.Capacity);

                // If the channel do not have a device connected to it, there will be an error.
                if (res == TPCANStatus.PCAN_ERROR_OK)
                {
                    // If we got here, the channel has a device connected to it.

                    // Convert the string buffer to a byte list.
                    var byteList = stringBuffer.ToString().Select(c => (byte)c).ToList();

                    // We want to convert the byte list to an uint variable.
                    // For this we need to increase the number of bytes in the list until we have exactly the
                    // amount of bytes of an uint variable. We do this by adding zeros.
                    while (byteList.Count < sizeof(uint))
                    {
                        byteList.Add(0);
                    }

                    // Get the id of the device connected to the selected channel, by converting array to uint.
                    var id = BitConverter.ToUInt32(byteList.ToArray(), 0);

                    // Since PCI cards can have many channels associated with only one id, we will increment the id,
                    // if we found a repeated device id. This represents another channel of the same device.
                    while (foundIds.Contains(id))
                    {
                        id++;
                    }

                    foundIds.Add(id);

                    // If the id matches the searched id, the searched device is connected to the selected channel.
                    // We can break the loop and return the channel.
                    if (deviceId == id)
                    {
                        canChannel = channel;
                        break;
                    }
                }
            }

            return canChannel;
        }

        /* =========================================================================================
        // CLASSES
        // ====================================================================================== */

        /// <summary>
        /// This class represents a physical quantity, composed by a value and an unit.
        /// </summary>
        public class ConvertedValue
        {
            /* =====================================================================================
            // PROPERTIES
            // ================================================================================== */

            /// <summary>
            /// Gets or sets the magnitude of the value.
            /// </summary>
            public double Value { get; set; } = 0;

            /// <summary>
            /// Gets or sets the unit of the value.
            /// </summary>
            public string Unit { get; set; } = string.Empty;
        }

        /// <summary>
        /// This class provides a convenience way to initialize an exception for the PXCP library, given
        /// a <see cref="TXCPResult"/> variable, which is the type of the return values of the methods in
        /// the PXCP-API.
        /// </summary>
        public class XcpException : Exception
        {
            /* =====================================================================================
            // CONSTRUCTORS
            // ================================================================================== */

            /// <summary>
            /// Initializes a new instance of the <see cref="XcpException"/> class.
            /// </summary>
            /// <param name="result">The result of a PXCP-API method.</param>
            public XcpException(TXCPResult result)
                : base(GetMessage(result))
            {
            }

            /* =====================================================================================
            // METHODS
            // ================================================================================== */

            /// <summary>
            /// Retrieves a text message describing an error code of the PXCP-API.
            /// </summary>
            /// <param name="result">The result of a PXCP-API method.</param>
            /// <returns>Error message.</returns>
            private static string GetMessage(TXCPResult result)
            {
                StringBuilder strText = new StringBuilder(256);
                XCPApi.GetErrorText(result, strText);
                return strText.ToString();
            }
        }
    }
}
