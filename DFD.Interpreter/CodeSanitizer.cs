using System.Text.RegularExpressions;

namespace DFD.Interpreter;

internal class CodeSanitizer
{
    public string StripCommentsAndBlankLines(string code)
    {
        // Use a regular expression to match and remove comments marked by "#" and entirely blank lines
        string commentlessCode = Regex.Replace(code, "^\\s*#.*", "", RegexOptions.Multiline);

        // Remove leading and trailing whitespaces from each line
        var strippedCode = StripEmptyLines(commentlessCode);
        return strippedCode;
    }

    string StripEmptyLines(string code)
    {
        var lines = code.Replace("\r\n", "\n").Split('\n');
        var newString = String.Empty;
        foreach (var line in lines)
        {
            if (!Regex.Match(line, "^\\s*$", RegexOptions.Singleline).Success)
            {
                newString = newString + line.TrimEnd() + '\n';
            }
        }

        return newString.TrimEnd();
    }
}