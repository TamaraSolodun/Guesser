using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataLayer_Guesser.Models;
using Shared_Guesser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using DataLayer_Guesser.RepositoryInterfaces;
using Shared_Guesser.Helpers;
using DataLayer_Guesser.AdditionalModels;
using BusinessLayer_Guesser.DTO;
using AutoMapper;
using BusinessLayer_Guesser.Helpers;
using Microsoft.Extensions.Configuration;
using BusinessLayer_Guesser.DTO.Responses;

namespace BusinessLayer_Guesser.Managers
{
    public class GameManager
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        

        public GameManager(IGameRepository gameRepository, IMapper mapper, IConfiguration config)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
            _config = config;
            
        }

        #region Game search

        public async Task<GameResponse> GetGameById(int id)
        {
            var game = _mapper.Map<GameResponse>(await _gameRepository.GetGameById(id));
            if (game == null)
            {
                throw new NotFoundException($"Game with provided Id: {id} not found.");
            }
            return game;
        }

        public async Task<PaginatedData<GameResponse>> GetGames(GameConfigs config)
        {
            PaginatedData<Game> gamesPaged = await _gameRepository.GetGames(config);
            return new PaginatedData<GameResponse>()
            {
                CurrentPage = gamesPaged.CurrentPage,
                RecordsPerPage = gamesPaged.RecordsPerPage,
                RecordsReturned = gamesPaged.RecordsReturned,
                TotalRecordsFound = gamesPaged.TotalRecordsFound,
                Data = _mapper.Map<List<GameResponse>>(gamesPaged.Data)
            };
        }

        #endregion

        #region Game management


        public async Task<GameResponse> CreateGame(Game gameToCreate)
        {
            var createdGame = _mapper.Map<GameResponse>(await _gameRepository.CreateGame(gameToCreate));
            return createdGame;
        }
        public async Task<bool> DeleteGame(int gameId)
        {
            return await _gameRepository.DeleteGame(gameId);
        }
        #endregion
    }
}
