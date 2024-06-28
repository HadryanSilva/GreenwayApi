using GreenwayApi.Controllers;
using GreenwayApi.Data;
using GreenwayApi.DTOs.User;
using GreenwayApi.Model;
using GreenwayApi.Model.AutenticatorModel;
using GreenwayApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SecureIdentity.Password;

namespace GreenwayApiTests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Register_ReturnsCreatedStatus()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);
            var controller = new AuthController(context);

            var userModel = new UserRequestDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "password123",
                Role = UserRole.User
            };

            // Act
            var result = await controller.Register(context, userModel);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsOkStatus()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);
            var tokenServiceMock = new Mock<ITokenService>();

            // Adicionando um usuário de teste ao banco de dados em memória
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = PasswordHasher.Hash("password123"),
                Role = UserRole.User
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Configurando o mock para retornar um token de teste
            tokenServiceMock.Setup(service => service.GenerateToken(It.IsAny<User>())).Returns("testtoken");

            // Act
            var result = await controller.Login(loginModel, context, tokenServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("testtoken", okResult.Value);
        }
    }
}
