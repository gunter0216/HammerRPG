using System;

namespace App.Common.AssemblyManager.Runtime
{
    public class AttributeNode
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