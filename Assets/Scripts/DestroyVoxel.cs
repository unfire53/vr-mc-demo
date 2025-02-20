using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DestroyVoxel : MonoBehaviour
{
    public GameObject xrOrigin;
    public World world;
    public Transform parent;
    XRGrabInteractable xRGrab;
    private bool is_grab;

    void Start()
    {
        parent = transform.parent;
        xRGrab = GetComponent<XRGrabInteractable>();
        xRGrab.selectEntered.AddListener(SlectEnter);
        xRGrab.selectExited.AddListener(SelectExit);
    }
    private void OnEnable()
    {
        StartCoroutine(Check());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator Check()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (is_grab && transform.position.y < WorldHelper.chunkHeight)
            {
                HitCheck();
            }
        }
    }
    void HitCheck()
    {
        Vector3Int chunkPos = Vector3Int.zero;
        Vector3Int localPos = Vector3Int.zero;
        ChunkHelper.GetChunkPosAndLocalPos(Vector3Int.RoundToInt(transform.position), ref chunkPos, ref localPos);
        int x = localPos.x;
        int y = localPos.y;
        int z = localPos.z;
        Chunk c;
        if (World.chunks.TryGetValue(chunkPos, out c))
        {
            if (y >= WorldHelper.chunkHeight || y < 0) return;
            if (c.chunkData.voxelMap[(int)x, (int)y, (int)z].blockName != BlockName.Air && c.chunkData.voxelMap[(int)x, (int)y, (int)z].blockName != BlockName.Water)
            {
                c.chunkData.voxelMap[(int)x, (int)y, (int)z].blockName = BlockName.Air;
                if (c.chunkData.voxelMap[(int)x, (int)y, (int)z].blockState != null)
                    c.chunkData.voxelMap[(int)x, (int)y, (int)z].blockState = null;
                Vector3[] directions = new Vector3[4]
                {
                    Vector3.left,Vector3.right,Vector3.forward,Vector3.back
                };
                BlockName tempNum = BlockName.Air;
                for(int i = 0;i<directions.Length;i++)
                {
                    if(GetVoxelInNeiber(c.chunkData,(int) (x + directions[i].x), (int)(y + directions[i].y), (int)(z + directions[i].z),ref tempNum)
                        && tempNum == BlockName.Water)
                    {
                        c.chunkData.voxelMap[(int)x, (int)y, (int)z].blockName = BlockName.Water;
                        break;
                    }
                }
                Debug.Log(c.chunkData.voxelMap[(int)x, (int)y, (int)z].blockName + "voxelmap");
                c.CreatChunkMesh();
                c.UpdateMesh();
            }
        }
    }
    public bool GetVoxelInNeiber(ChunkData c, int x, int y, int z,ref BlockName blockNum)
    {
        Vector3Int chunkoffset = Vector3Int.zero;
        if(!ChunkHelper.GetChunkOffset(ref x, ref y, ref z,ref chunkoffset))
        {
            blockNum = c.voxelMap[x, y, z].blockName;
            return true;
        }
        ChunkData c1;
        if(World.ChunkDatas.TryGetValue(c.chunkPos + chunkoffset, out c1))
        {
            blockNum = c1.voxelMap[x, y, z].blockName;
            return true;
        }
        return false;
    }
    private void SlectEnter(SelectEnterEventArgs arg0)
    {
        is_grab = true;
        gameObject.transform.DetachChildren();
        xRGrab.SetTargetLocalScale(Vector3.one);
    }
    private void SelectExit(SelectExitEventArgs e)
    {
        is_grab = false;
        gameObject.transform.parent = parent;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
    }
}
