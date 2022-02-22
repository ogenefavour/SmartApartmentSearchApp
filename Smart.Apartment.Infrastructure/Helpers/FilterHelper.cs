using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Apartment.Infrastructure.Helpers
{
    public static class FilterHelper
    {
        public static string FilterSearchString(string stringsToClean, string filters) 
        {
            var wordsToRemove = filters.Split(',').ToList();

            return string.Join(" ", stringsToClean.Split(' ').Except(wordsToRemove));
        }
    }
}
