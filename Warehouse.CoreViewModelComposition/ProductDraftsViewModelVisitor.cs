﻿using CoreViewModelComposition;
using HttpHelpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Warehouse.CoreViewModelComposition
{
    public class ProductDraftsViewModelVisitor : IProductDraftsViewModelVisitor
    {
        readonly IConfiguration _config;

        public ProductDraftsViewModelVisitor(IConfiguration config)
        {
            _config = config;
        }

        public async Task Visit(IEnumerable<dynamic> composedViewModels)
        {
            var ids = composedViewModels.Select(vm => vm.StockItemId).ToArray();
            if (ids.Any())
            {
                var apiUrl = _config.GetValue<string>("modules:warehouse:config:apiUrl");

                var client = new HttpClient();
                //WARN: should apply pagination
                var response = await client.GetAsync($"{apiUrl}StockItems/ByStockItem?ids={ string.Join(",", ids) }");
                dynamic[] stockItems = await response.Content.AsExpandoArrayAsync();

                foreach (var vm in composedViewModels)
                {
                    var obj = stockItems.Single(d => d.Id == vm.StockItemId);
                    vm.StockItemInfo = obj;
                }
            }
        }
    }
}
