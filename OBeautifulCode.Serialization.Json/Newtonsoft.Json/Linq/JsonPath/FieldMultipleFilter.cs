﻿// <auto-generated></auto-generated>

using System.Collections.Generic;
using System.Globalization;
#if NET20
using NewtonsoftFork.Json.Utilities.LinqBridge;
#else
using System.Linq;
#endif
using NewtonsoftFork.Json.Utilities;

namespace NewtonsoftFork.Json.Linq.JsonPath
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Serialization.Json", "See package version number")]
    internal class FieldMultipleFilter : PathFilter
    {
        public List<string> Names { get; set; }

        public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
        {
            foreach (JToken t in current)
            {
                JObject o = t as JObject;
                if (o != null)
                {
                    foreach (string name in Names)
                    {
                        JToken v = o[name];

                        if (v != null)
                        {
                            yield return v;
                        }

                        if (errorWhenNoMatch)
                        {
                            throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, name));
                        }
                    }
                }
                else
                {
                    if (errorWhenNoMatch)
                    {
                        throw new JsonException("Properties {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", Names.Select(n => "'" + n + "'").ToArray()), t.GetType().Name));
                    }
                }
            }
        }
    }
}