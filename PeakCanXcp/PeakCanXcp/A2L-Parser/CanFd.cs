// <copyright file="CanFd.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using MessagePack;

    /// <summary>
    /// Class representing the CAN-FD (Flexible Data-Rate) parameters wich can found as a nested element in the
    /// XCP_ON_CAN element of an A2L-file.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class CanFd
    {
        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets or sets the maximum Message Size: 8, 12, 16, 20, 24, 32, 48 or 64.
        /// </summary>
        public ushort MaxDlc { get; set; }

        /// <summary>
        /// Gets or sets the FD Baudrate in Hz.
        /// </summary>
        public uint CanFdDataTransferBaudrate { get; set; }

        /// <summary>
        /// Gets or sets the sample-point in %.
        /// </summary>
        public byte SamplePoint { get; set; }

        /// <summary>
        /// Gets or sets the number of BTL Cycles (slots per bit time).
        /// </summary>
        public byte BtlCycles { get; set; }

        /// <summary>
        /// Gets or sets the Synchronization Jump Width (SJW).
        /// </summary>
        public byte Sjw { get; set; }

        /// <summary>
        /// Gets or sets the secondary sample-point in %.
        /// </summary>
        public byte SecondarySamplePoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether transceiver delay compensation is turned on.
        /// </summary>
        public bool TransceiverDelayCompensation { get; set; }
    }
}
