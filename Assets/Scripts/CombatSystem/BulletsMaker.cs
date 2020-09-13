using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class BulletsMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire(int amoutOfShoots, Vector2 lookDirection, GameObject projectile, GameCharacter shooter, int waitingTimeBetwennShots)
    {
        StartCoroutine(FireShots(amoutOfShoots, lookDirection, projectile, shooter, waitingTimeBetwennShots));
    }
    
    IEnumerator FireShots(int amoutOfShoots, Vector2 lookDirection, GameObject projectile, GameCharacter shooter, int waitingTimeBetwennShots)
    {
        for(int i = 0; i < amoutOfShoots; i++)
        {
            Rigidbody2D projectileRb = Instantiate(projectile, shooter.transform.position, projectile.transform.rotation).GetComponent<Rigidbody2D>();
            projectileRb.AddForce(lookDirection, ForceMode2D.Impulse);
            yield return new WaitForSeconds(waitingTimeBetwennShots);
        }
    }
}
}
}