using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Timers;

namespace MonitoringDevice
{
    
    public class MonitoringDevice
    {
        private static System.Timers.Timer aTimer;
        public static string[] buffer;
        string[] temp;

        private static void SetTimer()
        {

            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        public void WriteToLogFile(string logFilePath,string logMessage)
        {
           
            FileStream outStream;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                outStream = new FileStream(logFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(outStream);
            }

            catch (Exception a)
            {
                Console.WriteLine(a.Message);
                return;
            }

            Console.SetOut(writer);
            Console.WriteLine(logMessage);
            Console.SetOut(oldOut);
            writer.Close();
            outStream.Close();
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            string file = "C:/Users/Aayush/Desktop/timeIssue.txt"; //File for the timer
            string message = string.Format("The Elapsed event was raised at {0:HH:mm:ss}", e.SignalTime);
            File.WriteAllText(file, message);

        }

        public int ReadingData(string filePath,string logFile)
        {
            try
            {
                buffer = System.IO.File.ReadAllLines(filePath);
                
                if (buffer.Length <= 1)
                {
                    WriteToLogFile(logFile,"EMPTY FILE");
                    return 0;
                }
                
            }
            catch(Exception e)
            {
                WriteToLogFile(logFile, "FILE NOT FOUND");
                Console.WriteLine(e.Message);
                
            }
            return 1;

        }

        public int FormatChecker(string logFile)
        {
            for(int i=1;i<buffer.Length;i++)
            {
               
                if (!(buffer[i].Contains(',')))
                {
                    WriteToLogFile(logFile,"DEVICE MALFUNCTION");
                    return 0;
                }
            }
            
            return 1;
        }
        public int ProcessingData()
        {
            int flag = 0;
            for(int i=1; i<buffer.Length; i++)  
            {   

                temp = buffer[i].Split(',');

                var missingValueIndicator = HandleMissingValues(temp);

                if ( missingValueIndicator == 1)
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
            for(int i=1;i<buffer.Length;i++)
            {
                aTimer.Start();

                Thread.Sleep(1200);

                if(!(buffer[i].Contains(',')))
                Console.WriteLine(buffer[i]);

                aTimer.Stop();
            }
            
            LastModified = System.IO.File.GetLastWriteTime(log);
            
 
            return LastModified;
            
        }

        

        static void Main(string[] args)
        {
            MonitoringDevice obj = new MonitoringDevice();

            string path = "C:/Users/Aayush/Desktop/test.csv";
            string logFile = "C:/Users/Aayush/Desktop/logfile.txt"; //file to log errors

            obj.ReadingData(path,logFile);

            int x = obj.FormatChecker(logFile);

            if (x == 1)
            {
                SetTimer();
                obj.ProcessingData();
                obj.SendingData(logFile);
                
            }
        }
    }
}
