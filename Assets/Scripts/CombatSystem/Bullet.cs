using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class Bullet : MonoBehaviour
{
    int damage;
    int baseChanceOfHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        }
    }

    public void SetInfomationForBullet(int damage, int baseChanceOfHit)
    {
        this.damage = damage;
        this.baseChanceOfHit = baseChanceOfHit;
    }
}
}
}
