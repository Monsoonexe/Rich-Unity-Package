using System.Collections;
using UnityEngine;
using NaughtyAttributes;

[SelectionBase]
[RequireComponent(typeof(SphereCollider))]
public class SwivelTurret : RichMonoBehaviour
{
    /// <summary>
    /// Stop adjusting rotation if target within this angle.
    /// </summary>
    private static readonly float optimalAngle = .05f;

    [Header("---Settings---")]
    public Transform attackTarget;
    [SerializeField] private int minPitch = 15;
    [SerializeField] private int maxPitch = 75;
    [SerializeField] private float swivelSpeed = 3f;

    [Header("---Raycast Settings---")]
    [MinValue(0)]
    [OnValueChanged("UpdateYieldInstruction")]//useful for changing during edit time
    [SerializeField]
    private float raycastInterval = 1.0f;

    [SerializeField]
    private float raycastDistance = 1000;

    [SerializeField]
    private LayerMask raycastLayers = -1;

    [SerializeField]
    private QueryTriggerInteraction queryTriggers
        = QueryTriggerInteraction.Ignore;

    [Header("---Prefab Refs---")]
    [SerializeField]
    [Required]
    private Transform turretTransform;//pitch on X //CAN BE THE SAME OBJECT AS BASE, IF NO SEPARATE TURRET.  WHOLE OBJECT WILL ROTATE AS EXPECTED

    [SerializeField]
    [Required]
    private HardpointController hardpoint;

    [ReadOnly]
    private bool targetInEngagementRange = false;
    public bool TargetInEngagementRange { get => targetInEngagementRange; }

    //member Components
    private Collider myCollider;

    //runtime data
    private Coroutine raycastIntervalRoutine;
    private YieldInstruction yieldInstruction;

    protected override void Awake()
    {
        base.Awake();
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;//verify
    }

    private void Start()
    {
        yieldInstruction = new WaitForSeconds(raycastInterval);
    }

    private void OnEnable()
    {
        raycastIntervalRoutine = StartCoroutine(TrackTarget());
    }

    private void OnDisable()
    {
        if (raycastIntervalRoutine != null)
            StopCoroutine(raycastIntervalRoutine);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == attackTarget)
        {
            targetInEngagementRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == attackTarget)
        {
            targetInEngagementRange = false;
        }
    }

    private void Update()
    {
        if (attackTarget)
        {
            HandleTargetTracking();
        }
        else
        {
            targetInEngagementRange = false;//just to be sure
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Called when raycastInterval changed.</remarks>
    private void UpdateYieldInstruction()
        => yieldInstruction = new WaitForSeconds(raycastInterval);

    // Update is called once per frame
    private IEnumerator TrackTarget()
    {
        var richIsAwesome = true;

        while(richIsAwesome)//infinite loop
        {
            yield return yieldInstruction;//wait

            if (attackTarget && targetInEngagementRange && hardpoint.CanShoot())
            {
                HandleShooting();
            }
        }
    }

    private void HandleShooting()
    {
        RaycastHit raycastHitInfo;
        
        //check line of sight
        if (Physics.Raycast(
            turretTransform.position,
            turretTransform.forward, 
            out raycastHitInfo,
            raycastDistance, 
            raycastLayers,
            queryTriggers))
        {
            //can I see the target?
            if (raycastHitInfo.collider.transform == attackTarget)
            {
                //Debug.Log("Time To ZAPPPPPPPPPPPPPPPP!");
                hardpoint.FireWeapon();
            }
            else
            {
                //Debug.Log("Did not hit player.... hit: " + raycastHitInfo.collider.name, this.gameObject);
            }
        }
        else
        {
            //Debug.Log("HIT NOTHING!", this.gameObject);
        }
        
    }

    private void HandleTargetTracking()
    {
        var deltaTime = Time.deltaTime; //cache
        //get relative position of target 
        var relative = myTransform.InverseTransformPoint(attackTarget.position);

        //get the angle between base forward and target on horizontal plane
        var horAngle = Mathf.Atan2(relative.x, relative.z).ToDeg();

        //get relative position vertical
        relative = turretTransform.InverseTransformPoint(attackTarget.position);

        //get angle between turrets and target on vertical slice plane
        var vertAngle = Mathf.Atan2(relative.y, relative.z).ToDeg();

        //Debug.Log("HorAngle: " + horAngle + " | VertAngle: " + vertAngle);

        //handle base swivel
        if (RichMath.AbsoluteValue(horAngle) > optimalAngle)
            myTransform.Rotate(0, deltaTime * horAngle * swivelSpeed, 0);

        //handle turret pitch
        if (RichMath.AbsoluteValue(vertAngle) > optimalAngle)
            turretTransform.Rotate(deltaTime * -vertAngle * swivelSpeed, 0, 0);

        //CLAMP TURRET SO DOESN'T SHOOT OWN SPACE STATION!!!
        var localRot = turretTransform.localEulerAngles.x;
        if (localRot < 180)
        {
            if (localRot > minPitch)
            {
                localRot = minPitch;
            }
        }
        else if (localRot < 360 - maxPitch)
        {
            localRot = 360 - maxPitch;
        }

        turretTransform.localEulerAngles = new Vector3(localRot, 0, 0);
    }

}
