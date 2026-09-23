using Microsoft.AspNetCore.Authorization;

public class PermissionHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // ==========================================
        // Get PermissionMask1 from JWT
        // ==========================================

        var mask1Claim =
            context.User.FindFirst("PermissionMask1")?.Value;

        // ==========================================
        // Get PermissionMask2 from JWT
        // ==========================================

        var mask2Claim =
            context.User.FindFirst("PermissionMask2")?.Value;

        // ==========================================
        // Convert values
        // ==========================================

        if (!long.TryParse(mask1Claim, out long userMask1))
            return Task.CompletedTask;

        if (!long.TryParse(mask2Claim, out long userMask2))
            return Task.CompletedTask;

        // ==========================================
        // Check Mask1
        // ==========================================

        if (requirement.PermissionMask1 != 0)
        {
            bool hasPermission1 =
                (userMask1 & requirement.PermissionMask1)
                == requirement.PermissionMask1;

            if (!hasPermission1)
                return Task.CompletedTask;
        }

        // ==========================================
        // Check Mask2
        // ==========================================

        if (requirement.PermissionMask2 != 0)
        {
            bool hasPermission2 =
                (userMask2 & requirement.PermissionMask2)
                == requirement.PermissionMask2;

            if (!hasPermission2)
                return Task.CompletedTask;
        }

        // ==========================================
        // User has required permissions
        // ==========================================

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}