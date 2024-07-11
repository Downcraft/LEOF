//  PCAN-UDS_2013.cs
//
//  ~~~~~~~~~~~~
//
//  PCAN-UDS API 2013 (ISO 14229-1:2013)
//
//  ~~~~~~~~~~~~
//
//  ------------------------------------------------------------------
//  Author : Fabrice Vergnaud
//  Last changed by:    $Author: Fabrice $
//  Last changed date:  $Date: 2021-08-19 16:58:13 +0200 (Thu, 19 Aug 2021) $
//
//  Language: C#
//  ------------------------------------------------------------------
//
//  Copyright (C) 2020  PEAK-System Technik GmbH, Darmstadt
//  more Info at http://www.peak-system.com
//

// To use PCAN-UDS_2006 and PCAN-UDS_2013 together: do define PUDS_API_COMPATIBILITY_ISO_2006"
// #define PUDS_API_COMPATIBILITY_ISO_2006

// To accept unsafe functions, define UNSAFE
//#define UNSAFE

using System;
using System.Text;
using System.Runtime.InteropServices;

using Peak.Can.Basic;
using Peak.Can.IsoTp;

using cantp_bitrate = System.String;    // Represents a PCAN-FD bit rate string
using cantp_timestamp = System.UInt64;  // Timestamp

namespace Peak.Can.Uds
{
    #region Enumerations
    ////////////////////////////////////////////////////////////
    // Enums definition for UDS API
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// Represents PUDS error codes (used in uds_status)
    /// </summary>
    public enum uds_errstatus : byte
    {
        PUDS_ERRSTATUS_SERVICE_NO_MESSAGE = 1,
        PUDS_ERRSTATUS_SERVICE_TIMEOUT_CONFIRMATION,
        PUDS_ERRSTATUS_SERVICE_TIMEOUT_RESPONSE,
        PUDS_ERRSTATUS_RESET,
        PUDS_ERRSTATUS_ERROR_WAIT_FOR_P3_TIMING,
        PUDS_ERRSTATUS_SERVICE_ALREADY_PENDING,
        PUDS_ERRSTATUS_SERVICE_TX_ERROR,
        PUDS_ERRSTATUS_SERVICE_RX_ERROR,
        PUDS_ERRSTATUS_SERVICE_RX_OVERFLOW,
        PUDS_ERRSTATUS_MESSAGE_BUFFER_ALREADY_USED
    }

    /// <summary>
    /// Defines constants used by the next enum: uds_status
    /// </summary>
    public enum uds_status_offset : byte
    {
        PCANTP_STATUS_OFFSET_BUS = 8,
        PCANTP_STATUS_OFFSET_NET = (PCANTP_STATUS_OFFSET_BUS + 5),
        PCANTP_STATUS_OFFSET_INFO = (PCANTP_STATUS_OFFSET_NET + 5),
        PCANTP_STATUS_OFFSET_UDS = (PCANTP_STATUS_OFFSET_INFO + 6)
    }

    /// <summary>
    /// Represents the PCANTP & UDS error and status codes.
    ///
    /// Bits information:
    ///   32|  28|  24|  20|  16|  12|   8|   4|   0|
    ///     |    |    |    |    |    |    |    |    |
    ///      0000 0000 0000 0000 0000 0000 0000 0000
    ///     |    |    |    |    |         [0000 0000] => PCAN-ISO-TP API errors
    ///     |    |    |    |    |  [0 0000]           => CAN Bus status
    ///     |    |    |    | [00 000]                 => Networking message status
    ///     |    |    [0000 00]                       => PCAN-ISO-TP API extra information
    ///     |  [0 0000]                               => API Status
    ///     | [0]                                     => UDS Status
    ///     |[0]                                      => Reserved
    ///     [0]                                       => PCANBasic error flag
    /// </summary>
    public enum uds_status : UInt32
    {
        /// <summary>
        /// No error
        /// </summary>
        PUDS_STATUS_OK = cantp_status.PCANTP_STATUS_OK,
        /// <summary>
        /// Not Initialized.
        /// </summary>
        PUDS_STATUS_NOT_INITIALIZED = cantp_status.PCANTP_STATUS_NOT_INITIALIZED,
        /// <summary>
        /// Already Initialized.
        /// </summary>
        PUDS_STATUS_ALREADY_INITIALIZED = cantp_status.PCANTP_STATUS_ALREADY_INITIALIZED,
        /// <summary>
        /// Could not obtain memory.
        /// </summary>
        PUDS_STATUS_NO_MEMORY = cantp_status.PCANTP_STATUS_NO_MEMORY,
        /// <summary>
        /// Input buffer overflow.
        /// </summary>
        PUDS_STATUS_OVERFLOW = cantp_status.PCANTP_STATUS_OVERFLOW,
        /// <summary>
        /// No message available.
        /// </summary>
        PUDS_STATUS_NO_MESSAGE = cantp_status.PCANTP_STATUS_NO_MESSAGE,
        /// <summary>
        /// Wrong message parameters.
        /// </summary>
        PUDS_STATUS_PARAM_INVALID_TYPE = cantp_status.PCANTP_STATUS_PARAM_INVALID_TYPE,
        /// <summary>
        /// Wrong message parameters.
        /// </summary>
        PUDS_STATUS_PARAM_INVALID_VALUE = cantp_status.PCANTP_STATUS_PARAM_INVALID_VALUE,
        /// <summary>
        /// Mapping not initialized.
        /// </summary>
        PUDS_STATUS_MAPPING_NOT_INITIALIZED = cantp_status.PCANTP_STATUS_MAPPING_NOT_INITIALIZED,
        /// <summary>
        /// Mapping parameters are invalid.
        /// </summary>
        PUDS_STATUS_MAPPING_INVALID = cantp_status.PCANTP_STATUS_MAPPING_INVALID,
        /// <summary>
        /// Mapping already defined.
        /// </summary>
        PUDS_STATUS_MAPPING_ALREADY_INITIALIZED = cantp_status.PCANTP_STATUS_MAPPING_ALREADY_INITIALIZED,
        /// <summary>
        /// Buffer is too small.
        /// </summary>
        PUDS_STATUS_PARAM_BUFFER_TOO_SMALL = cantp_status.PCANTP_STATUS_PARAM_BUFFER_TOO_SMALL,
        /// <summary>
        /// Tx queue is full.
        /// </summary>
        PUDS_STATUS_QUEUE_TX_FULL = cantp_status.PCANTP_STATUS_QUEUE_TX_FULL,
        /// <summary>
        /// Failed to get an access to the internal lock.
        /// </summary>
        PUDS_STATUS_LOCK_TIMEOUT = cantp_status.PCANTP_STATUS_LOCK_TIMEOUT,
        /// <summary>
        /// Invalid cantp_handle.
        /// </summary>
        PUDS_STATUS_HANDLE_INVALID = cantp_status.PCANTP_STATUS_HANDLE_INVALID,
        /// <summary>
        /// Unknown/generic error.
        /// </summary>
        PUDS_STATUS_UNKNOWN = cantp_status.PCANTP_STATUS_UNKNOWN,

        // Bus status flags (bits [8..11])

        /// <summary>
        /// Channel is in BUS - LIGHT error state.
        /// </summary>
        PUDS_STATUS_FLAG_BUS_LIGHT = cantp_status.PCANTP_STATUS_FLAG_BUS_LIGHT,
        /// <summary>
        /// Channel is in BUS - HEAVY error state.
        /// </summary>
        PUDS_STATUS_FLAG_BUS_HEAVY = cantp_status.PCANTP_STATUS_FLAG_BUS_HEAVY,
        /// <summary>
        /// Channel is in BUS - HEAVY error state.
        /// </summary>
        PUDS_STATUS_FLAG_BUS_WARNING = cantp_status.PCANTP_STATUS_FLAG_BUS_WARNING,
        /// <summary>
        /// Channel is error passive state.
        /// </summary>
        PUDS_STATUS_FLAG_BUS_PASSIVE = cantp_status.PCANTP_STATUS_FLAG_BUS_PASSIVE,
        /// <summary>
        /// Channel is in BUS - OFF error state.
        /// </summary>
        PUDS_STATUS_FLAG_BUS_OFF = cantp_status.PCANTP_STATUS_FLAG_BUS_OFF,
        /// <summary>
        /// Mask for all bus errors.
        /// </summary>
        PUDS_STATUS_FLAG_BUS_ANY = cantp_status.PCANTP_STATUS_FLAG_BUS_ANY,
        /// <summary>
        /// This flag states if one of the following network errors occurred with the fetched message.
        /// </summary>
        PUDS_STATUS_FLAG_NETWORK_RESULT = cantp_status.PCANTP_STATUS_FLAG_NETWORK_RESULT,

        // Network status (bits [13..17])

        /// <summary>
        /// Timeout occurred between 2 frames transmission (sender and receiver side).
        /// </summary>
        PUDS_STATUS_NETWORK_TIMEOUT_A = cantp_status.PCANTP_STATUS_NETWORK_TIMEOUT_A,
        /// <summary>
        /// Sender side timeout while waiting for flow control frame.
        /// </summary>
        PUDS_STATUS_NETWORK_TIMEOUT_Bs = cantp_status.PCANTP_STATUS_NETWORK_TIMEOUT_Bs,
        /// <summary>
        /// Receiver side timeout while waiting for consecutive frame.
        /// </summary>
        PUDS_STATUS_NETWORK_TIMEOUT_Cr = cantp_status.PCANTP_STATUS_NETWORK_TIMEOUT_Cr,
        /// <summary>
        /// Unexpected sequence number.
        /// </summary>
        PUDS_STATUS_NETWORK_WRONG_SN = cantp_status.PCANTP_STATUS_NETWORK_WRONG_SN,
        /// <summary>
        /// Invalid or unknown FlowStatus.
        /// </summary>
        PUDS_STATUS_NETWORK_INVALID_FS = cantp_status.PCANTP_STATUS_NETWORK_INVALID_FS,
        /// <summary>
        /// Unexpected protocol data unit.
        /// </summary>
        PUDS_STATUS_NETWORK_UNEXP_PDU = cantp_status.PCANTP_STATUS_NETWORK_UNEXP_PDU,
        /// <summary>
        /// Reception of flow control WAIT frame that exceeds the maximum counter defined by PUDS_PARAMETER_WFT_MAX.
        /// </summary>
        PUDS_STATUS_NETWORK_WFT_OVRN = cantp_status.PCANTP_STATUS_NETWORK_WFT_OVRN,
        /// <summary>
        /// Buffer on the receiver side cannot store the data length (server side only).
        /// </summary>
        PUDS_STATUS_NETWORK_BUFFER_OVFLW = cantp_status.PCANTP_STATUS_NETWORK_BUFFER_OVFLW,
        /// <summary>
        /// General error.
        /// </summary>
        PUDS_STATUS_NETWORK_ERROR = cantp_status.PCANTP_STATUS_NETWORK_ERROR,
        /// <summary>
        /// Message was invalid and ignored.
        /// </summary>
        PUDS_STATUS_NETWORK_IGNORED = cantp_status.PCANTP_STATUS_NETWORK_IGNORED,
        /// <summary>
        /// Receiver side timeout while transmitting.
        /// </summary>
        PUDS_STATUS_NETWORK_TIMEOUT_Ar = cantp_status.PCANTP_STATUS_NETWORK_TIMEOUT_Ar,
        /// <summary>
        /// Sender side timeout while transmitting.
        /// </summary>
        PUDS_STATUS_NETWORK_TIMEOUT_As = cantp_status.PCANTP_STATUS_NETWORK_TIMEOUT_As,

        // Extra information flags

        /// <summary>
        /// Input was modified by the API.
        /// </summary>
        PUDS_STATUS_CAUTION_INPUT_MODIFIED = cantp_status.PCANTP_STATUS_CAUTION_INPUT_MODIFIED,
        /// <summary>
        /// DLC value of the input was modified by the API.
        /// </summary>
        PUDS_STATUS_CAUTION_DLC_MODIFIED = cantp_status.PCANTP_STATUS_CAUTION_DLC_MODIFIED,
        /// <summary>
        /// Data Length value of the input was modified by the API.
        /// </summary>
        PUDS_STATUS_CAUTION_DATA_LENGTH_MODIFIED = cantp_status.PCANTP_STATUS_CAUTION_DATA_LENGTH_MODIFIED,
        /// <summary>
        /// FD flags of the input was modified by the API.
        /// </summary>
        PUDS_STATUS_CAUTION_FD_FLAG_MODIFIED = cantp_status.PCANTP_STATUS_CAUTION_FD_FLAG_MODIFIED,
        /// <summary>
        /// Receive queue is full.
        /// </summary>
        PUDS_STATUS_CAUTION_RX_QUEUE_FULL = cantp_status.PCANTP_STATUS_CAUTION_RX_QUEUE_FULL,
        /// <summary>
        /// Buffer is used by another thread or API.
        /// </summary>
        PUDS_STATUS_CAUTION_BUFFER_IN_USE = cantp_status.PCANTP_STATUS_CAUTION_BUFFER_IN_USE,

        // Lower API status code: see also PCANTP_STATUS_xx macros

        /// <summary>
        /// PCAN error flag, remove flag to get a usable PCAN error/status code (cf. PCANBasic API).
        /// </summary>
        PUDS_STATUS_FLAG_PCAN_STATUS = cantp_status.PCANTP_STATUS_FLAG_PCAN_STATUS,

        // Masks to merge/retrieve different status by type in a uds_status

        /// <summary>
        /// Filter general error.
        /// </summary>
        PUDS_STATUS_MASK_ERROR = cantp_status.PCANTP_STATUS_MASK_ERROR,
        /// <summary>
        /// Filter bus error.
        /// </summary>
        PUDS_STATUS_MASK_BUS = cantp_status.PCANTP_STATUS_MASK_BUS,
        /// <summary>
        /// Filter network error.
        /// </summary>
        PUDS_STATUS_MASK_ISOTP_NET = cantp_status.PCANTP_STATUS_MASK_ISOTP_NET,
        /// <summary>
        /// Filter extra information.
        /// </summary>
        PUDS_STATUS_MASK_INFO = cantp_status.PCANTP_STATUS_MASK_INFO,
        /// <summary>
        /// Filter PCAN error (encapsulated PCAN-Basic status).
        /// </summary>
        PUDS_STATUS_MASK_PCAN = cantp_status.PCANTP_STATUS_MASK_PCAN,

        // UDS service status.

