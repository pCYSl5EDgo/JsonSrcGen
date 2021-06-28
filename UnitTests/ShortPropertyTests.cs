using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonShortClass
    {
        public short Age {get;set;}
        public short Height {get;set;}
        public short Min {get;set;}
        public short Max {get;set;}
        public short Zero {get;set;}
    }

    public class ShortPropertyTests : ShortPropertyTestsBase
    {
        protected override string ToJson(JsonShortClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonShortClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8ShortPropertyTests : ShortPropertyTestsBase
    {
        protected override string ToJson(JsonShortClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonShortClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class ShortPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":32767,\"Min\":-32768,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }
        
        protected abstract string ToJson(JsonShortClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonShortClass()
            {
                Age = 42,
                Height = 176,
                Max = short.MaxValue,
                Min = short.MinValue,
                Zero = 0
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonShortClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonShortClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(short.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(short.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}