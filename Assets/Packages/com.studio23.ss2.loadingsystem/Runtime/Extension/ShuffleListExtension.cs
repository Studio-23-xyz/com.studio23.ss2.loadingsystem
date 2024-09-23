
using System.Collections.Generic;
using Random = System.Random;


namespace Studio23.SS2.SceneLoadingSystem.Extension
{
    public static class ShuffleListExtension
    {
        public static List<T> Shuffle<T>(List<T> list)
        {
            Random rng = new Random();

            // Fisher-Yates shuffle algorithm
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
            return list;

        }
    }

}