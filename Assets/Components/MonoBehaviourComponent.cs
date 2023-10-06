using System;
using UnityEngine;

public abstract class MonoBehaviourComponent : MonoBehaviour, IComponent
{
    private void OnEnable()
    {
        
    }
}

public interface IComponent{}