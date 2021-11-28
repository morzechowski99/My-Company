namespace My_Company.Models.AccountModels
{
    public class VerifyUserData
    {
        public string UserId { get; set; }
        public string VerifyCode { get; set; }

        public VerifyUserData(string userId, string verifyCode)
        {
            UserId = userId;
            VerifyCode = verifyCode;
        }
    }
}
