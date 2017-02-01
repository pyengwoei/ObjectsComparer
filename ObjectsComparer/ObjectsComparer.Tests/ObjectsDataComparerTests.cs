﻿using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using ObjectsComparer.Tests.TestClasses;

namespace ObjectsComparer.Tests
{
    [TestFixture]
    public class ObjectsDataComparerTests
    {
        [Test]
        public void PropertyEquality()
        {
            var a1 = new A { Property = 10, Property3 = 5 };
            var a2 = new A { Property = 10, Property3 = 8 };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void PropertyInequality()
        {
            var a1 = new A { Property = 10 };
            var a2 = new A { Property = 8 };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("Property", differences.First().MemberPath);
            Assert.AreEqual("10", differences.First().Value1);
            Assert.AreEqual("8", differences.First().Value2);
        }

        [Test]
        public void ReadOnlyPropertyEquality()
        {
            var a1 = new A(1.99);
            var a2 = new A(1.99);
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void ReadOnlyPropertyInequality()
        {
            var a1 = new A(1.99);
            var a2 = new A(0.89);
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ReadOnlyProperty", differences.First().MemberPath);
            Assert.AreEqual("1.99", differences.First().Value1);
            Assert.AreEqual("0.89", differences.First().Value2);
        }

        [Test]
        public void ProtectedProperty()
        {
            var a1 = new A(true);
            var a2 = new A(false);
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void FieldEquality()
        {
            var a1 = new A { Field = 9 };
            var a2 = new A { Field = 9 };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void FieldInequality()
        {
            var a1 = new A { Field = 10 };
            var a2 = new A { Field = 8 };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("Field", differences.First().MemberPath);
            Assert.AreEqual("10", differences.First().Value1);
            Assert.AreEqual("8", differences.First().Value2);
        }

        [Test]
        public void ReadOnlyFieldEquality()
        {
            var a1 = new A("Str1");
            var a2 = new A("Str1");
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void ReadOnlyFieldInequality()
        {
            var a1 = new A("Str1");
            var a2 = new A("Str2");
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ReadOnlyField", differences.First().MemberPath);
            Assert.AreEqual("Str1", differences.First().Value1);
            Assert.AreEqual("Str2", differences.First().Value2);
        }

        [Test]
        public void ProtectedField()
        {
            var a1 = new A(5);
            var a2 = new A(6);
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void ClassPropertyEquality()
        {
            var a1 = new A { ClassB = new B { Property1 = "Str1" } };
            var a2 = new A { ClassB = new B { Property1 = "Str1" } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void ClassPropertyInequality()
        {
            var a1 = new A { ClassB = new B { Property1 = "Str1" } };
            var a2 = new A { ClassB = new B { Property1 = "Str2" } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ClassB.Property1", differences.First().MemberPath);
            Assert.AreEqual("Str1", differences.First().Value1);
            Assert.AreEqual("Str2", differences.First().Value2);
        }

        [Test]
        public void ClassPropertyInequalityFirstNull()
        {
            var a1 = new A();
            var a2 = new A { ClassB = new B { Property1 = "Str2" } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ClassB", differences.First().MemberPath);
            Assert.AreEqual("", differences.First().Value1);
            Assert.AreEqual(a2.ClassB.ToString(), differences.First().Value2);
        }

        [Test]
        public void ClassPropertyInequalitySecondNull()
        {
            var a1 = new A { ClassB = new B { Property1 = "Str2" } };
            var a2 = new A();
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ClassB", differences.First().MemberPath);
            Assert.AreEqual(a1.ClassB.ToString(), differences.First().Value1);
            Assert.AreEqual("", differences.First().Value2);
        }

        [Test]
        public void NoRecursiveComparison()
        {
            var a1 = new A { ClassB = new B { Property1 = "Str1" } };
            var a2 = new A { ClassB = new B { Property1 = "Str2" } };
            var comparer = new ObjectsDataComparer<A>(false);

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void ValueTypeArrayEquality()
        {
            var a1 = new A { IntArray = new[] { 1, 2 } };
            var a2 = new A { IntArray = new[] { 1, 2 } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void PrimitiveTypeArrayInequalityCount()
        {
            var a1 = new A { IntArray = new[] { 1, 2 } };
            var a2 = new A { IntArray = new[] { 1, 2, 3 } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("IntArray[]", differences.First().MemberPath);
            Assert.AreEqual("2", differences.First().Value1);
            Assert.AreEqual("3", differences.First().Value2);
        }

        [Test]
        public void PrimitiveTypeArrayInequalityMember()
        {
            var a1 = new A { IntArray = new[] { 1, 2 } };
            var a2 = new A { IntArray = new[] { 1, 3 } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("IntArray[1]", differences.First().MemberPath);
            Assert.AreEqual("2", differences.First().Value1);
            Assert.AreEqual("3", differences.First().Value2);
        }

        [Test]
        public void PrimitiveTypeArrayInequalityFirstNullNull()
        {
            var a1 = new A();
            var a2 = new A { IntArray = new int[0] };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("IntArray", differences.First().MemberPath);
            Assert.AreEqual(string.Empty, differences.First().Value1);
            Assert.AreEqual(a2.IntArray.ToString(), differences.First().Value2);
        }

        [Test]
        public void PrimitiveTypeArrayInequalitySecondNullNull()
        {
            var a1 = new A { IntArray = new int[0] };
            var a2 = new A();
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("IntArray", differences.First().MemberPath);
            Assert.AreEqual(a1.IntArray.ToString(), differences.First().Value1);
            Assert.AreEqual(string.Empty, differences.First().Value2);
        }

        [Test]
        public void ClassArrayEquality()
        {
            var a1 = new A { ArrayOfB = new[] { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { ArrayOfB = new[] { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void ClassArrayInequalityCount()
        {
            var a1 = new A { ArrayOfB = new[] { new B { Property1 = "Str1" } } };
            var a2 = new A { ArrayOfB = new[] { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ArrayOfB[]", differences.First().MemberPath);
            Assert.AreEqual("1", differences.First().Value1);
            Assert.AreEqual("2", differences.First().Value2);
        }

        [Test]
        public void ClassArrayInequalityProperty()
        {
            var a1 = new A { ArrayOfB = new[] { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { ArrayOfB = new[] { new B { Property1 = "Str1" }, new B { Property1 = "Str3" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ArrayOfB[1].Property1", differences.First().MemberPath);
            Assert.AreEqual("Str2", differences.First().Value1);
            Assert.AreEqual("Str3", differences.First().Value2);
        }

        [Test]
        public void CollectionEquality()
        {
            var a1 = new A { CollectionOfB = new Collection<B> { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { CollectionOfB = new Collection<B> { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void CollectionInequalityCount()
        {
            var a1 = new A { CollectionOfB = new Collection<B> { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { CollectionOfB = new Collection<B> { new B { Property1 = "Str1" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("CollectionOfB[]", differences.First().MemberPath);
            Assert.AreEqual("2", differences.First().Value1);
            Assert.AreEqual("1", differences.First().Value2);
        }

        [Test]
        public void CollectionInequalityProperty()
        {
            var a1 = new A { CollectionOfB = new Collection<B> { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { CollectionOfB = new Collection<B> { new B { Property1 = "Str1" }, new B { Property1 = "Str3" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("CollectionOfB[1].Property1", differences.First().MemberPath);
            Assert.AreEqual("Str2", differences.First().Value1);
            Assert.AreEqual("Str3", differences.First().Value2);
        }

        [Test]
        public void ClassImplementsCollectionEquality()
        {
            var a1 = new A { ClassImplementsCollectionOfB = new CollectionOfB { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { ClassImplementsCollectionOfB = new CollectionOfB { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void ClassImplementsCollectionInequalityCount()
        {
            var a1 = new A { ClassImplementsCollectionOfB = new CollectionOfB { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { ClassImplementsCollectionOfB = new CollectionOfB { new B { Property1 = "Str1" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ClassImplementsCollectionOfB[]", differences.First().MemberPath);
            Assert.AreEqual("2", differences.First().Value1);
            Assert.AreEqual("1", differences.First().Value2);
        }

        [Test]
        public void ClassImplementsCollectionInequalityProperty()
        {
            var a1 = new A { ClassImplementsCollectionOfB = new CollectionOfB { new B { Property1 = "Str1" }, new B { Property1 = "Str2" } } };
            var a2 = new A { ClassImplementsCollectionOfB = new CollectionOfB { new B { Property1 = "Str1" }, new B { Property1 = "Str3" } } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("ClassImplementsCollectionOfB[1].Property1", differences.First().MemberPath);
            Assert.AreEqual("Str2", differences.First().Value1);
            Assert.AreEqual("Str3", differences.First().Value2);
        }

        [Test]
        public void InterfacePropertyEquality()
        {
            var a1 = new A { IntefaceProperty = new TestInterfaceImplementation1 { Property = "Str1" } };
            var a2 = new A { IntefaceProperty = new TestInterfaceImplementation2 { Property = "Str1", AnotherProperty = 50 } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void InterfacePropertyInequality()
        {
            var a1 = new A { IntefaceProperty = new TestInterfaceImplementation1 { Property = "Str1" } };
            var a2 = new A { IntefaceProperty = new TestInterfaceImplementation2 { Property = "Str2", AnotherProperty = 50 } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("IntefaceProperty.Property", differences.First().MemberPath);
            Assert.AreEqual("Str1", differences.First().Value1);
            Assert.AreEqual("Str2", differences.First().Value2);
        }

        [Test]
        public void StructPropertyEquality()
        {
            var a1 = new A { StructProperty = new TestStruct { FieldA = "FA", FieldB = "FB" } };
            var a2 = new A { StructProperty = new TestStruct { FieldA = "FA", FieldB = "FB" } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void StructPropertyInequality()
        {
            var a1 = new A { StructProperty = new TestStruct { FieldA = "FA", FieldB = "FB" } };
            var a2 = new A { StructProperty = new TestStruct { FieldA = "FA", FieldB = "FBB" } };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("StructProperty.FieldB", differences.First().MemberPath);
            Assert.AreEqual("FB", differences.First().Value1);
            Assert.AreEqual("FBB", differences.First().Value2);
        }

        [Test]
        public void EnumPropertyEquality()
        {
            var a1 = new A { EnumProperty = TestEnum.Value1 };
            var a2 = new A { EnumProperty = TestEnum.Value1 };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2);

            CollectionAssert.IsEmpty(differences);
        }

        [Test]
        public void EnumPropertyInequality()
        {
            var a1 = new A { EnumProperty = TestEnum.Value1 };
            var a2 = new A { EnumProperty = TestEnum.Value2 };
            var comparer = new ObjectsDataComparer<A>();

            var differences = comparer.Compare(a1, a2).ToList();

            CollectionAssert.IsNotEmpty(differences);
            Assert.AreEqual("EnumProperty", differences.First().MemberPath);
            Assert.AreEqual("Value1", differences.First().Value1);
            Assert.AreEqual("Value2", differences.First().Value2);
        }
    }
}