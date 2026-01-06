using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyVault.Application.Interfaces.Services;
using MyVault.Application.Models.Requests;
using MyVault.Application.Models.Responses;
using MyVault.Domain.Entities;

namespace MyVault.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyDayController(IMyDayService myDayService) : ControllerBase
    {
        private readonly IMyDayService _myDayService = myDayService;

        [HttpPost("create")]
        public async Task<GenericResponse<Day?>> Create([FromBody] CreateDayRequest model)
        {
            try
            {
                var day = await _myDayService.Create(model);
                if (day.Data is null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return day;
                }

                Response.StatusCode = (int)HttpStatusCode.Created;
                return day;
            }
            catch
            {

                throw;
            }
        }
    }
}
