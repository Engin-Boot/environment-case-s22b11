using System;

namespace ConditionsAnalyzer
{
    public interface IReporter
    {
        void sendMessage(string message);
    }

    public class SMSReporter : IReporter
    {
        public void sendMessage(string message)
        {
            Console.WriteLine($"SMS Reporter :  {message}");
        }
    }
    public class EmailReporter : IReporter
    {
        public void sendMessage(string message)
        {
            Console.WriteLine($"Email Reporter :  {message}");
        }
    }
}