using UnityEngine;
using System.IO;
using System.Threading;

namespace UEasyUI
{
    public class LogWrite : MonoBehaviour
    {
        public bool IsWriteLog = true;

        public static LogWrite Instance;
        private string outpath;
        private StreamWriter writer;
        // Use this for initialization
        private int mainThreadId = -1;

//        private string httpAddress = "";
//        private string loginAccount = "";

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            if (!this.IsWriteLog)
                return;

            string curTime = System.DateTime.Now.ToString("yyyyMMddhhmmss");
            outpath = Application.persistentDataPath + "/Log_" + curTime + ".txt";

            //每次启动客户端删除之前保存的Log
            if (File.Exists(outpath))
                File.Delete(outpath);

            writer = new StreamWriter(outpath, true, System.Text.Encoding.UTF8);
            writer.AutoFlush = true;
            mainThreadId = Thread.CurrentThread.ManagedThreadId;

            Application.logMessageReceived += OnLogMessageReceived;
            Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
        }

        public void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
        {
            if (this.mainThreadId != Thread.CurrentThread.ManagedThreadId)
                return;

            if (logType == LogType.Assert)
                logType = LogType.Error;

            try
            {
                string curTime = System.DateTime.Now.TimeOfDay.ToString();

                writer.WriteLine(curTime);
                writer.WriteLine(logMessage);
                writer.WriteLine(stackTrace);
                writer.Flush();
            }
            catch (System.Exception e)
            {
                Log.Warning("OnLogMessageReceived Catch exception = {0}", e.ToString());
            }
        }

        public void OnLogMessageReceivedFmod(string logMessage, LogType logType)
        {

            if (null == writer)
                return;

            try
            {
                string curTime = System.DateTime.Now.TimeOfDay.ToString();

                writer.WriteLine(curTime);
                writer.WriteLine(logMessage);
                writer.Flush();
            }
            catch (System.Exception e)
            {
                Log.Warning("OnLogMessageReceived Catch exception = {0}", e.ToString());
            }
        }


        public void OnLogMessageReceivedThreaded(string logMessage, string stackTrace, LogType logType)
        {
            if (this.mainThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                return;
            }

            if (logType == LogType.Assert)
            {
                logType = LogType.Error;
            }

            try
            {
                string curTime = System.DateTime.Now.TimeOfDay.ToString();

                writer.WriteLine(curTime + " Threaded log");
                writer.WriteLine(logMessage);
                writer.WriteLine(stackTrace);
                writer.Flush();

                //SimpleWebRequest.Post(httpAddress, WebRequestID.DebugLog, "\n account=" + loginAccount + "\n" + curTime + "\n" + logMessage + "\n" + stackTrace);
            }
            catch (System.Exception e)
            {
                Log.Warning("OnLogMessageReceivedThreaded Catch exception = {0}", e.ToString());
            }
        }

        private void OnDestroy()
        {
            if (Debug.isDebugBuild)
            {
                Application.logMessageReceived -= OnLogMessageReceived;
                Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;
            }

            writer.Close();
        }
    }
}

