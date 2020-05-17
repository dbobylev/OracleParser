using DataBaseRepository.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace DataBaseRepository
{
    class FileFilter
    {
        private string _PathToSearch;
        private static ParameterExpression _FilterParameter = Expression.Parameter(typeof(string), "x");
        private Expression _FilterExpression = Expression.Constant(true);

        public FileFilter(SelectRequest request, string RepositoryPath)
        {
            _PathToSearch = string.IsNullOrEmpty(request.Owner) ? RepositoryPath : Path.Combine(RepositoryPath, request.Owner.ToUpper());
            
            SetFileExtensionsFilter(request.FileTypes);
            SetPatternFilter(request.Pattern);
        }

        private void SetFileExtensionsFilter(List<eRepositoryObjectType> FileTypes)
        {
            if (FileTypes != null && FileTypes.Count > 0)
            {
                var EndWithMehod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                var FileEndingExspressions = FileTypes
                    .Select(x => Expression.Call(
                        _FilterParameter, 
                        EndWithMehod, 
                        Expression.Constant(Helper.FileExtensions[x])))
                    .Aggregate<Expression>((a, b) => Expression.Or(a, b));
                _FilterExpression = Expression.And(_FilterExpression, FileEndingExspressions);
            }
        }

        private void SetPatternFilter(string Pattern)
        {
            if (!string.IsNullOrEmpty(Pattern))
            {
                var ContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var PatternExpression = Expression.Call(_FilterParameter, ContainsMethod, Expression.Constant(Pattern.ToUpper()));
                _FilterExpression = Expression.And(_FilterExpression, PatternExpression);
            }
        }

        public IEnumerable<string> Search()
        {
            Seri.Log.Verbose($"Начинаем отбирать файлы, ExpressionFilter: {_FilterExpression}");
            Func<string, bool> predicateFilter = (Func<string, bool>)Expression.Lambda(_FilterExpression, _FilterParameter).Compile();
            return Directory
                .EnumerateFiles(_PathToSearch, "*.*", SearchOption.AllDirectories)
                .Select(x=>x.ToUpper())
                .Where(predicateFilter);
        }
    }
}
