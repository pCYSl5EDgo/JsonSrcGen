using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System.Threading;
using System;

namespace UnitTests
{
    [Json]
    public class JsonDoubleClass
    {
        public double Age {get;set;}
        public double Height {get;set;}
        public double Min {get;set;}
        public double Max {get;set;}
        public double Zero {get;set;}
    }

    public class DoublePropertyTests : DoublePropertyTestsBase
    {
        protected override string ToJson(JsonDoubleClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonDoubleClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8DoublePropertyTests : DoublePropertyTestsBase
    {
        protected override string ToJson(JsonDoubleClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonDoubleClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class DoublePropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42.21,\"Height\":176.568,\"Max\":1.7976931348623157E+308,\"Min\":-1.7976931348623157E+308,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
            //pt-BR using comma so this checks that we are not using the current culture (json spec requires dot)
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
        }

        protected abstract string ToJson(JsonDoubleClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonDoubleClass()
            {
                Age = 42.21d,
                Height = 176.568d,
                Max = double.MaxValue,
                Min = double.MinValue,
                Zero = 0
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonDoubleClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonDoubleClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42.21d));
            Assert.That(jsonClass.Height, Is.EqualTo(176.568d));
            Assert.That(jsonClass.Min, Is.EqualTo(double.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(double.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}