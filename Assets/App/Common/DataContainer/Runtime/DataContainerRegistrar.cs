using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Common.DataContainer.Runtime.Data;
using App.Common.Logger.Runtime;

namespace App.Common.DataContainer.Runtime
{
    public static class DataContainerRegistrar
    {
        private static List<Type> m_DataTypes = new List<Type>();

        public static void Register<T>() where T : IData
        {
            if (m_DataTypes == null)
            {
                HLogger.LogError("регистрация прошла, иди нахуй");    
            }
            
            var type = typeof(T);
            if (!m_DataTypes.Contains(type))
            {
                m_DataTypes.Add(type);
            }
        }

        internal static List<IContainerData> GetDatas()
        {
            var datas = new List<IContainerData>(m_DataTypes.Count);
            foreach (var dataType in m_DataTypes)
            {
                var instance = Activator.CreateInstance(dataType) as IContainerData;
                if (instance == null)
                {
                    HLogger.LogError($"data {dataType.Name} contains attribute but no interface");
                    continue;
                }

                datas.Add(instance);
            }

            m_DataTypes = null;

            return datas;
        }
    }
}