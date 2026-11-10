

namespace DonationHub.Domain.Results.Errors
{
    public static class AuthErrors
    {
        public static readonly Error InValidCredentials =
    new("Auth.InvalidCredentials", "Wrong Email or Password.", ErrorType.UnAuthorized);

        public static readonly Error EmailAlreadyExists =
            new Error("Auth.EmailAlreadyExists", "Registering with an existing email", ErrorType.Conflict);


        public static readonly Error RefreshTokenInvalid =
   new("Auth.RefreshTokenInvalid", "Refresh Token unkown , expired or revoked", ErrorType.UnAuthorized);


        public static readonly Error AccountDeactivated =
           new Error("Auth.AccountDeactivated", "Deactivated user logs in or refreshes", ErrorType.Forbidden);


    }
}
