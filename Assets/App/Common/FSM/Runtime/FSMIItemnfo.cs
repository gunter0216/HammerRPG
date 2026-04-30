namespace App.Common.FSM.Runtime
{
    public class FSMIItemnfo
    {
        private readonly int m_Stage;
        private readonly int m_Order;

        public int Stage => m_Stage;

        public int Order => m_Order;

        public FSMIItemnfo(int stage, int order)
        {
            m_Stage = stage;
            m_Order = order;
        }
    }
}