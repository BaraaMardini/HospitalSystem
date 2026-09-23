using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

public class PermissionPolicyProvider
    : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(
        IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override Task<AuthorizationPolicy?>
        GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permission_"))
        {
            var values =
                policyName
                    .Replace("Permission_", "")
                    .Split('_');

            long permissionMask1 =
                long.Parse(values[0]);

            long permissionMask2 =
                long.Parse(values[1]);

            var policy =
                new AuthorizationPolicyBuilder()
                    .AddRequirements(
                        new PermissionRequirement(
                            permissionMask1,
                            permissionMask2))
                    .Build();

            return Task.FromResult<AuthorizationPolicy?>(
                policy);
        }

        return base.GetPolicyAsync(policyName);
    }
}