using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordStrength
{
    public class Password
    {
        private int _strength = 0;
        private readonly string _value;

        private Password(string password)
        {
            _value = password;
        }
        
        public static int GetPasswordStrength(string str)
        {
            var password = new Password(str);
            password.FindComplexity();

            return password._strength;
        }
        
        private struct Properties
        {
            public int Digits, UpperCaseChars, LowerCaseChars, Duplicates, Len;
            public bool ContainsOnlyLetters, ContainsOnlyDigits;
        }
        
        private bool IsLatinLetter(char ch)
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        }
        
        private void CountCharProperties(char ch, ref Properties properties)
        {
            if (char.IsDigit(ch))
            {
                properties.Digits++;
                properties.ContainsOnlyDigits = false;
            }
            
            else if (IsLatinLetter(ch))
            {
                if (char.IsUpper(ch))
                {
                    properties.UpperCaseChars++;
                }
    
                if (char.IsLower(ch))
                {
                    properties.LowerCaseChars++;
                }
        
                properties.ContainsOnlyLetters = false;
            }
            
            else
            {
                throw new Exception("Password contains non-latin letters.");
            }
        }

        private int GetNumberOfDuplicates()
        {
            var duplicates = 0;
            var valueWithUniqueChars = _value.ToCharArray().Distinct();
            
            foreach (var ch in valueWithUniqueChars)
            {
                var currDuplicates = _value.Count(chr => ch == chr);
                if (currDuplicates > 1)
                {
                    duplicates += currDuplicates;
                }
            }

            return duplicates;
        }

        public void FindComplexity()
        {
            var properties = new Properties
            {
                Len = _value.Length,
                Duplicates = GetNumberOfDuplicates(),
                ContainsOnlyLetters = true, ContainsOnlyDigits = true
            };
            
            foreach (var ch in _value)
            {
                CountCharProperties(ch, ref properties);
            }

            _strength += AnalyseStrengthByLength(properties);
            _strength += AnalyseStrengthByLengthOfDigits(properties);
            _strength += AnalyseStrengthByUpperCase(properties);
            _strength += AnalyseStrengthByLowerCase(properties);
            _strength -= AnalyseStrengthByOnlyDigits(properties);
            _strength -= AnalyseStrengthByOnlyLetters(properties);
            _strength -= AnalyseStrengthByDuplicates(properties);
        }

        private static int AnalyseStrengthByLength(Properties properties)
        {
            return 4 * properties.Len;
        }

        private static int AnalyseStrengthByLengthOfDigits(Properties properties)
        {
            return 4 * properties.Digits;
        }

        private static int AnalyseStrengthByUpperCase(Properties properties)
        {
            if (properties.UpperCaseChars != 0)
            {
                return 2 * (properties.Len - properties.UpperCaseChars);
            }
            else
            {
                return 0;
            }
        }

        private static int AnalyseStrengthByLowerCase(Properties properties)
        {
            if (properties.LowerCaseChars != 0)
            {
                return 2 * (properties.Len - properties.LowerCaseChars);
            }
            else
            {
                return 0;
            }
        }

        private static int AnalyseStrengthByOnlyDigits(Properties properties)
        {
            return properties.ContainsOnlyDigits ? properties.Len : 0;
        }

        private static int AnalyseStrengthByOnlyLetters(Properties properties)
        {
            return properties.ContainsOnlyLetters ? properties.Len : 0;
        }

        private static int AnalyseStrengthByDuplicates(Properties properties)
        {
            return properties.Duplicates;
        }
    }
}