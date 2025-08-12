using System;
using App.Common.DataContainer.Runtime;

namespace Assets.App.Common.ModuleItem.Runtime
{
    public readonly struct ModuleItemResult<T> where T : class, IModuleItem
    {
        private readonly DataReference m_DataReference;
        private readonly T m_ModuleItem;
        private readonly string m_ErrorMessage;
        private readonly bool m_IsSuccess;

        public DataReference DataReference => m_DataReference;
        public T ModuleItem => m_ModuleItem;
        public bool IsSuccess => m_IsSuccess;

        public string ErrorMessage => m_ErrorMessage;

        public ModuleItemResult(T moduleItem, bool isSuccess, DataReference dataReference, string errorMessage)
        {
            m_ModuleItem = moduleItem;
            m_IsSuccess = isSuccess;
            m_DataReference = dataReference;
            m_ErrorMessage = errorMessage;
        }

        public static ModuleItemResult<T> Success(T moduleItem, DataReference dataReference)
        {
            return new ModuleItemResult<T>(moduleItem, true, dataReference, String.Empty);
        }

        public static ModuleItemResult<T> Fail(string errorMessage = "")
        {
            return new ModuleItemResult<T>(null, false, null, errorMessage);
        }   
    }
}