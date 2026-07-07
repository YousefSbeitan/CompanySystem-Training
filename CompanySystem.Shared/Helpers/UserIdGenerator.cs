using System;

namespace CompanySystem.Shared.Helpers;

public static class UserIdGenerator
{
    public static string Generate(int? departmentId)
    {
        string departmentCode = departmentId?.ToString("D3") ?? "000";

        string guid = Guid.NewGuid()
            .ToString("N")
            .ToUpper();

        return $"{departmentCode}_{guid}";
    }
}