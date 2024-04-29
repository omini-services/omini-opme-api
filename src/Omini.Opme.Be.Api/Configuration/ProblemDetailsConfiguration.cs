// namespace Omini.Opme.Be.Api.Configuration;

// internal static class ProblemDetailsConfiguration{
//     internal static IServiceCollection AddProblemDetails(this IServiceCollection services)
//     {
//         // services.AddProblemDetails()
//         // services.AddProblemDetails(options => {
//         //     op
//         //    options.CustomizeProblemDetails = (context) => {
//         //     context.
//         //    } 
//         // });
//         // builder.Services.AddProblemDetails(options =>
//         // options.CustomizeProblemDetails = (context) =>
//         // {

//         //     var mathErrorFeature = context.HttpContext.Features
//         //                                                .Get<MathErrorFeature>();
//         //     if (mathErrorFeature is not null)
//         //     {
//         //         (string Detail, string Type) details = mathErrorFeature.MathError switch
//         //         {
//         //             MathErrorType.DivisionByZeroError =>
//         //             ("Divison by zero is not defined.",
//         //                                   "https://wikipedia.org/wiki/Division_by_zero"),
//         //             _ => ("Negative or complex numbers are not valid input.",
//         //                                   "https://wikipedia.org/wiki/Square_root")
//         //         };

//         //         context.ProblemDetails.Type = details.Type;
//         //         context.ProblemDetails.Title = "Bad Input";
//         //         context.ProblemDetails.Detail = details.Detail;
//         //     }
//         // }
//     );

//     }

//     private static void ConfigureProblemDetails(ProblemDetailsOptions options)
//         {
//             // Only include exception details in a development environment. There's really no nee
//             // to set this as it's the default behavior. It's just included here for completeness :)
           
//         }
// }