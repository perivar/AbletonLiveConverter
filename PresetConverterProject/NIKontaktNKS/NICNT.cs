using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CommonUtils;
using Serilog;

namespace PresetConverterProject.NIKontaktNKS
{
    public static class NICNT
    {
        static readonly byte[] NKS_NICNT_MTD = new byte[] { 0x2F, 0x5C, 0x20, 0x4E, 0x49, 0x20, 0x46, 0x43, 0x20, 0x4D, 0x54, 0x44, 0x20, 0x20, 0x2F, 0x5C }; // /\ NI FC MTD  /\
        static readonly byte[] NKS_NICNT_TOC = new byte[] { 0x2F, 0x5C, 0x20, 0x4E, 0x49, 0x20, 0x46, 0x43, 0x20, 0x54, 0x4F, 0x43, 0x20, 0x20, 0x2F, 0x5C }; // /\ NI FC TOC  /\

        public static void Unpack(string file, string outputDirectoryPath, bool doList, bool doVerbose)
        {
            using (BinaryFile bf = new BinaryFile(file, BinaryFile.ByteOrder.LittleEndian, false))
            {
                var header = bf.ReadBytes(16);
                if (header.SequenceEqual(NKS_NICNT_MTD)) // 2F 5C 20 4E 49 20 46 43 20 4D 54 44 20 20 2F 5C   /\ NI FC MTD  /\
                {
                    bf.Seek(66, SeekOrigin.Begin);
                    string version = bf.ReadString(3 * 2, Encoding.Unicode);
                    Log.Information("Version: " + version);

                    string outputFileName = Path.GetFileNameWithoutExtension(file);
                    if (!doList) IOUtils.CreateDirectoryIfNotExist(Path.Combine(outputDirectoryPath, outputFileName));

                    // Save version in ContentVersion.txt 
                    if (!doList) IOUtils.WriteTextToFile(Path.Combine(outputDirectoryPath, outputFileName, "ContentVersion.txt"), version);

                    bf.Seek(132, SeekOrigin.Begin);
                    int unknown1 = bf.ReadInt32();
                    if (doVerbose) Log.Debug("Unknown1: " + unknown1);

                    bf.Seek(144, SeekOrigin.Begin);

                    int startOffset = bf.ReadInt32();
                    Log.Information("Start Offset: " + startOffset);

                    int unknown3 = bf.ReadInt32();
                    if (doVerbose) Log.Debug("Unknown3: " + unknown3);

                    bf.Seek(256, SeekOrigin.Begin);

                    string productHintsXml = bf.ReadStringNull();
                    if (doVerbose) Log.Debug("ProductHints Xml:\n" + productHintsXml);

                    // Save productHints as xml 
                    if (!doList) IOUtils.WriteTextToFile(Path.Combine(outputDirectoryPath, outputFileName, outputFileName + ".xml"), productHintsXml);

                    // the Data is an icon stored as Base64 String
                    // https://codebeautify.org/base64-to-image-converter

                    bf.Seek(startOffset + 256, SeekOrigin.Begin);
                    var header2 = bf.ReadBytes(16);
                    if (header2.SequenceEqual(NKS_NICNT_MTD)) // 2F 5C 20 4E 49 20 46 43 20 4D 54 44 20 20 2F 5C   /\ NI FC MTD  /\
                    {
                        bf.ReadBytes(116);

                        long unknown4 = bf.ReadInt64();
                        if (doVerbose) Log.Debug("Unknown4: " + unknown4);

                        bf.ReadBytes(4);

                        long unknown5 = bf.ReadInt64();
                        if (doVerbose) Log.Debug("Unknown5: " + unknown5);

                        bf.ReadBytes(104);

                        long unknown6 = bf.ReadInt64();
                        if (doVerbose) Log.Debug("Unknown6: " + unknown6);

                        var delimiter1 = bf.ReadBytes(8);
                        if (doVerbose) Log.Debug("Delimiter1: " + StringUtils.ByteArrayToHexString(delimiter1)); // F0 F0 F0 F0 F0 F0 F0 F0
                        if (!delimiter1.SequenceEqual(new byte[] { 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0 }))
                        {
                            Log.Error("Delimiter1 not as expected 'F0 F0 F0 F0 F0 F0 F0 F0' but got " + StringUtils.ToHexAndAsciiString(delimiter1));
                        }

                        long totalResourceCount = bf.ReadInt64();
                        Log.Information("Total Resource Count: " + totalResourceCount);

                        long totalResourceLength = bf.ReadInt64();
                        Log.Information("Total Resource Length: " + totalResourceLength);

                        var resourceList = new List<NICNTResource>();
                        var header3 = bf.ReadBytes(16);
                        if (header3.SequenceEqual(NKS_NICNT_TOC)) // 2F 5C 20 4E 49 20 46 43 20 54 4F 43 20 20 2F 5C  /\ NI FC TOC  /\
                        {
                            bf.ReadBytes(600);

                            long lastIndex = 0;
                            for (int i = 0; i < totalResourceCount; i++)
                            {
                                var resource = new NICNTResource();

                                Log.Information("-------- Index: " + bf.Position + " --------");

                                long resCounter = bf.ReadInt64();
                                Log.Information("Resource Counter: " + resCounter);
                                resource.Count = resCounter;

                                bf.ReadBytes(16);

                                string resName = bf.ReadString(600, Encoding.Unicode).TrimEnd('\0');
                                Log.Information("Resource Name: " + resName);
                                resource.Name = resName;

                                long resUnknown = bf.ReadInt64();
                                if (doVerbose) Log.Debug("Resource Unknown: " + resUnknown);

                                long resIndex = bf.ReadInt64();
                                Log.Information("Resource Index: " + resIndex);
                                resource.Index = resIndex;

                                if (lastIndex > 0)
                                {
                                    resource.Length = resIndex - lastIndex;
                                }
                                else
                                {
                                    resource.Length = resIndex;
                                }
                                Log.Information("Resource Length: " + resource.Length);

                                lastIndex = resIndex;
                                resourceList.Add(resource);
                            }
                            Log.Information("-------- Index: " + bf.Position + " --------");

                            var delimiter2 = bf.ReadBytes(8);
                            if (doVerbose) Log.Debug("Delimiter2: " + StringUtils.ByteArrayToHexString(delimiter2)); // F1 F1 F1 F1 F1 F1 F1 F1

                            if (!delimiter2.SequenceEqual(new byte[] { 0xF1, 0xF1, 0xF1, 0xF1, 0xF1, 0xF1, 0xF1, 0xF1 }))
                            {
                                Log.Error("Delimiter2 not as expected 'F1 F1 F1 F1 F1 F1 F1 F1' but got " + StringUtils.ToHexAndAsciiString(delimiter2));
                            }

                            long unknown13 = bf.ReadInt64();
                            if (doVerbose) Log.Debug("Unknown13: " + unknown13);

                            long unknown14 = bf.ReadInt64();
                            if (doVerbose) Log.Debug("Unknown14: " + unknown14);

                            var header4 = bf.ReadBytes(16);
                            if (header4.SequenceEqual(NKS_NICNT_TOC)) // 2F 5C 20 4E 49 20 46 43 20 54 4F 43 20 20 2F 5C  /\ NI FC TOC  /\
                            {
                                bf.ReadBytes(592);

                                if (!doList) IOUtils.CreateDirectoryIfNotExist(Path.Combine(outputDirectoryPath, outputFileName, "Resources"));

                                foreach (var res in resourceList)
                                {
                                    string escapedFileName = FromUnixFileNames(res.Name);
                                    Log.Information(String.Format("Resource '{0}' @ position {1} [{2} bytes]", escapedFileName, bf.Position, res.Length));

                                    res.Data = bf.ReadBytes((int)res.Length);

                                    // if not only listing, save files
                                    if (!doList)
                                    {
                                        string outputFilePath = Path.Combine(outputDirectoryPath, outputFileName, "Resources", escapedFileName);
                                        BinaryFile outBinaryFile = new BinaryFile(outputFilePath, BinaryFile.ByteOrder.LittleEndian, true);

                                        outBinaryFile.Write(res.Data);
                                        outBinaryFile.Close();
                                    }
                                }
                            }
                            else
                            {
                                Log.Error("Header4 not as expected '/\\ NI FC TOC  /\\' but got " + StringUtils.ToHexAndAsciiString(header4));
                            }
                        }
                        else
                        {
                            Log.Error("Header3 not as expected '/\\ NI FC TOC  /\\' but got " + StringUtils.ToHexAndAsciiString(header3));
                        }
                    }
                    else
                    {
                        Log.Error("Header2 not as expected '/\\ NI FC MTD  /\\' but got " + StringUtils.ToHexAndAsciiString(header2));
                    }
                }
                else
                {
                    Log.Error("Header not as expected '/\\ NI FC MTD  /\\' but got " + StringUtils.ToHexAndAsciiString(header));
                }
            }
        }

