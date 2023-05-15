using Booking.DataStore.Interfaces;
using Booking.Web.Models;
using Booking.Web.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;
using Booking.DataStore;

namespace Booking.Web.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]/[action]")]
    public class BookingsController : ControllerBase
    {

        private readonly ILogger<BookingsController> _logger;
        private readonly IBookingRepository _bookingRepository;
        

        public BookingsController(IBookingRepository bookingRepository, ILogger<BookingsController> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }
        
        [SwaggerOperation(Summary = "Create a booking")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(BookingResponse), Description = "Booking request details." )]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse), Description = "Bad request error response.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiErrorResponse), Description = "Internal server error response.")]
        [SwaggerResponse((int)HttpStatusCode.Conflict, Type = typeof(ApiErrorResponse), Description = "Booking slot is full error response.")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]BookingRequest bookingRequest)
        {
            try
            {
                BookingValidator.ValidateBookingRequest(bookingRequest);

                var bookingStartTime = TimeOnly.ParseExact((string)bookingRequest.BookingTime,
                    BookingValidator.TimeFormat,
                    CultureInfo.InvariantCulture);
                var booking = new DataStore.Documents.Booking
                {
                    Name = bookingRequest.Name,
                    BookingStartTime = bookingStartTime,
                    BookingEndTime = bookingStartTime.AddMinutes(59)
                };
                var bookingRef = await _bookingRepository.AddAsync(booking);
                if (Guid.TryParse(bookingRef, out var result))
                {
                    var bookingResponse = new BookingResponse
                    {
                        BookingId = result
                    };

                    return Ok(bookingResponse);
                }
                else
                {
                    _logger.LogError("Invalid bookingRef returned from bookingRepository.AddAsync: {0}", bookingRef);
                    var error = new ApiErrorResponse { errorMessage = "Internal Server Error" };
                    return StatusCode(StatusCodes.Status500InternalServerError, error
                    );
                }

            }
            catch (BookingSlotFullException bookingSlotFullException)
            {
                _logger.LogError("{0}", bookingSlotFullException);
                var apiErrorResponse = new ApiErrorResponse()
                {
                    errorMessage = bookingSlotFullException.Message
                };
                return StatusCode(StatusCodes.Status409Conflict,
                    apiErrorResponse);
            }
            catch (BadHttpRequestException badHttpRequestEx)
            {
                _logger.LogError("{0}", badHttpRequestEx);
                var apiErrorResponse = new ApiErrorResponse()
                {
                    errorMessage = badHttpRequestEx.Message
                };
                return StatusCode(StatusCodes.Status400BadRequest,
                    apiErrorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}", ex);
                var error = new ApiErrorResponse{ errorMessage = "Internal Server Error"};
                
                return StatusCode(StatusCodes.Status500InternalServerError, error
                );
            }
        }
    }
}