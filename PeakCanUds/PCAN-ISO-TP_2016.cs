//  PCAN-ISO-TP_2016.cs
//
//  ~~~~~~~~~~~~
//
//  PCAN-ISO-TP API
//
//  ~~~~~~~~~~~~
//
//  ------------------------------------------------------------------
//  Author : Fabrice Vergnaud
//	Last changed by:	$Author: Fabrice $
//  Last changed date:	$Date: 2021-10-26 12:35:54 +0200 (mar., 26 oct. 2021) $
//
//  Language: C#
//  ------------------------------------------------------------------
//
//  Copyright (C) 2020  PEAK-System Technik GmbH, Darmstadt
//  more Info at http://www.peak-system.com 
//

// To use PCAN-ISO-TP_2004 and PCAN-ISO-TP_2016 together: do define PCANTP_API_COMPATIBILITY_ISO_2004"
// #define PCANTP_API_COMPATIBILITY_ISO_2004

// To accept unsafe functions, define UNSAFE
//#define UNSAFE

using System;
using System.Runtime.InteropServices;
using System.Text;

using Peak.Can.Basic;


namespace Peak.Can.IsoTp
{

    ////////////////////////////////////////////////////////////
    // Types definition
    ////////////////////////////////////////////////////////////

    using cantp_bitrate = String;	// Represents a PCAN-FD bit rate string
    using cantp_timestamp = UInt64;  // Timestamp    

    using cantp_pcanstatus = UInt32; // Represents the PCAN error and status codes

    // Reserved extra information

    using cantp_msginfo_flag = UInt32;
    using cantp_msginfo_extra = UIntPtr;
    using cantp_isotp_info = UIntPtr;

    #region Enumerations

    /// <summary>
    /// Represents currently defined and supported PCANTP handle (a.k.a. channels)
    /// </summary>
    public enum cantp_handle : UInt32
    {
        /// <summary>
        /// Undefined/default value for a PCAN bus
        /// </summary>
        PCANTP_HANDLE_NONEBUS = PCANBasic.PCAN_NONEBUS,
        /// <summary>
        /// PCAN-ISA interface, channel 1
        /// </summary>
        PCANTP_HANDLE_ISABUS1 = PCANBasic.PCAN_ISABUS1,
        /// <summary>
        /// PCAN-ISA interface, channel 2
        /// </summary>
        PCANTP_HANDLE_ISABUS2 = PCANBasic.PCAN_ISABUS2,
        /// <summary>
        /// PCAN-ISA interface, channel 3
        /// </summary>
        PCANTP_HANDLE_ISABUS3 = PCANBasic.PCAN_ISABUS3,
        /// <summary>
        /// PCAN-ISA interface, channel 4
        /// </summary>
        PCANTP_HANDLE_ISABUS4 = PCANBasic.PCAN_ISABUS4,
        /// <summary>
        /// PCAN-ISA interface, channel 5
        /// </summary>
        PCANTP_HANDLE_ISABUS5 = PCANBasic.PCAN_ISABUS5,
        /// <summary>
        /// PCAN-ISA interface, channel 6
        /// </summary>
        PCANTP_HANDLE_ISABUS6 = PCANBasic.PCAN_ISABUS6,
        /// <summary>
        /// PCAN-ISA interface, channel 7
        /// </summary>
        PCANTP_HANDLE_ISABUS7 = PCANBasic.PCAN_ISABUS7,
        /// <summary>
        /// PCAN-ISA interface, channel 8
        /// </summary>
        PCANTP_HANDLE_ISABUS8 = PCANBasic.PCAN_ISABUS8,
        /// <summary>
        /// PCAN-Dongle/LPT interface, channel 1
        /// </summary>
        PCANTP_HANDLE_DNGBUS1 = PCANBasic.PCAN_DNGBUS1,
        /// <summary>
        /// PCAN-PCI interface, channel 1
        /// </summary>
        PCANTP_HANDLE_PCIBUS1 = PCANBasic.PCAN_PCIBUS1,
        /// <summary>
        /// PCAN-PCI interface, channel 2
        /// </summary>
        PCANTP_HANDLE_PCIBUS2 = PCANBasic.PCAN_PCIBUS2,
        /// <summary>
        /// PCAN-PCI interface, channel 3
        /// </summary>
        PCANTP_HANDLE_PCIBUS3 = PCANBasic.PCAN_PCIBUS3,
        /// <summary>
        /// PCAN-PCI interface, channel 4
        /// </summary>
        PCANTP_HANDLE_PCIBUS4 = PCANBasic.PCAN_PCIBUS4,
        /// <summary>
        /// PCAN-PCI interface, channel 5
        /// </summary>
        PCANTP_HANDLE_PCIBUS5 = PCANBasic.PCAN_PCIBUS5,
        /// <summary>
        /// PCAN-PCI interface, channel 6
        /// </summary>
        PCANTP_HANDLE_PCIBUS6 = PCANBasic.PCAN_PCIBUS6,
        /// <summary>
        /// PCAN-PCI interface, channel 7
        /// </summary>
        PCANTP_HANDLE_PCIBUS7 = PCANBasic.PCAN_PCIBUS7,
        /// <summary>
        /// PCAN-PCI interface, channel 8
        /// </summary>
        PCANTP_HANDLE_PCIBUS8 = PCANBasic.PCAN_PCIBUS8,
        /// <summary>
        /// PCAN-PCI interface, channel 9
        /// </summary>
        PCANTP_HANDLE_PCIBUS9 = PCANBasic.PCAN_PCIBUS9,
        /// <summary>
        /// PCAN-PCI interface, channel 10
        /// </summary>
        PCANTP_HANDLE_PCIBUS10 = PCANBasic.PCAN_PCIBUS10,
        /// <summary>
        /// PCAN-PCI interface, channel 11
        /// </summary>
        PCANTP_HANDLE_PCIBUS11 = PCANBasic.PCAN_PCIBUS11,
        /// <summary>
        /// PCAN-PCI interface, channel 12
        /// </summary>
        PCANTP_HANDLE_PCIBUS12 = PCANBasic.PCAN_PCIBUS12,
        /// <summary>
        /// PCAN-PCI interface, channel 13
        /// </summary>
        PCANTP_HANDLE_PCIBUS13 = PCANBasic.PCAN_PCIBUS13,
        /// <summary>
        /// PCAN-PCI interface, channel 14
        /// </summary>
        PCANTP_HANDLE_PCIBUS14 = PCANBasic.PCAN_PCIBUS14,
        /// <summary>
        /// PCAN-PCI interface, channel 15
        /// </summary>
        PCANTP_HANDLE_PCIBUS15 = PCANBasic.PCAN_PCIBUS15,
        /// <summary>
        /// PCAN-PCI interface, channel 16
        /// </summary>
        PCANTP_HANDLE_PCIBUS16 = PCANBasic.PCAN_PCIBUS16,
        /// <summary>
        /// PCAN-USB interface, channel 1
        /// </summary>
        PCANTP_HANDLE_USBBUS1 = PCANBasic.PCAN_USBBUS1,
        /// <summary>
        /// PCAN-USB interface, channel 2
        /// </summary>
        PCANTP_HANDLE_USBBUS2 = PCANBasic.PCAN_USBBUS2,
        /// <summary>
        /// PCAN-USB interface, channel 3
        /// </summary>
        PCANTP_HANDLE_USBBUS3 = PCANBasic.PCAN_USBBUS3,
        /// <summary>
        /// PCAN-USB interface, channel 4
        /// </summary>
        PCANTP_HANDLE_USBBUS4 = PCANBasic.PCAN_USBBUS4,
        /// <summary>
        /// PCAN-USB interface, channel 5
        /// </summary>
        PCANTP_HANDLE_USBBUS5 = PCANBasic.PCAN_USBBUS5,
        /// <summary>
        /// PCAN-USB interface, channel 6
        /// </summary>
        PCANTP_HANDLE_USBBUS6 = PCANBasic.PCAN_USBBUS6,
        /// <summary>
        /// PCAN-USB interface, channel 7
        /// </summary>
        PCANTP_HANDLE_USBBUS7 = PCANBasic.PCAN_USBBUS7,
        /// <summary>
        /// PCAN-USB interface, channel 8
        /// </summary>
        PCANTP_HANDLE_USBBUS8 = PCANBasic.PCAN_USBBUS8,
        /// <summary>
        /// PCAN-USB interface, channel 9
        /// </summary>
        PCANTP_HANDLE_USBBUS9 = PCANBasic.PCAN_USBBUS9,
        /// <summary>
        /// PCAN-USB interface, channel 10
        /// </summary>
        PCANTP_HANDLE_USBBUS10 = PCANBasic.PCAN_USBBUS10,
        /// <summary>
        /// PCAN-USB interface, channel 11
        /// </summary>
        PCANTP_HANDLE_USBBUS11 = PCANBasic.PCAN_USBBUS11,
        /// <summary>
        /// PCAN-USB interface, channel 12
        /// </summary>
        PCANTP_HANDLE_USBBUS12 = PCANBasic.PCAN_USBBUS12,
        /// <summary>
        /// PCAN-USB interface, channel 13
        /// </summary>
        PCANTP_HANDLE_USBBUS13 = PCANBasic.PCAN_USBBUS13,
        /// <summary>
        /// PCAN-USB interface, channel 14
        /// </summary>
        PCANTP_HANDLE_USBBUS14 = PCANBasic.PCAN_USBBUS14,
        /// <summary>
        /// PCAN-USB interface, channel 15
        /// </summary>
        PCANTP_HANDLE_USBBUS15 = PCANBasic.PCAN_USBBUS15,
        /// <summary>
        /// PCAN-USB interface, channel 16
        /// </summary>
        PCANTP_HANDLE_USBBUS16 = PCANBasic.PCAN_USBBUS16,
        /// <summary>
        /// PCAN-PC Card interface, channel 1
        /// </summary>
        PCANTP_HANDLE_PCCBUS1 = PCANBasic.PCAN_PCCBUS1,
        /// <summary>
        /// PCAN-PC Card interface, channel 2
        /// </summary>
        PCANTP_HANDLE_PCCBUS2 = PCANBasic.PCAN_PCCBUS2,
        /// <summary>
        /// PCAN-LAN interface, channel 1
        /// </summary>
        PCANTP_HANDLE_LANBUS1 = PCANBasic.PCAN_LANBUS1,
        /// <summary>
        /// PCAN-LAN interface, channel 2
        /// </summary>
        PCANTP_HANDLE_LANBUS2 = PCANBasic.PCAN_LANBUS2,
        /// <summary>
        /// PCAN-LAN interface, channel 3
        /// </summary>
        PCANTP_HANDLE_LANBUS3 = PCANBasic.PCAN_LANBUS3,
        /// <summary>
        /// PCAN-LAN interface, channel 4
        /// </summary>
        PCANTP_HANDLE_LANBUS4 = PCANBasic.PCAN_LANBUS4,
        /// <summary>
        /// PCAN-LAN interface, channel 5
        /// </summary>
        PCANTP_HANDLE_LANBUS5 = PCANBasic.PCAN_LANBUS5,
        /// <summary>
        /// PCAN-LAN interface, channel 6
        /// </summary>
        PCANTP_HANDLE_LANBUS6 = PCANBasic.PCAN_LANBUS6,
        /// <summary>
        /// PCAN-LAN interface, channel 7
        /// </summary>
        PCANTP_HANDLE_LANBUS7 = PCANBasic.PCAN_LANBUS7,
        /// <summary>
        /// PCAN-LAN interface, channel 8
        /// </summary>
        PCANTP_HANDLE_LANBUS8 = PCANBasic.PCAN_LANBUS8,
        /// <summary>
        /// PCAN-LAN interface, channel 9
        /// </summary>
        PCANTP_HANDLE_LANBUS9 = PCANBasic.PCAN_LANBUS9,
        /// <summary>
        /// PCAN-LAN interface, channel 10
        /// </summary>
        PCANTP_HANDLE_LANBUS10 = PCANBasic.PCAN_LANBUS10,
        /// <summary>
        /// PCAN-LAN interface, channel 11
        /// </summary>
        PCANTP_HANDLE_LANBUS11 = PCANBasic.PCAN_LANBUS11,
        /// <summary>
        /// PCAN-LAN interface, channel 12
        /// </summary>
        PCANTP_HANDLE_LANBUS12 = PCANBasic.PCAN_LANBUS12,
        /// <summary>
        /// PCAN-LAN interface, channel 13
        /// </summary>
        PCANTP_HANDLE_LANBUS13 = PCANBasic.PCAN_LANBUS13,
        /// <summary>
        /// PCAN-LAN interface, channel 14
        /// </summary>
        PCANTP_HANDLE_LANBUS14 = PCANBasic.PCAN_LANBUS14,
        /// <summary>
        /// PCAN-LAN interface, channel 15
        /// </summary>
        PCANTP_HANDLE_LANBUS15 = PCANBasic.PCAN_LANBUS15,
        /// <summary>
        /// PCAN-LAN interface, channel 16
        /// </summary>
        PCANTP_HANDLE_LANBUS16 = PCANBasic.PCAN_LANBUS16,
    }

    /// <summary>
    /// Represents the baudrate register for the PCANTP channel 
    /// </summary>
    public enum cantp_baudrate : UInt32
    {
        /// <summary>
        ///  Channel Baudrate 1 MBit/s
        /// </summary>
        PCANTP_BAUDRATE_1M = TPCANBaudrate.PCAN_BAUD_1M,
        /// <summary>
        ///  Channel Baudrate 800 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_800K = TPCANBaudrate.PCAN_BAUD_800K,
        /// <summary>
        ///  Channel Baudrate 500 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_500K = TPCANBaudrate.PCAN_BAUD_500K,
        /// <summary>
        ///  Channel Baudrate 250 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_250K = TPCANBaudrate.PCAN_BAUD_250K,
        /// <summary>
        ///  Channel Baudrate 125 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_125K = TPCANBaudrate.PCAN_BAUD_125K,
        /// <summary>
        ///  Channel Baudrate 100 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_100K = TPCANBaudrate.PCAN_BAUD_100K,
        /// <summary>
        ///  Channel Baudrate 95,238 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_95K = TPCANBaudrate.PCAN_BAUD_95K,
        /// <summary>
        ///  Channel Baudrate 83,333 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_83K = TPCANBaudrate.PCAN_BAUD_83K,
        /// <summary>
        ///  Channel Baudrate 50 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_50K = TPCANBaudrate.PCAN_BAUD_50K,
        /// <summary>
        ///  Channel Baudrate 47,619 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_47K = TPCANBaudrate.PCAN_BAUD_47K,
        /// <summary>
        ///  Channel Baudrate 33,333 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_33K = TPCANBaudrate.PCAN_BAUD_33K,
        /// <summary>
        ///  Channel Baudrate 20 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_20K = TPCANBaudrate.PCAN_BAUD_20K,
        /// <summary>
        ///  Channel Baudrate 10 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_10K = TPCANBaudrate.PCAN_BAUD_10K,
        /// <summary>
        ///  Channel Baudrate 5 kBit/s
        /// </summary>
        PCANTP_BAUDRATE_5K = TPCANBaudrate.PCAN_BAUD_5K,
    }

