using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MonitoringDevice
{
    public class MonitoringDevice
    {
        string[] buffer;
        string[] temp;

       
        public int ReadingData()
        {
            buffer = System.IO.File.ReadAllLines("C:/Users/Aayush/Desktop/test.csv");
            if (buffer.Length==0)
            {
                Console.WriteLine("FILE IS EMPTY");
                return 0;
            }
            return 1;

        }

        
        public int ProcessingData()
        {
            
            for(int i=1; i<buffer.Length; i++)
            {
                if (buffer[i].IndexOf(',') != 2)
                {
                    Console.WriteLine("File not in correct format");
                    return 0;
                }

                temp = buffer[i].Split(',');
                buffer[i] = temp[0] + " " + temp[1];
            }
            return 1;
        }

        public int SendingData()
        {

            int flag = 1;
            for(int i=1;i<buffer.Length;i++)
            {
                
                if ((buffer[i].Substring(0, 1)) == " ")
                {
                    Console.WriteLine("Missing Temprature value");
                    flag = 0;
                    continue;
                }

                else if ((buffer[i].Length) == 3)
                {
                    Console.WriteLine("Missing Humidity value");
                    flag = 0;
                    continue;
                }
                   
                Console.WriteLine(buffer[i]);
            }
            return flag;
        }

        static void Main(string[] args)
        {
            MonitoringDevice obj = new MonitoringDevice();
            obj.ReadingData();
            obj.ProcessingData();
            obj.SendingData();
        }
    }
}
