﻿using CoreViewModelComposition;
using HttpHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;

namespace CustomerCare.CoreViewModelComposition
{
    public class ProductViewModelVisitor : IProductViewModelVisitor
    {
        readonly IConfiguration _config;

        public ProductViewModelVisitor(IConfiguration config)
        {
            _config = config;
        }

        public async Task Visit(dynamic composedViewModel)
        {
            var apiUrl = _config.GetValue<string>("modules:customerCare:config:apiUrl");

            var getRatingsUrl = $"{apiUrl}Raitings/ByStockItem?ids={ composedViewModel.StockItemId }";
            var getReviewsUrl = $"{apiUrl}Reviews/ByStockItem?ids={ composedViewModel.StockItemId }";

            //var getRatings = new HttpRequestMessage(
            //    method: HttpMethod.Get,
            //    requestUri: getRatingsUrl);

            //var getReviews = new HttpRequestMessage(
            //    method: HttpMethod.Get,
            //    requestUri: getReviewsUrl);

            //var getRatingsContent = new HttpMessageContent(getRatings);
            //var getReviewsContent = new HttpMessageContent(getReviews);

            //MultipartContent content = new MultipartContent("mixed", "batch_" + Guid.NewGuid().ToString());
            //content.Add(new HttpMessageContent(getRatings));
            //content.Add(new HttpMessageContent(getReviews));

            var client = new HttpClient();
            var tasks = new List<Task>();

            var getRatingsTask = client.GetAsync(getRatingsUrl);
            tasks.Add(getRatingsTask);

            var getReviewsTask = client.GetAsync(getReviewsUrl);
            tasks.Add(getReviewsTask);

            await Task.WhenAll(tasks);

            dynamic[] ratings = await getRatingsTask.Result.Content.AsExpandoArrayAsync();
            dynamic[] reviews = await getReviewsTask.Result.Content.AsExpandoArrayAsync();

            composedViewModel.ItemRating = ratings.Single();
            composedViewModel.ItemReviews = reviews;
        }
    }
}
