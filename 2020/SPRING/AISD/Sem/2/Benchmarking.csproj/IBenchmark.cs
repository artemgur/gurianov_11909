namespace Benchmarking
{
    public interface IBenchmark
    {
        double MeasureDurationInMs(ITask task, int repetitionCount);
    }
}