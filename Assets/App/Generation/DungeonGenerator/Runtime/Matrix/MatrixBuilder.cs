using System.Collections.Generic;

namespace App.Generation.DungeonGenerator.Runtime.Matrix
{
    public class MatrixBuilder<T>
    {
        // private Matrix<T> m_Other;
        private Matrix<T> m_NewMatrix;
        private Dictionary<T, T> m_Dictionary;
    
        public MatrixBuilder()
        {
            // m_NewMatrix = new Matrix<T>(1, 1);
            // m_NewMatrix = new Matrix<T>(other.Width, other.Height);
            // m_Other = other;
        }

        public MatrixBuilder<T> SetSize(int width, int height)
        {
            m_NewMatrix = new Matrix<T>(width, height);
            return this;
        }
    
        public MatrixBuilder<T> Copy(Matrix<T> other)
        {
            m_NewMatrix = new Matrix<T>(other);
            return this;
        }
    
        public MatrixBuilder<T> Fill(T value)
        {
            m_NewMatrix.Fill(value);
            return this;
        }
    
        public MatrixBuilder<T> SetReplacedValues(T newValue, params T[] oldValues)
        {
            foreach (var oldValue in oldValues)
            {
                m_Dictionary.Add(oldValue, newValue);
            }
            return this;
        }
    
        public MatrixBuilder<T> Replace()
        {
            for (int i = 0; i < m_NewMatrix.Height; ++i)
            {
                for (int j = 0; j < m_NewMatrix.Width; ++j)
                {
                    if (m_Dictionary.TryGetValue(m_NewMatrix.GetCell(i, j), out var newValue))
                    {
                        m_NewMatrix.SetCell(i, j, newValue);
                    }
                }
            }
            return this;
        }

        public Matrix<T> GetMatrix()
        {
            return m_NewMatrix;
        }
    }
}