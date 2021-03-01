using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Park.API.DTOs;
using Park.Core.Interfaces;
using Park.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Park.API.Controllers
{
    [Route("api/v{version:apiVersion}/nationalpark")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "v1")]
    public class NationalParkV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public NationalParkV2Controller(INationalParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get List of national park
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var objList = _parkRepository.GetNationalParks().FirstOrDefault();
            return Ok(_mapper.Map<NationalParkDto>(objList));
        }       
    }
}