        /// <summary>
        /// UDS error flag.
        /// </summary>
        PUDS_STATUS_FLAG_UDS_ERROR = 0x20 << uds_status_offset.PCANTP_STATUS_OFFSET_UDS,
        /// <summary>
        /// Filter UDS error.
        /// </summary>
        PUDS_STATUS_MASK_UDS_ERROR = (UInt32)0x3f << uds_status_offset.PCANTP_STATUS_OFFSET_UDS,
        /// <summary>
        /// UDS No message avaiable.
        /// </summary>
        PUDS_STATUS_SERVICE_NO_MESSAGE = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_SERVICE_NO_MESSAGE << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// Timeout while waiting message confirmation (loopback).
        /// </summary>
        PUDS_STATUS_SERVICE_TIMEOUT_CONFIRMATION = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_SERVICE_TIMEOUT_CONFIRMATION << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// Timeout while waiting request message response.
        /// </summary>
        PUDS_STATUS_SERVICE_TIMEOUT_RESPONSE = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_SERVICE_TIMEOUT_RESPONSE << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// UDS reset error.
        /// </summary>
        PUDS_STATUS_RESET = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_RESET << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// UDS wait for P3 timing error.
        /// </summary>
        PUDS_STATUS_ERROR_WAIT_FOR_P3_TIMING = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_ERROR_WAIT_FOR_P3_TIMING << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// A message with the same service identifier is already pending in the reception queue,
        /// user must read response for his previous request before or clear the reception queues with Reset_2013.
        /// </summary>
        PUDS_STATUS_SERVICE_ALREADY_PENDING = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_SERVICE_ALREADY_PENDING << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// An error occurred during the transmission of the UDS request message.
        /// </summary>
        PUDS_STATUS_SERVICE_TX_ERROR = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_SERVICE_TX_ERROR << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// An error occurred during the reception of the UDS response message.
        /// </summary>
        PUDS_STATUS_SERVICE_RX_ERROR = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_SERVICE_RX_ERROR << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// Service received more messages than input buffer expected.
        /// </summary>
        PUDS_STATUS_SERVICE_RX_OVERFLOW = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_SERVICE_RX_OVERFLOW << uds_status_offset.PCANTP_STATUS_OFFSET_UDS),
        /// <summary>
        /// Given message buffer was already used, user must release buffer with MsgFree_2013 before reusing it.
        /// </summary>
        PUDS_STATUS_MESSAGE_BUFFER_ALREADY_USED = PUDS_STATUS_FLAG_UDS_ERROR | (uds_errstatus.PUDS_ERRSTATUS_MESSAGE_BUFFER_ALREADY_USED << uds_status_offset.PCANTP_STATUS_OFFSET_UDS)
    }

    /// <summary>
    /// List of parameters handled by PCAN-UDS
    /// Note: PCAN-ISO-TP and PCAN-Basic parameters (PCANTP_PARAMETER_xxx, PCAN_PARAM_xxx) are compatible via casting.
    /// </summary>
    public enum uds_parameter : UInt32
    {
        /// <summary>
        /// (R/ ) uint8_t[]: PCAN-UDS API version parameter
        /// </summary>
        PUDS_PARAMETER_API_VERSION = 0x201,
        /// <summary>
        /// (R/W) uint8_t: data describing the debug mode  (use PUDS_DEBUG_LVL_ values)
        /// </summary>
        PUDS_PARAMETER_DEBUG = 0x203,
        /// <summary>
        /// (R/W) uintptr_t: data is pointer to a HANDLE created by CreateEvent function
        /// </summary>
        PUDS_PARAMETER_RECEIVE_EVENT = 0x204,
        /// <summary>
        /// (R/W) uint16_t: ISO-TP physical address
        /// </summary>
        PUDS_PARAMETER_SERVER_ADDRESS = 0x207,
        /// <summary>
        /// (R/W) uds_sessioninfo: ECU Session information
        /// </summary>
        PUDS_PARAMETER_SESSION_INFO = 0x209,
        /// <summary>
        /// (R/W) uint32_t: max time to wait to receive the request loopback
        /// </summary>
        PUDS_PARAMETER_TIMEOUT_REQUEST = 0x20A,
        /// <summary>
        /// (R/W) uint32_t: max time to wait to receive the message response indication
        /// </summary>
        PUDS_PARAMETER_TIMEOUT_RESPONSE = 0x20B,
        /// <summary>
        /// (R/W) bool: Automatic tester present. Default value: true
        /// </summary>
        PUDS_PARAMETER_AUTOMATIC_TESTER_PRESENT = 0x20C,
        /// <summary>
        /// (R/W) bool: Use no response flag for automatic tester present. Default value: true
        /// </summary>
        PUDS_PARAMETER_USE_NO_RESPONSE_AUTOMATIC_TESTER_PRESENT = 0x213,
        /// <summary>
        /// (R/W) bool: Wait for P3 timing. Default value: true (ISO-14229-2_2013 §8.3 Minimum time between client request messages, p.36)
        /// </summary>
        PUDS_PARAMETER_AUTO_P3_TIMING_MANAGEMENT = 0x20D,
        /// <summary>
        /// (R/ ) uint16_t[size]: List of pysical addresses to listen to.
        /// NOTE: for the parameter PUDS_PARAMETER_LISTENED_ADDRESSES the size of the array must
        /// be specified in the "buffer_size" parameter of the "UDS_GetValue_2013" function
        /// </summary>
        PUDS_PARAMETER_LISTENED_ADDRESSES = 0x210,
        /// <summary>
        /// ( /W) uint16_t: Add a listening address to the list of physical addresses to listen to
        /// </summary>
        PUDS_PARAMETER_ADD_LISTENED_ADDRESS = 0x211,
        /// <summary>
        /// ( /W) uint16_t: Remove a listening address from the list of physical addresses to listen to
        /// </summary>
        PUDS_PARAMETER_REMOVE_LISTENED_ADDRESS = 0x212,


        /// <summary>
        /// (R/ ) uint8_t: data describing the condition of a channel.
        /// </summary>
        PUDS_PARAMETER_CHANNEL_CONDITION = cantp_parameter.PCANTP_PARAMETER_CHANNEL_CONDITION,
        /// <summary>
        /// (R/W) uint8_t: data stating the default DLC to use when transmitting messages with CAN FD
        /// </summary>
        PUDS_PARAMETER_CAN_TX_DL = cantp_parameter.PCANTP_PARAMETER_CAN_TX_DL,
        /// <summary>
        /// (R/W) uint8_t: data stating if CAN frame DLC uses padding or not
        /// </summary>
        PUDS_PARAMETER_CAN_DATA_PADDING = cantp_parameter.PCANTP_PARAMETER_CAN_DATA_PADDING,
        /// <summary>
        /// (R/W) uint8_t: data stating the value used for CAN data padding
        /// </summary>
        PUDS_PARAMETER_CAN_PADDING_VALUE = cantp_parameter.PCANTP_PARAMETER_CAN_PADDING_VALUE,
        /// <summary>
        /// (R/W) uint8_t: data stating the default priority value for normal fixed, mixed and enhanced addressing (default=6)
        /// </summary>
        PUDS_PARAMETER_J1939_PRIORITY = cantp_parameter.PCANTP_PARAMETER_J1939_PRIORITY,
        /// <summary>
        /// (R/W) uint8_t: data describing the block size parameter (BS)
        /// </summary>
        PUDS_PARAMETER_BLOCK_SIZE = cantp_parameter.PCANTP_PARAMETER_BLOCK_SIZE,
        /// <summary>
        /// (R/W) uint8_t: data describing the seperation time parameter (STmin)
        /// </summary>
        PUDS_PARAMETER_SEPARATION_TIME = cantp_parameter.PCANTP_PARAMETER_SEPARATION_TIME,
        /// <summary>
        /// (R/W) uint8_t[4]: data describing the Wait Frame Transmissions parameter.
        /// </summary>
        PUDS_PARAMETER_WFT_MAX = cantp_parameter.PCANTP_PARAMETER_WFT_MAX,
        /// <summary>
        /// (R/W) uint8_t: data to set predefined ISO values for timeouts (see PCANTP_ISO_TIMEOUTS_*).
        /// </summary>
        PUDS_PARAMETER_ISO_TIMEOUTS = cantp_parameter.PCANTP_PARAMETER_ISO_TIMEOUTS,
        /// <summary>
        /// ( /W) uint8_t: data stating to clear Rx/Tx queues and CAN controller (channel is unitialized and re-initialized but settings and mappings are kept)
        /// </summary>
        PUDS_PARAMETER_RESET_HARD = cantp_parameter.PCANTP_PARAMETER_RESET_HARD,

        PUDS_PARAMETER_HARDWARE_NAME = TPCANParameter.PCAN_HARDWARE_NAME,
        PUDS_PARAMETER_DEVICE_NUMBER = TPCANParameter.PCAN_DEVICE_NUMBER,
        PUDS_PARAMETER_CONTROLLER_NUMBER = TPCANParameter.PCAN_CONTROLLER_NUMBER,
        PUDS_PARAMETER_CHANNEL_FEATURES = TPCANParameter.PCAN_CHANNEL_FEATURES
    }

    /// <summary>
    /// Represents type and flags for a usd_msg
    /// </summary>
    [Flags]
    public enum uds_msgtype : UInt32
    {
        /// <summary>
        /// Unacknowledge Segmented Data Transfert (ISO-TP message)
        /// </summary>
        PUDS_MSGTYPE_USDT = 0,
        /// <summary>
        /// Unacknowledge Unsegmented Data Transfert (msg_physical will use a single CAN/CAN-FD frame without ISO-TP protocol control information)
        /// </summary>
        PUDS_MSGTYPE_UUDT = 1,
        /// <summary>
        /// ECU(s) shall not reply to the request on positive response.
        /// </summary>
        PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE = 2,
        /// <summary>
        /// Message is a loopback
        /// </summary>
        PUDS_MSGTYPE_FLAG_LOOPBACK = 4,
        /// <summary>
        /// Mask to get the type (USDT or UUDT)
        /// </summary>
        PUDS_MSGTYPE_MASK_TYPE = 0x01,
    }

    /// <summary>
    /// Represents ISO-TP network addressing information supported in UDS
    /// </summary>
    public enum uds_msgprotocol : UInt32
    {
        /// <summary>
        /// Non ISO-TP frame (Unacknowledge Unsegmented Data Transfer)
        /// </summary>
        PUDS_MSGPROTOCOL_NONE = 0x00,
        /// <summary>
        /// ISO-TP 11 bits Extended addressing (mapping required)
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_2_11B_EXTENDED = 0x07,
        /// <summary>
        /// ISO-TP 11 bits Normal addressing (mapping required)
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_2_11B_NORMAL = 0x01,
        /// <summary>
        /// ISO-TP 11 bits Mixed addressing (mapping required)
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_2_11B_REMOTE = 0x02,
        /// <summary>
        /// ISO-TP 29 bits Extended addressing (mapping required)
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_2_29B_EXTENDED = 0x08,
        /// <summary>
        /// ISO-TP 29 bits Fixed Normal addressing
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_2_29B_FIXED_NORMAL = 0x03,
        /// <summary>
        /// ISO-TP 29 bits Normal addressing (mapping required)
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_2_29B_NORMAL = 0x06,
        /// <summary>
        /// ISO-TP 29 bits Mixed addressing
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_2_29B_REMOTE = 0x04,
        /// <summary>
        /// ISO-TP Enhanced addressing
        /// </summary>
        PUDS_MSGPROTOCOL_ISO_15765_3_29B_ENHANCED = 0x05
    }

    /// <summary>
    /// Represents UDS negative response codes (see ISO 14229-1:2013 §A.1 Negative response codes p.325)
    /// </summary>
    public enum uds_nrc : Byte
    {
        /// <summary>
        /// Positive Response
        /// </summary>
        PUDS_NRC_PR = 0x00,
        /// <summary>
        /// General Reject
        /// </summary>
        PUDS_NRC_GR = 0x10,
        /// <summary>
        /// Service Not Supported
        /// </summary>
        PUDS_NRC_SNS = 0x11,
        /// <summary>
        /// Sub Function Not Supported
        /// </summary>
        PUDS_NRC_SFNS = 0x12,
        /// <summary>
        /// Incorrect Message Length Or Invalid Format
        /// </summary>
        PUDS_NRC_IMLOIF = 0x13,
        /// <summary>
        /// Response Too Long
        /// </summary>
        PUDS_NRC_RTL = 0x14,
        /// <summary>
        /// Busy Repeat Request
        /// </summary>
        PUDS_NRC_BRR = 0x21,
        /// <summary>
        /// Conditions Not Correct
        /// </summary>
        PUDS_NRC_CNC = 0x22,
        /// <summary>
        /// Request Sequence Error
        /// </summary>
        PUDS_NRC_RSE = 0x24,
        /// <summary>
        /// No Response From Subnet Component
        /// </summary>
        PUDS_NRC_NRFSC = 0x25,
        /// <summary>
        /// Failure Prevents Execution Of Requested Action
        /// </summary>
        PUDS_NRC_FPEORA = 0x26,
        /// <summary>
        /// Request Out Of Range
        /// </summary>
        PUDS_NRC_ROOR = 0x31,
        /// <summary>
        /// Security Access Denied
        /// </summary>
        PUDS_NRC_SAD = 0x33,
        /// <summary>
        /// Authentication Required
        /// </summary>
        PUDS_NRC_AR = 0x34,
        /// <summary>
        /// Invalid Key
        /// </summary>
        PUDS_NRC_IK = 0x35,
        /// <summary>
        /// Exceeded Number Of Attempts
        /// </summary>
        PUDS_NRC_ENOA = 0x36,
        /// <summary>
        /// Required Time Delay Not Expired
        /// </summary>
        PUDS_NRC_RTDNE = 0x37,
        /// <summary>
        /// Secure Data Transmission Required
        /// </summary>
        PUDS_NRC_SDTR = 0x38,
        /// <summary>
        /// Secure Data Transmission Not Allowed
        /// </summary>
        PUDS_NRC_SDTNA = 0x39,
        /// <summary>
        /// Secure Data Verification Failed
        /// </summary>
        PUDS_NRC_SDTF = 0x3A,
        /// <summary>
        /// Certificate Verification Failed Invalid Time Period
        /// </summary>
        PUDS_NRC_CVFITP = 0x50,
        /// <summary>
        /// Certificate Verification Failed Invalid SIGnature
        /// </summary>
        PUDS_NRC_CVFISIG = 0x51,
        /// <summary>
        /// Certificate Verification Failed Invalid Chain of Trust
        /// </summary>
        PUDS_NRC_CVFICOT = 0x52,
        /// <summary>
        /// Certificate Verification Failed Invalid Type
        /// </summary>
        PUDS_NRC_CVFIT = 0x53,
        /// <summary>
        /// Certificate Verification Failed Invalid Format
        /// </summary>
        PUDS_NRC_CVFIF = 0x54,
        /// <summary>
        /// Certificate Verification Failed Invalid Content
        /// </summary>
        PUDS_NRC_CVFIC = 0x55,
        /// <summary>
        /// Certificate Verification Failed Invalid SCoPe
        /// </summary>
        PUDS_NRC_CVFISCP = 0x56,
        /// <summary>
        /// Certificate Verification Failed Invalid CERTificate(revoked)
        /// </summary>
        PUDS_NRC_CVFICERT = 0x57,
        /// <summary>
        /// Ownership Verification Failed
        /// </summary>
        PUDS_NRC_OVF = 0x58,
        /// <summary>
        /// Challenge Calculation Failed
        /// </summary>
        PUDS_NRC_CCF = 0x59,
        /// <summary>
        /// Setting Access Rights Failed
        /// </summary>
        PUDS_NRC_SARF = 0x5A,
        /// <summary>
        /// Session Key Creation / Derivation Failed
        /// </summary>
        PUDS_NRC_SKCDF = 0x5B,
        /// <summary>
        /// Configuration Data Usage Failed
        /// </summary>
        PUDS_NRC_CDUF = 0x5C,
        /// <summary>
        /// DeAuthentication Failed
        /// </summary>
        PUDS_NRC_DAF = 0x5D,
        /// <summary>
        /// Upload Download Not Accepted
        /// </summary>
        PUDS_NRC_UDNA = 0x70,
        /// <summary>
        /// Transfer Data Suspended
        /// </summary>
        PUDS_NRC_TDS = 0x71,
        /// <summary>
        /// General Programming Failure
        /// </summary>
        PUDS_NRC_GPF = 0x72,
        /// <summary>
        /// Wrong Block Sequence Counter
        /// </summary>
        PUDS_NRC_WBSC = 0x73,
        /// <summary>
        /// Request Correctly Received - Response Pending
        /// </summary>
        PUDS_NRC_RCRRP = 0x78,
        /// <summary>
        /// Sub Function Not Supported In Active Session
        /// </summary>
        PUDS_NRC_SFNSIAS = 0x7E,
        /// <summary>
        /// Service Not Supported In Active Session
        /// </summary>
        PUDS_NRC_SNSIAS = 0x7F,
        /// <summary>
        /// RPM Too High
        /// </summary>
        PUDS_NRC_RPMTH = 0x81,
        /// <summary>
        /// RPM Too Low
        /// </summary>
        PUDS_NRC_RPMTL = 0x82,
        /// <summary>
        /// Engine Is Running
        /// </summary>
        PUDS_NRC_EIR = 0x83,
        /// <summary>
        /// Engine Is Not Running
        /// </summary>
        PUDS_NRC_EINR = 0x84,
        /// <summary>
        /// Engine Run Time Too Low
        /// </summary>
        PUDS_NRC_ERTTL = 0x85,
        /// <summary>
        /// TEMPerature Too High
        /// </summary>
        PUDS_NRC_TEMPTH = 0x86,
        /// <summary>
        /// TEMPerature Too Low
        /// </summary>
        PUDS_NRC_TEMPTL = 0x87,
        /// <summary>
        /// Vehicle Speed Too High
        /// </summary>
        PUDS_NRC_VSTH = 0x88,
        /// <summary>
        /// Vehicle Speed Too Low
        /// </summary>
        PUDS_NRC_VSTL = 0x89,
        /// <summary>
        /// Throttle / Pedal Too High
        /// </summary>
        PUDS_NRC_TPTH = 0x8A,
        /// <summary>
        /// Throttle / Pedal Too Low
        /// </summary>
        PUDS_NRC_TPTL = 0x8B,
        /// <summary>
        /// Transmission Range Not In Neutral
        /// </summary>
        PUDS_NRC_TRNIN = 0x8C,
        /// <summary>
        /// Transmission Range Not In Gear
        /// </summary>
        PUDS_NRC_TRNIG = 0x8D,
        /// <summary>
        /// Brake Switch(es) Not Closed(brake pedal not pressed or not applied)
        /// </summary>
        PUDS_NRC_BSNC = 0x8F,
        /// <summary>
        /// Shifter Lever Not In Park
        /// </summary>
        PUDS_NRC_SLNIP = 0x90,
        /// <summary>
        /// Torque Converter Clutch Locked
        /// </summary>
        PUDS_NRC_TCCL = 0x91,
        /// <summary>
        /// Voltage Too High
        /// </summary>
        PUDS_NRC_VTH = 0x92,
        /// <summary>
        /// Voltage Too Low
        /// </summary>
        PUDS_NRC_VTL = 0x93,
        /// <summary>
        /// Resource Temporarily Not Available
        /// </summary>
        PUDS_NRC_RTNA = 0x94
    }

    /// <summary>
    /// PUDS ISO_15765_4 11 bit CAN ID definitions
    /// </summary>
    public enum uds_can_id : UInt32
    {
        /// <summary>
        /// CAN ID for functionally addressed request messages sent by external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_FUNCTIONAL_REQUEST = 0x7DF,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #1
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_1 = 0x7E0,
        /// <summary>
        /// physical response CAN ID from ECU #1 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_1 = 0x7E8,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #2
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_2 = 0x7E1,
        /// <summary>
        /// physical response CAN ID from ECU #2 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_2 = 0x7E9,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #3
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_3 = 0x7E2,
        /// <summary>
        /// physical response CAN ID from ECU #3 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_3 = 0x7EA,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #4
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_4 = 0x7E3,
        /// <summary>
        /// physical response CAN ID from ECU #4 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_4 = 0x7EB,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #5
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_5 = 0x7E4,
        /// <summary>
        /// physical response CAN ID from ECU #5 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_5 = 0x7EC,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #6
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_6 = 0x7E5,
        /// <summary>
        /// physical response CAN ID from ECU #6 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_6 = 0x7ED,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #7
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_7 = 0x7E6,
        /// <summary>
        /// physical response CAN ID from ECU #7 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_7 = 0x7EE,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #8
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_REQUEST_8 = 0x7E7,
        /// <summary>
        /// physical response CAN ID from ECU #8 to external test equipment
        /// </summary>
        PUDS_CAN_ID_ISO_15765_4_PHYSICAL_RESPONSE_8 = 0x7EF
    }

    /// <summary>
    /// PUDS ISO_15765_4 address definitions
    /// </summary>
    public enum uds_address : UInt16
    {
        /// <summary>
        /// external test equipment
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_TEST_EQUIPMENT = 0xF1,
        /// <summary>
        /// OBD funtional system
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_OBD_FUNCTIONAL = 0x33,
        /// <summary>
        /// ECU 1
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_1 = 0x01,
        /// <summary>
        /// ECU 2
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_2 = 0x02,
        /// <summary>
        /// ECU 3
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_3 = 0x03,
        /// <summary>
        /// ECU 4
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_4 = 0x04,
        /// <summary>
        /// ECU 5
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_5 = 0x05,
        /// <summary>
        /// ECU 6
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_6 = 0x06,
        /// <summary>
        /// ECU 7
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_7 = 0x07,
        /// <summary>
        /// ECU 8
        /// </summary>
        PUDS_ADDRESS_ISO_15765_4_ADDR_ECU_8 = 0x08
    }
    #endregion

    ////////////////////////////////////////////////////////////
    // PUDS parameter values
    ////////////////////////////////////////////////////////////

    #region Parameter values
    public static partial class UDSApi
    {
#if (!PUDS_API_COMPATIBILITY_ISO_2006)
        /// <summary>
        /// Default maximum timeout for UDS transmit confirmation
        /// </summary>
        public const UInt32 PUDS_TIMEOUT_REQUEST = 10000;
        /// <summary>
        /// Default maximum timeout for UDS response reception
        /// </summary>
        public const UInt32 PUDS_TIMEOUT_RESPONSE = 10000;
        /// <summary>
        /// Flag stating that the address is defined as a ISO-15765-3 address
        /// </summary>
        public const UInt16 PUDS_SERVER_ADDR_FLAG_ENHANCED_ISO_15765_3 = 0x1000;
        /// <summary>
        /// Mask used for the ISO-15765-3 enhanced addresses
        /// </summary>
        public const UInt16 PUDS_SERVER_ADDR_MASK_ENHANCED_ISO_15765_3 = 0x07FF;
        /// <summary>
        /// The Channel is illegal or not available
        /// </summary>
        public const Byte PUDS_CHANNEL_UNAVAILABLE = 0x00;
        /// <summary>
        /// The Channel is available
        /// </summary>
        public const Byte PUDS_CHANNEL_AVAILABLE = 0x01;
        /// <summary>
        /// The Channel is valid, and is being used
        /// </summary>
        public const Byte PUDS_CHANNEL_OCCUPIED = 0x02;
        /// <summary>
        /// Uses CAN frame data optimization
        /// </summary>
        public const Byte PUDS_CAN_DATA_PADDING_NONE = 0x00;
        /// <summary>
        /// Uses CAN frame data padding (default, i.e. CAN DLC = 8)
        /// </summary>
        public const Byte PUDS_CAN_DATA_PADDING_ON = 0x01;
        /// <summary>
        /// Default value used if CAN data padding is enabled
        /// </summary>
        public const Byte PUDS_CAN_DATA_PADDING_VALUE = 0x55;
#endif

        /// <summary>
        /// Default server performance requirement in ms (See ISO_14229-2_2013 §7.2 table 4)
        /// </summary>
        public const UInt16 PUDS_P2CAN_SERVER_MAX_DEFAULT = 50;
        /// <summary>
        /// Enhanced server performance requirement in ms (See ISO_14229-2_2013 §7.2 table 4)
        /// </summary>
        public const UInt16 PUDS_P2CAN_ENHANCED_SERVER_MAX_DEFAULT = 5000;
        /// <summary>
        /// Recommended S3 client timeout in ms (See ISO_14229-2_2013 §7.5 table 5)
        /// </summary>
        public const UInt16 PUDS_S3_CLIENT_TIMEOUT_RECOMMENDED = 2000;
        /// <summary>
        /// Default P3 timing parameter in ms (See ISO_14229-2_2013 §7.2 table 4)
        /// </summary>
        public const UInt16 PUDS_P3CAN_DEFAULT = PUDS_P2CAN_SERVER_MAX_DEFAULT;
        /// <summary>
        /// Disable debug messages (default)
        /// </summary>
        public const Byte PUDS_DEBUG_LVL_NONE = 0x00;
        /// <summary>
        /// Enable debug messages (only errors)
        /// </summary>
        public const Byte PUDS_DEBUG_LVL_ERROR = 0xF1;
        /// <summary>
        /// Enable debug messages (only warnings, errors)
        /// </summary>
        public const Byte PUDS_DEBUG_LVL_WARNING = 0xF2;
        /// <summary>
        /// Enable debug messages (only informations, warnings, errors)
        /// </summary>
        public const Byte PUDS_DEBUG_LVL_INFORMATION = 0xF3;
        /// <summary>
        /// Enable debug messages (only notices, informations, warnings, errors)
        /// </summary>
        public const Byte PUDS_DEBUG_LVL_NOTICE = 0xF4;
        /// <summary>
        /// Enable debug messages (only debug, notices, informations, warnings, errors)
        /// </summary>
        public const Byte PUDS_DEBUG_LVL_DEBUG = 0xF5;
        /// <summary>
        /// Enable all debug messages
        /// </summary>
        public const Byte PUDS_DEBUG_LVL_TRACE = 0xF6;

        /// <summary>
        /// Option that can be used as channel identifier in Svc* functions: only prepare uds_msg structure and do not send it
        /// </summary>
        public const cantp_handle PUDS_ONLY_PREPARE_REQUEST = cantp_handle.PCANTP_HANDLE_NONEBUS;
    }
    #endregion

    #region Structures

    ////////////////////////////////////////////////////////////
    // Message definitions
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// Represents a UDS Network Addressing Information
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct uds_netaddrinfo
    {/// <summary>
     /// communication protocol
     /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public uds_msgprotocol protocol;
        /// <summary>
        /// ISO-TP target type
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
        public Byte extension_addr;
    }


    /// <summary>
    /// Represents the diagnostic session's information of a server
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct uds_sessioninfo
    {
        /// <summary>
        /// Network address information
        /// </summary>
        public uds_netaddrinfo nai;
        /// <summary>
        /// Types and flags of the CAN/CAN-FD frames
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_can_msgtype can_msg_type;
        /// <summary>
        /// Activated Diagnostic Session (see PUDS_SVC_PARAM_DSC_xxx values)
        /// </summary>
        public Byte session_type;
        /// <summary>
        /// Default P2Can_Server_Max timing for the activated session (resolution: 1ms)
        /// </summary>
        public UInt16 timeout_p2can_server_max;
        /// <summary>
        /// Enhanced P2Can_Server_Max timing for the activated session (resolution: 10ms)
        /// </summary>
        public UInt16 timeout_enhanced_p2can_server_max;
        /// <summary>
        /// Time between 2 TesterPresents
        /// </summary>
        public UInt16 s3_client_ms;
    }

    /// <summary>
    /// Represents the configuration of a PUDS message
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct uds_msgconfig
    {
        /// <summary>
        /// structure specific flags
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public uds_msgtype type;
        /// <summary>
        /// Network Addressing Information
        /// </summary>
        public uds_netaddrinfo nai;
        /// <summary>
        /// (optional) CAN ID (for configuration use either nai or m_can_id)
        /// </summary>
        public UInt32 can_id;
        /// <summary>
        /// optional flags for the CAN layer (29 bits CAN-ID, FD, BRS)
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public cantp_can_msgtype can_msgtype;
        /// <summary>
        /// Default CAN DLC value to use with segmented messages
        /// </summary>
        public Byte can_tx_dlc;
    }

    /// <summary>
    /// Represents a mapping between an UDS Network Addressing Information and a CAN ID.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct uds_mapping
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
        public Byte can_tx_dlc;
        /// <summary>
        /// Network Addressing Information
        /// </summary>
        public uds_netaddrinfo nai;
    }

    public static partial class UDSApi
    {
        /// <summary>
        /// Mapping does not require a Flow Control frame.
        /// </summary>
        public const UInt32 PUDS_MAPPING_FLOW_CTRL_NONE = 0xFFFFFFFF;
    }

    /// <summary>
    /// Provides accessors to the corresponding data in the cantp_msg
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct uds_msgaccess
    {
        /// <summary>
        /// Pointer to the Service ID in message's data.
        /// Use it in safe mode with Marshal.ReadByte, Marshal.WriteByte or unsafe mode with IntPtr.toPointer.
        /// See special C# functions
        /// </summary>
        public IntPtr service_id;
        /// <summary>
        /// Pointer to the first parameter in message's data.
        /// Use it in safe mode with Marshal.ReadByte, Marshal.WriteByte or unsafe mode with IntPtr.toPointer.
        /// See special C# functions
        /// </summary>
        public IntPtr param;
        /// <summary>
        /// Pointer to the Negative Response Code (see uds_nrc enumeration) in message's data (NULL on positive response).
        /// Use it in safe mode with Marshal.ReadByte, Marshal.WriteByte or unsafe mode with IntPtr.toPointer.
        /// See special C# functions
        /// </summary>
        public IntPtr nrc;
    }

    /// <summary>
    /// Represents the content of a UDS message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct uds_msg
    {
        /// <summary>
        /// structure specific flags
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public uds_msgtype type;
        /// <summary>
        /// quick accessors to the cantp_msg data
        /// </summary>
        public uds_msgaccess links;
        /// <summary>
        /// the PCANTP message encapsulating the UDS data
        /// </summary>
        public cantp_msg msg;
    }
    #endregion

    #region PCAN UDS Api
    public static partial class UDSApi
    {

        ////////////////////////////////////////////////////////////
        // PCAN-UDS API: Core function declarations
        ////////////////////////////////////////////////////////////

        #region PCAN UDS API Implementation

        /// <summary>
        /// Initializes a PUDS channel based on a PCANTP channel handle (without CAN-FD support)
        /// </summary>
        /// <remarks>Only one PUDS channel can be initialized per CAN-Channel</remarks>
        /// <param name="channel">A PCANTP channel handle</param>
        /// <param name="baudrate">The CAN Hardware speed</param>
        /// <param name="hw_type">NON PLUG-N-PLAY: The type of hardware and operation mode</param>
        /// <param name="io_port">NON PLUG-N-PLAY: The I/O address for the parallel port</param>
        /// <param name="interrupt">NON PLUG-N-PLAY: Interrupt number of the parallel port</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Initialize_2013")]
        public static extern uds_status Initialize_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            cantp_baudrate baudrate,
            [MarshalAs(UnmanagedType.U4)]
            cantp_hwtype hw_type,
            UInt32 io_port,
            UInt16 interrupt);

        /// <summary>
        /// Initializes a PUDS channel based on a PCANTP channel handle (without CAN-FD support)
        /// </summary>
        /// <remarks>Only one PUDS channel can be initialized per CAN-Channel</remarks>
        /// <param name="channel">A PCANTP channel handle</param>
        /// <param name="baudrate">The CAN Hardware speed</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status Initialize_2013(
            cantp_handle channel,
            cantp_baudrate baudrate)
        {
            return Initialize_2013(channel, baudrate, (cantp_hwtype)0, 0, 0);
        }

        /// <summary>
        /// Initializes a PUDS channel based on a PCANTP channel handle (including CAN-FD support)
        /// </summary>
        /// <param name="channel">The handle of a FD capable PCANTP Channel</param>
        /// <param name="bitrate_fd">The speed for the communication (FD bit rate string)</param>
        /// <remarks>Only one PUDS channel can be initialized per CAN-Channel.
        /// See PCAN_BR_* values
        /// * Parameter and values must be separated by '='
        /// * Couples of parameter/value must be separated by ','
        /// * Following parameter must be filled out: f_clock, data_brp, data_sjw, data_tseg1, data_tseg2,
        ///   nom_brp, nom_sjw, nom_tseg1, nom_tseg2.
        /// * Following parameters are optional (not used yet): data_ssp_offset, nom_samp
        /// </remarks>
        /// <example>f_clock_mhz=80,nom_brp=0,nom_tseg1=13,nom_tseg2=0,nom_sjw=0,data_brp=0,
        /// data_tseg1=13,data_tseg2=0,data_sjw=0</example>
        /// <returns>A uds_status error code</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_InitializeFD_2013")]
        public static extern uds_status InitializeFD_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            cantp_bitrate bitrate_fd);

        /// <summary>
        /// Uninitializes a PUDS channel
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Uninitialize_2013")]
        public static extern uds_status Uninitialize_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel);

        /// <summary>
        /// Resets the receive and transmit queues of a PUDS channel
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Reset_2013")]
        public static extern uds_status Reset_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel);

        /// <summary>
        /// Gets information about the internal BUS status of a PUDS channel
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetCanBusStatus_2013")]
        public static extern uds_status GetCanBusStatus_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel);

        /// <summary>
        /// Reads a PUDS message from the receive queue of a PUDS channel
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="out_msg_buffer">[out]A uds_msg structure buffer to store the PUDS message</param>
        /// <param name="in_msg_request">(Optional) If NULL the first available message is fetched.
        ///     Otherwise in_msg_request must represent a sent PUDS request.
        ///     To look for the request confirmation, in_msg_request->type should not have the loopback flag;
        ///     otherwise a response from the target ECU will be searched.</param>
        /// <param name="out_timestamp">A cantp_timestamp structure buffer to get
        /// the reception time of the message. If this value is not desired, this parameter
        /// should be passed as NULL</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Read_2013")]
        public static extern uds_status Read_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out uds_msg out_msg_buffer,
            [In] ref uds_msg in_msg_request,
            out cantp_timestamp out_timestamp);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Read_2013")]
        private static extern uds_status Read_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out uds_msg out_msg_buffer,
            [In] ref uds_msg in_msg_request,
            IntPtr out_timestamp);

        /// <summary>
        /// Reads a PUDS message from the receive queue of a PUDS channel
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="out_msg_buffer">[out]A uds_msg structure buffer to store the PUDS message</param>
        /// <param name="in_msg_request">(Optional) If NULL the first available message is fetched.
        ///     Otherwise in_msg_request must represent a sent PUDS request.
        ///     To look for the request confirmation, in_msg_request->type should not have the loopback flag;
        ///     otherwise a response from the target ECU will be searched.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status Read_2013(
            cantp_handle channel,
            out uds_msg out_msg_buffer,
            ref uds_msg in_msg_request)
        {
            return Read_2013(channel, out out_msg_buffer, ref in_msg_request, IntPtr.Zero);
        }

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Read_2013")]
        private static extern uds_status Read_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out uds_msg out_msg_buffer,
            IntPtr in_msg_request,
            IntPtr out_timestamp);

        /// <summary>
        /// Reads a PUDS message from the receive queue of a PUDS channel
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="out_msg_buffer">[out]A uds_msg structure buffer to store the PUDS message</param>
        /// <param name="in_msg_request">(Optional) If NULL the first available message is fetched.
        ///     Otherwise in_msg_request must represent a sent PUDS request.
        ///     To look for the request confirmation, in_msg_request->type should not have the loopback flag;
        ///     otherwise a response from the target ECU will be searched.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status Read_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out uds_msg out_msg_buffer)
        {
            return Read_2013(channel, out out_msg_buffer, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Transmits a PUDS message
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="msg_buffer">A uds_msg buffer with the message to be sent</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Write_2013")]
        public static extern uds_status Write_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            ref uds_msg msg_buffer);

        /// <summary>
        /// Adds a user-defined UDS mapping (relation between a CAN ID and a UDS Network Address Information)
        /// </summary>
        /// <remark>
        /// Defining a mapping enables ISO-TP communication with opened Addressing Formats
        /// (like PCANTP_ISOTP_FORMAT_NORMAL or PCANTP_ISOTP_FORMAT_EXTENDED).
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="mapping">Mapping to be added</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_AddMapping_2013")]
        public static extern uds_status AddMapping_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            ref uds_mapping mapping);


        /// <summary>
        /// Removes all user-defined PUDS mappings corresponding to a CAN ID
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="can_id">The mapped CAN ID to search for</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_RemoveMappingByCanId_2013")]
        public static extern uds_status RemoveMappingByCanId_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            UInt32 can_id);

        /// <summary>
        /// Removes a user-defined PUDS mapping
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="mapping">The mapping to remove</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_RemoveMapping_2013")]
        public static extern uds_status RemoveMapping_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_mapping mapping);

        /// <summary>
        /// Retrieves a mapping matching the given CAN identifier and message type (11bits, 29 bits, FD, etc.)
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="buffer">[out]Buffer to store the searched mapping</param>
        /// <param name="can_id">The mapped CAN ID to look for</param>
        /// <param name="can_msgtype">The CAN message type to look for (11bits, 29 bits, FD, etc.)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success,
        /// PUDS_STATUS_MAPPING_NOT_INITIALIZED if no mapping was found.</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetMapping_2013")]
        public static extern uds_status GetMapping_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out uds_mapping buffer,
            UInt32 can_id,
            cantp_can_msgtype can_msgtype);

        /// <summary>
        /// Retrieves all the mappings defined for a PUDS channel
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="buffer">[out]Buffer of mappings</param>
        /// <param name="buffer_length">The number of uds_mapping elements the buffer can store.</param>
        /// <param name="count">[out]The actual number of elements copied in the buffer.</param>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetMappings_2013")]
        public static extern uds_status GetMappings_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            [Out] uds_mapping[] buffer,
            UInt16 buffer_length,
            ref UInt16 count);

        /// <summary>
        /// Adds a "PASS" filter on a CAN ID
        /// </summary>
        /// <remark>
        /// CAN and CAN FD frames matching this CAN ID will be fetchable by the UDS API with UDS_Read_2013 function.
        /// By default all frames are ignored and are available in lower APIs.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="can_id">CAN identifier to listen to</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_AddCanIdFilter_2013")]
        public static extern uds_status AddCanIdFilter_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            UInt32 can_id);

        /// <summary>
        /// Remove a "PASS" filter on a CAN ID
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="can_id">CAN identifier to remove</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_RemoveCanIdFilter_2013")]
        public static extern uds_status RemoveCanIdFilter_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            UInt32 can_id);

        /// <summary>
        /// Retrieves a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status (see PUDS_SERVER_FILTER_LISTEN).
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to get</param>
        /// <param name="buffer">Buffer for the parameter value</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue_2013")]
        public static extern uds_status GetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            IntPtr buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Retrieves a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status (see PUDS_SERVER_FILTER_LISTEN).
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to get</param>
        /// <param name="buffer">Buffer for the parameter value</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue_2013")]
        public static extern uds_status GetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            StringBuilder buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Retrieves a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status (see PUDS_SERVER_FILTER_LISTEN).
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to get</param>
        /// <param name="buffer">Buffer for the parameter value</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue_2013")]
        public static extern uds_status GetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            out UInt32 buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Retrieves a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status (see PUDS_SERVER_FILTER_LISTEN).
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to get</param>
        /// <param name="buffer">Buffer for the parameter value</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue_2013")]
        public static extern uds_status GetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            [MarshalAs(UnmanagedType.LPArray)]
            [Out] Byte[] buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Configures or sets a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to set</param>
        /// <param name="buffer">Buffer with the value to be set</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue_2013")]
        public static extern uds_status SetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            IntPtr buffer,
            UInt32 buffer_size);
        /// <summary>
        /// Configures or sets a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to set</param>
        /// <param name="buffer">Buffer with the value to be set</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue_2013")]
        public static extern uds_status SetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            ref UInt32 buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Configures or sets a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to set</param>
        /// <param name="buffer">Buffer with the value to be set</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue_2013")]
        public static extern uds_status SetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            [MarshalAs(UnmanagedType.LPStr, SizeParamIndex =3)]
            String buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Configures or sets a PUDS channel value
        /// </summary>
        /// <remark>
        ///  * Parameter PUDS_PARAM_SERVER_ADDRESS uses 2 bytes data to describe
        /// the physical address of the equipment, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long.
        ///  * Parameter PUDS_PARAM_SERVER_FILTER uses 2 bytes data to describe
        /// a functional address, but the first byte is needed only
        /// for ISO-15765-3 Enhanced diagnostics 29 bit CAN ID where addresses
        /// are 11 bits long; the Most Significant Bit is used to define filter
        /// status.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="parameter">The parameter to set</param>
        /// <param name="buffer">Buffer with the value to be set</param>
        /// <param name="buffer_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue_2013")]
        public static extern uds_status SetValue_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [MarshalAs(UnmanagedType.U4)]
            uds_parameter parameter,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex =3)]
            Byte[] buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Returns a descriptive text of a given cantp_status error
        /// code, in any desired language
        /// </summary>
        /// <remarks>The current languages available for translation are:
        /// Neutral (0x00), German (0x07), English (0x09), Spanish (0x0A),
        /// Italian (0x10) and French (0x0C)</remarks>
        /// <param name="error_code">A uds_status error code</param>
        /// <param name="language">Indicates a 'Primary language ID'</param>
        /// <param name="buffer">Buffer for a null terminated char array</param>
        /// <param name="buffer_size">Buffer size</param>
        /// <returns>A uds_status error code</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetErrorText_2013")]
        public static extern uds_status GetErrorText_2013(
            [MarshalAs(UnmanagedType.U4)]
            uds_status error_code,
            UInt16 language,
            StringBuilder buffer,
            UInt32 buffer_size);

        /// <summary>
        /// Gets the session information known by the API
        /// </summary>
        /// <remark>
        /// session_info must be initialized a network address information associated to an ECU.
        /// Note that the session's information within the API may be different to the actual session of the corresponding ECU.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="session_info">[in|out] The session is filled if an ECU session, matching session_info->nai, exists</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetSessionInformation_2013")]
        public static extern uds_status GetSessionInformation_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            out uds_sessioninfo session_info);

        /// <summary>
        /// Checks if a status matches an expected result (default is PUDS_STATUS_OK).
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <param name="status_expected">The expected status (default is PUDS_STATUS_OK).</param>
        /// <param name="strict_mode">Enable strict mode (default is false). Strict mode ensures that bus or extra information are the same.</param>
        /// <returns>Returns true if the status matches expected parameter.</returns>
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_StatusIsOk_2013")]
        public static extern bool StatusIsOk_2013(
            [MarshalAs(UnmanagedType.U4)]
            uds_status status,
            [MarshalAs(UnmanagedType.U4)]
            uds_status status_expected,
            [MarshalAs(UnmanagedType.I1)]
            bool strict_mode);

        /// <summary>
        /// Checks if a status matches an expected result in a non-strict mode.
        /// Strict mode ensures that bus or extra information are the same.
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <param name="status_expected">The expected status (default is PUDS_STATUS_OK).</param>
        /// <returns>Returns true if the status matches expected parameter.</returns>
        public static bool StatusIsOk_2013(
            uds_status status,
            uds_status status_expected)
        {
            return StatusIsOk_2013(status, status_expected, false);
        }

        /// <summary>
        /// Checks if a status matches PUDS_STATUS_OK in a non-strict mode.
        /// Strict mode ensures that bus or extra information are the same.
        /// </summary>
        /// <param name="status">The status to analyze.</param>
        /// <returns>Returns true if the status matches PUDS_STATUS_OK.</returns>
        public static bool StatusIsOk_2013(
            uds_status status)
        {
            return StatusIsOk_2013(status, uds_status.PUDS_STATUS_OK, false);
        }


        ////////////////////////////////////////////////////////////
        // PCAN-UDS API: PUDS Message initialization function declarations
        ////////////////////////////////////////////////////////////

        /// <summary>
        /// Allocates a PUDS message based on the given configuration
        /// </summary>
        /// <param name="msg_buffer">A uds_msg structure buffer (it will be freed if required)</param>
        /// <param name="msg_configuration">Configuration of the PUDS message to allocate</param>
        /// <param name="msg_data_length">Length of the message's data</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_MsgAlloc_2013")]
        public static extern uds_status MsgAlloc_2013(
            out uds_msg msg_buffer,
            uds_msgconfig msg_configuration,
            UInt32 msg_data_length
        );

        /// <summary>
        /// Deallocates a PUDS message
        /// </summary>
        /// <param name="msg_buffer">An allocated uds_msg structure buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_MsgFree_2013")]
        public static extern uds_status MsgFree_2013(
            ref uds_msg msg_buffer
        );

        /// <summary>
        /// Copies a PUDS message to another buffer.
        /// </summary>
        /// <param name="msg_buffer_dst">A uds_msg structure buffer to store the copied message.</param>
        /// <param name="msg_buffer_src">The uds_msg structure buffer to copy.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_MsgCopy_2013")]
        public static extern uds_status MsgCopy_2013(
            out uds_msg msg_buffer_dst,
            [In]ref uds_msg msg_buffer_src
        );

        /// <summary>
        /// Moves a PUDS message to another buffer (and cleans the original message structure).
        /// </summary>
        /// <param name="msg_buffer_dst">A uds_msg structure buffer to store the message.</param>
        /// <param name="msg_buffer_src">The uds_msg structure buffer used as the source (will be cleaned).</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_MsgMove_2013")]
        public static extern uds_status MsgMove_2013(
            out uds_msg msg_buffer_dst,
            ref uds_msg msg_buffer_src
        );

        #region PCAN UDS API Implementation: Service handlers

        ////////////////////////////////////////////////////////////
        // PCAN-UDS API: Utility function declarations
        ////////////////////////////////////////////////////////////

        /// <summary>
        /// Waits for a message (a response or a transmit confirmation) based on a UDS request
        /// </summary>
        /// <remarks>
        /// Warning: The order of the parameters has changed in PCAN-UDS 2.0 API.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="msg_request">A sent uds_msg message used as a reference to find the waited message</param>
        /// <param name="is_waiting_for_tx">States if the message to wait for is a transmit confirmation</param>
        /// <param name="timeout">Maximum time to wait (in milliseconds) for a message indication corresponding to the message request</param>
        /// <param name="timeout_enhanced">Maximum time to wait for a message indication if the server requests more time</param>
        /// <param name="out_msg_response">A uds_msg structure buffer to store the PUDS response</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForSingleMessage_2013")]
        public static extern uds_status WaitForSingleMessage_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [In] ref uds_msg msg_request,
            [MarshalAs(UnmanagedType.U1)]
            bool is_waiting_for_tx,
            UInt32 timeout,
            UInt32 timeout_enhanced,
            out uds_msg out_msg_response);

        /// <summary>
        /// Waits for multiple responses (from a functional request for instance) based on a PUDS message request.
        /// </summary>
        /// <remarks>
        /// Warning: The order of the parameters has changed in PCAN-UDS 2.0 API.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="msg_request">A sent uds_msg message used as a reference to find the waited message</param>
        /// <param name="timeout">Maximum time to wait (in milliseconds) for a message indication corresponding to the message request.</param>
        /// <param name="timeout_enhanced">Maximum time to wait for a message indication if the server requested more time</param>
        /// <param name="wait_until_timeout">if <code>FALSE</code> the function is interrupted if out_msg_count reaches max_msg_count.</param>
        /// <param name="max_msg_count">Length of the buffer array (max. messages that can be received)</param>
        /// <param name="out_msg_responses">Buffer must be an array of 'max_msg_count' entries (must have at least
        /// a size of max_msg_count * sizeof(uds_msg) bytes</param>
        /// <param name="out_msg_count">Actual number of messages read</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success,
        /// PUDS_ERROR_OVERFLOW indicates success but buffer was too small to hold all responses.</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForFunctionalResponses_2013")]
        public static extern uds_status WaitForFunctionalResponses_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [In] ref uds_msg msg_request,
            UInt32 timeout,
            UInt32 timeout_enhanced,
            [MarshalAs(UnmanagedType.U1)]
            bool wait_until_timeout,
            UInt32 max_msg_count,
            [MarshalAs(UnmanagedType.LPArray,SizeParamIndex=5)]
            [Out] uds_msg[] out_msg_responses,
            out UInt32 out_msg_count
        );

        /// <summary>
        /// Handles the communication workflow for a UDS service expecting a single response.
        /// </summary>
        /// <remark>
        /// 1) Warning: The order of the parameters has changed in PCAN-UDS 2.0 API.
        /// 2) The function waits for a transmit confirmation then for a message response.
        /// Even if the SuppressPositiveResponseMessage flag is set, the function will still wait
        /// for an eventual Negative Response.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="msg_request">A sent uds_msg message used as a reference to manage the UDS service</param>
        /// <param name="out_msg_response">A uds_msg structure buffer to store the PUDS response</param>
        /// <param name="out_msg_request_confirmation">A uds_msg structure buffer to store the PUDS request confirmation</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForService_2013")]
        public static extern uds_status WaitForService_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            [In] ref uds_msg msg_request,
            out uds_msg out_msg_response,
            out uds_msg out_msg_request_confirmation);

        /// <summary>
        /// Handles the communication workflow for a UDS service expecting multiple responses.
        /// </summary>
        /// <remark>
        /// 1) Warning: The order of the parameters has changed in PCAN-UDS 2.0 API.
        /// 2) The function waits for a transmit confirmation then for N message responses.
        /// Even if the SuppressPositiveResponseMessage flag is set, the function will still wait
        /// for eventual Negative Responses.
        /// </remark>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="msg_request">sent uds_msg message</param>
        /// <param name="max_msg_count">Length of the buffer array (max. messages that can be received)</param>
        /// <param name="wait_until_timeout">if <code>FALSE</code> the function is interrupted if out_msg_count reaches max_msg_count.</param>
        /// <param name="out_msg_responses">Buffer must be an array of 'max_msg_count' entries (must have at least
        /// a size of max_msg_count * sizeof(uds_msg) bytes</param>
        /// <param name="out_msg_count">Actual number of messages read</param>
        /// <param name="out_msg_request_confirmation">A uds_msg structure buffer to store the PUDS request confirmation</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success,
        /// PUDS_ERROR_OVERFLOW indicates success but buffer was too small to hold all responses.</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForServiceFunctional_2013")]
        public static extern uds_status WaitForServiceFunctional_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            ref uds_msg msg_request,
            UInt32 max_msg_count,
            [MarshalAs(UnmanagedType.U1)]
            bool wait_until_timeout,
            [MarshalAs(UnmanagedType.LPArray,SizeParamIndex=2)]
            [Out] uds_msg[] out_msg_responses,
            out UInt32 out_msg_count,
            out uds_msg out_msg_request_confirmation);

        #endregion
    }
    #region PCAN UDS API Implementation: Services

    ////////////////////////////////////////////////////////////
    // PCAN-UDS API: UDS Service function declarations
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// PUDS Service ids defined in ISO 14229-1:2013
    /// </summary>
    public enum uds_service : Byte
    {
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_DiagnosticSessionControl = 0x10,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ECUReset = 0x11,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_SecurityAccess = 0x27,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_CommunicationControl = 0x28,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_TesterPresent = 0x3E,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_AccessTimingParameter = 0x83,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_SecuredDataTransmission = 0x84,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ControlDTCSetting = 0x85,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ResponseOnEvent = 0x86,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_LinkControl = 0x87,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ReadDataByIdentifier = 0x22,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ReadMemoryByAddress = 0x23,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ReadScalingDataByIdentifier = 0x24,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ReadDataByPeriodicIdentifier = 0x2A,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_DynamicallyDefineDataIdentifier = 0x2C,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_WriteDataByIdentifier = 0x2E,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_WriteMemoryByAddress = 0x3D,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ClearDiagnosticInformation = 0x14,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_ReadDTCInformation = 0x19,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_InputOutputControlByIdentifier = 0x2F,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_RoutineControl = 0x31,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_RequestDownload = 0x34,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_RequestUpload = 0x35,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_TransferData = 0x36,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_RequestTransferExit = 0x37,
        /// <summary>
        /// see ISO 14229-1:2013
        /// </summary>
        PUDS_SERVICE_SI_RequestFileTransfer = 0x38,
        /// <summary>
        /// see ISO 14229-1:2020
        /// </summary>
        PUDS_SERVICE_SI_Authentication = 0x29,
        /// <summary>
        /// negative response
        /// </summary>
        PUDS_SERVICE_NR_SI = 0x7F,
    }

    public static partial class UDSApi
    {
#if (!PUDS_API_COMPATIBILITY_ISO_2006)
        /// <summary>
        /// server wants more time
        /// </summary>
        public const Byte PUDS_NRC_EXTENDED_TIMING = 0x78;
        /// <summary>
        /// positive response offset
        /// </summary>
        public const Byte PUDS_SI_POSITIVE_RESPONSE = 0x40;
#endif

        #region UDS Service: DiagnosticSessionControl

        // ISO-14229-1:2013 §9.2.2.2 p.39
        public enum uds_svc_param_dsc : Byte
        {
            /// <summary>
            /// Default Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_DS = 0x01,
            /// <summary>
            /// ECU Programming Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_ECUPS = 0x02,
            /// <summary>
            /// ECU Extended Diagnostic Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_ECUEDS = 0x03,
            /// <summary>
            /// Safety System Diagnostic Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_SSDS = 0x04
        }

        /// <summary>
        /// The DiagnosticSessionControl service is used to enable different diagnostic sessions in the server.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="session_type">Subfunction parameter: type of the session (see PUDS_SVC_PARAM_DSC_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDiagnosticSessionControl_2013")]
        public static extern uds_status SvcDiagnosticSessionControl_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_dsc session_type);

        #endregion

        #region UDS Service: ECU_Reset
        // ISO-14229-1:2013 §9.3.2.2 p.43
        public enum uds_svc_param_er : Byte
        {
            /// <summary>
            /// Hard Reset
            /// </summary>
            PUDS_SVC_PARAM_ER_HR = 0x01,
            /// <summary>
            /// Key Off on Reset
            /// </summary>
            PUDS_SVC_PARAM_ER_KOFFONR = 0x02,
            /// <summary>
            /// Soft Reset
            /// </summary>
            PUDS_SVC_PARAM_ER_SR = 0x03,
            /// <summary>
            /// Enable Rapid Power Shutdown
            /// </summary>
            PUDS_SVC_PARAM_ER_ERPSD = 0x04,
            /// <summary>
            /// Disable Rapid Power Shutdown
            /// </summary>
            PUDS_SVC_PARAM_ER_DRPSD = 0x05,
        }
        /// <summary>
        /// The ECUReset service is used by the client to request a server reset.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="reset_type">Subfunction parameter: type of Reset (see PUDS_SVC_PARAM_ER_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcECUReset_2013")]
        public static extern uds_status SvcECUReset_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_er reset_type);
        #endregion

        #region UDS Service: SecurityAccess
        // ISO-14229-1:2013 §9.4.2.2 p.49

