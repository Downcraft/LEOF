// <copyright file="CompuMethod.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using MessagePack;

    /// <summary>
    /// Class representing a COMPU_METHOD element of an A2L-file.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class CompuMethod
    {
        /* =========================================================================================
        // ENUMS
        // ====================================================================================== */

        /// <summary>
        /// Represents the possible conversion types for COMPU_METHOD elements.
        /// </summary>
        public enum ConversionType
        {
            /// <summary>
            /// No conversion.
            /// </summary>
            IDENTICAL,

            /// <summary>
            /// Formula which consists of a specific set of perators and functions.
            /// </summary>
            FORM,

            /// <summary>
            /// Linear, 2-coefficient function with slope and offset.
            /// </summary>
            LINEAR,

            /// <summary>
            /// 6-coefficient rational function with 2nd-degree numerator and denominator polynomials.
            /// </summary>
            RAT_FUNC,

            /// <summary>
            /// Table with interpolation.
            /// </summary>
            TAB_INTP,

            /// <summary>
            /// Table without interpolation.
            /// </summary>
            TAB_NOINTP,

            /// <summary>
            /// Verbal table (i.e. enumeration).
            /// </summary>
            TAB_VERB,
        }

        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets or sets the name of the COMPU_METHOD element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the long identifier of the COMPU_METHOD element.
        /// </summary>
        public string LongIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the conversion type of the COMPU_METHOD element.
        /// </summary>
        public ConversionType Type { get; set; }

        /// <summary>
        /// Gets or sets the display format of object values in C-printf notation.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the physical unit of the converted value.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the coefficients for the rational function (<see cref="ConversionType.RAT_FUNC"/>).
        /// </summary>
        public double[] Coeffs { get; set; } = new double[6];

        /// <summary>
        /// Gets or sets coefficients for the linear function (<see cref="ConversionType.LINEAR"/>).
        /// </summary>
        public double[] CoeffsLinear { get; set; } = new double[2];

        /// <summary>
        /// Gets or sets the reference to a conversion table.
        /// </summary>
        public string CompuTabRef { get; set; }
    }
}
