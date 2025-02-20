using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CanvasFollow:MonoBehaviour
{
    public Transform CamTransform;
    public Transform Cube;

    private void Update()
    {
        Vector3 dir = transform.position - CamTransform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        Vector3 pos = Cube.position;
        pos.y += Cube.localScale.y + 0.5f;
        transform.position = pos;
    }
    public void Close()
    {
        Cube.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
