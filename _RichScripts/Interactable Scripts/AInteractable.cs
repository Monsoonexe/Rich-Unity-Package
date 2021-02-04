using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

/// <summary>
/// 
/// </summary>
/// <remarks>Must be tagged "Interactable"</remarks>
[SelectionBase]
[RequireComponent(typeof(Collider))]
public abstract class AInteractable : RichMonoBehaviour, IPlayerInteractable
{
    protected static CinemachineBrain mainCameraBrain;
    protected static float cameraDefaultBlendTime
        { get => mainCameraBrain.m_DefaultBlend.m_Time;
        set => mainCameraBrain.m_DefaultBlend.m_Time = value;
    } 

    [Header("---Interactable---")]
    [SerializeField]
    protected bool isInteractable = true;
    public bool IsInteractable { get { return isInteractable; } }// exposed; readonly

    [SerializeField]
    private MeshRenderer iconRenderer;

    [SerializeField]
    protected Transform interactionSpot;
    public Transform InteractionSpot { get { return interactionSpot; } }//exposed; readonly

    [SerializeField]//set in Inspector
    protected CinemachineVirtualCamera eventVirtualCam;
    public CinemachineVirtualCamera EventVirtualCam { get { return eventVirtualCam; } }//exposed, readonly

    [Header("--- Timing ---")]
    [SerializeField]
    protected float cameraMoveTime = 2.0f;

    [Header("---Game Events---")]
    public UnityEvent OnInteractEvent = new UnityEvent();

    protected override void Awake()
    {
        base.Awake();
        //gather static references
        if (!mainCameraBrain)
        {
            mainCameraBrain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
        }
    }

    protected virtual void Start()
    {
        //start with everything disabled
        ShowIcon(false);
        eventVirtualCam.enabled = false;
    }
    
    private void ShowIcon(bool showbool)
    {
        //fade ! cannot fade because the shader is Transparent Cutout and does not have a color property
        //    if (showbool)
        //    {
        //        iconRenderer.material.DOFade(1, iconFadeTime);
        //    }
        //    else
        //    {
        //        iconRenderer.material.DOFade(0, iconFadeTime);
        //    }
        //TODO boing in / fade out
        if(iconRenderer)
            iconRenderer.enabled = showbool;
    }

    public virtual void OnEnterRange(Player player)
    {
        ShowIcon(true);
    }

    public virtual void OnExitRange(Player player)
    {
        ShowIcon(false);
    }

    public virtual void Interact(Player player)
    {
        ShowIcon(false);
    }
}
