

using System.Collections;
using System.Data.Common;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{   

    
    [SerializeField]
    public GameObject pistol;
    [SerializeField]
    public GameObject shotgun;
  
    
  
    [SerializeField]
    private bool AddBulletSpread = true;
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
    private float BulletSpeed = 100f;
    [SerializeField]
    Camera cam;
    [SerializeField]
    EnemyScript enemyScript;
    [SerializeField] 
    public bool isShotgun;
    [SerializeField]
    public float range = 30f;
    [SerializeField]
    public float bullets;  
    [SerializeField]
    public float reloadtime = 2.5f;  
    [SerializeField]
    public float reloadtimer = 0;
    [SerializeField]
    public bool reloading;
    [SerializeField]
    public float magazinesize;
    [SerializeField]
    public float magazine = 1;
    [SerializeField]
    bool shooting = false;
    
    private Animator animator;
    private float LastShootTime;
    int pellets = 8;
    

    public void Update()
    {       
        if (Input.GetMouseButtonDown(0) && pistol.activeInHierarchy && !reloading && magazine != 0)
        {
        Shoot();
        bullets -= 1;
        }
        else if(Input.GetMouseButtonDown(0) && shotgun.activeInHierarchy && !reloading && magazine != 0 && LastShootTime + ShootDelay < Time.time)
        {
        Shoot();
        bullets -= 1;
        shooting = true;
        }
       
       

        if (bullets <= 0 && !reloading && magazine != 0)
        {
         StartCoroutine("Reloading", reloadtime);
        }
        else if (Input.GetKeyDown(KeyCode.R) && !reloading && magazine != 0)
        {
         StartCoroutine("Reloading", reloadtime);
        }
        

        
    }
    public void Awake()
    {
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        cam = Camera.main;
        bullets = magazinesize;
    }
    
    IEnumerator Reloading(float reloadtime)
    {    
        reloading = true;    
        yield return new WaitForSeconds(reloadtime); 
        magazine -= 1; 
        bullets = magazinesize;
        reloading = false;            
    }
    

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time && !reloading && bullets <= magazinesize && bullets != 0 && magazine != 0)
        {
            animator.Play("Shooting");
            animator.SetBool("isShooting", true);
            ShootingSystem.Play();
            Vector3 direction = GetDirection();
            Vector3 spread = Vector3.zero;
            
            
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
           
            //shotgun
             if(isShotgun)
             {
                for(int i = 0;i < pellets; i++)
                {
           if (Physics.Raycast(cam.transform.position + GetDirection(),cam.transform.forward, out hit, range))
        {
                Debug.DrawLine(cam.transform.position  + cam.transform.forward, hit.point, Color.green, 1f);
                ShootingSystem.Play();     
                LastShootTime = Time.time;   
                                         
             }
           else
            {
             Debug.DrawLine(cam.transform.position  + cam.transform.forward + GetDirection(), cam.transform.forward * range, Color.red, 1f);
             ShootingSystem.Play();
             LastShootTime = Time.time;    
                            
        }
                
           if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
             if(hit.transform.TryGetComponent<EnemyScript> (out var enemyScript))
             {
             enemyScript.TakeDamage(5);

             }
            }
                }
             }
                                           
           
            //PISTOL 
            if (Physics.Raycast(ray,out hit) && !isShotgun)
            {
                
                 if(hit.transform.TryGetComponent<EnemyScript> (out var enemyScript))
                {
                    enemyScript.TakeDamage(20);
                }
                Physics.Raycast(BulletSpawnPoint.position, direction,out hit, float.MaxValue);
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
               // StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
                
                Debug.DrawRay(BulletSpawnPoint.position, hit.point, Color.green, 1f);
                LastShootTime = Time.time;   
                           
            }
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

               // StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + GetDirection() * 100, Vector3.zero, false));

                LastShootTime = Time.time;
            }
                 animator.Play("Shooting", 0, 0);
                animator.SetBool("isShooting", false);
                
                
                
            }

            

    }
    
    

    private Vector3 GetDirection()
    {
        Vector3 direction = cam.transform.forward;
        Vector3 spread = Vector3.zero;
        

        if (AddBulletSpread)
        {
            spread += cam.transform.up * Random.Range(-1f, 1f);
            spread += cam.transform.right * Random.Range(-1f, 1f);
            
            direction += spread.normalized * Random.Range(0f, 0.2f);

        }

        return direction;
    }

   // private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
   // {
   //    
   //     Vector3 startPosition = BulletSpawnPoint.transform.position;
   //     
   //     float distance = Vector3.Distance(BulletSpawnPoint.transform.position, HitPoint);
   //     float remainingDistance = distance;
//
   //     while (distance > 0)
   //     {
   //         BulletSpawnPoint.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));
//
   //         distance -= BulletSpeed * Time.deltaTime;
//
   //         yield return null;
//
   //         //Debug.Log(distance);
   //     }
   //     
   //     BulletSpawnPoint.transform.position = HitPoint;
//
//
   //     if (MadeImpact)
   //     {
   //         Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
   //     }
//
   //     Destroy(Trail.gameObject, Trail.time);
   // }
}