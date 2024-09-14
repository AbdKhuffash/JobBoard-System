
namespace JobBoardAppTest.Controllers
{
    /// <summary>
    /// Provides an implementation of <see cref="IAsyncEnumerator{T}"/> for testing purposes,
    /// wrapping a synchronous <see cref="IEnumerator{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="enumerator">The synchronous enumerator to wrap.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="enumerator"/> is null.</exception>
        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        }

        /// <summary>
        /// Gets the current element in the enumeration.
        /// </summary>
        /// <value>The current element in the enumeration.</value>
        public T Current => _enumerator.Current;

        /// <summary>
        /// Asynchronously releases the resources used by the enumerator.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous disposal operation.</returns>
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        /// <summary>
        /// Asynchronously advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>A <see cref="ValueTask{Boolean}"/> that represents the asynchronous operation.
        /// The value of the <see cref="ValueTask{Boolean}"/> indicates whether the enumerator was successfully advanced to the next element.</returns>
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_enumerator.MoveNext());
    }
}
