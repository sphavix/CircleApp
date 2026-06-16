using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CircleApp.Data.Helpers
{
    public static class HashtagHelper
    {
        public static List<string> ExtractHashtags(string content)
        {
            var hashTagsPattern = new Regex(@"#\w+");
            var matches = hashTagsPattern.Matches(content)
                            .Select(m => m.Value.TrimEnd('.', ',', '!', '?'))
                            .Distinct()
                            .ToList();
            return matches;
        }
    }
}