        // TODO: copied from SteinbergREVerence, where should this go?
        private static void WritePaddedUnicodeString(BinaryFile bf, string text, int totalCount)
        {
            int count = bf.WriteStringNull(text, Encoding.Unicode);
            int remaining = totalCount - count;
            var bytes = new byte[remaining];
            bf.Write(bytes);
        }

        public static void Pack(string inputDirectoryPath, string outputFilePath, bool doList, bool doVerbose)
        {
            using (BinaryFile bf = new BinaryFile(outputFilePath, BinaryFile.ByteOrder.LittleEndian, true))
            {
                bf.Write(NKS_NICNT_MTD); // 2F 5C 20 4E 49 20 46 43 20 4D 54 44 20 20 2F 5C   /\ NI FC MTD  /\                
                bf.Write(new byte[50]); // 50 zero bytes

                string version = "1.0";
                WritePaddedUnicodeString(bf, version, 66); // zero padded string

                Int32 unknown1 = 1;
                bf.Write(unknown1);

                bf.Write(new byte[8]); // 8 zero bytes

                Int32 startOffset = 512000;
                bf.Write(startOffset);

                Int32 unknown3 = 512000;
                bf.Write(unknown3);

                bf.Write(new byte[104]); // 104 zero bytes

                string productHintsXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?><ProductHints spec=""1.0.16""></ProductHints>";
                bf.Write(productHintsXml);

                bf.Write(new byte[startOffset + 256 - bf.Position]); // 512000 + 256 zero bytes - current pos

                bf.Write(NKS_NICNT_MTD); // 2F 5C 20 4E 49 20 46 43 20 4D 54 44 20 20 2F 5C   /\ NI FC MTD  /\                
                bf.Write(new byte[116]); // 116 zero bytes

                Int64 unknown4 = 2;
                bf.Write(unknown4);

                bf.Write(new byte[4]); // 4 zero bytes

                Int64 unknown5 = 1;
                bf.Write(unknown5);

                bf.Write(new byte[104]); // 104 zero bytes

                Int64 unknown6 = 1;
                bf.Write(unknown6);

                // write delimiter
                bf.Write(StringUtils.HexStringToByteArray("F0F0F0F0F0F0F0F0"));

                var resourceList = new List<NICNTResource>();

                Int64 totalResourceCount = resourceList.Count;
                bf.Write(totalResourceCount);

                Int64 totalResourceLength = 0; // sum of bytes in the resourceList
                bf.Write(totalResourceLength);

                bf.Write(NKS_NICNT_TOC); // 2F 5C 20 4E 49 20 46 43 20 54 4F 43 20 20 2F 5C  /\ NI FC TOC  /\

                bf.Write(new byte[600]); // 600 zero bytes

                for (int i = 0; i < totalResourceCount; i++)
                {
                    var res = resourceList[i];

                    Int64 resCounter = i + 1;
                    bf.Write(resCounter);

                    bf.Write(new byte[16]); // 16 zero bytes

                    WritePaddedUnicodeString(bf, res.Name, 600); // zero padded string    

                    Int64 resUnknown = 0;
                    bf.Write(resUnknown);

                    Int64 resIndex = 0; // aggregated index
                    bf.Write(resIndex);
                }

                // write delimiter
                bf.Write(StringUtils.HexStringToByteArray("F1F1F1F1F1F1F1F1"));

                Int64 unknown13 = 1;
                bf.Write(unknown13);

                Int64 unknown14 = 1;
                bf.Write(unknown14);

                bf.Write(NKS_NICNT_TOC); // 2F 5C 20 4E 49 20 46 43 20 54 4F 43 20 20 2F 5C  /\ NI FC TOC  /\

                bf.Write(new byte[592]); // 592 zero bytes

                foreach (var res in resourceList)
                {
                    string unescapedFileName = ToUnixFileName(res.Name);
                    Log.Information(String.Format("Resource '{0}' @ position {1} [{2} bytes]", unescapedFileName, bf.Position, res.Length));

                    bf.Write(res.Data);
                }
            }
        }

