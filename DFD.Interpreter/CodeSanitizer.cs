using System.Text.RegularExpressions;

namespace DFD.Parsing;

internal class CodeSanitizer
{
    public DfdCodeLine[] PrepareAsCode(string code)
    {
        code = code.Replace("\r\n", "\n");
        var lines = code.Split('\n');
        List<DfdCodeLine> statememnts = new List<DfdCodeLine>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (!Regex.Match(lines[i], "^\\s*#.*").Success // If not a comment
                && !Regex.Match(lines[i], "^\\s*$").Success) // If not blank line
            {
                statememnts.Add(new DfdCodeLine()
                {
                    Statement = lines[i].TrimEnd(),
                    LineNumber = i
                });
            }
        }

        return statememnts.ToArray();
    }

}