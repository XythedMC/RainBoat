using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _pInput;

    public void EnableInput()
    {
        _pInput.Enable();
    }

    public void DisableInput()
    {
        _pInput.Disable();
    }
}