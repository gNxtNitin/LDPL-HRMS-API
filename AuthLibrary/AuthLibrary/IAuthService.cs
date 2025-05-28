using AuthLibrary.Models;
using ModelsLibrary.Models;


namespace AuthLibrary.Interface
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user based on their mobile number or email and password, or via OTP.
        /// </summary>
        /// <param name="loginReq">The login request model containing the user's mobile/email and password or OTP details.</param>
        /// <returns>
        /// A <see cref="ResponseModel"/> containing the result of the authentication process,
        /// including id, message, and JWT token if successful.
        /// </returns>
        public Task<ResponseModel> AuthenticateUser(AuthRequestModel req);

        /// <summary>
        /// Sets the One-Time Password (OTP) for user authentication.
        /// </summary>
        /// <param name="req">The login request model containing the user's mobile/email and resend code flag.</param>
        /// <returns>
        /// A <see cref="ResponseModel"/> containing the result of the OTP setting process,
        /// including id, message and the generated verification code.
        /// </returns>
        public Task<ResponseModel> SetOTP(AuthRequestModel req);


        /// <summary>
        /// Retrieves the roles associated with a user by their ID.
        /// </summary>
        /// <param name="req">The request model containing the user ID and role ID.</param>
        /// <returns>
        /// A <see cref="ResponseModel"/> object containing the result of the operation,
        /// including id, message, and role data in JSON format if successful.
        /// </returns>
        public Task<ResponseModel> GetUserRoleById(string userId);


        /// <summary>
        /// Validates the One-Time Password (OTP) provided by the user for authentication.
        /// </summary>
        /// <param name="req">The login request model containing the user's mobile/email and OTP.</param>
        /// <returns>
        /// A <see cref="ResponseModel"/> containing the result of the OTP validation process,
        /// including id, message and a JWT token if validation is successful.
        /// </returns>
        public Task<ResponseModel> ValidateOTP(AuthRequestModel req);

        public Task<ResponseModel> UserDetail(string userId);



    }
}
