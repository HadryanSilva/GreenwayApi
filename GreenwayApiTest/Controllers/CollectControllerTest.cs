using GreenwayApi.Controllers;
using GreenwayApi.Data;
using GreenwayApi.DTOs.Collect;
using GreenwayApi.DTOs.Request;
using GreenwayApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GreenwayApi.Tests
{
    public class CollectControllerTests
    {
        private readonly Mock<ICollectService> _mockCollectService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly CollectController _controller;

        public CollectControllerTests()
        {
            // Substitua CollectService por Mock de DbContext
            var mockDbContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockCollectService = new Mock<ICollectService>();

            _mockTokenService = new Mock<ITokenService>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = new DefaultHttpContext();
            _mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            _controller = new CollectController(
                _mockCollectService.Object,
                _mockTokenService.Object,
                _mockHttpContextAccessor.Object
            );
        }

        [Fact]
        public async Task FindAll_ReturnsOkStatus()
        {
            // Arrange
            var requestParams = new RequestParams();
            var collects = new List<CollectResponseDto> { new CollectResponseDto() };
            _mockCollectService.Setup(service => service.FindAll(It.IsAny<RequestParams>())).ReturnsAsync(collects);

            // Act
            var result = await _controller.FindAll(requestParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(collects, okResult.Value);
        }

        [Fact]
        public async Task FindAllByUser_ReturnsOkStatus()
        {
            // Arrange
            var requestParams = new RequestParams();
            var collects = new List<CollectResponseDto> { new CollectResponseDto() };
            _mockCollectService.Setup(service => service.FindAllByUser(It.IsAny<RequestParams>())).ReturnsAsync(collects);

            // Act
            var result = await _controller.FindAllByUser(requestParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(collects, okResult.Value);
        }

        [Fact]
        public void FindById_ReturnsOkStatus()
        {
            // Arrange
            var id = 1;
            var collect = new CollectResponseDto();

            _mockCollectService
                .Setup(service => service.FindById(It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(collect);

            // Act
            var result = _controller.FindById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(collect, okResult.Value);
        }

        [Fact]
        public void Save_ReturnsCreatedStatus()
        {
            // Arrange
            var collectGetRequest = new CollectPostRequestDto();
            var collectSaved = new CollectResponseDto { Id = 1 };
            _mockCollectService.Setup(service => service.Save(It.IsAny<CollectPostRequestDto>())).Returns(collectSaved);

            // Act
            var result = _controller.Save(collectGetRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(collectSaved, createdResult.Value);
            Assert.Equal("FindById", createdResult.ActionName);
            Assert.Equal(collectSaved.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public void Update_ReturnsOkStatus()
        {
            // Arrange
            var collectPutRequest = new CollectPutRequestDto();
            var collectUpdated = new CollectResponseDto();
            _mockCollectService.Setup(service => service.Update(It.IsAny<CollectPutRequestDto>(), It.IsAny<Dictionary<string, string>>())).Returns(collectUpdated);

            // Act
            var result = _controller.Update(collectPutRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(collectUpdated, okResult.Value);
        }

        [Fact]
        public void Delete_ReturnsNoContentStatus()
        {
            // Arrange
            var id = 1;
            _mockCollectService
                .Setup(service => service.Delete(It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()));

            // Act
            var result = _controller.Delete(id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}
