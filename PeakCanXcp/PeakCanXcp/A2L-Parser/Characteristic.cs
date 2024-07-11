// <copyright file="Characteristic.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using MessagePack;

    /// <summary>
    /// Class representing a CHARACTERISTIC element of an A2L-file. <br/>
    /// <br/>
    /// Since these elements share a lot of attributes with the MEASUREMENT elements, both
    /// <see cref="Characteristic"/> and <see cref="Measurement"/> inherit from the
    /// <see cref="XcpValue"/> class.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class Characteristic : XcpValue
    {
        /* =========================================================================================
        // ENUMS
        // ====================================================================================== */

        /// <summary>
        /// Represents the possible types for CHARACTERISTIC elements.
        /// </summary>
        public enum CharacteristicType
        {
            /// <summary>
            /// String.
            /// </summary>
            ASCII,

            /// <summary>
            /// 1D-Table.
            /// </summary>
            CURVE,

            /// <summary>
            /// 2D-Table.
            /// </summary>
            MAP,

            /// <summary>
            /// 3D-Table.
            /// </summary>
            CUBOID,

            /// <summary>
            /// 4D-Table.
            /// </summary>
            CUBE_4,

            /// <summary>
            /// 5D-Table
            /// </summary>
            CUBE_5,

            /// <summary>
            /// Array (no axes).
            /// </summary>
            VAL_BLK,

            /// <summary>
            /// Scalar.
            /// </summary>
            VALUE,
        }

        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets or sets the name of the CHARACTERISTIC element.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// Gets or sets the long identifier of the CHARACTERISTIC element.
        /// </summary>
        public override string LongIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the type of the CHARACTERISTIC element.
        /// </summary>
        public CharacteristicType Type { get; set; }

        /// <summary>
        /// Gets or sets the address of the CHARACTERISTIC in ECU memory.
        /// </summary>
        public override uint EcuAddress { get; set; }

        /// <summary>
        /// Gets or sets the data type of the CHARACTERISTIC.
        /// </summary>
        public override DataTypes DataType { get; set; }

        /// <summary>
        /// Gets or sets the maximum adjustment allowed for the CHARACTERISTIC. <br/>
        /// Entered as an absolute value, not as a percentage.
        /// </summary>
        public double MaxDiff { get; set; }

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
        /// Gets or sets the lower limit for the value of the CHARACTERISTIC.
        /// </summary>
        public override double LowerLimit { get; set; }

        /// <summary>
        /// Gets or sets the upper limit for the value of the CHARACTERISTIC.
        /// </summary>
        public override double UpperLimit { get; set; }

        /// <summary>
        /// Gets or sets the bit mask to extract the CHARACTERISTIC from the object's value stored in the <see cref="EcuAddress"/>.
        /// </summary>
        public override ulong BitMask { get; set; }

        /// <summary>
        /// Gets or sets additional address information, for instance to distinguish between different address spaces of an ECU.
        /// </summary>
        public override byte EcuAddressExtension { get; set; }

        /// <summary>
        /// Gets or sets an extended upper and lower value beyond the normal upper and lower limit values. <br/>
        /// <br/>
        /// Can be used to distinguish between out-of-range warnings and out-of-range error messages,
        /// or to allow specific power-users to set calibration values beyond a safe margin.
        /// </summary>
        public ExtendedLimits ExtLimits { get; set; } = new ExtendedLimits();

        /// <summary>
        /// Gets or sets the display format of object values in C-printf notation.
        /// Overrules the format parameter in referenced <see cref="CompuMethod"/>.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the dimensions of multidimensional arrays.
        /// </summary>
        public override ulong[] MatrixDim { get; set; } = new ulong[3] { 0, 0, 0 };

        /// <summary>
        /// Gets or sets the number of elements (ASCII characters or values) in a 1D array.
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CHARACTERISTIC is read-only.
        /// </summary>
        public bool ReadOnly { get; set; } = false;

        /* =========================================================================================
        // CLASSES
        // ====================================================================================== */

        /// <summary>
        /// Class representing extended upper and lower value beyond the normal upper and lower limit values.
        /// </summary>
        [MessagePackObject(keyAsPropertyName: true)]
        public class ExtendedLimits
        {
            /* ====================================================================================
            // PROPERTIES
            // ================================================================================= */

            /// <summary>
            /// Gets or sets the extended lower limit.
            /// </summary>
            public double Lower { get; set; }

            /// <summary>
            /// Gets or sets the extended upper limit.
            /// </summary>
            public double Upper { get; set; }
        }
    }
}
