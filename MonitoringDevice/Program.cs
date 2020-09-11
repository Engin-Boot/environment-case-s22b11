using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MonitoringDevice
{
    class MonitoringDevice
    {
        string[] buffer;
        static List<string> Temprature = new List<string>();
        static List<string> Humidity = new List<string>();
        void ReadingData()
        {
            buffer = System.IO.File.ReadAllLines("C:/Users/Aayush/Desktop/test.csv");
            if (buffer.Length==0)
            {
                //file empty
            }

        }

        

        void ProcessingData()
        {
            
           
            for(int i=1; i < buffer.Length; i++)
            {
                var value = buffer[i].Split(',');
                Temprature.Add(value[0]);
                Humidity.Add(value[1]);
            }

      

        }

        void SendingData()
        {
            foreach(var x in Temprature)
            {
                Console.WriteLine(x);
            }
            foreach (var x in Humidity)
            {
                Console.WriteLine(x);
            }

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
