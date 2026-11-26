using App.Common.ModuleItem.Runtime;
using UnityEngine;

namespace App.Common.ModuleItem.External
{
    [DisallowMultipleComponent]
    public class ModuleItemView : MonoBehaviour
    {
        public IModuleItem ModuleItem { get; set; }
    }
}