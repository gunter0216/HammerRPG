using System;

namespace App.Common.AssemblyManager.Runtime
{
    public struct AttributeNode
    {
        public Attribute Attribute { get; }
        public Type Holder { get; }

        public AttributeNode(Type holder, Attribute attribute)
        {
            Holder = holder;
            Attribute = attribute;
        }
    }
}