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
                    if (strVal.StartsWith("@"))
                    {
                        // a Lookup Table
                    }
                    else
                    {
                        // a literal string property or a column value
                        val = GetScalarValue(new KeyValuePair<string, object>(kv.Key, strVal), src);
                    }
                }
                else
                {
                    val = GetScalarValue(kv, src);
                }

                // only add the value if it is not NULL
                // TODO: Allow overriding this via a Section/Job setting
                if (val != null)
                    dst[kv.Key] = val;
            }

            return result;
        }

        private object GetScalarValue(KeyValuePair<string, object> kv, IDictionary<string, object> src)
        {
            if (kv.Value == null)
                return null;

            //var v = src[kv.Key];
            //if (v == null)
            //    return null;

            //if (!src.ContainsKey(kv.Key))
            //    return null;

            switch (kv.Value.GetType())
            {
                case Type t when t == typeof(string):

                    var strVal = (string) kv.Value;
                    var val = strVal.StartsWith("$") 
                        ? src[strVal.TrimStart('$')] 
                        : kv.Value;

                    // as of Driver version 2.4, System.Decimal gets serialized to MongoDB as a nested object
                    // HACK: cast it to double
                    return val is decimal ? Convert.ToDouble(val) : val;
                case Type t when t == typeof(JToken):
                    var jt = kv.Value;
                    return null;
                case Type t when t == typeof(JObject):
                    var jo = kv.Value;
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

        private object GetObjectValue(IDictionary<string, object> src, JObject tpl)
        {
            var result = new ExpandoObject();
            var dst = (IDictionary<string, object>)result;

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
                else if (kv.Value is JValue)
                {
                    // cast to work with JValue members
                    var jval = (JValue)kv.Value;

                    switch (jval.Type)
                    {
                        case JTokenType.String:
                            var strVal = (string)kv.Value;
                            if (strVal.StartsWith("@"))
                            {
                                // a Lookup Table
                            }
                            else
                            {
                                // a literal string property or a column value
                                val = GetScalarValue(new KeyValuePair<string, object>(kv.Key, strVal), src);
                            }
                            break;
                        case JTokenType.Boolean:
                        case JTokenType.Bytes:
                        case JTokenType.Date:
                        case JTokenType.Float:
                        case JTokenType.Guid:
                        case JTokenType.Integer:
                        case JTokenType.TimeSpan:
                            val = GetScalarValue(new KeyValuePair<string, object>(kv.Key, kv.Value), src);
                            break;

                    }


                }


                // only add the value if it is not NULL
                // TODO: Allow overriding this via a Section/Job setting
                if (val != null)
                    dst[kv.Key] = val;
            }

            return result;
        }
    }
}