#if (!PUDS_API_COMPATIBILITY_ISO_2006)
        /// <summary>
        /// Request Seed
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_RSD_1 = 0x01;
        /// <summary>
        /// Request Seed
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_RSD_3 = 0x03;
        /// <summary>
        /// Request Seed
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_RSD_5 = 0x05;
        /// <summary>
        /// Request Seed (odd numbers)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_RSD_MIN = 0x07;
        /// <summary>
        /// Request Seed (odd numbers)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_RSD_MAX = 0x5F;
        /// <summary>
        /// Send Key
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_SK_2 = 0x02;
        /// <summary>
        /// Send Key
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_SK_4 = 0x04;
        /// <summary>
        /// Send Key
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_SK_6 = 0x06;
        /// <summary>
        /// Send Key (even numbers)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_SK_MIN = 0x08;
        /// <summary>
        /// Send Key (even numbers)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_SA_SK_MAX = 0x60;
#endif
        /// <summary>
        /// SecurityAccess service provides a means to access data and/or diagnostic services which have
        /// restricted access for security, emissions or safety reasons.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="security_access_type">Subfunction parameter: type of SecurityAccess (see PUDS_SVC_PARAM_SA_xxx)</param>
        /// <param name="security_access_data">If Requesting Seed, buffer is the optional data to transmit to a server (like identification).
        /// If Sending Key, data holds the value generated by the security algorithm corresponding to a specific "seed" value</param>
        /// <param name="security_access_data_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcSecurityAccess_2013")]
        public static extern uds_status SvcSecurityAccess_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte security_access_type,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            Byte[] security_access_data,
            UInt32 security_access_data_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcSecurityAccess_2013")]
        private static extern uds_status SvcSecurityAccess_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte security_access_type,
            IntPtr security_access_data,
            UInt32 security_access_data_size);

        /// <summary>
        /// SecurityAccess service provides a means to access data and/or diagnostic services which have
        /// restricted access for security, emissions or safety reasons.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="security_access_type">Subfunction parameter: type of SecurityAccess (see PUDS_SVC_PARAM_SA_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcSecurityAccess_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte security_access_type)
        {
            return SvcSecurityAccess_2013(channel, request_config, out out_msg_request, security_access_type, IntPtr.Zero, 0);
        }

        #endregion
        #region UDS Service: CommunicationControl
        // ISO-14229-1:2013 §9.5.2.2 p.54
        public enum uds_svc_param_cc : Byte
        {
            /// <summary>
            /// Enable Rx and Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_ERXTX = 0x00,
            /// <summary>
            /// Enable Rx and Disable Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_ERXDTX = 0x01,
            /// <summary>
            /// Disable Rx and Enable Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_DRXETX = 0x02,
            /// <summary>
            /// Disable Rx and Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_DRXTX = 0x03,
            /// <summary>
            /// Enable Rx And Disable Tx With Enhanced Address Information
            /// </summary>
            PUDS_SVC_PARAM_CC_ERXDTXWEAI = 0x04,
            /// <summary>
            /// Enable Rx And Tx With Enhanced Address Information
            /// </summary>
            PUDS_SVC_PARAM_CC_ERXTXWEAI = 0x05
        }

