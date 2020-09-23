using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class Bullet : MonoBehaviour
{
    static int damage;
    static int baseChanceOfHit;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            int hitNumber = Random.Range(0, 101);
            if( hitNumber < baseChanceOfHit)
                other.gameObject.GetComponent<GameCharacter>().TakeDamage(damage);
        }
        else if(other.gameObject.CompareTag("Wall"))
        {
            baseChanceOfHit -= other.gameObject.GetComponent<Wall>().ChancePointsDecrease;
            damage -= other.gameObject.GetComponent<Wall>().DamageDecrase;
        }
    }

    public void SetInfomationForBullet(int damageG, int baseChanceOfHitG)
    {
        damage = damageG;
        baseChanceOfHit = baseChanceOfHitG;
    }
}
}
}
