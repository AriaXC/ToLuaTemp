using System;
using UnityEngine;

namespace MoonScrpts
{
    // 日志堆栈信息
    public class LogData
    {
        //这些暂时没有用到

        //普通日志
        public const string Type_Log = "Log";
        //警告日志
        public const string Type_Warning = "Warning";
        //报错日志
        public const string Type_Error = "Error";
        //assert
        public const string Type_Assert = "Assert";
        //Exception
        public const string Type_Exception = "Exception";


        public string time;
        //暂时不显示
        public string type;
        public string msg;
        public string trace;

        //tosting 使用
        public string str;

        public static LogData Append(string str,string trace,string logType)
        {
            LogData log = new LogData();
            log.msg = str;
            log.trace = trace;
            log.time = DateTime.Now.ToString();
            log.type = logType;

            // 这里去写入文本日志中
            LogFileWriter.Append(log);
            return log;
        }


        public override string ToString()
        {
            if (str == null)
            {
                if (trace == null)
                {
                    str = string.Format("[{0}][{1}] {2}",type, time, msg);
                }
                else
                {
                    str = string.Format("[{0}][{1}] {2}{3}",type, time, msg,trace);
                }
            }
            return str;
        }
    }
}
