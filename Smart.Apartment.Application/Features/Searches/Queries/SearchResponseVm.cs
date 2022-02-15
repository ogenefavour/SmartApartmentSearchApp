using Smart.Apartment.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Apartment.Application.Features.Searches.Queries
{
    public class SearchResponseVm
    {
        public ICollection<SearchListContentVm> Content { get; set; }
    }
}
