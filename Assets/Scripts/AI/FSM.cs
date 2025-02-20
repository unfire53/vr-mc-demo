using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;
public enum Profession
{
    None, Logger, famer, miner, Collectors
}
public class FSM : MonoBehaviour
{
    public Profession profession;
    public IResource company;
    public IResource tempWork;
    public Vector3Int home;
    public Vector3Int workPlace;
    public bool has_Target = false;
    public bool has_navigate = false;
    public Vector3Int TargetPos;
    public float workStartTime = 2;
    public float workEndTime = 18;

    public TimeManager timeManager;
    public Rigidbody rb;
    public XRGrabInteractable interactable;
    public Foot_Sensor Foot;
    private Navi_Mannager mannager;

    private float rotateSpeed = 10f;
    private float accuracy = 0.06f;
    private Graph graph;
    private int wayPointCur = 0;
    private Vector3Int wayPointPos;
    private List<Node> path = new List<Node>();

    private Vector3 direction;
    private Vector3 lookRotate;
    public bool isGrab = false;
    public Vector3 lastTransform;
    //public Resource resource;

    //public ResourceMap resourceMap;
    //public ChunkData workchunk;
    //public Feature point;

    public BaseState NowState;

    public BaseState WalkState;

    public BaseState WorkState;

    public BaseState IdleState;

    public BaseState RestState;

    void Start()
    {
        RegisterHome();
        rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        timeManager = TimeManager.instance;
        graph = new Graph();
        Foot = gameObject.GetComponentInChildren<Foot_Sensor>();
        mannager = GameObject.Find("Navi_Mannager").GetComponent<Navi_Mannager>();
        interactable = gameObject.AddComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
 //       resourceMap = GameObject.Find("ResourceMap").GetComponent<ResourceMap>();

        WalkState = new WalkState(this);
        WorkState = new WorkState(this);
        IdleState = new IdleState(this);
        RestState = new RestState(this);
        NowState = IdleState;
        ResourcePool.Instance.OnResourceLoaded += GetWorkPos;
    }
    private void GetWorkPos()
    {
        if (profession != Profession.None) return;
        if (tempWork != null) return;
        tempWork = ResourcePool.Instance.GetResource();
        if (tempWork == null) return;
        tempWork.GetPos(out workPlace);
        tempWork.OnWorkFinish += QuitWork;
    }
    private bool GetWorkPos1()
    {
        if (company == null) return false;
        company.GetPos(out workPlace);
        if(workPlace == default) return false;
        company.GetInterface(out tempWork);
        tempWork.OnWorkFinish += QuitWork;
        return true;
    }
    private void QuitWork()
    {
        tempWork.OnWorkFinish -= QuitWork;
        tempWork = null;
        workPlace = default;
        NowState.OnExit();
        NowState = IdleState;
        NowState.OnEnter();
    }

    void Update()
    {
        NowState.LogicalUpdate();
    }


    public void FixedUpdate()
    {
        NowState.PhyicsUpdate();
    }
    public void Enqueue()
    {
        mannager.navi_Queue.Enqueue(this);
        //Debug.Log("enqueue");
    }
    public bool navigate()
    {
        if (!has_Target) return false;
        Vector3Int startPos = Vector3Int.RoundToInt(transform.position);
        if (graph.Astar_bidirectional(startPos, TargetPos, path))
        {
            wayPointCur = 0;
            wayPointPos = path[0].pos;
            return true;
        }
        else
        {
            Debug.LogWarning("No path");
            return false;
        }
    }
    #region IdleState
    public void IdleEnter()
    {
    }
    public void IdleCheck()
    {
        if (isGrab || !Foot.OnGround()) return;
        if (timeManager.GetGameTime() > workEndTime || timeManager.GetGameTime() < workStartTime)
        {
            NowState.OnExit();
            TargetPos = home;
            has_Target = true;
            NowState = WalkState;
            NowState.OnEnter();
        }
        else if (timeManager.GetGameTime() > workStartTime && timeManager.GetGameTime() < workEndTime)
        {
            if (tempWork == null && !GetWorkPos1()) return;
            NowState.OnExit();
            TargetPos = workPlace;
            has_Target = true;
            NowState = WalkState;
            NowState.OnEnter();
        }
    }
    public void Idle()
    {
        ////nothing
    }
    public void IdleExit()
    {

    }
    #endregion

    #region WorkState
    public void WorkEnter()
    {
        simple();
    }
    public void simple()
    {
        StartCoroutine(tempWork.Interact());
    }
    //IEnumerator WorkIE()
    //{

