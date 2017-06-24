///////////////////////////////////////////////////////////////////////////////////////////////////
// Massive v2.0. SQL Server specific code
///////////////////////////////////////////////////////////////////////////////////////////////////
// Licensed to you under the New BSD License
// http://www.opensource.org/licenses/bsd-license.php
// Massive is copyright (c) 2009-2016 various contributors.
// All rights reserved.
// See for sourcecode, full history and contributors list: https://github.com/FransBouma/Massive
//
// Redistribution and use in source and binary forms, with or without modification, are permitted 
// provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the 
//   following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//   the following disclaimer in the documentation and/or other materials provided with the distribution.
// - The names of its contributors may not be used to endorse or promote products derived from this software 
//   without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY 
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using NetSyphon.Relational.Shared;

namespace NetSyphon.Relational.SqlServer
{
    /// <summary>
    /// A class that wraps your database table in Dynamic Funtime
    /// </summary>
    public class SqlServerDynamicModel : DynamicModel
    {
        #region Constants

        // Mandatory constants every DB has to define. 
        /// <summary>
        /// The default sequence name for initializing the pk sequence name value in the ctor. 
        /// </summary>
        protected override string DefaultSequenceName => "SCOPE_IDENTITY()";

        /// <summary>
        /// Flag to signal whether the sequence retrieval call (if any) is executed before the insert query (true) or after (false). Not a const, to avoid warnings. 
        /// </summary>
        protected override bool SequenceValueCallsBeforeMainInsert => false;

        #endregion

        #region Properties

        /// <summary>
        /// Provides the default DbProviderFactoryName to the core to create a factory on the fly in generic code.
        /// </summary>
        protected override string DbProviderFactoryName => "System.Data.SqlClient";

        /// <summary>
        /// Gets the table schema query to use to obtain meta-data for a given table and schema
        /// </summary>
        protected override string TableWithSchemaQuery => "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @0 AND TABLE_SCHEMA = @1";

        /// <summary>
        /// Gets the table schema query to use to obtain meta-data for a given table which is specified as the single parameter
        /// </summary>
        protected override string TableWithoutSchemaQuery => "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @0";

        #endregion

        #region Constructor

        public SqlServerDynamicModel(string connectionStringName, IConnectionStringProvider connectionStringProvider, string tableName, string pkField = "Id", string descriptorField = "", string pkFieldSequence = "")
            : base(connectionStringName, connectionStringProvider, tableName, pkField, descriptorField, pkFieldSequence) { }

        public SqlServerDynamicModel(string connectionString, string tableName, string pkField = "Id", string descriptorField = "", string pkFieldSequence = "")
            : base(connectionString, tableName, pkField, descriptorField, pkFieldSequence) { }

        #endregion

        #region DynamicModel members

        /// <summary>
        /// Add single parameter to DbCommand. 
        /// </summary>
        /// <param name="cmd">The command to add the parameter to.</param>
        /// <param name="value">The value to add as a parameter to the command.</param>
        public override void AddParam(DbCommand cmd, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = $"@{cmd.Parameters.Count}";
            if (value == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                var o = value as ExpandoObject;
                if (o == null)
                {
                    p.Value = value;
                    var s = value as string;
                    if (s != null)
                        p.Size = s.Length > 4000 ? -1 : 4000;
                }
                else
                {
                    p.Value = ((IDictionary<string, object>)value).Values.FirstOrDefault();
                }
            }
            cmd.Parameters.Add(p);
        }

        /// <summary>
        /// Gets a default value for the column as defined in the schema.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        protected override dynamic GetDefaultValue(dynamic column)
        {
            string defaultValue = column.COLUMN_DEFAULT;
            if (string.IsNullOrEmpty(defaultValue))
            {
                return null;
            }
            dynamic result;
            switch (defaultValue)
            {
                case "getdate()":
                case "(getdate())":
                    result = DateTime.Now;
                    break;
                case "newid()":
                    result = Guid.NewGuid().ToString();
                    break;
                default:
                    result = defaultValue.Replace("(", "").Replace(")", "");
                    break;
            }
            return result;
        }

