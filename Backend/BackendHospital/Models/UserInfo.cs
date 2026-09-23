namespace BackendHospital.Models
{
    public class UserInfo
    {
     public   int UserID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public long PermissionMask1 { get; set; }
        public long PermissionMask2 { get; set; }

        public string Role { get; set; }



    }
}
