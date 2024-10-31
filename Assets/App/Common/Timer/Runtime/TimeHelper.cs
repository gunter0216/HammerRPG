using System;

namespace App.Common.Time.Runtime
{
    // todo сделать перемотку времени
    public static class TimeHelper
    {
        public static DateTime Now => DateTime.Now;
        public static DateTime UtcNow => DateTime.UtcNow;
    }
}