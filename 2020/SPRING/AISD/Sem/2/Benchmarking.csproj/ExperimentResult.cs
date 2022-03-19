namespace Benchmarking
{
    public class ExperimentResult
    {
        public int InputSize;
        public double AverageResult;

        public ExperimentResult(int inputSize, double averageResult)
        {
            InputSize = inputSize;
            AverageResult = averageResult;
        }
    }
}