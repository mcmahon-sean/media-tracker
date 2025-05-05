using media_tracker_desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop
{
    public static class DataFunctions
    {
        /// <summary>
        /// Sorts the list based on the query options.
        /// </summary>
        /// <typeparam name="T">The object type of the list.</typeparam>
        /// <param name="dataList">The list of data.</param>
        /// <param name="options">The query options.</param>
        /// <returns>The list of sorted data.</returns>
        public static List<T>? Sort<T>(List<T> dataList, QueryOptions<T> options)
        {
            // Query
            IQueryable<T> query = dataList.AsQueryable();

            // If there is a where condition in the query options,
            if (options.HasWhere)
            {
                // Add it.
                query = query.Where(options.Where);
            }

            // If there is an orderby condition in the query options,
            if (options.HasOrderBy)
            {
                // If it is asc,
                if (options.OrderByDirection == "asc")
                {
                    // Add orderby condition, ascending.
                    query = query.OrderBy(options.OrderBy);
                }
                // Else,
                else
                {
                    // Add orderby condition, descending.
                    query = query.OrderByDescending(options.OrderBy);
                }
            }

            try
            {
                return query.ToList();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
