﻿using System;

namespace _1CProgrammerAssistant.Additions
{
    public static class AdditionsString
    {
        public static string RemoveStartText(this string source, string removingText)
        {
            if (source.StartsWith(removingText, true, null))
                return source.Substring(removingText.Length);
            else
                return source;
        }

        public static string RemoveEndText(this string source, string removingText)
        {
            if (source.EndsWith(removingText, true, null))
                return source.Remove(source.Length - removingText.Length);
            else
                return source;
        }

        public static string RemoveSpace(this string source)
            => source.Replace(" ", "");

        public static string Left(this string source, int length)
        {
            if (length > source.Length)
                return source;

            if (length < 0)
                return source;

            return source.Substring(0, length);
        }
    }
}