    /// <summary>
    /// Type of PCAN (non plug-n-play) hardware
    /// </summary>
    public enum cantp_hwtype : UInt32
    {
        /// <summary>
        /// PCAN-ISA 82C200
        /// </summary>
        PCANTP_HWTYPE_ISA = TPCANType.PCAN_TYPE_ISA,
        /// <summary>
        /// PCAN-ISA SJA1000
        /// </summary>
        PCANTP_HWTYPE_ISA_SJA = TPCANType.PCAN_TYPE_ISA_SJA,
        /// <summary>
        /// PHYTEC ISA 
        /// </summary>
        PCANTP_HWTYPE_ISA_PHYTEC = TPCANType.PCAN_TYPE_ISA_PHYTEC,
        /// <summary>
        /// PCAN-Dongle 82C200
        /// </summary>
        PCANTP_HWTYPE_DNG = TPCANType.PCAN_TYPE_DNG,
        /// <summary>
        /// PCAN-Dongle EPP 82C200
        /// </summary>
        PCANTP_HWTYPE_DNG_EPP = TPCANType.PCAN_TYPE_DNG_EPP,
        /// <summary>
        /// PCAN-Dongle SJA1000
        /// </summary>
        PCANTP_HWTYPE_DNG_SJA = TPCANType.PCAN_TYPE_DNG_SJA,
        /// <summary>
        /// PCAN-Dongle EPP SJA1000
        /// </summary>
        PCANTP_HWTYPE_DNG_SJA_EPP = TPCANType.PCAN_TYPE_DNG_SJA_EPP,
    }

    /// <summary>
    /// PCAN devices
    /// </summary>
    public enum cantp_device : UInt32
    {
        /// <summary>
        /// Undefined, unknown or not selected PCAN device value
        /// </summary>
        PCANTP_DEVICE_NONE = TPCANDevice.PCAN_NONE,
        /// <summary>
        /// PCAN Non-Plug&Play devices. NOT USED WITHIN PCAN-Basic API
        /// </summary>
        PCANTP_DEVICE_PEAKCAN = TPCANDevice.PCAN_PEAKCAN,
        /// <summary>
        /// PCAN-ISA, PCAN-PC/104, and PCAN-PC/104-Plus
        /// </summary>
        PCANTP_DEVICE_ISA = TPCANDevice.PCAN_ISA,
        /// <summary>
        /// PCAN-Dongle
        /// </summary>
        PCANTP_DEVICE_DNG = TPCANDevice.PCAN_DNG,
        /// <summary>
        /// PCAN-PCI, PCAN-cPCI, PCAN-miniPCI, and PCAN-PCI Express
        /// </summary>
        PCANTP_DEVICE_PCI = TPCANDevice.PCAN_PCI,
        /// <summary>
        /// PCAN-USB and PCAN-USB Pro
        /// </summary>
        PCANTP_DEVICE_USB = TPCANDevice.PCAN_USB,
        /// <summary>
        /// PCAN-PC Card
        /// </summary>
        PCANTP_DEVICE_PCC = TPCANDevice.PCAN_PCC,
        /// <summary>
        /// PCAN Virtual hardware. NOT USED WITHIN PCAN-Basic API
        /// </summary>
        PCANTP_DEVICE_VIRTUAL = TPCANDevice.PCAN_VIRTUAL,
        /// <summary>
        /// PCAN Gateway devices
        /// </summary>
        PCANTP_DEVICE_LAN = TPCANDevice.PCAN_LAN,
    }

    ////////////////////////////////////////////////////////////
    // Enums definition for ISO-TP API
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// Represents each group of errors a status can hold
    /// </summary>
    [Flags]
    public enum cantp_statustype : UInt32
    {
        /// <summary>
        /// no error
        /// </summary>
        PCANTP_STATUSTYPE_OK = 0x00,
        /// <summary>
        /// general error
        /// </summary>
        PCANTP_STATUSTYPE_ERR = 0x01,
        /// <summary>
        /// bus status
        /// </summary>
        PCANTP_STATUSTYPE_BUS = 0x02,
        /// <summary>
        /// network status
        /// </summary>
        PCANTP_STATUSTYPE_NET = 0x04,
        /// <summary>
        /// extra information
        /// </summary>
        PCANTP_STATUSTYPE_INFO = 0x08,
        /// <summary>
        /// encapsulated PCAN-Basic status
        /// </summary>
        PCANTP_STATUSTYPE_PCAN = 0x10,
    }

    /// <summary>
    /// Represents the network result of the communication of an ISO-TP message (used in cantp_status).
    /// </summary>
    public enum cantp_netstatus : UInt32
    {
        // ISO-TP network errors:
        /// <summary>
        /// No network error
        /// </summary>
        PCANTP_NETSTATUS_OK = 0x00,
        /// <summary>
        /// timeout occured between 2 frames transmission (sender and receiver side)
        /// </summary>
        PCANTP_NETSTATUS_TIMEOUT_A = 0x01,
        /// <summary>
        /// sender side timeout while waiting for flow control frame
        /// </summary>
        PCANTP_NETSTATUS_TIMEOUT_Bs = 0x02,
        /// <summary>
        /// receiver side timeout while waiting for consecutive frame
        /// </summary>
        PCANTP_NETSTATUS_TIMEOUT_Cr = 0x03,
        /// <summary>
        /// unexpected sequence number
        /// </summary>
        PCANTP_NETSTATUS_WRONG_SN = 0x04,
        /// <summary>
        /// invalid or unknown FlowStatus
        /// </summary>
        PCANTP_NETSTATUS_INVALID_FS = 0x05,
        /// <summary>
        /// unexpected protocol data unit
        /// </summary>
        PCANTP_NETSTATUS_UNEXP_PDU = 0x06,
        /// <summary>
        /// reception of flow control WAIT frame that exceeds the maximum counter defined by PCANTP_PARAMETER_WFT_MAX
        /// </summary>
        PCANTP_NETSTATUS_WFT_OVRN = 0x07,
        /// <summary>
        /// buffer on the receiver side cannot store the data length (server side only)
        /// </summary>
        PCANTP_NETSTATUS_BUFFER_OVFLW = 0x08,
        /// <summary>
        /// general error
        /// </summary>
        PCANTP_NETSTATUS_ERROR = 0x09,
        /// <summary>
        /// message was invalid and ignored
        /// </summary>
        PCANTP_NETSTATUS_IGNORED = 0x0A,
        /// <summary>
        /// sender side timeout while transmitting
        /// </summary>
        PCANTP_NETSTATUS_TIMEOUT_As = 0x0B,
        /// <summary>
        /// receiver side timeout while transmitting
        /// </summary>     
        PCANTP_NETSTATUS_TIMEOUT_Ar = 0x0C,

        // NON ISO-TP related network results:
        /// <summary>
        /// transmit queue is full (failed too many times)
        /// </summary>
        PCANTP_NETSTATUS_XMT_FULL = 0x0D,
        /// <summary>
        /// CAN bus error
        /// </summary>
        PCANTP_NETSTATUS_BUS_ERROR = 0x0E,
        /// <summary>
        /// memory allocation error
        /// </summary>
        PCANTP_NETSTATUS_NO_MEMORY = 0x0F,
    }

    /// <summary>
    /// Represents the status of a CAN bus (used in cantp_status).
    /// </summary>
    [Flags]
    public enum cantp_busstatus : UInt32
    {
        /// <summary>
        /// Bus is in active state
        /// </summary>
        PCANTP_BUSSTATUS_OK = 0x00,
        /// <summary>
        /// Bus error: an error counter reached the 'light' limit
        /// </summary>
        PCANTP_BUSSTATUS_LIGHT = 0x01,
        /// <summary>
        /// Bus error: an error counter reached the 'heavy' limit
        /// </summary>
        PCANTP_BUSSTATUS_HEAVY = 0x02,
        /// <summary>
        /// Bus error: an error counter reached the 'warning/heavy' limit
        /// </summary>
        PCANTP_BUSSTATUS_WARNING = PCANTP_BUSSTATUS_HEAVY,
        /// <summary>
        /// Bus error: the CAN controller is error passive
        /// </summary>
        PCANTP_BUSSTATUS_PASSIVE = 0x04,
        /// <summary>
        /// Bus error: the CAN controller is in bus-off state
        /// </summary>
        PCANTP_BUSSTATUS_OFF = 0x08,
        /// <summary>
        /// Mask for all bus errors 
        /// </summary>
        PCANTP_BUSSTATUS_ANY = PCANTP_BUSSTATUS_LIGHT | PCANTP_BUSSTATUS_HEAVY | PCANTP_BUSSTATUS_WARNING | PCANTP_BUSSTATUS_PASSIVE | PCANTP_BUSSTATUS_OFF,
    }

    /// <summary>
    /// Represents an general error (used in cantp_status).
    /// </summary>
    public enum cantp_errstatus : UInt32
    {
        /// <summary>
        /// No error 
        /// </summary>
        PCANTP_ERRSTATUS_OK = 0x00,
        /// <summary>
        /// Not Initialized
        /// </summary>
        PCANTP_ERRSTATUS_NOT_INITIALIZED = 0x01,
        /// <summary>
        /// Already Initialized
        /// </summary>
        PCANTP_ERRSTATUS_ALREADY_INITIALIZED = 0x02,
        /// <summary>
        /// Could not obtain memory
        /// </summary>
        PCANTP_ERRSTATUS_NO_MEMORY = 0x03,
        /// <summary>
        /// Input buffer overflow
        /// </summary>
        PCANTP_ERRSTATUS_OVERFLOW = 0x04,
        /// <summary>
        /// No Message available
        /// </summary>
        PCANTP_ERRSTATUS_NO_MESSAGE = 0x07,
        /// <summary>
        /// Parameter has an invalid or unexpected type
        /// </summary>
        PCANTP_ERRSTATUS_PARAM_INVALID_TYPE = 0x08,
        /// <summary>
        /// Parameter has an invalid value
        /// </summary>
        PCANTP_ERRSTATUS_PARAM_INVALID_VALUE = 0x09,
        /// <summary>
        /// PCANTP mapping not initialized
        /// </summary>
        PCANTP_ERRSTATUS_MAPPING_NOT_INITIALIZED = 0x0D,
        /// <summary>
        /// PCANTP mapping parameters are invalid
        /// </summary>
        PCANTP_ERRSTATUS_MAPPING_INVALID = 0x0E,
        /// <summary>
        /// PCANTP mapping already defined
        /// </summary>
        PCANTP_ERRSTATUS_MAPPING_ALREADY_INITIALIZED = 0x0F,
        /// <summary>
        /// 
        /// </summary>
        PCANTP_ERRSTATUS_PARAM_BUFFER_TOO_SMALL = 0x10,
        /// <summary>
        /// Tx queue is full
        /// </summary>
        PCANTP_ERRSTATUS_QUEUE_TX_FULL = 0x11,
        /// <summary>
        /// Failed to get an access to the internal lock
        /// </summary>
        PCANTP_ERRSTATUS_LOCK_TIMEOUT = 0x12,
        /// <summary>
        /// Invalid cantp_handle
        /// </summary>
        PCANTP_ERRSTATUS_INVALID_HANDLE = 0x13,

        /// <summary>
        /// unknown/generic error
        /// </summary>
        PCANTP_ERRSTATUS_UNKNOWN = 0xFF,
    }

    /// <summary>
    /// Represents additional status information (used in cantp_status).
    /// </summary>
    [Flags]
    public enum cantp_infostatus : UInt32
    {
        /// <summary>
        /// no extra information
        /// </summary>
        PCANTP_INFOSTATUS_OK = 0x00,
        /// <summary>
        /// input was modified by the API
        /// </summary>
        PCANTP_INFOSTATUS_CAUTION_INPUT_MODIFIED = 0x01,
        /// <summary>
        /// DLC value was modified by the API 
        /// </summary>
        PCANTP_INFOSTATUS_CAUTION_DLC_MODIFIED = 0x02,
        /// <summary>
        /// Data Length value was modified by the API 
        /// </summary>
        PCANTP_INFOSTATUS_CAUTION_DATA_LENGTH_MODIFIED = 0x04,
        /// <summary>
        /// FD related flags value were modified by the API 
        /// </summary>
        PCANTP_INFOSTATUS_CAUTION_FD_FLAG_MODIFIED = 0x08,
        /// <summary>
        /// Message receive queue is full (oldest messages may be lost)
        /// </summary>
        PCANTP_INFOSTATUS_CAUTION_RX_QUEUE_FULL = 0x10,
        /// <summary>
        /// Buffer is used by another thread or API
        /// </summary>
        PCANTP_INFOSTATUS_CAUTION_BUFFER_IN_USE = 0x20,
    }

    /// <summary>
    /// Defines constants used by the next enum: cantp_status
    /// </summary>
    public enum cantp_status_offset : byte
    {
        PCANTP_STATUS_OFFSET_BUS = 8,
        PCANTP_STATUS_OFFSET_NET = (PCANTP_STATUS_OFFSET_BUS + 5),
        PCANTP_STATUS_OFFSET_INFO = (PCANTP_STATUS_OFFSET_NET + 5),
    }

    /// <summary>
    ///  Represent the PCANTP error and status codes .
    ///   
    /// Bits information:
    ///   32|  28|  24|  20|  16|  12|   8|   4|   0|
    ///     |    |    |    |    |    |    |    |    |
    ///      0000 0000 0000 0000 0000 0000 0000 0000 
    ///     |    |    |    |    |         [0000 0000] => PCAN-ISO-TP API errors
    ///     |    |    |    |    |  [0 0000]           => CAN Bus status
    ///     |    |    |    | [00 000]                 => Networking message status
    ///     |    |    [0000 00]                       => API extra information
    ///     |[000 0000]                               => Reserved
    ///     [0]                                       => PCANBasic error flag (overrides the meaning of all bits)
    /// </summary>
    public enum cantp_status : UInt32
    {
        PCANTP_STATUS_OK = cantp_errstatus.PCANTP_ERRSTATUS_OK,
        PCANTP_STATUS_NOT_INITIALIZED = cantp_errstatus.PCANTP_ERRSTATUS_NOT_INITIALIZED,
        PCANTP_STATUS_ALREADY_INITIALIZED = cantp_errstatus.PCANTP_ERRSTATUS_ALREADY_INITIALIZED,
        PCANTP_STATUS_NO_MEMORY = cantp_errstatus.PCANTP_ERRSTATUS_NO_MEMORY,
        PCANTP_STATUS_OVERFLOW = cantp_errstatus.PCANTP_ERRSTATUS_OVERFLOW,
        PCANTP_STATUS_NO_MESSAGE = cantp_errstatus.PCANTP_ERRSTATUS_NO_MESSAGE,
        PCANTP_STATUS_PARAM_INVALID_TYPE = cantp_errstatus.PCANTP_ERRSTATUS_PARAM_INVALID_TYPE,
        PCANTP_STATUS_PARAM_INVALID_VALUE = cantp_errstatus.PCANTP_ERRSTATUS_PARAM_INVALID_VALUE,
        PCANTP_STATUS_MAPPING_NOT_INITIALIZED = cantp_errstatus.PCANTP_ERRSTATUS_MAPPING_NOT_INITIALIZED,
        PCANTP_STATUS_MAPPING_INVALID = cantp_errstatus.PCANTP_ERRSTATUS_MAPPING_INVALID,
        PCANTP_STATUS_MAPPING_ALREADY_INITIALIZED = cantp_errstatus.PCANTP_ERRSTATUS_MAPPING_ALREADY_INITIALIZED,
        PCANTP_STATUS_PARAM_BUFFER_TOO_SMALL = cantp_errstatus.PCANTP_ERRSTATUS_PARAM_BUFFER_TOO_SMALL,
        PCANTP_STATUS_QUEUE_TX_FULL = cantp_errstatus.PCANTP_ERRSTATUS_QUEUE_TX_FULL,
        PCANTP_STATUS_LOCK_TIMEOUT = cantp_errstatus.PCANTP_ERRSTATUS_LOCK_TIMEOUT,
        PCANTP_STATUS_HANDLE_INVALID = cantp_errstatus.PCANTP_ERRSTATUS_INVALID_HANDLE,
        PCANTP_STATUS_UNKNOWN = cantp_errstatus.PCANTP_ERRSTATUS_UNKNOWN,

