using System;
using System.Collections.Generic;

namespace BoogiepopTCore
{
    /// <summary>
    /// Defines a way of comparing ordered pairs of objects from which (a set of) associated objects can be defined.
    /// Given that the set of these associated objects defines a metric space.
    /// </summary>
    /// <typeparam name="TObj">
    /// Datatype of the objects one wants to compare ordered pairs of.
    /// </typeparam>
    /// <typeparam name="TAssocObj">
    /// Datatype of the associated objects to the objects one wishes to compare ordered pairs of.
    /// </typeparam>
    /// <typeparam name="TRealM">
    /// Numeric return type of the metric one chooses to use.
    /// </typeparam>
    /// <typeparam name="TRealML">
    /// Numeric return type of comparing ordered pairs of type TObj. In order to not lose information from the
    /// metric this type should be one which has an implicit cast from TRealM.
    /// </typeparam>
    public class MetricLift<TObj, TAssocObj, TRealM, TRealML> where TRealM : struct where TRealML : struct
    {
        /// <summary>
        /// Ordered pairs of objects can be viewed as arrows. This is the first element i.e., where it starts from.
        /// </summary>
        public TObj From { get; }

        /// <summary>
        /// Ordered pairs of objects can be viewed as arrows. This is the second element i.e., where it points to.
        /// </summary>
        public TObj To { get; }

        /// <summary>
        /// Function that for every object of type TObj assign to it a collection of associated objects.
        /// </summary>
        private readonly Func<TObj, IEnumerable<TAssocObj>> associated;

        /// <summary>
        /// Equality comparison for these objects.
        /// </summary>
        private readonly Func<TAssocObj, TAssocObj, bool> areEqual;

        /// <summary>
        /// Metric on the set of all elements of type TAssocObj.
        /// </summary>
        private readonly IMetric<TAssocObj, TRealM> metric;

        private readonly Dictionary<(TAssocObj, TAssocObj), TRealM> prf;

        /// <summary>
        /// Addition operation on TRealML. Should just be + of the numeric datatype chosen.
        /// </summary>
        private readonly Func<TRealML, TRealML, TRealML> addition;

        /// <summary>
        /// Multiplication operation on TRealML. Should just be * of the numeric datatype chosen.
        /// </summary>
        private readonly Func<TRealML, TRealML, TRealML> product;

        private readonly TRealML prfNormSquared;

        /// <summary>
        /// Inverse with respect to addition, meaning: addition(x, inverse(x)) = default(TRealML) has to hold.
        /// </summary>
        private readonly Func<TRealML, TRealML> inverse;

        public MetricLift(Func<TRealML, TRealML, TRealML> addition, Func<TRealML, TRealML, TRealML> product,
            Func<TRealML, TRealML> inverse, Func<TObj, IEnumerable<TAssocObj>> associated, Func<TAssocObj, TAssocObj, bool> areEqual,
            IMetric<TAssocObj, TRealM> metric, TObj prfA, TObj prfB)
        {
            From = prfA;
            To = prfB;
            this.addition = addition;
            this.product = product;
            this.inverse = inverse;
            this.associated = associated;
            this.areEqual = areEqual;
            this.metric = metric;
            prf = IndexedDistances(prfA, prfB);
            prfNormSquared = InnerProductWithPRF(prf);
        }

        /// <summary>
        /// Calculates a vector, with the dimensions indexed by pairs of associated objects, of the distance
        /// between those paired associated objects.
        /// </summary>
        /// <param name="A">
        /// First object, i.e. where it starts from.
        /// </param>
        /// <param name="B">
        /// Second object, i.e. where it points to.
        /// </param>
        /// <returns>
        /// Dictionary with keys the pairs of associated objects and values the distance between said associated
        /// objects.
        /// </returns>
        private Dictionary<(TAssocObj, TAssocObj), TRealM> IndexedDistances(TObj A, TObj B)
        {
            Dictionary<(TAssocObj, TAssocObj), TRealM> indexedDistances
                = new Dictionary<(TAssocObj, TAssocObj), TRealM>();

            foreach (TAssocObj aA in associated(A))
                foreach (TAssocObj aB in associated(B))
                    indexedDistances[(aA, aB)] = metric.Distance(aA, aB);

            return indexedDistances;
        }

        /// <summary>
        /// Calculates the inner product of the argument with the preference vector prf, in which the dimension
        /// are aligned.
        /// </summary>
        /// <param name="indexedDistances">
        /// Vector to calculate inner product with prf with.
        /// </param>
        /// <returns>
        /// Standard inner product of indexedDistances with prf.
        /// </returns>
        private TRealML InnerProductWithPRF(Dictionary<(TAssocObj, TAssocObj), TRealM> indexedDistances)
        {
            TRealML sum = default(TRealML);
            foreach (KeyValuePair<(TAssocObj, TAssocObj), TRealM> kv in prf)
            {
                foreach (KeyValuePair<(TAssocObj, TAssocObj), TRealM> kvID in indexedDistances)
                {
                    if (areEqual(kvID.Key.Item1, kv.Key.Item1) && areEqual(kvID.Key.Item2, kv.Key.Item2))
                    {
                        sum = addition(sum, product((TRealML)(object)kv.Value, (TRealML)(object)kvID.Value));
                        break;
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// Compare another ordered pair of objects to prf.
        /// </summary>
        /// <param name="C">
        /// First object, i.e. where it starts from.
        /// </param>
        /// <param name="D">
        /// Second object, i.e. where it points to.
        /// </param>
        /// <returns>
        /// Norm(prf)^2 - StandardInnerProduct(IndexedDistance(C, D), prf).
        /// </returns>
        /// <remarks>
        /// This is always non-negative, see report of BoogiepopT Core for a proof.
        /// </remarks>
        public TRealML CompareToPRF(TObj C, TObj D)
        {
            return addition(prfNormSquared, inverse(InnerProductWithPRF(IndexedDistances(C, D))));
        }
    }
}
