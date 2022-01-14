using System.Collections.Generic;
using System.Linq;

namespace Shared_Guesser.Helpers
{
    public class PaginatedData<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int RecordsReturned { get; set; }
        public int TotalRecordsFound { get; set; }
        public int CurrentPage { get; set; }
        public int RecordsPerPage { get; set; }
    }

    public class DataProviderHelper<T>
    {
        public PaginatedData<T> GetPaginatedData(List<T> foundData, int currentPage, int itemsOnPage, int totalData)
        {
            PaginatedData<T> paginatedDataResult = new PaginatedData<T>
            {
                CurrentPage = currentPage,
                RecordsPerPage = itemsOnPage,
                TotalRecordsFound = totalData
            };

            if (foundData != null && foundData.Any() && currentPage > 0 && itemsOnPage > 0)
            {
                paginatedDataResult.Data = foundData;
                paginatedDataResult.RecordsReturned = foundData.Count();
            }

            return paginatedDataResult;
        }

        public int GetItemsToSkip(int initialDataCount, int currentPage, int itemsOnPage)
        {
            int itemsToSkip = -1;

            if (initialDataCount > 0 && currentPage > 0 && itemsOnPage > 0)
            {
                itemsToSkip = (currentPage - 1) * itemsOnPage;
            }

            return itemsToSkip;
        }

        public int GetItemsToTake(int initialDataCount, int currentPage, int itemsOnPage)
        {
            int itemsToTake = -1;

            if (initialDataCount > 0 && currentPage > 0 && itemsOnPage > 0)
            {
                int itemsToSkip = (currentPage - 1) * itemsOnPage;

                if (initialDataCount > itemsToSkip)
                {
                    int remainingDataCount = initialDataCount - itemsToSkip;
                    itemsToTake = (itemsOnPage <= remainingDataCount) ? itemsOnPage : remainingDataCount;
                }
            }
            return itemsToTake;
        }
    }
}
