namespace App.Game.KruskalAlgorithm.Runtime
{
    public class UnionFind
    {
        private int[] parent;
        private int[] rank;

        public UnionFind(int size)
        {
            parent = new int[size];
            rank = new int[size];
        
            // Инициализация: каждый элемент - корень своего множества
            for (int i = 0; i < size; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }
        }

        // Поиск корня множества с сжатием пути
        public int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]); // Сжатие пути
            }
            return parent[x];
        }

        // Объединение двух множеств по рангу
        public bool Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            // Если элементы уже в одном множестве
            if (rootX == rootY)
                return false;

            // Объединение по рангу
            if (rank[rootX] < rank[rootY])
            {
                parent[rootX] = rootY;
            }
            else if (rank[rootX] > rank[rootY])
            {
                parent[rootY] = rootX;
            }
            else
            {
                parent[rootY] = rootX;
                rank[rootX]++;
            }

            return true;
        }

        // Проверка, находятся ли элементы в одном множестве
        public bool Connected(int x, int y)
        {
            return Find(x) == Find(y);
        }
    }
}