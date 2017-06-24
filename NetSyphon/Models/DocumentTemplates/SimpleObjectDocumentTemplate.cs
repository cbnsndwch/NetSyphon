using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NetSyphon.Relational.Shared;
using Newtonsoft.Json.Linq;

namespace NetSyphon.Models.DocumentTemplates
{
    /// <summary>
    /// A DocumentTemplate builder that can output a simple, non-nested object
    /// </summary>
    public class SimpleObjectDocumentTemplate : IDocumentTemplate
    {
        public string Name { get; set; }

        public object Build(object item, IDictionary<string, object> tpl)
        {
            var result = new ExpandoObject();
            var dst = (IDictionary<string, object>)result;
            var src = item.ToDictionary();

            foreach (var kv in tpl)
            {
                if (kv.Value == null)
                    continue;

                object val = null;
                if (kv.Value.GetType() == typeof(JArray))
                {
                    var arr = ((JArray)kv.Value).ToArray();
                    val = null;
                }
                else if (kv.Value.GetType() == typeof(JObject))
                {
                    // an object property
                    val = GetObjectValue(src, (JObject)kv.Value);
                }
                else if (kv.Value is string)
                {
                    var strVal = (string)kv.Value;
                    if (strVal.StartsWith("$"))
                    {
                        // a column


                    }
                    else if (strVal.StartsWith("@"))
                    {
                        // a Lookup Table
                    }
                    else
                    {
                        // a literal string property
                        val = GetScalarValue(src, kv);
                    }
                }
                else
                {
                    val = GetScalarValue(src, kv);
                }

                // only add the value if it is not NULL
                // TODO: Allow overriding this via a Section/Job setting
                if (val != null)
                    dst[kv.Key] = val;
            }

            return result;
        }

        private object GetScalarValue(IDictionary<string, object> src, KeyValuePair<string, object> kv)
        {
            if (!src.ContainsKey(kv.Key))
                return null;

            var v = src[kv.Key];
            if (v == null)
                return null;

            switch (v.GetType())
            {
                case Type t when t == typeof(string):
                    var val = ((string)kv.Value).StartsWith("$") ? src[kv.Key.TrimStart('$')] : kv.Value;

                    // as of Driver version 2.4, System.Decimal gets serialized to MongoDB as a nested object
                    // HACK: cast it to double
                    return val is decimal ? Convert.ToDouble(val) : val;
                case Type t when t == typeof(JObject):
                    var xxx = kv.Value;
                    return null;
                case Type decimalType when decimalType == typeof(decimal):
                    // as of Driver version 2.4, System.Decimal gets serialized to MongoDB as a nested object
                    // HACK: cast it to double
                    return Convert.ToDouble(src[kv.Key]);
                case Type doubleType when doubleType == typeof(double):
                case Type floaType when floaType == typeof(float):
                case Type intType when intType == typeof(int):
                case Type longType when longType == typeof(long):
                case Type shortType when shortType == typeof(short):
                case Type boolType when boolType == typeof(bool):
                case Type dateType when dateType == typeof(DateTime):
                    return src[kv.Key];
                default:
                    return kv.Value;
            }
        }

        private object GetObjectValue(IDictionary<string, object> src, JObject kv)
        {
            var result = new ExpandoObject();
            var dst = (IDictionary<string, object>)result;




            return result;
        }
    }
}