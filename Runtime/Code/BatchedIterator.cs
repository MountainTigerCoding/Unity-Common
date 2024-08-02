namespace Runtime.Shared
{ 
    /// <summary>
    /// An alternative to foreach iterator where iterations are batched instead of iterating once per frame.
    /// The class is 'ticked' in a coroutine
    /// </summary>
    public sealed class BatchedIterator
    {
        public delegate void BranchFunc (int accessIndex);

    #region Fields
        public readonly int CollectionSize;
        public readonly BranchFunc Branch;
        public readonly int BatchSize;

        public int IterationsCompleted { private set; get; }
    #endregion

    #region Properties
        public bool IsDone
        {
            get {
                return CollectionSize == 0 || IterationsCompleted >= CollectionSize || BatchSize == 0;
            }
        }
    #endregion Properties

        public BatchedIterator (int collectionSize, BranchFunc branch, int batchSize)
        {
            CollectionSize = collectionSize;
            Branch = branch;
            BatchSize = batchSize;
            IterationsCompleted = 0;
        }

        /// <summary>
        /// Will execute the next batch of iterations
        /// </summary>
        public void Iterate ()
        {
            if (IsDone) return;

            int size;
            if ((IterationsCompleted + BatchSize) > CollectionSize) size = CollectionSize - IterationsCompleted;
            else size = BatchSize;

            int startingNumExecuted = IterationsCompleted;
            for (int i = IterationsCompleted; i < startingNumExecuted + size; i++) Execute(i);
        }

        public void Execute (int accessIndex)
        {
            Branch.Invoke(accessIndex);
            IterationsCompleted++;
        }
    }
}