using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBComputing.util
{
    class MemoryTranslation
    {
        static readonly System.Collections.Hashtable REVERSE_STACKOPS = new System.Collections.Hashtable();

        static readonly char[] HEX = "0123456789ABCDEF".ToCharArray();
        static readonly byte[] REVERSE_HEX;
        static MemoryTranslation()
        {
            REVERSE_HEX = new byte[256];
            for (byte i = 0; i < HEX.Length; i++)
            {
                REVERSE_HEX[HEX[i]] = i;
            }

            for (int i = 0; i < StackOpCodeTranslate.NAMES.Length; i++)
            {
                string name = StackOpCodeTranslate.NAMES[i];
                REVERSE_STACKOPS.Add(name, i);
            }
        }

        public static byte[] HEXtoBytes(int size, string value)
        {
            byte[] bytes = new byte[size];

            string s = string.Copy(value);
            s = s.Replace(" ", "").Replace("\n", "").Replace("\r", "").ToUpper().Trim();

            int w = 0;
            int h = 0;
            byte b = 0;
            for (int r = 0; r < s.Length; r++)
            {
                char c = s[r];
                if (c >= 'a' && c <= 'z')
                {
                    c = (char)(c - 'a' + 'A');
                }
                if (!HEX.Contains(c))
                {
                    continue;
                }
                if (h == 0)
                {
                    //b = 0;
                    b = REVERSE_HEX[c];
                    h = 1;
                }
                else
                if (h == 1)
                {
                    b <<= 4;
                    b |= REVERSE_HEX[c];
                    bytes[w] = b;
                    w++;
                    h = 0;
                }
            }
            return bytes;
        }

        public static string bytesToHEX(byte[] bytes)
        {
            //return "test string\n more text";
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i != 0)
                {
                    if (i % 256 == 0)
                    {
                        sb.Append("\n\n\n\n");
                    }
                    else
                    if (i % 64 == 0)
                    {
                        sb.Append("\n\n");
                    }
                    else
                    if (i % 8 == 0)
                    {
                        sb.Append("\n");
                    }
                    else
                    if (i % 4 == 0)
                    {
                        sb.Append(" ");
                    }
                }
                byte b = bytes[i];
                sb.Append((char)HEX[(b >> 4) & 0x0F]);
                sb.Append((char)HEX[(b >> 0) & 0x0F]);
                sb.Append(" ");
            }
            return sb.ToString().Trim();
        }

        public static string bytesToStackOps(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                // StackOpCodeTranslate.NAMES
                sb.Append(StackOpCodeTranslate.NAMES[(int)(b >> 0) & 0x0F]);
                sb.Append("\n");
                sb.Append(StackOpCodeTranslate.NAMES[(int)(b >> 4) & 0x0F]);
                sb.Append("\n");
                //sb.Append(Enum.GetName(typeof(StackOpCodes), HEX[(b >> 0) & 0x0F]));
                //sb.Append("\n");
                //sb.Append(Enum.GetName(typeof(StackOpCodes), HEX[(b >> 4) & 0x0F]));
                //sb.Append("\n");
            }
            return sb.ToString().Trim();
        }
        public static void Main(string[] args)
        {
            System.Console.Out.WriteLine("dfsd");
            byte[] bb = new byte[] { 0,1,2,3,4,5,6,7};
            System.Console.Out.WriteLine(bytesToStackOps(bb));
            System.Console.Out.WriteLine("dfsd");
        }
        public static byte[] StackOpsToBytes(int size, string value)
        {
            byte[] bytes = new byte[size];

            string[] lines = value.Trim().Split('\n');
            int w = 0;
            int h = 0;
            byte b = 0;
            for (int i = 0; i < lines.Length && w < size; i++)
            {
                string s = lines[i].Trim().ToLower();
                if (s.Length <= 0)
                {
                    continue;
                }
                if (h == 0)
                {
                    b = (byte)REVERSE_STACKOPS[s];
                    h = 1;
                }
                else
                {
                    byte c = (byte)REVERSE_STACKOPS[s];
                    b |= (byte)(c << 4);
                    bytes[w++] = b;
                    h = 0;
                }
            }
            return bytes;
        }
    }
}
