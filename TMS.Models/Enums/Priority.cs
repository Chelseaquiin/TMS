using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Models.Enums
{
    public enum Priority
    {
        High = 1,
        Low
    }
    public static class PriorityTypeExtension
    {
        public static string? GetStringValue(this Priority priority)
        {
            return priority switch
            {
                Priority.High => "high",
                Priority.Low => "low",
                _ => null
            };
        }
    }
}