        #region Bus status flags (bits [8..11])
        /// <summary>
        /// PCANTP Channel is in BUS-LIGHT error state
        /// </summary>
        PCANTP_STATUS_FLAG_BUS_LIGHT = (cantp_busstatus.PCANTP_BUSSTATUS_LIGHT << cantp_status_offset.PCANTP_STATUS_OFFSET_BUS),
        /// <summary>
        /// PCANTP Channel is in BUS-HEAVY error state
        /// </summary>
        PCANTP_STATUS_FLAG_BUS_HEAVY = (cantp_busstatus.PCANTP_BUSSTATUS_HEAVY << cantp_status_offset.PCANTP_STATUS_OFFSET_BUS),
        /// <summary>
        /// PCANTP Channel is in BUS-HEAVY error state
        /// </summary>
        PCANTP_STATUS_FLAG_BUS_WARNING = PCANTP_STATUS_FLAG_BUS_HEAVY,
        /// <summary>
        /// PCANTP Channel is error passive state
        /// </summary>
        PCANTP_STATUS_FLAG_BUS_PASSIVE = (cantp_busstatus.PCANTP_BUSSTATUS_PASSIVE << cantp_status_offset.PCANTP_STATUS_OFFSET_BUS),
        /// <summary>
        /// PCANTP Channel is in BUS-OFF error state
        /// </summary>
        PCANTP_STATUS_FLAG_BUS_OFF = (cantp_busstatus.PCANTP_BUSSTATUS_OFF << cantp_status_offset.PCANTP_STATUS_OFFSET_BUS),
        /// <summary>
        /// 
        /// </summary>
        PCANTP_STATUS_FLAG_BUS_ANY = (cantp_busstatus.PCANTP_BUSSTATUS_ANY << cantp_status_offset.PCANTP_STATUS_OFFSET_BUS),
        #endregion

        #region Network status (bits [13..17])
        /// <summary>
        /// This flag states if one of the following network errors occured with the fetched message
        /// </summary>
        PCANTP_STATUS_FLAG_NETWORK_RESULT = (1 << cantp_status_offset.PCANTP_STATUS_OFFSET_NET),
        /// <summary>
        /// timeout occured between 2 frames transmission (sender and receiver side)
        /// </summary>
        PCANTP_STATUS_NETWORK_TIMEOUT_A = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_TIMEOUT_A << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// sender side timeout while waiting for flow control frame
        /// </summary>
        PCANTP_STATUS_NETWORK_TIMEOUT_Bs = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_TIMEOUT_Bs << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// receiver side timeout while waiting for consecutive frame
        /// </summary>
        PCANTP_STATUS_NETWORK_TIMEOUT_Cr = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_TIMEOUT_Cr << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// unexpected sequence number
        /// </summary>
        PCANTP_STATUS_NETWORK_WRONG_SN = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_WRONG_SN << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// invalid or unknown FlowStatus
        /// </summary>
        PCANTP_STATUS_NETWORK_INVALID_FS = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_INVALID_FS << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// unexpected protocol data unit
        /// </summary>
        PCANTP_STATUS_NETWORK_UNEXP_PDU = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_UNEXP_PDU << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// reception of flow control WAIT frame that exceeds the maximum counter defined by PCANTP_PARAMETER_WFT_MAX
        /// </summary>
        PCANTP_STATUS_NETWORK_WFT_OVRN = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_WFT_OVRN << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// buffer on the receiver side cannot store the data length (server side only)
        /// </summary>
        PCANTP_STATUS_NETWORK_BUFFER_OVFLW = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_BUFFER_OVFLW << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// general error
        /// </summary>
        PCANTP_STATUS_NETWORK_ERROR = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_ERROR << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// message was invalid and ignored
        /// </summary>
        PCANTP_STATUS_NETWORK_IGNORED = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_IGNORED << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        #endregion

        #region ISO-TP extra information flags
        /// <summary>
        /// Receiver side timeout while transmitting
        /// </summary>
        PCANTP_STATUS_NETWORK_TIMEOUT_Ar = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_TIMEOUT_Ar << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// Sender side timeout while transmitting
        /// </summary>
        PCANTP_STATUS_NETWORK_TIMEOUT_As = (PCANTP_STATUS_FLAG_NETWORK_RESULT | (cantp_netstatus.PCANTP_NETSTATUS_TIMEOUT_As << (cantp_status_offset.PCANTP_STATUS_OFFSET_NET + 1))),
        /// <summary>
        /// input was modified 
        /// </summary>
        PCANTP_STATUS_CAUTION_INPUT_MODIFIED = (cantp_infostatus.PCANTP_INFOSTATUS_CAUTION_INPUT_MODIFIED << cantp_status_offset.PCANTP_STATUS_OFFSET_INFO),
        /// <summary>
        /// DLC value of the input was modified
        /// </summary>
        PCANTP_STATUS_CAUTION_DLC_MODIFIED = (cantp_infostatus.PCANTP_INFOSTATUS_CAUTION_DLC_MODIFIED << cantp_status_offset.PCANTP_STATUS_OFFSET_INFO),
        /// <summary>
        /// Data Length value of the input was modified 
        /// </summary>
        PCANTP_STATUS_CAUTION_DATA_LENGTH_MODIFIED = (cantp_infostatus.PCANTP_INFOSTATUS_CAUTION_DATA_LENGTH_MODIFIED << cantp_status_offset.PCANTP_STATUS_OFFSET_INFO),
        /// <summary>
        /// FD flags of the input was modified 
        /// </summary>
        PCANTP_STATUS_CAUTION_FD_FLAG_MODIFIED = (cantp_infostatus.PCANTP_INFOSTATUS_CAUTION_FD_FLAG_MODIFIED << cantp_status_offset.PCANTP_STATUS_OFFSET_INFO),
        /// <summary>
        /// Receive queue is full
        /// </summary>
        PCANTP_STATUS_CAUTION_RX_QUEUE_FULL = (cantp_infostatus.PCANTP_INFOSTATUS_CAUTION_RX_QUEUE_FULL << cantp_status_offset.PCANTP_STATUS_OFFSET_INFO),
        /// <summary>
        /// Buffer is used by another thread or API
        /// </summary>
        PCANTP_STATUS_CAUTION_BUFFER_IN_USE = (cantp_infostatus.PCANTP_INFOSTATUS_CAUTION_BUFFER_IN_USE << cantp_status_offset.PCANTP_STATUS_OFFSET_INFO),
        #endregion

        #region Lower API status code: see also PCANTP_STATUS_xx macros
        /// <summary>
        /// PCAN error flag, remove flag to get a usable PCAN error/status code (cf. PCANBasic API)
        /// </summary>
        PCANTP_STATUS_FLAG_PCAN_STATUS = 0x80000000U,
        #endregion

        #region Masks to merge/retrieve different PCANTP status by type in a cantp_status
        /// <summary>
        /// filter by PCANTP_STATUSTYPE_ERR type
        /// </summary>
        PCANTP_STATUS_MASK_ERROR = 0x000000FFU,
        /// <summary>
        /// filter by PCANTP_STATUSTYPE_BUS type
        /// </summary>
        PCANTP_STATUS_MASK_BUS = 0x00001F00U,
        /// <summary>
        /// filter by PCANTP_STATUSTYPE_NET type
        /// </summary>
        PCANTP_STATUS_MASK_ISOTP_NET = 0x0003E000U,
        /// <summary>
        /// filter by PCANTP_STATUSTYPE_INFO type
        /// </summary>
        PCANTP_STATUS_MASK_INFO = 0x00FC0000U,
        /// <summary>
        /// filter by PCANTP_STATUSTYPE_PCAN type
        /// </summary>
        PCANTP_STATUS_MASK_PCAN = ~PCANTP_STATUS_FLAG_PCAN_STATUS,
        #endregion
    }

    /// <summary>
    /// List of parameters handled by PCAN-ISO-TP (rev. 2016)
    /// Note: PCAN-Basic parameters (PCAN_PARAM_xxx) are compatible via casting.
    /// </summary>
    public enum cantp_parameter : UInt32
    {
        /// <summary>
        /// PCAN-ISO-TP API version parameter
        /// </summary>
        PCANTP_PARAMETER_API_VERSION = 0x101,
        /// <summary>
        /// 1 BYTE data describing the condition of a channel. 
        /// </summary>
        PCANTP_PARAMETER_CHANNEL_CONDITION = 0x102,
        /// <summary>
        /// 1 BYTE data describing the debug mode 
        /// </summary>
        PCANTP_PARAMETER_DEBUG = 0x103,
        /// <summary>
        /// data is pointer to a HANDLE created by CreateEvent function
        /// </summary>
        PCANTP_PARAMETER_RECEIVE_EVENT = 0x104,

        /// <summary>
        /// 1 BYTE data stating if unsegmented (NON-ISO-TP) CAN frames can be received
        /// </summary>
        PCANTP_PARAMETER_FRAME_FILTERING = 0x105,
        /// <summary>
        /// 1 BYTE data stating the default DLC to use when transmitting messages with CAN FD
        /// </summary>
        PCANTP_PARAMETER_CAN_TX_DL = 0x106,
        /// <summary>
        /// 1 BYTE data stating if CAN frame DLC uses padding or not
        /// </summary>
        PCANTP_PARAMETER_CAN_DATA_PADDING = 0x107,
        /// <summary>
        /// 1 BYTE data stating the value used for CAN data padding
        /// </summary>
        PCANTP_PARAMETER_CAN_PADDING_VALUE = 0x108,
        /// <summary>
        /// 1 BYTE data stating which revision of ISO 15765-2 to use (see PCANTP_ISO_REV_*).
        /// </summary>
        PCANTP_PARAMETER_ISO_REV = 0x109,
        /// <summary>
        /// 1 BYTE data stating the default priority value for normal fixed, mixed and enhanced addressing (default=6)
        /// </summary>
        PCANTP_PARAMETER_J1939_PRIORITY = 0x10A,
        /// <summary>
        /// 1 BYTE data stating if pending messages are displayed/hidden
        /// </summary>
        PCANTP_PARAMETER_MSG_PENDING = 0x10B,

        /// <summary>
        /// 1 BYTE data describing the block size parameter (BS)
        /// </summary>
        PCANTP_PARAMETER_BLOCK_SIZE = 0x10C,
        /// <summary>
        /// 2 BYTE data describing the transmit block size parameter (BS_TX)
        /// </summary>
        PCANTP_PARAMETER_BLOCK_SIZE_TX = 0x10D,
        /// <summary>
        /// 1 BYTE data describing the seperation time parameter (STmin)
        /// </summary>
        PCANTP_PARAMETER_SEPARATION_TIME = 0x10E,
        /// <summary>
        /// 2 BYTE data describing the transmit seperation time parameter (STmin_TX)
        /// </summary>
        PCANTP_PARAMETER_SEPARATION_TIME_TX = 0x10F,
        /// <summary>
        /// 4 BYTE data describing the Wait Frame Transmissions parameter. 
        /// </summary>
        PCANTP_PARAMETER_WFT_MAX = 0x110,

        /// <summary>
        /// 4 BYTE data describing ISO-15765-2:Timeout As. 
        /// </summary>
        PCANTP_PARAMETER_TIMEOUT_AS = 0x111,
        /// <summary>
        /// 4 BYTE data describing ISO-15765-2:Timeout Ar. 
        /// </summary>
        PCANTP_PARAMETER_TIMEOUT_AR = 0x112,
        /// <summary>
        /// 4 BYTE data describing ISO-15765-2:Timeout Bs. 
        /// </summary>
        PCANTP_PARAMETER_TIMEOUT_BS = 0x113,
        /// <summary>
        /// 4 BYTE data describing ISO-15765-2:Timeout Cr. 
        /// </summary>
        PCANTP_PARAMETER_TIMEOUT_CR = 0x114,
        /// <summary>
        /// 1 BYTE data describing the tolerence to apply to all timeout as a percentage ([0..100]. 
        /// </summary>
        PCANTP_PARAMETER_TIMEOUT_TOLERANCE = 0x115,
        /// <summary>
        /// 1 BYTE data to set predefined ISO values for timeouts (see PCANTP_ISO_TIMEOUTS_*).
        /// </summary>
        PCANTP_PARAMETER_ISO_TIMEOUTS = 0x116,
        /// <summary>
        /// 1 BYTE data to set optimization options to improve delay between ISO-TP consecutive frames.
        /// </summary>
        PCANTP_PARAMETER_SELFRECEIVE_LATENCY = 0x117,
        /// <summary>
        /// 2 BYTE data describing the maximum number of messages in the Rx queue.
        /// </summary>
        PCANTP_PARAMETER_MAX_RX_QUEUE = 0x118,
        /// <summary>
        /// 1 BYTE data stating if messages handled by higher layer APIs are still available in this API (default=0).
        /// </summary>
        PCANTP_PARAMETER_KEEP_HIGHER_LAYER_MESSAGES = 0x119,
        /// <summary>
        /// 1 BYTE data stating if the white-list CAN IDs filtering mechanism is enabled.
        /// </summary>
        PCANTP_PARAMETER_FILTER_CAN_ID = 0x11A,
        /// <summary>
        /// 1 BYTE data stating if the 29 bit Enhanced Diagnostic CAN identifier is supported (ISO-15765-3:2004, default is false with ISO revision 2016).
        /// </summary>
        PCANTP_PARAMETER_SUPPORT_29B_ENHANCED = 0x11B,
        /// <summary>
        /// 1 BYTE data stating if the 29 bit Fixed Normal addressing CAN identifier is supported (default is true).
        /// </summary>
        PCANTP_PARAMETER_SUPPORT_29B_FIXED_NORMAL = 0x11C,
        /// <summary>
        /// 1 BYTE data stating if the 29 bit Mixed addressing CAN identifier is supported (default is true).
        /// </summary>
        PCANTP_PARAMETER_SUPPORT_29B_MIXED = 0x11D,
        /// <summary>
        /// Pointer to a cantp_msg, checks if the message is valid and can be sent (ex. if a mapping is needed) and corrects input if needed.
        /// </summary>
        PCANTP_PARAMETER_MSG_CHECK = 0x11E,
        /// <summary>
        /// 1 BYTE data stating to clear Rx/Tx queues and CAN controller (channel is unitialized and re-initialized but settings and mappings are kept)
        /// </summary>
        PCANTP_PARAMETER_RESET_HARD = 0x11F,
        /// <summary>
        /// 1 BYTE data stating if network is full-duplex (default) or half-duplex
        /// </summary>
        PCANTP_PARAMETER_NETWORK_LAYER_DESIGN = 0x120,

        /// <summary>
        /// PCAN hardware name parameter
        /// </summary>
        PCANTP_PARAMETER_HARDWARE_NAME = TPCANParameter.PCAN_HARDWARE_NAME,
        /// <summary>
        /// PCAN-USB device identifier parameter
        /// </summary>
        PCANTP_PARAMETER_DEVICE_ID = TPCANParameter.PCAN_DEVICE_NUMBER,
        /// <summary>
        /// Deprecated use PCANTP_PARAMETER_DEVICE_ID instead
        /// </summary>
        PCANTP_PARAMETER_DEVICE_NUMBER = TPCANParameter.PCAN_DEVICE_NUMBER,
        /// <summary>
        /// CAN-Controller number of a PCAN-Channel 
        /// </summary>
        PCANTP_PARAMETER_CONTROLLER_NUMBER = TPCANParameter.PCAN_CONTROLLER_NUMBER,
        /// <summary>
        /// Capabilities of a PCAN device (FEATURE_***)
        /// </summary>
        PCANTP_PARAMETER_CHANNEL_FEATURES = TPCANParameter.PCAN_CHANNEL_FEATURES
    }