    //    yield return new WaitForSeconds(1f);
    //    Structure structure = null;
    //    Vector3Int localPos = Vector3Int.zero;
    //    ChunkHelper.GetLocalPos(workPlace, ref localPos);
    //    StructLibrary.dictionary.TryGetValue(Feature.GetSturctName(resource.feature), out structure);Vector3Int offset = structure.GetOffset();
    //    switch(structure.structureType)
    //    {
    //        case StructureType.tree: case StructureType.mineral:
    //            {
    //                foreach (Structure_Node node in structure.funcNodes)
    //                {
    //                    int x = localPos.x + node.x - offset.x;
    //                    int y = localPos.y + node.y - offset.y;
    //                    int z = localPos.z + node.z - offset.z;
    //                    Vector3Int chunkOffset = Vector3Int.zero;
    //                    ChunkData chunk = Feature.Get_Chunkdata(resource.feature);
    //                    if (ChunkHelper.GetChunkOffset(ref x, ref y, ref z, ref chunkOffset))
    //                    {
    //                        if (!World.ChunkDatas.TryGetValue(workchunk.chunkPos + chunkOffset, out chunk))
    //                        {
    //                            continue;
    //                        }
    //                    }
    //                    if (chunk.voxelMap[x, y, z].blockName != BlockName.Air && chunk.voxelMap[x, y, z].blockName == node.voxelMap.blockName)
    //                    {
    //                        chunk.voxelMap[x, y, z].blockName = BlockName.Air;
    //                        chunk.voxelMap[x, y, z].blockState = null;
    //                        chunk.IsModify();
    //                        yield return new WaitForSeconds(0.5f);
    //                    }
    //                }
    //                workchunk.structPos.Remove(resource.feature);
    //                ResourcePool.Instance.RemoveResource(resource);
    //                resource = null;
    //                break;
    //            }
    //        case StructureType.farmland:
    //            {
    //                foreach (Structure_Node node in structure.funcNodes)
    //                {
    //                    int x = localPos.x + node.x - offset.x;
    //                    int y = localPos.y + node.y - offset.y;
    //                    int z = localPos.z + node.z - offset.z;
    //                    Vector3Int chunkOffset = Vector3Int.zero;
    //                    ChunkData chunk = Feature.Get_Chunkdata(resource.feature);
    //                    if (ChunkHelper.GetChunkOffset(ref x, ref y, ref z, ref chunkOffset))
    //                    {
    //                        if (!World.ChunkDatas.TryGetValue(chunk.chunkPos + chunkOffset, out chunk))
    //                        {
    //                            continue;
    //                        }
    //                    }
    //                    //if (chunk.voxelMap[x, y, z].blockName != BlockName.Air)
    //                    //{
    //                    //    if(chunk.voxelMap[x, y, z].blockName == BlockName.Wheat_1)
    //                    //    {
    //                    //        if (chunk.voxelMap[x, y, z].GrowUp())
    //                    //        {
    //                    //            chunk.voxelMap[x, y, z].blockName = BlockName.Wheat_2;
    //                    //        }
    //                    //        chunk.IsModify();
    //                    //        yield return new WaitForSeconds(0.5f);
    //                    //    }
    //                    //    else if(chunk.voxelMap[x, y, z].blockName == BlockName.Wheat_2)
    //                    //    {
    //                    //        if (chunk.voxelMap[x, y, z].GrowUp())
    //                    //        {
    //                    //            chunk.voxelMap[x, y, z].blockName = BlockName.Wheat_3;
    //                    //            chunk.voxelMap[x, y, z].blockState = null;
    //                    //        }
    //                    //        chunk.IsModify();
    //                    //        yield return new WaitForSeconds(0.5f);
    //                    //    }
    //                    //}
    //                }
    //                break;
    //            }
    //    }
    //    NowState = IdleState;
    //    NowState.OnEnter();
    //    WorkExit();
    //}
    public void WorkCheck()
    {
        if (isGrab)
        {
            NowState.OnExit();
            NowState = IdleState;
            NowState.OnEnter();
        }
        //if (!workchunk.structPos.Contains(resource.feature))
        //{
        //    ResourcePool.Instance.RemoveResource(tempWork);
        //    resource = null;
        //    NowState.OnExit();
        //    NowState = IdleState;
        //    NowState.OnEnter();
        //}
        else if (timeManager.GetGameTime() > workEndTime || timeManager.GetGameTime() < workStartTime)
        {
            NowState.OnExit();
            TargetPos = home;
            has_Target = true;
            NowState = WalkState;
            NowState.OnEnter();
        }
        if (tempWork == null)
        {
            NowState.OnExit();
            NowState = IdleState;
            NowState.OnEnter();
        }
    }
    public void Work()
    {
        ////dig or fill
    }
    public void WorkExit()
    {
        StopAllCoroutines();
        
    }
    #endregion
    #region RestState
    public void RestEnter()
    {
    }
    public void RestCheck()
    {
        if(isGrab)
        {
            NowState.OnExit();
            NowState = IdleState;
            NowState.OnEnter();
        }
        else if (timeManager.GetGameTime() > workStartTime && timeManager.GetGameTime() < workEndTime)
        {
            if(tempWork == null && !GetWorkPos1()) return;
            NowState.OnExit();
            TargetPos = workPlace;
            has_Target = true;
            NowState = WalkState;
            NowState.OnEnter();
        }
    }
    public void Rest()
    {
        ////dig or fill
    }
    public void RestExit()
    {

    }
    #endregion
    #region WalkState
    public void WalkEnter()
    {
        StartCoroutine(CheckAiPos());
    }
    IEnumerator CheckAiPos()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            Vector3 newTransfrom = transform.position;
            if (newTransfrom == lastTransform)
            {
                if(!navigate())
                {
                    yield return new WaitForSeconds(5f);
                    NowState = IdleState;
                    NowState.OnEnter();
                    yield break;
                }
            }
            lastTransform = newTransfrom;

        }
    }
    public void WalkCheck()
    {
        ///检查是否换状态了
        if (isGrab)
        {
            NowState.OnExit();
            NowState = IdleState;
            NowState.OnEnter();
        }
        if (Vector3.Distance(TargetPos,transform.position) <=2.1)
        {
            rb.velocity = Vector3.zero;
            NowState.OnExit();
            if (TargetPos == workPlace)
                NowState = WorkState;
            else if (TargetPos == home)
                NowState = RestState;
            NowState.OnEnter();
        }
    }
    public void Walk()
    {
        if (Vector3.Distance(wayPointPos, transform.position) <= accuracy)
        {
            wayPointCur++;
            if (wayPointCur >= path.Count) return;
            wayPointPos = path[wayPointCur].pos;
        }
        if (wayPointCur < path.Count)
        {
            direction = wayPointPos - transform.position;
            rb.velocity = direction.normalized * 4;

            lookRotate = new Vector3(direction.x, 0, direction.z);
            if (lookRotate == Vector3.zero)
                return;
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(lookRotate);

            // 平滑地旋转到目标旋转
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            rb.MoveRotation(newRotation);
        }
    }
    public void WalkExit()
    {
        wayPointCur = 0;
        path.Clear();
        StopAllCoroutines();
        has_Target = false;
    }
    #endregion
    public void RegisterHome()
    {
        home = Vector3Int.RoundToInt(transform.position + Vector3.down);
    }
    public void RegisterWorkPlace()
    {
        Chunk chunk;
        Vector3 pos = transform.position;
        Vector3Int chunkPos = ChunkHelper.GetChunkPos(pos);
        if (World.chunks.TryGetValue(chunkPos, out chunk))
        {
            if (chunk.chunkData.structPos.Count == 0) return;
            float distance = 6;
            int num = 0;
            for (int i = 0; i < chunk.chunkData.structPos.Count; i++)
            {
                IResource temp = chunk.chunkData.structPos[i];
                if(temp is Feature)continue;
                //WorkPlace m = temp as WorkPlace;
                if (temp.GetPos(out Vector3Int vector3))
                {
                    //float dis = Vector3.Distance(vector3 , transform.position);
                    //if (dis < distance)
                    //{
                        //switch(m.GetStructureType())
                        //{
                        //    case StructureType.farmland: profession = Profession.famer;break;
                        //    case StructureType.tree: profession = Profession.Logger;break;
                        //}
                        //distance = dis;
                        company = temp;
                        temp.GetInterface(out tempWork);
                        tempWork.OnWorkFinish += QuitWork;
                        workPlace = vector3;
                        num = i;
                        return;
                    //}
                }
            }
            //if (WorkPos != null)
            //{
            //    workchunk = chunk;
            //    point = chunk.chunkData.structPos[num];
            //    switch (Structure_Point.GetBlockName(point))
            //    {
            //        case BlockName.Wood: profession = Profession.Logger; break;
            //        case BlockName.Coal: case BlockName.Iron: profession = Profession.miner; break;
            //        case BlockName.Mushroom: case BlockName.Vegetable: profession = Profession.Collectors; break;
            //        default: profession = Profession.Logger; break;
            //    }
            //    workPlace = Vector3Int.RoundToInt(WorkPos.Value);
            //}
        }

    }
    //public void RegisterWorkPlace()
    //{
    //    Chunk chunk;
    //    Vector3 pos = transform.position;
    //    Vector3Int chunkPos = ChunkHelper.GetChunkPos(pos);
    //    if (World.chunks.TryGetValue(chunkPos, out chunk))
    //    {
    //        if (chunk.chunkData.structPos.Count == 0) return;
    //        Vector3? WorkPos = null;
    //        float distance = 6;
    //        int num = 0;
    //        for (int i = 0; i < chunk.chunkData.structPos.Count; i++)
    //        {
    //            Vector3Int temp = Structure_Point.Get_WorldPoint(chunk.chunkData.structPos[i], chunk.chunkData);
    //            float dis = Vector3.Distance(temp, transform.position);
    //            if (dis < distance)
    //            {
    //                distance = dis;
    //                WorkPos = temp;
    //                num = i;
    //            }
    //        }
    //        if (WorkPos != null)
    //        {
    //            workchunk = chunk;
    //            point = chunk.chunkData.structPos[num];
    //            switch (Structure_Point.GetBlockName(point))
    //            {
    //                case BlockName.Wood: profession = Profession.Logger; break;
    //                case BlockName.Coal: case BlockName.Iron: profession = Profession.miner; break;
    //                case BlockName.Mushroom: case BlockName.Vegetable: profession = Profession.Collectors; break;
    //                default: profession = Profession.Logger; break;
    //            }
    //            workPlace = Vector3Int.RoundToInt(WorkPos.Value);
    //        }
    //    }

    //}
    //public void RegisterWorkPlaceRandom()
    //{
    //    if (workchunk.chunkData.structPos.Count <= 0)
    //    {
    //        Debug.LogWarning("place need to find other neibor struct");
    //        return;
    //    }
    //    switch (profession)
    //    {
    //        case Profession.Logger:
    //            {
    //                foreach (Structure_Point point in workchunk.chunkData.structPos)
    //                {
    //                    if (Structure_Point.GetBlockName(point) == BlockName.Wood)
    //                    {
    //                        workPlace = Structure_Point.Get_WorldPoint(point,workchunk.chunkData);
    //                        break;
    //                    }
    //                }
    //                break;
    //            }
    //        case Profession.miner:
    //            {
    //                foreach (Structure_Point point in workchunk.chunkData.structPos)
    //                {
    //                    if (Structure_Point.GetBlockName(point) == BlockName.Coal || Structure_Point.GetBlockName(point) == BlockName.Iron)
    //                    {
    //                        workPlace = Structure_Point.Get_WorldPoint(point, workchunk.chunkData);
    //                        break;
    //                    }
    //                }
    //                break;
    //            }
    //        case Profession.Collectors:
    //            {
    //                foreach (Structure_Point point in workchunk.chunkData.structPos)
    //                {
    //                    if (Structure_Point.GetBlockName(point) == BlockName.Mushroom || Structure_Point.GetBlockName(point) == BlockName.Vegetable)
    //                    {
    //                        workPlace = Structure_Point.Get_WorldPoint(point, workchunk.chunkData);
    //                        break;
    //                    }
    //                }
    //                break;
    //            }
    //    }


    //Chunk chunk;
    //Vector3Int pos = workPlace;
    //Vector3Int chunkPos = ChunkHelper.GetChunkPos(pos);
    //if (World.chunks.TryGetValue(chunkPos, out chunk))
    //{
    //    if (chunk.chunkData.structPos.Count > 0)
    //    {
    //        workPlace = Vector3Int.FloorToInt(Structure_Point.Get_WorldPoint(chunk.chunkData.structPos[0], chunk.chunkData) + Vector3.up);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("place need to find other neibor struct");
    //    }
    //}
    //}
    public void SelectEnter(SelectEnterEventArgs args)
    {
        isGrab = true;
        //resourceMap.Set(transform, true);
    }
    public void SelectExit(SelectExitEventArgs args)
    {
        isGrab = false;
        rb.rotation = Quaternion.identity;
        RegisterWorkPlace();
        //resourceMap.Set(null, false);
    }

    public void DrawList()
    {
        Gizmos.color = Color.red;
        for (int i = 1; i < path.Count; i++)
        {
            Gizmos.DrawLine(path[i].pos, path[i - 1].pos);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (NowState == WalkState)
            {
                graph.Draw();
                DrawList();
            }
        }
    }
}
