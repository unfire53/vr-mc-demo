using Unity.XR.CoreUtils;
using UnityEngine;

public class socketsTransform : MonoBehaviour
{
    public Transform cubeTrans;
    public Transform cameraTrans;
    public XROrigin xrOrigin;
    [Range(-1,1)]
    public float offsetHeight;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cubeTrans.position.x, (xrOrigin.CameraInOriginSpaceHeight + offsetHeight) * xrOrigin.transform.localScale.y, cubeTrans.position.z);
        transform.LookAt(cameraTrans.position);
    }
}
