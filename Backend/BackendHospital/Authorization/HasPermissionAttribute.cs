using Microsoft.AspNetCore.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(long permissionMask1, long permissionMask2)
    {
        Policy = $"Permission_{permissionMask1}_{permissionMask2}";
    }
}