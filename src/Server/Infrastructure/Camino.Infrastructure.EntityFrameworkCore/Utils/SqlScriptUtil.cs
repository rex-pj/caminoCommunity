using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Camino.Infrastructure.EntityFrameworkCore.Utils
{
    public class SqlScriptUtil
    {
        protected SqlScriptUtil() { }

        public static IList<string> GetCommandsFromScript(string sql)
        {
            var commands = new List<string>();

            //origin from the Microsoft.EntityFrameworkCore.Migrations.SqlServerMigrationsSqlGenerator.Generate method
            sql = Regex.Replace(sql, @"\\\r?\n", string.Empty, default, TimeSpan.FromMilliseconds(1000.0));
            var batches = Regex.Split(sql, @"^\s*(GO[ \t]+[0-9]+|GO)(?:\s+|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var batchLength = batches.Length;
            for (var i = 0; i < batchLength; i++)
            {
                if (string.IsNullOrWhiteSpace(batches[i]) || batches[i].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var count = 1;
                if (i != batches.Length - 1 && batches[i + 1].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    var match = Regex.Match(batches[i + 1], "([0-9]+)");
                    if (match.Success)
                    {
                        count = int.Parse(match.Value);
                    }
                }

                var builder = new StringBuilder();
                for (var j = 0; j < count; j++)
                {
                    builder.Append(batches[i]);
                    if (i == batches.Length - 1)
                    {
                        builder.AppendLine();
                    }
                }

                commands.Add(builder.ToString());
            }

            return commands;
        }
    }
}