#if (!PUDS_API_COMPATIBILITY_ISO_2006)
        /// <summary>
        /// Application (01b)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_CC_FLAG_APPL = 0x01;
        /// <summary>
        /// NetworkManagement (10b)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_CC_FLAG_NWM = 0x02;
        /// <summary>
        /// Disable/Enable specified communicationType (see Flags APPL/NMW)
        /// in the receiving node and all connected networks
        /// </summary>
        public const Byte PUDS_SVC_PARAM_CC_FLAG_DESCTIRNCN = 0x00;
        /// <summary>
        /// Disable/Enable network which request is received on
        /// </summary>
        public const Byte PUDS_SVC_PARAM_CC_FLAG_DENWRIRO = 0xF0;
        /// <summary>
        /// Disable/Enable specific network identified by network number (minimum value)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_CC_FLAG_DESNIBNN_MIN = 0x10;
        /// <summary>
        /// Disable/Enable specific network identified by network number (maximum value)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_CC_FLAG_DESNIBNN_MAX = 0xE0;
        /// <summary>
        /// Mask for DESNIBNN bits
        /// </summary>
        public const Byte PUDS_SVC_PARAM_CC_FLAG_DESNIBNN_MASK = 0xF0;
#endif
        /// <summary>
        /// CommunicationControl service's purpose is to switch on/off the transmission
        /// and/or the reception of certain messages of (a) server(s).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="control_type">Subfunction parameter: type of CommunicationControl (see PUDS_SVC_PARAM_CC_xxx)</param>
        /// <param name="communication_type">a bit-code value to reference the kind of communication to be controlled,
        /// See PUDS_SVC_PARAM_CC_FLAG_xxx flags and ISO_14229-1:2013 §B.1 p.333 for bit-encoding</param>
        /// <param name="node_identification_number">Identify a node on a sub-network (only used with
        /// PUDS_SVC_PARAM_CC_ERXDTXWEAI or PUDS_SVC_PARAM_CC_ERXTXWEAI control type)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcCommunicationControl_2013")]
        public static extern uds_status SvcCommunicationControl_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_cc control_type,
            Byte communication_type,
            UInt16 node_identification_number);

        /// <summary>
        /// CommunicationControl service's purpose is to switch on/off the transmission
        /// and/or the reception of certain messages of (a) server(s).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="control_type">Subfunction parameter: type of CommunicationControl (see PUDS_SVC_PARAM_CC_xxx)</param>
        /// <param name="communication_type">a bit-code value to reference the kind of communication to be controlled,
        /// See PUDS_SVC_PARAM_CC_FLAG_xxx flags and ISO_14229-1:2013 §B.1 p.333 for bit-encoding</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcCommunicationControl_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_cc control_type,
            Byte communication_type)
        {
            return SvcCommunicationControl_2013(channel, request_config, out out_msg_request, control_type, communication_type, 0);
        }

        #endregion
        #region UDS Service: TesterPresent
        // ISO-14229-1:2013 §9.6.2.2 p.59
        public enum uds_svc_param_tp : Byte
        {
            /// <summary>
            /// Zero SubFunction
            /// </summary>
            PUDS_SVC_PARAM_TP_ZSUBF = 0x00
        }
        /// <summary>
        /// TesterPresent service indicates to a server (or servers) that a client is still connected
        /// to the vehicle and that certain diagnostic services and/or communications
        /// that have been previously activated are to remain active.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="testerpresent_type">No Subfunction parameter by default (PUDS_SVC_PARAM_TP_ZSUBF)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcTesterPresent_2013")]
        public static extern uds_status SvcTesterPresent_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_tp testerpresent_type);

        /// <summary>
        /// TesterPresent service indicates to a server (or servers) that a client is still connected
        /// to the vehicle and that certain diagnostic services and/or communications
        /// that have been previously activated are to remain active.
        /// No Subfunction parameter by default (PUDS_SVC_PARAM_TP_ZSUBF)
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcTesterPresent_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request)
        {
            return SvcTesterPresent_2013(channel, request_config, out out_msg_request, uds_svc_param_tp.PUDS_SVC_PARAM_TP_ZSUBF);
        }

        #endregion
        #region UDS Service: SecuredDataTransmission
        // ISO-14229-1:2013 §9.8 p.66
        /// <summary>
        /// SecuredDataTransmission(2013) service's purpose is to transmit data that is protected
        /// against attacks from third parties, which could endanger data security.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="security_data_request_record">buffer containing the data as processed by the Security Sub-Layer (See ISO-15764)</param>
        /// <param name="security_data_request_record_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcSecuredDataTransmission_2013")]
        public static extern uds_status SvcSecuredDataTransmission_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            Byte[] security_data_request_record,
            UInt32 security_data_request_record_size);

        // ISO-14229-1:2020 §16.2 p.358
        /// <summary>
        /// The messsage is a request message.
        /// </summary>
        public const Byte PUDS_SVC_PARAM_APAR_REQUEST_MSG_FLAG = 0x1;
        /// <summary>
        /// A pre - established key is used.
        /// </summary>
        public const Byte PUDS_SVC_PARAM_APAR_PRE_ESTABLISHED_KEY_FLAG = 0x8;
        /// <summary>
        /// Message is encrypted.
        /// </summary>
        public const Byte PUDS_SVC_PARAM_APAR_ENCRYPTED_MSG_FLAG = 0x10;
        /// <summary>
        /// Message is signed.
        /// </summary>
        public const Byte PUDS_SVC_PARAM_APAR_SIGNED_MSG_FLAG = 0x20;
        /// <summary>
        /// Signature on the response is requested.
        /// </summary>
        public const Byte PUDS_SVC_PARAM_APAR_REQUEST_RESPONSE_SIGNATURE_FLAG = 0x40;

        /// <summary>
        /// SecuredDataTransmission(2020) service's purpose is to transmit data that is protected
        /// against attacks from third parties, which could endanger data security.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="administrative_parameter">Security features used in the message (see PUDS_SVC_PARAM_APAR* definitions)</param>
        /// <param name="signature_encryption_calculation">Signature or encryption algorithm identifier</param>
        /// <param name="anti_replay_counter">Anti-replay counter value</param>
        /// <param name="internal_service_identifier">Internal message service request identifier</param>
        /// <param name="service_specific_parameters">Buffer that contains internal message service request data</param>
        /// <param name="service_specific_parameters_size">Internal message service request data size (in bytes)</param>
        /// <param name="signature_mac">Buffer that contains signature used to verify the message</param>
        /// <param name="signature_size">Size in bytes of the signature</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcSecuredDataTransmission_2020")]
        public static extern uds_status SvcSecuredDataTransmission_2020(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            UInt16 administrative_parameter,
            Byte signature_encryption_calculation,
            UInt16 anti_replay_counter,
            Byte internal_service_identifier,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            Byte[] service_specific_parameters,
            UInt32 service_specific_parameters_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 10)]
            Byte[] signature_mac,
            UInt16 signature_size);

        #endregion
        #region UDS Service: ControlDTCSetting
        // ISO-14229-1:2013 §9.9.2.2 p.72
        public enum uds_svc_param_cdtcs : Byte
        {
            /// <summary>
            /// The server(s) shall resume the setting of diagnostic trouble codes
            /// </summary>
            PUDS_SVC_PARAM_CDTCS_ON = 0x01,
            /// <summary>
            /// The server(s) shall stop the setting of diagnostic trouble codes
            /// </summary>
            PUDS_SVC_PARAM_CDTCS_OFF = 0x02
        }
        /// <summary>
        /// ControlDTCSetting service shall be used by a client to stop or resume the setting of
        /// diagnostic trouble codes (DTCs) in the server(s).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_setting_type">Subfunction parameter (see PUDS_SVC_PARAM_CDTCS_xxx)</param>
        /// <param name="dtc_setting_control_option_record">This parameter record is user-optional and transmits data to a server when controlling the DTC setting.
        /// It can contain a list of DTCs to be turned on or off.</param>
        /// <param name="dtc_setting_control_option_record_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcControlDTCSetting_2013")]
        public static extern uds_status SvcControlDTCSetting_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_cdtcs dtc_setting_type,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            Byte[] dtc_setting_control_option_record,
            UInt32 dtc_setting_control_option_record_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcControlDTCSetting_2013")]
        private static extern uds_status SvcControlDTCSetting_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_cdtcs dtc_setting_type,
            IntPtr dtc_setting_control_option_record,
            UInt32 dtc_setting_control_option_record_size);

        /// <summary>
        /// ControlDTCSetting service shall be used by a client to stop or resume the setting of
        /// diagnostic trouble codes (DTCs) in the server(s).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_setting_type">Subfunction parameter (see PUDS_SVC_PARAM_CDTCS_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcControlDTCSetting_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_cdtcs dtc_setting_type)
        {
            return SvcControlDTCSetting_2013(channel, request_config, out out_msg_request, dtc_setting_type, IntPtr.Zero, 0);
        }


        #endregion
        #region UDS Service: ResponseOnEvent
        // ISO-14229-1:2013 §9.10.2.2.1 p.78
        public enum uds_svc_param_roe : Byte
        {
            /// <summary>
            /// Stop Response On Event
            /// </summary>
            PUDS_SVC_PARAM_ROE_STPROE = 0x00,
            /// <summary>
            /// On DTC Status Change
            /// </summary>
            PUDS_SVC_PARAM_ROE_ONDTCS = 0x01,
            /// <summary>
            /// On Timer Interrupt
            /// </summary>
            PUDS_SVC_PARAM_ROE_OTI = 0x02,
            /// <summary>
            /// On Change Of Data Identifier
            /// </summary>
            PUDS_SVC_PARAM_ROE_OCODID = 0x03,
            /// <summary>
            /// Report Activated Events
            /// </summary>
            PUDS_SVC_PARAM_ROE_RAE = 0x04,
            /// <summary>
            /// Start Response On Event
            /// </summary>
            PUDS_SVC_PARAM_ROE_STRTROE = 0x05,
            /// <summary>
            /// Clear Response On Event
            /// </summary>
            PUDS_SVC_PARAM_ROE_CLRROE = 0x06,
            /// <summary>
            /// On Comparison Of Values
            /// </summary>
            PUDS_SVC_PARAM_ROE_OCOV = 0x07,
            /// <summary>
            /// Report Most Recent Dtc On Status Change (ISO 14229-1:2020 10.9.2.2 p.121)
            /// </summary>
            PUDS_SVC_PARAM_ROE_RMRDOSC = 0x08,
            /// <summary>
            /// Report Dtc Record Information On Dtc Status Change (ISO 14229-1:2020 10.9.2.2 p.121)
            /// </summary>
            PUDS_SVC_PARAM_ROE_RDRIODSC = 0x09
        }