    /// <summary>
    /// Represents the type of a CANTP message (see field "cantp_msg.type").
    /// </summary>
    [Flags]
    public enum cantp_msgtype : UInt32
    {
        /// <summary>
        /// uninitialized message (data is NULL)
        /// </summary>
        PCANTP_MSGTYPE_NONE = 0,
        /// <summary>
        /// standard CAN frame
        /// </summary>
        PCANTP_MSGTYPE_CAN = 1,
        /// <summary>
        /// CAN frame with FD support
        /// </summary>
        PCANTP_MSGTYPE_CANFD = 2,
        /// <summary>
        /// ISO-TP message (ISO:15765) 
        /// </summary>
        PCANTP_MSGTYPE_ISOTP = 4,

        /// <summary>
        /// frame only: unsegmented messages
        /// </summary>
        PCANTP_MSGTYPE_FRAME = PCANTP_MSGTYPE_CAN | PCANTP_MSGTYPE_CANFD,
        /// <summary>
        /// any supported message type
        /// </summary>
        PCANTP_MSGTYPE_ANY = PCANTP_MSGTYPE_FRAME | PCANTP_MSGTYPE_ISOTP | 0xFFFFFFFF
    }

    /// <summary>
    /// Represents the flags common to all types of cantp_msg (see field "cantp_msg.msgdata.flags").
    /// </summary>
    public enum cantp_msgflag : UInt32
    {
        /// <summary>
        /// no flag
        /// </summary>
        PCANTP_MSGFLAG_NONE = 0,
        /// <summary>
        /// message is the confirmation of a transmitted message
        /// </summary>
        PCANTP_MSGFLAG_LOOPBACK = 1,
        /// <summary>
        /// message is a frame of a segmented ISO-TP message
        /// </summary>
        PCANTP_MSGFLAG_ISOTP_FRAME = 2,
    }

    /// <summary>
    /// Represents the flags of a CAN or CAN FD frame (must be used as flags for ex. EXTENDED|FD|BRS.) (see field "cantp_msg.can_info.can_msgtype")
    /// </summary>
    public enum cantp_can_msgtype : UInt32
    {
        /// <summary>
        /// The PCAN message is a CAN Standard Frame (11-bit identifier)
        /// </summary>
        PCANTP_CAN_MSGTYPE_STANDARD = TPCANMessageType.PCAN_MESSAGE_STANDARD,
        /// <summary>
        /// The PCAN message is a CAN Remote-Transfer-Request Frame
        /// </summary>
        PCANTP_CAN_MSGTYPE_RTR = TPCANMessageType.PCAN_MESSAGE_RTR,
        /// <summary>
        /// The PCAN message is a CAN Extended Frame (29-bit identifier)
        /// </summary>
        PCANTP_CAN_MSGTYPE_EXTENDED = TPCANMessageType.PCAN_MESSAGE_EXTENDED,
        /// <summary>
        /// The PCAN message represents a FD frame in terms of CiA Specs
        /// </summary>
        PCANTP_CAN_MSGTYPE_FD = TPCANMessageType.PCAN_MESSAGE_FD,
        /// <summary>
        /// The PCAN message represents a FD bit rate switch (CAN data at a higher bit rate)
        /// </summary>
        PCANTP_CAN_MSGTYPE_BRS = TPCANMessageType.PCAN_MESSAGE_BRS,
        /// <summary>
        /// The PCAN message represents a FD error state indicator(CAN FD transmitter was error active)
        /// </summary>
        PCANTP_CAN_MSGTYPE_ESI = TPCANMessageType.PCAN_MESSAGE_ESI,
        /// <summary>
        /// The PCAN message represents an error frame.
        /// </summary>
        PCANTP_CAN_MSGTYPE_ERRFRAME = TPCANMessageType.PCAN_MESSAGE_ERRFRAME,
        /// <summary>
        /// The PCAN message represents a PCAN status message. 
        /// </summary>
        PCANTP_CAN_MSGTYPE_STATUS = TPCANMessageType.PCAN_MESSAGE_STATUS,
		/// <summary>
		/// Flag stating that the message is not a CAN Frame
		/// </summary>
        PCANTP_CAN_MSGTYPE_FLAG_INFO = (TPCANMessageType.PCAN_MESSAGE_ERRFRAME | TPCANMessageType.PCAN_MESSAGE_STATUS)
    }

    /// <summary>
    /// Represents the type of an ISO-TP message (see field "cantp_msg.msgdata_isotp.netaddrinfo.msgtype").
    /// </summary>
    public enum cantp_isotp_msgtype : UInt32
    {
        /// <summary>
        /// Unknown (non-ISO-TP) message
        /// </summary>
        PCANTP_ISOTP_MSGTYPE_UNKNOWN = 0x00,
        /// <summary>
        /// Diagnostic message (request or confirmation)
        /// </summary>
        PCANTP_ISOTP_MSGTYPE_DIAGNOSTIC = 0x01,
        /// <summary>
        /// Remote Diagnostic message (request or confirmation)
        /// </summary>
        PCANTP_ISOTP_MSGTYPE_REMOTE_DIAGNOSTIC = 0x02,
        /// <summary>
        /// Multi-Frame Message is being received
        /// </summary>
        PCANTP_ISOTP_MSGTYPE_FLAG_INDICATION_RX = 0x10,
        /// <summary>
        /// Multi-Frame Message is being transmitted
        /// </summary>
        PCANTP_ISOTP_MSGTYPE_FLAG_INDICATION_TX = 0x20,
        /// <summary>
        /// Multi-Frame Message is being communicated (Tx or Rx)
        /// </summary>
        PCANTP_ISOTP_MSGTYPE_FLAG_INDICATION = (0x10 | 0x20),
        /// <summary>
        /// Mask to remove Indication flags
        /// </summary>
        PCANTP_ISOTP_MSGTYPE_MASK_INDICATION = 0x0F
    }

    /// <summary>
    /// Represents the addressing format of an ISO-TP message (see field "cantp_msg.msgdata_isotp.netaddrinfo.format").
    /// </summary>
    public enum cantp_isotp_format : UInt32
    {
        /// <summary>
        /// unknown adressing format
        /// </summary>
        PCANTP_ISOTP_FORMAT_UNKNOWN = 0xFF,
        /// <summary>
        /// unsegmented CAN frame
        /// </summary>
        PCANTP_ISOTP_FORMAT_NONE = 0x00,
        /// <summary>
        /// normal adressing format from ISO 15765-2
        /// </summary>
        PCANTP_ISOTP_FORMAT_NORMAL = 0x01,
        /// <summary>
        /// fixed normal adressing format from ISO 15765-2
        /// </summary>
        PCANTP_ISOTP_FORMAT_FIXED_NORMAL = 0x02,
        /// <summary>
        /// extended adressing format from ISO 15765-2
        /// </summary>
        PCANTP_ISOTP_FORMAT_EXTENDED = 0x03,
        /// <summary>
        /// mixed adressing format from ISO 15765-2
        /// </summary>
        PCANTP_ISOTP_FORMAT_MIXED = 0x04,
        /// <summary>
        /// enhanced adressing format from ISO 15765-3
        /// </summary>
        PCANTP_ISOTP_FORMAT_ENHANCED = 0x05,
    }

    /// <summary>
    /// Represents the type of target of an ISO-TP message (see field "cantp_msg.msgdata_isotp.netaddrinfo.target_type").
    /// </summary>
    public enum cantp_isotp_addressing : UInt32
    {
        /// <summary>
        /// Unknown adressing format
        /// </summary>
        PCANTP_ISOTP_ADDRESSING_UNKNOWN = 0x00,
        /// <summary>
        /// Physical addressing ("peer to peer")
        /// </summary>
        PCANTP_ISOTP_ADDRESSING_PHYSICAL = 0x01,
        /// <summary>
        /// Functional addressing ("peer to any")
        /// </summary>
        PCANTP_ISOTP_ADDRESSING_FUNCTIONAL = 0x02,
    }


    /// <summary>
    /// Represents the options of a message (mainly supported for ISO-TP message) (see field "cantp_msg.msgdata.options").
    /// </summary>
    public enum cantp_option : UInt32
    {
        /// <summary>
        /// 1 BYTE data stating if unsegmented (NON-ISO-TP) CAN frames can be received
        /// </summary>
        PCANTP_OPTION_FRAME_FILTERING = cantp_parameter.PCANTP_PARAMETER_FRAME_FILTERING,
        /// <summary>
        /// 1 BYTE data stating if CAN frame DLC uses padding or not
        /// </summary>
        PCANTP_OPTION_CAN_DATA_PADDING = cantp_parameter.PCANTP_PARAMETER_CAN_DATA_PADDING,
        /// <summary>
        /// 1 BYTE data stating the value used for CAN data padding
        /// </summary>
        PCANTP_OPTION_CAN_PADDING_VALUE = cantp_parameter.PCANTP_PARAMETER_CAN_PADDING_VALUE,
        /// <summary>
        /// 1 BYTE data stating the default priority value for normal fixed, mixed and enhanced addressing (default=6)
        /// </summary>
        PCANTP_OPTION_J1939_PRIORITY = cantp_parameter.PCANTP_PARAMETER_J1939_PRIORITY,
        /// <summary>
        /// 1 BYTE data stating if pending messages are displayed/hidden
        /// </summary>
        PCANTP_OPTION_MSG_PENDING = cantp_parameter.PCANTP_PARAMETER_MSG_PENDING,
        /// <summary>
        /// 1 BYTE data describing the block size parameter (BS)
        /// </summary>
        PCANTP_OPTION_BLOCK_SIZE = cantp_parameter.PCANTP_PARAMETER_BLOCK_SIZE,
        /// <summary>
        /// 2 BYTE data describing the transmit block size parameter (BS_TX)
        /// </summary>
        PCANTP_OPTION_BLOCK_SIZE_TX = cantp_parameter.PCANTP_PARAMETER_BLOCK_SIZE_TX,
        /// <summary>
        /// 1 BYTE data describing the seperation time parameter (STmin)
        /// </summary>
        PCANTP_OPTION_SEPARATION_TIME = cantp_parameter.PCANTP_PARAMETER_SEPARATION_TIME,
        /// <summary>
        /// 2 BYTE data describing the transmit seperation time parameter (STmin_TX)
        /// </summary>
        PCANTP_OPTION_SEPARATION_TIME_TX = cantp_parameter.PCANTP_PARAMETER_SEPARATION_TIME_TX,
        /// <summary>
        /// 4 BYTE data describing the Wait Frame Transmissions parameter. 
        /// </summary>
        PCANTP_OPTION_WFT_MAX = cantp_parameter.PCANTP_PARAMETER_WFT_MAX,
        /// <summary>
        /// 1 BYTE data to set optimization options to improve delay between ISO-TP consecutive frames.
        /// </summary>
        PCANTP_OPTION_SELFRECEIVE_LATENCY = cantp_parameter.PCANTP_PARAMETER_SELFRECEIVE_LATENCY
    }

    /// <summary>
    /// Represents the status for a message whose transmission is in progress.
    /// </summary>
    public enum cantp_msgprogress_state : UInt32
    {
        /// <summary>
        /// Message is not yet handled.
        /// </summary>
        PCANTP_MSGPROGRESS_STATE_QUEUED = 0,
        /// <summary>
        /// Message is being processed (received or transmitted).
        /// </summary>
        PCANTP_MSGPROGRESS_STATE_PROCESSING = 1,
        /// <summary>
        /// Message is completed.
        /// </summary>
        PCANTP_MSGPROGRESS_STATE_COMPLETED = 2,
        /// <summary>
        /// Message is unknown/not found.
        /// </summary>
        PCANTP_MSGPROGRESS_STATE_UNKNOWN = 3
    }

    /// <summary>
    /// Represents the direction of a message's communication.
    /// </summary>
    public enum cantp_msgdirection : UInt32
    {
        /// <summary>
        /// Message is being received.
        /// </summary>
        PCANTP_MSGDIRECTION_RX = 0,
        /// <summary>
        /// Message is being transmitted.
        /// </summary>
        PCANTP_MSGDIRECTION_TX = 1,
    }
    #endregion

    #region Structures

    ////////////////////////////////////////////////////////////
    // miscellaneous message related definitions
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// Internal information about cantp_msg message (reserved).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msginfo
    {
        /// <summary>
        /// (read-only) size of the message object 
        /// </summary>
        public UInt32 size;
        /// <summary>
        /// (read-only) reserved
        /// </summary>
        public cantp_msginfo_flag flags;
        /// <summary>
        /// (read-only) reserved	
        /// </summary>
        public cantp_msginfo_extra extra;
    }


    /// <summary>
    /// Represents message's options to override.	
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msgoption
    {
        /// <summary>
        /// Name of the option.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_option name;
        /// <summary>
        /// Value of the option.
        /// </summary>
        public UInt32 value;
    }

    /// <summary>
    /// Represents a list of message's options to override.	
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msgoption_list
    {
        /// <summary>
        /// Pointer to an array of cantp_msgoption.
        /// Use it with unsafe { cantp_msgoption* opts = (cantp_msgoption*)buffer.ToPointer(); } 
        /// Or with Marshal.Copy, Marshal.ReadInt32, Marshal.WriteInt32
        /// See special C# functions
        /// </summary>
        public IntPtr buffer;
        /// <summary>
        /// Number of options in the array.
        /// </summary>
        public UInt32 count;
    }


    /// <summary>
    /// Represents common CAN information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_can_info
    {
        /// <summary>
        /// CAN identifier
        /// </summary>
        public UInt32 can_id;
        /// <summary>
        /// Types and flags of the CAN/CAN-FD frame (see cantp_can_msgtype)
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_can_msgtype can_msgtype;
        /// <summary>
        /// Data Length Code of the frame (0..15)
        /// </summary>
        public byte dlc;
    }

    /// <summary>
    /// Represents the network address information of an ISO-TP message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_netaddrinfo
    {
        /// <summary>
        /// ISO-TP message type
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_isotp_msgtype msgtype;
        /// <summary>
        /// ISO-TP format addressing
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_isotp_format format;
        /// <summary>
        /// ISO-TP addressing/target type
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_isotp_addressing target_type;
        /// <summary>
        /// source address 
        /// </summary>
        public UInt16 source_addr;
        /// <summary>
        /// target address 
        /// </summary>
        public UInt16 target_addr;
        /// <summary>
        /// extension address
        /// </summary>
        public byte extension_addr;
    }

    /// <summary>
    /// Represents a mapping between an ISO-TP network address information and a CAN ID.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_mapping
    {
        /// <summary>
        /// Mapping's unique ID 
        /// </summary>
        public UIntPtr uid;
        /// <summary>
        /// CAN ID mapped to the Network Address Information
        /// </summary>
        public UInt32 can_id;
        /// <summary>
        /// CAN ID used for the flow control frame (formerly 'can_id_resp')
        /// </summary>
        public UInt32 can_id_flow_ctrl;
        /// <summary>
        /// CAN frame msgtype (only PCANTP_CAN_MSGTYPE_STANDARD or PCANTP_CAN_MSGTYPE_EXTENDED is mandatory)
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_can_msgtype can_msgtype;
        /// <summary>
        /// Default CAN DLC value to use with segmented messages
        /// </summary>
        public byte can_tx_dlc;
        /// <summary>
        /// ISO-TP Network Address Information
        /// </summary>
        public cantp_netaddrinfo netaddrinfo;
    }

    ////////////////////////////////////////////////////////////
    // Message definitions
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// Represents the content of a generic message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msgdata
    {
        /// <summary>
        /// structure specific flags
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_msgflag flags;
        /// <summary>
        /// Length of the message
        /// </summary>
        public UInt32 length;
        /// <summary>
        /// Data of the message
        /// Array of bytes
        /// Use it with unsafe { byte* don = (byte*)data.ToPointer(); } 
        /// or with Marshal.WriteByte, Marshal.ReadByte, Marshal.Copy...
        /// See special C# functions
        /// </summary>
        public IntPtr data;
        /// <summary>
        /// Network status
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_netstatus netstatus;
        /// <summary>
        /// Defines specific options to override global message configuration
        /// Pointer to a cantp_msgoption_list
        /// Use it with Marshal.PtrToStructure() and Marshal.StructureToPtr()
        /// or with unsafe {options.toPointer()}
        /// See special C# functions
        /// </summary>
        public IntPtr options;
    }

