using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsBox.Handle
{
    internal class IpHandler
    {
        public static string GetBitWiseAnd(string ip,string mask)
        {

            string[] ips = ip.Split('.');
            var ipbytes = from i in ips select byte.Parse(i);
            int ipInt = BitConverter.ToInt32(ipbytes.ToArray(), 0);
            IEnumerable<byte> maskBytes;
            string result;
            if (mask.Contains('.'))
            {
                string[] masks = mask.Split('.');
                maskBytes = from m in masks select byte.Parse(m);
                int maskInt = BitConverter.ToInt32(maskBytes.ToArray(), 0);
                int bitwiseResult = ipInt & maskInt;
                var bytes = BitConverter.GetBytes(bitwiseResult);
                result= string.Join(".", bytes);
            }
            else
            {
                int count = int.Parse(mask);
                int maskInt = 0;
                int num = 1 << 31;
                for (int i = 0; i < count; i++)
                {

                    maskInt = (maskInt >> 1) | num;
                }
                int bitwiseResult = ipInt & maskInt;
                var bytes = BitConverter.GetBytes(bitwiseResult);
                result= string.Join(".", bytes);

            }
            return result;
        }
       
    }
}
