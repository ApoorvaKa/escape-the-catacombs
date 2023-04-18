using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class GuardController : MonoBehaviour
{
    public enum GuardStates
    {
        Stopped,
        Patrolling,
        FollowingPlayer,
        GoingToPoint,
        SpinAndSearch,
        Distracted
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

    public float distractionRange = 7.0f;

    public GameObject spriteController;

    private AudioSource audioSource;
    public AudioClip shootingSound;

    public GameObject witchTextCanvas;
    public TextMeshProUGUI displayedDialogue;
    public string dialogue;
    private List<string> standardDialogueList = new List<string>{
        "Keep an eye on the cat!", "Cats are evil!", "Keep that cat locked up!", "Make sure we keep the keys", "Did you forget your supplies somewhere?",
        "Don't underestimate that cat. It's smarter than you think.","I don't trust that cat. It's been plotting something since day one.",
        "Keep your broomstick close. That cat has a knack for sneaking up on you.","I heard that cat can talk. Keep your ears open.",
        "Make sure you check the cells thoroughly. That cat could be hiding anywhere.","If that cat gets away, it'll be on your head.",
        "I'm not taking any chances with that cat. I've got my wand at the ready.","I swear that cat is possessed. It's not natural.",
        "Keep your eyes peeled. That cat might have some tricks up its sleeve."
    };
    
    private List<string> alertDialogueList = new List<string>
    {
        "Get them!", "Quick Shoot", "Grab the Cat!", "The cat is out", "The cat is loose", "Grab them!", "Get them! Don't let that cat escape!",
        "Quick, shoot a spell at it!","Grab the cat before it's too late!","The cat is out! After it!",
        "The cat is loose, we have to catch it before it's too late!","Grab them both, don't let the cat get away!",
        "After that cat, it can't have gone far!","I won't let that cat slip away again!",
        "The cat is quick, but we're quicker!","Get that cat back in its cell!"

    };

    private List<string> distractedDialoguseList = new List<string>
    {
        "Guess a potion fell...", "That Cat must be near!", "I don't want to clean that.", "Guess a potion fell... Wait, where did the cat go?","I smell trouble... And a cat.",
        "This place needs more light. I can barely see the cat in the shadows.",
        "I'm not touching that. It might be cursed. Hey, where did the cat run off to?","Did you hear something? I think the cat might be trying to escape.",
        "I'm taking a break. Keep an eye on the cat for me, will you?","I need a snack. But first, where did that darn cat go?",
        "I don't want to clean that. I'd rather chase the cat around for a bit.","I'm bored. Let's play a game of cat and witch.",
        "This place could use a bit of decoration. Oh, and where's the cat?"
    };
    bool alertStart;
    
    Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        InvokeRepeating("StandardDialogueSelection", 5.0f, 5.0f);
        GameManager.gm.distracts.Add(this);
        displayedDialogue.text = "";
        alertStart = true;
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

    public void GoToDistraction(Transform pos)
    {
        if(state != GuardStates.FollowingPlayer)
        {
            Debug.Log("Going to potion");
            target = pos;
            float dist = Vector2.Distance(this.transform.position, target.position);
            if (dist < distractionRange)
            {
                destSetter.target = target;
                state = GuardStates.Distracted;
            }
        }

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
        displayedDialogue.text = dialogue;
        witchTextCanvas.gameObject.transform.position = transform.position;
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
                _animator.SetTrigger("isAlert");
                AlertDialogueSelection();
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
                    _animator.ResetTrigger("isAlert");
                    target = null;
                    stopTimeLeft = 3f;
                    sight.intensity = .8f;
                    state = GuardStates.Stopped;
                    alertStart = true;
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
            case GuardStates.Distracted:
                path.maxSpeed = patrolSpeed;
                if(Vector2.Distance(transform.position, target.position) < 1f)
                {
                    stopTimeLeft = 1.5f;
                    state = GuardStates.Stopped;
                    DistractedDialogueSelection();
                }
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

    private void AlertDialogueSelection(){
        if (alertStart)
        {
            dialogue = alertDialogueList[Random.Range(0, alertDialogueList.Count - 1)];
            Debug.Log(dialogue);
            StartCoroutine(clearDialogue());
            alertStart = false;
        }
    }

    private void StandardDialogueSelection(){
        if (GuardStates.FollowingPlayer != state && GuardStates.Distracted != state){
            Debug.Log("StandardDialogueSelection");
            if(Random.Range(0, 5) == 0){
                dialogue = standardDialogueList[Random.Range(0, standardDialogueList.Count - 1)];
                Debug.Log(dialogue);
                StartCoroutine(clearDialogue());
            }
        }
    }

    private void DistractedDialogueSelection()
    {
        dialogue = distractedDialoguseList[Random.Range(0, distractedDialoguseList.Count - 1)];
        Debug.Log(dialogue);
        StartCoroutine(clearDialogue());
    }

    IEnumerator clearDialogue()
    {
        yield return new WaitForSeconds(3f);
        dialogue = "";
    }
}
