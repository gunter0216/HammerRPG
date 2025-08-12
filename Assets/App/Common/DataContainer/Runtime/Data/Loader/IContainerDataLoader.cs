using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.DataContainer.Runtime.Data.Loader
{
    public interface IContainerDataLoader
    {
        Optional<IReadOnlyList<IContainerData>> Load();
    }
}