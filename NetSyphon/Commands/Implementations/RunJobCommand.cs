using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using NetSyphon.Commands.Contracts;
using NetSyphon.Models;
using NetSyphon.Relational.Shared;
using NetSyphon.Relational.SqlServer;
using NetSyphon.Services;

namespace NetSyphon.Commands.Implementations
{
    /// <summary>
    /// A command to run and ETL job, as specified by the Job Configuration model
    /// </summary>
    public class RunJobCommand : ICommand<JobDescription>
    {
        #region Fields

        private JobDescription _jobModel;
        private DynamicModel _dbContext;
        private MongoClient _mongoClient;
        private DocumentGeneratorService _documentGenerator;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RunJobCommand));

        #endregion

        #region ICommand members

        /// <summary>
        /// The entry point to the command for synchronous execution
        /// </summary>
        /// <param name="model">An instance of <see cref="JobDescription"/> describing the ETL job</param>
        public void Execute(JobDescription model)
        {
            // ensure model is not null
            _jobModel = model ?? throw new ArgumentException("The Job Description model cannot be null");

            // get DBMS DbContext
            switch (_jobModel.ProviderName)
            {
                case "System.Data.SqlClient":
                    _dbContext = new SqlServerDynamicModel(_jobModel.DatabaseConnection, "");
                    break;
            }

            // ensure a DbContext was obtained
            if (_dbContext == null)
                throw new ArgumentException($"The DbProvider [{_jobModel.ProviderName}] could not be found.");

            // get a DocumentGeneratorService
            _documentGenerator = new DocumentGeneratorService(null, _dbContext, _jobModel);

            // get a MongoDB Database object
            _mongoClient = new MongoClient(_jobModel.MongoConnection);
            var db = _mongoClient.GetDatabase(_jobModel.MongoDatabase);

            // if the job is being run in "Copy" mode, we need to truncate the collection first,
            // which we do by issuing a DropCollection command. 
            // connecting to it after and inserting documents will recreate it.
            // WARNING: Please note this will remove any Indices from the collection. 
            // See https://stackoverflow.com/questions/16493902/truncate-a-collection
            if (_jobModel.JobMode == JobMode.Copy)
                db.DropCollection(_jobModel.MongoCollection);

            // get a handle to the MongoDB Collection
            var col = db.GetCollection<dynamic>(_jobModel.MongoCollection);

            // Using Bulk Writes to improve efficiency.
            // See http://mongodb.github.io/mongo-csharp-driver/2.4/reference/driver/crud/writing/#bulk-writes
            var batch = new List<WriteModel<dynamic>>(_jobModel.BatchSize);

            // Open a cursor to the SQL query
            var data = _documentGenerator.AsEnumerable();

            var pages = 0;
            _logger.Info($"Starting Job with batch size: {_jobModel.BatchSize}");
            foreach (var doc in data)
            {
                _logger.Info($"Loading document {pages * _jobModel.BatchSize + batch.Count}");
                batch.Add(new InsertOneModel<dynamic>(doc));

                if (batch.Count != _jobModel.BatchSize)
                    continue;

                _logger.Info($"Performing batch operation against destination with batch size={_jobModel.BatchSize}");

                // TODO: Make this a bit more resilient
                // TODO: Consider using async/await
                col.BulkWrite(batch);

                batch.Clear();
                pages++;

                _logger.Info($"Batch operation number {pages} completed succesfully");
            }

            if (batch.Count > 0)
            {
                _logger.Info($"Performing batch job against destination with batch size={batch.Count}");

                // TODO: Make this a bit more resilient
                // TODO: Consider using async/await
                col.BulkWrite(batch);

                batch.Clear();
                pages++;

                _logger.Info($"Batch operation number {pages} completed succesfully");
            }

            // Finished

            _logger.Info("Finished Running Job");
        }

        /// <summary>
        /// The entry point to the command for asynchronous execution
        /// </summary>
        /// <param name="model">An instance of <see cref="JobDescription"/> describing the ETL job</param>
        public Task ExecuteAsync(JobDescription model)
        {
            return Task.Run(() => Execute(model));
        }

        #endregion
    }
}
