using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Data.EntityFrameworkCore.Utilities
{
    public static class QueryHelper<TModel>
    {
        public static IQueryable<TModel> Filter(IQueryable<TModel> query, Dictionary<string, object> filterDictionary)
        {
            if (filterDictionary != null && !filterDictionary.Count.Equals(0))
            {
                foreach (var f in filterDictionary)
                {
                    string key = f.Key;
                    object Value = f.Value;
                    string filterQuery = string.Concat(string.Empty, key, " == @0");

                    query = query.Where(filterQuery, Value);
                }
            }
            return query;
        }

        public static IQueryable<TModel> Order(IQueryable<TModel> query, Dictionary<string, string> orderDictionary)
        {

            if (!orderDictionary.Count.Equals(0))
            {
                string Key = orderDictionary.Keys.First();
                string OrderType = orderDictionary[Key];

                query = query.OrderBy(string.Concat(Key, " ", OrderType));
            }

            return query;
        }

        public static IQueryable<TModel> Search(IQueryable<TModel> query, List<string> searchAttributes, string keyword, bool ToLowerCase = false)
        {
            /* Search with Keyword */
            if (keyword != null)
            {
                string SearchQuery = String.Empty;
                foreach (string Attribute in searchAttributes)
                {
                    if (Attribute.Contains("."))
                    {
                        var Key = Attribute.Split(".");
                        SearchQuery = string.Concat(SearchQuery, Key[0], $".Any({Key[1]}.Contains(@0)) OR ");
                    }
                    else
                    {
                        SearchQuery = string.Concat(SearchQuery, Attribute, ".Contains(@0) OR ");
                    }
                }

                SearchQuery = SearchQuery.Remove(SearchQuery.Length - 4);

                if (ToLowerCase)
                {
                    SearchQuery = SearchQuery.Replace(".Contains(@0)", ".ToLower().Contains(@0)");
                    keyword = keyword.ToLower();
                }

                query = query.Where(SearchQuery, keyword);
            }
            return query;
        }

        public static IQueryable Select(IQueryable<TModel> query, string selectString)
        {
            if (selectString != null)
            {
                var selectedQuery = query.Select(selectString);
                return selectedQuery;
            }

            return query;
        }
    }
}
