using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Common.Data.Runtime
{
    public class DataRegistrar
    {
        private List<Type> m_DataTypes = new List<Type>();

        public void Register<T>() where T : IData
        {
            if (m_DataTypes == null)
            {
                Debug.LogError("регистрация прошла, иди нахуй");    
            }
            
            var type = typeof(T);
            if (!m_DataTypes.Contains(type))
            {
                m_DataTypes.Add(type);
            }
        }

        internal List<IData> GetDatas()
        {
            var datas = new List<IData>(m_DataTypes.Count);
            foreach (var dataType in m_DataTypes)
            {
                var instance = Activator.CreateInstance(dataType) as IData;
                if (instance == null)
                {
                    Debug.LogError($"data {dataType.Name} contains attribute but no interface");
                    continue;
                }

                datas.Add(instance);
            }

            m_DataTypes = null;

            return datas;
        }
    }
}