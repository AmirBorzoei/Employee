using System;

namespace Employees.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConsiderIsDirtyAttribute : Attribute
    {
    }
}