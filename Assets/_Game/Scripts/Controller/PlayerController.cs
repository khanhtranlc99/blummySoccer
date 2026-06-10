// using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Example9;
public class PlayerController : Singleton<PlayerController>
{
    [Header("Components")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;
    [Header("Effects")]
    [SerializeField] protected ParticleSystem effectCharge;
    [SerializeField] protected ParticleSystem effectShoot;

    [Header("Childs")]
    [SerializeField] protected Transform Arrow;
    [SerializeField] protected Transform Decoy;
    [SerializeField] protected Transform FakeBall, DirectContainer;
    [SerializeField] protected Camera Cam;
    [SerializeField] protected LineRenderer powerLineRenderer;
    [Header("Values")]
    protected int totalBalls = 5;
    public int currentBalls;
    protected float sensitivity = 100f;

    protected bool isDragging = false;

    protected float InitDistace = 0;
    protected Coroutine TempRoutineFire;
    public float ForceAccumulated = 1;
 
    protected float maxDistanceToGetMaxForce = 20f;
    protected float power;
    protected const string ANIMATION_POWER = "Power";
    protected const string ANIMATION_SHOOTLOW = "ShootLow";
    protected const string ANIMATION_SHOOTMED = "ShootMed";
    protected const string ANIMATION_SHOOTMAX = "ShootMax";

    private Vector3 firstPost;  
    public GameObject objCanvas; 
 
    private void Start()
    {
        TimeManager.Instance.OnReset();
        this.Cam = Camera.main;
        this.InitDistace = (this.Decoy.position - this.FakeBall.position).magnitude;
        currentBalls = totalBalls;
        fakeDirectionContainer.SetActive(false);
        EventHandler.ExecuteEvent(EventID.OnBallChanged, currentBalls);



#if UNITY_EDITOR
        sensitivity = 200f;
#else
        sensitivity = 100f;
#endif

        firstPost = this.transform.position;
    }
    public bool IsPointerOverUI()
    {
        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(0);
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

    }

    private void Update()
    {
        DragProcessing();

        powerLineRenderer.SetPosition(0, FakeBall.position);
        powerLineRenderer.SetPosition(1, Decoy.position);

        fakeLine.SetPosition(0, FakeBall.position);
        fakeLine.SetPosition(1, fakeDecoy.position);

    }
    private void DragProcessing()
    {
        if (GameManager.Instance.GAME_STATE != GAME_STATE.PLAYING) return;
        if (currentBalls <= 0) return;

        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            if (IsPointerOverUI()) return;
            OnBeginDrag();
        }

        if (isDragging) OnDrag();

    }
    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            OnEndDrag();
        }
    }
    #region PROCESS FIRE BALL

    #region BeginDrag
    protected void OnBeginDrag()
    {
        isDragging = true;
        // StartAccumulating();
        TimeManager.Instance.DoSlowmotion();

        effectCharge.gameObject.SetActive(true);
        Vector3 CurrentDir = this.Decoy.position - this.FakeBall.position;
        this.Decoy.position = CurrentDir.normalized * this.InitDistace + this.FakeBall.position;
        //Spawn temp ball
        this.TempBall = CreateController.Instance.GetPoolObject(PoolEnum.Ball).GetComponent<BallController>();
        this.TempBall.ResetBall();
        this.TempBall.transform.position = this.FakeBall.position;
        this.TempBall.transform.SetParent(this.transform);
        this.TempBall.transform.localScale = Vector3.one;

        this.TempBall.circleCollider2D.enabled = false;
        this.TempBall.gameObject.SetActive(true);

        ForceAccumulated = 0;

    }
    #endregion

    #region Drag
    protected BallController TempBall;
    // private Vector3 previousMousePos;
    protected void OnDrag()
    {
        if (!isDragging) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        Decoy.position += new Vector3(mouseX, mouseY, 0);



        Vector3 direction = Decoy.position - FakeBall.position;
        if (direction.magnitude > maxDistanceToGetMaxForce)
        {
            direction = direction.normalized * maxDistanceToGetMaxForce;
        }
        Vector2 pointDirection = Decoy.position - FakeBall.position;
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
        Arrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        angle = transform.eulerAngles.y != 0 ? (angle > 0 ? angle - 180f : angle + 180f) : angle;

        if (angle >= 90 || angle <= -90)
        {
            angle = Mathf.Clamp(angle, -89, 89);
            transform.rotation = Quaternion.Euler(Vector3.up * (transform.rotation.y != 0 ? 0 : 180));
        }

        if (angle < 90 && angle > -90)
            Decoy.position = FakeBall.position + direction;




        Arrow.transform.position = Decoy.position;

        // powerLineRenderer.SetPosition(0, FakeBall.position);
        // powerLineRenderer.SetPosition(1, Decoy.position);
        // DrawReflection(FakeBall.position, direction.normalized, 2);

        Accumulating();
    }
    bool shaking = false;
    protected void Accumulating()
    {
        power = ForceAccumulated / Constant.MaxForceAccumulated;
        anim.SetFloat(ANIMATION_POWER, power);
        effectCharge.transform.localScale = Vector2.one * power;
        effectShoot.transform.localScale = Vector2.one * power;

        float sqrDistance = (FakeBall.position - Decoy.position).sqrMagnitude;


        float normalizedDistance = Mathf.Clamp01(Mathf.Sqrt(sqrDistance) / maxDistanceToGetMaxForce);

        this.ForceAccumulated = normalizedDistance * Constant.MaxForceAccumulated;

        if (CanShake())
        {
            //Camera Shake
            if (!shaking)
            {
                shaking = true;
                CameraManager.Instance.ShakeMainCam(true, CAMERA_SHAKE_TYPE.MAXFORCE_LOOP);
            }

        }
        else
        {
            if (shaking)
            {
                shaking = false;
                CameraManager.Instance.ShakeMainCam(false);
            }
        }

    }
    #endregion
    #region EndDrag
    protected void OnEndDrag()
    {
        isDragging = false;


        // powerLineRenderer.positionCount = 0;
        SetLastDirection();

        Vector3 CurrentDir = this.Decoy.position - this.FakeBall.position;
        this.Decoy.position = CurrentDir.normalized * this.InitDistace + this.FakeBall.position;
        this.TempBall.circleCollider2D.enabled = true;

        CreateController.Instance.SetParentToPool(this.TempBall.gameObject);

        if (ForceAccumulated >= Constant.MaxForceAccumulated * 0.05f)
        {
            Fire();
        }
        else
        {
            CreateController.Instance.Despawn(TempBall.gameObject);
        }

        Arrow.transform.position = Decoy.position;
        // powerLineRenderer.SetPosition(1, Decoy.position);
        // powerLineRenderer.SetPosition(1, Decoy.position);

        effectCharge.gameObject.SetActive(false);
        CameraManager.Instance.ShakeMainCam(false);
        TimeManager.Instance.OnReset();

        anim.SetFloat(ANIMATION_POWER, 0f);
    }




    public void Fire()
    {
        //Check has ammo
        //Stop Camera Shake
        if (power >= 0.9f)
        {
            anim.Play(ANIMATION_SHOOTMAX, 0, 0f);
        }
        else if (power < 0.5f)
        {
            anim.Play(ANIMATION_SHOOTLOW, 0, 0f);
        }
        else
        {
            anim.Play(ANIMATION_SHOOTMED, 0, 0f);
        }
        effectShoot.Play();

        Vector3 Direction = this.Decoy.position - this.FakeBall.position;

        currentBalls--;
        EventHandler.ExecuteEvent(EventID.OnBallChanged, currentBalls);
        this.TempBall.PushBall(Direction.normalized * 15f * this.ForceAccumulated);
    }
    #endregion
    protected bool isMaxForce()
    {
        if (this.ForceAccumulated >= Constant.MaxForceAccumulated || (Constant.MaxForceAccumulated - this.ForceAccumulated) < .1)
        {
            this.ForceAccumulated = Constant.MaxForceAccumulated;
            return true;
        }
        return false;
    }
    protected bool CanShake()
    {
        if (this.ForceAccumulated >= Constant.MaxForceAccumulated * 0.8f)
        {
            return true;
        }
        return false;
    }
    #endregion
    [Header("Fake Direction")]
    [SerializeField] private GameObject fakeDirectionContainer;
    [SerializeField] private Transform fakeDecoy;
    [SerializeField] private Transform fakeArrow;
    [SerializeField] private LineRenderer fakeLine;
    protected void SetLastDirection()
    {
        fakeDirectionContainer.SetActive(true);

        fakeDecoy.position = Decoy.position;
        fakeArrow.position = Decoy.position;

        Vector2 pointDirection = Decoy.position - FakeBall.position;
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
        fakeArrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<WaterSpring>() != null)
        {
            this.transform.position = firstPost;
        }
    }

}
