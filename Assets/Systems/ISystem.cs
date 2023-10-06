using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISystem
{
    void OnFrameUpdate();
    void OnSystemUpdate();
}