using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TVMario
{
    public class GlobalSetting
    {
        public static int GRAVITY = 2;  // Trọng lực
        public static int JUMP = 85;    // Độ cao tối đa nhảy đc
        public static int JUMP_STEP = 5;    // Độ cao 1 bước nhảy
        public static int STEP_WIDTH = 2;   // Bước đi
        public static int COIN_TO_LIFE = 10;    // Số xu đủ đổi 1 mạng
        public static int INDEX_TEXTURE_JUMP = 7;   // Chỉ số khung hình khi nhảy
        public static int INDEX_TEXTURE_COIN = 8;   // Chỉ số khung hình đồng xu chuẩn
        public static int INDEX_TEXTURE_TRANSPARENT = 0;    // Chỉ số khung hình trong suốt
        public static int MONSTER_STEP = 1;   // Độ dài 1 bước đi của monster
        
    }
}
