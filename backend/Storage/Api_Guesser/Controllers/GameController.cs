using AutoMapper;
using BusinessLayer_Guesser.DTO;
using BusinessLayer_Guesser.DTO.Requests;
using BusinessLayer_Guesser.DTO.Responses;
using BusinessLayer_Guesser.Managers;
using DataLayer_Guesser.AdditionalModels;
using DataLayer_Guesser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shared_Guesser;
using Shared_Guesser.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api_Guesser.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]/[action]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IConfiguration _config;
        private readonly GameManager _gameManager;

        public GameController(ILogger<GameController> logger, IConfiguration config, GameManager gameManager)
        {
            _config = config;
            _gameManager = gameManager;
            _logger = logger;
        }

        #region GameRegion

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "GameEndpoints" })]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGameById([FromQuery, Required] int gameId)
        {
            _logger.LogInformation($"Calling {nameof(GetGameById)}. gameId: {gameId}");

            GameResponse game = await _gameManager.GetGameById(gameId);

            return Ok(game);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "GameEndpoints" })]
        [ProducesResponseType(typeof(PaginatedData<GameResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGames([FromBody, Required] GameConfigs config)
        {
            _logger.LogInformation($"Calling {nameof(GetGames)}. config: {JsonConvert.SerializeObject(config)}");

            PaginatedData<GameResponse> foundData = await _gameManager.GetGames(config);

            return Ok(foundData);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { "GameEndpoints" })]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateGame([FromBody, Required] Game request)
        {
            _logger.LogInformation($"Calling {nameof(CreateGame)}. Game: {JsonConvert.SerializeObject(request)}");

            GameResponse createdGame = await _gameManager.CreateGame(request);
            return Ok(createdGame);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "GameEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteGame([FromQuery, Required] int gameId)
        {
            _logger.LogInformation($"Calling {nameof(DeleteGame)}. gameId: {gameId}");

            var response = await _gameManager.DeleteGame(gameId);
            return Ok(response);
        }

        #endregion
    }
}
