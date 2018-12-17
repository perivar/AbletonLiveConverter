using System;
using System.Collections.Generic;
using System.IO;

namespace CommonUtils
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Search with an array of bytes to find a specific pattern
        /// </summary>
        /// <param name="byteArray">byte array</param>
        /// <param name="bytePattern">byte array pattern</param>
        /// <param name="startIndex">index to start searching at</param>
        /// <param name="count">how many elements to look through</param>
        /// <returns>position</returns>
        /// <example>
        /// find the last 'List' entry	
        /// reading all bytes at once is not very performant, but works for these relatively small files	
        /// byte[] allBytes = File.ReadAllBytes(fileName);
        /// reading from the end of the file by reversing the array	
        /// byte[] reversed = allBytes.Reverse().ToArray();
        /// find 'List' backwards	
        /// int reverseIndex = IndexOfBytes(reversed, Encoding.UTF8.GetBytes("tsiL"), 0, reversed.Length);
        /// if (reverseIndex < 0)
        /// {
        /// reverseIndex = 64;
        /// }
        /// int index = allBytes.Length - reverseIndex - 4; // length of List is 4	
        /// Console.WriteLine("DEBUG: File length: {0}, 'List' found at index: {1}", allBytes.Length, index);
        /// </example>
        public static int IndexOf(this byte[] byteArray, byte[] bytePattern, int startIndex, int count)
        {
            if (byteArray == null || byteArray.Length == 0 || bytePattern == null || bytePattern.Length == 0 || count == 0)
            {
                return -1;
            }

            int i = startIndex;
            int endIndex = count > 0 ? Math.Min(startIndex + count, byteArray.Length) : byteArray.Length;
            int foundIndex = 0;
            int lastFoundIndex = 0;

            while (i < endIndex)
            {
                lastFoundIndex = foundIndex;
                foundIndex = (byteArray[i] == bytePattern[foundIndex]) ? ++foundIndex : 0;
                if (foundIndex == bytePattern.Length)
                {
                    return i - foundIndex + 1;
                }
                if (lastFoundIndex > 0 && foundIndex == 0)
                {
                    i = i - lastFoundIndex;
                    lastFoundIndex = 0;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Find all occurrences of byte pattern in byte array
        /// </summary>
        /// <param name="byteArray">byte array</param>
        /// <param name="bytePattern">byte array pattern</param>
        /// <returns>positions</returns>
        /// <example>
        /// foreach (int i in FindAll(byteArray, bytePattern))
        /// {
        ///    Console.WriteLine(i);
        /// }
        /// </example>
        public static IEnumerable<int> FindAll(this byte[] byteArray, byte[] bytePattern)
        {
            for (int startIndex = 0; startIndex < byteArray.Length - bytePattern.Length;)
            {
                int i = IndexOf(byteArray, bytePattern, startIndex, byteArray.Length);

                if (i < 0) break;
                yield return i;

                startIndex = i + 1;
            }
        }

        /// <summary>
        /// Seek in BinaryFile until pattern is found, return start index
        /// </summary>
        /// <param name="binaryFile">binaryFile</param>
        /// <param name="pattern">byte pattern to find</param>
        /// <param name="offset">offset to seek to (if > 0)</param>
        /// <returns>index where found (BinaryFile position will be at this index)</returns>
        public static int IndexOf(this BinaryFile binaryFile, byte[] pattern, int offset)
        {
            // seek to offset
            if (offset > 0 && offset < binaryFile.Length)
            {
                binaryFile.Seek(offset, SeekOrigin.Begin);
            }

            int success = 0;
            for (int i = 0; i < binaryFile.Length - binaryFile.Position; i++)
            {
                var b = binaryFile.ReadByte();
                if (b == pattern[success])
                {
                    success++;
                }
                else
                {
                    success = 0;
                }

                if (pattern.Length == success)
                {
                    int index = (int)(binaryFile.Position - pattern.Length);
                    binaryFile.Seek(index, SeekOrigin.Begin);
                    return index;
                }
            }
            return -1;
        }
    }
}