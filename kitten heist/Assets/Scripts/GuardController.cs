using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GuardController : MonoBehaviour
{
    public enum GuardStates
    {
        Stopped,
        Patrolling,
        FollowingPlayer,
        GoingToPoint,
        SpinAndSearch
    }

    public GuardStates state = GuardStates.Stopped;

    public AIDestinationSetter destSetter;
    public AIPath path;
    public Transform[] patrolPoints;
    public float angle;
    public bool goToAngle = false;
    public int index = 0;
    public Transform target;

    public Transform[] rayPoints;
    public LineRenderer[] lines;
    public float dist = 4f;
    public LayerMask rayLayer;
    public bool debugLines;

    public float patrolSpeed = 2f, alertSpeed = 4f;
    public float turnRate = 5;
    public float AlertTimer = 5f;
    public float AlertTimeLeft = 0;

    public float stopTimeLeft = 0f;
    public float shootCD = 1f;
    private bool shootingDone = true;
    public Light2D sight;
    public GameObject projectile;
    public Transform shootPoint;
    public bool lightOn = true;
    public ToggleLights tLight;

    public GameObject spriteController;

    private AudioSource audioSource;
    public AudioClip shootingSound;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void TurnOffLight()
    {
        sight.enabled = false;
    }
    public void TurnOnLight()
    {
        sight.enabled = true;
    }

    public void GoToFixLight(Transform pos, ToggleLights t)
    {
        tLight = t;
        target = pos;
        destSetter.target = target;
        state = GuardStates.GoingToPoint;
    }

    public void CastRays(float multiplier = 1f)
    {

        RaycastHit2D hit;
        for (int i = 0; i < rayPoints.Length; i++)
        {
            if (debugLines)
                lines[i].SetPosition(0, rayPoints[i].position);
            hit = Physics2D.Raycast(rayPoints[i].position, rayPoints[i].up, dist*multiplier, rayLayer);
            
            if (hit)
            {
                //Vector2 localHit = transform.InverseTransformPoint(hit.point);
                if (debugLines)
                    lines[i].SetPosition(1, hit.point);
                if (hit.transform.gameObject.CompareTag("Player") && !Player.p.isHiding)
                {
                    sight.intensity = 4f;
                    target = hit.transform;
                    destSetter.target = target;
                    state = GuardStates.FollowingPlayer;
                    AlertTimeLeft = AlertTimer;
                }
            } else
            {
                if (debugLines)
                {
                    Vector2 end = rayPoints[i].position + rayPoints[i].up * dist * multiplier;
                    lines[i].SetPosition(1, end);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (state)
        {
            case GuardStates.Stopped:
                if (goToAngle)
                {
                    transform.localEulerAngles = new Vector3(0, 0, angle);
                }
                stopTimeLeft -= Time.deltaTime;
                path.maxSpeed = 0f;
                if(lightOn)
                    CastRays();
                if (stopTimeLeft <= 0)
                {
                    destSetter.target = patrolPoints[index];
                    state = GuardStates.Patrolling;
                }
                break;
            case GuardStates.Patrolling:
                path.endReachedDistance = 0;
                path.maxSpeed = patrolSpeed;
                if (Vector2.Distance(transform.position, patrolPoints[index].position) < .1f)
                {
                    index = (index + 1) % patrolPoints.Length;
                    stopTimeLeft = 1f;
                    state = GuardStates.Stopped;
                }
                if (lightOn)
                    CastRays();
                break;
            case GuardStates.FollowingPlayer:
                
                path.maxSpeed = alertSpeed;
                if (lightOn)
                    CastRays(2);
                path.endReachedDistance = 2f;
                StartCoroutine(Shoot(shootCD));
                AlertTimeLeft -= Time.deltaTime;
                if (path.remainingDistance <= dist*.9f && !Player.p.isHiding)
                {
                    transform.up = Vector3.Lerp(transform.up, (target.position - transform.position), turnRate * Time.deltaTime);
                }
                if (AlertTimeLeft <= 0)
                {
                    target = null;
                    stopTimeLeft = 3f;
                    sight.intensity = .8f;
                    state = GuardStates.Stopped;
                }
                
                break;
            case GuardStates.GoingToPoint:
                path.maxSpeed = patrolSpeed;
                if (Vector2.Distance(transform.position, target.position) < 1f)
                {
                    tLight.Toggle();
                    stopTimeLeft = 1f;
                    state = GuardStates.Stopped;
                    
                }
                break;
            case GuardStates.SpinAndSearch:
                
                break;
            default:
                break;
        }
        spriteController.transform.localEulerAngles = new Vector3(0, 0, -transform.localEulerAngles.z);
        Animator a = spriteController.GetComponent<Animator>();
        a.SetFloat("Speed", path.velocity.magnitude);
        a.SetFloat("Horizontal", path.velocity.x);
    }

    IEnumerator Shoot(float cd)
    {
        if (shootingDone)
        {
            audioSource.PlayOneShot(shootingSound);
            shootingDone = false;
            yield return new WaitForSeconds(cd);
            if(state == GuardStates.FollowingPlayer && !Player.p.isHiding)
            {
                var g = Instantiate(projectile, shootPoint.position, transform.rotation);
                g.GetComponent<Projectile>().Move();
            }
            shootingDone = true;
        }
    }


}
