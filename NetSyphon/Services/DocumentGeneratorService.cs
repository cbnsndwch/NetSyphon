using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MongoDB.Bson;
using NetSyphon.Models;
using NetSyphon.Relational.Shared;

namespace NetSyphon.Services
{
    public class DocumentGeneratorService
    {
        #region Services

        private readonly ILog _logger;
        private readonly DynamicModel _dbContext;
        private readonly JobDescription _model;

        #endregion

        #region Constructor

        public DocumentGeneratorService(ILog logger, DynamicModel dbContext, JobDescription model)
        {
            _logger = logger;
            _model = model;
            _dbContext = dbContext;
            _dbContext.TableName = _model.StartSection.TableName;
        }

        #endregion

        #region Public Methods

        public IEnumerable<dynamic> AsEnumerable()
        {
            return _dbContext.Query(_model.StartSection.Sql);
        }

        #endregion

        #region Migrate

        private DynamicModel dbContext = null;
        private JobDescription jobdesc;

        BsonDocument section = null;
        BsonDocument template = null;
        BsonDocument data = null;

        Dictionary<string, DocumentGeneratorService> docGens = new Dictionary<String, DocumentGeneratorService>();

        void Foo(JobDescription jobdesc, string sectionName, BsonDocument data)
        {
            var section = jobdesc.Sections.FirstOrDefault(s => s.Name == sectionName);
            if (section == null)
            {
                var msg = $"Cannot find section named [{sectionName}] in config - aborting";
                //_logger.Error(msg);
                throw new Exception(msg);
            }

            var template = /*(BsonDocument)*/ section.Template;



        }

        //        public BsonDocument GetNext()
        //        {
        //            BsonDocument rval = null;
        //            BsonDocument row = new BsonDocument();

        //            // Do I have a connection - if not get one
        //            if (connection == null)
        //            {
        //                connection = new RDBMSConnection(jobdesc.getDatabaseConnection(),
        //                    section.containsKey("cached"));
        //                connection.Connect(jobdesc.getDatabaseUser(),
        //                    jobdesc.getDatabasePass());

        //            }

        //            ArrayList<String> paramArray = section.get("params", ArrayList.class);

        //            if (connection.hasResults() == false) {
        //                connection.RunQuery(section.getString("sql"), paramArray, params);
        //            }

        //            try {
        //                row = connection.GetNextRow();
        //            } catch (SQLException e) {
        //                logger.error(e.getMessage());
        //                System.exit(1);
        //            }

        //            boolean found = false;
        //            // In MERGE mode, we want to look at each row and see if we want it
        //            // If it's < the value we want we ignore and take the next one
        //            // If it's the value we want we use it
        //            // If it's > the value we want we push it back on the cursor, but
        //            // tricky in RDBMSConnection

        //            while (section.containsKey("mergeon") && found == false) {
        //                String mergeField = section.getString("mergeon");
        //                // Row is the data below
        //                // Params is the row above
        //                if (params.containsKey(mergeField) && row != null
        //                    && row.containsKey(mergeField)) {
        //                    int compval = -1;
        //Object parent = params.get(mergeField);
        //Object child = row.get(mergeField);
        //                    if (parent.getClass() == child.getClass()) {
        //                        if (parent.getClass() == String.class) {
        //                            String a = (String)parent;
        //String b = (String)child;
        //compval = a.compareTo(b);
        //                        } else if (parent.getClass() == Integer.class) {
        //                            compval = (Integer) child - (Integer) parent;
        //                        }
        //                    }
        //                    // Loop again whilst we haven't found one
        //                    if (compval< 0) {
        //                        try {
        //                            row = connection.GetNextRow();
        //                        } catch (SQLException e) {
        //                            logger.error(e.getMessage());
        //                            System.exit(1);
        //                        }
        //                        found = false;
        //                    } else if (compval > 0) {
        //                        // Put it back
        //                        connection.PushBackRow(row);
        //                        // Tell parent we ran out of options
        //                        found = false;
        //                        return null;
        //                    } else {
        //                        found = true;
        //                    }
        //                } else {
        //                    found = true; // Actually not but it gets us out
        //                }
        //            }
        //            // End Merge

        //            // Apply template to database ROW
        //            if (row != null) {
        //                rval = TemplateRow(template, row);
        //            }
        //            return rval;
        //        } 

        #endregion
    }
}