        /// <summary>
        /// Class to store a NICNT resource
        /// </summary>
        class NICNTResource
        {
            public long Count { get; set; }
            public string Name { get; set; }
            public long Length { get; set; }
            public byte[] Data { get; set; }
            public long Index { get; set; }
            public long RealIndex { get; set; }
        }

        // replacement map
        static Dictionary<string, string> entityReplacements = new Dictionary<string, string> {
                { "\\", "[bslash]" },
                { "?", "[qmark]" },
                { "*", "[star]" },
                { "\"", "[quote]" },
                { "|", "[pipe]" },
                { ":", "[colon]" },
                { "<", "[less]" },
                { ">", "[greater]" }
             };

        /// <summary>
        /// Convert from unix filenames to a filename that can be stored on windows
        /// i.e. convert | to [pipe], etc.
        /// </summary>
        /// <param name="fileName">unix filename</param>
        /// <returns>a windows supported unix filename</returns>
        public static string FromUnixFileNames(string fileName)
        {
            // \ [bslash]
            // ? [qmark]
            // * [star]
            // " [quote]
            // | [pipe]
            // : [colon]
            // < [less]
            // > [greater]
            // _ [space] (only at the end of the name)
            // . [dot] (only at the end of the name)

            // Regexp background information - test using https://regex101.com/
            // ----------------------------------------------------------------------------------------------------------------
            // https://stackoverflow.com/questions/8113104/what-is-regex-for-odd-length-series-of-a-known-character-in-a-string
            // (?<!A)(?:AA)*A(?!A)        
            //   (?<!A)     # asserts that it should not be preceded by an 'A' 
            //   (?:AA)*A   # matches an odd number of 'A's 
            //   (?!A)      # asserts it should not be followed by an 'A'.

            // https://stackoverflow.com/questions/28113962/regular-expression-to-match-unescaped-characters-only
            // (?<!\\)(?:(\\\\)*)[*]

            // https://stackoverflow.com/questions/816915/match-uneven-number-of-escape-symbols
            // (?<!\\)(?:\\\\)*\\ \n
            //   (?<!\\)    # not preceded by a backslash
            //   (?:\\\\)*  # zero or more escaped backslashes
            //   \\ \n      # single backslash and linefeed

            // https://stackoverflow.com/questions/22375138/regex-in-c-sharp-expression-in-negative-lookbehind
            // (?<=(^|[^?])(\?\?)*\?)
            //    (^|[^?])   # not a question mark (possibly also start of string, i.e. nothing)
            //    (\?\?)*    # any number of question mark pairs
            //    \?         # a single question mark

            // https://www.wipfli.com/insights/blogs/connect-microsoft-dynamics-365-blog/c-regex-multiple-replacements
            // using Regex.Replace MatchEvaluator delegate to perform multiple replacements

            // escape all control sequences 
            // match even number of [ in front of a control character
            const string replaceControlSequencesEven = @"(?<!\[)(\[\[)+(?!\[)(?:bslash|qmark|star|quote|pipe|colon|less|greater|space|dot)";
            // (?<!\[)               # asserts that it should not be preceded by a '['
            // (\[\[)+               # matches an even number of '['s (at least one pair)
            // (?!\[)                # asserts it should not be followed by an '['
            // (?:bslash|qmark|...)  # non-caputuring group that only matches a control sequence  
            fileName = Regex.Replace(fileName, replaceControlSequencesEven,
                // add the first group to effectively double the found '['s, which will escape them  
                m => m.Groups[1].Value + m.Value
            );

            // escape all control sequences 
            // match odd number of [ in front of a control character
            const string replaceControlSequencesOdd = @"(?<!\[)((?:\[\[)*\[)(?!\[)(?:bslash|qmark|star|quote|pipe|colon|less|greater|space|dot)";
            // (?<!\[)               # asserts that it should not be preceded by a '['
            // ((?:\[\[)*\[)         # matches a odd number of '['s (at least one)
            // (?!\[)                # asserts it should not be followed by an '['
            // (?:bslash|qmark|...)  # non-caputuring group that only matches a control sequence  
            fileName = Regex.Replace(fileName, replaceControlSequencesOdd,
                // escape every odd number of '[' with another '[', which makes them even - meaning this regexp must come after the even check!
                m => "[" + m.Value
            );

            // replace all control characters that does start with a character [
            // Note! remember to add another [
            const string replaceControlWithEscape = @"(\[+)([\?*""|:<>])";
            // (\[+)                 # match at least one '['
            // ([\?*""|:<>])         # match the control character
            fileName = Regex.Replace(fileName, replaceControlWithEscape,
                // double the first group match to effectively double the found '['s, which will escape them  
                m => m.Groups[1].Value + m.Groups[1].Value + entityReplacements[m.Groups[2].Value]
            );

            // replace all control characters that doesn't start with an escape character [
            const string replaceControlWithoutEscape = @"(?<!\[)[\?*""|:<>]";
            // (?<!\[)               # asserts that it should not be preceded by a '['
            // [\?*""|:<>]           # match the control character
            fileName = Regex.Replace(fileName, replaceControlWithoutEscape,
                m => entityReplacements[m.Value]
            );

            while (fileName.EndsWith(" "))
            {
                fileName = fileName.Replace(" ", "[space]");
            }

            while (fileName.EndsWith("."))
            {
                fileName = fileName.Replace(".", "[dot]");
            }

            return fileName;
        }

