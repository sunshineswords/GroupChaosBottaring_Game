using UnityEngine;
using UnityEngine.InputSystem;
using System;
using static UnityEngine.UI.Button;
using UnityEngine.Serialization;

public class UIInputSystem_Gabu : MonoBehaviour
{
    #region 変数

    private InputAction _action;
    [SerializeField] InputActionAsset playerInput;
    [SerializeField] string actionName;

    // Event delegates triggered on click.
    [FormerlySerializedAs("onPressed")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    #endregion
    #region 関数

    private void OnGetThisAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }
        if (context.performed)
        {
            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }
        if (context.canceled)
        {
            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }
    }

    #endregion

    private void Awake()
    {
        _action = playerInput.FindAction(actionName);
        if (_action == null)
        {
            Debug.LogError($"存在しないアクションを指定しています:{actionName} of {playerInput}");
        }
    }

    private void OnEnable()
    {
        _action.started += OnGetThisAction;
        _action.performed += OnGetThisAction;
        _action.canceled += OnGetThisAction;
    }

    private void OnDisable()
    {
        _action.started -= OnGetThisAction;
        _action.performed -= OnGetThisAction;
        _action.canceled -= OnGetThisAction;
    }
}
