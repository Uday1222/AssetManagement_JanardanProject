namespace AssetManagement.ExcelUtility
{
    public class ResourceCleaner : IDisposable
    {

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;


        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCleaner" /> class.
        /// </summary>
        protected ResourceCleaner()
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
                this.disposed = true;
            }
        }
    }
}