        /// <summary>
        /// Convert from windows filename with unix patterns back to unix filename
        /// i.e. convert from [pipe] to |, etc.
        /// </summary>
        /// <param name="fileName">windows supported unix filename</param>
        /// <returns>a unix filename</returns>
        public static string ToUnixFileName(string fileName)
        {
            // \ [bslash]
            // ? [qmark]
            // * [star]
            // " [quote]
            // | [pipe]
            // : [colon]
            // < [less]
            // > [greater]
            // _ [space] (only at the end of the name)
            // . [dot] (only at the end of the name)

            // replace all control sequences 
            // match odd number of [ in front of a control character
            const string replaceControlSequencesOdd = @"(?<!\[)((?:\[\[)*\[)(?!\[)(bslash|qmark|star|quote|pipe|colon|less|greater|space|dot)\]";
            // (?<!\[)               # asserts that it should not be preceded by a '['
            // ((?:\[\[)*\[)         # matches a odd number of '['s (at least one)
            // (?!\[)                # asserts it should not be followed by an '['
            // (bslash|qmark|...)    # matches any control sequence  
            // \]                    # asserts that it needs to end with a ']'
            fileName = Regex.Replace(fileName, replaceControlSequencesOdd,
                m =>
                {
                    var val = "[" + m.Groups[2].Value + "]";
                    var entity = entityReplacements.FirstOrDefault(x => x.Value == val);
                    // if the number of brackets are 3 - reduce them by two
                    var prefix = (m.Groups[1].Value.Length >= 3 ? new String('[', (m.Groups[1].Value.Length - 2)) : "");
                    return prefix + entity.Key;
                }
            );

            // replace all control sequences 
            // match even number of [ in front of a control character
            // each pair of these brackets ([[) will be replaced with a single ([).
            const string replaceControlSequencesEven = @"(?<!\[)((?:\[\[)+)(?!\[)(bslash|qmark|star|quote|pipe|colon|less|greater|space|dot)\]";
            // (?<!\[)               # asserts that it should not be preceded by a '['
            // ((?:\[\[)+)           # matches an even number of '['s (at least one pair)
            // (?!\[)                # asserts it should not be followed by an '['
            // (bslash|qmark|...)    # matches any control sequence  
            // \]                    # asserts that it needs to end with a ']'
            fileName = Regex.Replace(fileName, replaceControlSequencesEven,
                m =>
                {
                    var prefix = (m.Groups[1].Value.Length >= 4 ? new String('[', (m.Groups[1].Value.Length / 2)) : "[");
                    return prefix + m.Groups[2].Value + "]";
                }
            );

            while (fileName.EndsWith("[space]"))
            {
                fileName = fileName.Replace("[space]", " ");
            }

            while (fileName.EndsWith("[dot]"))
            {
                fileName = fileName.Replace("[dot]", ".");
            }

            return fileName;
        }
    }
}