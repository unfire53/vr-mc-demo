using System;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;

public class Controler : MonoBehaviour
{
    public InputActionReference leftTrigger;
    public InputActionReference rightTrigger;
    public InputActionReference leftGrip;
    public InputActionReference rightGrip;
    public InputActionReference left2DAxis;
    public InputActionReference right2DAxis;
    public InputActionReference leftX;
    public InputActionReference leftY;
    public InputActionReference rightA;
    public InputActionReference rightB;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    public Transform xrOrigin;
    public Transform leftHand;
    public Transform rightHand;


    public event Action<IXRActivateInteractor> OnSelectActive;
    public event Action<Vector3> OnStartDraw;
    public event Action<Vector3> OnDraw;
    public event Action OnEndDraw;

    public GameObject ui;
    public GameObject backPack;

    public Vector3 startPosition;

    public bool leftTriggerPress;
    public bool rightTriggerPress;
    public bool x_Press;
    public bool y_Press;
    public bool isWire;
    public float moveSpeed;
    private bool endWait;

    public float WaitTime { get; private set; }

    private void Update()
    {
        LeftTrigger();
        RightTrigger();
    }
    private void LateUpdate()
    {
        LeftButton();
        RightButton();
        Movement();
    }
    void LeftTrigger()
    {
        if (!leftTriggerPress) return;
        if (WaitTime < 0.2f)
        {
            WaitTime += Time.deltaTime;
            return;
        }
        if(leftRay.isSelectActive)
        {
            if (OnSelectActive != null) OnSelectActive(leftRay);
        }else
        {
            if (OnDraw != null) OnDraw(leftHand.position);
        }
    }
    void RightTrigger()
    {
        if (!rightTriggerPress) return;
        if (WaitTime < 0.2f)
        {
            WaitTime += Time.deltaTime;
            return;
        }
        if (rightRay.isSelectActive)
        {
            if(OnSelectActive != null)OnSelectActive(rightRay);
        }
        else
        {
            if (OnDraw != null) OnDraw(rightHand.position);
        }
    }
    void LeftButton()
    {
        if (x_Press)
        {
            transform.localScale += 0.1f * Vector3.one;
        }
        else if (y_Press)
        {
            transform.localScale -= 0.1f * Vector3.one;
        }
    }
    void RightButton()
    {
    }
    void Movement()
    {
        var movement = left2DAxis.action.ReadValue<Vector2>();
        if (movement != Vector2.zero)
        {
            Quaternion quaternion = Quaternion.Euler(0, xrOrigin.transform.eulerAngles.y, 0);
            transform.position += quaternion * new Vector3(movement.x, 0, movement.y) * Time.deltaTime * moveSpeed;
        }
    }
    void Start()
    {
        leftTrigger.action.started += Left_Trigger_Press;
        leftTrigger.action.canceled += Left_Trigger_Release;
        rightTrigger.action.started += Right_Trigger_Press;
        rightTrigger.action.canceled += Right_Trigger_Release;

        leftGrip.action.started += Left_Grip_Press;
        leftGrip.action.canceled += Left_Grip_Release;
        rightGrip.action.started += Right_Grip_Press;
        rightGrip.action.canceled += Right_Grip_Release;


        leftX.action.started += X_Press;
        leftY.action.started += Y_Press;
        leftX.action.canceled += X_Release;
        leftY.action.canceled += Y_Release;
        rightA.action.started += A_Press;
        rightB.action.started += B_Press;
    }


    private void OnDestroy()
    {
        leftTrigger.action.started -= Left_Trigger_Press;
        leftTrigger.action.canceled -= Left_Trigger_Release;
        rightTrigger.action.started -= Right_Trigger_Press;
        rightTrigger.action.canceled -= Right_Trigger_Release;

        leftGrip.action.started -= Left_Grip_Press;
        leftGrip.action.canceled -= Left_Grip_Release;
        rightGrip.action.started -= Right_Grip_Press;
        rightGrip.action.canceled -= Right_Grip_Release;

        leftX.action.started -= X_Press;
        leftY.action.started -= Y_Press;
        leftX.action.canceled -= X_Release;
        leftY.action.canceled -= Y_Release;
        rightA.action.started -= A_Press;
        rightB.action.started -= B_Press;
    }
    private void Left_Trigger_Release(InputAction.CallbackContext context)
    {
        if(!leftRay.isSelectActive &&!TouchUI(leftRay) && WaitTime > 0.2f)
        {
            if (OnEndDraw != null) OnEndDraw();
        }
        leftTriggerPress = false;
        WaitTime = 0;
        
    }

    private void Left_Trigger_Press(InputAction.CallbackContext context)
    {
        leftTriggerPress = true;
        if(leftRay.isSelectActive)
        {
            if (OnSelectActive != null) OnSelectActive(leftRay);
        }
        startPosition = leftHand.position;
        if (OnStartDraw != null && !TouchUI(leftRay)) OnStartDraw(startPosition);
    }
    private void Right_Trigger_Release(InputAction.CallbackContext context)
    {
        if (!rightRay.isSelectActive && !TouchUI(rightRay) && WaitTime > 0.2f)
        {
            if (OnEndDraw != null) OnEndDraw();
        }
        rightTriggerPress = false;
        WaitTime = 0;
    }

    private void Right_Trigger_Press(InputAction.CallbackContext context)
    {
        rightTriggerPress = true;
        if(rightRay.isSelectActive)
        {
            if (OnSelectActive != null) OnSelectActive(rightRay);
        }
        startPosition = rightHand.position;
        if (OnStartDraw != null && !TouchUI(rightRay)) OnStartDraw(startPosition);
    }
    bool TouchUI(XRRayInteractor rayInteractor)
    {
        if (rayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult result))
        {
            return true;
        }
        return false;
    }
    private void Right_Grip_Release(InputAction.CallbackContext context)
    {
    }

    private void Right_Grip_Press(InputAction.CallbackContext context)
    {
    }

    private void Left_Grip_Release(InputAction.CallbackContext context)
    {
    }

    private void Left_Grip_Press(InputAction.CallbackContext context)
    {
    }

    private void X_Press(InputAction.CallbackContext context)
    {
        x_Press = true;
    }
    private void Y_Press(InputAction.CallbackContext context)
    {
        y_Press = true;
    }
    private void X_Release(InputAction.CallbackContext context)
    {
        x_Press = false;
    }
    private void Y_Release(InputAction.CallbackContext context)
    {
        y_Press = false;
    }
    private void A_Press(InputAction.CallbackContext context)
    {
        ui.SetActive(!ui.activeSelf);
    }
    private void B_Press(InputAction.CallbackContext context)
    {
        backPack.SetActive(!backPack.activeSelf);
    }

}
