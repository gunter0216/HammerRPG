﻿using System.Collections.Generic;
using App.Common.Utility.Runtime;

namespace App.Common.DataContainer.Runtime.Data.Loader
{
    public interface IContainerDataLoader
    {
        Optional<IReadOnlyList<IContainerData>> Load();
    }
}