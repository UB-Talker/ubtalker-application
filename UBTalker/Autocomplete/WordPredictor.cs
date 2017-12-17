using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBTalker.Autocomplete
{
 
    class WordPredictor
    {
        public static void Test()
        {
            IWordPredictor predictor = new FrequencyOptimizedWordPredictor("Resources/frequencies.txt");

            Console.WriteLine("---\n" + String.Join("\n", predictor.SuggestTop("th", 3)));
            Console.WriteLine("---\n" + String.Join("\n", predictor.SuggestTop("com", 3)));
            Console.WriteLine("---\n" + String.Join("\n", predictor.SuggestTop("tha", 3)));
        }
    }
}
