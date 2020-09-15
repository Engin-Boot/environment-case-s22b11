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
        }
    }
    public class EmailReporter : IReporter
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"Email Reporter :  {message}");
        }
    }
}