using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;

namespace MonitoringDevice
{
    
        public class MonitoringDevice
        {
        private static System.Timers.Timer aTimer;
        public  static string[] buffer;
        string[] temp;

        public static void SetTimer()
        {

            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            string file = "../../../Test Files/timeIssue.txt"; //File for the timer
            string message = string.Format("The Elapsed event was raised at {0:HH:mm:ss}", e.SignalTime);
            File.WriteAllText(file, message);

        }

        public int ReadingData(string filePath, string logFile)
        {
            
            try
            {
                buffer = System.IO.File.ReadAllLines(filePath);

                if (buffer.Length <= 1)
                {
                    LogFileLibrary.LogFile obj = new LogFileLibrary.LogFile(logFile, "EMPTY FILE");
                    obj.WriteToLogFile();

                    return 0;
                    
                }

            }
            catch (Exception e)
            {
                LogFileLibrary.LogFile obj = new LogFileLibrary.LogFile(logFile, "FILE NOT FOUND"); 
                obj.WriteToLogFile();
                Console.WriteLine(e.Message);

            }
            return 1;

        }

        public int FormatChecker(string logFile)
        {
            for (int i = 1; i < buffer.Length; i++)
            {

                if (!(buffer[i].Contains(',')))
                {
                    LogFileLibrary.LogFile obj = new LogFileLibrary.LogFile(logFile, "DEVICE MALFUNCTION");
                    obj.WriteToLogFile();
                    return 0;
                }
            }

            return 1;
        }
        public int ProcessingData()
        {
            int flag = 0;
            for (int i = 1; i < buffer.Length; i++)
            {

                temp = buffer[i].Split(',');

                var missingValueIndicator = HandleMissingValues(temp);

                if (missingValueIndicator == 1)
                {

                    flag = 1;
                    continue;
                }

                buffer[i] = temp[0] + " " + temp[1];

            }

            return flag;
        }
        public int HandleMissingValues(string[] tempBuf)
        {
            if ((tempBuf[0] == "") || (tempBuf[1] == ""))
            {
                return 1;
            }

            return 0;
        }
        public DateTime SendingData(string log)
        {
            DateTime LastModified;
            for (int i = 1; i < buffer.Length; i++)
            {
                aTimer.Start();

                Thread.Sleep(1200);

                if (!(buffer[i].Contains(','))) 
                    Console.WriteLine(buffer[i]);

                aTimer.Stop();
            }

            LastModified = System.IO.File.GetLastWriteTime(log);


            return LastModified;

        }



        static void Main(string[] args)
        {
            MonitoringDevice obj = new MonitoringDevice();

            string path = "../../../Test Files/test.csv";
            string logFile = "../../../Test Files/logfile.txt"; //file to log errors

            obj.ReadingData(path, logFile);

            obj.FormatChecker(logFile);

            SetTimer();

            obj.ProcessingData();

            obj.SendingData(logFile);

            
            
        }
    }      
}

