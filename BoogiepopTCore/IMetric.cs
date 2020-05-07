namespace BoogiepopTCore
{
    /// <summary>
    /// Interface to show that a class defines a metric.
    /// </summary>
    /// <typeparam name="TObj">Object type which one defines the metric on.</typeparam>
    /// <typeparam name="TReal">A numeric return type for the metric e.g. double, int, long, BigInteger etc.</typeparam>
    public interface IMetric<TObj, TReal> where TReal : struct
    {
        TReal Distance(TObj A, TObj B);
    }
}
