// <copyright file="Measurement.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using MessagePack;

    /// <summary>
    /// Class representing a MEASUREMENT element of an A2L-file. <br/>
    /// <br/>
    /// Since these elements share a lot of attributes with the CHARACTERISTIC elements, both
    /// <see cref="Measurement"/> and <see cref="Characteristic"/> inherit from the
    /// <see cref="XcpValue"/> class.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class Measurement : XcpValue
    {
        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets or sets the name of the MEASUREMENT element.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// Gets or sets the long identifier of the MEASUREMENT element.
        /// </summary>
        public override string LongIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the data type of the MEASUREMENT.
        /// </summary>
        public override DataTypes DataType { get; set; }

        /// <summary>
        /// Gets or sets the name of the COMPU_METHOD used to convert the data to a physical value. <br/>
        /// "NO_COMPU_METHOD" means that a conversion is not required.
        /// </summary>
        public override string Conversion { get; set; }

        /// <summary>
        /// Gets or sets the corresponding COMPU_METHOD.
        /// </summary>
        public override CompuMethod CompuMethod { get; set; }

        /// <summary>
        /// Gets or sets the smallest possible change in bits. (0 to 255).
        /// </summary>
        public long Resolution { get; set; }

        /// <summary>
        /// Gets or sets the possible variation from exact value in %.
        /// </summary>
        public double Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the lower limit for the value of the MEASUREMENT.
        /// </summary>
        public override double LowerLimit { get; set; }

        /// <summary>
        /// Gets or sets the upper limit for the value of the MEASUREMENT.
        /// </summary>
        public override double UpperLimit { get; set; }

        /// <summary>
        /// Gets or sets the dimensions of multidimensional arrays.
        /// </summary>
        public override ulong[] MatrixDim { get; set; } = new ulong[3] { 0, 0, 0 };

        /// <summary>
        /// Gets or sets the bit mask to extract the MEASUREMENT from the object's value stored in the <see cref="EcuAddress"/>.
        /// </summary>
        public override ulong BitMask { get; set; }

        /// <summary>
        /// Gets or sets the address of the MEASUREMENT in ECU memory.
        /// </summary>
        public override uint EcuAddress { get; set; }

        /// <summary>
        /// Gets or sets additional address information, for instance to distinguish between different address spaces of an ECU.
        /// </summary>
        public override byte EcuAddressExtension { get; set; }

        /// <summary>
        /// Gets or sets the display format of object values in C-printf notation.
        /// Overrules the format parameter in referenced <see cref="CompuMethod"/>.
        /// </summary>
        public string Format { get; set; }
    }
}
