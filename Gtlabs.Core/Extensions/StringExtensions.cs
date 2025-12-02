namespace Gtlabs.Core.Extensions;

public static  class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var sb = new System.Text.StringBuilder(input.Length + 10);

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (char.IsUpper(c))
            {
                if (i > 0 && input[i - 1] != '_')
                    sb.Append('_');

                sb.Append(char.ToLower(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}