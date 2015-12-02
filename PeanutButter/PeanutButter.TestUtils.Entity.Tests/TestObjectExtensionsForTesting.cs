﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace PeanutButter.TestUtils.Entity.Tests
{
    [TestFixture]
    public class TestObjectExtensionsForTesting
    {

        public class SomePOCO
        {
            public int SomeGeneratedPOCOId { get; set; }
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int SomeOtherNotGeneratedPOCOId { get; set; }
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int SomeNotGeneratedPOCOId { get; set; }

            public string PropertyWithoutMaxLength { get; set; }
            [MaxLength(50)]
            public string PropertyWithMaxLength { get; set; }
            [Required]
            public string RequiredProperty { get; set; }
            public string NotRequiredProperty { get; set; }

            public int NotAForeignKey { get; set; }
            [ForeignKey("foo")]
            public int WrongForeinKey { get; set; }
            [ForeignKey("SomeOtherID")]
            public int ValidForeignKey { get; set; }
        }

        private SomePOCO Create()
        {
            return new SomePOCO();
        }

        [Test]
        public void ShouldHaveMaxLengthOf_WhenPropertyHasNoMaxLengthAttribute_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var expected = RandomValueGen.GetRandomInt();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<AssertionException>(() => sut.ShouldHaveMaxLengthOf(expected, o => o.PropertyWithoutMaxLength));

            //---------------Test Result -----------------------
            StringAssert.Contains("no maxlength attribute", ex.Message.ToLower());
        }

        [Test]
        public void ShouldHaveMaxLengthOf_WhenPropertyHasMaxLengthAttributeWithDifferentValue_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var expected = RandomValueGen.GetRandomInt(10, 20);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<AssertionException>(() => sut.ShouldHaveMaxLengthOf(expected, o => o.PropertyWithMaxLength));

            //---------------Test Result -----------------------
            StringAssert.Contains("incorrect maxlength", ex.Message.ToLower());
        }

        [Test]
        public void ShouldHaveMaxLengthOf_WhenPropertyHasMaxLengthAttributeWithSameValue_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => sut.ShouldHaveMaxLengthOf(50, o => o.PropertyWithMaxLength));

            //---------------Test Result -----------------------
        }

        [Test]
        public void ShouldBeRequired_WhenPropertyHasNoRequiredAttribute_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<AssertionException>(() => sut.ShouldBeRequired(o => o.NotRequiredProperty));

            //---------------Test Result -----------------------
            StringAssert.Contains("no required attribute", ex.Message.ToLower());
        }

        [Test]
        public void ShouldBeRequired_WhenPropertyHasRequiredAttribute_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => sut.ShouldBeRequired(o => o.RequiredProperty));

            //---------------Test Result -----------------------
        }

        [Test]
        public void ShouldNotBeDatabaseGenerated_WhenPropertyDoesNotHaveAttributeWithRequiredValue_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<AssertionException>(() => sut.ShouldNotBeDatabaseGenerated(o => o.SomeOtherNotGeneratedPOCOId));

            //---------------Test Result -----------------------
            StringAssert.Contains("expected [databasegenerated(databasegeneratedoption.none)]", ex.Message.ToLower());
        }

        [Test]
        public void ShouldNotBeDatabaseGenerated_WhenPropertyHasAttributeWithRequiredValue_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => sut.ShouldNotBeDatabaseGenerated(o => o.SomeNotGeneratedPOCOId));

            //---------------Test Result -----------------------
        }

        [Test]
        public void ShouldHaveForeignKey_WhenNoForeignKeyAttribute_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<AssertionException>(() => sut.ShouldHaveForeignKey("SomeOtherID", o => o.NotAForeignKey));

            //---------------Test Result -----------------------
            StringAssert.Contains("no foreignkey attribute", ex.Message.ToLower());
        }

        [Test]
        public void ShouldHaveForeignKey_WhenHasForeignKeyAttribute_ButIncorrectName_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<AssertionException>(() => sut.ShouldHaveForeignKey("SomeOtherID", o => o.WrongForeinKey));

            //---------------Test Result -----------------------
            StringAssert.Contains("incorrect foreignkey", ex.Message.ToLower());
        }

        [Test]
        public void ShouldHaveForeignKey_WhenHasForeignKeyAttribute_WithCorrectName_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>sut.ShouldHaveForeignKey("SomeOtherID", o => o.ValidForeignKey));

            //---------------Test Result -----------------------
        }



    }
}