#if (!PUDS_API_COMPATIBILITY_ISO_2006)
        /// <summary>
        /// Expected size of event type record for ROE_STPROE
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_STPROE_LEN = 0;
        /// <summary>
        /// Expected size of event type record for ROE_ONDTCS
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_ONDTCS_LEN = 1;
        /// <summary>
        /// Expected size of event type record for ROE_OTI
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_OTI_LEN = 1;
        /// <summary>
        /// Expected size of event type record for ROE_OCODID
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_OCODID_LEN = 2;
        /// <summary>
        /// Expected size of event type record for ROE_RAE
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_RAE_LEN = 0;
        /// <summary>
        /// Expected size of event type record for ROE_STRTROE
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_STRTROE_LEN = 0;
        /// <summary>
        /// Expected size of event type record for ROE_CLRROE
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_CLRROE_LEN = 0;
        /// <summary>
        /// Expected size of event type record for ROE_OCOV
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_OCOV_LEN = 10;
        /// <summary>
        /// Expected size of event type record for ROE_RMRDOSC
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_RMRDOSC_LEN = 1;
#endif
#if (!PUDS_API_COMPATIBILITY_ISO_2006)
        /// <summary>
        /// Infinite Time To Response (eventWindowTime parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_EWT_ITTR = 0x02;
#endif
        /// <summary>
        /// Short event window time (eventWindowTime parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_EWT_SEWT = 0x03;
        /// <summary>
        /// Medium event window time (eventWindowTime parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_EWT_MEWT = 0x04;
        /// <summary>
        /// Long event window time (eventWindowTime parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_EWT_LEWT = 0x05;
        /// <summary>
        /// Power window time (eventWindowTime parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_EWT_PWT = 0x06;
        /// <summary>
        /// Ignition window time (eventWindowTime parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_EWT_IWT = 0x07;
        /// <summary>
        /// Manufacturer trigger event window time (eventWindowTime parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_EWT_MTEWT = 0x08;

        /// <summary>
        /// Slow rate (onTimerInterrupt parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_OTI_SLOW_RATE = 0x01;
        /// <summary>
        /// Medium rate (onTimerInterrupt parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_OTI_MEDIUM_RATE = 0x02;
        /// <summary>
        /// Fast rate (onTimerInterrupt parameter)
        /// </summary>
        public const Byte PUDS_SVC_PARAM_ROE_OTI_FAST_RATE = 0x03;

        enum uds_svc_param_roe_recommended_service_id : Byte
        {
            /// <summary>
            /// Recommended service (first byte of service to respond to record)
            /// </summary>
            PUDS_SVC_PARAM_ROE_STRT_SI_RDBI = uds_service.PUDS_SERVICE_SI_ReadDataByIdentifier,
            /// <summary>
            /// Recommended service (first byte of service to respond to record)
            /// </summary>
            PUDS_SVC_PARAM_ROE_STRT_SI_RDTCI = uds_service.PUDS_SERVICE_SI_ReadDTCInformation,
            /// <summary>
            /// Recommended service (first byte of service to respond to record)
            /// </summary>
            PUDS_SVC_PARAM_ROE_STRT_SI_RC = uds_service.PUDS_SERVICE_SI_RoutineControl,
            /// <summary>
            /// Recommended service (first byte of service to respond to record)
            /// </summary>
            PUDS_SVC_PARAM_ROE_STRT_SI_IOCBI = uds_service.PUDS_SERVICE_SI_InputOutputControlByIdentifier
        }

        /// <summary>
        /// The ResponseOnEvent service requests a server to
        /// start or stop transmission of responses on a specified event.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="event_type">Subfunction parameter: event type (see PUDS_SVC_PARAM_ROE_xxx)</param>
        /// <param name="store_event">Storage State (TRUE = Store Event, FALSE = Do Not Store Event)</param>
        /// <param name="event_window_time">Specify a window for the event logic to be active in the server (see PUDS_SVC_PARAM_ROE_EWT_ITTR)</param>
        /// <param name="event_type_record">Additional parameters for the specified event type</param>
        /// <param name="event_type_record_size">Size in bytes of the event type record (see PUDS_SVC_PARAM_ROE_xxx_LEN)</param>
        /// <param name="service_to_respond_to_record">Service parameters, with first byte as service Id (see PUDS_SVC_PARAM_ROE_STRT_SI_xxx)</param>
        /// <param name="service_to_respond_to_record_size">Size in bytes of the service to respond to record</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcResponseOnEvent_2013")]
        public static extern uds_status SvcResponseOnEvent_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_roe event_type,
            [MarshalAs(UnmanagedType.I1)]
            bool store_event,
            Byte event_window_time,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            Byte[] event_type_record,
            UInt32 event_type_record_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 9)]
            Byte[] service_to_respond_to_record,
            UInt32 service_to_respond_to_record_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcResponseOnEvent_2013")]
        private static extern uds_status SvcResponseOnEvent_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_roe event_type,
            [MarshalAs(UnmanagedType.I1)]
            bool store_event,
            Byte event_window_time,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            Byte[] event_type_record,
            UInt32 event_type_record_size,
            IntPtr service_to_respond_to_record,
            UInt32 service_to_respond_to_record_size);

        /// <summary>
        /// The ResponseOnEvent service requests a server to
        /// start or stop transmission of responses on a specified event.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="event_type">Subfunction parameter: event type (see PUDS_SVC_PARAM_ROE_xxx)</param>
        /// <param name="store_event">Storage State (TRUE = Store Event, FALSE = Do Not Store Event)</param>
        /// <param name="event_window_time">Specify a window for the event logic to be active in the server (see PUDS_SVC_PARAM_ROE_EWT_ITTR)</param>
        /// <param name="event_type_record">Additional parameters for the specified event type</param>
        /// <param name="event_type_record_size">Size in bytes of the event type record (see PUDS_SVC_PARAM_ROE_xxx_LEN)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcResponseOnEvent_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_roe event_type,
            bool store_event,
            Byte event_window_time,
            Byte[] event_type_record,
            UInt32 event_type_record_size)
        {
            return SvcResponseOnEvent_2013(channel, request_config, out out_msg_request, event_type, store_event, event_window_time, event_type_record, event_type_record_size, IntPtr.Zero, 0);
        }

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcResponseOnEvent_2013")]
        private static extern uds_status SvcResponseOnEvent_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_roe event_type,
            [MarshalAs(UnmanagedType.I1)]
            bool store_event,
            Byte event_window_time,
            IntPtr event_type_record,
            UInt32 event_type_record_size,
            IntPtr service_to_respond_to_record,
            UInt32 service_to_respond_to_record_size);

        /// <summary>
        /// The ResponseOnEvent service requests a server to
        /// start or stop transmission of responses on a specified event.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="event_type">Subfunction parameter: event type (see PUDS_SVC_PARAM_ROE_xxx)</param>
        /// <param name="store_event">Storage State (TRUE = Store Event, FALSE = Do Not Store Event)</param>
        /// <param name="event_window_time">Specify a window for the event logic to be active in the server (see PUDS_SVC_PARAM_ROE_EWT_ITTR)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcResponseOnEvent_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_roe event_type,
            bool store_event,
            Byte event_window_time)
        {
            return SvcResponseOnEvent_2013(channel, request_config, out out_msg_request, event_type, store_event, event_window_time, IntPtr.Zero, 0, IntPtr.Zero, 0);
        }

        #endregion
        #region UDS Service: LinkControl
        // ISO-14229-1:2013 §9.11.2.2 p.101
        public enum uds_svc_param_lc : Byte
        {
            /// <summary>
            /// Verify Baudrate Transition With Fixed Baudrate
            /// </summary>
            PUDS_SVC_PARAM_LC_VBTWFBR = 0x01,
            /// <summary>
            /// Verify Baudrate Transition With Specific Baudrate
            /// </summary>
            PUDS_SVC_PARAM_LC_VBTWSBR = 0x02,
            /// <summary>
            /// Transition Baudrate
            /// </summary>
            PUDS_SVC_PARAM_LC_TB = 0x03
        }

        public enum uds_svc_param_lc_baudrate_identifier : Byte
        {
            /// <summary>
            /// standard PC baud rate of 9.6 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_9600 = 0x01,
            /// <summary>
            /// standard PC baud rate of 19.2 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_19200 = 0x02,
            /// <summary>
            /// standard PC baud rate of 38.4 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_38400 = 0x03,
            /// <summary>
            /// standard PC baud rate of 57.6 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_57600 = 0x04,
            /// <summary>
            /// standard PC baud rate of 115.2 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_115200 = 0x05,
            /// <summary>
            /// standard CAN baud rate of 125 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_125K = 0x10,
            /// <summary>
            /// standard CAN baud rate of 250 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_250K = 0x11,
            /// <summary>
            /// standard CAN baud rate of 500 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_500K = 0x12,
            /// <summary>
            /// standard CAN baud rate of 1 MBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_1M = 0x13,
            /// <summary>
            /// Programming setup
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PROGSU = 0x20
        }

        /// <summary>
        /// The LinkControl service is used to control the communication link baud rate
        /// between the client and the server(s) for the exchange of diagnostic data.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="link_control_type">Subfunction parameter: Link Control Type (see PUDS_SVC_PARAM_LC_xxx)</param>
        /// <param name="baudrate_identifier">defined baud rate identifier (see PUDS_SVC_PARAM_LC_BAUDRATE_xxx)</param>
        /// <param name="link_baudrate">used only with PUDS_SVC_PARAM_LC_VBTWSBR parameter:
        /// a three-byte value baud rate (baudrate High, Middle and Low bytes).</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success </returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcLinkControl_2013")]
        public static extern uds_status SvcLinkControl_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_lc link_control_type,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_lc_baudrate_identifier baudrate_identifier,
            UInt32 link_baudrate);

        /// <summary>
        /// The LinkControl service is used to control the communication link baud rate
        /// between the client and the server(s) for the exchange of diagnostic data.
        /// Use only if link_control_type parameter is not PUDS_SVC_PARAM_LC_VBTWSBR
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="link_control_type">Subfunction parameter: Link Control Type (see PUDS_SVC_PARAM_LC_xxx)</param>
        /// <param name="baudrate_identifier">defined baud rate identifier (see PUDS_SVC_PARAM_LC_BAUDRATE_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success </returns>
        public static uds_status SvcLinkControl_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_lc link_control_type,
            uds_svc_param_lc_baudrate_identifier baudrate_identifier)
        {
            return SvcLinkControl_2013(channel, request_config, out out_msg_request, link_control_type, baudrate_identifier, 0);
        }

        #endregion
        #region UDS Service: ReadDataByIdentifier
        // ISO-14229-1:2013 §C.1 p337
        public enum uds_svc_param_di : UInt16
        {
            /// <summary>
            /// bootSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_BSIDID = 0xF180,
            /// <summary>
            /// applicationSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ASIDID = 0xF181,
            /// <summary>
            /// applicationDataIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADIDID = 0xF182,
            /// <summary>
            /// bootSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_BSFPDID = 0xF183,
            /// <summary>
            /// applicationSoftwareFingerprintDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ASFPDID = 0xF184,
            /// <summary>
            /// applicationDataFingerprintDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADFPDID = 0xF185,
            /// <summary>
            /// activeDiagnosticSessionDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADSDID = 0xF186,
            /// <summary>
            /// vehicleManufacturerSparePartNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMSPNDID = 0xF187,
            /// <summary>
            /// vehicleManufacturerECUSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUSNDID = 0xF188,
            /// <summary>
            /// vehicleManufacturerECUSoftwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUSVNDID = 0xF189,
            /// <summary>
            /// systemSupplierIdentifierDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSIDDID = 0xF18A,
            /// <summary>
            /// ECUManufacturingDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ECUMDDID = 0xF18B,
            /// <summary>
            /// ECUSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ECUSNDID = 0xF18C,
            /// <summary>
            /// supportedFunctionalUnitsDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SFUDID = 0xF18D,
            /// <summary>
            /// vehicleManufacturerKitAssemblyPartNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMKAPNDID = 0xF18E,
            /// <summary>
            /// VINDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VINDID = 0xF190,
            /// <summary>
            /// vehicleManufacturerECUHardwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUHNDID = 0xF191,
            /// <summary>
            /// systemSupplierECUHardwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUHWNDID = 0xF192,
            /// <summary>
            /// systemSupplierECUHardwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUHWVNDID = 0xF193,
            /// <summary>
            /// systemSupplierECUSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUSWNDID = 0xF194,
            /// <summary>
            /// systemSupplierECUSoftwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUSWVNDID = 0xF195,
            /// <summary>
            /// exhaustRegulationOrTypeApprovalNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EROTANDID = 0xF196,
            /// <summary>
            /// systemNameOrEngineTypeDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SNOETDID = 0xF197,
            /// <summary>
            /// repairShopCodeOrTesterSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_RSCOTSNDID = 0xF198,
            /// <summary>
            /// programmingDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_PDDID = 0xF199,
            /// <summary>
            /// calibrationRepairShopCodeOrCalibrationEquipmentSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CRSCOCESNDID = 0xF19A,
            /// <summary>
            /// calibrationDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CDDID = 0xF19B,
            /// <summary>
            /// calibrationEquipmentSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CESWNDID = 0xF19C,
            /// <summary>
            /// ECUInstallationDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EIDDID = 0xF19D,
            /// <summary>
            /// ODXFileDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ODXFDID = 0xF19E,
            /// <summary>
            /// entityDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EDID = 0xF19F
        }

        // ISO-14229-1:2013 §10.2 p.106
        /// <summary>
        /// The ReadDataByIdentifier service allows the client to request data record values
        /// from the server identified by one or more dataIdentifiers.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="data_identifier">buffer containing a list of two-byte Data Identifiers (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="data_identifier_length">Number of elements in the buffer (size in uint16_t of the buffer)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDataByIdentifier_2013")]
        public static extern uds_status SvcReadDataByIdentifier_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 4)]
            uds_svc_param_di[] data_identifier,
            UInt32 data_identifier_length);


        #endregion
        #region UDS Service: ReadMemoryByAddress
        // ISO-14229-1:2013 §10.3 p.113
        /// <summary>
        /// The ReadMemoryByAddress service allows the client to request memory data from the server
        /// via a provided starting address and to specify the size of memory to be read.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="memory_address_buffer">starting address buffer of server memory from which data is to be retrieved</param>
        /// <param name="memory_address_size">Size in bytes of the memory_address_buffer (max.: 0xF)</param>
        /// <param name="memory_size_buffer">number of bytes to be read starting at the address specified by memory_address_buffer</param>
        /// <param name="memory_size_size">Size in bytes of the memory_size_buffer (max.: 0xF)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadMemoryByAddress_2013")]
        public static extern uds_status SvcReadMemoryByAddress_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] memory_address_buffer,
            byte memory_address_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)]
            byte[] memory_size_buffer,
            byte memory_size_size);

        #endregion
        #region UDS Service: ReadScalingDataByIdentifier
        // ISO-14229-1:2013 §10.4 p.119
        /// <summary>
        /// The ReadScalingDataByIdentifier service allows the client to request
        /// scaling data record information from the server identified by a dataIdentifier.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="data_identifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadScalingDataByIdentifier_2013")]
        public static extern uds_status SvcReadScalingDataByIdentifier_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_di data_identifier);

        #endregion
        #region UDS Service: ReadDataByPeriodicIdentifier
        // ISO-14229-1:2013 §C.4 p.351
        public enum uds_svc_param_rdbpi : Byte
        {
            /// <summary>
            /// Send At Slow Rate
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SASR = 0x01,
            /// <summary>
            /// Send At Medium Rate
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SAMR = 0x02,
            /// <summary>
            /// Send At Fast Rate
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SAFR = 0x03,
            /// <summary>
            /// Stop Sending
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SS = 0x04
        }

        /// <summary>
        /// The ReadDataByPeriodicIdentifier service allows the client to request the periodic transmission
        /// of data record values from the server identified by one or more periodicDataIdentifiers.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="transmission_mode">transmission rate code (see PUDS_SVC_PARAM_RDBPI_xxx)</param>
        /// <param name="periodic_data_identifier">buffer containing a list of Periodic Data Identifiers</param>
        /// <param name="periodic_data_identifier_size">Number of elements in the buffer (size in bytes of the buffer)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDataByPeriodicIdentifier_2013")]
        public static extern uds_status SvcReadDataByPeriodicIdentifier_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rdbpi transmission_mode,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] periodic_data_identifier,
            UInt32 periodic_data_identifier_size);

        #endregion
        #region UDS Service: DynamicallyDefineDataIdentifier
        // ISO-14229-1:2013 §10.6.2.2 p.144
        public enum uds_svc_param_dddi : Byte
        {
            /// <summary>
            /// Define By Identifier
            /// </summary>
            PUDS_SVC_PARAM_DDDI_DBID = 0x01,
            /// <summary>
            /// Define By Memory Address
            /// </summary>
            PUDS_SVC_PARAM_DDDI_DBMA = 0x02,
            /// <summary>
            /// Clear Dynamically Defined Data Identifier
            /// </summary>
            PUDS_SVC_PARAM_DDDI_CDDDI = 0x03
        }
        /// <summary>
        /// The DynamicallyDefineDataIdentifier service allows the client to dynamically define
        /// in a server a data identifier that can be read via the ReadDataByIdentifier service at a later time.
        /// The Define By Identifier subfunction specifies that definition of the dynamic data
        /// identifier shall occur via a data identifier reference.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dynamically_defined_data_identifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="source_data_identifier">buffer containing the sources of information to be included into the dynamic data record</param>
        /// <param name="memory_size">buffer containing the total numbers of bytes from the source data record address</param>
        /// <param name="position_in_source_data_record">buffer containing the starting byte positions of the excerpt of the source data record</param>
        /// <param name="number_of_elements">Number of elements in SourceDataIdentifier/position_in_source_data_record/memory_size triplet.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDynamicallyDefineDataIdentifierDBID_2013")]
        public static extern uds_status SvcDynamicallyDefineDataIdentifierDBID_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_di dynamically_defined_data_identifier,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            UInt16[] source_data_identifier,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            Byte[] memory_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            Byte[] position_in_source_data_record,
            UInt32 number_of_elements);

        /// <summary>
        /// The DynamicallyDefineDataIdentifier service allows the client to dynamically define
        /// in a server a data identifier that can be read via the ReadDataByIdentifier service at a later time.
        /// The Define By Memory Address subfunction specifies that definition of the dynamic data
        /// identifier shall occur via an address reference.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dynamically_defined_data_identifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="memory_address_size">Size in bytes of the memory address items in the memory_address_buffer (max.: 0xF)</param>
        /// <param name="memory_size_size">Size in bytes of the memory size items in the memory_size_buffer (max.: 0xF)</param>
        /// <param name="memory_address_buffer">buffer containing the memory address buffer,
        /// must be an array of 'number_of_elements' items whose size is 'memory_address_size'
        /// (size is 'number_of_elements * memory_address_size' bytes)</param>
        /// <param name="memory_size_buffer">buffer containing the memory size buffer,
        /// must be an array of 'number_of_elements' items whose size is 'memory_size_size'
        /// (size is 'number_of_elements * memory_size_size' bytes)</param>
        /// <param name="number_of_elements">Number of elements in memory_address_buffer/memory_size_buffer couple.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDynamicallyDefineDataIdentifierDBMA_2013")]
        public static extern uds_status SvcDynamicallyDefineDataIdentifierDBMA_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_di dynamically_defined_data_identifier,
            Byte memory_address_size,
            Byte memory_size_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            Byte[] memory_address_buffer,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            Byte[] memory_size_buffer,
            UInt32 number_of_elements);

        /// <summary>
        /// The Clear Dynamically Defined Data Identifier subfunction shall be used to clear
        /// the specified dynamic data identifier.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dynamically_defined_data_identifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDynamicallyDefineDataIdentifierCDDDI_2013")]
        public static extern uds_status SvcDynamicallyDefineDataIdentifierCDDDI_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_di dynamically_defined_data_identifier);

        /// <summary>
        /// The Clear All Dynamically Defined Data Identifier function shall be used to clear
        /// all dynamic data identifier declared in the server.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDynamicallyDefineDataIdentifierClearAllDDDI_2013")]
        public static extern uds_status SvcDynamicallyDefineDataIdentifierClearAllDDDI_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request);

        #endregion
        #region UDS Service: WriteDataByIdentifier
        // ISO-14229-1:2013 §10.7 p.162
        /// <summary>
        /// The WriteDataByIdentifier service allows the client to write information into the server at an internal location
        /// specified by the provided data identifier.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="data_identifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="data_record">buffer containing the data to write</param>
        /// <param name="data_record_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcWriteDataByIdentifier_2013")]
        public static extern uds_status SvcWriteDataByIdentifier_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_di  data_identifier,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            Byte[] data_record,
            UInt32 data_record_size);

        #endregion
        #region UDS Service: WriteMemoryByAddress
        // ISO-14229-1:2013 §10.8 p.167
        /// <summary>
        /// The WriteMemoryByAddress service allows the client to write
        /// information into the server at one or more contiguous memory locations.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="memory_address_buffer">Starting address buffer of server memory to which data is to be written</param>
        /// <param name="memory_address_size">Size in bytes of the memory_address_buffer (max.: 0xF)</param>
        /// <param name="memory_size_buffer">number of bytes to be written starting at the address specified by memory_address_buffer</param>
        /// <param name="memory_size_size">Size in bytes of the memory_size_buffer (max.: 0xF)</param>
        /// <param name="data_record">buffer containing the data to write</param>
        /// <param name="data_record_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcWriteMemoryByAddress_2013")]
        public static extern uds_status SvcWriteMemoryByAddress_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            Byte[] memory_address_buffer,
            Byte memory_address_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)]
            Byte[] memory_size_buffer,
            Byte memory_size_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            Byte[] data_record,
            UInt32 data_record_size);

        #endregion
        #region UDS Service: ClearDiagnosticInformation
        // ISO-14229-1:2013 §11.2 p.175
