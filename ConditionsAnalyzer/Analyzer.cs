﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConditionsAnalyzer
{

    public struct RangeResult
    {
        public bool isInRange;
        public string breachType;
        public string message;
    }
    public class Analyzer
    {
        IReporter reporter;
        static float temperatureLowerWarning;
        static float temperatureHigherWarning;
        static float temperatureLowerError;
        static float temperatureHigherError;

        static float humidityLowerWarning;
        static float humidityHigherWarning;
        static float humidityLowerError;
        static float humidityHigherError;

        public struct Conditions
        {
            public float temperature;
            public float humidity;
        }
        public Analyzer(IReporter reporter)
        {
            this.reporter = reporter;
            temperatureLowerWarning = 4;
            temperatureHigherWarning = 37;
            temperatureLowerError = 0;
            temperatureHigherError = 40;

            humidityLowerWarning = humidityLowerError = 0;
            humidityHigherWarning = 70;
            humidityHigherError = 90;
            

        }
        public static Conditions getConditions(string Data)
        {
            Conditions condition = new Conditions();
            string[] ConditionArray = Data.Split(' ');
            condition.temperature = float.Parse(ConditionArray[0]);
            condition.humidity = float.Parse(ConditionArray[1]);
            return condition;
        }
        public void Analyze(float value, string ConditionName, float WarningHighLevel, float WarningLowLevel, float ErrorHighLevel, float ErrorLowLevel)
        {
            RangeResult rangeResult = new RangeResult();
            rangeResult = CheckConditionsisInRange(value,ConditionName, WarningHighLevel, WarningLowLevel);
            if(!rangeResult.isInRange)
            {
                rangeResult.message = checkForBreachedCondition(value, ConditionName, ErrorHighLevel, ErrorLowLevel);
                reporter.sendMessage(rangeResult.message);
            }
            
        }
        public static RangeResult CheckConditionsisInRange(float value, string ConditionName, float WarningHighLevel, float WarningLowLevel)
        {
            RangeResult rangeResult = new RangeResult();
            rangeResult.isInRange = true;
            rangeResult.isInRange = !(value < WarningLowLevel || value > WarningHighLevel);
            return rangeResult;
        }
      
        public static string checkForBreachedCondition(float value, string ConditionName, float HigherExtremeCondition, float LowerextremeCondition)
        {
            bool ExtremeConditionCheck = (value < LowerextremeCondition || value > HigherExtremeCondition);
            string AlertType = ExtremeConditionCheck ? "ERROR" : "WARNING";
            string message = GenerateAnAlertMessage(ConditionName, AlertType);
            return message;
        }
        public static string GenerateAnAlertMessage(string ConditionName,string AlertType)
        {
            string message = $"{ConditionName} condition is at {AlertType} level";
            return message;   
        }

        public static void Main(string[] args)
        {
            SMSReporter reporter = new SMSReporter();
            Analyzer analyzer = new Analyzer(reporter);
            Conditions conditions = new Conditions();
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                conditions = getConditions(line);
                analyzer.Analyze(conditions.temperature,"Temperature",temperatureHigherWarning,temperatureLowerWarning,temperatureHigherError,temperatureLowerError);
                analyzer.Analyze(conditions.humidity, "Humidity", humidityHigherWarning,humidityLowerWarning,humidityHigherError,humidityLowerError);
            }
        }
    }
}