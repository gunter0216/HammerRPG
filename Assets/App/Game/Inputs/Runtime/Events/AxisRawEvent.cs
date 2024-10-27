namespace App.Game.Inputs.Runtime.Events
{
    public struct AxisRawEvent
    {
        public float Horizontal { get; }
        public float Vertical { get; }

        public AxisRawEvent(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}