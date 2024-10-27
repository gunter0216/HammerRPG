using App.Game.Inputs.Runtime.Attributes;

namespace App.Game.Inputs.Runtime.Events
{
    public struct MousePressedEvent
    {
        public MouseKey MouseKey { get; }

        public MousePressedEvent(MouseKey mouseKey)
        {
            MouseKey = mouseKey;
        }
    }
}