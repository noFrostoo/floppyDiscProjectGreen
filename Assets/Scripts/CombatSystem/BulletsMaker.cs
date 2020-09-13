using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class BulletsMaker : MonoBehaviour
{
    [SerializeField] public float forceStrengh = 0.2f;
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
            GameObject bullet =  Instantiate(projectile, shooter.transform.position + new Vector3(1, 0, 0), projectile.transform.rotation);
            Rigidbody2D projectileRb = bullet.GetComponent<Rigidbody2D>();
            projectileRb.AddForce(lookDirection.normalized*forceStrengh, ForceMode2D.Impulse);
            yield return new WaitForSeconds(waitingTimeBetwennShots);
        }
    }
}
}
}