using System;

namespace ConditionsAnalyzer
{
    public interface IReporter
    {
        void SendMessage(string message);
    }

    public class SMSReporter : IReporter
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"SMS Reporter :  {message}");
            LogFileLibrary.LogFile obj = new LogFileLibrary.LogFile("../../../logfile.txt", message);
            obj.WriteToLogFile();
        }
    }
    public class EmailReporter : IReporter
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"Email Reporter :  {message}");
            LogFileLibrary.LogFile obj = new LogFileLibrary.LogFile("../../../logfile.txt", message);
            obj.WriteToLogFile();
        }
    }
}