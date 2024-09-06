using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Handle
{
    internal class ParseMatrix
    {
      
         List<int> Parse(string s,ref int start)
        {
            List<int> result = new List<int>();
            int i = start;
            for (; i < s.Length; )
            {
                if (char.IsDigit(s[i]) || s[i]=='-')
                {
                    result.Add(ReadNum(s,ref i));

                }
                if (s[i] == ']')
                {
                    ++i;
                    return result;
                } 
                else  ++i;
            }
            return result;
        }
        int ReadNum(string s, ref int i)
        {
            StringBuilder sb=new StringBuilder();

            do
            {
                sb.Append(s[i]);
                ++i;

            } while (char.IsDigit(s[i]));
            //++i;
            return int.Parse(sb.ToString());
        }

        public List<List<int>> Parse(string s)
        {

            int i = 0;
            List<List<int>> res = new List<List<int>>();
            for(;i<s.Length;++i)
            {
                if (s[i]=='[')
                {
                    ++i;
                    res.Add(Parse(s, ref i));
                    ++i;
                }    
            }

            return res;
        }



    }
}
