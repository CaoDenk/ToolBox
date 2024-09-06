using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.ColorLayer
{
    internal class BRGAColorMask
    {
        /*
         * 
         * 红：255，0，0
橙: 255,125,0
        绿：0，255，0
黄：255，255，0

蓝：0，0，255
靛: 0,255,255
紫: 255,0,255

        bRA
         * 
         */


        public static Vec4b RED = new Vec4b(0, 0, 255, 100);
        public static Vec4b ORANGE= new Vec4b(0, 155, 255, 100);
        public static Vec4b GREEN = new Vec4b(0, 255, 0, 100);
        public static Vec4b YELLOW= new Vec4b(0, 255, 255, 100);
        public static Vec4b BLUE = new Vec4b(255, 0, 0, 100);
       /* static Vec4b ALPHA = new Vec4b(0, 255, 0, 0)*/
      

        public static Vec4b GetRandMask()
        {

            Random random = new Random();
            int rand = random.Next() % 5;
            switch(rand)
            {
                case 0:
                    return RED;
                case 1:
                    return ORANGE;
                case 2:
                    return GREEN;
                case 3:
                    return YELLOW;
                default:
                    return BLUE;
               
            }
                


        }

    }
}
