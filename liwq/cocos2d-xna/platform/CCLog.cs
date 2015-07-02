﻿using System;
using System.Diagnostics;

namespace cocos2d
{
    public class CCLog
    {
        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }
        public static void Log(string format, params object[] args)
        {
            Debug.WriteLine(format, args);
        }
    }
}
