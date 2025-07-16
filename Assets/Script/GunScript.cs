using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] Transform firePos;
    [SerializeField] float attackInterval = .5f;
    [SerializeField] GameObject bulletPrefab;
    float timer = 0;
    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > attackInterval && Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        }
        
    }
}
