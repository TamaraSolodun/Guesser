using DataLayer_Guesser.AdditionalModels;
using DataLayer_Guesser.Models;
using DataLayer_Guesser.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared_Guesser;
using Shared_Guesser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using static Shared_Guesser.Helpers.ExtensionHelper;

namespace DataLayer_Guesser.RepositoryImplementations
{
    public class GameRepository : IGameRepository
    {
        private readonly GuesserDBContext _context;

        public GameRepository(GuesserDBContext context, IConfiguration _config)
        {
            _context = context;
        }

        public async Task<Game> GetGameById(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(i => i.Id == id);
            
            return game;
        }

        public async Task<PaginatedData<Game>> GetGames(GameConfigs config)
        {
            var query = _context.Games.Include(i => i.Player).AsQueryable();

            //Other filter params
            if (config.PlayerId != null)
            {
                query.Where(game => game.PlayerId == config.PlayerId);
            }
            if (config.PlayedDateFrom != null)
            {
                query.Where(game => game.PlayedAt >= config.PlayedDateFrom);
            }
            if (config.PlayedDateTo != null)
            {
                query.Where(game => game.PlayedAt <= config.PlayedDateTo);
            }
            if (config.ResultType != null)
            {
                query.Where(game => game.GameResultType == config.ResultType);
            }

            int totalGamesCount = query.Count();
            DataProviderHelper<Game> paginatedResult = new DataProviderHelper<Game>();
            if (config.CurrentPage <= 0)
            {
                config.CurrentPage = 1;
            }
            if (config.ItemsOnPage <= 0)
            {
                config.ItemsOnPage = 10;
            }

            int itemsToSkip = paginatedResult.GetItemsToSkip(totalGamesCount, config.CurrentPage, config.ItemsOnPage);
            int itemsToTake = paginatedResult.GetItemsToTake(totalGamesCount, config.CurrentPage, config.ItemsOnPage);

            return new DataProviderHelper<Game>().GetPaginatedData(await query.Skip(itemsToSkip).Take(itemsToTake).ToListAsync(), config.CurrentPage, config.ItemsOnPage, totalGamesCount);
        }

        public async Task<Game> CreateGame(Game gameToCreate)
        {
            gameToCreate.Id = 0;
            gameToCreate.PlayedAt = DateTime.UtcNow;
            await _context.Games.AddAsync(gameToCreate);
            await _context.SaveChangesAsync();
            return await GetGameById(gameToCreate.Id);
        }
        public async Task<bool> DeleteGame(int gameId)
        {
            Game gameToDelete = await GetGameById(gameId);
            if (gameToDelete == null)
            {
                throw new NotFoundException("Game not found.");
            }
            _context.Games.Remove(gameToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