    /// <summary>
    /// Represents the content of a standard CAN frame.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msgdata_can
    {
        /// <summary>
        /// structure specific flags
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_msgflag flags;
        /// <summary>
        /// Length of the message (0..8)
        /// </summary>
        public UInt32 length;
        /// <summary>
        /// Data of the message
        /// Array of bytes
        /// Use it with unsafe { byte* don = (byte*)data.ToPointer(); } 
        /// or with Marshal.WriteByte, Marshal.ReadByte, Marshal.Copy...
        /// See special C# functions
        /// </summary>
        public IntPtr data;
        /// <summary>
        /// Network status
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_netstatus netstatus;
        /// <summary>
        /// Defines specific options to override global CAN configuration (not used yet) 
        /// Pointer to a cantp_msgoption_list
        /// Use it with Marshal.PtrToStructure() and Marshal.StructureToPtr()
        /// or with unsafe {options.toPointer()}
        /// See special C# functions
        /// </summary>
        public IntPtr options;
        /// <summary>
        /// Data of the message (DATA[0]..DATA[7])
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = CanTpApi.PCANTP_MAX_LENGTH_CAN_STANDARD)]
        public byte[] data_max;
    }



    /// <summary>
    /// Represents the content of a CAN FD frame.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msgdata_canfd
    {
        /// <summary>
        /// structure specific flags
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_msgflag flags;
        /// <summary>
        /// Length of the message (0..64)
        /// </summary>
        public UInt32 length;
        /// <summary>
        /// Data of the message
        /// Array of bytes
        /// Use it with unsafe { byte* don = (byte*)data.ToPointer(); } 
        /// or with Marshal.WriteByte, Marshal.ReadByte, Marshal.Copy...
        /// See special C# functions
        /// </summary>
        public IntPtr data;
        /// <summary>
        /// Network status
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_netstatus netstatus;
        /// <summary>
        /// Defines specific options to override global CAN configuration (not used yet) 
        /// Pointer to a cantp_msgoption_list
        /// Use it with Marshal.PtrToStructure() and Marshal.StructureToPtr()
        /// or with unsafe {options.toPointer()}
        /// See special C# functions
        /// </summary>
        public IntPtr options;
        /// <summary>
        /// Data of the message (DATA[0]..DATA[63])
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = CanTpApi.PCANTP_MAX_LENGTH_CAN_FD)]
        public byte[] data_max;
    }


    /// <summary>
    /// Represents the content of an ISO-TP message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msgdata_isotp
    {
        /// <summary>
        /// structure specific flags
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_msgflag flags;
        /// <summary>
        /// Length of the data
        /// </summary>
        public UInt32 length;
        /// <summary>
        /// Data of the message
        /// Array of bytes
        /// Use it with unsafe { byte* don = (byte*)data.ToPointer(); } 
        /// or with Marshal.WriteByte, Marshal.ReadByte, Marshal.Copy...
        /// See special C# functions
        /// </summary>
        public IntPtr data;
        /// <summary>
        /// Network status
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_netstatus netstatus;
        /// <summary>
        /// Defines specific options to override global CAN configuration 
        /// Pointer to a cantp_msgoption_list
        /// Use it with Marshal.PtrToStructure() and Marshal.StructureToPtr()
        /// or with unsafe {options.toPointer()}
        /// See special C# functions
        /// </summary>
        public IntPtr options;
        /// <summary>
        /// ISO-TP network address information
        /// </summary>
        public cantp_netaddrinfo netaddrinfo;
        /// <summary>
        /// Reserved ISO-TP information 
        /// </summary>
        public cantp_isotp_info reserved;
    }



    /// <summary>
    /// A cantp_msg message is a generic CAN related message than can be either a standard CAN frame,
    /// a CAN FD frame, an ISO-TP message, etc.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msg
    {
        /// <summary>
        /// type of the message
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_msgtype type;
        /// <summary>
        /// misc. read-only information
        /// </summary>
        public cantp_msginfo reserved;
        /// <summary>
        /// common CAN information
        /// </summary>
        public cantp_can_info can_info;
        /// <summary>
        /// data of the message
        /// pointer to different structures depending on the message type 
        /// Use it with following properties (Msgdata_any_Copy, Msgdata_can_copy etc.)
        /// Or with Marshal.PtrToStructure and Marshal.StructureToPtr
        /// Or with unsafe {msgdata.ToPointer()}
        /// See special C# functions
        /// </summary>
        private IntPtr msgdata;
        /// <summary>
        /// get msgdata field as a property
        /// </summary>
        public IntPtr Msgdata
        {
            get { return msgdata; }
        }
        /// <summary>
        /// get msgdata field as a cantp_msgdata property
        /// The fields of this property will not be writable except (not directly) those whose type is "IntPtr".
        /// Before using this property, check if msgdata is not IntPtr.Zero
        /// </summary>
        public cantp_msgdata Msgdata_any_Copy
        {
            get
            {
                return (cantp_msgdata)Marshal.PtrToStructure(msgdata, typeof(cantp_msgdata));
            }
        }

        /// <summary>
        /// get msgdata field as a cantp_msgdata_can property
        /// The fields of this property will not be writable except (not directly) those whose type is "IntPtr".
        /// Before using this property, check if msgdata is not IntPtr.Zero
        /// </summary>
        public cantp_msgdata_can Msgdata_can_Copy
        {
            get
            {
                return (cantp_msgdata_can)Marshal.PtrToStructure(msgdata, typeof(cantp_msgdata_can));
            }

        }
        /// <summary>
        /// get msgdata field as a cantp_msgdata_canfd property
        /// The fields of this property will not be writable except (not directly) those whose type is "IntPtr".
        /// Before using this property, check if msgdata is not IntPtr.Zero
        /// </summary>
        public cantp_msgdata_canfd Msgdata_canfd_Copy
        {
            get
            {
                return (cantp_msgdata_canfd)Marshal.PtrToStructure(msgdata, typeof(cantp_msgdata_canfd));
            }
        }
        /// <summary>
        /// get msgdata field as a cantp_msgdata_isotp property
        /// The fields of this property will not be writable except (not directly) those whose type is "IntPtr".
        /// Before using this property, check if msgdata is not IntPtr.Zero
        /// </summary>
        public cantp_msgdata_isotp Msgdata_isotp_Copy
        {
            get
            {
                return (cantp_msgdata_isotp)Marshal.PtrToStructure(msgdata, typeof(cantp_msgdata_isotp));
            }
        }

#if (UNSAFE)
        /// <summary>
        /// get the msgdata field as a pointer to cantp_msgdata 
        /// all fields of this property will be writable (in unsafe mode) since it is a pointer.
        /// Before using this property, check if msgdata is not IntPtr.Zero
        /// </summary>
        public unsafe cantp_msgdata* Msgdata_any
        {
            get
            {
                cantp_msgdata* str = (cantp_msgdata*)msgdata.ToPointer();
                return str;
            }
        }

        /// <summary>
        /// get the msgdata field as a pointer to cantp_msgdata_isotp 
        /// all fields of this property will be writable (in unsafe mode) since it is a pointer.
        /// Before using this property, check if msgdata is not IntPtr.Zero
        /// </summary>
        public unsafe cantp_msgdata_isotp* Msgdata_isotp
        {
            get
            {
                cantp_msgdata_isotp* str = (cantp_msgdata_isotp*)msgdata.ToPointer();
                return str;
            }
        }
#endif
    }


    /// <summary>
    /// Holds information on the communication progress of a message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct cantp_msgprogress
    {
        /// <summary>
        /// State of the message
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_msgprogress_state state;
        /// <summary>
        /// Progress of the transmission/reception in percent.
        /// </summary>
        public byte percentage;
        /// <summary>
        /// Buffer to get a copy of the pending message.
        /// Pointer to a cantp_msg
        /// If this is not needed, set it to IntPtr.Zero.
        /// If this is needed, set it with unsafe { buffer = IntPtr(&msg);}
        /// Use it in read-only mode with cantp_msg mp = (cantp_msg)Marshal.PtrToStructure(buffer, typeof(cantp_msg));
        /// or with unsafe {(cantp_msg*)buffer.toPointer();}
        /// See special C# functions
        /// </summary>
        public IntPtr buffer;
    }

    #endregion

    #region PCANTP Api
    public static partial class CanTpApi
    {
        #region bitrate
        // Represents the configuration for a CAN bit rate
        // Note: 
        //    * Each parameter and its value must be separated with a '='.
        //    * Each pair of parameter/value must be separated using ','. 
        //
        // Example:
        //    f_clock=80000000,nom_brp=10,nom_tseg1=5,nom_tseg2=2,nom_sjw=1,data_brp=4,data_tseg1=7,data_tseg2=2,data_sjw=1
        //
        public const string PCANTP_BITRATE_CLOCK = PCANBasic.PCAN_BR_CLOCK;
        public const string PCANTP_BITRATE_CLOCK_MHZ = PCANBasic.PCAN_BR_CLOCK_MHZ;
        public const string PCANTP_BITRATE_NOM_BRP = PCANBasic.PCAN_BR_NOM_BRP;
        public const string PCANTP_BITRATE_NOM_TSEG1 = PCANBasic.PCAN_BR_NOM_TSEG1;
        public const string PCANTP_BITRATE_NOM_TSEG2 = PCANBasic.PCAN_BR_NOM_TSEG2;
        public const string PCANTP_BITRATE_NOM_SJW = PCANBasic.PCAN_BR_NOM_SJW;
        public const string PCANTP_BITRATE_NOM_SAMPLE = PCANBasic.PCAN_BR_NOM_SAMPLE;
        public const string PCANTP_BITRATE_DATA_BRP = PCANBasic.PCAN_BR_DATA_BRP;
        public const string PCANTP_BITRATE_DATA_TSEG1 = PCANBasic.PCAN_BR_DATA_TSEG1;
        public const string PCANTP_BITRATE_DATA_TSEG2 = PCANBasic.PCAN_BR_DATA_TSEG2;
        public const string PCANTP_BITRATE_DATA_SJW = PCANBasic.PCAN_BR_DATA_SJW;
        public const string PCANTP_BITRATE_DATA_SAMPLE = PCANBasic.PCAN_BR_DATA_SAMPLE;
        #endregion

        #region PCANTP parameter values

        public const byte PCANTP_VALUE_PARAMETER_OFF = PCANBasic.PCAN_PARAMETER_OFF;
        public const byte PCANTP_VALUE_PARAMETER_ON = PCANBasic.PCAN_PARAMETER_ON;
        /// <summary>
        /// Enable debug messages (only notices, informations, warnings, errors)
        /// </summary>
        public const byte PCANTP_DEBUG_NOTICE = 0xF4;
        /// <summary>
        /// Enable debug messages (only informations, warnings, errors)
        /// </summary>
        public const byte PCANTP_DEBUG_INFO = 0xF3;
        /// <summary>
        /// Enable debug messages (only warnings, errors)
        /// </summary>
        public const byte PCANTP_DEBUG_WARNING = 0xF2;
        /// <summary>
        /// Enable debug messages (only errors)
        /// </summary>
        public const byte PCANTP_DEBUG_ERROR = 0xF1;

#if (!PCANTP_API_COMPATIBILITY_ISO_2004)
        /// <summary>
        /// No debug messages
        /// </summary>
        public const byte PCANTP_DEBUG_NONE = 0x00;
        /// <summary>
        /// Puts CAN debug messages to stdout
        /// </summary>
        public const byte PCANTP_DEBUG_CAN = 0x01;
        /// <summary>
        /// The Channel is illegal or not available
        /// </summary>
        public const byte PCANTP_CHANNEL_UNAVAILABLE = 0x00;
        /// <summary>
        /// The Channel is available
        /// </summary>
        public const byte PCANTP_CHANNEL_AVAILABLE = 0x01;
        /// <summary>
        /// The Channel is valid, and is being used
        /// </summary>
        public const byte PCANTP_CHANNEL_OCCUPIED = 0x02;
        /// <summary>
        /// if set Flow Control frame shall not use the WT flow status value
        /// </summary>
        public const byte PCANTP_WFT_MAX_UNLIMITED = 0x00;
        /// <summary>
        /// an integer describing the Wait Frame Transmissions parameter. 
        /// </summary>
        public const byte PCANTP_WFT_MAX_DEFAULT = 0x10;
        /// <summary>
        /// hide messages with type PCANTP_MESSAGE_INDICATION from CANTP_Read function
        /// </summary>
        public const byte PCANTP_MSG_PENDING_HIDE = 0x00;
        /// <summary>
        /// show messages with type PCANTP_MESSAGE_INDICATION from CANTP_Read function
        /// </summary>
        public const byte PCANTP_MSG_PENDING_SHOW = 0x01;

        /// <summary>
        /// uses CAN frame data optimization
        /// </summary>
        public const byte PCANTP_CAN_DATA_PADDING_NONE = 0x00;
        /// <summary>
        /// uses CAN frame data padding (default, i.e. CAN DLC = 8)
        /// </summary>
        public const byte PCANTP_CAN_DATA_PADDING_ON = 0x01;
        /// <summary>
        /// default value used if CAN data padding is enabled
        /// </summary>
        public const byte PCANTP_CAN_DATA_PADDING_VALUE = 0x55;
#endif
        /// <summary>
        /// disable reception of unformatted (NON-ISO-TP) CAN frames (default)
        ///	only ISO 15765 messages will be received
        /// </summary>
        public const byte PCANTP_FRAME_FILTERING_ISOTP = 0x00;
        /// <summary>
        /// enable reception of unformatted (NON-ISO-TP) CAN frames 
        ///	received messages will be treated as either ISO 15765 or as an unformatted CAN frame
        /// </summary>
        public const byte PCANTP_FRAME_FILTERING_CAN = 0x01;
        /// <summary>
        /// enable reception of unformatted (NON-ISO-TP) CAN frames 
        ///	received messages will be treated as ISO 15765, unformatted CAN frame, or both (user will able to read fragmented CAN frames)
        /// </summary>
        public const byte PCANTP_FRAME_FILTERING_VERBOSE = 0x02;
#if (!PCANTP_API_COMPATIBILITY_ISO_2004)


        /// <summary>
        /// default priority for ISO-TP messages (only available fot normal fixed, mixed and enhanced addressing)
        /// </summary>
        public const byte PCANTP_J1939_PRIORITY_DEFAULT = 0x06;
#endif
        /// <summary>
        /// maximum size of a CAN (non-FD) frame (8)
        /// </summary>
        public const byte PCANTP_MAX_LENGTH_CAN_STANDARD = 0x08;
        /// <summary>
        /// maximum size of a CAN FD frame (64)
        /// </summary>
        public const byte PCANTP_MAX_LENGTH_CAN_FD = 0x40;
        /// <summary>
        /// maximum size of an ISO-TP rev. 2004 frame (4095)
        /// </summary>
        public const UInt32 PCANTP_MAX_LENGTH_ISOTP2004 = 0xFFF;
        /// <summary>
        /// maximum size of an ISO-TP rev. 2016 frame (4294967295)
        /// </summary>
        public const UInt32 PCANTP_MAX_LENGTH_ISOTP2016 = 0xFFFFFFFF;
        /// <summary>
        /// maximum size before using virtual allocation for ISO-TP messages
        /// </summary>
        public const UInt32 PCANTP_MAX_LENGTH_ALLOC = 0xFFFFFFU;
        /// <summary>
        /// default DLC for fragmented frames when transmiting ISO-TP messages
        /// </summary>
        public const byte PCANTP_CAN_TX_DL_DEFAULT = PCANTP_MAX_LENGTH_CAN_STANDARD;
        /// <summary>
        /// ISO-15765-2:2004(E)
        /// </summary>
        public const byte PCANTP_ISO_REV_2004 = 0x01;
        /// <summary>
        /// ISO-15765-2:2016(E)
        /// </summary>
        public const byte PCANTP_ISO_REV_2016 = 0x02;
        /// <summary>
        /// disables the feature "ignore received BlockSize value"
        /// </summary>
        public const UInt32 PCANTP_BLOCK_SIZE_TX_IGNORE = 0xFFFF;
        /// <summary>
        /// disables the feature "ignore received STMin value"
        /// </summary>
        public const UInt32 PCANTP_SEPERATION_TIME_TX_IGNORE = 0xFFFF;
        /// <summary>
        /// sets timeouts according to ISO-15765-2
        /// </summary>
        public const byte PCANTP_ISO_TIMEOUTS_15765_2 = 0;
        /// <summary>
        /// sets timeouts according to ISO-15765-4 (OBDII)
        /// </summary>
        public const byte PCANTP_ISO_TIMEOUTS_15765_4 = 1;
        /// <summary>
        /// no optimization (use this parameter if ECU requires strict respect of Minimum Separation Time)
        /// </summary>
        public const byte PCANTP_SELFRECEIVE_LATENCY_NONE = 0;
        /// <summary>
        /// (DEFAULT) fragmented self-receive frame mechanism is ignored when STmin is set to = 0xF3 and lower (<300µs) 
        /// </summary>
        public const byte PCANTP_SELFRECEIVE_LATENCY_LIGHT = 1;
        /// <summary>
        /// as LIGHT value plus optimize self-receive latency by predicting the time to effectively write frames on bus
        /// </summary>
        public const byte PCANTP_SELFRECEIVE_LATENCY_OPTIMIZED = 2;
        /// <summary>
        /// (DEFAULT) maxinum number of items in the receive queue
        /// </summary>
        public const UInt32 PCANTP_MAX_RX_QUEUE_DEFAULT = 32767;
        /// <summary>
        /// (DEFAULT) Network layer design is full-duplex
        /// </summary>
        public const byte PCANTP_NETWORK_LAYER_FULL_DUPLEX = 0;
        /// <summary>
        /// Network layer design is half-duplex (only one transmission/reception of the same NAI at a time)
        /// </summary>
        public const byte PCANTP_NETWORK_LAYER_HALF_DUPLEX = 1;

        // Standard ISO-15765-2 values

        /// <summary>
        /// Default value for Separation time
        /// </summary>
        public const byte PCANTP_STMIN_ISO_15765_2 = 10;
        /// <summary>
        /// Default value for BlockSize
        /// </summary>
        public const byte PCANTP_BS_ISO_15765_2 = 10;
        /// <summary>
        /// Default value for Timeout Ar in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_AR_ISO_15765_2 = (1000 * 1000);
        /// <summary>
        /// Default value for Timeout As in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_AS_ISO_15765_2 = (1000 * 1000);
        /// <summary>
        /// Default value for Timeout Br in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_BR_ISO_15765_2 = (1000 * 1000);
        /// <summary>
        /// Default value for Timeout Bs in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_BS_ISO_15765_2 = (1000 * 1000);
        /// <summary>
        /// Default value for Timeout Cr in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_CR_ISO_15765_2 = (1000 * 1000);
        /// <summary>
        /// Default value for Timeout Cs in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_CS_ISO_15765_2 = (1000 * 1000);
        /// <summary>
        /// Default value for timeout tolerance [0..100] (timeout = t * (1 + tolerance/100))
        /// </summary>
        public const byte PCANTP_TIMEOUT_TOLERANCE = 0;

        // Standard ISO-15765-4 (OBDII) values

        /// <summary>
        /// OBDII value for Separation time
        /// </summary>
        public const byte PCANTP_STMIN_ISO_15765_4 = 0;
        /// <summary>
        /// OBDII value for BlockSize
        /// </summary>
        public const byte PCANTP_BS_ISO_15765_4 = 0;
        /// <summary>
        /// OBDII value for Timeout Ar in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_AR_ISO_15765_4 = (1000 * 25);
        /// <summary>
        /// OBDII value for Timeout As in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_AS_ISO_15765_4 = (1000 * 25);
        /// <summary>
        /// OBDII value for Timeout Br in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_BR_ISO_15765_4 = (1000 * 75);
        /// <summary>
        /// OBDII value for Timeout Bs in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_BS_ISO_15765_4 = (1000 * 75);
        /// <summary>
        /// OBDII value for Timeout Cr in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_CR_ISO_15765_4 = (1000 * 150);
        /// <summary>
        /// OBDII value for Timeout Cs in µs
        /// </summary>
        public const UInt32 PCANTP_TIMEOUT_CS_ISO_15765_4 = (1000 * 150);

        // Values for cfg_value

        /// <summary>
        /// Mask for the 29bits ISOTP priority value (stored in bits [0..2])
        /// </summary>
        public const byte PCANTP_FLAG_MASK_PRIORITY = 0x07;
        /// <summary>
        /// Padding is enabled 
        /// </summary>
        public const byte PCANTP_FLAG_PADDING_ON = 0x08;
        /// <summary>
        /// Message's indication is enabled 
        /// </summary>
        public const byte PCANTP_FLAG_INDICATION_ON = 0x10;
        /// <summary>
        /// Echo of fragmented frames is enabled 
        /// </summary>
        public const byte PCANTP_FLAG_ECHO_FRAMES_ON = 0x20;
        #endregion

        #region constants 

        ////////////////////////////////////////////////////////////
        // Constants definition
        ////////////////////////////////////////////////////////////

        /// <summary>
        /// Mapping does not require a Flow Control frame.
        /// </summary>
        public const UInt32 PCANTP_MAPPING_FLOW_CTRL_NONE = 0xFFFFFFFF;
        #endregion

        #region PCAN ISO-TP API Implementation

        /// <summary>
        /// Initializes a PCANTP-Client based on a CANTP handle (without CAN-FD support)
        /// </summary>
        /// <remarks>Only one PCANTP-Client can be initialized per CAN-Channel</remarks>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="baudrate">The CAN Hardware speed</param>
        /// <param name="hw_type">NON PLUG-N-PLAY: The type of hardware and operation mode</param>
        /// <param name="io_port">NON PLUG-N-PLAY: The I/O address for the parallel port</param>
        /// <param name="interrupt">NON PLUG-N-PLAY: Interrupt number of the parallel port</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_Initialize_2016")]
        public static extern cantp_status Initialize_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_baudrate baudrate,
            [MarshalAs(UnmanagedType.U4)]
            cantp_hwtype hw_type,
            UInt32 io_port,
            UInt16 interrupt);

        /// <summary>
        /// Initializes a PCANTP-Client based on a CANTP handle (without CAN-FD support)
        /// </summary>
        /// <remarks>Only one PCANTP-Client can be initialized per CAN-Channel</remarks>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="baudrate">The CAN Hardware speed</param>
        /// <param name="hw_type">NON PLUG-N-PLAY: The type of hardware and operation mode</param>
        /// <param name="io_port">NON PLUG-N-PLAY: The I/O address for the parallel port</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        public static cantp_status Initialize_2016(
            cantp_handle channel,
            cantp_baudrate baudrate,
            cantp_hwtype hw_type,
            UInt32 io_port)
        {
            return Initialize_2016(channel, baudrate, hw_type, io_port, 0);
        }

        /// <summary>
        /// Initializes a PCANTP-Client based on a CANTP handle (without CAN-FD support)
        /// </summary>
        /// <remarks>Only one PCANTP-Client can be initialized per CAN-Channel</remarks>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="baudrate">The CAN Hardware speed</param>
        /// <param name="hw_type">NON PLUG-N-PLAY: The type of hardware and operation mode</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        public static cantp_status Initialize_2016(
            cantp_handle channel,
            cantp_baudrate baudrate,
            cantp_hwtype hw_type)
        {
            return Initialize_2016(channel, baudrate, hw_type, 0, 0);
        }

        /// <summary>
        /// Initializes a PCANTP-Client based on a CANTP handle (without CAN-FD support)
        /// </summary>
        /// <remarks>Only one PCANTP-Client can be initialized per CAN-Channel</remarks>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="baudrate">The CAN Hardware speed</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        public static cantp_status Initialize_2016(
            cantp_handle channel,
            cantp_baudrate baudrate)
        {
            return Initialize_2016(channel, baudrate, (cantp_hwtype)0, 0, 0);
        }


        /// <summary>
        /// Initializes a PCANTP-Client based on a CANTP Channel (including CAN-FD support)
        /// </summary>
        /// <param name="channel">"The handle of a FD capable PCAN Channel"</param>
        /// <param name="bitrate_fd">"The speed for the communication (FD bit rate string)"</param>
        /// <remarks>Only one PCANTP-Client can be initialized per CAN-Channel.
        /// See PCAN_BR_* values
        /// * Parameter and values must be separated by '='
        /// * Couples of Parameter/value must be separated by ','
        /// * Following Parameter must be filled out: f_clock, data_brp, data_sjw, data_tseg1, data_tseg2,
        ///   nom_brp, nom_sjw, nom_tseg1, nom_tseg2.
        /// * Following Parameters are optional (not used yet): data_ssp_offset, nom_samp
        ///</remarks>
        /// <example>f_clock_mhz=80,nom_brp=0,nom_tseg1=13,nom_tseg2=0,nom_sjw=0,data_brp=0,
        /// data_tseg1=13,data_tseg2=0,data_sjw=0</example>
        /// <returns>"A cantp_status code"</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_InitializeFD_2016")]
        public static extern cantp_status InitializeFD_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.LPStr)]
            cantp_bitrate bitrate_fd);

        /// <summary>
        /// Uninitializes a PCANTP-Client initialized before
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_Uninitialize_2016")]
        public static extern cantp_status Uninitialize_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel);

        /// <summary>
        /// Resets the receive and transmit queues of a PCANTP-Client 
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_Reset_2016")]
        public static extern cantp_status Reset_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel);

        /// <summary>
        /// Gets information about the internal BUS status of a PCANTP-Channel.
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_GetCanBusStatus_2016")]
        public static extern cantp_status GetCanBusStatus_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel);

        /// <summary>
        /// Reads a PCANTP message from the receive queue of a PCANTP-Client
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="msg_buffer">A cantp_msg structure buffer to store the PUDS message</param>
        /// <param name="timestamp_buffer">A cantp_timestamp structure buffer to get 
        /// the reception time of the message.</param>
        /// <param name="msg_type">A cantp_msgtype structure buffer to filter the message to read.</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_Read_2016")]
        public static extern cantp_status Read_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out cantp_msg msg_buffer,
            out cantp_timestamp timestamp_buffer,
            [MarshalAs(UnmanagedType.U4)]
            cantp_msgtype msg_type);

        /// <summary>
        /// Reads a PCANTP message from the receive queue of a PCANTP-Client
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="msg_buffer">A cantp_msg structure buffer to store the PUDS message</param>
        /// <param name="timestamp_buffer">A cantp_timestamp structure buffer to get 
        /// the reception time of the message.</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        public static cantp_status Read_2016(
            cantp_handle channel,
            out cantp_msg msg_buffer,
            out cantp_timestamp timestamp_buffer)
        {
            return Read_2016(channel, out msg_buffer, out timestamp_buffer, cantp_msgtype.PCANTP_MSGTYPE_ANY);
        }

        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_Read_2016")]
        private static extern cantp_status Read_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out cantp_msg msg_buffer,
            IntPtr timestamp_buffer,
            [MarshalAs(UnmanagedType.U4)]
            cantp_msgtype msg_type);

        /// <summary>
        /// Reads a PCANTP message from the receive queue of a PCANTP-Client
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="msg_buffer">A cantp_msg structure buffer to store the PUDS message</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        public static cantp_status Read_2016(
            cantp_handle channel,
            out cantp_msg msg_buffer)
        {
            return Read_2016(channel, out msg_buffer, IntPtr.Zero, cantp_msgtype.PCANTP_MSGTYPE_ANY);
        }

        /// <summary>
        /// Gets progress information on a specific message
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="msg_buffer">A cantp_msg structure buffer matching the message to look for</param>
        /// <param name="direction">The expected direction (incoming/outgoing) of the message</param>
        /// <param name="msgprogress_buffer">A cantp_msgprogress structure buffer to store the progress information</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_GetMsgProgress_2016")]
        public static extern cantp_status GetMsgProgress_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [In]ref cantp_msg msg_buffer,
            [MarshalAs(UnmanagedType.U4)]
            cantp_msgdirection direction,
            out cantp_msgprogress msgprogress_buffer);

        /// <summary>
        /// Adds a PCANTP message to the Transmit queue
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="msg_buffer">A cantp_msg buffer with the message to be sent</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_Write_2016")]
        public static extern cantp_status Write_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            ref cantp_msg msg_buffer);

        /// <summary>
        /// Retrieves a PCANTP-Client value
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="parameter">The cantp_parameter parameter to get</param>
        /// <param name="buffer">Buffer for the parameter value</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_GetValue_2016")]
        public static extern cantp_status GetValue_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_parameter parameter,
            StringBuilder StringBuffer,
            UInt32 BufferLength);

        /// <summary>
        /// Retrieves a PCANTP-Client value
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="parameter">The cantp_parameter parameter to get</param>
        /// <param name="Numericbuffer">Buffer for the parameter value</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_GetValue_2016")]
        public static extern cantp_status GetValue_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_parameter parameter,
            out UInt32 NumericBuffer,
            UInt32 BufferLength);

        /// <summary>
        /// Retrieves a PCANTP-Client value
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="parameter">The cantp_parameter parameter to get</param>
        /// <param name="buffer">Buffer for the parameter value</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_GetValue_2016")]
        public static extern cantp_status GetValue_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_parameter parameter,
            [MarshalAs(UnmanagedType.LPArray)]
            [Out] Byte[] Buffer,
            UInt32 BufferLength);

        /// <summary>
        /// Configures or sets a PCANTP-Client value
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="parameter">The cantp_parameter parameter to set</param>
        /// <param name="NumericBuffer">Buffer with the value to be set</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_SetValue_2016")]
        public static extern cantp_status SetValue_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_parameter parameter,
            ref UInt32 NumericBuffer,
            UInt32 BufferLength);

        /// <summary>
        /// Configures or sets a PCANTP-Client value
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="parameter">The cantp_parameter parameter to set</param>
        /// <param name="StringBuffer">Buffer with the value to be set</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_SetValue_2016")]
        public static extern cantp_status SetValue_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_parameter parameter,
            [MarshalAs(UnmanagedType.LPStr, SizeParamIndex = 3)]
            string StringBuffer,
            UInt32 BufferLength);

        /// <summary>
        /// Configures or sets a PCANTP-Client value
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="parameter">The cantp_parameter parameter to set</param>
        /// <param name="Buffer">Buffer with the value to be set</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_SetValue_2016")]
        public static extern cantp_status SetValue_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_parameter parameter,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            Byte[] Buffer,
            UInt32 BufferLength);

        /// <summary>
        /// Adds a user-defined PCAN-TP mapping between CAN ID and Network Address Information
        /// </summary>
        /// <remark>
        /// Defining a mapping enables ISO-TP communication with 11BITS CAN ID or 
        /// with opened Addressing Formats (like PCANTP_ISOTP_FORMAT_NORMAL or PCANTP_ISOTP_FORMAT_EXTENDED).
        /// </remark>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="mapping">Buffer to the mapping to be added</param>
        /// <returns>A cantp_status code : PCANTP_STATUS_OK is returned on success, 
        /// PCANTP_STATUS_WRONG_PARAM states invalid Network Address Information parameters.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_AddMapping_2016")]
        public static extern cantp_status AddMapping_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            ref cantp_mapping mapping);

        /// <summary>
        /// Removes all user-defined PCAN-TP mappings corresponding to a CAN ID
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="can_id">The mapped CAN ID to search for</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_RemoveMappings_2016")]
        public static extern cantp_status RemoveMappings_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            UInt32 can_id);

        /// <summary>
        /// Removes a user-defined PCAN-TP mapping between a CAN ID and Network Address Information
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="uid">The identifier of the mapping</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_RemoveMapping_2016")]
        public static extern cantp_status RemoveMapping_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            UIntPtr uid);


        /// <summary>
        /// Retrieves all the mappings defined for a PCAN-TP channel
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="buffer">A buffer to store an array of cantp_mapping</param> 
        /// <param name="buffer_length">[In]The number of cantp_mapping element the buffer can store. of the mapping. 
        ///	[Out]The actual number of element copied in the buffer.</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success, 
        ///	PCANTP_STATUS_PARAM_BUFFER_TOO_SMALL if the number of mappings exceeds buffer_length.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_GetMappings_2016")]
        public static extern cantp_status GetMappings_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            [Out] cantp_mapping[] buffer,
            ref UInt32 buffer_length);


        /// <summary>
        /// Adds an entry to the CAN-ID white-list filtering.
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="can_id_from">The lowest CAN ID wanted to be received</param>
        /// <param name="can_id_to">The highest CAN ID wanted to be received</param>
        /// <param name="ignore_can_msgtype">States if filter should check the CAN message type.</param>
        /// <param name="can_msgtype">If ignore_can_msgtype is false, the value states which types of CAN frame should be allowed.</param>
        /// <returns>A cantp_status code : PCANTP_STATUS_OK is returned on success, and PCANTP_STATUS_ALREADY_INITIALIZED otherwise.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_AddFiltering_2016")]
        public static extern cantp_status AddFiltering_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            UInt32 can_id_from,
            UInt32 can_id_to,
            [MarshalAs(UnmanagedType.U1)]
            bool ignore_can_msgtype,
            [MarshalAs(UnmanagedType.U4)]
            cantp_can_msgtype can_msgtype);

        /// <summary>
        /// Removes an entry from the CAN-ID white-list filtering.
        /// </summary>
        /// <param name="channel">A PCANTP Channel Handle representing a PCANTP-Client</param>
        /// <param name="can_id_from">The lowest CAN ID wanted to be removed</param>
        /// <param name="can_id_to">The highest CAN ID wanted to be removed</param>
        /// <param name="ignore_can_msgtype">States if filter should check the CAN message type.</param>
        /// <param name="can_msgtype">If ignore_can_msgtype is false, the value states which types of CAN frame should be allowed.</param>
        /// <returns>A cantp_status code : PCANTP_STATUS_OK is returned on success, and PCANTP_STATUS_ALREADY_INITIALIZED otherwise.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_RemoveFiltering_2016")]
        public static extern cantp_status RemoveFiltering_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            UInt32 can_id_from,
            UInt32 can_id_to,
            [MarshalAs(UnmanagedType.U1)]
            bool ignore_can_msgtype,
            [MarshalAs(UnmanagedType.U4)]
            cantp_can_msgtype can_msgtype);


        /// <summary>
        /// Returns a descriptive text of a given cantp_status error 
        /// code, in any desired language
        /// </summary>
        /// <remarks>The current languages available for translation are: 
        /// Neutral (0x00), German (0x07), English (0x09), Spanish (0x0A),
        /// Italian (0x10) and French (0x0C)</remarks>
        /// <param name="error">A cantp_status error code</param>
        /// <param name="language">Indicates a 'Primary language ID'</param>
        /// <param name="buffer">Buffer for a null terminated char array</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <returns>A cantp_status error code</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_GetErrorText_2016")]
        public static extern cantp_status GetErrorText_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_status error,
            UInt16 language,
            StringBuilder StringBuffer,
            UInt32 bufferSize);


        /// <summary>
        /// Allocates a PCAN-TP message based on the given type
        /// </summary>
        /// <param name="msg_buffer">A cantp_msg structure buffer (it will be freed if required)</param>
        /// <param name="type">Type of the message to allocate</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgDataAlloc_2016")]
        public static extern cantp_status MsgDataAlloc_2016(
            out cantp_msg msg_buffer,
            [MarshalAs(UnmanagedType.U4)]
            cantp_msgtype type);

        /// <summary>
        /// Initializes an allocated PCAN-TP message
        /// </summary>
        /// <param name="msg_buffer">An allocated cantp_msg structure buffer</param>
        /// <param name="can_id">CAN identifier (ISO-TP message may ignore this parameter and use PCANTP_MAPPING_FLOW_CTRL_NONE (-1))</param>
        /// <param name="can_msgtype">Combination of CAN message types (like "extended CAN ID", "FD", "RTR", etc. flags)</param>
        /// <param name="data_length">Length of the data</param>
        /// <param name="data">A buffer to initialize the message's data with. If NULL, message's data is initialized with zeros.</param>
        /// <param name="netaddrinfo">Network address information of the ISO-TP message (only valid with an ISO-TP message)</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgDataInit_2016")]
        public static extern cantp_status MsgDataInit_2016(
            out cantp_msg msg_buffer,
            UInt32 can_id,
            [MarshalAs(UnmanagedType.U4)]
            cantp_can_msgtype can_msgtype,
            UInt32 data_length,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            Byte[] data,
            ref cantp_netaddrinfo netaddrinfo);

        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgDataInit_2016")]
        private static extern cantp_status MsgDataInit_2016(
            out cantp_msg msg_buffer,
            UInt32 can_id,
            [MarshalAs(UnmanagedType.U4)]
            cantp_can_msgtype can_msgtype,
            UInt32 data_length,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            Byte[] data,
            IntPtr netaddrinfo);

        /// <summary>
        /// Initializes an allocated PCAN-TP message
        /// </summary>
        /// <param name="msg_buffer">An allocated cantp_msg structure buffer</param>
        /// <param name="can_id">CAN identifier (ISO-TP message may ignore this parameter and use PCANTP_MAPPING_FLOW_CTRL_NONE (-1))</param>
        /// <param name="can_msgtype">Combination of CAN message types (like "extended CAN ID", "FD", "RTR", etc. flags)</param>
        /// <param name="data_length">Length of the data</param>
        /// <param name="data">A buffer to initialize the message's data with. If NULL, message's data is initialized with zeros.</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        public static cantp_status MsgDataInit_2016(
            out cantp_msg msg_buffer,
            UInt32 can_id,
            cantp_can_msgtype can_msgtype,
            UInt32 data_length,
            Byte[] data)
        {
            return MsgDataInit_2016(out msg_buffer, can_id, can_msgtype, data_length, data, IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a number of options for the PCAN-TP message that will override the channel's parameter(s)
        /// </summary>
        /// <param name="msg_buffer">An allocated cantp_msg structure buffer.</param>
        /// <param name="nb_options">Number of options to initialize.</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgDataInitOptions_2016")]
        public static extern cantp_status MsgDataInitOptions_2016(
            out cantp_msg msg_buffer,
            UInt32 nb_options);

        /// <summary>
        /// Deallocates a PCAN-TP message
        /// </summary>
        /// <param name="msg_buffer">An allocated cantp_msg structure buffer.</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgDataFree_2016")]
        public static extern cantp_status MsgDataFree_2016(
            ref cantp_msg msg_buffer);


        /// <summary>
        /// Checks if two PCAN-TP messages are equal. 
        ///	If one message is the indication of an incoming/outgoing ISO-TP message, the actual data-content will not be compared.
        ///	In that case the function checks if the messages' network address information matches.
        /// </summary>
        /// <param name="msg_buffer1">A cantp_msg structure buffer.</param>
        /// <param name="msg_buffer2">Another cantp_msg structure buffer to compare with first parameter.</param>
        /// <param name="ignoreSelfReceiveFlag">States if comparison should ignore loopback flag 
        ///	(i.e if true the function will return true when comparing a request and its loopback confirmation).</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgEqual_2016")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool MsgEqual_2016(
            [In]ref cantp_msg msg_buffer1,
            [In]ref cantp_msg msg_buffer2,
            [MarshalAs(UnmanagedType.I1)]
            bool ignoreSelfReceiveFlag);

        /// <summary>
        /// Copies a PCAN-TP message to another buffer. 
        /// </summary>
        /// <param name="msg_buffer_dst">A cantp_msg structure buffer to store the copied message.</param>
        /// <param name="msg_buffer_src">The cantp_msg structure buffer used as the source.</param>
        /// <returns>A cantp_status code. PCANTP_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgCopy_2016")]
        public static extern cantp_status MsgCopy_2016(
            out cantp_msg msg_buffer_dst,
            [In]ref cantp_msg msg_buffer_src);


        /// <summary>
        /// Converts a CAN DLC to its corresponding length. 
        /// </summary>
        /// <param name="dlc">The Data Length Code (DLC) to convert.</param>
        /// <returns>The corresponding length of the dlc parameter.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgDlcToLength_2016")]
        public static extern UInt32 MsgDlcToLength_2016(
            byte dlc);

        /// <summary>
        /// Converts a data length to a corresponding CAN DLC. 
        ///	Note: the returned DLC can hold more data that the requested length.
        /// </summary>
        /// <param name="dlc">The length to convert.</param>
        /// <returns>The smallest DLC that can hold the requested length (0x00-0x0F).</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_MsgLengthToDlc_2016")]
        public static extern byte MsgLengthToDlc_2016(
            UInt32 length);

        /// <summary>
        /// Lists the subtypes contained in the PCAN-TP status. 
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <returns>An aggregation of cantp_statustype values.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_StatusListTypes_2016")]
        public static extern cantp_statustype StatusListTypes_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_status status);

        /// <summary>
        /// Retrieves the value of a cantp_status subtype (like cantp_errstatus, cantp_busstatus, etc.). 
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <param name="type">The type of status to filter.</param>
        /// <returns>The value of the enumeration matching the requested type.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_StatusGet_2016")]
        public static extern UInt32 StatusGet_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_status status,
            [MarshalAs(UnmanagedType.U4)]
            cantp_statustype type);

        /// <summary>
        /// Checks if a status matches an expected result. 
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <param name="status_expected">The expected status.</param>
        /// <param name="strict">Enable strict mode. Strict mode ensures that bus or extra information are the same.</param>
        /// <returns>Returns true if the status matches expected parameter.</returns>
        [DllImport("PCAN-ISO-TP.dll", EntryPoint = "CANTP_StatusIsOk_2016")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool StatusIsOk_2016(
            [MarshalAs(UnmanagedType.U4)]
            cantp_status status,
            [MarshalAs(UnmanagedType.U4)]
            cantp_status status_expected,
            [MarshalAs(UnmanagedType.I1)]
            bool strict);

        /// <summary>
        /// Checks if a status matches an expected result in non-strict mode. 
        /// (Strict mode ensures that bus or extra information are the same)
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <param name="status_expected">The expected status.</param>
        /// <returns>Returns true if the status matches expected parameter.</returns>
        public static bool StatusIsOk_2016(
            cantp_status status,
            cantp_status status_expected
            )
        {
            return StatusIsOk_2016(status, status_expected, false);
        }

        /// <summary>
        /// Checks if a status matches PCANTP_STATUS_OK in non-strict mode. 
        /// (Strict mode ensures that bus or extra information are the same)
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <returns>Returns true if the status matches expected parameter.</returns>
        public static bool StatusIsOk_2016(
            cantp_status status
            )
        {
            return StatusIsOk_2016(status, cantp_status.PCANTP_STATUS_OK, false);
        }

        #region special C# functions, examples of how to use the structures IntPtr fields  in safe mode, with Marshaling operations

        /// <summary>
        /// Get an option value of an option list
        /// </summary>
        /// <param name="l">option list</param>
        /// <param name="number">number of the option</param>
        /// <param name="option">where to store a copy of the option</param>
        /// <returns>true if ok, false if not ok</returns>
        private static bool getOption_2016(ref cantp_msgoption_list l, int number, out cantp_msgoption option)
        {
            option = new cantp_msgoption();
            if (number < l.count && l.buffer != IntPtr.Zero)
            {
                option.name = (cantp_option)Marshal.ReadInt32(l.buffer, number * 8);
                option.value = (UInt32)Marshal.ReadInt32(l.buffer, number * 8 + 4);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Modifies an option
        /// </summary>
        /// <param name="l">option list</param>
        /// <param name="number">number of the option</param>
        /// <param name="option">value to set to the option</param>
        /// <returns>true if ok, false if not ok</returns>
        private static bool setOption_2016(ref cantp_msgoption_list l, int number, ref cantp_msgoption option)
        {
            if (number < l.count && l.buffer != IntPtr.Zero)
            {
                Marshal.WriteInt32(l.buffer, number * 8, (Int32)option.name);
                Marshal.WriteInt32(l.buffer, number * 8 + 4, (Int32)option.value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// in a progress structure, allocate a buffer to receive a copy of the pending message.
        /// when finished, must be released with freeProgressBuffer_2016()
        /// </summary>
        /// <param name="prog">progress structure</param>
        /// <param name="type">type of the pending message</param>
        /// <returns>resulting status</returns>
        public static cantp_status allocProgressBuffer_2016(ref cantp_msgprogress prog, cantp_msgtype type)
        {
            cantp_msg pendingMsg;
            cantp_status status = CanTpApi.MsgDataAlloc_2016(out pendingMsg, type);
            if (status == cantp_status.PCANTP_STATUS_OK)
            {
                try
                {
                    // Initialize unmanaged memory to hold the struct.
                    prog.buffer = Marshal.AllocHGlobal(Marshal.SizeOf(pendingMsg));
                }
                catch
                {
                    prog.buffer = IntPtr.Zero;
                }
                if (prog.buffer != IntPtr.Zero)
                    Marshal.StructureToPtr(pendingMsg, prog.buffer, true);
                else
                    status = cantp_status.PCANTP_STATUS_NO_MEMORY;
            }
            return status;
        }

        /// <summary>
        /// free the buffer receiving the pending message in a progress object,
        /// if allocated with allocProgressBuffer_2016()
        /// </summary>
        /// <param name="prog">progress object</param>
        public static void freeProgressBuffer_2016(ref cantp_msgprogress prog)
        {
            // Free unmanaged memory holding the struct.
            if (prog.buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(prog.buffer);
                prog.buffer = IntPtr.Zero;
            }
        }

        /// <summary>
        /// get the current pending message of a progress structure.
        /// </summary>
        /// <param name="prog">progress structure</param>
        /// <param name="pendmsg">pending message</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getProgressBuffer_2016(ref cantp_msgprogress prog, ref cantp_msg pendmsg)
        {
            if (prog.buffer != IntPtr.Zero)
            {
                pendmsg = (cantp_msg)Marshal.PtrToStructure(prog.buffer, typeof(cantp_msg));
                return true;
            }
            return false;
        }

        /// <summary>
        /// get the flags of a message
        /// </summary>
		/// <param name="msg">Message structure</param>
        /// <param name="flags">value of the flags</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getFlags_2016(ref cantp_msg msg, out cantp_msgflag flags)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                flags = msg.Msgdata_any_Copy.flags;
                return true;
            }
            flags = cantp_msgflag.PCANTP_MSGFLAG_NONE;
            return false;
        }

        /// <summary>
        /// set the length of a message
        /// use carefully
        /// </summary>
        /// <param name="len">value to set</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool setLength_2016(ref cantp_msg msg, UInt32 len)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = (cantp_msgdata)Marshal.PtrToStructure(msg.Msgdata, typeof(cantp_msgdata));
                msgdata.length = len;
                Marshal.StructureToPtr(msgdata, msg.Msgdata, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// get the length of a message
        /// </summary>
		/// <param name="msg">Message structure</param>
        /// <param name="len">value of the length</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getLength_2016(ref cantp_msg msg, out UInt32 len)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                len = msg.Msgdata_any_Copy.length;
                return true;
            }
            len = 0;
            return false;
        }

        /// <summary>
        /// set a byte of the data of a message
        /// </summary>
		/// <param name="msg">Message structure to modify.</param>
        /// <param name="i">offset of the byte, cannot be more than 2147483647</param> 
        /// <param name="val"> value to set</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool setData_2016(ref cantp_msg msg, Int32 i, byte val)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = msg.Msgdata_any_Copy;
                if (msgdata.length > 0 && msgdata.data != IntPtr.Zero && i >= 0 && i < msgdata.length)
                {
                    Marshal.WriteByte(msgdata.data, i, val);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// set bytes of the data of a message from a contiguous byte array
        /// </summary>
		/// <param name="msg">Message structure to modify.</param>
        /// <param name="i">offset of the first byte in the message, cannot be more than 2147483647</param> 
        /// <param name="nb">number of bytes</param> 
        /// <param name="vals">values to set, from offset 0</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool setData_2016(ref cantp_msg msg, Int32 i, byte[] vals, Int32 nb)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = msg.Msgdata_any_Copy;
                if (msgdata.length > 0 && msgdata.data != IntPtr.Zero && i >= 0 && i + nb - 1 < msgdata.length && vals.Length >= nb)
                {
                    if (i == 0)
                    {
                        Marshal.Copy(vals, 0, msgdata.data, nb);
                    }
                    else
                    {
                        for (Int32 j = 0; j < nb; j++)
                            Marshal.WriteByte(msgdata.data, j + i, vals[j]);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// get a byte of the data of a message
        /// </summary>
		/// <param name="msg">Message structure containing data.</param>
        /// <param name="i">offset of the bytes in the message, cannot be more than 2147483647</param> 
        /// <param name="val"> value of the byte</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getData_2016(ref cantp_msg msg, Int32 i, out byte val)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = msg.Msgdata_any_Copy;
                if (msgdata.length > 0 && msgdata.data != IntPtr.Zero && i >= 0 && i < msgdata.length)
                {
                    val = Marshal.ReadByte(msgdata.data, i);
                    return true;
                }
            }
            val = 0;
            return false;
        }

        /// <summary>
        /// get bytes of the data of a message
        /// </summary>
		/// <param name="msg">Message structure containing data.</param>
        /// <param name="i">offset of the first byte to get in the message, cannot be more than 2147483647</param> 
        /// <param name="nb">number of bytes</param> 
        /// <param name="vals"> values of the bytes, from offset 0</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getData_2016(ref cantp_msg msg, Int32 i, byte[] vals, Int32 nb)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = msg.Msgdata_any_Copy;
                if (msgdata.length > 0 && msgdata.data != IntPtr.Zero && i >= 0 && i + nb - 1 < msgdata.length && nb <= vals.Length)
                {
                    if (i == 0)
                    {
                        Marshal.Copy(msgdata.data, vals, 0, nb);
                    }
                    else
                    {
                        for (Int32 j = 0; j < nb; j++)
                            vals[j] = Marshal.ReadByte(msgdata.data, j + i);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// get the net status of a message
        /// </summary>
		/// <param name="msg">Message structure containing the network status</param>
        /// <param name="status">value of the status</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getNetStatus_2016(ref cantp_msg msg, out cantp_netstatus status)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                status = msg.Msgdata_any_Copy.netstatus;
                return true;
            }
            status = new cantp_netstatus();
            return false;
        }

        /// <summary>
        /// Get an option of a message
        /// </summary>
        /// <param name="number">number of the option</param>
        /// <param name="option">where to store a copy of the option</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getOption_2016(ref cantp_msg msg, int number, out cantp_msgoption option)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = msg.Msgdata_any_Copy;
                if (msgdata.options != IntPtr.Zero)
                {
                    cantp_msgoption_list l = (cantp_msgoption_list)Marshal.PtrToStructure(msgdata.options, typeof(cantp_msgoption_list));
                    return getOption_2016(ref l, number, out option);
                }
            }
            option = new cantp_msgoption();
            return false;
        }

        /// <summary>
        /// Modifies an option of a message
        /// </summary>
		/// <param name="msg">Message structure to modify</param>
        /// <param name="number">number of the option</param>
        /// <param name="option">value to set to the option</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool setOption_2016(ref cantp_msg msg, int number, ref cantp_msgoption option)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = msg.Msgdata_any_Copy;
                if (msgdata.options != IntPtr.Zero)
                {
                    cantp_msgoption_list l = (cantp_msgoption_list)Marshal.PtrToStructure(msgdata.options, typeof(cantp_msgoption_list));
                    return setOption_2016(ref l, number, ref option);
                }
            }
            return false;
        }

        /// <summary>
        /// Get the number of options of a message
        /// </summary>
        /// <param name="msg">message</param>
        /// <param name="number">number of options</param>
        /// <returns>true if ok, false if Not ok</returns>
        public static bool getOptionsNumber_2016(ref cantp_msg msg, out UInt32 number)
        {
            if (msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata msgdata = msg.Msgdata_any_Copy;
                if (msgdata.options != IntPtr.Zero)
                {
                    cantp_msgoption_list l = (cantp_msgoption_list)Marshal.PtrToStructure(msgdata.options, typeof(cantp_msgoption_list));
                    number = l.count;
                    return true;
                }
            }
            number = 0;
            return false;
        }

        /// <summary>
        /// set the network address information of an ISO-TP message
        /// </summary>
        /// <param name="adr">address to set</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool setNetaddrinfo_2016(ref cantp_msg msg, ref cantp_netaddrinfo adr)
        {
            if (msg.type == cantp_msgtype.PCANTP_MSGTYPE_ISOTP && msg.Msgdata != IntPtr.Zero)
            {
                cantp_msgdata_isotp msgdata = (cantp_msgdata_isotp)Marshal.PtrToStructure(msg.Msgdata, typeof(cantp_msgdata_isotp));
                msgdata.netaddrinfo = adr;
                Marshal.StructureToPtr(msgdata, msg.Msgdata, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// get the network address information of an ISO-TP message
        /// </summary>
        /// <param name="adr">value of address</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool getNetaddrinfo_2016(ref cantp_msg msg, out cantp_netaddrinfo adr)
        {
            if (msg.type == cantp_msgtype.PCANTP_MSGTYPE_ISOTP && msg.Msgdata != IntPtr.Zero)
            {
                adr = msg.Msgdata_isotp_Copy.netaddrinfo;
                return true;
            }
            adr = new cantp_netaddrinfo();
            return false;
        }
        #endregion


        #region special C# functions, examples of how to use the structures IntPtr fields  in unsafe mode

#if (UNSAFE)

        /// <summary>
        /// Get a copy of an option in an option list
        /// </summary>
        /// <param name="l">option list</param>
        /// <param name="number">number of the option</param>
        /// <param name="option">where to store a copy of the option</param>
        /// <returns>true if ok, false if not ok</returns>
        private unsafe static bool getOption_unsafe_2016(cantp_msgoption_list* l, int number, out cantp_msgoption option)
        {
            option = new cantp_msgoption();
            if (l != null && number < l->count && l->buffer != IntPtr.Zero)
            {
                cantp_msgoption* opts = (cantp_msgoption*)l->buffer.ToPointer();
                if (opts != null)
                {
                    option = opts[number];
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Modifies an option
        /// </summary>
        /// <param name="l">option list</param>
        /// <param name="number">number of the option</param>
        /// <param name="option">value to set to the option</param>
        /// <returns>true if ok, false if not ok</returns>
        private unsafe static bool setOption_unsafe_2016(cantp_msgoption_list* l, int number, ref cantp_msgoption option)
        {
            if (l != null && number < l->count && l->buffer != IntPtr.Zero)
            {
                cantp_msgoption* opts = (cantp_msgoption*)l->buffer.ToPointer();
                if (opts != null)
                {
                    opts[number] = option;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// set a buffer to receive a copy of the pending message, in a progress structure
        /// </summary>
        /// <param name="pendingMsg">buffer to set</param>
        public unsafe static void setProgressBuffer_unsafe_2016(ref cantp_msgprogress prog, ref cantp_msg pendingMsg)
        {
            fixed (cantp_msg* pt = &pendingMsg) { prog.buffer = new IntPtr(pt); }
        }

        /// <summary>
        /// get the flags of a message
        /// </summary>
		/// <param name="msg">Message structure</param>
        /// <param name="flags">value of the flags</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getFlags_unsafe_2016(ref cantp_msg msg, out cantp_msgflag flags)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null)
            {
                flags = msg.Msgdata_any->flags;
                return true;
            }
            flags = new cantp_msgflag();
            return false;
        }

        /// <summary>
        /// set the length of a message
        /// use carefully
        /// </summary>
        /// <param name="len">value to set</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool setLength_unsafe_2016(ref cantp_msg msg, UInt32 len)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null)
            {
                msg.Msgdata_any->length = len;
                return true;
            }
            return false;
        }

        /// <summary>
        /// get the length of a message
        /// </summary>
		/// <param name="msg">Message structure</param>
        /// <param name="len">value of the length</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getLength_unsafe_2016(ref cantp_msg msg, out UInt32 len)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null)
            {
                len = msg.Msgdata_any->length;
                return true;
            }
            len = 0;
            return false;
        }

        /// <summary>
        /// set a byte of the data of a message
        /// </summary>
		/// <param name="msg">Message structure to modify.</param>
        /// <param name="i">offset of the byte, cannot be more than 2147483647</param> 
        /// <param name="val"> value to set</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool setData_unsafe_2016(ref cantp_msg msg, Int32 i, byte val)
        {

            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null && msg.Msgdata_any->data != IntPtr.Zero)
            {
                byte* don = (byte*)msg.Msgdata_any->data.ToPointer();
                if (don != null && msg.Msgdata_any->length > 0 && i >= 0 && i < msg.Msgdata_any->length)
                {
                    don[i] = val;
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// set bytes of the data of a message
        /// </summary>
		/// <param name="msg">Message structure to modify.</param>
        /// <param name="i">offset of the first byte in the message, cannot be more than 2147483647</param> 
        /// <param name="nb">number of bytes</param> 
        /// <param name="vals"> values to set, from offset 0</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool setData_unsafe_2016(ref cantp_msg msg, Int32 i, byte[] vals, Int32 nb)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null && msg.Msgdata_any->data != IntPtr.Zero)
            {
                byte* don = (byte*)msg.Msgdata_any->data.ToPointer();
                if (don != null && msg.Msgdata_any->length > 0 && i >= 0 && i + nb - 1 < msg.Msgdata_any->length && nb <= vals.Length)
                {
                    for (Int32 j = 0; j < nb; j++)
                        don[j + i] = vals[j];
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// get a byte of the data of a message
        /// </summary>
		/// <param name="msg">Message structure containing data.</param>
        /// <param name="i">offset of the byte, cannot be more than 2147483647</param> 
        /// <param name="val"> value of the byte</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getData_unsafe_2016(ref cantp_msg msg, Int32 i, out byte val)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null && msg.Msgdata_any->data != IntPtr.Zero)
            {
                byte* don = (byte*)msg.Msgdata_any->data.ToPointer();
                if (don != null && msg.Msgdata_any->length > 0 && i >= 0 && i < msg.Msgdata_any->length)
                {
                    val = don[i];
                    return true;
                }
            }
            val = 0;
            return false;

        }

        /// <summary>
        /// get bytes of the data of a message
        /// </summary>
		/// <param name="msg">Message structure containing data.</param>
        /// <param name="i">offset of the first byte in the message, cannot be more than 2147483647</param> 
        /// <param name="nb">number of bytes</param> 
        /// <param name="vals"> values of the bytes, from offset 0</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getData_unsafe_2016(ref cantp_msg msg, Int32 i, byte[] vals, Int32 nb)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null && msg.Msgdata_any->data != IntPtr.Zero)
            {
                byte* don = (byte*)msg.Msgdata_any->data.ToPointer();
                if (don != null && msg.Msgdata_any->length > 0 && i >= 0 && i + nb - 1 < msg.Msgdata_any->length && vals.Length >= nb)
                {
                    for (Int32 j = 0; j < nb; j++)
                        vals[j] = don[j + i];
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// get the net status of a message
        /// </summary>
		/// <param name="msg">Message structure containing the network status</param>
        /// <param name="status">value of the status</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getNetStatus_unsafe_2016(ref cantp_msg msg, out cantp_netstatus status)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null)
            {
                status = msg.Msgdata_any->netstatus;
                return true;
            }
            status = new cantp_netstatus();
            return false;
        }

        /// <summary>
        /// Get a copy of an option of a message
        /// </summary>
        /// <param name="number">number of the option</param>
        /// <param name="option">where to store a copy of the option</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getOption_unsafe_2016(ref cantp_msg msg, int number, out cantp_msgoption option)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null && msg.Msgdata_any->options != IntPtr.Zero)
            {
                return getOption_unsafe_2016((cantp_msgoption_list*)msg.Msgdata_any->options.ToPointer(), number, out option);
            }

            option = new cantp_msgoption();
            return false;
        }

        /// <summary>
        /// Modifies an option of a message
        /// </summary>
        /// <param name="number">number of the option</param>
        /// <param name="option">value to set to the option</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool setOption_unsafe_2016(ref cantp_msg msg, int number, ref cantp_msgoption option)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null && msg.Msgdata_any->options != IntPtr.Zero)
            {
                return setOption_unsafe_2016((cantp_msgoption_list*)msg.Msgdata_any->options.ToPointer(), number, ref option);
            }
            return false;
        }

        /// <summary>
        /// Get the number of options of a message
        /// </summary>
        /// <param name="msg">message</param>
        /// <param name="number">number of options</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getOptionsNumber_unsafe_2016(ref cantp_msg msg, out UInt32 number)
        {
            if (msg.Msgdata != IntPtr.Zero && msg.Msgdata_any != null && msg.Msgdata_any->options != IntPtr.Zero)
            {
                cantp_msgoption_list* l = (cantp_msgoption_list*)msg.Msgdata_any->options.ToPointer();
                if (l != null)
                {
                    number = l->count;
                    return true;
                }
            }
            number = 0;
            return false;
        }

        /// <summary>
        /// set the network address information of an ISO-TP message
        /// </summary>
        /// <param name="adr">address to set</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool setNetaddrinfo_unsafe_2016(ref cantp_msg msg, ref cantp_netaddrinfo adr)
        {

            if (msg.type == cantp_msgtype.PCANTP_MSGTYPE_ISOTP && msg.Msgdata != IntPtr.Zero && msg.Msgdata_isotp != null)
            {
                msg.Msgdata_isotp->netaddrinfo = adr;
                return true;
            }
            return false;

        }

        /// <summary>
        /// get the network address information of an ISO-TP message
        /// </summary>
        /// <param name="adr">value of address</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool getNetaddrinfo_unsafe_2016(ref cantp_msg msg, out cantp_netaddrinfo adr)
        {
            if (msg.type == cantp_msgtype.PCANTP_MSGTYPE_ISOTP && msg.Msgdata != IntPtr.Zero && msg.Msgdata_isotp != null)
            {
                adr = msg.Msgdata_isotp->netaddrinfo;
                return true;
            }
            adr = new cantp_netaddrinfo();
            return false;
        }
#endif
        #endregion

        #endregion


    }

    #endregion

}