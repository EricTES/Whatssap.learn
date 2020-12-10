using System;
using Whatssap.Learn.Repository;
using Xunit;

namespace Whatssap.Learn.Services.Test
{
    public class UserServiceTests
    {
        private UserService CreateSut()
        {
            return new UserService(NSubstitute.Substitute.For<IUserRepository>());
        }

        [Fact]
        public void IsValidEmail_ValidValue_ReturnsTrue()
        {
            // Arrange
            var email = "test@gmail.com";
            var sut = CreateSut();

            // Act 
            var result = sut.IsValidEmail(email);

            // Assert
            Assert.True(result.isValidEmail);
        }

        
    }
}