#if (!PUDS_API_COMPATIBILITY_ISO_2006)
        /// <summary>
        /// Emissions-related systems group of DTCs
        /// </summary>
        public const UInt32 PUDS_SVC_PARAM_CDI_ERS = 0x000000;
        /// <summary>
        /// All Groups of DTCs
        /// </summary>
        public const UInt32 PUDS_SVC_PARAM_CDI_AGDTC = 0xFFFFFF;
#endif
        /// <summary>
        /// The ClearDiagnosticInformation service is used by the client to clear diagnostic information
        /// in one server's or multiple servers' memory.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="group_of_dtc">a three-byte value indicating the group of DTCs (e.g. powertrain, body, chassis)
        /// or the particular DTC to be cleared (see PUDS_SVC_PARAM_CDI_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcClearDiagnosticInformation_2013")]
        public static extern uds_status SvcClearDiagnosticInformation_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            UInt32 group_of_dtc);

        /// <summary>
        /// The ClearDiagnosticInformation service is used by the client to clear diagnostic information
        /// in one server's or multiple servers' memory.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="group_of_dtc">a three-byte value indicating the group of DTCs (e.g. powertrain, body, chassis)
        /// or the particular DTC to be cleared (see PUDS_SVC_PARAM_CDI_xxx)</param>
        /// <param name="memory_selection">User defined DTC memory</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcClearDiagnosticInformation_2020")]
        public static extern uds_status SvcClearDiagnosticInformation_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            UInt32 group_of_dtc,
            Byte memory_selection);

        #endregion
        #region UDS Service: ReadDTCInformation
        // ISO-14229-1:2013 §11.3.2.2 p.194
        public enum uds_svc_param_rdtci : Byte
        {
            /// <summary>
            /// report Number Of DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNODTCBSM = 0x01,
            /// <summary>
            /// report DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCBSM = 0x02,
            /// <summary>
            /// report DTC Snapshot Identification
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCSSI = 0x03,
            /// <summary>
            /// report DTC Snapshot Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCSSBDTC = 0x04,
            /// <summary>
            /// report DTC Snapshot Record By Record Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCSSBRN = 0x05,
            /// <summary>
            /// report DTC Extended Data Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCEDRBDN = 0x06,
            /// <summary>
            /// report Number Of DTC By Severity Mask Record
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNODTCBSMR = 0x07,
            /// <summary>
            /// report DTC By Severity Mask Record
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCBSMR = 0x08,
            /// <summary>
            /// report Severity Information Of DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RSIODTC = 0x09,
            /// <summary>
            /// report Supported DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RSUPDTC = 0x0A,
            /// <summary>
            /// report First Test Failed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RFTFDTC = 0x0B,
            /// <summary>
            /// report First Confirmed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RFCDTC = 0x0C,
            /// <summary>
            /// report Most Recent Test Failed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMRTFDTC = 0x0D,
            /// <summary>
            /// report Most Recent Confirmed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMRCDTC = 0x0E,
            /// <summary>
            /// report Mirror Memory DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMMDTCBSM = 0x0F,
            /// <summary>
            /// report Mirror Memory DTC Extended Data Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMMDEDRBDN = 0x10,
            /// <summary>
            /// report Number Of Mirror MemoryDTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNOMMDTCBSM = 0x11,
            /// <summary>
            /// report Number Of Emissions Related OBD DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNOOBDDTCBSM = 0x12,
            /// <summary>
            /// report Emissions Related OBD DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_ROBDDTCBSM = 0x13,

            /// <summary>
            /// report DTC Ext Data Record By Record Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCEDBR = 0x16,
            /// <summary>
            /// report User Def Memory DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RUDMDTCBSM = 0x17,
            /// <summary>
            /// report User Def Memory DTC Snapshot Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RUDMDTCSSBDTC = 0x18,
            /// <summary>
            /// report User Def Memory DTC Ext Data Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RUDMDTCEDRBDN = 0x19,
            /// <summary>
            /// report report DTC Extended Data Record Identification (ISO_14229-1 2020)
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCEDI = 0x1A,
            /// <summary>
            /// report WWHOBD DTC By Mask Record
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RWWHOBDDTCBMR = 0x42,
            /// <summary>
            /// report WWHOBD DTC With Permanent Status
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RWWHOBDDTCWPS = 0x55,
            /// <summary>
            /// report DTC Information By DTC Readiness Group Identifier (ISO_14229-1 2020)
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCBRGI = 0x56,
            // Reminder: following parameters were not defined as they are NOT in ISO-15765-3 :
            /// <summary>
            /// report DTC Fault Detection Counter
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCFDC = 0x14,
            /// <summary>
            /// report DTC With Permanent Status
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCWPS = 0x15
        }

        // DTCSeverityMask (DTCSVM): ISO-14229-1:2013 §D.3 p.366
        [Flags]
        public enum uds_svc_param_rdtci_dtcsvm : Byte
        {
            /// <summary>
            /// DTC severity bit definitions: no SeverityAvailable
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_NSA = 0x00,
            /// <summary>
            /// DTC severity bit definitions: maintenance Only
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_MO = 0x20,
            /// <summary>
            /// DTC severity bit definitions: check At Next Halt
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_CHKANH = 0x40,
            /// <summary>
            /// DTC severity bit definitions: check Immediately
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_CHKI = 0x80
        }
        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// Only reportNumberOfDTCByStatusMask, reportDTCByStatusMask, reportMirrorMemoryDTCByStatusMask,
        /// reportNumberOfMirrorMemoryDTCByStatusMask, reportNumberOfEmissionsRelatedOBDDTCByStatusMask,
        /// reportEmissionsRelatedOBDDTCByStatusMask Sub-functions are allowed.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="PUDS_SVC_PARAM_RDTCI_Type">Subfunction parameter: ReadDTCInformation type, use one of the following:
        /// PUDS_SVC_PARAM_RDTCI_RNODTCBSM, PUDS_SVC_PARAM_RDTCI_RDTCBSM,
        /// PUDS_SVC_PARAM_RDTCI_RMMDTCBSM, PUDS_SVC_PARAM_RDTCI_RNOMMDTCBSM,
        /// PUDS_SVC_PARAM_RDTCI_RNOOBDDTCBSM, PUDS_SVC_PARAM_RDTCI_ROBDDTCBSM</param>
        /// <param name="dtc_status_mask">Contains eight DTC status bit.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformation_2013")]
        public static extern uds_status SvcReadDTCInformation_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rdtci PUDS_SVC_PARAM_RDTCI_Type,
            Byte dtc_status_mask);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportDTCSnapshotRecordByDTCNumber (PUDS_SVC_PARAM_RDTCI_RDTCSSBDTC) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_mask">a unique identification number (three byte value) for a specific diagnostic trouble code</param>
        /// <param name="dtc_snapshot_record_number">the number of the specific DTCSnapshot data records</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRDTCSSBDTC_2013")]
        public static extern uds_status SvcReadDTCInformationRDTCSSBDTC_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            UInt32 dtc_mask,
            Byte dtc_snapshot_record_number);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportDTCSnapshotByRecordNumber (PUDS_SVC_PARAM_RDTCI_RDTCSSBRN) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_snapshot_record_number">the number of the specific DTCSnapshot data records</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRDTCSSBRN_2013")]
        public static extern uds_status SvcReadDTCInformationRDTCSSBRN_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte dtc_snapshot_record_number);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// Only reportDTCExtendedDataRecordByDTCNumber and reportMirrorMemoryDTCExtendedDataRecordByDTCNumber Sub-functions are allowed.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="PUDS_SVC_PARAM_RDTCI_Type">Subfunction parameter: ReadDTCInformation type, use one of the following:
        /// PUDS_SVC_PARAM_RDTCI_RDTCEDRBDN, PUDS_SVC_PARAM_RDTCI_RMMDEDRBDN</param>
        /// <param name="dtc_mask">a unique identification number (three byte value) for a specific diagnostic trouble code</param>
        /// <param name="dtc_extended_data_record_number">the number of the specific DTCExtendedData record requested.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationReportExtended_2013")]
        public static extern uds_status SvcReadDTCInformationReportExtended_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rdtci PUDS_SVC_PARAM_RDTCI_Type,
            UInt32 dtc_mask,
            Byte dtc_extended_data_record_number);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// Only reportNumberOfDTCBySeverityMaskRecord and reportDTCSeverityInformation Sub-functions are allowed.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="PUDS_SVC_PARAM_RDTCI_Type">Subfunction parameter: ReadDTCInformation type, use one of the following:
        /// PUDS_SVC_PARAM_RDTCI_RNODTCBSMR, PUDS_SVC_PARAM_RDTCI_RDTCBSMR</param>
        /// <param name="dtc_severity_mask">a mask of eight (8) DTC severity bits (see PUDS_SVC_PARAM_RDTCI_DTCSVM_xxx)</param>
        /// <param name="dtc_status_mask">a mask of eight (8) DTC status bits</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationReportSeverity_2013")]
        public static extern uds_status SvcReadDTCInformationReportSeverity_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rdtci PUDS_SVC_PARAM_RDTCI_Type,
            byte dtc_severity_mask,
            Byte dtc_status_mask);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportSeverityInformationOfDTC (PUDS_SVC_PARAM_RDTCI_RSIODTC) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_mask">a unique identification number for a specific diagnostic trouble code</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRSIODTC_2013")]
        public static extern uds_status SvcReadDTCInformationRSIODTC_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            UInt32 dtc_mask);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code _2013(DTC) information.
        /// Only reportSupportedDTC, reportFirstTestFailedDTC, reportFirstConfirmedDTC, reportMostRecentTestFailedDTC,
        /// reportMostRecentConfirmedDTC, reportDTCFaultDetectionCounter, reportDTCWithPermanentStatus,
        /// and reportDTCSnapshotIdentification Sub-functions are allowed.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="PUDS_SVC_PARAM_RDTCI_Type">Subfunction parameter: ReadDTCInformation type, use one of the following:
        /// PUDS_SVC_PARAM_RDTCI_RFTFDTC, PUDS_SVC_PARAM_RDTCI_RFCDTC,
        /// PUDS_SVC_PARAM_RDTCI_RMRTFDTC, PUDS_SVC_PARAM_RDTCI_RMRCDTC,
        /// PUDS_SVC_PARAM_RDTCI_RSUPDTC, PUDS_SVC_PARAM_RDTCI_RDTCWPS,
        /// PUDS_SVC_PARAM_RDTCI_RDTCSSI, PUDS_SVC_PARAM_RDTCI_RDTCFDC</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationNoParam_2013")]
        public static extern uds_status SvcReadDTCInformationNoParam_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rdtci PUDS_SVC_PARAM_RDTCI_Type);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportDTCExtDataRecordByRecordNumber (PUDS_SVC_PARAM_RDTCI_RDTCEDBR) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_extended_data_record_number">DTC extended data record number</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRDTCEDBR_2013")]
        public static extern uds_status SvcReadDTCInformationRDTCEDBR_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte dtc_extended_data_record_number);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportUserDefMemoryDTCByStatusMask (PUDS_SVC_PARAM_RDTCI_RUDMDTCBSM) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_status_mask">a mask of eight (8) DTC status bits</param>
        /// <param name="memory_selection">Memory selection</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRUDMDTCBSM_2013")]
        public static extern uds_status SvcReadDTCInformationRUDMDTCBSM_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte dtc_status_mask,
            Byte memory_selection);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportUserDefMemoryDTCSnapshotRecordByDTCNumber (PUDS_SVC_PARAM_RDTCI_RUDMDTCSSBDTC) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_mask">a unique identification number (three byte value) for a specific diagnostic trouble code</param>
        /// <param name="user_def_dtc_snapshot_record_number">User DTC snapshot record number</param>
        /// <param name="memory_selection">Memory selection</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRUDMDTCSSBDTC_2013")]
        public static extern uds_status SvcReadDTCInformationRUDMDTCSSBDTC_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            UInt32 dtc_mask,
            Byte user_def_dtc_snapshot_record_number,
            Byte memory_selection);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportUserDefMemoryDTCExtDataRecordByDTCNumber (PUDS_SVC_PARAM_RDTCI_RUDMDTCEDRBDN) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_mask">a unique identification number (three byte value) for a specific diagnostic trouble code</param>
        /// <param name="dtc_extended_data_record_number">DTC extened data record number</param>
        /// <param name="memory_selection">Memory selection</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRUDMDTCEDRBDN_2013")]
        public static extern uds_status SvcReadDTCInformationRUDMDTCEDRBDN_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            UInt32 dtc_mask,
            Byte dtc_extended_data_record_number,
            Byte memory_selection);

        /// <summary>
        /// ISO_14229-1 2020
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportSupportedDTCExtDataRecord (PUDS_SVC_PARAM_RDTCI_RDTCEDI) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="dtc_extended_data_record_number">DTC extended data record number</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRDTCEDI_2020")]
        public static extern uds_status SvcReadDTCInformationRDTCEDI_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte dtc_extended_data_record_number);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportWWHOBDDTCByMaskRecord (PUDS_SVC_PARAM_RDTCI_RWWHOBDDTCBMR) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="functional_group_identifier">Functional group identifier</param>
        /// <param name="dtc_status_mask">a mask of eight (8) DTC status bits</param>
        /// <param name="dtc_severity_mask">a mask of eight (8) DTC severity bits (see PUDS_SVC_PARAM_RDTCI_DTCSVM_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRWWHOBDDTCBMR_2013")]
        public static extern uds_status SvcReadDTCInformationRWWHOBDDTCBMR_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte functional_group_identifier,
            Byte dtc_status_mask,
            Byte dtc_severity_mask);

        /// <summary>
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportWWHOBDDTCWithPermanentStatus (PUDS_SVC_PARAM_RDTCI_RWWHOBDDTCWPS ) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="functional_group_identifier">Functional group identifier</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRWWHOBDDTCWPS_2013")]
        public static extern uds_status SvcReadDTCInformationRWWHOBDDTCWPS_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte functional_group_identifier);

        /// <summary>
        /// ISO_14229-1 2020
        /// This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// The sub-function reportDTCInformationByDTCReadinessGroupIdentifier (PUDS_SVC_PARAM_RDTCI_RDTCBRGI ) is implicit.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="functional_group_identifier">Functional group identifier</param>
        /// <param name="dtc_readiness_group_identifier">DTC readiness group identifier</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRDTCBRGI_2020")]
        public static extern uds_status SvcReadDTCInformationRDTCBRGI_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte functional_group_identifier,
            Byte dtc_readiness_group_identifier);

        #endregion
        #region UDS Service: InputOutputControlByIdentifier
        // ISO-14229-1:2013 §E.1 p.374
        public enum uds_svc_param_iocbi : Byte
        {
            /// <summary>
            /// inputOutputControlParameter: returnControlToECU (0 controlState bytes in request)
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_RCTECU = 0x00,
            /// <summary>
            /// inputOutputControlParameter: resetToDefault (0 controlState bytes in request)
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_RTD = 0x01,
            /// <summary>
            /// inputOutputControlParameter: freezeCurrentState (0 controlState bytes in request)
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_FCS = 0x02,
            /// <summary>
            /// inputOutputControlParameter: shortTermAdjustment
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_STA = 0x03
        }
        /// <summary>
        /// The InputOutputControlByIdentifier service is used by the client to substitute a value for an input signal,
        /// internal server function and/or control an output (actuator) of an electronic system.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="data_identifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="control_option_record">First byte can be used as either an inputOutputControlParameter
        /// that describes how the server shall control its inputs or outputs (see PUDS_SVC_PARAM_IOCBI_xxx),
        /// or as an additional controlState byte</param>
        /// <param name="control_option_record_size">Size in bytes of the control_option_record buffer</param>
        /// <param name="control_enable_mask_record">The control_enable_mask_record shall only be supported when
        /// the inputOutputControlParameter is used (see control_option_record) and the dataIdentifier to be controlled consists
        /// of more than one parameter (i.e. the dataIdentifier is bit-mapped or packeted by definition).
        /// There shall be one bit in the control_enable_mask_record corresponding to each individual parameter
        /// defined within the dataIdentifier.</param>
        /// <param name="control_enable_mask_record_size">Size in bytes of the control_enable_mask_record buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcInputOutputControlByIdentifier_2013")]
        public static extern uds_status SvcInputOutputControlByIdentifier_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_di data_identifier,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] control_option_record,
            UInt32 control_option_record_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            byte[] control_enable_mask_record,
            UInt32 control_enable_mask_record_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcInputOutputControlByIdentifier_2013")]
        private static extern uds_status SvcInputOutputControlByIdentifier_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_di data_identifier,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] control_option_record,
            UInt32 control_option_record_size,
            IntPtr control_enable_mask_record,
            UInt32 control_enable_mask_record_size);

        /// <summary>
        /// The InputOutputControlByIdentifier service is used by the client to substitute a value for an input signal,
        /// internal server function and/or control an output (actuator) of an electronic system.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="data_identifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="control_option_record">First byte can be used as either an inputOutputControlParameter
        /// that describes how the server shall control its inputs or outputs (see PUDS_SVC_PARAM_IOCBI_xxx),
        /// or as an additional controlState byte</param>
        /// <param name="control_option_record_size">Size in bytes of the control_option_record buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcInputOutputControlByIdentifier_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_di data_identifier,
            byte[] control_option_record,
            UInt32 control_option_record_size)
        {
            return SvcInputOutputControlByIdentifier_2013(channel, request_config, out out_msg_request,
                data_identifier, control_option_record, control_option_record_size, IntPtr.Zero, 0);
        }

        #endregion
        #region UDS Service: RoutineControl
        // ISO-14229-1:2013 §13.2.2.2 p.262
        public enum uds_svc_param_rc : Byte
        {
            /// <summary>
            /// Start Routine
            /// </summary>
            PUDS_SVC_PARAM_RC_STR = 0x01,
            /// <summary>
            /// Stop Routine
            /// </summary>
            PUDS_SVC_PARAM_RC_STPR = 0x02,
            /// <summary>
            /// Request Routine Results
            /// </summary>
            PUDS_SVC_PARAM_RC_RRR = 0x03
        }
        // routineIdentifier: ISO-14229-1:2013 §F.1 p.375
        public enum uds_svc_param_rc_rid : UInt16
        {
            /// <summary>
            /// routineIdentifier: DeployLoopRoutineID
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_DLRI_ = 0xE200,
            /// <summary>
            /// routineIdentifier: eraseMemory
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_EM_ = 0xFF00,
            /// <summary>
            /// routineIdentifier: checkProgrammingDependencies
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_CPD_ = 0xFF01,
            /// <summary>
            /// routineIdentifier: eraseMirrorMemoryDTCs
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_EMMDTC_ = 0xFF02
        }

        /// <summary>
        /// The RoutineControl service is used by the client to start/stop a routine,
        /// and request routine results.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="routine_control_type">Subfunction parameter: RoutineControl type (see PUDS_SVC_PARAM_RC_xxx)</param>
        /// <param name="routine_identifier">Server Local Routine Identifier (see PUDS_SVC_PARAM_RC_RID_xxx)</param>
        /// <param name="routine_control_option_record">buffer containing the Routine Control Options (only with start and stop routine sub-functions)</param>
        /// <param name="routine_control_option_record_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRoutineControl_2013")]
        public static extern uds_status SvcRoutineControl_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rc routine_control_type,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_rc_rid routine_identifier,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)]
            byte[] routine_control_option_record,
            UInt32 routine_control_option_record_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRoutineControl_2013")]
        private static extern uds_status SvcRoutineControl_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rc routine_control_type,
            [MarshalAs(UnmanagedType.U2)]
            uds_svc_param_rc_rid routine_identifier,
            IntPtr routine_control_option_record,
            UInt32 routine_control_option_record_size);

        /// <summary>
        /// The RoutineControl service is used by the client to start/stop a routine,
        /// and request routine results.
        /// Use only with not start nor stop routine sub-functions.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="routine_control_type">Subfunction parameter: RoutineControl type (see PUDS_SVC_PARAM_RC_xxx)</param>
        /// <param name="routine_identifier">Server Local Routine Identifier (see PUDS_SVC_PARAM_RC_RID_xxx)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcRoutineControl_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_rc routine_control_type,
            uds_svc_param_rc_rid routine_identifier)
        {
            return SvcRoutineControl_2013(channel, request_config, out out_msg_request, routine_control_type, routine_identifier, IntPtr.Zero, 0);
        }

        #endregion
        #region UDS Service: RequestDownload
        // ISO-14229-1:2013 §14.2 p.270
        /// <summary>
        /// The requestDownload service is used by the client to initiate a data transfer
        /// from the client to the server (download).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="compression_method">A nibble-value that specifies the "compressionMethod",
        /// The value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="encrypting_method">A nibble-value that specifies the "encryptingMethod",
        /// The value 0x0 specifies that no encryptingMethod is used.</param>
        /// <param name="memory_address_buffer">starting address of server memory to which data is to be written</param>
        /// <param name="memory_address_size">Size in bytes of the memory_address_buffer buffer (max.: 0xF)</param>
        /// <param name="memory_size_buffer">used by the server to compare the uncompressed memory size with
        /// the total amount of data transferred during the TransferData service</param>
        /// <param name="memory_size_size">Size in bytes of the memory_size_buffer buffer (max.: 0xF)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestDownload_2013")]
        public static extern uds_status SvcRequestDownload_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte compression_method,
            Byte encrypting_method,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)]
            byte[] memory_address_buffer,
            byte memory_address_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            byte[] memory_size_buffer,
            byte memory_size_size);

        #endregion
        #region UDS Service: RequestUpload
        // ISO-14229-1:2013 §14.3 p.275
        /// <summary>
        /// The requestUpload service is used by the client to initiate a data transfer
        /// from the server to the client (upload).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="compression_method">A nibble-value that specifies the "compressionMethod",
        /// The value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="encrypting_method">A nibble-value that specifies the "encryptingMethod",
        /// The value 0x0 specifies that no encryptingMethod is used.</param>
        /// <param name="memory_address_buffer">starting address of server memory from which data is to be retrieved</param>
        /// <param name="memory_address_size">Size in bytes of the memory_address_buffer buffer (max.: 0xF)</param>
        /// <param name="memory_size_buffer">used by the server to compare the uncompressed memory size with
        /// the total amount of data transferred during the TransferData service</param>
        /// <param name="memory_size_size">Size in bytes of the memory_size_buffer buffer (max.: 0xF)</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestUpload_2013")]
        public static extern uds_status SvcRequestUpload_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte compression_method,
            byte encrypting_method,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)]
            byte[] memory_address_buffer,
            byte memory_address_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            byte[] memory_size_buffer,
            byte memory_size_size);

        #endregion
        #region UDS Service: TransferData
        // ISO-14229-1:2013 §14.4 p.280
        /// <summary>
        /// The TransferData service is used by the client to transfer data either from the client
        /// to the server (download) or from the server to the client (upload).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="block_sequence_counter">The blockSequenceCounter parameter value starts at 01 hex
        /// with the first TransferData request that follows the RequestDownload (34 hex)
        /// or RequestUpload (35 hex) service. Its value is incremented by 1 for each subsequent
        /// TransferData request. At the value of FF hex, the blockSequenceCounter rolls over
        /// and starts at 00 hex with the next TransferData request message.</param>
        /// <param name="transfer_request_parameter_record">buffer containing the required transfer parameters</param>
        /// <param name="transfer_request_parameter_record_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcTransferData_2013")]
        public static extern uds_status SvcTransferData_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte block_sequence_counter,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] transfer_request_parameter_record,
            UInt32 transfer_request_parameter_record_size);


        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcTransferData_2013")]
        private static extern uds_status SvcTransferData_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte block_sequence_counter,
            IntPtr transfer_request_parameter_record,
            UInt32 transfer_request_parameter_record_size);

        /// <summary>
        /// The TransferData service is used by the client to transfer data either from the client
        /// to the server (download) or from the server to the client (upload).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="block_sequence_counter">The blockSequenceCounter parameter value starts at 01 hex
        /// with the first TransferData request that follows the RequestDownload (34 hex)
        /// or RequestUpload (35 hex) service. Its value is incremented by 1 for each subsequent
        /// TransferData request. At the value of FF hex, the blockSequenceCounter rolls over
        /// and starts at 00 hex with the next TransferData request message.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcTransferData_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte block_sequence_counter)
        {
            return SvcTransferData_2013(channel, request_config, out out_msg_request, block_sequence_counter, IntPtr.Zero, 0);
        }

        #endregion
        #region UDS Service: RequestTransferExit
        // ISO-14229-1:2013 §14.5 p.285
        /// <summary>
        /// The RequestTransferExit service is used by the client to terminate a data
        /// transfer between client and server (upload or download).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="transfer_request_parameter_record">buffer containing the required transfer parameters</param>
        /// <param name="transfer_request_parameter_record_size">Size in bytes of the buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestTransferExit_2013")]
        public static extern uds_status SvcRequestTransferExit_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] transfer_request_parameter_record,
            UInt32 transfer_request_parameter_record_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestTransferExit_2013")]
        private static extern uds_status SvcRequestTransferExit_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            IntPtr transfer_request_parameter_record,
            UInt32 transfer_request_parameter_record_size);

        /// <summary>
        /// The RequestTransferExit service is used by the client to terminate a data
        /// transfer between client and server (upload or download).
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcRequestTransferExit_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request)
        {
            return SvcRequestTransferExit_2013(channel, request_config, out out_msg_request, IntPtr.Zero, 0);
        }

        #endregion
        #region UDS Service: AccessTimingParameter
        // See ISO-14229-1:2013 §9.7.2.2 p.62 table 74
        public enum uds_svc_param_atp : Byte
        {
            /// <summary>
            /// Read Extended Timing Parameter Set
            /// </summary>
            PUDS_SVC_PARAM_ATP_RETPS = 0x01,
            /// <summary>
            /// Set Timing Parameters To Default Values
            /// </summary>
            PUDS_SVC_PARAM_ATP_STPTDV = 0x02,
            /// <summary>
            /// Read Currently Active Timing Parameters
            /// </summary>
            PUDS_SVC_PARAM_ATP_RCATP = 0x03,
            /// <summary>
            /// Set Timing Parameters To Given Values
            /// </summary>
            PUDS_SVC_PARAM_ATP_STPTGV = 0x04
        }
        /// <summary>
        ///  AccessTimingParameter service.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="access_type">Access type, see PUDS_SVC_PARAM_ATP_* values</param>
        /// <param name="request_record">Timing parameter request record</param>
        /// <param name="request_record_size">Size in byte of the request record</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAccessTimingParameter_2013")]
        public static extern uds_status SvcAccessTimingParameter_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_atp access_type,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] request_record,
            UInt32 request_record_size);

        #endregion
        #region UDS Service: RequestFileTransfer
        // See ISO-14229-1:2013 Annex G p.376 table G.1
        public enum uds_svc_param_rft_moop : Byte
        {
            /// <summary>
            /// Add File
            /// </summary>
            PUDS_SVC_PARAM_RFT_MOOP_ADDFILE = 0x1,
            /// <summary>
            /// Delete File
            /// </summary>
            PUDS_SVC_PARAM_RFT_MOOP_DELFILE = 0x2,
            /// <summary>
            /// Replace File
            /// </summary>
            PUDS_SVC_PARAM_RFT_MOOP_REPLFILE = 0x3,
            /// <summary>
            /// Read File
            /// </summary>
            PUDS_SVC_PARAM_RFT_MOOP_RDFILE = 0x4,
            /// <summary>
            /// Read Dir
            /// </summary>
            PUDS_SVC_PARAM_RFT_MOOP_RDDIR = 0x5,
            /// <summary>
            /// Resume File (ISO-14229-1:2020 Annex G p.447 table G.1)
            /// </summary>
            PUDS_SVC_PARAM_RFT_MOOP_RSFILE = 0x6
        }
        /// <summary>
        ///  RequestFileTransfer service.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="mode_of_operation">Mode of operation (add, delete, replace, read), see PUDS_SVC_PARAM_RFT_MOOP_* values</param>
        /// <param name="file_path_and_name_size">Size in bytes of file_path_and_name buffer</param>
        /// <param name="file_path_and_name">File path and name string</param>
        /// <param name="compression_method">A nibble-value that specifies the "compressionMethod", the value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="encrypting_method">A nibble-value that specifies the "encryptingMethod", the value 0x0 specifies that no encryptingMethod is used.</param>
        /// <param name="file_size_parameter_size">Size in byte of file_size_uncompressed and file_size_compressed parameters</param>
        /// <param name="file_size_uncompressed">Uncompressed file size</param>
        /// <param name="file_size_compressed">Compressed file size</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestFileTransfer_2013")]
        public static extern uds_status SvcRequestFileTransfer_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] file_path_and_name,
            byte compression_method,
            byte encrypting_method,
            byte file_size_parameter_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            byte[] file_size_uncompressed,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            byte[] file_size_compressed);

        /// <summary>
        ///  RequestFileTransfer service.
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="mode_of_operation">Mode of operation (add, delete, replace, read), see PUDS_SVC_PARAM_RFT_MOOP_* values</param>
        /// <param name="file_path_and_name_size">Size in bytes of file_path_and_name buffer</param>
        /// <param name="file_path_and_name">File path and name string</param>
        /// <param name="compression_method">A nibble-value that specifies the "compressionMethod", the value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="encrypting_method">A nibble-value that specifies the "encryptingMethod", the value 0x0 specifies that no encryptingMethod is used.</param>
        /// <param name="file_size_parameter_size">Size in byte of file_size_uncompressed and file_size_compressed parameters</param>
        /// <param name="file_size_uncompressed">Uncompressed file size</param>
        /// <param name="file_size_compressed">Compressed file size</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestFileTransfer_2013")]
        public static extern uds_status SvcRequestFileTransfer_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            [MarshalAs(UnmanagedType.LPStr, SizeParamIndex = 4)]
            string file_path_and_name,
            byte compression_method,
            byte encrypting_method,
            byte file_size_parameter_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            byte[] file_size_uncompressed,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
            byte[] file_size_compressed);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestFileTransfer_2013")]
        private static extern uds_status SvcRequestFileTransfer_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] file_path_and_name,
            byte compression_method,
            byte encrypting_method,
            byte file_size_parameter_size,
            IntPtr file_size_uncompressed,
            IntPtr file_size_compressed);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestFileTransfer_2013")]
        private static extern uds_status SvcRequestFileTransfer_2013(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.U1)]
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            [MarshalAs(UnmanagedType.LPStr, SizeParamIndex = 4)]
            string file_path_and_name,
            byte compression_method,
            byte encrypting_method,
            byte file_size_parameter_size,
            IntPtr file_size_uncompressed,
            IntPtr file_size_compressed);

        /// <summary>
        ///  RequestFileTransfer service.
        ///  Use with mode of operation which does not require file compressed or uncompressed sizes
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="mode_of_operation">Mode of operation (add, delete, replace, read), see PUDS_SVC_PARAM_RFT_MOOP_* values</param>
        /// <param name="file_path_and_name_size">Size in bytes of file_path_and_name buffer</param>
        /// <param name="file_path_and_name">File path and name string</param>
        /// <param name="compression_method">A nibble-value that specifies the "compressionMethod", the value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="encrypting_method">A nibble-value that specifies the "encryptingMethod", the value 0x0 specifies that no encryptingMethod is used.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcRequestFileTransfer_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            byte[] file_path_and_name,
            byte compression_method,
            byte encrypting_method)
        {
            return SvcRequestFileTransfer_2013(channel, request_config, out out_msg_request, mode_of_operation, file_path_and_name_size,
                file_path_and_name,
                compression_method, encrypting_method, 0, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        ///  RequestFileTransfer service.
        ///  Use with mode of operation which does not require file compressed or uncompressed sizes
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="mode_of_operation">Mode of operation (add, delete, replace, read), see PUDS_SVC_PARAM_RFT_MOOP_* values</param>
        /// <param name="file_path_and_name_size">Size in bytes of file_path_and_name buffer</param>
        /// <param name="file_path_and_name">File path and name string</param>
        /// <param name="compression_method">A nibble-value that specifies the "compressionMethod", the value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="encrypting_method">A nibble-value that specifies the "encryptingMethod", the value 0x0 specifies that no encryptingMethod is used.</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcRequestFileTransfer_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            string file_path_and_name,
            byte compression_method,
            byte encrypting_method)
        {
            return SvcRequestFileTransfer_2013(channel, request_config, out out_msg_request, mode_of_operation, file_path_and_name_size,
                file_path_and_name,
                compression_method, encrypting_method, 0, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        ///  RequestFileTransfer service.
        ///  Use with mode of operation which does not require file compressed or uncompressed sizes, nor compression or encrypting methods
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="mode_of_operation">Mode of operation (add, delete, replace, read), see PUDS_SVC_PARAM_RFT_MOOP_* values</param>
        /// <param name="file_path_and_name_size">Size in bytes of file_path_and_name buffer</param>
        /// <param name="file_path_and_name">File path and name string</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcRequestFileTransfer_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            byte[] file_path_and_name)
        {
            return SvcRequestFileTransfer_2013(channel, request_config, out out_msg_request, mode_of_operation, file_path_and_name_size,
                file_path_and_name,
                0, 0, 0, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        ///  RequestFileTransfer service.
        ///  Use with mode of operation which does not require file compressed or uncompressed sizes, nor compression or encrypting methods
        /// </summary>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration (PUDS_MSGTYPE_FLAG_NO_POSITIVE_RESPONSE is ignored)</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="mode_of_operation">Mode of operation (add, delete, replace, read), see PUDS_SVC_PARAM_RFT_MOOP_* values</param>
        /// <param name="file_path_and_name_size">Size in bytes of file_path_and_name buffer</param>
        /// <param name="file_path_and_name">File path and name string</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcRequestFileTransfer_2013(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            uds_svc_param_rft_moop mode_of_operation,
            UInt16 file_path_and_name_size,
            string file_path_and_name)
        {
            return SvcRequestFileTransfer_2013(channel, request_config, out out_msg_request, mode_of_operation, file_path_and_name_size,
                file_path_and_name,
                0, 0, 0, IntPtr.Zero, IntPtr.Zero);
        }
        #endregion
        #region UDS Service: Authentication

        // Represents the subfunction parameter for UDS service Authentication (see ISO 14229-1:2020 §10.6.5.2 Table 74 Request message SubFunction parameter definition p.76)
        public enum uds_svc_authentication_subfunction : Byte
        {
            /// <summary>
            /// DeAuthenticate
            /// </summary>
            PUDS_SVC_PARAM_AT_DA = 0x00,
            /// <summary>
            /// VerifyCertificateUnidirectional
            /// </summary>
            PUDS_SVC_PARAM_AT_VCU = 0x01,
            /// <summary>
            /// VerifyCertificateBidirectional
            /// </summary>
            PUDS_SVC_PARAM_AT_VCB = 0x02,
            /// <summary>
            /// ProofOfOwnership
            /// </summary>
            PUDS_SVC_PARAM_AT_POWN = 0x03,
            /// <summary>
            /// TransmitCertificate
            /// </summary>
            PUDS_SVC_PARAM_AT_TC = 0x04,
            /// <summary>
            /// RequestChallengeForAuthentication
            /// </summary>
            PUDS_SVC_PARAM_AT_RCFA = 0x05,
            /// <summary>
            /// VerifyProofOfOwnershipUnidirectional
            /// </summary>
            PUDS_SVC_PARAM_AT_VPOWNU = 0x06,
            /// <summary>
            /// VerifyProofOfOwnershipBidirectional
            /// </summary>
            PUDS_SVC_PARAM_AT_VPOWNB = 0x07,
            /// <summary>
            /// AuthenticationConfiguration
            /// </summary>
            PUDS_SVC_PARAM_AT_AC = 0x08
        }

        // Represents the return parameter for UDS service Authentication (see ISO 14229-1:2020 §B.5 AuthenticationReturnParameter definitions p.403)
        public enum uds_svc_authentication_return_parameter : Byte
        {
            /// <summary>
            /// Request Accepted
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_RA = 0x00,
            /// <summary>
            /// General Reject
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_GR = 0x01,
            /// <summary>
            /// Authentication Configuration APCE
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_ACAPCE = 0x02,
            /// <summary>
            /// Authentication Configuration ACR with Asymmetric Cryptography
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_ACACRAC = 0x03,
            /// <summary>
            /// Authentication Configuration ACR with Symmetric Cryptography
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_ACACRSC = 0x04,
            /// <summary>
            /// DeAuthentication Successful
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_DAS = 0x10,
            /// <summary>
            /// Certificate Verified, Ownership Verification Necessary
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_CVOVN = 0x11,
            /// <summary>
            /// Ownership Verified, Authentication Complete
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_OVAC = 0x12,
            /// <summary>
            /// Certificate Verified
            /// </summary>
            PUDS_SVC_PARAM_AT_RV_CV = 0x13
        }

        /// <summary>
        ///  Sends Authentication service request with deAuthenticate subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationDA_2020")]
        public static extern uds_status SvcAuthenticationDA_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request);

        /// <summary>
        ///  Sends Authentication service request with verifyCertificateUnidirectional subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="communication_configuration">Configuration information about communication</param>
        /// <param name="certificate_client">Buffer containing the certificate of the client</param>
        /// <param name="certificate_client_size">Size in bytes of the certificate buffer</param>
        /// <param name="challenge_client">Buffer containing the challenge of the client</param>
        /// <param name="challenge_client_size">Size in bytes of the challenge buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVCU_2020")]
        public static extern uds_status SvcAuthenticationVCU_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte communication_configuration,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] certificate_client,
            UInt16 certificate_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            byte[] challenge_client,
            UInt16 challenge_client_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVCU_2020")]
        private static extern uds_status SvcAuthenticationVCU_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte communication_configuration,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] certificate_client,
            UInt16 certificate_client_size,
            IntPtr challenge_client,
            UInt16 challenge_client_size);

        /// <summary>
        ///  Sends Authentication service request with verifyCertificateUnidirectional subfunction,
        ///  without challenge buffer.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="communication_configuration">Configuration information about communication</param>
        /// <param name="certificate_client">Buffer containing the certificate of the client</param>
        /// <param name="certificate_client_size">Size in bytes of the certificate buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcAuthenticationVCU_2020(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte communication_configuration,
            byte[] certificate_client,
            UInt16 certificate_client_size)
        {
            return SvcAuthenticationVCU_2020(channel, request_config, out out_msg_request,
                communication_configuration, certificate_client, certificate_client_size, IntPtr.Zero, 0);
        }

        /// <summary>
        ///  Sends Authentication service request with verifyCertificateBidirectional subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="communication_configuration">Configuration information about communication</param>
        /// <param name="certificate_client">Buffer containing the certificate of the client</param>
        /// <param name="certificate_client_size">Size in bytes of the certificate buffer</param>
        /// <param name="challenge_client">Buffer containing the challenge of the client</param>
        /// <param name="challenge_client_size">Size in bytes of the challenge buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVCB_2020")]
        public static extern uds_status SvcAuthenticationVCB_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte communication_configuration,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] certificate_client,
            UInt16 certificate_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            byte[] challenge_client,
            UInt16 challenge_client_size);

        /// <summary>
        ///  Sends Authentication service request with proofOfOwnership subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="proof_of_ownership_client">Buffer containing the proof of ownership of the client</param>
        /// <param name="proof_of_ownership_client_size">Size in bytes of the proof of ownership buffer</param>
        /// <param name="ephemeral_public_key_client">Buffer containing the ephemeral public key of the client</param>
        /// <param name="ephemeral_public_key_client_size">Size in bytes of the ephemeral public key buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationPOWN_2020")]
        public static extern uds_status SvcAuthenticationPOWN_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)]
            byte[] ephemeral_public_key_client,
            UInt16 ephemeral_public_key_client_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationPOWN_2020")]
        private static extern uds_status SvcAuthenticationPOWN_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            IntPtr ephemeral_public_key_client,
            UInt16 ephemeral_public_key_client_size);

        /// <summary>
        ///  Sends Authentication service request with proofOfOwnership subfunction,
        ///  without ephemeral public key.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="proof_of_ownership_client">Buffer containing the proof of ownership of the client</param>
        /// <param name="proof_of_ownership_client_size">Size in bytes of the proof of ownership buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcAuthenticationPOWN_2020(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size)
        {
            return SvcAuthenticationPOWN_2020(channel, request_config, out out_msg_request,
                proof_of_ownership_client, proof_of_ownership_client_size, IntPtr.Zero, 0);
        }

        /// <summary>
        ///  Sends Authentication service request with requestChallengeForAuthentication subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="communication_configuration">Configuration information about communication</param>
        /// <param name="algorithm_indicator">Buffer of 16 bytes containing the algorithm indicator</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationRCFA_2020")]
        public static extern uds_status SvcAuthenticationRCFA_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            Byte communication_configuration,
            byte[] algorithm_indicator);

        /// <summary>
        ///  Sends Authentication service request with verifyProofOfOwnershipUnidirectional subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="algorithm_indicator">Buffer of 16 bytes containing the algorithm indicator</param>
        /// <param name="proof_of_ownership_client">Buffer containing the proof of ownership of the client</param>
        /// <param name="proof_of_ownership_client_size">Size in bytes of the proof of ownership buffer</param>
        /// <param name="challenge_client">Buffer containing the challenge of the client</param>
        /// <param name="challenge_client_size">Size in bytes of the challenge buffer</param>
        /// <param name="additional_parameter">Buffer containing additional parameters</param>
        /// <param name="additional_parameter_size">Size in bytes of the additional parameter buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVPOWNU_2020")]
        public static extern uds_status SvcAuthenticationVPOWNU_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] algorithm_indicator,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            byte[] challenge_client,
            UInt16 challenge_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 9)]
            byte[] additional_parameter,
            UInt16 additional_parameter_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVPOWNU_2020")]
        private static extern uds_status SvcAuthenticationVPOWNU_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] algorithm_indicator,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            IntPtr challenge_client,
            UInt16 challenge_client_size,
            IntPtr additional_parameter,
            UInt16 additional_parameter_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVPOWNU_2020")]
        private static extern uds_status SvcAuthenticationVPOWNU_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] algorithm_indicator,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            byte[] challenge_client,
            UInt16 challenge_client_size,
            IntPtr additional_parameter,
            UInt16 additional_parameter_size);

        /// <summary>
        ///  Sends Authentication service request with verifyProofOfOwnershipUnidirectional subfunction,
        ///  without additional parameter
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="algorithm_indicator">Buffer of 16 bytes containing the algorithm indicator</param>
        /// <param name="proof_of_ownership_client">Buffer containing the proof of ownership of the client</param>
        /// <param name="proof_of_ownership_client_size">Size in bytes of the proof of ownership buffer</param>
        /// <param name="challenge_client">Buffer containing the challenge of the client</param>
        /// <param name="challenge_client_size">Size in bytes of the challenge buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcAuthenticationVPOWNU_2020(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte[] algorithm_indicator,
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            byte[] challenge_client,
            UInt16 challenge_client_size)
        {
            return SvcAuthenticationVPOWNU_2020(channel, request_config, out out_msg_request,
                algorithm_indicator, proof_of_ownership_client, proof_of_ownership_client_size,
                challenge_client, challenge_client_size, IntPtr.Zero, 0);
        }

        /// <summary>
        ///  Sends Authentication service request with verifyProofOfOwnershipUnidirectional subfunction,
        ///  without additional parameter, without challenge buffer
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="algorithm_indicator">Buffer of 16 bytes containing the algorithm indicator</param>
        /// <param name="proof_of_ownership_client">Buffer containing the proof of ownership of the client</param>
        /// <param name="proof_of_ownership_client_size">Size in bytes of the proof of ownership buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcAuthenticationVPOWNU_2020(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte[] algorithm_indicator,
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size)
        {
            return SvcAuthenticationVPOWNU_2020(channel, request_config, out out_msg_request, algorithm_indicator,
                proof_of_ownership_client, proof_of_ownership_client_size, IntPtr.Zero, 0, IntPtr.Zero, 0);
        }

        /// <summary>
        ///  Sends Authentication service request with verifyProofOfOwnershipBidirectional subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="algorithm_indicator">Buffer of 16 bytes containing the algorithm indicator</param>
        /// <param name="proof_of_ownership_client">Buffer containing the proof of ownership of the client</param>
        /// <param name="proof_of_ownership_client_size">Size in bytes of the proof of ownership buffer</param>
        /// <param name="challenge_client">Buffer containing the challenge of the client</param>
        /// <param name="challenge_client_size">Size in bytes of the challenge buffer</param>
        /// <param name="additional_parameter">Buffer containing additional parameters</param>
        /// <param name="additional_parameter_size">Size in bytes of the additional parameter buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVPOWNB_2020")]
        public static extern uds_status SvcAuthenticationVPOWNB_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] algorithm_indicator,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            byte[] challenge_client,
            UInt16 challenge_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 9)]
            byte[] additional_parameter,
            UInt16 additional_parameter_size);

        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationVPOWNB_2020")]
        private static extern uds_status SvcAuthenticationVPOWNB_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] algorithm_indicator,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
            byte[] challenge_client,
            UInt16 challenge_client_size,
            IntPtr additional_parameter,
            UInt16 additional_parameter_size);

        /// <summary>
        ///  Sends Authentication service request with verifyProofOfOwnershipBidirectional subfunction,
        ///  without additional parameters
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <param name="algorithm_indicator">Buffer of 16 bytes containing the algorithm indicator</param>
        /// <param name="proof_of_ownership_client">Buffer containing the proof of ownership of the client</param>
        /// <param name="proof_of_ownership_client_size">Size in bytes of the proof of ownership buffer</param>
        /// <param name="challenge_client">Buffer containing the challenge of the client</param>
        /// <param name="challenge_client_size">Size in bytes of the challenge buffer</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        public static uds_status SvcAuthenticationVPOWNB_2020(
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request,
            byte[] algorithm_indicator,
            byte[] proof_of_ownership_client,
            UInt16 proof_of_ownership_client_size,
            byte[] challenge_client,
            UInt16 challenge_client_size)
        {
            return SvcAuthenticationVPOWNB_2020(channel, request_config, out out_msg_request, algorithm_indicator,
                proof_of_ownership_client, proof_of_ownership_client_size, challenge_client, challenge_client_size,
                IntPtr.Zero, 0);
        }

        /// <summary>
        ///  Sends Authentication service request with authenticationConfiguration subfunction.
        /// </summary>
        /// <remarks>
        ///  API provides uds_svc_authentication_subfunction and uds_svc_authentication_return_parameter
        ///  enumerations to help user to decode Authentication service responses.
        /// </remarks>
        /// <param name="channel">A PCANTP channel handle representing a PUDS channel</param>
        /// <param name="request_config">Request configuration</param>
        /// <param name="out_msg_request">(out) request message created and sent by the function</param>
        /// <returns>A uds_status code. PUDS_STATUS_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcAuthenticationAC_2020")]
        public static extern uds_status SvcAuthenticationAC_2020(
            [MarshalAs(UnmanagedType.U4)]
            cantp_handle channel,
            uds_msgconfig request_config,
            out uds_msg out_msg_request);
        #endregion
        #endregion

        #region special C# functions, examples of how to use the structures IntPtr fields  in safe mode, with Marshaling operations
        /// <summary>
        /// Get PUDS message data service id, in safe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">service id value</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool GetDataServiceId_2013(ref uds_msg msg, out Byte val)
        {
            if (msg.links.service_id == IntPtr.Zero)
            {
                val = 0;
                return false;
            }

            val = Marshal.ReadByte(msg.links.service_id);
            return true;
        }
        /// <summary>
        /// Set PUDS message data service id, in safe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">service id value</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool SetDataServiceId_2013(ref uds_msg msg, Byte val)
        {
            if (msg.links.service_id == IntPtr.Zero)
            {
                return false;
            }

            Marshal.WriteByte(msg.links.service_id, val);
            return true;
        }
        /// <summary>
        /// Get PUDS message data negative response code (nrc), in safe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">negative response code value</param>
        /// <returns>true if ok, false if not ok (no nrc available, which means positive response)</returns>
        public static bool GetDataNrc_2013(ref uds_msg msg, out Byte val)
        {
            if (msg.links.nrc == IntPtr.Zero)
            {
                val = 0;
                return false;
            }

            val = Marshal.ReadByte(msg.links.nrc);
            return true;
        }
        /// <summary>
        /// Set PUDS message data negative response code (nrc), in safe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">negative response code value</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool SetDataNrc_2013(ref uds_msg msg, Byte val)
        {
            if (msg.links.nrc == IntPtr.Zero)
            {
                return false;
            }

            Marshal.WriteByte(msg.links.nrc, val);
            return true;
        }
        /// <summary>
        /// Get PUDS message data parameter, in safe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="nump">parameter number (from 0)</param>
        /// <param name="val">parameter value</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool GetDataParameter_2013(ref uds_msg msg, int nump, out Byte val)
        {
            if (msg.links.param == IntPtr.Zero)
            {
                val = 0;
                return false;
            }

            val = Marshal.ReadByte(msg.links.param, nump);
            return true;
        }
        /// <summary>
        /// Set PUDS message data parameter, in safe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="nump">parameter number (from 0)</param>
        /// <param name="val">parameter value</param>
        /// <returns>true if ok, false if not ok</returns>
        public static bool SetDataParameter_2013(ref uds_msg msg, int nump, Byte val)
        {
            if (msg.links.param == IntPtr.Zero)
            {
                return false;
            }

            Marshal.WriteByte(msg.links.param, nump, val);
            return true;
        }

        #endregion
        #region special C# functions, examples of how to use the structures IntPtr fields  in unsafe mode
