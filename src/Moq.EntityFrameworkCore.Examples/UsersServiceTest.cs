namespace Moq.EntityFrameworkCore.Examples
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Moq.EntityFrameworkCore;
    using Moq.EntityFrameworkCore.Examples.Users;
    using Moq.EntityFrameworkCore.Examples.Users.Entities;
    using Xunit;

    public class UsersServiceTest
    {
        private static readonly Fixture Fixture = new Fixture();

        [Fact]
        public void Given_ListOfUsers_When_AddingNewAccount_Then_CorrectAccountIsAdded()
        {
            // Arrange
            IList<User> users = GenerateNotLockedUsers();

            var userContextMock = new Mock<UsersContext>();
            userContextMock.Setup(x => x.Users).ReturnsDbSet(users);

            var usersService = new UsersService(userContextMock.Object);

            var lockedUser = Fixture.Build<User>().With(u => u.AccountLocked, true).Create();

            // Act
            usersService.AddUsers(lockedUser);

            // Assert
            Assert.Equal(3, userContextMock.Object.Users.Count());
        }

        [Fact]
        public void Given_ListOfUsersWithOneUserAccountLock_When_RemovingLockedAccount_Then_CorrectLockedUserIsRemoved()
        {
            // Arrange
            IList<User> users = GenerateNotLockedUsers();
            var lockedUser = Fixture.Build<User>().With(u => u.AccountLocked, true).Create();
            users.Add(lockedUser);

            var userContextMock = new Mock<UsersContext>();
            userContextMock.Setup(x => x.Users).ReturnsDbSet(users);

            var usersService = new UsersService(userContextMock.Object);

            // Act
            usersService.RemoveLockedUsers();

            // Assert
            Assert.Equal(2, userContextMock.Object.Users.Count());
        }

        [Fact]
        public void Given_ListOfUsersWithOneUserAccountLock_When_CheckingWhoIsLocked_Then_CorrectLockedUserIsReturned()
        {
            // Arrange
            IList<User> users = GenerateNotLockedUsers();
            var lockedUser = Fixture.Build<User>().With(u => u.AccountLocked, true).Create();
            users.Add(lockedUser);

            var userContextMock = new Mock<UsersContext>();
            userContextMock.Setup(x => x.Users).ReturnsDbSet(users);

            var usersService = new UsersService(userContextMock.Object);

            // Act
            var lockedUsers = usersService.GetLockedUsers();

            // Assert
            Assert.Equal(new List<User> {lockedUser}, lockedUsers);
        }

        [Fact]
        public async Task Given_ListOfUsersWithOneUserAccountLock_When_CheckingWhoIsLockedAsync_Then_CorrectLockedUserIsReturned()
        {
            // Arrange
            IList<User> users = GenerateNotLockedUsers();
            var lockedUser = Fixture.Build<User>().With(u => u.AccountLocked, true).Create();
            users.Add(lockedUser);

            var userContextMock = new Mock<UsersContext>();
            userContextMock.Setup(x => x.Users).ReturnsDbSet(users);

            var usersService = new UsersService(userContextMock.Object);

            // Act
            var lockedUsers = await usersService.GetLockedUsersAsync();

            // Assert
            Assert.Equal(new List<User> { lockedUser }, lockedUsers);
        }

        private static IList<User> GenerateNotLockedUsers()
        {
            IList<User> users = new List<User>
            {
                Fixture.Build<User>().With(u => u.AccountLocked, false).Create(),
                Fixture.Build<User>().With(u => u.AccountLocked, false).Create()
            };

            return users;
        }
    }
}