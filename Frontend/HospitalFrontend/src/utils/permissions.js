// نفس منطق PermissionHandler.cs بالباك اند تماماً.
// استخدمنا BigInt لأن بعض الـ masks أكبر من Number.MAX_SAFE_INTEGER.
export function hasPermission(userMask1, userMask2, requiredMask1 = 0, requiredMask2 = 0) {
  const m1 = BigInt(userMask1 ?? 0);
  const m2 = BigInt(userMask2 ?? 0);
  const r1 = BigInt(requiredMask1);
  const r2 = BigInt(requiredMask2);

  if (r1 !== 0n && (m1 & r1) !== r1) return false;
  if (r2 !== 0n && (m2 & r2) !== r2) return false;
  return true;
}