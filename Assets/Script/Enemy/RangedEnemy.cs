using UnityEngine;
using System.Collections;

public class RangedEnemy : Enemy
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] protected Transform firePos;
    [SerializeField] protected float attackDelay = .5f;
    public override void ChangeState()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);

        // Run away if health is low
        if (health.GetHealth() < health.maxHealth / 4)
        {
            currentState = States.runAway;
            return;
        }

        // Stand and shoot if close and visible
        if (distance < InRangeDistance && PlayerIsVisable())
        {
            currentState = States.standAndAttack;
            return;
        }

        // Follow player if within spotting distance and visible, and currently patrolling
        if (distance <= SpottingDistance && PlayerIsVisable() && currentState == States.patrolling)
        {
            currentState = States.followPlayer;
            return;
        }

        // Go back to patrolling if too far or no longer visible
        if ((distance > SpottingDistance || !PlayerIsVisable()) &&
           (currentState == States.followPlayer || currentState == States.standAndAttack))
        {
            currentState = States.patrolling;
            MoveToNextPoint();
            return;
        }
    }
    public override void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            StartCoroutine(ShootWithDelay());
            attackTimer = 0f;
        }
    }
    IEnumerator ShootWithDelay()
    {
        float randomDelay = Random.Range(attackDelay - 0.3f, attackDelay + 0.3f);
        yield return new WaitForSeconds(randomDelay);

        GameObject prefab = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        prefab.GetComponent<BulletScript>().damageAmount = damageAmount;
    }
}
