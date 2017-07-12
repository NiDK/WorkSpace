using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace PwC.C4.Metadata.Storage.MongoDB
{
    public static class MongoScriptUtilities
    {
        private static readonly Assembly Assembly;
        private static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<int, string> PreparedCache = new ConcurrentDictionary<int, string>();

        /*
         * ...the mode where the dot also matches newlines is called "single-line mode". This is a bit unfortunate, because it is 
         * easy to mix up this term with "multi-line mode". Multi-line mode only affects anchors, and single-line mode only 
         * affects the dot ... When using the regex classes of the .NET framework, you activate this mode by specifying 
         * RegexOptions.Singleline, such as in Regex.Match("string", "regex", RegexOptions.Singleline).
         */
        private static readonly Regex RegexTests = new Regex(@"[\r\n]*//\s*\<test\>.*?\</test>.*?(?=[\r\n]*)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        static MongoScriptUtilities()
        {
            Assembly = Assembly.GetAssembly(typeof(MongoScriptUtilities));
        }

        public static string GetString<T>(string s)
        {
            return GetString(typeof(T), s);
        }

        public static string GetString(Type t, string s)
        {
            var key = t.Namespace + "." + s;
            object value = null;

            if (!Cache.TryGetValue(key, out value))
            {
                using (var stream = Assembly.GetManifestResourceStream(key))
                {
                    if (stream == null) return null;

                    using (var reader = new StreamReader(stream))
                    {
                        value = reader.ReadToEnd();

                        Cache[key] = value;
                    }
                }
            }

            return (string)value;
        }

        public static string PrepareScript(string script, params object[] doItParams)
        {
            string preparedScript = null;

            if (!PreparedCache.TryGetValue(script.GetHashCode(), out preparedScript))
            {
                preparedScript = RegexTests.Replace(script, string.Empty);

                PreparedCache[script.GetHashCode()] = preparedScript;
            }

            preparedScript = "function() {\r\n" + preparedScript;

            preparedScript += "\r\nreturn doIt(";

            var paramList = new List<string>();

            if (doItParams != null && doItParams.Length > 0)
            {
                paramList.AddRange(doItParams.Select(p => p != null ? p.ToJson() : "null"));
            }

            preparedScript += string.Join(", ", paramList);

            preparedScript += ");";

            preparedScript += "\r\n}";

            return preparedScript;
        }

        public static void ExecuteScript(MongoDatabase db, string script)
        {
            var results = db.Eval(new EvalArgs() { Code = script, Lock = false });
        }

        public static BsonJavaScript GetScript(this MongoDatabase db, string scriptName)
        {
            var code = db.Eval(scriptName);
            return code != null ? code.AsBsonJavaScript.Code : new BsonJavaScript(scriptName);

        }

        public static BsonDocument GetResultsAsBsonDocument(this MongoDatabase db, string script, BsonArray args)
        {

            var value = GetResults(db, script, args);

            if (value.IsBsonNull)
            {
                return null;
            }
            else
            {
                return value.AsBsonDocument;
            }

        }

        private static BsonValue GetResults(MongoDatabase db, string script, BsonArray args)
        {

            var results = db.Eval(new EvalArgs() { Code = script, Lock = false, Args = args });

            return results;
        }

        public static List<T> GetResultsAs<T>(this MongoDatabase db, string script,BsonArray args)
        {
            var ret = new List<T>();

            var results = GetResults(db, script, args);

            var value = results;

            if (value.IsBsonNull)
            {
                return ret;
            }
            else if (value.IsBsonArray)
            {
                var arr = value.AsBsonArray;
                ret.AddRange(arr.Select(item => BsonSerializer.Deserialize<T>(item.AsBsonDocument)));
            }
            else
            {
                var val = BsonSerializer.Deserialize<T>(value.AsBsonDocument);
                ret.Add(val);
            }

            return ret;
        }
    }
}
