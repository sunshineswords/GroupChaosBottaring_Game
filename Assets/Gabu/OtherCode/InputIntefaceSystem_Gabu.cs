using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.UI.Button;

public class InputIntefaceSystem_Gabu : MonoBehaviour
{
    #region 変数

    private InputAction _action;
    [SerializeField] InputActionAsset playerInput;
    [SerializeField] string actionName;

    // Event delegates triggered on click.
    [FormerlySerializedAs("onStarted")]
    [SerializeField]
    private ButtonClickedEvent m_OnStardedClick = new ButtonClickedEvent();
    // Event delegates triggered on click.
    [FormerlySerializedAs("onStarted")]
    [SerializeField]
    private ButtonClickedEvent m_OnPreformedClick = new ButtonClickedEvent();
    // Event delegates triggered on click.
    [FormerlySerializedAs("onStarted")]
    [SerializeField]
    private ButtonClickedEvent m_OnCanceledClick = new ButtonClickedEvent();



    #endregion
    #region 関数

    private void OnGetThisAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UISystemProfilerApi.AddMarker("Button.onClick", this);
            if (m_OnStardedClick is not null)
            {
                m_OnStardedClick.Invoke();
            }
        }
        if (context.performed)
        {
            UISystemProfilerApi.AddMarker("Button.onClick", this);
            if (m_OnPreformedClick is not null)
            {
                m_OnPreformedClick.Invoke();
            }
        }
        if (context.canceled)
        {
            UISystemProfilerApi.AddMarker("Button.onClick", this);
            if (m_OnCanceledClick is not null)
            {
                m_OnCanceledClick.Invoke();
            }
        }
    }

    #endregion

    private void Awake()
    {
        try
        {
            _action = playerInput.FindAction(actionName);
        }
        catch (System.Exception)
        {
            Debug.LogWarning("PlayerInputが設定されていません");
        }
        if (_action == null)
        {
            Debug.LogError($"存在しないアクションを指定しています:{actionName} of {playerInput} in {gameObject}");
        }
    }

    private void OnEnable()
    {
        try
        {
            _action.started += OnGetThisAction;
            _action.performed += OnGetThisAction;
            _action.canceled += OnGetThisAction;
        }
        catch (System.Exception)
        {
            Debug.LogWarning("アクションが見つかりませんでした");
        }
    }

    private void OnDisable()
    {
        try
        {
            _action.started -= OnGetThisAction;
            _action.performed -= OnGetThisAction;
            _action.canceled -= OnGetThisAction;
        }
        catch (System.Exception)
        {
            Debug.LogWarning("アクションが見つかりませんでした");
        }
    }
}
