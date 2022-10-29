﻿using Microsoft.AspNetCore.Http;
using ProEventos.API.Models;
using System.Text.Json;

namespace ProEventos.API.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var pagination = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);


            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }

}
