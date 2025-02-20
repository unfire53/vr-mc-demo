using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResource
{
    event Action OnWorkFinish;
    bool GetPos(out Vector3Int vector3);
    bool GetInterface(out IResource resource);
    IEnumerator Interact();
}
