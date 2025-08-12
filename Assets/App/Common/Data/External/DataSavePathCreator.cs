using System.IO;
using App.Common.Data.Runtime;
using UnityEngine;

namespace App.Common.Data.External
{
    public class DataSavePathCreator : IDataSavePathCreator
    {
        public string Create()
        {
            return Path.Combine(Application.persistentDataPath, "Data");
        }
    }
}