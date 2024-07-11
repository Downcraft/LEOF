// <copyright file="CanParameters.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using MessagePack;

    /// <summary>
    /// Class representing the XCP_ON_CAN element of an A2L-file.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class CanParameters
    {
        /* =========================================================================================
        // ENUMS
        // ====================================================================================== */

        /// <summary>
        /// Represents the possible sampling modes.
        /// </summary>
        public enum SampleRates
        {
            /// <summary>
            /// Single sampling mode.
            /// </summary>
            Single = 1,

            /// <summary>
            /// Triple sampling mode.
            /// </summary>
            Triple = 3,
        }

        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets or sets the XCP on CAN version.
        /// </summary>
        public ushort XcpOnCanVersion { get; set; }

        /// <summary>
        /// Gets or sets the CAN-ID for broadcast.
        /// </summary>
        public uint CanIdBroadcast { get; set; }

        /// <summary>
        /// Gets or sets the CAN-ID for XCP-Master.
        /// </summary>
        public uint CanIdMaster { get; set; }

        /// <summary>
        /// Gets or sets the CAN-ID for XCP-Slave.
        /// </summary>
        public uint CanIdSlave { get; set; }

        /// <summary>
        /// Gets or sets the Baudrate in Hz.
        /// </summary>
        public uint Baudrate { get; set; }

        /// <summary>
        /// Gets or sets the sample point in % of bit time.
        /// </summary>
        public byte SamplePoint { get; set; }

        /// <summary>
        /// Gets or sets the sampling mode (sample per bit).
        /// </summary>
        public SampleRates SampleRate { get; set; }

        /// <summary>
        /// Gets or sets the number of BTL Cycles (slots per bit time).
        /// </summary>
        public byte BtlCycles { get; set; }

        /// <summary>
        /// Gets or sets the Synchronization Jump Width (SJW).
        /// </summary>
        public byte Sjw { get; set; }

        /// <summary>
        /// Gets or sets the CAN-FD parameters.
        /// </summary>
        public CanFd CanFdParameters { get; set; } = new CanFd();
    }
}
