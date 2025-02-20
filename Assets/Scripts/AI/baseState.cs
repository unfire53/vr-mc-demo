using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseState
{
    public FSM Resident;
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void LogicalUpdate();
    public abstract void PhyicsUpdate();
    public BaseState(FSM resident)
    { Resident = resident; }
}
public class WalkState : BaseState
{
    public WalkState(FSM resident) : base(resident)
    {
    }

    public override void LogicalUpdate()
    {
        Resident.WalkCheck();
    }

    public override void OnEnter()
    {
        Resident.WalkEnter();
    }

    public override void OnExit()
    {
        Resident.WalkExit();
    }

    public override void PhyicsUpdate()
    {
        Resident.Walk();
    }
}
public class WorkState : BaseState
{
    public WorkState(FSM resident) : base(resident)
    {
    }

    public override void LogicalUpdate()
    {
        Resident.WorkCheck();
    }

    public override void OnEnter()
    {
        Resident.WorkEnter();
    }

    public override void OnExit()
    {
        Resident.WorkExit();
    }

    public override void PhyicsUpdate()
    {
        Resident.Work();
    }
}
public class IdleState : BaseState
{
    public IdleState(FSM resident) : base(resident)
    {
    }

    public override void LogicalUpdate()
    {
        Resident.IdleCheck();
    }

    public override void OnEnter()
    {
        Resident.IdleEnter();
    }

    public override void OnExit()
    {
        Resident.IdleExit();
    }

    public override void PhyicsUpdate()
    {
        Resident.Idle();
    }
}
public class RestState : BaseState
{
    public RestState(FSM resident) : base(resident)
    {
    }

    public override void LogicalUpdate()
    {
        Resident.RestCheck();
    }

    public override void OnEnter()
    {
        Resident.RestEnter();
    }

    public override void OnExit()
    {
        Resident.RestExit();
    }

    public override void PhyicsUpdate()
    {
        Resident.Rest();
    }
}
