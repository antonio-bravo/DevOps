using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using GloboTicket.Catalog.Repositories;
using GloboTicket.Catalog.Controllers;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GloboTicket.Catalog.Tests
{
    public class EventControllerTests
    {
        private readonly Mock<IEventRepository> _mockRepo;
        private readonly Mock<ILogger<EventController>> _mockLogger;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockRepo = new Mock<IEventRepository>();
            _mockLogger = new Mock<ILogger<EventController>>();
            _controller = new EventController(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllEvents_ReturnsOk_WhenEventsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetEvents()).ReturnsAsync(new List<Event>());

            // Act
            var result = await _controller.GetAllEvents();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllEvents_ReturnsNotFound_WhenNoEventsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetEvents()).ReturnsAsync((List<Event>)null);

            // Act
            var result = await _controller.GetAllEvents();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetEvent_ReturnsOk_WhenEventExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.GetEventById(id)).ReturnsAsync(new Event());

            // Act
            var result = await _controller.GetEvent(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetEvent_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.GetEventById(id)).ReturnsAsync((Event)null);

            // Act
            var result = await _controller.GetEvent(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Add more tests for CreateEvent method here
    }
}