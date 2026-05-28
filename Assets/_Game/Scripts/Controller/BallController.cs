using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D circleCollider2D;
    protected bool CanSpawnHitFx = false;
    private void OnEnable()
    {
        if (circleCollider2D == null) circleCollider2D = GetComponent<CircleCollider2D>();
        StartCoroutine(RoutineCountTimeCanDoFxhit());
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

            GameObject fx_confettiDirection = CreateController.Instance.GetPoolItemVfx(PoolEnum.Fx_ConfettiDirectional);
            fx_confettiDirection.transform.position = transform.position;

            Vector3 direction = rb.linearVelocity.normalized;


            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            fx_confettiDirection.transform.rotation = Quaternion.Euler(angle, 90f, 0f);

            fx_confettiDirection.SetActive(true);

            DOVirtual.DelayedCall(1f, () =>
            {
                CreateController.Instance.Despawn(fx_confettiDirection);
            });
            OnWin();
            //cho cam chạy anim win
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<IAnimPlayable>()?.PlayAnim();
        if (!this.CanSpawnHitFx || this.rb.linearVelocity.magnitude < 7.5f)
            return;
        GameObject FX_Hit = CreateController.Instance.GetPoolObject(PoolEnum.Fx_BallHit);
        //hướng fx sẽ ngược với hướng bắn của bóng
        // Vector3 DirectFX = -collision.transform.position + this.transform.position;
        // FX_Hit.transform.position = this.transform.position - DirectFX.normalized * .3f;
        // FX_Hit.transform.up = DirectFX;
        GlobalAudioPlayer.PlaySFX(eAudioType.BALL_HIT);
        ContactPoint2D contact = collision.GetContact(0);
        FX_Hit.transform.position = contact.point;

        FX_Hit.gameObject.SetActive(true);
        DOVirtual.DelayedCall(.7f, delegate
        {
            CreateController.Instance.Despawn(FX_Hit);
        });
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
