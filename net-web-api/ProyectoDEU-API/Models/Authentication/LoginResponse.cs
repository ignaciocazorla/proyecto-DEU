﻿namespace ProyectoDEU_API.Models.Authentication
{
    public class LoginResponse
    {
        public string Username { get; set; }
        //public string Email { get; set; }
        public bool IsValid { get; set; }
        public string AuthToken { get; set; }
        public Guid? SessionId { get; set; }
        public LoginStatus Status { get; set; }
        public string StatusText { get; set; }
        //public Guid? UserId { get; set; }
    }

    public enum LoginStatus
    {
        Success = 0,
        InvalidUserName = 1,
        InvalidPassword = 2,
        ServerError = 11,
        PendingValidation = 32,
        UserLocked = 35,
        UserNotActive = 36
    }
}
