
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BattleInArea.Core
{
    public partial class Reader
    {
        public class CSV
        {
            static private string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
            static private string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
            static private char[] TRIM_CHARS = { '\"' };

            public static void Read(string inPath, List<Dictionary<string, object>> outList)
            {
                Debug.Log(string.Format("read file path : {0}", inPath));

                TextAsset data = Resources.Load(inPath) as TextAsset;

                var lines = Regex.Split(data.text, LINE_SPLIT_RE);

                if (lines.Length <= 1)
                {
                    return;
                }
                else
                {
                    var header = Regex.Split(lines[0], SPLIT_RE);
                    for (var i = 1; i < lines.Length; i++)
                    {
                        var values = Regex.Split(lines[i], SPLIT_RE);
                        if (values.Length == 0 || values[0] == "") continue;

                        var entry = new Dictionary<string, object>();
                        for (var j = 0; j < header.Length && j < values.Length; j++)
                        {
                            string value = values[j];
                            value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                            object finalvalue = value;
                            int n;
                            float f;
                            if (int.TryParse(value, out n))
                            {
                                finalvalue = n;
                            }
                            else if (float.TryParse(value, out f))
                            {
                                finalvalue = f;
                            }
                            entry[header[j]] = finalvalue;
                        }
                        outList.Add(entry);
                    }
                }
                Resources.UnloadAsset(data);
            }
        }
    }

}