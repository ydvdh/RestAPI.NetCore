using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Park.API.DTOs;
using Park.Core.Interfaces;
using Park.Core.Models;
using System.Collections.Generic;

namespace Park.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository parkRepository, IMapper mapper)
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
            var objList = _parkRepository.GetNationalParks();
            var objDto = new List<NationalParkDto>();

            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get a single national park
        /// </summary>
        /// <param name="nationalParkId">The id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _parkRepository.GetNationalPark(nationalParkId);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);

            //Without automapper we have to write below code
            //var objDto = new NationalParkDto()
            //{
            //    Id = obj.Id,
            //    Name = obj.Name,
            //    State = obj.State,
            //    Created = obj.Created,
            //};

            return Ok(objDto);
        }

        /// <summary>
        /// Creating new national park
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark(NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_parkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists!!");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_parkRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("",$"Something went wrong while saving {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id}, nationalParkObj);
        }

        /// <summary>
        /// Update the existing national park
        /// </summary>
        /// <param name="nationalParkId">Id of national park to update</param>
        /// <param name="nationalParkDto">National part to be update</param>
        /// <returns></returns>
        [HttpPut("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null|| nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_parkRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete the existing national park
        /// </summary>
        /// <param name="nationalParkId">ID of national park to delete</param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_parkRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }
            var nationalParkObj = _parkRepository.GetNationalPark(nationalParkId);
            if (!_parkRepository.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
