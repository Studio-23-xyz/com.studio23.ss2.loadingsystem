using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Extension
{
    public static class ShuffleListExtension
    {
        public static List<T> Shuffle<T>(List<T> sourceList)
        {
            var tempHints = new List<T>(sourceList);
            if (tempHints.Count <= 0) return null;
            tempHints.RemoveAt(0);
            tempHints = tempHints.OrderBy(_ => Guid.NewGuid()).ToList();
            tempHints.Add(sourceList[0]);
            sourceList = new List<T>(tempHints);
            return sourceList;
        }
    }

}