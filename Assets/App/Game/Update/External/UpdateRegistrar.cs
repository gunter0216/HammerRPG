using System;
using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Update.External
{
    public static class UpdateRegistrar
    {
        private static Dictionary<Type, int> m_Info = new();
        
        public static void Register<T>(UpdateStage stage) where T : class
        {
            var type = typeof(T);
            if (m_Info.TryGetValue(type, out var order))
            {
                HLogger.LogError("Already Exists");
                return;
            }
            
            m_Info.Add(type, (int)stage);
        }

        public static Dictionary<Type, int> GetInfo()
        {
            return m_Info;
        }
        
        public static Optional<int> GetOrder<T>()
        {
            return GetOrder(typeof(T));
        }

        public static Optional<int> GetOrder(Type type)
        {
            if (!m_Info.TryGetValue(type, out var order))
            {
                HLogger.LogError("order not found");
                return Optional<int>.Fail();
            }
            
            return Optional<int>.Success(order);
        }
    }
}