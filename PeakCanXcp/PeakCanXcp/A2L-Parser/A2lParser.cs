// <copyright file="A2lParser.cs" company="SPEA GmbH">
// Copyright (c) SPEA GmbH. All rights reserved.
// </copyright>

namespace PeakCanXcp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using MessagePack;

    /// <summary>
    /// Class responsible for parsing and representing A2L files. <br/>
    /// It can serialize the parsed data using the MessagePack protocoll to speed up the initialization process,
    /// since deserializing is much quicker than parsing. <br/> <br/>
    ///
    /// [WARNING] This parser is not feature complete and is not able to parse all keywords / elements of an A2L file. <br/> <br/>
    ///
    /// The following keywords / elements are supported: <br/>
    ///
    /// - XCP_ON_CAN <br/>
    /// |__+ CAN Version <br/>
    /// |__+ CAN ID Master <br/>
    /// |__+ CAN ID Slave <br/>
    /// |__+ Baudrate <br/>
    /// |__+ Sample Point <br/>
    /// |__+ BTL Cycles <br/>
    /// |__+ SJW <br/>
    /// |__+ CAN-FD Parameters <br/>
    /// .....|__+ Maximal Data Length Code <br/>
    /// .....|__+ CAN-FD Data Transfer Baudrate <br/>
    /// .....|__+ Sample Point <br/>
    /// .....|__+ BTL Cycles <br/>
    /// .....|__+ SJW <br/>
    /// .....|__+ Secondary Sample Point <br/>
    /// .....|__+ Transceiver Delay Compensation <br/>
    /// - CHARACTERISTIC <br/>
    /// |__+ Name <br/>
    /// |__+ Long Identifier <br/>
    /// |__+ Type (See supported data types in <see cref="Characteristic.CharacteristicType"/>) <br/>
    /// |__+ ECU Address <br/>
    /// |__+ Data Type (See supported data types in <see cref="Characteristic.DataType"/>) <br/>
    /// |__+ Max Diff <br/>
    /// |__+ Conversion <br/>
    /// |__+ Compu_Method <br/>
    /// |__+ Lower Limit <br/>
    /// |__+ Upper Limit <br/>
    /// |__+ Bit Mask <br/>
    /// |__+ ECU Address Extension <br/>
    /// |__+ Extended Limits <br/>
    /// |__+ Format <br/>
    /// |__+ Matrix_Dim <br/>
    /// |__+ Number <br/>
    /// |__+ Read Only <br/>
    /// - MEASUREMENT <br/>
    /// |__+ Name <br/>
    /// |__+ Long Identifier <br/>
    /// |__+ Data Type (See supported data types in <see cref="Characteristic.DataType"/>) <br/>
    /// |__+ Conversion <br/>
    /// |__+ Compu_Method <br/>
    /// |__+ Resolution <br/>
    /// |__+ Accuracy <br/>
    /// |__+ Lower Limit <br/>
    /// |__+ Upper Limit <br/>
    /// |__+ Matrix_Dim <br/>
    /// |__+ Bit Mask <br/>
    /// |__+ ECU Address <br/>
    /// |__+ ECU Address Extension <br/>
    /// |__+ Format <br/>
    /// - COMPU_METHOD <br/>
    /// |__+ Name <br/>
    /// |__+ Long Identifier <br/>
    /// |__+ Type <br/>
    /// |__+ Format <br/>
    /// |__+ Unit <br/>
    /// |__+ Coeffs <br/>
    /// |__+ Coeffs_Linear <br/>
    /// |__+ Comput_Tab_Ref. <br/>
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class A2lParser
    {
        /* =========================================================================================
        // CONSTRUCTORS
        // ====================================================================================== */

        /// <summary>
        /// Initializes a new instance of the <see cref="A2lParser"/> class by parsing or deserializing the file represented by the path passed as <paramref name="a2lPath"/>. After construction, the object can be used to access A2L elements.<br/><br/>
        /// The decision if the file should be parsed or deserialized is made depending on the file type (<paramref name="fileType"/>). Files of type <see cref="FileType.A2L"/> are parsed, while files of type <see cref="FileType.BIN"/>  are deserialized.<br/>
        /// Finally, a boolean parameter (<paramref name="serialize"/>) indicates if the file should be serialized after parsed. The MessagePack protocol is used for the serialization. <br/>
        /// The serialized file is saved on the same directory with the same name as the original file, but with the extension ".bin". <br/><br/>
        /// If a file with the same name as the passed file and the extension ".bin" doesn't exist in the directory of the file, an A2L file will be assumed.<br/><br/>
        ///
        /// Example:<br/><br/>
        ///
        /// <code>
        /// // This will serialize the file by the first call and by subsequent calls use the serialized file,<br/>
        /// // even though the extension ".a2l" is used in the path.<br/><br/>
        ///
        /// var path = "MyA2l.a2l";<br/>
        /// var a2l = new A2lParser(path);<br/>
        /// </code>
        /// </summary>
        /// <param name="a2lPath">The Path of the A2L file to be parsed or the binary file to be deserialized.</param>
        /// <param name="fileType">The type of the file.</param>
        /// <param name="serialize">Indicates if an A2L file should be serialized after parsing.</param>
        public A2lParser(string a2lPath, FileType fileType = FileType.BIN, bool serialize = true)
        {
            if (!File.Exists(a2lPath))
            {
                throw new ArgumentException($"Can't find the following file: {a2lPath}");
            }

            this.XcpStationId = Path.GetFileNameWithoutExtension(a2lPath);
            var binFilename = this.XcpStationId + ".bin";
            var directory = Path.GetDirectoryName(Path.GetFullPath(a2lPath));

            // Assume A2L if .bin file doesn't exist.
            if (!File.Exists(directory + "\\" + binFilename))
            {
                fileType = FileType.A2L;
            }

            // Parsing / Deserializing.
            switch (fileType)
            {
                // Parsing.
                case FileType.A2L:
                    this.A2l = File.ReadAllText(a2lPath);
                    this.A2l = this.StripComments(this.A2l);
                    this.A2l = this.StripAnnotations(this.A2l);
                    this.GetXcpOnCan(this.A2l);
                    this.GetCompuMethods(this.A2l);
                    this.GetMeasurements(this.A2l);
                    this.GetCharacteristics(this.A2l);
                    break;

                // Deserializing.
                case FileType.BIN:
                    var bytes = File.ReadAllBytes(directory + "\\" + binFilename);
                    var a2lParser = MessagePackSerializer.Deserialize<A2lParser>(bytes, MessagePack.Resolvers.StandardResolverAllowPrivate.Options);
                    this.CanParameters = a2lParser.CanParameters;
                    this.CompuMethods = a2lParser.CompuMethods;
                    this.Measurements = a2lParser.Measurements;
                    this.Characteristics = a2lParser.Characteristics;
                    break;
            }

            // Serializing.
            if (serialize && (fileType == FileType.A2L))
            {
                var bytes = MessagePackSerializer.Serialize(this);
                File.WriteAllBytes(directory + "\\" + binFilename, bytes);
            }
        }

#pragma warning disable IDE0051 // Remove unused private members

        /// <summary>
        /// Initializes a new instance of the <see cref="A2lParser"/> class by passing the necessary data explicitely. <br/><br/>
        ///
        /// [WARNING] This constructor is used to perform deserialization only, hence it being private.
        ///
        /// </summary>
        /// <param name="canParameters">The CAN Parameters (marked by the XCP_ON_CAN keyword).</param>
        /// <param name="measurements">A dictionary containing all MEASUREMENT elements as values with their names used as keys.</param>
        /// <param name="compuMethods">A dictionary containing all COMPU_METHOD elements as values with their names used as keys.</param>
        /// <param name="characteristics">A dictionary containing all CHARACTERISTIC elements as values with their names used as keys.</param>
        [SerializationConstructor]
        private A2lParser(CanParameters canParameters, Dictionary<string, Measurement> measurements, Dictionary<string, CompuMethod> compuMethods, Dictionary<string, Characteristic> characteristics)
        {
            this.CanParameters = canParameters;
            this.Measurements = measurements;
            this.CompuMethods = compuMethods;
            this.Characteristics = characteristics;
        }

#pragma warning restore IDE0051 // Remove unused private members

        /* =========================================================================================
        // ENUMS
        // ====================================================================================== */

        /// <summary>
        /// The possible file types tha can be passed to the <see cref="A2lParser(string, FileType, bool)"/> constructor.
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// Represents .a2l files, which are parsed by the parser.
            /// </summary>
            A2L,

            /// <summary>
            /// Represents .bin files, containing the parsed data serialized with the MessagePack protocol.
            /// This files are deserialized by the <see cref="A2lParser(string, FileType, bool)"/> constructor.
            /// </summary>
            BIN,
        }

        /* =========================================================================================
        // PROPERTIES
        // ====================================================================================== */

        /// <summary>
        /// Gets the name of the ASAP2 File.
        /// </summary>
        public string XcpStationId { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets an object representing the CAN parameters as read from the XCP_ON_CAN keyword in the A2L file.
        /// </summary>
        public CanParameters CanParameters { get; set; } = new CanParameters();

        /// <summary>
        /// Gets or sets a Dictionary, which contains the MEASUREMENT elements as read from the A2L file. The key being the name of that specific MEASUREMENT.
        /// </summary>
        public Dictionary<string, Measurement> Measurements { get; set; } = new Dictionary<string, Measurement>();

        /// <summary>
        /// Gets or sets a Dictionary, which contains the COMPU_METHOD elements as read from the A2L file. The key being the name of that specific COMPU_METHOD.
        /// </summary>
        public Dictionary<string, CompuMethod> CompuMethods { get; set; } = new Dictionary<string, CompuMethod>();

        /// <summary>
        /// Gets or sets a Dictionary, which contains the CHARACTERISTIC elements as read from the A2L file. The key being the name of that specific CHARACTERISTIC.
        /// </summary>
        public Dictionary<string, Characteristic> Characteristics { get; set; } = new Dictionary<string, Characteristic>();

        /// <summary>
        /// Gets or sets the content of the A2L file. It is only used for parsing, hence it is not serialized ([IgnoreMember] attribute).
        /// </summary>
        [IgnoreMember]
        private string A2l { get; set; }

        /* =========================================================================================
        // METHODS
        // ====================================================================================== */

        /// <summary>
        /// Removes all comments from the A2L content.
        /// </summary>
        /// <param name="a2l">The content of the A2L file.</param>
        /// <returns>The A2L content without comments.</returns>
        private string StripComments(string a2l)
        {
            // Regular expression for removing all single-line comments.
            a2l = Regex.Replace(a2l, @"//.+", string.Empty);

            // Regular expression for removing all multi-line comments.
            a2l = Regex.Replace(a2l, @"\/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+\/", string.Empty);

            return a2l;
        }

        /// <summary>
        /// Removes all annotations from the A2L content.
        /// </summary>
        /// <param name="a2l">The content of the A2L file.</param>
        /// <returns>The A2L content without comments.</returns>
        private string StripAnnotations(string a2l)
        {
            // Regular expression for removing annotation blocks.
            a2l = Regex.Replace(a2l, @"\/begin ANNOTATION((.|[\r\n]|[\r]|[\n])*?)\/end ANNOTATION([\r\n]|[\r]|[\n])", string.Empty);

            return a2l;
        }

        /// <summary>
        /// Parses the XCP_ON_CAN element of the A2L content string into the <see cref="CanParameters"/> property.
        /// </summary>
        /// <param name="a2l">The content of the A2L file.</param>
        private void GetXcpOnCan(string a2l)
        {
            // Regular expression for extracting the XCP_ON_CAN element.
            var xcpOnCanStr = Regex.Match(a2l, @"\/begin XCP_ON_CAN((.|[\r\n]|[\r]|[\n])*?)\/end XCP_ON_CAN").Groups[0].Value.Trim();

            // Regular expression for extracting the content between the \begin mark of the XCP_ON_CAN element
            // and its \end mark or the \begin mark of a nested element.
            var canStr = Regex.Match(xcpOnCanStr, @"\/begin XCP_ON_CAN(([^/]|[\r\n]|[\r]|[\n])*)").Groups[1].Value.Trim();

            // Regular expression for extracting the content between the \begin mark of the \CAN_FD element
            // and its \end mark or the \begin mark of a nested element.
            var canFdStr = Regex.Match(xcpOnCanStr, @"\/begin CAN_FD(([^/]|[\r\n]|[\r]|[\n])*)").Groups[1].Value.Trim();

            // The contents will be splited at each line break.
            string[] splitString = { "\r\n", "\n" };

            // Split the CAN content at each line break and trim each string to remove any leading or trailing white-space.
            var canSplit = canStr.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
            var canList = canSplit.Select(str => str.Trim()).ToList();

            // Split the CAN-FD content at each line break and trim each string to remove any leading or trailing white-space.
            var canFdSplit = canFdStr.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
            var canFdList = canFdSplit.Select(str => str.Trim()).ToList();

            // Parse the CAN content.
            this.ProcessCanParameters(canList);

            // Parse the CAN-FD content.
            this.ProcessCanFdParameters(canFdList);
        }

        /// <summary>
        /// Parses the CAN parameters of the A2L file into the <see cref="CanParameters"/> property.
        /// </summary>
        /// <param name="parametersList">
        /// List of strings corresponding the content between the \begin mark of XCP_ON_CAN and and its \end mark, or the
        /// \begin mark of a nested element. Each string in the list corresponds to one line of this content.
        /// </param>
        /// <note type="note">
        /// This method is used as part of the parsing process of the XCP_ON_CAN element and
        /// it is called exclusively by <see cref="GetXcpOnCan(string)"/>.
        /// </note>
        private void ProcessCanParameters(List<string> parametersList)
        {
            foreach (var parameter in parametersList)
            {
                // The only line that doesn't have the format "KEYWORD VALUE" is the CAN-Version, hence
                // if no space is detected, we can assume we are parsing the CAN-Version.
                if (!parameter.Contains(" "))
                {
                    this.CanParameters.XcpOnCanVersion = Convert.ToUInt16(parameter, 16);
                }
                else
                {
                    // Split the string in KEYWORD and VALUE.
                    var paramSplit = parameter.Split(' ');

                    // Parse each KEYWORD accordingly.
                    switch (paramSplit[0])
                    {
                        case "CAN_ID_BROADCAST":
                            this.CanParameters.CanIdBroadcast = Convert.ToUInt32(paramSplit[1], 16);
                            break;
                        case "CAN_ID_MASTER":
                            this.CanParameters.CanIdMaster = Convert.ToUInt32(paramSplit[1], 16);
                            break;
                        case "CAN_ID_SLAVE":
                            this.CanParameters.CanIdSlave = Convert.ToUInt32(paramSplit[1], 16);
                            break;
                        case "BAUDRATE":
                            this.CanParameters.Baudrate = Convert.ToUInt32(paramSplit[1], 16);
                            break;
                        case "SAMPLE_POINT":
                            this.CanParameters.SamplePoint = Convert.ToByte(paramSplit[1], 16);
                            break;
                        case "SAMPLE_RATE":
                            this.CanParameters.SampleRate = (CanParameters.SampleRates)Enum.Parse(typeof(CanParameters.SampleRates), paramSplit[1], true);
                            break;
                        case "BTL_CYCLES":
                            this.CanParameters.BtlCycles = Convert.ToByte(paramSplit[1], 16);
                            break;
                        case "SJW":
                            this.CanParameters.Sjw = Convert.ToByte(paramSplit[1], 16);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Parses the CAN-FD parameters of the A2L file into the <see cref="CanParameters"/> property.
        /// </summary>
        /// <param name="parametersList">
        /// List of strings corresponding the content between the \begin mark of CAN_FD and and its \end mark, or the
        /// \begin mark of a nested element. Each string in the list corresponds to one line of this content.
        /// </param>
        /// <note type="note">
        /// This method is used as part of the parsing process of the XCP_ON_CAN element and
        /// it is called exclusively by <see cref="GetXcpOnCan(string)"/>.
        /// </note>
        private void ProcessCanFdParameters(List<string> parametersList)
        {
            foreach (var parameter in parametersList)
            {
                // Split the string in KEYWORD and VALUE.
                var paramSplit = parameter.Split(' ');

                // Parse each KEYWORD accordingly.
                switch (paramSplit[0])
                {
                    case "MAX_DLC":
                        this.CanParameters.CanFdParameters.MaxDlc = Convert.ToUInt16(paramSplit[1], 16);
                        break;
                    case "CAN_FD_DATA_TRANSFER_BAUDRATE":
                        this.CanParameters.CanFdParameters.CanFdDataTransferBaudrate = Convert.ToUInt32(paramSplit[1], 16);
                        break;
                    case "SAMPLE_POINT":
                        this.CanParameters.CanFdParameters.SamplePoint = Convert.ToByte(paramSplit[1], 16);
                        break;
                    case "BTL_CYCLES":
                        this.CanParameters.CanFdParameters.BtlCycles = Convert.ToByte(paramSplit[1], 16);
                        break;
                    case "SJW":
                        this.CanParameters.CanFdParameters.Sjw = Convert.ToByte(paramSplit[1], 16);
                        break;
                    case "SECONDARY_SAMPLE_POINT":
                        this.CanParameters.CanFdParameters.SecondarySamplePoint = Convert.ToByte(paramSplit[1], 16);
                        break;
                    case "TRANSCEIVER_DELAY_COMPENSATION ON":
                        if (paramSplit[1] == "ON")
                        {
                            this.CanParameters.CanFdParameters.TransceiverDelayCompensation = true;
                        }
                        else
                        {
                            this.CanParameters.CanFdParameters.TransceiverDelayCompensation = false;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Parses the COMPU_METHOD elements of the A2L file into the <see cref="CompuMethods"/> property.
        /// </summary>
        /// <param name="a2l">The content of the A2L file.</param>
        private void GetCompuMethods(string a2l)
        {
            // Regular expression for extracting all COMPU_METHOD elements.
            var compuMethodMatches = Regex.Matches(a2l, @"\/begin COMPU_METHOD((.|[\r\n]|[\r]|[\n])*?)(?:\/end COMPU_METHOD)");

            foreach (Match match in compuMethodMatches)
            {
                // Regular expression for extracting the content between the \begin mark of the COMPU_METHOD element
                // and its \end mark or the \begin mark of a nested element.
                var compuMethodStr = Regex.Match(match.Groups[0].Value.Trim(), @"\/begin COMPU_METHOD((.|[\r\n]|[\r]|[\n])*?)(?:\/begin|\/end)").Groups[1].Value.Trim();

                // The contents will be splited at each line break.
                string[] splitString = { "\r\n", "\n" };

                // Split the COMPU_METHOD content at each line break and trim each string to remove any leading or trailing white-space.
                var compuMethodSplit = compuMethodStr.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
                var compuMethodList = compuMethodSplit.Select(str => str.Trim()).ToList();

                // Parse COMPU_METHOD and add to the dictionary in the CompuMethods property.
                var compuMethod = this.ProcessCompuMethod(compuMethodList);
                this.CompuMethods.Add(compuMethod.Name, compuMethod);
            }
        }

        /// <summary>
        /// Parses a COMPU_METHOD element and returns an object representing it.
        /// </summary>
        /// <param name="compuMethodList">
        /// List of strings corresponding the content between the \begin mark of COMPU_METHOD and and its \end mark, or the
        /// \begin mark of a nested element. Each string in the list corresponds to one line of this content.
        /// </param>
        /// <returns>An object representing the parsed COMPU_METHOD element.</returns>
        /// <note type="note">
        /// This method is used as part of the parsing process of COMPU_METHOD elements and
        /// it is called exclusively by <see cref="GetCompuMethods(string)"/>.
        /// </note>
        private CompuMethod ProcessCompuMethod(List<string> compuMethodList)
        {
            var compuMethod = new CompuMethod();

            // Parse the first line.
            compuMethod.Name = compuMethodList[0].Split(' ')[0];
            compuMethod.LongIdentifier = compuMethodList[0].Split(new char[] { ' ' }, 2)[1].Replace("\"", string.Empty);

            // Remove the first line.
            compuMethodList.Remove(compuMethodList[0]);

            // Parse the second line.
            compuMethod.Type = (CompuMethod.ConversionType)Enum.Parse(typeof(CompuMethod.ConversionType), compuMethodList[0].Split(' ')[0]);
            compuMethod.Format = Regex.Matches(compuMethodList[0], "\".*?\"")[0].Value.Replace("\"", string.Empty);
            compuMethod.Unit = Regex.Matches(compuMethodList[0], "\".*?\"")[1].Value.Replace("\"", string.Empty);

            // Remove the second line.
            compuMethodList.Remove(compuMethodList[0]);

            // Parse the subsequent lines which are all formatted as "KEYWORD VALUE".
            foreach (var parameter in compuMethodList)
            {
                // Split the string in KEYWORD and VALUE.
                var paramSplit = parameter.Split(' ');

                // Parse each KEYWORD accordingly.
                switch (paramSplit[0])
                {
                    case "COEFFS":
                        compuMethod.Coeffs = paramSplit.ToList().Where((param, i) => i != 0).Select(c => Convert.ToDouble(c)).ToArray();
                        break;
                    case "COEFFS_LINEAR":
                        compuMethod.CoeffsLinear = paramSplit.ToList().Where((param, i) => i != 0).Select(c => Convert.ToDouble(c)).ToArray();
                        break;
                    case "COMPU_TAB_REF":
                        compuMethod.CompuTabRef = paramSplit[1];
                        break;
                }
            }

            // Return the parsed COMPU_METHOD.
            return compuMethod;
        }

        /// <summary>
        /// Parses the MEASUREMENT elements of the A2L file into the <see cref="Measurements"/> property.
        /// </summary>
        /// <param name="a2l">The content of the A2L file.</param>
        private void GetMeasurements(string a2l)
        {
            // Regular expression for extracting all MEASUREMENT elements.
            var measurementMatches = Regex.Matches(a2l, @"\/begin MEASUREMENT((.|[\r\n]|[\r]|[\n])*?)(?:\/end MEASUREMENT)");

            foreach (Match match in measurementMatches)
            {
                // Regular expression for extracting the content between the \begin mark of the MEASUREMENT element
                // and its \end mark or the \begin mark of a nested element.
                var measurementStr = Regex.Match(match.Groups[0].Value.Trim(), @"\/begin MEASUREMENT((.|[\r\n]|[\r]|[\n])*?)(?:\/begin|\/end)").Groups[1].Value.Trim();

                // The contents will be splited at each line break.
                string[] splitString = { "\r\n", "\n" };

                // Split the MEASUREMENT content at each line break and trim each string to remove any leading or trailing white-space.
                var measurementSplit = measurementStr.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
                var measurementList = measurementSplit.Select(str => str.Trim()).ToList();

                // Parse MEASUREMENT and add to the dictionary in the Measurements property.
                var measurement = this.ProcessMeasurement(measurementList);
                this.Measurements.Add(measurement.Name, measurement);
            }
        }

        /// <summary>
        /// Parses a MEASUREMENT element and returns an object representing it.
        /// </summary>
        /// <param name="measurementList">
        /// List of strings corresponding the content between the \begin mark of MEASUREMENT and and its \end mark, or the
        /// \begin mark of a nested element. Each string in the list corresponds to one line of this content.
        /// </param>
        /// <returns>An object representing the parsed MEASUREMENT element.</returns>
        /// <note type="note">
        /// This method is used as part of the parsing process of MEASUREMENT elements and
        /// it is called exclusively by <see cref="GetMeasurements(string)"/>.
        /// </note>
        private Measurement ProcessMeasurement(List<string> measurementList)
        {
            var measurement = new Measurement();

            // Parse the first line.
            measurement.Name = measurementList[0].Split(' ')[0];
            measurement.LongIdentifier = measurementList[0].Split(new char[] { ' ' }, 2)[1].Replace("\"", string.Empty);

            // Remove the first line.
            measurementList.Remove(measurementList[0]);

            // Parse the second line.
            measurement.DataType = (Measurement.DataTypes)Enum.Parse(typeof(Measurement.DataTypes), measurementList[0].Split(' ')[0]);
            measurement.Conversion = measurementList[0].Split(' ')[1];
            measurement.Resolution = long.Parse(measurementList[0].Split(' ')[2]);
            measurement.Accuracy = double.Parse(measurementList[0].Split(' ')[3]);
            measurement.LowerLimit = double.Parse(measurementList[0].Split(' ')[4]);
            measurement.UpperLimit = double.Parse(measurementList[0].Split(' ')[5]);

            // Remove the second line.
            measurementList.Remove(measurementList[0]);

            // Parse the subsequent lines which are all formatted as "KEYWORD VALUE".
            foreach (var parameter in measurementList)
            {
                // Split the string in KEYWORD and VALUE.
                var paramSplit = parameter.Split(' ');

                // Parse each KEYWORD accordingly.
                switch (paramSplit[0])
                {
                    case "MATRIX_DIM":
                        measurement.MatrixDim[0] = Convert.ToUInt32(paramSplit[1]);
                        measurement.MatrixDim[1] = paramSplit.Length > 2 ? Convert.ToUInt32(paramSplit[2]) : 0;
                        measurement.MatrixDim[2] = paramSplit.Length > 3 ? Convert.ToUInt32(paramSplit[3]) : 0;
                        break;
                    case "ARRAY_SIZE":
                    case "NUMBER":
                        measurement.MatrixDim[0] = Convert.ToUInt32(paramSplit[1], 10);
                        break;
                    case "BIT_MASK":
                        measurement.BitMask = Convert.ToUInt64(paramSplit[1], 16);
                        break;
                    case "ECU_ADDRESS":
                        measurement.EcuAddress = Convert.ToUInt32(paramSplit[1], 16);
                        break;
                    case "ECU_ADDRESS_EXTENSION":
                        measurement.EcuAddressExtension = Convert.ToByte(paramSplit[1], 16);
                        break;
                    case "FORMAT":
                        measurement.Format = paramSplit[1].Replace("\"", string.Empty);
                        break;
                }
            }

            // Only add a correspondent COMPU_METHOD, if required.
            if (measurement.Conversion != "NO_COMPU_METHOD")
            {
                try
                {
                    measurement.CompuMethod = this.CompuMethods[measurement.Conversion];
                }
                catch
                {
                    MessageBox.Show($"{measurement.Conversion}");
                }
            }

            // Return the parsed MEASUREMENT.
            return measurement;
        }

        /// <summary>
        /// Parses the CHARACTERISTIC elements of the A2L file into the <see cref="Characteristics"/> property.
        /// </summary>
        /// <param name="a2l">The content of the A2L file.</param>
        private void GetCharacteristics(string a2l)
        {
            // Regular expression for extracting all CHARACTERISTIC elements.
            var characteristicMatches = Regex.Matches(a2l, @"\/begin CHARACTERISTIC((.|[\r\n]|[\r]|[\n])*?)(?:\/end CHARACTERISTIC)");

            foreach (Match match in characteristicMatches)
            {
                // Regular expression for extracting the content between the \begin mark of the CHARACTERISTIC element
                // and its \end mark or the \begin mark of a nested element.
                var characteristicStr = Regex.Match(match.Groups[0].Value.Trim(), @"\/begin CHARACTERISTIC((.|[\r\n]|[\r]|[\n])*?)(?:\/begin|\/end)").Groups[1].Value.Trim();

                // The contents will be splited at each line break.
                string[] splitString = { "\r\n", "\n" };

                // Split the CHARACTERISTIC content at each line break and trim each string to remove any leading or trailing white-space.
                var characteristicSplit = characteristicStr.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
                var characteristicList = characteristicSplit.Select(str => str.Trim()).ToList();

                // Parse CHARACTERISTIC and add to the dictionary in the Measurements property.
                var characteristic = this.ProcessCharacteristic(characteristicList);
                this.Characteristics.Add(characteristic.Name, characteristic);
            }
        }

        /// <summary>
        /// Parses a CHARACTERISTIC element and returns an object representing it.
        /// </summary>
        /// <param name="characteristicList">
        /// List of strings corresponding the content between the \begin mark of CHARACTERISTIC and and its \end mark, or the
        /// \begin mark of a nested element. Each string in the list corresponds to one line of this content.
        /// </param>
        /// <returns>An object representing the parsed CHARACTERISTIC element.</returns>
        /// <note type="note">
        /// This method is used as part of the parsing process of CHARACTERISTIC elements and
        /// it is called exclusively by <see cref="GetCharacteristics(string)"/>.
        /// </note>
        private Characteristic ProcessCharacteristic(List<string> characteristicList)
        {
            var characteristic = new Characteristic();

            // Parse the first line.
            characteristic.Name = characteristicList[0].Split(' ')[0];
            characteristic.LongIdentifier = characteristicList[0].Split(new char[] { ' ' }, 2)[1].Replace("\"", string.Empty);

            // Remove the first line.
            characteristicList.RemoveAt(0);

            // Parse the second line.
            characteristic.Type = (Characteristic.CharacteristicType)Enum.Parse(typeof(Characteristic.CharacteristicType), characteristicList[0].Split(' ')[0]);
            characteristic.EcuAddress = Convert.ToUInt32(characteristicList[0].Split(' ')[1], 16);

            // Get a list of all possible Data Types for a CHARACTERISTIC element.
            var listOfEnums = Enum.GetValues(typeof(XcpValue.DataTypes)).Cast<XcpValue.DataTypes>();

            var dataTypeString = characteristicList[0].Split(' ')[2];

            // Get a subset of the previous list with the enums that have their name contained in the data type field of the CHARACTERISTIC being parsed.
            var listOfMatchingEnumStrings = listOfEnums.Where(t => dataTypeString.Contains(t.ToString())).Select(t => t.ToString());

            // Get only the longest match from the previous list, which is the parsed data type.
            var longestString = listOfMatchingEnumStrings.Aggregate(string.Empty, (max, cur) => max.Length > cur.Length ? max : cur);

            // The data type is casted to the correspondent enum.
            try
            {
                characteristic.DataType = (XcpValue.DataTypes)Enum.Parse(typeof(XcpValue.DataTypes), longestString);
            }
            catch
            {
                characteristic.DataType = XcpValue.DataTypes.UNKNOWN;
            }

            switch (characteristic.DataType)
            {
                case XcpValue.DataTypes.STANDARD_VALUE_U8:
                    characteristic.DataType = XcpValue.DataTypes.UBYTE;
                    break;
                case XcpValue.DataTypes.STANDARD_VALUE_S8:
                    characteristic.DataType = XcpValue.DataTypes.SBYTE;
                    break;
                case XcpValue.DataTypes.STANDARD_VALUE_U16:
                    characteristic.DataType = XcpValue.DataTypes.UWORD;
                    break;
                case XcpValue.DataTypes.STANDARD_VALUE_S16:
                    characteristic.DataType = XcpValue.DataTypes.SWORD;
                    break;
                default:
                    break;
            }

            // Continue parsing the second line.
            characteristic.MaxDiff = Convert.ToDouble(characteristicList[0].Split(' ')[3]);
            characteristic.Conversion = characteristicList[0].Split(' ')[4];
            characteristic.LowerLimit = Convert.ToDouble(characteristicList[0].Split(' ')[5]);
            characteristic.UpperLimit = Convert.ToDouble(characteristicList[0].Split(' ')[6]);

            // Remove the second line.
            characteristicList.RemoveAt(0);

            // Parse the subsequent lines which are all formatted as "KEYWORD VALUE".
            foreach (var parameter in characteristicList)
            {
                // Split the string in KEYWORD and VALUE.
                var paramSplit = parameter.Split(' ');

                // Parse each KEYWORD accordingly.
                switch (paramSplit[0])
                {
                    case "BIT_MASK":
                        characteristic.BitMask = Convert.ToUInt64(paramSplit[1], 16);
                        break;
                    case "ECU_ADDRESS_EXTENSION":
                        characteristic.EcuAddressExtension = Convert.ToByte(paramSplit[1], 16);
                        break;
                    case "EXTENDED_LIMITS":
                        characteristic.ExtLimits.Lower = Convert.ToDouble(paramSplit[1]);
                        characteristic.ExtLimits.Upper = Convert.ToDouble(paramSplit[2]);
                        break;
                    case "FORMAT":
                        characteristic.Format = paramSplit[1];
                        break;
                    case "MATRIX_DIM":
                        characteristic.MatrixDim[0] = Convert.ToUInt32(paramSplit[1]);
                        characteristic.MatrixDim[1] = paramSplit.Length > 2 ? Convert.ToUInt32(paramSplit[2]) : 0;
                        characteristic.MatrixDim[2] = paramSplit.Length > 3 ? Convert.ToUInt32(paramSplit[3]) : 0;
                        break;
                    case "NUMBER":
                        characteristic.Number = Convert.ToInt32(paramSplit[1]);
                        break;
                    case "READ_ONLY":
                        characteristic.ReadOnly = true;
                        break;
                }
            }

            // Only add a correspondent COMPU_METHOD, if required.
            if (characteristic.Conversion != "NO_COMPU_METHOD")
            {
                characteristic.CompuMethod = this.CompuMethods[characteristic.Conversion];
            }

            // Return the parsed CHARACTERISTIC.
            return characteristic;
        }
    }
}
