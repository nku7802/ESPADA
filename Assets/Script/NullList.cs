using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NullList<T> : List<T>
{
    public bool AddIfNull(T item) {
        for (int i = 0; i < this.Count; i++)
        {
            if(this[i] == null) {
                this[i] = item;
                return true;
            }
        }
        
        return false;
    }
}
