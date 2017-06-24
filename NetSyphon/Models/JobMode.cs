namespace NetSyphon.Models
{
    /// <summary>
    /// Describes the write-mode the job should run in.
    /// </summary>
    public enum JobMode
    {
        /// <summary>
        /// An insert operation with an implied collection truncate before loading data.
        /// </summary>
        Copy,

        /// <summary>
        /// An insert operation that append new data to the collection and keeps any existing data.
        /// </summary>
        Append,

        /// <summary>
        /// An upsert operation that replaces documents matching a specified criteria or inserts documents that don't.
        /// </summary>
        Replace,

        /// <summary>
        /// An upsert operation that modifies documents matching a specified criteria or inserts documents that don't.
        /// </summary>
        Merge

    }
}
