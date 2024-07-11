// <copyright file="XcpValue.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using MessagePack;

    /// <summary>
    /// The base class for both <see cref="Measurement"/> and <see cref="Characteristic"/>.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public abstract class XcpValue
    {
        /* =========================================================================================
        // ENUMS
        // ====================================================================================== */

        /// <summary>
        /// Represents the possible data types for MEASUREMENT and CHARACTERISTIC elements.
        /// </summary>
        public enum DataTypes
        {
            /// <summary>
            /// 8-bit unsigned integer.
            /// </summary>
            UBYTE,

            /// <summary>
            /// 8-bit signed two's complement integer.
            /// </summary>
            SBYTE,

            /// <summary>
            /// 16-bit unsigned integer.
            /// </summary>
            UWORD,

            /// <summary>
            /// 16-bit signed two's complement integer.
            /// </summary>
            SWORD,

            /// <summary>
            /// 32-bit unsigned integer.
            /// </summary>
            ULONG,

            /// <summary>
            /// 32-bit signed two's complement integer.
            /// </summary>
            SLONG,

            /// <summary>
            /// 64-bit unsigned integer.
            /// </summary>
            A_UINT64,

            /// <summary>
            /// 64-bit signed two's complement integer.
            /// </summary>
            A_INT64,

            /// <summary>
            /// 32-bit signed IEEE 754 single precision floating point number.
            /// </summary>
            FLOAT32_IEEE,

            /// <summary>
            /// 64-bit signed IEEE 754 double precision floating point number.
            /// </summary>
            FLOAT64_IEEE,

            /// <summary>
            /// 8-bit unsigned integer.
            /// </summary>
            STANDARD_VALUE_U8,

            /// <summary>
            /// 8-bit signed two's complement integer.
            /// </summary>
            STANDARD_VALUE_S8,

            /// <summary>
            /// 16-bit unsigned integer.
            /// </summary>
            STANDARD_VALUE_U16,

            /// <summary>
            /// 16-bit signed two's complement integer.
            /// </summary>
            STANDARD_VALUE_S16,

            /// <summary>
            /// Unknown type.
            /// </summary>
            UNKNOWN,
        }

        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Gets or sets the long identifier of the object.
        /// </summary>
        public abstract string LongIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the data type of the object.
        /// </summary>
        public abstract DataTypes DataType { get; set; }

        /// <summary>
        /// Gets or sets the name of the COMPU_METHOD used to convert the data to a physical value. <br/>
        /// "NO_COMPU_METHOD" means that a conversion is not required.
        /// </summary>
        public abstract string Conversion { get; set; }

        /// <summary>
        /// Gets or sets the corresponding COMPU_METHOD.
        /// </summary>
        public abstract CompuMethod CompuMethod { get; set; }

        /// <summary>
        /// Gets or sets the lower limit for the value of the object.
        /// </summary>
        public abstract double LowerLimit { get; set; }

        /// <summary>
        /// Gets or sets the upper limit for the value of the object.
        /// </summary>
        public abstract double UpperLimit { get; set; }

        /// <summary>
        /// Gets or sets the address of the object in ECU memory.
        /// </summary>
        public abstract uint EcuAddress { get; set; }

        /// <summary>
        /// Gets or sets additional address information, for instance to distinguish between different address spaces of an ECU.
        /// </summary>
        public abstract byte EcuAddressExtension { get; set; }

        /// <summary>
        /// Gets or sets the dimensions of multidimensional arrays.
        /// </summary>
        public abstract ulong[] MatrixDim { get; set; }

        /// <summary>
        /// Gets or sets the bit mask to extract single bits from the object's value stored in the <see cref="EcuAddress"/>.
        /// </summary>
        public abstract ulong BitMask { get; set; }

        /// <summary>
        /// Gets the byte size of the value based on the data type (<see cref="DataType"/>).
        /// </summary>
        public uint ByteSize
        {
            get
            {
                uint byteSize = 0;

                switch (this.DataType)
                {
                    case DataTypes.A_INT64:
                    case DataTypes.A_UINT64:
                    case DataTypes.FLOAT64_IEEE:
                        byteSize = 8;
                        break;
                    case DataTypes.SLONG:
                    case DataTypes.ULONG:
                    case DataTypes.FLOAT32_IEEE:
                        byteSize = 4;
                        break;
                    case DataTypes.SWORD:
                    case DataTypes.UWORD:
                        byteSize = 2;
                        break;
                    case DataTypes.SBYTE:
                    case DataTypes.UBYTE:
                        byteSize = 1;
                        break;
                }

                return byteSize;
            }
        }
    }
}
