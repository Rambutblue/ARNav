using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class SerializablePair<T1, T2>
    {
        [SerializeField] public T1 first;
        [SerializeField] public T2 second;
    }
}
