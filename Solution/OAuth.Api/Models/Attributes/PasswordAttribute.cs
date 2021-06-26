﻿using System.ComponentModel.DataAnnotations;

namespace OAuth.Api.Models.Attributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute() { ErrorMessage = "Invalid password"; }

        private static readonly string[] require = new string
                [] { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijkllmnopqrstuvwxyz", "1234567890", @"!@#$%¨&*()_+{`^}:?><,./\-§=ºª" };

        public static string[] Require { get => require; }

        public override bool IsValid(object? obj)
        {
            string str = obj.ToString();
            for (int i = 0; i < Require.Length; i++)
            {
                bool contains = false;
                foreach (char letter in Require[i])
                {
                    if (str.Contains(letter))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
