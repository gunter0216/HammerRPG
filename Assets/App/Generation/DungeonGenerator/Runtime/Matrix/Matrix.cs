using System;
using System.Collections;
using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Extensions;

namespace App.Generation.DungeonGenerator.Runtime.Matrix
{
    public class Matrix : Matrix<int>
    {
        public Matrix(Matrix other) : base(other) {}
        public Matrix(int width, int height) : base(width, height) { }
    }
    
    public class Matrix<T> : IEnumerable<T>
    {
        public static readonly ValueTuple<int, int> InvalidPosition = (-1, -1);
        
        private readonly int m_Width;
        private readonly int m_Height;
        private readonly T[] m_Matrix;

        public int Width => m_Width;
        public int Height => m_Height;
        public int Cols => m_Width;
        public int Rows => m_Height;

        public Matrix(Matrix<T> other)
        {
            m_Width = other.m_Width;
            m_Height = other.m_Height;
            m_Matrix = new T[other.m_Matrix.Length];
            Array.Copy(other.m_Matrix, m_Matrix, other.m_Matrix.Length);
        }

        public Matrix(int width, int height)
        {
            m_Width = width;
            m_Height = height;
            m_Matrix = new T[m_Width * m_Height];
        }
        
        public T[] GetMatrix()
        {
            return m_Matrix;
        }

        public void Fill(T value)
        {
            m_Matrix.Fill(value);
        }
    
        // public bool IsCorrectPos(Position pos)
        // {
        //     return IsCorrectPos(pos.Row, pos.Col);
        // }
    
        public bool IsCorrectPos(int row, int col)
        {
            return col >= 0 && col < m_Width && row >= 0 && row < m_Height;
        }
    
        // public T GetCell(Position pos)
        // {
        //     return GetCell(pos.Row, pos.Col);
        // }

        public T this[int i, int j]
        {
            get => GetCell(i, j);
            set => SetCell(i, j, value);
        }

        public T GetCell(int row, int col)
        {
            return m_Matrix[row * m_Width + col];
        }

        // public void SetCell(Position pos, T value)
        // {
        //     SetCell(pos.Row, pos.Col, value);
        // }
    
        public void SetCell(int row, int col, T value)
        {
            m_Matrix[row * m_Width + col] = value;
        }
        
        public (int Row, int Col) GetFirstDefaultCell()
        {
            for (int row = 0; row < m_Height; ++row)
            {
                for (int col = 0; col < m_Width; ++col)
                {
                    if (EqualityComparer<T>.Default.Equals(GetCell(row, col), default(T)))
                    {
                        return (row, col);
                    }
                }
            }

            return InvalidPosition;
        }

        public Matrix<T> Scale(int scale)
        {
            Matrix<T> matrix = new Matrix<T>(m_Width * scale, m_Height * scale);
            for (int i = 0; i < m_Height; ++i)
            {
                for (int j = 0; j < m_Width; ++j)
                {
                    for (int k = 0; k < scale; ++k)
                    {
                        for (int l = 0; l < scale; ++l)
                        {
                            matrix.SetCell(i * scale + k, j * scale + l, GetCell(i, j));
                        }
                    }
                }
            }

            return matrix;
        }
    
        public bool CopyValuesFrom(Matrix<T> other)
        {
            if (m_Width != other.m_Width ||
                m_Height != other.m_Height)
            {
                return false;
            }
        
            for (int i = 0; i < other.m_Matrix.Length; ++i)
            {
                m_Matrix[i] = other.m_Matrix[i];
            }

            return true;
        }
    
        public bool ContainsOnly(params T[] values)
        {
            HashSet<T> valuesDictionary = new HashSet<T>(values);
            for (int i = 0; i < m_Height; ++i)
            {
                for (int j = 0; j < m_Width; ++j)
                {
                    var value = GetCell(i, j);
                    if (!valuesDictionary.Contains(value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    
        public void Print()
        {
            for (int i = 0; i < m_Height; ++i)
            {
                for (int j = 0; j < m_Width; ++j)
                {
                    Console.Write(GetCell(i, j));
                }
                Console.Write('\n');
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MatrixEnumerator(m_Matrix);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class MatrixEnumerator : IEnumerator<T>
        {
            private readonly T[] m_Matrix;
            private int m_Position = -1;
        
            public MatrixEnumerator(T[] matrix)
            {
                m_Matrix = matrix;
            }
        
            public bool MoveNext()
            {
                m_Position += 1;
                return m_Position < m_Matrix.Length;
            }

            public void Reset()
            {
                m_Position = -1;
            }

            object IEnumerator.Current => Current;

            public T Current
            {
                get
                {
                    try
                    {
                        return m_Matrix[m_Position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            public void Dispose()
            {
                // do nothing
            }
        }
    }
}