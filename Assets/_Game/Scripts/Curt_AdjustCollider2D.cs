#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class Curt_AdjustCollider2D : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D;
    private SpriteRenderer SpriteRenderer;
    [SerializeField] protected float EdgeRadius = .21f;
    [SerializeField] protected float VirtualDistance = .42f;
    private void OnValidate()
    {
        ReloadCollider();
    }
    protected void ReloadCollider()
    {
        if (this == null) return;
        if (BoxCollider2D == null) this.BoxCollider2D = GetComponent<BoxCollider2D>();
        if (SpriteRenderer == null) this.SpriteRenderer = GetComponent<SpriteRenderer>();
        this.BoxCollider2D.edgeRadius = EdgeRadius;
        if (SpriteRenderer.drawMode != SpriteDrawMode.Tiled)
        {
            this.SpriteRenderer.drawMode = SpriteDrawMode.Tiled;
            Debug.LogError("Cho Draw Mode thành TileMode");
        }
        Sprite Raw9SlicedTexture = Resources.Load<Sprite>("Square9Sliced");
        this.SpriteRenderer.sprite = Raw9SlicedTexture;

        //Nếu localscale nó khác hẳn với Vector2.one thì mới chỉnh lại Size và Scale
        if (Vector2.Distance(Vector2.one, this.transform.localScale) > 0.05)
        {
            //Set lại size theo scale 
            this.SpriteRenderer.size = new Vector2(this.transform.localScale.x, this.transform.localScale.y);
            this.transform.localScale = Vector3.one;
        }
        UpdateCollider();
    }
    protected void UpdateCollider()
    {

        float sizeX = this.SpriteRenderer.size.x - this.VirtualDistance;
        float sizeY = this.SpriteRenderer.size.y - this.VirtualDistance;

        BoxCollider2D.size = new Vector2(sizeX, sizeY);
    }

    Curt_AdjustCollider2D()
    {
        PrefabStage.prefabSaved += delegate
        {
            ReloadCollider();
        };
    }
}

#endif