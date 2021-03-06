﻿namespace BetterMembership.IntegrationTests.MembershipProvider
{
    using BetterMembership.IntegrationTests.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class FindUsersByUserNameTests : BaseMembershipTests
    {
        [TestCase(SqlClientProviderWithEmail)]
        [TestCase(SqlClientProviderWithUniqueEmail)]
        [TestCase(SqlClientCeProviderWithEmail)]
        [TestCase(SqlClientCeProviderWithUniqueEmail)]
        [TestCase(SqlClientCeProviderWithoutEmail)]
        public void GivenConfirmedUsersWhenFindUsersbyUserNameThenPageResultsSuccessFully(string providerName)
        {
            // arrange
            var testClass = this.WithProvider(providerName);
            const int PageSize = 2;
            const int PageIndex = 1;
            string prefix1;
            string prefix2;
            const int UserGroup1Count = 8;
            const int UserGroup2Count = 3;
            var users1 = testClass.WithConfirmedUsers(UserGroup1Count, out prefix1).Value;
            var users2 = testClass.WithConfirmedUsers(UserGroup2Count, out prefix2).Value;

            // act
            int totalRecords1;
            int totalRecords2;
            int totalRecords3;
            var results1 = testClass.FindUsersByName(prefix1, PageIndex, PageSize, out totalRecords1);
            var results2 = testClass.FindUsersByName(prefix2, PageIndex, PageSize, out totalRecords2);
            var results3 = testClass.FindUsersByName("missing", PageIndex, PageSize, out totalRecords3);

            // assert
            Assert.That(results1, Is.Not.Null);
            Assert.That(results2, Is.Not.Null);
            Assert.That(results3, Is.Not.Null);
            Assert.That(results1.Count, Is.EqualTo(PageSize));
            Assert.That(results2.Count, Is.EqualTo(1));
            Assert.That(results3.Count, Is.EqualTo(0));
            Assert.That(totalRecords1, Is.EqualTo(UserGroup1Count));
            Assert.That(totalRecords2, Is.EqualTo(UserGroup2Count));
            Assert.That(totalRecords3, Is.EqualTo(0));
            Assert.That(results1.ToArray()[0].UserName, Is.EqualTo(users1[2].UserName));
            Assert.That(results2.ToArray()[0].UserName, Is.EqualTo(users2[2].UserName));
        }

        [TestCase(SqlClientProviderWithEmail)]
        [TestCase(SqlClientProviderWithUniqueEmail)]
        [TestCase(SqlClientCeProviderWithEmail)]
        [TestCase(SqlClientCeProviderWithUniqueEmail)]
        [TestCase(SqlClientCeProviderWithoutEmail)]
        public void GivenConfirmedUsersWhenFindUsersbyUserNameThenFirstPageCorrect(string providerName)
        {
            // arrange
            var testClass = this.WithProvider(providerName);
            const int PageSize = 2;
            const int PageIndex = 0;
            const int TotalUsers = 3;
            string prefix1;
            var users1 = testClass.WithConfirmedUsers(TotalUsers, out prefix1).Value;

            // act
            int totalRecords1;
            var results1 = testClass.FindUsersByName(prefix1, PageIndex, PageSize, out totalRecords1);

            // assert
            Assert.That(results1, Is.Not.Null);
            Assert.That(results1.Count, Is.EqualTo(PageSize));
            Assert.That(totalRecords1, Is.EqualTo(TotalUsers));
            Assert.That(results1.ToArray()[0].UserName, Is.EqualTo(users1[0].UserName));
            Assert.That(results1.ToArray()[1].UserName, Is.EqualTo(users1[1].UserName));
        }

        [TestCase(SqlClientProviderWithEmail)]
        [TestCase(SqlClientProviderWithUniqueEmail)]
        [TestCase(SqlClientCeProviderWithEmail)]
        [TestCase(SqlClientCeProviderWithUniqueEmail)]
        [TestCase(SqlClientCeProviderWithoutEmail)]
        public void GivenConfirmedUsersWhenFindUsersbyUserNameThenLastPageCorrect(string providerName)
        {
            // arrange
            var testClass = this.WithProvider(providerName);
            const int PageSize = 2;
            const int PageIndex = 1;
            const int TotalUsers = 3;
            string prefix1;
            var users1 = testClass.WithConfirmedUsers(TotalUsers, out prefix1).Value;

            // act
            int totalRecords1;
            var results1 = testClass.FindUsersByName(prefix1, PageIndex, PageSize, out totalRecords1);

            // assert
            Assert.That(results1, Is.Not.Null);
            Assert.That(results1.Count, Is.EqualTo(1));
            Assert.That(totalRecords1, Is.EqualTo(TotalUsers));
            Assert.That(results1.ToArray()[0].UserName, Is.EqualTo(users1[2].UserName));
        }
    }
}