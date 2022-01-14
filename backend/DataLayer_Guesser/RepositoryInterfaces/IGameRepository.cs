using DataLayer_Guesser.AdditionalModels;
using DataLayer_Guesser.Models;
using Shared_Guesser.Helpers;
using System.Threading.Tasks;

namespace DataLayer_Guesser.RepositoryInterfaces
{
    public interface IGameRepository
    {
        Task<Game> GetGameById(int id);
        Task<PaginatedData<Game>> GetGames(GameConfigs config);
        Task<Game> CreateGame(Game gameToCreate);
        Task<bool> DeleteGame(int gameId);
    }
}
