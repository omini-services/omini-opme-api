using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Omini.Opme.Be.Api.Extensions;

internal static class ValidationExtensions
{
    public static ValidationProblemDetails ToProblemDetails(this ValidationException ex){
        var error = new ValidationProblemDetails{
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Status = 400
        };

        foreach(var validationFailure in ex.Errors){
            if (error.Errors.ContainsKey(validationFailure.PropertyName)){
                error.Errors[validationFailure.PropertyName] = error.Errors[validationFailure.PropertyName].Concat([validationFailure.ErrorMessage]).ToArray();
                continue;
            }

              error.Errors.Add(new KeyValuePair<string, string[]>(
                    validationFailure.PropertyName,
                    [validationFailure.ErrorMessage])
                );
        }

        return error;
    }      
}