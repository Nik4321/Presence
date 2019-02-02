using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Presence.Data;
using Presence.Data.Models;
using Presence.Infrastructure.Options;
using Presence.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Presence.Tests.Services
{
    [TestFixture]
    public class UserServicesTests
    {
        private PresenceDbContext db;
        private IOptions<JwtSettings> jwtSettings;

        private Mock<UserManager<User>> userManager;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .Build();

            this.jwtSettings = Options.Create(configuration.Get<JwtSettings>());
        }

        [SetUp]
        public void SetUp()
        {
            this.db = this.SeedDb();
            this.userManager = this.GetMockUserManager();
        }

        #region AllUsers method tests

        [Test]
        public void AllUsersReturnTypeUser()
        {
            // Arrange
            var userService = new UserService(
                this.db,
                this.userManager.Object,
                jwtSettings
                );

            // Act
            var users = userService.AllUsers();

            // Assert
            users.Should().AllBeOfType<User>();
        }

        [Test]
        public void AllUsersReturnsCorrectCount()
        {
            // Arrange
            var userService = new UserService(
                this.db,
                this.userManager.Object,
                jwtSettings
                );

            // Act
            var users = userService.AllUsers();

            // Assert
            users.Should().HaveCount(3).And.OnlyHaveUniqueItems();
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Create fake DB for testing
        /// </summary>
        /// <returns></returns>
        private PresenceDbContext SeedDb()
        {
            var dbOptions = new DbContextOptionsBuilder<PresenceDbContext>()
                .UseInMemoryDatabase("TestDb").Options;
            var db = new PresenceDbContext(dbOptions);

            // Create some fake users
            // Password for fake users: "123qweasD"
            if (db.Users.Count() == 0)
            {
                IList<User> users = new List<User>()
                {
                    new User
                    {
                        Id = 1,
                        UserName = "TestUserNameOne@email.com",
                        Email = "TestUserNameOne@email.com",
                        PasswordHash = "AQAAAAEAACcQAAAAEICWMwuHVjjRn+XLOyf/jAC02fyYBpnZdxjefrbRn2srlQpXAKmXm43KQrgZ2gA5xQ=="
                    },
                    new User
                    {
                        Id = 2,
                        UserName = "TestUserNameTwo@email.com",
                        Email = "TestUserNameTwo@email.com",
                        PasswordHash = "AQAAAAEAACcQAAAAEICWMwuHVjjRn+XLOyf/jAC02fyYBpnZdxjefrbRn2srlQpXAKmXm43KQrgZ2gA5xQ=="
                    },
                    new User
                    {
                        Id = 3,
                        UserName = "TestUserNameThree@email.com",
                        Email = "TestUserNameThree@email.com",
                        PasswordHash = "AQAAAAEAACcQAAAAEICWMwuHVjjRn+XLOyf/jAC02fyYBpnZdxjefrbRn2srlQpXAKmXm43KQrgZ2gA5xQ=="
                    }
                };

                db.Users.AddRange(users);
                db.SaveChanges();
            }

            return db;
        }

        private Mock<UserManager<User>> GetMockUserManager()
        {
            return new Mock<UserManager<User>>(
                     new Mock<IUserStore<User>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<IPasswordHasher<User>>().Object,
                     new IUserValidator<User>[0],
                     new IPasswordValidator<User>[0],
                     new Mock<ILookupNormalizer>().Object,
                     new Mock<IdentityErrorDescriber>().Object,
                     new Mock<IServiceProvider>().Object,
                     new Mock<ILogger<UserManager<User>>>().Object);
        }

        #endregion
    }
}