#if (UNSAFE)
        /// <summary>
        /// Get PUDS message data service id, in unsafe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">service id value</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool GetDataServiceId_unsafe_2013(ref uds_msg msg, out Byte val)
        {
            if (msg.links.service_id == IntPtr.Zero)
            {
                val = 0;
                return false;
            }
            Byte* pt = (Byte*)msg.links.service_id.ToPointer();
            val = *pt;
            return true;
        }
        /// <summary>
        /// Set PUDS message data service id, in unsafe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">service id value</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool SetDataServiceId_unsafe_2013(ref uds_msg msg, Byte val)
        {
            if (msg.links.service_id == IntPtr.Zero)
            {
                return false;
            }

            Byte* pt = (Byte*)msg.links.service_id.ToPointer();
            *pt = val;
            return true;
        }
        /// <summary>
        /// Get PUDS message data negative response code (nrc), in unsafe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">negative response code value</param>
        /// <returns>true if ok, false if not ok (no nrc available, which means positive response)</returns>
        public unsafe static bool GetDataNrc_unsafe_2013(ref uds_msg msg, out Byte val)
        {
            if (msg.links.nrc == IntPtr.Zero)
            {
                val = 0;
                return false;
            }

            Byte* pt = (Byte*)msg.links.nrc.ToPointer();
            val = *pt;
            return true;
        }
        /// <summary>
        /// Set PUDS message data negative response code (nrc), in unsafe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="val">negative response code value</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool SetDataNrc_unsafe_2013(ref uds_msg msg, Byte val)
        {
            if (msg.links.nrc == IntPtr.Zero)
            {
                return false;
            }

            Byte* pt = (Byte*)msg.links.nrc.ToPointer();
            *pt = val;
            return true;
        }
        /// <summary>
        /// Get PUDS message data parameter, in unsafe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="nump">parameter number (from 0)</param>
        /// <param name="val">parameter value</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool GetDataParameter_unsafe_2013(ref uds_msg msg, int nump, out Byte val)
        {
            if (msg.links.param == IntPtr.Zero)
            {
                val = 0;
                return false;
            }

            Byte* pt = (Byte*)msg.links.param.ToPointer();
            val = pt[nump];
            return true;
        }
        /// <summary>
        /// Set PUDS message data parameter, in unsafe mode
        /// </summary>
        /// <param name="msg">PUDS message</param>
        /// <param name="nump">parameter number (from 0)</param>
        /// <param name="val">parameter value</param>
        /// <returns>true if ok, false if not ok</returns>
        public unsafe static bool SetDataParameter_unsafe_2013(ref uds_msg msg, int nump, Byte val)
        {
            if (msg.links.param == IntPtr.Zero)
            {
                return false;
            }

            Byte* pt = (Byte*)msg.links.param.ToPointer();
            pt[nump] = val;
            return true;
        }
#endif
        #endregion

        #endregion

    }

    #endregion
}