using System;
using T.Issue.Commons.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Issue.Commons.Utils.Tests
{
    [TestClass]
    public class PersonalCodeUtilsTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            UnitTest.Assert.IsFalse(PersonalCodeUtils.IsValid("38804161729"));
            UnitTest.Assert.IsTrue(PersonalCodeUtils.IsValid("38804161728"));
        }

        [TestMethod]
        public void GetDateOfBirthTest()
        {
            DateTime expected = new DateTime(1988, 4, 16, 0, 0, 0);
            DateTime? result = PersonalCodeUtils.GetDateOfBirth("38804161728");

            UnitTest.Assert.IsTrue(expected.Equals(result));
        }
    }
}