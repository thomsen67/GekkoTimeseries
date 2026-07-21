using System;
using System.Security.Cryptography;

namespace Gekko
{
    public static class Hashing
    {

        //Hashing is like the following schema:

        /*        
        GekkoNull:
        [Null] name

        SeriesNormal
        [SeriesNormal] name per1 [ListDouble] n val1..val2..

        SeriesArray
        [SeriesArray] name n series1..series2..

        List
        [List] name n var1..var2.. (null names)

        Map
        [Dictionary] name n var1..var2..

        Matrix
        [MatrixDouble] name [ListDouble2d] n1 n2 val1..val2..

        ScalarString
        [ScalarString] name [String] n char1..char2..

        ScalarDate
        [ScalarDate] name [Date] freq super sub subsub

        ScalarVal
        [ScalarVal] name [Double] val
*/


        public enum EHashType : byte
        {
            Null = 0,
            Dictionary = 1,
            List = 2,
            ListString = 3,
            ListDouble = 4,
            ListDouble2d = 5,
            MatrixDouble = 6,
            SeriesArray = 7,
            SeriesNormal = 8,
            ScalarString = 9,
            ScalarDate = 10,
            ScalarVal = 11,
            Double = 12,            
            String = 13,
            Date = 14,
            SeriesMetadata = 15,
            SeriesTraces = 16

        }

        public static void HashString(string input, SHA256 hash)
        {
            if (input == null) { HashNull(hash); return; }            
            byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(input.ToLowerInvariant());
            HashEnum1(EHashType.String, hash);
            HashInteger(stringBytes.Length, hash);            
            hash.TransformBlock(stringBytes, 0, stringBytes.Length, null, 0);
        }

        public static void HashDouble(double input, SHA256 hash)
        {                     
            HashEnum1(EHashType.Double, hash);            
            HashDouble2(input, hash);
        }

        public static void HashDate(GekkoTime input, SHA256 hash)
        {
            HashEnum1(EHashType.Date, hash);
            HashEnum2(input.freq, hash);
            HashInteger(input.super, hash);
            HashInteger(input.sub, hash);
            HashInteger(input.subsub, hash);
        }

        public static void HashDoubleArray(double[] input, SHA256 hash)
        {
            if (input == null) { HashNull(hash); return; }
            HashEnum1(EHashType.ListDouble, hash);
            HashInteger(input.Length, hash);
            int byteCount = input.Length * sizeof(double);
            byte[] buffer = new byte[byteCount];
            Buffer.BlockCopy(input, 0, buffer, 0, byteCount);
            hash.TransformBlock(buffer, 0, buffer.Length, null, 0);
        }

        public static void HashDoubleArray(double[,] input, SHA256 hash)
        {
            if (input == null) { HashNull(hash); return; }
            HashEnum1(EHashType.ListDouble2d, hash);
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);
            HashInteger(rows, hash);
            HashInteger(cols, hash);
            int byteCount = rows * cols * sizeof(double);
            byte[] buffer = new byte[byteCount];
            Buffer.BlockCopy(input, 0, buffer, 0, byteCount);
            hash.TransformBlock(buffer, 0, buffer.Length, null, 0);
        }

        public static void HashStringArray(string[] input, SHA256 hash)
        {
            if (input == null) { HashNull(hash); return; }
            HashEnum1(EHashType.ListString, hash);
            HashInteger(input.Length, hash);
            foreach (string s in input) HashString(s, hash);
        }

        public static void HashEnum1(EHashType value, SHA256 sha)
        {
            int intValue = (int)value;
            HashInteger(intValue, sha);
        }

        public static void HashInteger(int input, SHA256 hash)
        {            
            byte[] intBytes = BitConverter.GetBytes(input);            
            hash.TransformBlock(intBytes, 0, intBytes.Length, null, 0);
        }

        // ------------ privates ------------------

        private static void HashEnum2(EFreq value, SHA256 sha)
        {
            int intValue = (int)value;
            HashInteger(intValue, sha);
        }

        private static void HashDouble2(double value, SHA256 hash)
        {
            //We do not care about +0 and -0 differing, or different variants of NaN
            byte[] bytes = BitConverter.GetBytes(value);
            hash.TransformBlock(bytes, 0, bytes.Length, null, 0);
        }

        private static void HashNull(SHA256 hash)
        {
            byte[] nullBytes = BitConverter.GetBytes(-1);
            hash.TransformBlock(nullBytes, 0, nullBytes.Length, null, 0);
        }
    }
}
