
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas thirdPersionCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera= GetComponent<CinemachineVirtualCamera>();
        aimAction = input.actions["Aim"];
    }
    private void OnEnable()
    {
        aimAction.performed += contex => StartAim();
        aimAction.canceled += contex => CancelAim();

    }
    private void OnDisable()
    {
        aimAction.performed -= contex => StartAim();
        aimAction.canceled -= contex => CancelAim();
    }
    void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled= true;
        thirdPersionCanvas.enabled= false;
    }
    void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersionCanvas.enabled = true;
    }

}
