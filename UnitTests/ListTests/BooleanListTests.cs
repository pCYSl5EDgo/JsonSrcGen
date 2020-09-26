using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonList(typeof(bool))] 

namespace UnitTests.ListTests
{
    public class BooleanListTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[true,false]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<bool>(){true, false};

            //act
            var json = _convert.ToJson(list);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        } 

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((List<bool>)null);

            //assert
            Assert.That(json, Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<bool>();

            //act
            _convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.True);
            Assert.That(list[1], Is.False);
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<bool>(){false, false, false};

            //act
            list =_convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.True);
            Assert.That(list[1], Is.False);
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<bool>(){false, false, false};

            //act
            list = _convert.FromJson(list, "null");

            //assert
            Assert.That(list, Is.Null);
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var list = _convert.FromJson((List<bool>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.True);
            Assert.That(list[1], Is.False);
        }

        [Test]
        public void FromJson_EmptyListJson_EmptyList()
        {
            //arrange
            //act
            var list = _convert.FromJson((List<bool>)null, "[]");

            //assert
            Assert.That(list.Count, Is.EqualTo(0));
        }
    }
}