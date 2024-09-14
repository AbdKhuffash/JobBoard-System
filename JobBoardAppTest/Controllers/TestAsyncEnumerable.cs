using System.Linq.Expressions;


namespace JobBoardAppTest.Controllers
{
    /// <summary>
    /// Provides an implementation of <see cref="IAsyncEnumerable{T}"/> and <see cref="IQueryable{T}"/> for testing purposes,
    /// wrapping a synchronous <see cref="IEnumerable{T}"/> to simulate asynchronous enumeration.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerable{T}"/> class with a given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="enumerable">The synchronous collection to wrap.</param>
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerable{T}"/> class with a given <see cref="Expression"/>.
        /// </summary>
        /// <param name="expression">The expression to use for querying.</param>
        public TestAsyncEnumerable(Expression expression) : base(expression) { }

        /// <summary>
        /// Returns an asynchronous enumerator that iterates through the collection.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the asynchronous operation to complete.</param>
        /// <returns>An <see cref="IAsyncEnumerator{T}"/> that provides asynchronous enumeration over the collection.</returns>
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        /// <summary>
        /// Returns the <see cref="IQueryProvider"/> for this queryable.
        /// This method is not implemented.
        /// </summary>
        /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
        internal IQueryProvider Provider()
        {
            throw new NotImplementedException();
        }
    }
}
