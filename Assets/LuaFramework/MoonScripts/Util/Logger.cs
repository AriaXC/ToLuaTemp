using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;


namespace MoonScrpts
{
    //绑定日志相关的
    public static class Logger 
    {
        public static void Initialize()
        {
            Application.logMessageReceived += UnityLogCallBack;
        }

        private static void UnityLogCallBack(string condition, string stackTrace, LogType type)
        {
            string logType = "";
            switch (type)
            {
                case LogType.Exception:
                    logType = LogData.Type_Exception;
                    break;
                case LogType.Assert:
                    logType = LogData.Type_Assert;
                    break;
                case LogType.Error:
                    logType = LogData.Type_Error;
                    break;
                case LogType.Warning:
                    logType = LogData.Type_Warning;
                    break;
                case LogType.Log:
                    stackTrace = null;
                    logType = LogData.Type_Log;
                    break;
            }
            LogData.Append(condition, stackTrace, logType);
             
        }

        public static void Log(string str, string trace = null)
        {
            LogData log = LogData.Append(str, trace, LogData.Type_Log);
            Debug.Log(log);
        }

        public static void LogWarning(string str, string trace = null)
        {
            LogData log = LogData.Append(str, trace, LogData.Type_Warning);
            Debug.LogWarning(log);
        }

        /// <summary>
        /// 记录了lua堆栈的日志
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trace"></param>
        public static void LogError(string str, string trace = null)
        {
            LogData log = LogData.Append(str, trace, LogData.Type_Error);
            //参数需要是个obj  调用这个obj的tostring方法
            Debug.LogError(log);
        }
    }
}
