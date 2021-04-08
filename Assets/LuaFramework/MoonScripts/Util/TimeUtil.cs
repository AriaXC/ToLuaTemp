using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

namespace MoonScrpts{

    public static class TimeUtil
    {
        private static Stopwatch stopwatch;

        //程序运行的时间  秒
        public static float timeSec;
        //程序运行的时间  毫秒
        public static float timeMsec;

        public static void Initialize()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public static void Update()
        {
            //获取运行的总秒数
            long value = stopwatch.ElapsedMilliseconds;
            timeSec = (float)value / 1000;
            timeMsec = Convert.ToInt32(value);
        }

        public static float GetTimeSec()
        {
            Update();
            return timeSec;
        }

        public static float GetTiemMsec()
        {
            Update();
            return timeMsec;
        }

    }

}