        /// <summary>
        /// Gets the aggregate function to use in a scalar query for the fragment specified
        /// </summary>
        /// <param name="aggregateCalled">The aggregate called on the dynamicmodel, which should be converted to a DB function. Expected to be lower case</param>
        /// <returns>the aggregate function to use, or null if no aggregate function is supported for aggregateCalled</returns>
        protected override string GetAggregateFunction(string aggregateCalled)
        {
            switch (aggregateCalled)
            {
                case "sum":
                    return "SUM";
                case "max":
                    return "MAX";
                case "min":
                    return "MIN";
                case "avg":
                    return "AVG";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the sql statement to use for obtaining the identity value of the last insert.
        /// </summary>
        /// <returns></returns>
        protected override string GetIdentityRetrievalScalarStatement()
        {
            return string.IsNullOrEmpty(PrimaryKeyFieldSequence) ? string.Empty : $"SELECT {PrimaryKeyFieldSequence} as newID";
        }

        /// <summary>
        /// Gets the sql statement pattern for a count row query (count(*)). The pattern should include as place holders: {0} for source (FROM clause).
        /// </summary>
        /// <returns></returns>
        protected override string GetCountRowQueryPattern()
        {
            return "SELECT COUNT(*) FROM {0} ";
        }

        /// <summary>
        /// Gets the name of the parameter with the prefix to use in a query, e.g. @rawName or :rawName
        /// </summary>
        /// <param name="rawName">raw name of the parameter, without parameter prefix</param>
        /// <returns>rawName prefixed with the db specific prefix (if any)</returns>
        protected override string PrefixParameterName(string rawName)
        {
            return "@" + rawName;
        }

        /// <summary>
        /// Gets the select query pattern, to use for building select queries. The pattern should include as place holders: {0} for project list, {1} for the source (FROM clause).
        /// </summary>
        /// <param name="limit">The limit for the resultset. 0 means no limit.</param>
        /// <param name="whereClause">The where clause. Expected to have a prefix space if not empty</param>
        /// <param name="orderByClause">The order by clause. Expected to have a prefix space if not empty</param>
        /// <returns>
        /// string pattern which is usable to build select queries.
        /// </returns>
        protected override string GetSelectQueryPattern(int limit, string whereClause, string orderByClause)
        {
            return $"SELECT{(limit > 0 ? " TOP " + limit : string.Empty)} {{0}} FROM {{1}}{whereClause}{orderByClause}";
        }

        /// <summary>
        /// Gets the insert query pattern, to use for building insert queries. The pattern should include as place holders: {0} for target, {1} for field list, {2} for parameter list
        /// </summary>
        /// <returns></returns>
        protected override string GetInsertQueryPattern()
        {
            return "INSERT INTO {0} ({1}) VALUES ({2})";
        }

        /// <summary>
        /// Gets the update query pattern, to use for building update queries. The pattern should include as placeholders: {0} for target, {1} for field list with sets. Has to have
        /// trailing space
        /// </summary>
        /// <returns></returns>
        protected override string GetUpdateQueryPattern()
        {
            return "UPDATE {0} SET {1} ";
        }

        /// <summary>
        /// Gets the delete query pattern, to use for building delete queries. The pattern should include as placeholders: {0} for the target. Has to have trailing space
        /// </summary>
        /// <returns></returns>
        protected override string GetDeleteQueryPattern()
        {
            return "DELETE FROM {0} ";
        }

        /// <summary>
        /// Gets the name of the column using the expando object representing the column from the schema
        /// </summary>
        /// <param name="columnFromSchema">The column from schema in the form of an expando.</param>
        /// <returns>the name of the column as defined in the schema</returns>
        protected override string GetColumnName(dynamic columnFromSchema)
        {
            return columnFromSchema.COLUMN_NAME;
        }

        /// <summary>
        /// Post-processes the query used to obtain the meta-data for the schema. If no post-processing is required, simply return a toList 
        /// </summary>
        /// <param name="toPostProcess">To post process.</param>
        /// <returns></returns>
        protected override IEnumerable<dynamic> PostProcessSchemaQuery(IEnumerable<dynamic> toPostProcess)
        {
            return toPostProcess?.ToList() ?? new List<dynamic>();
        }

        /// <summary>
        /// Builds a paging query and count query pair. 
        /// </summary>
        /// <param name="sql">The SQL statement to build the query pair for. Can be left empty, in which case the table name from the schema is used</param>
        /// <param name="primaryKeyField">The primary key field. Used for ordering. If left empty the defined PK field is used</param>
        /// <param name="whereClause">The where clause. Default is empty string.</param>
        /// <param name="orderByClause">The order by clause. Default is empty string.</param>
        /// <param name="columns">The columns to use in the project. Default is '*' (all columns, in table defined order).</param>
        /// <param name="pageSize">Size of the page. Default is 20</param>
        /// <param name="currentPage">The current page. 1-based. Default is 1.</param>
        /// <returns>ExpandoObject with two properties: MainQuery for fetching the specified page and CountQuery for determining the total number of rows in the resultset</returns>
        protected override dynamic BuildPagingQueryPair(string sql = "", string primaryKeyField = "", string whereClause = "", string orderByClause = "", string columns = "*", int pageSize = 20, int currentPage = 1)
        {
            var countSql = string.IsNullOrEmpty(sql)
                ? $"SELECT COUNT({PrimaryKeyField}) FROM {TableName}"
                : $"SELECT COUNT({primaryKeyField}) FROM ({sql}) AS PagedTable";

            var orderByClauseFragment = orderByClause;
            if (string.IsNullOrEmpty(orderByClauseFragment))
                orderByClauseFragment = string.IsNullOrEmpty(primaryKeyField) ? PrimaryKeyField : primaryKeyField;

            var whereClauseFragment = ReadifyWhereClause(whereClause);
            var query = string.IsNullOrEmpty(sql)
                ? $"SELECT {columns} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {orderByClauseFragment}) AS Row, {columns} FROM {TableName} {whereClauseFragment}) AS Paged "
                : $"SELECT {columns} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {orderByClauseFragment}) AS Row, {columns} FROM ({sql}) AS PagedTable {whereClauseFragment}) AS Paged ";

            var pageStart = (currentPage - 1) * pageSize;
            query += $" WHERE Row > {pageStart} AND Row <={(pageStart + pageSize)}";
            countSql += whereClauseFragment;

            dynamic toReturn = new ExpandoObject();
            toReturn.MainQuery = query;
            toReturn.CountQuery = countSql;

            return toReturn;
        }

        #endregion

    }
}
