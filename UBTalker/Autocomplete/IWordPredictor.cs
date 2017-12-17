using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBTalker.Autocomplete
{
    /// <summary>
    /// Predict words
    /// </summary>
    interface IWordPredictor
    {
        /// <summary>
        /// Get the top suggestions for a given prefix
        /// </summary>
        /// <param name="prefix">The prefix to search</param>
        /// <param name="n">The maximum number of results to return</param>
        /// <returns></returns>
        ICollection<string> SuggestTop(string prefix, int n);
    }
}