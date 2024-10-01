

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{
    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private float BulletSpeed = 100;
    [SerializeField]
    Camera cam;
    [SerializeField]
    EnemyScript enemyScript;
    [SerializeField] 
    public bool IsShotgun;
    
    private Animator animator;
    private float LastShootTime;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        cam = Camera.main;
    }

    

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            animator.SetBool("isShooting", true);
            ShootingSystem.Play();
            Vector3 direction = GetDirection();

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit) && IsShotgun)
            {
                
            }

            if (Physics.Raycast(ray,out hit))
            {

                 if(hit.transform.TryGetComponent<EnemyScript> (out var enemyScript))
                {
                    enemyScript.TakeDamage(10);
                }

                Physics.Raycast(BulletSpawnPoint.position, direction,out hit, float.MaxValue);

                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                LastShootTime = Time.time;

               
            }
            
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + GetDirection() * 100, Vector3.zero, false));

                LastShootTime = Time.time;
            }

            Debug.DrawRay(BulletSpawnPoint.position,direction);
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = cam.transform.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
       
        Vector3 startPosition = BulletSpawnPoint.transform.position;
        
        float distance = Vector3.Distance(BulletSpawnPoint.transform.position, HitPoint);
        float remainingDistance = distance;

        while (distance > 0)
        {
            BulletSpawnPoint.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            distance -= BulletSpeed * Time.deltaTime;

            yield return null;

            Debug.Log(distance);
        }
        
        animator.Play("Shooting", 0, 0);
        animator.SetBool("isShooting", false);
        BulletSpawnPoint.transform.position = HitPoint;



        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));

        }

        Destroy(Trail.gameObject, Trail.time);
    }
}