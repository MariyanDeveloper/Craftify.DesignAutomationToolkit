using Craftify.DesignAutomationToolkit.Measurements;

namespace Craftify.DesignAutomationToolkit.Tests.Measurements;

public class TimedExecutionResultMeasurementTests
{
    [Fact]
    public async Task MeasureAsync_ForNonZeroTask_ReportsNonZeroDuration()
    {
        var measurement = new TimedExecutionResultMeasurement();
        
        var result = await measurement.MeasureAsync(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return true;
        });
        
        Assert.True(result.Duration.Seconds >= 2);
    }
    
    [Fact]
    public async Task MeasureAsync_WhenInnerTaskFails_ThrowsException()
    {
        var measurement = new TimedExecutionResultMeasurement();
        
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await measurement.MeasureAsync(async () =>
            {
                throw new InvalidOperationException("Test exception");
                return true;
            });
        });
    }
    
    [Fact]
    public async Task MeasureAsync_FromInnerTask_ReturnsCorrectValue()
    {
        var measurement = new TimedExecutionResultMeasurement();
        
        var expectedResult = "Hello, World!";
        
        var result = await measurement.MeasureAsync(async () =>
        {
            await Task.Delay(50);
            return expectedResult;
        });

        Assert.Equal(expectedResult, result.Result);
    }
}