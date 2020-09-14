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

            aTimer = new System.Timers.Timer(1500);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Machine did not send the message on time");

        }
        public int ReadingData(string filePath)
        {
            buffer = System.IO.File.ReadAllLines(filePath);
            if (buffer.Length==0)
            {
                Console.WriteLine("FILE IS EMPTY");
                return 0;
            }
            return 1;

        }

        public int FormatChecker()
        {
            for(int i=1;i<buffer.Length;i++)
            {
               
                if (!(buffer[i].Contains(',')))
                {
                    Console.WriteLine("Device Malfunction");
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

                if (temp[0]=="")
                {
                    Console.WriteLine("Missing Temprature Data");
                    flag = 1;  
                    continue;
                }
                else if(temp[1]=="")
                {
                    Console.WriteLine("Missing Humidity Data");
                    flag = 1;
                    continue;
                }

                buffer[i] = temp[0] + " " + temp[1];
                
            }
            return flag;
        }

        public void SendingData()
        {
            for(int i=1;i<buffer.Length;i++)
            {
                aTimer.Start();

                Thread.Sleep(1000);

                if(!(buffer[i].Contains(',')))
                Console.WriteLine(buffer[i]);

                aTimer.Stop();
            }
            
        }

        

        static void Main(string[] args)
        {
            MonitoringDevice obj = new MonitoringDevice();

            string path = "../../../test.csv";

            obj.ReadingData(path);

            int x = obj.FormatChecker();

            if (x == 1)
            {
                SetTimer();
                obj.ProcessingData();
                obj.SendingData();
            }
        }
    }
}
