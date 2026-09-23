using Microsoft.AspNetCore.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public long PermissionMask1 { get; }
    public long PermissionMask2 { get; }

    public PermissionRequirement(
        long permissionMask1,
        long permissionMask2)
    {
        PermissionMask1 = permissionMask1;
        PermissionMask2 = permissionMask2;
    }
}