using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpConsole
{
    public class CsvWriter
    {
        public CsvWriter(IEnumerable<string> source)
        {
            Source = source;
        }

        public IEnumerable<string> Source { get; }

        public void CreateCsv()
        {
            var sb = new StringBuilder();
            sb.AppendLine("HW,XX,AW");
            sb.Append(string.Join("\n", Source.Select(x => x.Replace(" ", ",").Replace("X", "0")).ToArray()));
            File.WriteAllText("data.csv", sb.ToString());
        }
    }
}
