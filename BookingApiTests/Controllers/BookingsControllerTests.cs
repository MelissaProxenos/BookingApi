using Booking.DataStore.Interfaces;
using Booking.Web.Controllers;
using Booking.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingApiTests.Controllers
{
    public class BookingsControllerTests
    {
        private readonly Mock<IBookingRepository> _mockBookingRepository = new();
        private readonly Mock<ILogger<BookingsController>> _mockLogger = new();
        private BookingsController _bookingsController;

        [SetUp]
        public void Setup()
        {
            var bookingReference = "2086fa61-1f1a-4fd2-8ff2-1b172788b607";
            _mockBookingRepository
                .Setup(m => m.AddAsync(It.IsAny<Booking.DataStore.Documents.Booking>()))
                .ReturnsAsync(bookingReference).Verifiable();
            _bookingsController = new BookingsController(_mockBookingRepository.Object, _mockLogger.Object);

        }

        [Test]
        public async Task BookingController_Create_WhenValidRequest_ShouldReturnOk()
        {
            // Arrange
            var bookingRequest = new BookingRequest()
            {
                Name = "Bob Smith",
                BookingTime = "09:00",
            };

            // Act
            var result = await _bookingsController.Create(bookingRequest);
            var objResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(objResult);
            var bookingRef = objResult.Value as BookingResponse;
            Assert.IsNotNull(bookingRef);
            _mockBookingRepository.Verify(m => m.AddAsync(It.IsAny<Booking.DataStore.Documents.Booking>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public async Task BookingController_Create_WhenNoName_ShouldReturnBadRequest()
        {
            // Arrange
            var bookingRequest = new BookingRequest()
            {
                BookingTime = "09:00",
            };

            // Act
            var result = await _bookingsController.Create(bookingRequest);

            var statusCodeResult = result as ObjectResult;

            // Assert
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode.Value);
        }

        [Test]
        public async Task BookingController_Create_WhenOutsideBusinessHours_ShouldReturnBadRequest()
        {
            // Arrange
            var bookingRequest = new BookingRequest()
            {
                BookingTime = "07:00",
            };

            // Act
            var result = await _bookingsController.Create(bookingRequest);

            var statusCodeResult = result as ObjectResult;

            // Assert
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode.Value);
        }

        [Test]
        public async Task BookingController_Create_WhenNoBookingTime_ShouldReturnBadRequest()
        {
            // Arrange
            var bookingRequest = new BookingRequest()
            {
                Name = "Bob Smith",
            };

            // Act
            var result = await _bookingsController.Create(bookingRequest);

            var statusCodeResult = result as ObjectResult;

            // Assert
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode.Value);
        }
    }
}