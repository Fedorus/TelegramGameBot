using System;
using System.Collections.Generic;

namespace GameLibrary.Helpers
{
    public static class KeyboardHelper
    {
        public static IEnumerable<IEnumerable<T>> SplitList<T>(this List<T> locations, int nSize=3)  
        {        
            for (int i=0; i < locations.Count; i+= nSize) 
            { 
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i)); 
            }  
        }
    }
}