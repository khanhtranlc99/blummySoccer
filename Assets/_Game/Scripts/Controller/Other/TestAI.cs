using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
[System.Serializable]
public class RaycastPoint
{
    public Vector3 startPoint;
    public Vector3 endPoint;
}
 

 
public class TestAI : Singleton<TestAI>
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
    [SerializeField] protected int totalBalls = 3;
    [SerializeField] int currentBalls;
    protected float sensitivity = 100f;

    protected bool isDragging = false;

    protected float InitDistace = 0;
    protected Coroutine TempRoutineFire;
    public float ForceAccumulated = 1000;

    protected float maxDistanceToGetMaxForce = 20f;
    protected float power;
 
    protected const string ANIMATION_Charge = "Charge_AI";
    protected const string ANIMATION_SHOOTMED = "ShootMax_AI";
    protected const string ANIMATION_IDLE = "Idle_AI";

    [Header("Fake Direction")]
    [SerializeField] private GameObject fakeDirectionContainer;
    [SerializeField] private Transform fakeDecoy;
    [SerializeField] private Transform fakeArrow;
    [SerializeField] private LineRenderer fakeLine;
    protected BallPvP TempBall;

    public GameObject goals;
    [SerializeField] protected LineRenderer lineRenderer;
    public Vector2 initialDirection;
    public List<RaycastPoint> lsRaycastPoints;
    RaycastHit2D hit;
    public LayerMask hitLayers;
    public Transform parentArrow;
    public Sequence sequence;
    public BallPvP tempBall;
    List<int> lsTime = new List<int>() {1,2,3,4 };
    public List<Vector3> lsTempPostRay;
    [SerializeField] protected LineRenderer linePower;
    Coroutine coroutine_line;
    int lineIndex;
    int countState = 0;
    public bool cooldown;
    int getForce(int param)
    {
        if (param == 1)
        {
            return 40;
        }
        if (param == 2)
        {
            return 50;
        }
        if (param == 3)
        {
            return 60;
        }
        if (param == 4)
        {
            return 100;
        }
        return 0;
    }
    public Vector3 firstPost;
 
    private void Start()
    {
        firstPost = this.transform.position;
        lineIndex = 1;
        linePower.positionCount = 1;
        linePower.SetPosition(0, FakeBall.transform.position);
        cooldown = false;
        anim.Play(ANIMATION_IDLE);
    }
    public void StartAI()
    {
        var temp = this.transform.localScale ;
        var randomTime = Random.RandomRange(0.2f,1);
        //var randomTime =5;
        sequence =  DOTween.Sequence();
        sequence.Append( parentArrow.DORotate(new Vector3(0,0,100), randomTime));
        sequence.Append( parentArrow.DORotate(new Vector3(0, 0, 0), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, -100), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, 0), randomTime));
        sequence.OnUpdate(delegate { HandleTest(); });
        sequence.OnComplete(delegate { this.transform.localScale = new Vector3(-temp.x, temp.y,0); StartAIFindUser(); });
    }    

    public void HandleTest()
    {
        Vector3 mousePosition = Arrow.position;
        Vector3 direction = (mousePosition - FakeBall.transform.position).normalized;

        // Lưu hướng của raycast để sử dụng khi khởi tạo bóng
        initialDirection = direction;

        // Sử dụng CircleCast thay cho Raycast, với bán kính hình tròn là 0.4f
        Vector3 currentDirection = direction;
        Vector3 currentOrigin = FakeBall.transform.position;

        // Khởi tạo LineRenderer
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, currentOrigin);

        int lineIndex = 1;

        // Clear the list at the start of the raycasting process
        lsRaycastPoints.Clear();

        for (int i = 0; i < 4; i++)
        {
            // CircleCast để phát hiện va chạm với các lớp được chỉ định bởi hitLayers
            hit = Physics2D.CircleCast(currentOrigin, 0.1f, currentDirection, Mathf.Infinity, hitLayers);

            // Debug raycast với đầu là hình tròn
            Debug.DrawRay(currentOrigin, currentDirection * 100f, Color.red, 0.05f);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Goal")
                {
                    countState = 1;
                    sequence.Kill();
                    if(!cooldown)
                    {
                        cooldown = true;
                        StartCoroutine(HandleClick());
                    }    
                  
                    Debug.LogError("1");
                }
                // Tọa độ của điểm va chạm
                Vector2 hitPoint = hit.point;

                // Tính toán hướng vuông góc với raycast (normal của va chạm)
                Vector2 normal = hit.normal;

                // Vẽ một hình tròn tại điểm va chạm để kiểm tra
                Vector2 circleCastOrigin = hitPoint + normal * 0.4f;


                // Tăng số điểm của LineRenderer
                //lsTempPostRay.Add(new Vector3(circleCastOrigin.x, circleCastOrigin.y, 0));
                lineRenderer.positionCount = lineIndex + 1;
                lineRenderer.SetPosition(lineIndex, new Vector3(circleCastOrigin.x, circleCastOrigin.y, 0));
                lineIndex++;

                // Tạo và lưu thông tin raycast vào lsRaycastPoints
                RaycastPoint raycastPoint = new RaycastPoint
                {
                    startPoint = currentOrigin,
                    endPoint = (Vector3)circleCastOrigin
                };
                lsRaycastPoints.Add(raycastPoint);

                // Tính toán hướng phản chiếu từ điểm va chạm
                Vector2 reflectDirection = Vector2.Reflect(currentDirection, normal);

                // Vẽ raycast phản chiếu từ điểm va chạm
                Debug.DrawRay(circleCastOrigin, reflectDirection * 100f, Color.blue, 0.05f);

                // Cập nhật hướng và gốc cho lần phản chiếu tiếp theo
                currentDirection = reflectDirection;
                currentOrigin = circleCastOrigin;


            }
            else
            {
                // Nếu không có va chạm, dừng vòng lặp
                break;
            }
        }
    }




    public void StartAIFindUser()
    {
        var temp = this.transform.localScale;
        var randomTime = Random.RandomRange(0.2f, 1);
 
        sequence = DOTween.Sequence();
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, 100), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, 0), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, -100), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, 0), randomTime));
        sequence.OnUpdate(delegate { HandleFindUser(); });
        sequence.OnComplete(delegate { this.transform.localScale = new Vector3(-temp.x, temp.y, 0); StartAIFindGround(); });
    }

    public void HandleFindUser()
    {
        Vector3 mousePosition = Arrow.position;
        Vector3 direction = (mousePosition - FakeBall.transform.position).normalized;

        // Lưu hướng của raycast để sử dụng khi khởi tạo bóng
        initialDirection = direction;

        // Sử dụng CircleCast thay cho Raycast, với bán kính hình tròn là 0.4f
        Vector3 currentDirection = direction;
        Vector3 currentOrigin = FakeBall.transform.position;

        // Khởi tạo LineRenderer
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, currentOrigin);

        int lineIndex = 1;

        // Clear the list at the start of the raycasting process
        lsRaycastPoints.Clear();

        for (int i = 0; i < 4; i++)
        {
            // CircleCast để phát hiện va chạm với các lớp được chỉ định bởi hitLayers
            hit = Physics2D.CircleCast(currentOrigin, 0.1f, currentDirection, Mathf.Infinity, hitLayers);

            // Debug raycast với đầu là hình tròn
            Debug.DrawRay(currentOrigin, currentDirection * 100f, Color.red, 0.05f);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    countState = 2;
                    sequence.Kill();
                    if (!cooldown)
                    {
                        cooldown = true;
                        StartCoroutine(HandleClick());
                    }
                    Debug.LogError("2");
                }
                // Tọa độ của điểm va chạm
                Vector2 hitPoint = hit.point;

                // Tính toán hướng vuông góc với raycast (normal của va chạm)
                Vector2 normal = hit.normal;

                // Vẽ một hình tròn tại điểm va chạm để kiểm tra
                Vector2 circleCastOrigin = hitPoint + normal * 0.4f;


                // Tăng số điểm của LineRenderer
                //lsTempPostRay.Add(new Vector3(circleCastOrigin.x, circleCastOrigin.y, 0));
                lineRenderer.positionCount = lineIndex + 1;
                lineRenderer.SetPosition(lineIndex, new Vector3(circleCastOrigin.x, circleCastOrigin.y, 0));
                lineIndex++;

                // Tạo và lưu thông tin raycast vào lsRaycastPoints
                RaycastPoint raycastPoint = new RaycastPoint
                {
                    startPoint = currentOrigin,
                    endPoint = (Vector3)circleCastOrigin
                };
                lsRaycastPoints.Add(raycastPoint);

                // Tính toán hướng phản chiếu từ điểm va chạm
                Vector2 reflectDirection = Vector2.Reflect(currentDirection, normal);

                // Vẽ raycast phản chiếu từ điểm va chạm
                Debug.DrawRay(circleCastOrigin, reflectDirection * 100f, Color.blue, 0.05f);

                // Cập nhật hướng và gốc cho lần phản chiếu tiếp theo
                currentDirection = reflectDirection;
                currentOrigin = circleCastOrigin;


            }
            else
            {
                // Nếu không có va chạm, dừng vòng lặp
                break;
            }
        }

    }




    public void StartAIFindGround()
    {
        var temp = this.transform.localScale;
        var randomTime = Random.RandomRange(0.2f, 1);
        sequence = DOTween.Sequence();
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, 100), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, 0), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, -100), randomTime));
        sequence.Append(parentArrow.DORotate(new Vector3(0, 0, 0), randomTime));
        sequence.OnUpdate(delegate { HandleFindGround(); });
        sequence.OnComplete(delegate { this.transform.localScale = new Vector3(-temp.x, temp.y, 0); StartAI(); });
    }

    public void HandleFindGround()
    {
        Vector3 mousePosition = Arrow.position;
        Vector3 direction = (mousePosition - FakeBall.transform.position).normalized;

        // Lưu hướng của raycast để sử dụng khi khởi tạo bóng
        initialDirection = direction;

        // Sử dụng CircleCast thay cho Raycast, với bán kính hình tròn là 0.4f
        Vector3 currentDirection = direction;
        Vector3 currentOrigin = FakeBall.transform.position;

        // Khởi tạo LineRenderer
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, currentOrigin);

        int lineIndex = 1;

        // Clear the list at the start of the raycasting process
        lsRaycastPoints.Clear();

        for (int i = 0; i < 4; i++)
        {
            // CircleCast để phát hiện va chạm với các lớp được chỉ định bởi hitLayers
            hit = Physics2D.CircleCast(currentOrigin, 0.1f, currentDirection, Mathf.Infinity, hitLayers);

            // Debug raycast với đầu là hình tròn
            Debug.DrawRay(currentOrigin, currentDirection * 100f, Color.red, 0.05f);

            if (hit.collider != null)
            {
                if (  hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") /*|| hit.collider.gameObject.layer == LayerMask.NameToLayer("Default")*/)
                {
                    countState = 3;
                    sequence.Kill();
                    if (!cooldown)
                    {
                        cooldown = true;
                        StartCoroutine(HandleClick());
                        Debug.LogError("3");
                    }
       
                }
                // Tọa độ của điểm va chạm
                Vector2 hitPoint = hit.point;

                // Tính toán hướng vuông góc với raycast (normal của va chạm)
                Vector2 normal = hit.normal;

                // Vẽ một hình tròn tại điểm va chạm để kiểm tra
                Vector2 circleCastOrigin = hitPoint + normal * 0.4f;


                // Tăng số điểm của LineRenderer
                //lsTempPostRay.Add(new Vector3(circleCastOrigin.x, circleCastOrigin.y, 0));
                lineRenderer.positionCount = lineIndex + 1;
                lineRenderer.SetPosition(lineIndex, new Vector3(circleCastOrigin.x, circleCastOrigin.y, 0));
                lineIndex++;

                // Tạo và lưu thông tin raycast vào lsRaycastPoints
                RaycastPoint raycastPoint = new RaycastPoint
                {
                    startPoint = currentOrigin,
                    endPoint = (Vector3)circleCastOrigin
                };
                lsRaycastPoints.Add(raycastPoint);

                // Tính toán hướng phản chiếu từ điểm va chạm
                Vector2 reflectDirection = Vector2.Reflect(currentDirection, normal);

                // Vẽ raycast phản chiếu từ điểm va chạm
                Debug.DrawRay(circleCastOrigin, reflectDirection * 100f, Color.blue, 0.05f);

                // Cập nhật hướng và gốc cho lần phản chiếu tiếp theo
                currentDirection = reflectDirection;
                currentOrigin = circleCastOrigin;


            }
            else
            {
                // Nếu không có va chạm, dừng vòng lặp
                break;
            }
        }

    }


    public IEnumerator HandleClick()
    {

 
  
        if (totalBalls > 0)
        {
            totalBalls -= 1;
            Debug.LogError("totalBalls_" + totalBalls);
            anim.Play(ANIMATION_Charge);
            effectCharge.gameObject.SetActive(true);
            effectCharge.Play();
            var force = lsTime[Random.Range(0, lsTime.Count)];
            yield return new WaitForSeconds(force);
            effectCharge.gameObject.SetActive(false);
            anim.Play(ANIMATION_SHOOTMED);
            var temp = SimplePool2.Spawn(tempBall);
            temp.transform.position = FakeBall.transform.position;
      
            Debug.LogError("____CountState_____" + countState);
            temp.PushBall(initialDirection * getForce(force));
            PvPController.Instance.pvpScene.HandleSubtrackAI();


            yield return new WaitForSeconds(Random.Range(1, 3));
            cooldown = false;
            StartAI();


        }
        else
        {
            yield return null;
        }    
 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            this.transform.position = firstPost;
        }
    }

    public void StopAI()
    {
        StopAllCoroutines();
        DOTween.KillAll();
    }    


}