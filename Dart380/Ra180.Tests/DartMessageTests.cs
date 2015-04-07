using NUnit.Framework;
using Ra180.Devices.Dart380;
using Ra180.Programs;

namespace Ra180.Tests
{
    [TestFixture]
    public class DartMessageTests
    {
        private DartFormatCollection _formats;
        private DartFormat _fmt100;

        [SetUp]
        public void SetUp()
        {

            _formats = new DartFormatCollection();
            _fmt100 = _formats.GetFormat(100);
        }

        [Test]
        public void Fmt100()
        {
            var expected = new[]
            {
                "TEXT:           ", // 1
                "                ", // 2
                "                ", // 3
                "                ", // 4
                "                ", // 5
                "                ", // 6
                "                ", // 7
                "                ", // 8
                "                ", // 9
                "                ", // 10
                "                ", // 11
                "                ", // 12
            };

            var actual = _fmt100.BodyFormat;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Editor()
        {
            var message = new DartMessage(_fmt100);
            message.Timestamp = "052156";
            message.Sender = "CR";

            var editor = new DartMessageEditor(message);
            editor.Write("RG");
            editor.MoveDown();
            editor.MoveDown();
            editor.MoveDown();
            editor.MoveDown();
            editor.MoveDown();
            editor.Write("HELLO WORLD");

            var actual = editor.ToStringArray();
            Assert.That(actual, Is.EqualTo(new[]
            {
                "TILL:RG         ",
                "                ",
                "052156*FR:CR    ",
                "                ",
                "FRÅN:     *U:   ",
                "TEXT:HELLO WORLD", // 1
                "                ", // 2
                "                ", // 3
                "                ", // 4
                "                ", // 5
                "                ", // 6
                "                ", // 7
                "                ", // 8
                "                ", // 9
                "                ", // 10
                "                ", // 11
                "                ", // 12
                "------SLUT------",
            }));
        }
    }
}