namespace BoogiepopTCore
{
    /// <summary>
    /// Defines a general format one can use to define a metric on a set of objects of a certain type.
    /// </summary>
    /// <typeparam name="TObj">
    /// Object type which one defines the metric on.
    /// </typeparam>
    /// <typeparam name="TExt">
    /// Datatype which contains extracted information from TObj.
    /// Elements of this type should be an element of a normed vector space.
    /// </typeparam>
    /// <typeparam name="TReal">
    /// The return type of the metric, should be numeric e.g. double, int, long, BigInteger etc.
    /// </typeparam>
    public abstract class ExtractMetric<TObj, TExt, TReal> : IMetric<TObj, TReal> where TReal : struct
    {
        /// <summary>
        /// Extract some information from A e.g. a statistic or compute a related quantity.
        /// </summary>
        /// <param name="A">
        /// Object which to extract from.
        /// </param>
        /// <returns>
        /// Extracted information from A, which can be compared.
        /// </returns>
        protected abstract TExt Extract(TObj A);

        /// <summary>
        /// Since the elements of TExt should make up a vector space, this is meant to mean sA - sB.
        /// </summary>
        /// <param name="sA">
        /// Extracted information from A
        /// </param>
        /// <param name="sB">
        /// Extracted information from B.
        /// </param>
        /// <returns>
        /// The difference sA - sB.
        /// </returns>
        protected abstract TExt Subtract(TExt sA, TExt sB);

        /// <summary>
        /// This defines the norm used on the elements of type TExt.
        /// </summary>
        /// <param name="sC">
        /// An element of type TExt, usually sA - sB.
        /// </param>
        /// <returns>
        /// The norm of sC.
        /// </returns>
        protected abstract TReal Norm(TExt sC);

        /// <summary>
        /// A metric that defines the inherent distance between the objects.
        /// </summary>
        /// <param name="A">
        /// Object to compare.
        /// </param>
        /// <param name="B">
        /// Object to compare.
        /// </param>
        /// <returns>
        /// The inherent distance between A and B.
        /// </returns>
        protected abstract TReal Inherent(TObj A, TObj B);

        /// <summary>
        /// The numeric return type of the metric is left to the implementer of a subclass, hence
        /// the general way of calling addition on these numerical values. This should just be + for
        /// that numeric datatype.
        /// </summary>
        /// <param name="a">
        /// Number a.
        /// </param>
        /// <param name="b">
        /// Number b.
        /// </param>
        /// <returns>
        /// The result of a + b.
        /// </returns>
        protected abstract TReal Add(TReal a, TReal b);

        /// <summary>
        /// The default metric on the set of objects of type TObj.
        /// </summary>
        /// <param name="A">
        /// Object of type TObj.
        /// </param>
        /// <param name="B">
        /// Object of type TObj.
        /// </param>
        /// <returns>
        /// The distance (of ExtractMetric) between A and B.
        /// </returns>
        protected TReal DefaultDistance(TObj A, TObj B)
        {
            return Add(Norm(Subtract(Extract(A), Extract(B))), Inherent(A, B));
        }

        /// <summary>
        /// The desired metric on the set of objects of type TObj.
        /// </summary>
        /// <param name="A">
        /// Object of type TObj.
        /// </param>
        /// <param name="B">
        /// Object of type TObj.
        /// </param>
        /// <returns>
        /// The distance (of ExtractMetric) between A and B.
        /// </returns>
        /// <remarks>
        /// One may want to override this to bound or shift the metric defined by defaultdistance.
        /// </remarks>
        public virtual TReal Distance(TObj A, TObj B)
        {
            return DefaultDistance(A, B);
        }
    }
}
