using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Helpers
{
    public static class DESHelper
    {

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                var ms = new System.IO.MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                var str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            var inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                var ms = new System.IO.MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                var str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
    }
    ///<summary >
    /// MD5 Encrypt Class
    ///</summary> 
    public static class MD5Helper
    {
        //static state variables
        private static UInt32 A;
        private static UInt32 B;
        private static UInt32 C;
        private static UInt32 D;

        //number of bits to rotate in tranforming
        private const int S11 = 7;
        private const int S12 = 12;
        private const int S13 = 17;
        private const int S14 = 22;
        private const int S21 = 5;
        private const int S22 = 9;
        private const int S23 = 14;
        private const int S24 = 20;
        private const int S31 = 4;
        private const int S32 = 11;
        private const int S33 = 16;
        private const int S34 = 23;
        private const int S41 = 6;
        private const int S42 = 10;
        private const int S43 = 15;
        private const int S44 = 21;
        private static UInt32 F(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y) | ((~x) & z);
        }
        private static UInt32 G(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & z) | (y & (~z));
        }
        private static UInt32 H(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ y ^ z;
        }
        private static UInt32 I(UInt32 x, UInt32 y, UInt32 z)
        {
            return y ^ (x | (~z));
        }
        private static void FF(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + F(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }
        private static void GG(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + G(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }
        private static void HH(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + H(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }
        private static void II(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + I(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }
        private static void MD5_Init()
        {
            A = 0x67452301; //in memory, this is 0x01234567
            B = 0xefcdab89; //in memory, this is 0x89abcdef
            C = 0x98badcfe; //in memory, this is 0xfedcba98
            D = 0x10325476; //in memory, this is 0x76543210
        }
        private static UInt32[] MD5_Append(byte[] input)
        {
            int zeros = 0;
            int ones = 1;
            int size = 0;
            int n = input.Length;
            int m = n % 64;
            if (m < 56)
            {
                zeros = 55 - m;
                size = n - m + 64;
            }
            else if (m == 56)
            {
                zeros = 0;
                ones = 0;
                size = n + 8;
            }
            else
            {
                zeros = 63 - m + 56;
                size = n + 64 - m + 64;
            }

            ArrayList bs = new ArrayList(input);
            if (ones == 1)
            {
                bs.Add((byte)0x80); // 0x80 = $10000000
            }
            for (int i = 0; i < zeros; i++)
            {
                bs.Add((byte)0);
            }

            UInt64 N = (UInt64)n * 8;
            byte h1 = (byte)(N & 0xFF);
            byte h2 = (byte)((N >> 8) & 0xFF);
            byte h3 = (byte)((N >> 16) & 0xFF);
            byte h4 = (byte)((N >> 24) & 0xFF);
            byte h5 = (byte)((N >> 32) & 0xFF);
            byte h6 = (byte)((N >> 40) & 0xFF);
            byte h7 = (byte)((N >> 48) & 0xFF);
            byte h8 = (byte)(N >> 56);
            bs.Add(h1);
            bs.Add(h2);
            bs.Add(h3);
            bs.Add(h4);
            bs.Add(h5);
            bs.Add(h6);
            bs.Add(h7);
            bs.Add(h8);
            byte[] ts = (byte[])bs.ToArray(typeof(byte));

            /* Decodes input (byte[]) into output (UInt32[]). Assumes len is
            * a multiple of 4.
           */
            UInt32[] output = new UInt32[size / 4];
            for (Int64 i = 0, j = 0; i < size; j++, i += 4)
            {
                output[j] = (UInt32)(ts[i] | ts[i + 1] << 8 | ts[i + 2] << 16 | ts[i + 3] << 24);
            }
            return output;
        }
        private static UInt32[] MD5_Trasform(UInt32[] x)
        {

            UInt32 a, b, c, d;

            for (int k = 0; k < x.Length; k += 16)
            {
                a = A;
                b = B;
                c = C;
                d = D;

                /* Round 1 */
                FF(ref a, b, c, d, x[k + 0], S11, 0xd76aa478); /* 1 */
                FF(ref d, a, b, c, x[k + 1], S12, 0xe8c7b756); /* 2 */
                FF(ref c, d, a, b, x[k + 2], S13, 0x242070db); /* 3 */
                FF(ref b, c, d, a, x[k + 3], S14, 0xc1bdceee); /* 4 */
                FF(ref a, b, c, d, x[k + 4], S11, 0xf57c0faf); /* 5 */
                FF(ref d, a, b, c, x[k + 5], S12, 0x4787c62a); /* 6 */
                FF(ref c, d, a, b, x[k + 6], S13, 0xa8304613); /* 7 */
                FF(ref b, c, d, a, x[k + 7], S14, 0xfd469501); /* 8 */
                FF(ref a, b, c, d, x[k + 8], S11, 0x698098d8); /* 9 */
                FF(ref d, a, b, c, x[k + 9], S12, 0x8b44f7af); /* 10 */
                FF(ref c, d, a, b, x[k + 10], S13, 0xffff5bb1); /* 11 */
                FF(ref b, c, d, a, x[k + 11], S14, 0x895cd7be); /* 12 */
                FF(ref a, b, c, d, x[k + 12], S11, 0x6b901122); /* 13 */
                FF(ref d, a, b, c, x[k + 13], S12, 0xfd987193); /* 14 */
                FF(ref c, d, a, b, x[k + 14], S13, 0xa679438e); /* 15 */
                FF(ref b, c, d, a, x[k + 15], S14, 0x49b40821); /* 16 */

                /* Round 2 */
                GG(ref a, b, c, d, x[k + 1], S21, 0xf61e2562); /* 17 */
                GG(ref d, a, b, c, x[k + 6], S22, 0xc040b340); /* 18 */
                GG(ref c, d, a, b, x[k + 11], S23, 0x265e5a51); /* 19 */
                GG(ref b, c, d, a, x[k + 0], S24, 0xe9b6c7aa); /* 20 */
                GG(ref a, b, c, d, x[k + 5], S21, 0xd62f105d); /* 21 */
                GG(ref d, a, b, c, x[k + 10], S22, 0x2441453); /* 22 */
                GG(ref c, d, a, b, x[k + 15], S23, 0xd8a1e681); /* 23 */
                GG(ref b, c, d, a, x[k + 4], S24, 0xe7d3fbc8); /* 24 */
                GG(ref a, b, c, d, x[k + 9], S21, 0x21e1cde6); /* 25 */
                GG(ref d, a, b, c, x[k + 14], S22, 0xc33707d6); /* 26 */
                GG(ref c, d, a, b, x[k + 3], S23, 0xf4d50d87); /* 27 */
                GG(ref b, c, d, a, x[k + 8], S24, 0x455a14ed); /* 28 */
                GG(ref a, b, c, d, x[k + 13], S21, 0xa9e3e905); /* 29 */
                GG(ref d, a, b, c, x[k + 2], S22, 0xfcefa3f8); /* 30 */
                GG(ref c, d, a, b, x[k + 7], S23, 0x676f02d9); /* 31 */
                GG(ref b, c, d, a, x[k + 12], S24, 0x8d2a4c8a); /* 32 */

                /* Round 3 */
                HH(ref a, b, c, d, x[k + 5], S31, 0xfffa3942); /* 33 */
                HH(ref d, a, b, c, x[k + 8], S32, 0x8771f681); /* 34 */
                HH(ref c, d, a, b, x[k + 11], S33, 0x6d9d6122); /* 35 */
                HH(ref b, c, d, a, x[k + 14], S34, 0xfde5380c); /* 36 */
                HH(ref a, b, c, d, x[k + 1], S31, 0xa4beea44); /* 37 */
                HH(ref d, a, b, c, x[k + 4], S32, 0x4bdecfa9); /* 38 */
                HH(ref c, d, a, b, x[k + 7], S33, 0xf6bb4b60); /* 39 */
                HH(ref b, c, d, a, x[k + 10], S34, 0xbebfbc70); /* 40 */
                HH(ref a, b, c, d, x[k + 13], S31, 0x289b7ec6); /* 41 */
                HH(ref d, a, b, c, x[k + 0], S32, 0xeaa127fa); /* 42 */
                HH(ref c, d, a, b, x[k + 3], S33, 0xd4ef3085); /* 43 */
                HH(ref b, c, d, a, x[k + 6], S34, 0x4881d05); /* 44 */
                HH(ref a, b, c, d, x[k + 9], S31, 0xd9d4d039); /* 45 */
                HH(ref d, a, b, c, x[k + 12], S32, 0xe6db99e5); /* 46 */
                HH(ref c, d, a, b, x[k + 15], S33, 0x1fa27cf8); /* 47 */
                HH(ref b, c, d, a, x[k + 2], S34, 0xc4ac5665); /* 48 */

                /* Round 4 */
                II(ref a, b, c, d, x[k + 0], S41, 0xf4292244); /* 49 */
                II(ref d, a, b, c, x[k + 7], S42, 0x432aff97); /* 50 */
                II(ref c, d, a, b, x[k + 14], S43, 0xab9423a7); /* 51 */
                II(ref b, c, d, a, x[k + 5], S44, 0xfc93a039); /* 52 */
                II(ref a, b, c, d, x[k + 12], S41, 0x655b59c3); /* 53 */
                II(ref d, a, b, c, x[k + 3], S42, 0x8f0ccc92); /* 54 */
                II(ref c, d, a, b, x[k + 10], S43, 0xffeff47d); /* 55 */
                II(ref b, c, d, a, x[k + 1], S44, 0x85845dd1); /* 56 */
                II(ref a, b, c, d, x[k + 8], S41, 0x6fa87e4f); /* 57 */
                II(ref d, a, b, c, x[k + 15], S42, 0xfe2ce6e0); /* 58 */
                II(ref c, d, a, b, x[k + 6], S43, 0xa3014314); /* 59 */
                II(ref b, c, d, a, x[k + 13], S44, 0x4e0811a1); /* 60 */
                II(ref a, b, c, d, x[k + 4], S41, 0xf7537e82); /* 61 */
                II(ref d, a, b, c, x[k + 11], S42, 0xbd3af235); /* 62 */
                II(ref c, d, a, b, x[k + 2], S43, 0x2ad7d2bb); /* 63 */
                II(ref b, c, d, a, x[k + 9], S44, 0xeb86d391); /* 64 */

                A += a;
                B += b;
                C += c;
                D += d;
            }
            return new UInt32[] { A, B, C, D };
        }
        /// <summary>
        /// MD5 Encrypt Array
        /// </summary>
        /// <param name="input">the array as need to encrypt</param>
        /// <returns>encrypt code</returns>
        public static byte[] MD5Array(byte[] input)
        {
            MD5_Init();
            UInt32[] block = MD5_Append(input);
            UInt32[] bits = MD5_Trasform(block);

            /* Encodes bits (UInt32[]) into output (byte[]). Assumes len is
            * a multiple of 4.
            */
            byte[] output = new byte[bits.Length * 4];
            for (int i = 0, j = 0; i < bits.Length; i++, j += 4)
            {
                output[j] = (byte)(bits[i] & 0xff);
                output[j + 1] = (byte)((bits[i] >> 8) & 0xff);
                output[j + 2] = (byte)((bits[i] >> 16) & 0xff);
                output[j + 3] = (byte)((bits[i] >> 24) & 0xff);
            }
            return output;
        }
        /// <summary>
        /// get the Hex value of array
        /// </summary>
        /// <param name="array">the array as need to get the Hex value</param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public static string ArrayToHexString(byte[] array, bool uppercase)
        {
            string hexString = "";
            string format = "x2";
            if (uppercase)
            {
                format = "X2";
            }
            foreach (byte b in array)
            {
                hexString += b.ToString(format);
            }
            return hexString;
        }
        /// <summary>
        /// MD5 Enctypt String
        /// </summary>
        /// <param name="message">the string as need to encrypt</param>
        /// <returns>encrypt code</returns>
        public static string MDString(string message)
        {
            char[] c = message.ToCharArray();
            byte[] b = new byte[c.Length];
            for (int i = 0; i < c.Length; i++)
            {
                b[i] = (byte)c[i];
            }
            byte[] digest = MD5Array(b);
            return ArrayToHexString(digest, false);
        }
        /// <summary>
        /// Encrypt the file by MD5
        /// </summary>
        /// <param name="fileName">the path of file</param>
        /// <returns></returns>
        public static string MDFile(string fileName)
        {
            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
            byte[] array = new byte[fs.Length];
            fs.Read(array, 0, (int)fs.Length);
            byte[] digest = MD5Array(array);
            fs.Close();
            return ArrayToHexString(digest, false);
        }
    }
    /// <summary>
    /// RC2 Encrypt Class
    /// </summary>
    public class RC2
    {
        private byte[] key;
        private byte[] iv;
        private System.Text.ASCIIEncoding asciiEncoding;
        private System.Text.UnicodeEncoding textConverter;
        private RC2CryptoServiceProvider rc2CSP;
        public void Crypt()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            key = new byte[] { 106, 51, 25, 141, 157, 142, 23, 111, 234, 159, 187, 154, 215, 34, 37, 204 };
            iv = new byte[] { 135, 186, 133, 136, 184, 149, 153, 144 };
            asciiEncoding = new System.Text.ASCIIEncoding();
            textConverter = new System.Text.UnicodeEncoding();
            rc2CSP = new RC2CryptoServiceProvider();
        }
        /// <summary> 
        /// create a file whit 10261B size，to be used to svae the encrypt code.以便将加密数据写入固定大小的文件。 
        /// </summary> 
        /// <param name="filePath">the full path of file[inculd fileName]</param> 
        public string InitBinFile(string filePath)
        {
            byte[] tmp = new byte[10261];
            try //create file stream  which is used to save the content 
            {
                System.IO.FileStream writeFileStream = new FileStream(filePath,
                System.IO.FileMode.Create,
                System.IO.FileAccess.Write,
                System.IO.FileShare.None, 512, false);

                for (int i = 0; i < 10261; i++)
                    tmp[i] = 0;
                writeFileStream.Write(tmp, 0, 10261);
                writeFileStream.Flush();
                writeFileStream.Close();
            }
            catch (System.IO.IOException)
            {
                // MessageBox.Show("file error", "error！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ("Error,file operation error!");
            }
            return ("OK");
        }
        /// <summary> 
        /// 将文本数据加密后写入一个文件，其中，这个文件是用InitBinFile建立的，这个文件将被分成十块， 
        /// 用来分别保存10组不同的数据，第一个byte位保留，第2位到第21位分别用来存放每块数据的长度，但 
        /// 一个byte的取值为0-127，所以，用两个byte来存放一个长度。 
        /// </summary> 
        /// <param name="toEncryptText">要加密的文本数据</param> 
        /// <param name="filePath">要写入的文件</param> 
        /// <param name="dataIndex">写入第几块，取值为1--10</param> 
        /// <returns>是否操作成功</returns> 
        public bool EncryptToFile(string toEncryptText, string filePath, int dataIndex)
        {
            bool r = false;
            if (dataIndex > 10 && dataIndex < 1)
            {
                // MessageBox.Show("数据索引的取值范围在1至10之间！", "错误！",
                //MessageBoxButtons.OK, MessageBoxIcon.Error);
                return r;
            }
            byte[] encrypted;
            //打开要写入的文件，主要是为了保持原文件的内容不丢失 
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Open,
            System.IO.FileAccess.Read,
            System.IO.FileShare.None, 1024, true);

            byte[] index = new byte[10261];
            //将读取的内容写到byte数组 
            tmpFileStream.Read(index, 0, 10261);
            tmpFileStream.Close();
            //定义基本的加密转换运算 
            System.Security.Cryptography.ICryptoTransform Encryptor = rc2CSP.CreateEncryptor(this.key, this.iv);
            System.IO.MemoryStream msEncrypt = new MemoryStream();
            //在此加密转换流中，加密将从csEncrypt，加密后，结果在msEncrypt流中。 
            System.Security.Cryptography.CryptoStream csEncrypt = new CryptoStream(msEncrypt,
            Encryptor, CryptoStreamMode.Write);
            //将要加密的文本转换成UTF-16 编码，保存在tmp数组。 
            byte[] tmp = textConverter.GetBytes(toEncryptText);
            //将tmp输入csEncrypt,将通过Encryptor来加密。 
            csEncrypt.Write(tmp, 0, tmp.Length);
            //输出到msEnctypt 
            csEncrypt.FlushFinalBlock();
            //将流转成byte[] 
            encrypted = msEncrypt.ToArray();
            if (encrypted.Length > 1024)
            {
                //MessageBox.Show("加密后，数据长度大于1KB，无法保存");
                return false;
            }
            //得到加密后数据的大小，将结果存在指定的位置。 
            index[dataIndex * 2 - 1] = Convert.ToByte(Convert.ToString(encrypted.Length / 128));
            index[dataIndex * 2] = Convert.ToByte(Convert.ToString(encrypted.Length % 128));
            //将加密后的结果写入index（覆盖） 
            for (int i = 0; i < encrypted.Length; i++)
                index[1024 * (dataIndex - 1) + 21 + i] = encrypted[i];
            //建立文件流 
            tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Truncate,
            System.IO.FileAccess.Write,
            System.IO.FileShare.None, 1024, true);
            //写文件 
            tmpFileStream.Write(index, 0, 10261);
            tmpFileStream.Flush();
            r = true;
            tmpFileStream.Close();
            return r;
        }
        /// <summary> 
        /// 从一个文件中解密出一段文本，其中，这个文件是由InitBinFile建立的，并且由 EncryptToFile加密的 
        /// </summary> 
        /// <param name="filePath">要解密的文件</param> 
        /// <param name="dataIndex">要从哪一个块中解密</param> 
        /// <returns>解密后的文本</returns> 
        public string DecryptFromFile(string filePath, int dataIndex)
        {
            string r = "";
            if (dataIndex > 10 && dataIndex < 1)
            {
                //MessageBox.Show("数据索引的取值范围在1至10之间！", "错误！",
                //MessageBoxButtons.OK, MessageBoxIcon.Error);
                return r;
            }
            byte[] decrypted;
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Open,
            System.IO.FileAccess.Read,
            System.IO.FileShare.None, 1024, true);

            System.Security.Cryptography.ICryptoTransform Decryptor = rc2CSP.CreateDecryptor(this.key, this.iv);
            System.IO.MemoryStream msDecrypt = new MemoryStream();
            System.Security.Cryptography.CryptoStream csDecrypt = new CryptoStream(msDecrypt,
            Decryptor, CryptoStreamMode.Write);
            byte[] index = new byte[10261];

            tmpFileStream.Read(index, 0, 10261);
            int startIndex = 1024 * (dataIndex - 1) + 21;
            int count = index[dataIndex * 2 - 1] * 128 + index[dataIndex * 2];
            byte[] tmp = new byte[count];

            Array.Copy(index, 1024 * (dataIndex - 1) + 21, tmp, 0, count);
            csDecrypt.Write(tmp, 0, count);
            csDecrypt.FlushFinalBlock();
            decrypted = msDecrypt.ToArray();
            r = textConverter.GetString(decrypted, 0, decrypted.Length);
            tmpFileStream.Close();
            return r;
        }
        /**/
        /// <summary> 
        /// 将一段文本加密后保存到一个文件 
        /// </summary> 
        /// <param name="toEncryptText">要加密的文本数据</param> 
        /// <param name="filePath">要保存的文件</param> 
        /// <returns>是否加密成功</returns> 
        public bool EncryptToFile(string toEncryptText, string filePath)
        {
            bool r = false;
            byte[] encrypted;
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.OpenOrCreate,
            System.IO.FileAccess.Write,
            System.IO.FileShare.None, 1024, true);

            System.Security.Cryptography.ICryptoTransform Encryptor = rc2CSP.CreateEncryptor(this.key, this.iv);
            System.IO.MemoryStream msEncrypt = new MemoryStream();
            System.Security.Cryptography.CryptoStream csEncrypt = new CryptoStream(msEncrypt,
            Encryptor, CryptoStreamMode.Write);

            byte[] tmp = textConverter.GetBytes(toEncryptText);
            csEncrypt.Write(tmp, 0, tmp.Length);
            csEncrypt.FlushFinalBlock();
            encrypted = msEncrypt.ToArray();
            tmpFileStream.Write(encrypted, 0, encrypted.Length);
            tmpFileStream.Flush();
            r = true;
            tmpFileStream.Close();
            return r;
        }
        /// <summary> 
        /// 将一个被加密的文件解密 
        /// </summary> 
        /// <param name="filePath">要解密的文件</param> 
        /// <returns>解密后的文本</returns> 
        public string DecryptFromFile(string filePath)
        {
            string r = "";
            byte[] decrypted;
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Open,
            System.IO.FileAccess.Read,
            System.IO.FileShare.None, 1024, true);
            System.Security.Cryptography.ICryptoTransform Decryptor = rc2CSP.CreateDecryptor(this.key, this.iv);
            System.IO.MemoryStream msDecrypt = new MemoryStream();
            System.Security.Cryptography.CryptoStream csDecrypt = new CryptoStream(msDecrypt,
            Decryptor, CryptoStreamMode.Write);

            byte[] tmp = new byte[tmpFileStream.Length];
            tmpFileStream.Read(tmp, 0, tmp.Length);
            csDecrypt.Write(tmp, 0, tmp.Length);
            csDecrypt.FlushFinalBlock();
            decrypted = msDecrypt.ToArray();
            r = textConverter.GetString(decrypted, 0, decrypted.Length);
            tmpFileStream.Close();
            return r;
        }
        //------------------------------------------------------------- 
        /**/
        /// <summary> 
        /// 将文本数据加密后写入一个文件，其中，这个文件是用InitBinFile建立的，这个文件将被分成十块， 
        /// 用来分别保存10组不同的数据，第一个byte位保留，第2位到第21位分别用来存放每块数据的长度，但 
        /// 一个byte的取值为0-127，所以，用两个byte来存放一个长度。 
        /// </summary> 
        /// <param name="toEncryptText">要加密的文本数据</param> 
        /// <param name="filePath">要写入的文件</param> 
        /// <param name="dataIndex">写入第几块，取值为1--10</param> 
        /// <param name="IV">初始化向量</param> 
        /// <param name="Key">加密密匙</param> 
        /// <returns>是否操作成功</returns> 
        public bool EncryptToFile(string toEncryptText, string filePath, int dataIndex, byte[] IV, byte[] Key)
        {
            bool r = false;
            if (dataIndex > 10 && dataIndex < 1)
            {
                //MessageBox.Show("数据索引的取值范围在1至10之间！", "错误！",
                //MessageBoxButtons.OK, MessageBoxIcon.Error);
                return r;
            }
            byte[] encrypted;
            //打开要写入的文件，主要是为了保持原文件的内容不丢失 
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Open,
            System.IO.FileAccess.Read,
            System.IO.FileShare.None, 1024, true);

            byte[] index = new byte[10261];
            //将读取的内容写到byte数组 
            tmpFileStream.Read(index, 0, 10261);
            tmpFileStream.Close();
            //定义基本的加密转换运算 
            System.Security.Cryptography.ICryptoTransform Encryptor = rc2CSP.CreateEncryptor(Key, IV);
            System.IO.MemoryStream msEncrypt = new MemoryStream();
            //在此加密转换流中，加密将从csEncrypt，加密后，结果在msEncrypt流中。 
            System.Security.Cryptography.CryptoStream csEncrypt = new CryptoStream(msEncrypt,
            Encryptor, CryptoStreamMode.Write);
            //将要加密的文本转换成UTF-16 编码，保存在tmp数组。 
            byte[] tmp = textConverter.GetBytes(toEncryptText);
            //将tmp输入csEncrypt,将通过Encryptor来加密。 
            csEncrypt.Write(tmp, 0, tmp.Length);
            //输出到msEnctypt 
            csEncrypt.FlushFinalBlock();
            //将流转成byte[] 
            encrypted = msEncrypt.ToArray();
            if (encrypted.Length > 1024)
            {
                //MessageBox.Show("加密后，数据长度大于1KB，无法保存");
                return false;
            }
            //得到加密后数据的大小，将结果存在指定的位置。 
            index[dataIndex * 2 - 1] = Convert.ToByte(Convert.ToString(encrypted.Length / 128));
            index[dataIndex * 2] = Convert.ToByte(Convert.ToString(encrypted.Length % 128));
            //将加密后的结果写入index（覆盖） 
            for (int i = 0; i < encrypted.Length; i++)
                index[1024 * (dataIndex - 1) + 21 + i] = encrypted[i];
            //建立文件流 
            tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Truncate,
            System.IO.FileAccess.Write,
            System.IO.FileShare.None, 1024, true);
            //写文件 
            tmpFileStream.Write(index, 0, 10261);
            tmpFileStream.Flush();
            r = true;
            tmpFileStream.Close();
            return r;
        }
        /// <summary> 
        /// 从一个文件中解密出一段文本，其中，这个文件是由InitBinFile建立的，并且由 EncryptToFile加密的 
        /// </summary> 
        /// <param name="filePath">要解密的文件</param> 
        /// <param name="dataIndex">要从哪一个块中解密</param> 
        /// <param name="IV">初始化向量</param> 
        /// <param name="Key">解密密匙</param> 
        /// <returns>解密后的文本</returns> 
        public string DecryptFromFile(string filePath, int dataIndex, byte[] IV, byte[] Key)
        {
            string r = "";
            if (dataIndex > 10 && dataIndex < 1)
            {
                // MessageBox.Show("数据索引的取值范围在1至10之间！", "错误！",
                //MessageBoxButtons.OK, MessageBoxIcon.Error);
                return r;
            }
            byte[] decrypted;
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Open,
            System.IO.FileAccess.Read,
            System.IO.FileShare.None, 1024, true);

            System.Security.Cryptography.ICryptoTransform Decryptor = rc2CSP.CreateDecryptor(Key, IV);
            System.IO.MemoryStream msDecrypt = new MemoryStream();
            System.Security.Cryptography.CryptoStream csDecrypt = new CryptoStream(msDecrypt,
            Decryptor, CryptoStreamMode.Write);
            byte[] index = new byte[10261];

            tmpFileStream.Read(index, 0, 10261);
            int startIndex = 1024 * (dataIndex - 1) + 21;
            int count = index[dataIndex * 2 - 1] * 128 + index[dataIndex * 2];
            byte[] tmp = new byte[count];

            Array.Copy(index, 1024 * (dataIndex - 1) + 21, tmp, 0, count);
            csDecrypt.Write(tmp, 0, count);
            csDecrypt.FlushFinalBlock();
            decrypted = msDecrypt.ToArray();
            r = textConverter.GetString(decrypted, 0, decrypted.Length);
            tmpFileStream.Close();
            return r;
        }
        /// <summary> 
        /// 将一段文本加密后保存到一个文件 
        /// </summary> 
        /// <param name="toEncryptText">要加密的文本数据</param> 
        /// <param name="filePath">要保存的文件</param> 
        /// <param name="IV">初始化向量</param> 
        /// <param name="Key">加密密匙</param> 
        /// <returns>是否加密成功</returns> 
        public bool EncryptToFile(string toEncryptText, string filePath, byte[] IV, byte[] Key)
        {
            bool r = false;
            byte[] encrypted;
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.OpenOrCreate,
            System.IO.FileAccess.Write,
            System.IO.FileShare.None, 1024, true);

            System.Security.Cryptography.ICryptoTransform Encryptor = rc2CSP.CreateEncryptor(Key, IV);
            System.IO.MemoryStream msEncrypt = new MemoryStream();
            System.Security.Cryptography.CryptoStream csEncrypt = new CryptoStream(msEncrypt,
            Encryptor, CryptoStreamMode.Write);

            byte[] tmp = textConverter.GetBytes(toEncryptText);
            csEncrypt.Write(tmp, 0, tmp.Length);
            csEncrypt.FlushFinalBlock();
            encrypted = msEncrypt.ToArray();
            tmpFileStream.Write(encrypted, 0, encrypted.Length);
            tmpFileStream.Flush();
            r = true;
            tmpFileStream.Close();
            return r;
        }
        /// <summary> 
        /// 将一个被加密的文件解密 
        /// </summary> 
        /// <param name="filePath">要解密的文件</param> 
        /// <param name="IV">初始化向量</param> 
        /// <param name="Key">解密密匙</param> 
        /// <returns>解密后的文本</returns> 
        public string DecryptFromFile(string filePath, byte[] IV, byte[] Key)
        {
            string r = "";
            byte[] decrypted;
            System.IO.FileStream tmpFileStream = new FileStream(filePath,
            System.IO.FileMode.Open,
            System.IO.FileAccess.Read,
            System.IO.FileShare.None, 1024, true);
            System.Security.Cryptography.ICryptoTransform Decryptor = rc2CSP.CreateDecryptor(Key, IV);
            System.IO.MemoryStream msDecrypt = new MemoryStream();
            System.Security.Cryptography.CryptoStream csDecrypt = new CryptoStream(msDecrypt,
            Decryptor, CryptoStreamMode.Write);

            byte[] tmp = new byte[tmpFileStream.Length];
            tmpFileStream.Read(tmp, 0, tmp.Length);
            csDecrypt.Write(tmp, 0, tmp.Length);
            csDecrypt.FlushFinalBlock();
            decrypted = msDecrypt.ToArray();
            r = textConverter.GetString(decrypted, 0, decrypted.Length);
            tmpFileStream.Close();
            return r;
        }
        /// <summary> 
        /// 设置加密或解密的初始化向量 
        /// </summary> 
        /// <param name="s">长度等于8的ASCII字符集的字符串</param> 
        public void SetIV(string s)
        {
            if (s.Length != 8)
            {
                // MessageBox.Show("输入的字符串必须为长度为8的且属于ASCII字符集的字符串");
                this.iv = null;
                return;
            }
            try
            {
                this.iv = this.asciiEncoding.GetBytes(s);
            }
            catch (System.Exception)
            {
                // MessageBox.Show("输入的字符串必须为长度为8的且属于ASCII字符集的字符串");
                this.iv = null;
            }
        }
        /// <summary> 
        /// 设置加密或解密的密匙 
        /// </summary> 
        /// <param name="s">长度等于16的ASCII字符集的字符串</param> 
        public void SetKey(string s)
        {
            if (s.Length != 16)
            {
                // MessageBox.Show("输入的字符串必须为长度为16的且属于ASCII字符集的字符串");
                this.key = null;
                return;
            }
            try
            {
                this.key = this.asciiEncoding.GetBytes(s);
            }
            catch (System.Exception)
            {
                //MessageBox.Show("输入的字符串必须为长度为16的且属于ASCII字符集的字符串");
                this.key = null;
            }
        }
    }
}
