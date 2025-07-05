using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PoolSettings
{
    [SerializeField] private int _initialSize = 10;
    [SerializeField] private int _maxSize = 50;
    //[SerializeField] private PoolExpandMethods _expandMethod = PoolExpandMethods.OneAtATime;

    public int InitialSize => _initialSize;
    public int MaxSize => _maxSize;
    //public PoolExpandMethods ExpandMethod => _expandMethod;
}