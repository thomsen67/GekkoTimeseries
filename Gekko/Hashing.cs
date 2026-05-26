using System;
using System.Security.Cryptography;

namespace Gekko
{
    public static class Hashing
    {
        /// <summary>
        /// Helper for hashing. Can handle null input.
        /// </summary>
        /// <param name="hash"></param>
        public static void HashDoubleArray(double[] input, SHA256 hash)
        {
            if (input == null) { Helper_HandleNull(hash); return; }
            byte[] doubleArrayBytes = new byte[input.Length * sizeof(double)];
            Buffer.BlockCopy(input, 0, doubleArrayBytes, 0, doubleArrayBytes.Length); //fast!
            HashInteger(input.Length, hash);
            Helper_HashByteArray(doubleArrayBytes, hash);
        }        

        public static void HashString(string input, SHA256 hash)
        {
            if (input == null) { Helper_HandleNull(hash); return; }            
            byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(input);
            HashInteger(stringBytes.Length, hash);     
            Helper_HashByteArray(stringBytes, hash);
        }

        public static void HashStringArray(string[] input, SHA256 hash)
        {
            if (input == null) { Helper_HandleNull(hash); return; }
            HashInteger(input.Length, hash);
            foreach (string s in input) HashString(s, hash);            
        }

        public static void HashInteger(int input, SHA256 hash)
        {
            byte[] intBytes = BitConverter.GetBytes(input);
            Helper_HashByteArray(intBytes, hash);
        }

        // =================================================================

        private static void Helper_HashByteArray(byte[] input, SHA256 hash)
        {
            hash.TransformBlock(input, 0, input.Length, null, 0);
        }

        private static void Helper_HandleNull(SHA256 hash)
        {
            byte[] nullBytes = BitConverter.GetBytes(-1);
            Helper_HashByteArray(nullBytes, hash);
            return;
        }
    }
}
