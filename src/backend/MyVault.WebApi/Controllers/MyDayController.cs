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
        public async Task<GenericResponse<Day?>> CreateAsync([FromBody] CreateDayRequest model)
        {
            try
            {
                var day = await _myDayService.CreateAsync(model);
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

        [HttpGet("all")]
        public async Task<GenericResponse<List<Day>>> GetAllAsync([FromQuery] BaseRequest model)
        {
            try
            {
                var dayList = await _myDayService.GetAsync(model);
                if (dayList.Data is null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return dayList;
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                return dayList;
            }
            catch
            {

                throw;
            }
        }

        [HttpGet(":id")]
        public async Task<GenericResponse<Day?>> GetByIdAsync(int id)
        {
            try
            {
                var day = await _myDayService.GetAsync(id);
                if (day.Data is null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return day;
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                return day;
            }
            catch
            {

                throw;
            }
        }
    }
}
