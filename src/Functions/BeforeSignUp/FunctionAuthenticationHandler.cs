// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authorization;

// namespace BeforeSignUp;

//     public static class AuthenticationFunctionHandler
//     {
//         public static AuthenticationBuilder AddSyncAuth(this AuthenticationBuilder builder, Action<AuthenticationFunctionOptions> configureOptions)
//         {
//             return builder.AddScheme<AuthenticationFunctionOptions, SyncAuthHandler>(SyncAuthHandler.SyncAuthScheme, null, configureOptions);
//         }
//     }

//     public class AuthenticationFunctionOptions : AuthenticationSchemeOptions
//     {
//         public AuthenticationFunctionOptions() { }
//     }
