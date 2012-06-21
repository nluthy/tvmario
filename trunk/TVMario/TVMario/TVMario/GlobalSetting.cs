﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TVMario
{
    public class GlobalSetting
    {
        public static int GRAVITY = 2;  // Trọng lực
        public static int JUMP = 80;    // Độ cao nhảy
        public static int STEP_WIDTH = 2;   // Bước đi
        public static int COIN_TO_LIFE = 10;    // Số xu đủ đổi 1 mạng
        public static int INDEX_TEXTURE_JUMP = 7;   // Chỉ số khung hình khi nhảy
        public static int INDEX_TEXTURE_COIN = 8;   // Chỉ số khung hình đồng xu chuẩn
        public static int INDEX_TEXTURE_TRANSPARENT = 0;    // Chỉ số khung hình trong suốt
        
    }
}
