using System;
using Xunit;
using ConditionsAnalyzer;

namespace ConditionsAnalyzerTest
{
    public class ConditionsAnalyerTest
    {
        [Fact]
        public void TestforGetConditionValues()
        {
            string Conditiondata = "25.7 50";
            Analyzer.Conditions conditions = Analyzer.getConditions(Conditiondata);
            Assert.True(conditions.temperature.Equals(25.7F));
            Assert.True(conditions.humidity.Equals(50F));
        }

        [Fact]
        public void WhenConditionsisNotInRange()
        {
            RangeResult TemperatureRangeResult = Analyzer.CheckConditionsisInRange(45, "Temperature", 37, 4);
            RangeResult HumidityRangeResult = Analyzer.CheckConditionsisInRange(75, "Humidity", 70, 0);
            Assert.False(TemperatureRangeResult.isInRange);
            Assert.False(HumidityRangeResult.isInRange);

        }
        [Fact]
        public void WhenConditionsisInRange()
        {
            RangeResult TemperatureRangeResult = Analyzer.CheckConditionsisInRange(35, "Temperature", 37, 4);
            RangeResult HumidityRangeResult = Analyzer.CheckConditionsisInRange(68, "Humidity", 70, 0);
            Assert.True(TemperatureRangeResult.isInRange);
            Assert.True(HumidityRangeResult.isInRange);
        }
        [Fact]
        public void WhenConditionisInWarningLevels()
        {
            string TemperatureMessage = Analyzer.checkForBreachedCondition(39, "Temperature", 40, 0);
            string HumidityMessage = Analyzer.checkForBreachedCondition(72, "Humidity", 90, 0);
            Assert.True(TemperatureMessage.Equals("Temperature condition is at WARNING level"));
            Assert.True(HumidityMessage.Equals("Humidity condition is at WARNING level"));
        }

        [Fact]
        public void WhenConditionisInErrorLevels()
        {
            string TemperatureMessage = Analyzer.checkForBreachedCondition(42, "Temperature", 40, 0);
            string HumidityMessage = Analyzer.checkForBreachedCondition(95, "Humidity", 90, 0);
            Assert.True(TemperatureMessage.Equals("Temperature condition is at ERROR level"));
            Assert.True(HumidityMessage.Equals("Humidity condition is at ERROR level"));
        }
        
    }
}
