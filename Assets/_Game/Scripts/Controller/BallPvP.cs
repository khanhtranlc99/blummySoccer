using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeBallPvP
{
    Ai,
    User
}
public class BallPvP : MonoBehaviour
{
    public TypeBallPvP typeBallPvP;
    [SerializeField] protected Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D circleCollider2D;
    public ParticleSystem vfxSmoke;
    protected bool CanSpawnHitFx = false;
    public bool isCount;
    private void OnEnable()
    {
        PvPController.Instance.playerContain.lsBallPvP.Add(this);
        if (circleCollider2D == null) circleCollider2D = GetComponent<CircleCollider2D>();
        StartCoroutine(RoutineCountTimeCanDoFxhit());
        isCount = false;
    }
    private void OnDisable()
    {

    }
    public void PushBall(Vector3 Force)
    {
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        this.rb.AddForce(Force, ForceMode2D.Impulse);
    }
    public void ResetBall()
    {
        this.rb.constraints = RigidbodyConstraints2D.None;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = 0;
        transform.rotation = Quaternion.identity;
        rb.Sleep();
        rb.WakeUp();
        this.rb.bodyType = RigidbodyType2D.Kinematic;
    }
    //Khi win thì cho bóng xoay nhanh
    protected void OnWin()
    {
        DOVirtual.DelayedCall(0.1f, () =>
        {
            this.rb.constraints = RigidbodyConstraints2D.FreezePosition;


            CameraManager.Instance.DoAnimWin(this.transform);
        });
        this.rb.AddTorque(100f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            if (GameManager.Instance.GAME_STATE == GAME_STATE.END)
            {
                return;
            }

            if (!isCount)
            {
                isCount = true;
                if (typeBallPvP == TypeBallPvP.User)
                {
                    PvPController.Instance.pvpScene.HandlePlusScoreUser();
                }
                else
                {
                    PvPController.Instance.pvpScene.HandlePlusScoreAi();
                }
            }    
 

            Vector3 direction = rb.linearVelocity.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject fx_confettiDirection = SimplePool2.Spawn(vfxSmoke.gameObject);
            fx_confettiDirection.transform.position = transform.position;
            fx_confettiDirection.transform.rotation = Quaternion.Euler(angle, 90f, 0f);
            fx_confettiDirection.SetActive(true);
            DOVirtual.DelayedCall(1f, () =>
            {
                SimplePool2.Despawn(fx_confettiDirection);
            });

            GameManager.Instance.GAME_STATE = GAME_STATE.END;
            TestAI.Instance.StopAI();
            PvPController.Instance.playerContain.HandleGoals();
            GlobalAudioPlayer.PlaySFX(eAudioType.WIN);
            //   OnWin();
            //cho cam chạy anim win
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<IAnimPlayable>()?.PlayAnim();
        if (!this.CanSpawnHitFx || this.rb.linearVelocity.magnitude < 7.5f)
            return;
   
        //hướng fx sẽ ngược với hướng bắn của bóng
        // Vector3 DirectFX = -collision.transform.position + this.transform.position;
        // FX_Hit.transform.position = this.transform.position - DirectFX.normalized * .3f;
        // FX_Hit.transform.up = DirectFX;
        GlobalAudioPlayer.PlaySFX(eAudioType.BALL_HIT);
        ContactPoint2D contact = collision.GetContact(0);

        GameObject FX_Hit = SimplePool2.Spawn(vfxSmoke.gameObject) ;
        FX_Hit.transform.position = contact.point;
        FX_Hit.gameObject.SetActive(true);
        DOVirtual.DelayedCall(.7f, delegate
        {
            SimplePool2.Despawn(FX_Hit);
        });

        if (collision.gameObject.tag == "Resspawn")
        {
            SimplePool2.Despawn(this.gameObject);
        }
    }
    IEnumerator RoutineCountTimeCanDoFxhit()
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(.2f);
        while (true)
        {
            yield return delay;
            this.CanSpawnHitFx = true;
        }
    }


  
}
