using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBTalker.Autocomplete
{
    /// <summary>
    /// A word predictor defined as a composition of many word predictors
    /// Each predictor will be checked in order until the required number of words is found
    /// </summary>
    class ChainedWordPredictor : IWordPredictor
    {
        private IWordPredictor[] _predictors;

        /// <summary>
        /// Create a new ChainedWordPredictor
        /// </summary>
        /// <param name="predictors">A list of IWordPredictors, in order of priority from highest to lowest</param>
        public ChainedWordPredictor(params IWordPredictor[] predictors)
        {
            _predictors = predictors;
        }

        public ICollection<string> SuggestTop(string prefix, int n)
        {
            HashSet<string> result = new HashSet<string>();

            foreach (IWordPredictor predictor in _predictors)
            {
                foreach (string word in predictor.SuggestTop(prefix, n - result.Count))
                {
                    result.Add(word);
                }

                if (result.Count == n)
                    break;
            }

            return result;
        }
    }
}
