using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace C42A.Ra180.Infrastructure
{
    public class PassiveKeyCalculator
    {
        private readonly IFormatProvider _formatProvider;
        private static volatile PassiveKeyCalculator _passive;
        private static readonly object _syncroot = new object();

        public PassiveKeyCalculator(IFormatProvider formatProvider)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }

        public PassiveKeyCalculator()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public static PassiveKeyCalculator Default
        {
            get
            {
                if (_passive == null)
                {
                    lock (_syncroot)
                    {
                        if (_passive == null)
                            _passive = new PassiveKeyCalculator();
                    }
                }

                return _passive;
            }
        }

        public string GetPny(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var pnyGroups = input.Split(',', ' ', ';', '\n').Select(s => s.Trim()).ToArray();
            return GetPny(pnyGroups);
        }

        private static bool IsAllDigits(string s)
        {
            return s.All(c => Char.IsDigit(c));
        }

        public string GetPny(string[] pnyGroups)
        {
            if (pnyGroups == null) throw new ArgumentNullException("pnyGroups");
            var threeLetterPnyGroups = pnyGroups.Select(s => s.Substring(0, 3)).ToArray();
            var result = "";
            for (var i = 0; i < 3; i++)
            {
                var i1 = i;
                var sum = Calculate(threeLetterPnyGroups.Select(s => s[i1]));
                result += ToString(sum);
            }

            return result;
        }

        public string GetPnyGroup(string threeLetterPny)
        {
            return threeLetterPny + GetPnyChecksum(threeLetterPny);
        }

        public string GetPnyChecksum(string threeLetterPny)
        {
            if (threeLetterPny == null) throw new ArgumentNullException("threeLetterPny");
            if (threeLetterPny.Length != 3) throw new ArgumentException("Input must be three characters long, exactly.", "threeLetterPny");
            if (!IsAllDigits(threeLetterPny)) throw new ArgumentException("Input must be consisting of digits only", "threeLetterPny");
            var sum = Calculate(threeLetterPny.ToCharArray());
            var result = ToString(sum);
            return result;
        }

        public PassiveKey GenerateNewKey()
        {
            var groups = GenerateNewKeyGroups().Select(s => GetPnyGroup(s)).ToArray();
            var result = new PassiveKey
            {
                PN1 = groups[0],
                PN2 = groups[1],
                PN3 = groups[2],
                PN4 = groups[3],
                PN5 = groups[4],
                PN6 = groups[5],
                PN7 = groups[6],
                PN8 = groups[7],
                PN9 = groups[8],
                PNY = GetPny(groups)
            };

            return result;
        }

        private IEnumerable<string> GenerateNewKeyGroups(Random random = null, int groups = 9)
        {
            random = random ?? new Random();
            for (var i = 0; i < groups; i++)
            {
                var values = new[]
                {
                    random.Next(0, 7),
                    random.Next(0, 7),
                    random.Next(0, 7),
                };

                yield return string.Join("", values.Select(n => ToString(n)));
            }
        }

        private int Calculate(IEnumerable<char> values)
        {
            return Calculate(values.Select(c => ToInt32(c)));
        }

        private static int Calculate(IEnumerable<int> values)
        {
            return values.Aggregate(0, (current, value) => current ^ value);
        }

        private string ToString(int n)
        {
            return n.ToString(_formatProvider);
        }

        private int ToInt32(char c)
        {
            return int.Parse(c.ToString(_formatProvider));
        }
    }
}
