using System.ComponentModel;

namespace Employees.Shared.Constants
{
    public enum PermissionAccessTypes
    {
        [Description("نامشخص")]
        None,

        [Description("مخفی")]
        Hide,

        [Description("فقط خواندن")]
        Readonly,

        [Description("فعال")]
        Active,
    }
}