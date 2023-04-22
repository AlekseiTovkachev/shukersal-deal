using Azure;

namespace HotelBackend.Util
{
    public static class UserRoles
    {
        public const string Member = "Member";
        public const string Administrator = "Administrator";

        public const string AllGroup = "Guest, Member, Administrator";
        public const string MemberGroup = "Member, Administrator";
        public const string AdministratorGroup = "Administrator";
    }
}
