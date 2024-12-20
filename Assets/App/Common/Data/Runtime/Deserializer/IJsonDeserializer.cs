﻿using System;
using App.Common.Utility.Runtime;

namespace App.Common.Data.Runtime.Deserializer
{
    public interface IJsonDeserializer
    {
        Optional<T> Deserialize<T>(string json, Type type);
        Optional<T> Deserialize<T>(string json);
    }
}