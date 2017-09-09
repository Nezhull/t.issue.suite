using System;
using T.Issue.Commons.Utils;
using Xunit;
using Assert = Xunit.Assert;

namespace T.Issue.Commons.Test.Utils
{
    public class PersonalCodeUtilsTests
    {
        [Fact]
        public void IsValidTest()
        {
            Assert.False(PersonalCodeUtils.IsValid("38804161729"));
            Assert.True(PersonalCodeUtils.IsValid("38804161728"));
        }

        [Fact]
        public void GetDateOfBirthTest()
        {
            DateTime expected = new DateTime(1988, 4, 16, 0, 0, 0);
            DateTime? result = PersonalCodeUtils.GetDateOfBirth("38804161728");

            Assert.Equal(expected, result);
        }
    }
}