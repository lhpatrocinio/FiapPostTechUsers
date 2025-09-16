using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text;

namespace Users.Infrastructure.Common
{
    public static class ErrorExtensions
    {
        public static string ConvertToString(this List<ValidationFailure> listErrors)
        {
            StringBuilder result = new StringBuilder();
            foreach (var error in listErrors)
            {
                result.Append(string.Concat(error.ErrorMessage, " - "));
            }
            return result.ToString();
        }

        public static string ConvertToString(this IEnumerable<IdentityError> listErrors)
        {
            StringBuilder result = new StringBuilder();
            foreach (var error in listErrors)
            {
                result.Append(string.Concat(error.Description, " - "));
            }
            return result.ToString();
        }
    }
}

