using UnityEngine;

public abstract class ThrowAble : MonoBehaviour
{
    protected float radius = 5f;
    protected float duration = 5f;
    protected float elapsedTime = 0f;
    [SerializeField] protected GameObject explosionPrefab;

    protected virtual void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > duration)
        {
            Explode();
            Destroy(gameObject);
        }
    }

     protected virtual void Explode()
    {
        //code for explosion
    }

    protected void OnDrawGizmosSelected()
    {
        //draw explosion radius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
