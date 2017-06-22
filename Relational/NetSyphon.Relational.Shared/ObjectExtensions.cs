using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

namespace NetSyphon.Relational.Shared
{
    /// <summary>
    /// Class which provides extension methods for various ADO.NET objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Turns an IDataReader to a Dynamic list of things
        /// </summary>
        /// <param name="reader">The datareader which rows to convert to a list of expandos.</param>
        /// <returns>List of expandos, one for every row read.</returns>
        public static List<dynamic> ToExpandoList(this IDataReader reader)
        {
            var result = new List<dynamic>();
            while (reader.Read())
            {
                result.Add(reader.RecordToExpando());
            }
            return result;
        }


        /// <summary>
        /// Converts the current row the datareader points to to a new Expando object.
        /// </summary>
        /// <param name="reader">The RDR.</param>
        /// <returns>expando object which contains the values of the row the reader points to</returns>
        public static dynamic RecordToExpando(this IDataReader reader)
        {
            dynamic e = new ExpandoObject();
            var d = (IDictionary<string, object>)e;
            var values = new object[reader.FieldCount];
            reader.GetValues(values);
            for (var i = 0; i < values.Length; i++)
            {
                var v = values[i];
                d.Add(reader.GetName(i), DBNull.Value.Equals(v) ? null : v);
            }
            return e;
        }


        /// <summary>
        /// Turns the object into an ExpandoObject 
        /// </summary>
        /// <param name="o">The object to convert.</param>
        /// <returns>a new expando object with the values of the passed in object</returns>
        public static dynamic ToExpando(this object o)
        {
            if (o is ExpandoObject)
            {
                return o;
            }
            var result = new ExpandoObject();
            var d = (IDictionary<string, object>)result; //work with the Expando as a Dictionary
            if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(i => d.Add(i));
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }


        /// <summary>
        /// Turns the object into a Dictionary with for each property a name-value pair, with name as key.
        /// </summary>
        /// <param name="thingy">The object to convert to a dictionary.</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object thingy)
        {
            return (IDictionary<string, object>)thingy.ToExpando();
        }


        /// <summary>
        /// Extension method to convert dynamic data to a DataTable. Useful for databinding.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>A DataTable with the copied dynamic data.</returns>
        /// <remarks>Credit given to Brian Vallelunga http://stackoverflow.com/a/6298704/5262210 </remarks>
        public static DataTable ToDataTable(this IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            var toReturn = new DataTable();
            if (!data.Any())
                return toReturn;

            foreach (var kvp in (IDictionary<string, object>)data[0])
            {
                // for now we'll fall back to string if the value is null, as we don't know any type information on null values.
                var type = kvp.Value?.GetType() ?? typeof(string);
                toReturn.Columns.Add(kvp.Key, type);
            }
            return data.ToDataTable(toReturn);
        }


        /// <summary>
        /// Extension method to convert dynamic data to a DataTable. Useful for databinding.
        /// </summary>
        /// <param name="items">The items to convert to data rows.</param>
        /// <param name="toFill">The datatable to fill. It's required this datatable has the proper columns setup.</param>
        /// <returns>
        /// toFill with the data from items.
        /// </returns>
        /// <remarks>
        /// Credit given to Brian Vallelunga http://stackoverflow.com/a/6298704/5262210
        /// </remarks>
        public static DataTable ToDataTable(this IEnumerable<dynamic> items, DataTable toFill)
        {
            var data = items is dynamic[] ? (dynamic[])items : items.ToArray();
            if (toFill == null || toFill.Columns.Count <= 0)
                return toFill;
            
            foreach (var d in data)
                toFill.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            
            return toFill;
        }
    }
}