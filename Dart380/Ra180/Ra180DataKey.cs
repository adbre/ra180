using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ra180
{
    public class Ra180DataKey
    {
        private readonly string[] _data;
        private readonly string _checksum;

        public Ra180DataKey(string[] input) : this(input, false)
        {
        }

        public Ra180DataKey(string[] input, bool includeLastDigit)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (input.Length != 8) throw new ArgumentException("Input must contain eight groups.", "input");
            _checksum = CalculateChecksum(input, includeLastDigit);
            _data = input.ToArray();
        }

        public string[] Data
        {
            get { return _data.ToArray(); }
        }

        public string Checksum { get { return _checksum; } }

        public static string CalculateChecksum(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var sum = 0;
            foreach (var c in input)
            {
                var n = int.Parse(c.ToString());
                sum ^= n;
            }

            return sum.ToString(CultureInfo.InvariantCulture);
        }

        public static string CalculateChecksum(string[] input)
        {
            return CalculateChecksum(input, false);
        }

        public static string CalculateChecksum(string[] input, bool includeLastDigit)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (input.Length < 1) throw new ArgumentException("Input must not be empty", "input");
            if (input.Any(string.IsNullOrEmpty)) throw new ArgumentException("Input must not contain null or empty parts", "input");
            if (input.Any(s => !s.All(Char.IsDigit))) throw new ArgumentException("No element in input can contain anything else than digits.", "input");
            if (input.Any(s => s.Length != input[0].Length)) throw new ArgumentException("All elements in input must have identical length", "input");

            if (!includeLastDigit)
                input = input.Select(s => s.Substring(0, s.Length - 1)).ToArray();

            var result = new StringBuilder(input[0].Length);

            for (var i = 0; i < input[0].Length; i++)
            {
                var index = i;
                var tmp = string.Join("", input.Select(s => s.Substring(index, 1)));
                result.Append(CalculateChecksum(tmp));
            }

            return result.ToString();
        }

        public static string AddChecksum(string data)
        {
            return data + CalculateChecksum(data);
        }

        public static bool IsValid(string pn)
        {
            if (pn == null) return false;
            if (pn.Length < 3) return false;
            if (pn.Any(c => !Char.IsDigit(c))) return false;
            return AddChecksum(pn.Substring(0, pn.Length - 1)) == pn;
        }

        public static Ra180DataKey Generate()
        {
            return Generate(8);
        }

        public static Ra180DataKey Generate(int groupCount)
        {
            return Generate(groupCount, 3);
        }

        public static Ra180DataKey Generate(int groupCount, int size)
        {
            var groups = new string[groupCount];

            var group = new StringBuilder(size + 1);
            var random = new Random();
            for (var i = 0; i < groupCount; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    group.Append(random.Next(0, 8));
                }

                groups[i] = AddChecksum(group.ToString());
                group.Length = 0;
            }

            return new Ra180DataKey(groups);
        }
    }
}