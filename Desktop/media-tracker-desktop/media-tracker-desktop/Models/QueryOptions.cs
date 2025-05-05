using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models
{
    /// <summary>
    /// The class for query options. Set the properties for the query.
    /// </summary>
    /// <typeparam name="T">The object type of the data you are sorting.</typeparam>
    public class QueryOptions<T>
    {
        // Lambda expression that you would put in a where condition.
        public Expression<Func<T, bool>> Where { get; set; } = null!;
        // Lambda expression that you would put in an orderBy condition.
        public Expression<Func<T, Object>> OrderBy { get; set; } = null!;
        // asc / desc
        public string OrderByDirection { get; set; } = "asc";

        // Properties that indicate if a where or orderby exists.
        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }
}
