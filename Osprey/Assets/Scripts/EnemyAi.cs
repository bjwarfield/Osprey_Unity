using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementAction
{
    public enum DirectionMode
    {
        FACE_PATH,
        FACE_TARGET,
        FACE_DIRECTION
    }
    private float startTime;
    public float duration;
    public EaseMode easeMode;
    public DirectionMode directionMode;
    public float lookDirectionStart;
    public float lookDirectionEnd;

    public Transform lookTarget;
    private Vector3 lookVector;
    public float startDistance;
    public float endDistance;
    private bool endflag;

    public void Init()
    {
        endflag = false;
        startTime = Time.time;
        
        if (directionMode == DirectionMode.FACE_TARGET)
        {
            lookTarget = GameObject.FindGameObjectWithTag("Player").transform;
            if (lookTarget)
            lookVector = lookTarget.position;
        }

    }

    public void Execute(EnemyAi enemy)
    {
        float t = Easing.Ease(easeMode, (Time.time - startTime) / duration);
        float d = startDistance + t * (endDistance - startDistance);
        if ((Time.time - startTime) / duration < 1f && !endflag)
        {
            enemy.transform.position = enemy.path.GetDistancePoint(d);
            if(directionMode == DirectionMode.FACE_PATH)
            {
                
                enemy.transform.LookAt(enemy.path.GetDistancePoint(d + 0.001f), Vector3.back);
            }else if(directionMode == DirectionMode.FACE_TARGET)
            {
                lookTarget = GameObject.FindGameObjectWithTag("Player").transform;

                if (lookTarget) { 
                lookVector = lookTarget.position;
                }
                else
                {
                    Quaternion.RotateTowards(
                        enemy.transform.rotation,
                        Quaternion.LookRotation(GetVectorInDirection(enemy.transform.position, lookDirectionEnd)),
                        2f);

                }
                enemy.transform.LookAt(lookVector, Vector3.back);
            }else if(directionMode == DirectionMode.FACE_DIRECTION)
            {
                float ang = Mathf.Lerp(lookDirectionStart, lookDirectionEnd, t);
                lookVector = GetVectorInDirection(enemy.transform.position, ang);
                enemy.transform.LookAt(lookVector, Vector3.back);
            }
        }
        else
        {
            endflag = true;
        }
    }

    private Vector3 GetVectorInDirection(Vector3 origin, float angle)
    {
        Vector3 dir = origin;
        angle *= Mathf.Deg2Rad;
        dir.y += Mathf.Sin(angle);
        dir.x += Mathf.Cos(angle);
        
        return dir;
    }
    
    public bool Finished()
    {
        return endflag;
    }
}

[System.Serializable]
public struct ShootAction
{
    public enum TargetMode
    {
        ABSOLUTE,
        TARGET_PLAYER,
        RELATIVE,
        SEQUENCE
    }


    private float startTime;
    public float delay;
    public TargetMode targetMode;
    public Transform targetTransform;
    private Vector3 targetVector;
    public float interval;
    private float lastShot;
    public int repeatCount;
    private int repeats;
    public float angle;
    public float angle2;
    public int burstCount;
    public bool randomizeAngle;
    private float currentAngle;
    bool endFlag;


    public void Init()
    {
        endFlag = false;
        startTime = Time.time;
        repeats = 0;
        currentAngle = 0;
        targetVector = Vector3.zero;
        if(angle2 < angle)
        {
            float j = angle2;
            angle2 = angle;
            angle = j;
        }

    }


    public void Execute(EnemyAi enemy)
    {
        if(Time.time - startTime > delay)
        {

            if (Time.time - lastShot > interval)
            {
                for(int i =0; i < burstCount; i++){
                    if (targetMode == TargetMode.SEQUENCE)
                    {
                        currentAngle += randomizeAngle ? Random.Range(angle, angle2) : angle;
                    }
                    else if (targetMode == TargetMode.TARGET_PLAYER)
                    {
                        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
                        if (targetTransform)
                        {
                            targetVector = targetTransform.position;
                        }
                        //currentAngle = Vector3.SignedAngle(enemy.transform.position, targetVector, Vector3.back);
                        Vector3 diff = targetVector - enemy.transform.position;
                        currentAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                        currentAngle += randomizeAngle ? Random.Range(angle, angle2) : angle;

                    }
                    else if (targetMode == TargetMode.ABSOLUTE)
                    {
                        currentAngle = randomizeAngle ? Random.Range(angle, angle2) : angle;
                    }
                    else if (targetMode == TargetMode.RELATIVE)
                    {
                        Vector3 diff = enemy.transform.forward - enemy.transform.position;
                        currentAngle = Mathf.Atan2(diff.y, diff.x);
                        currentAngle += randomizeAngle ? Random.Range(angle, angle2) : angle;

                    }

                    //Debug.Log(currentAngle);
                    Vector3 dir = GetVectorInDirection(Vector3.zero, currentAngle);

                    MonoBehaviour.Instantiate(enemy.shot[(int)enemy.Polarity],
                        enemy.gunBarrel[0].transform.position,
                        Quaternion.LookRotation(dir, Vector3.back));

                }

                lastShot = Time.time;
                repeats++;
            }
        }
        if(repeats >= repeatCount)
        {
            endFlag = true;
        }
    }

    public bool Finished()
    {
        return endFlag;
    }

    private Vector3 GetVectorInDirection(Vector3 origin, float angle)
    {
        Vector3 dir = origin;
        angle *= Mathf.Deg2Rad;
        dir.y += Mathf.Sin(angle);
        dir.x += Mathf.Cos(angle);

        return dir;
    }
}


public class EnemyAi : Entity {
    public float speed = 1.0f;
    public Entity[] shot;
    public GameObject[] gunBarrel;
    //public float shotFireRate = 25.0f;
    protected float lastShot;
    private float health;
    public float maxHealth;
    private bool hit;
    public float flashTime;
    private float hitTimer;
    public Color[] colors;
    [SerializeField]
    public MovementAction[] moveList;
    private int moveIndex;
    [SerializeField]
    public ShootAction[] shootList;
    private int shootIndex;


    public BezierSpline path;




    public GameObject playerTarget;
    void Start () {
        thisTransform = GetComponent<Transform>();
        thisRenderer.sharedMaterial = materials[(int)polarity];
        hitTimer = 0;
        playerTarget = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Init()
    {
        thisRenderer.sharedMaterial = materials[(int)polarity];
        hitTimer = 0;
        moveIndex = 0;
        if(moveList.Length > 0)
        {
            moveList[moveIndex].Init();
        }
        if(shootList.Length > 0)
        {
            shootList[shootIndex].Init();
        }
        health = maxHealth;
    }

    void Update () {



        

        //execute current moveAction
        if(moveIndex < moveList.Length)
        {
            moveList[moveIndex].Execute(this);
            if(moveList[moveIndex].Finished() && moveIndex < moveList.Length -1)
            {
                moveIndex++;
                moveList[moveIndex].Init();
            }
        }   

        //execute current shoot action
        if(shootIndex < shootList.Length)
        {
            shootList[shootIndex].Execute(this);
            if(shootList[shootIndex].Finished())
            {
                if(shootIndex < shootList.Length - 1)
                {
                    shootIndex++;
                }
                
                shootList[shootIndex].Init();
            }

        } 

        //hit dynamics, swap colors when hit
        if(hit)
        {
            if (Time.time < hitTimer)
            {
                thisRenderer.material.SetColor("_SpecColor", colors[1]);
            }else
            {
                hit = false;
                thisRenderer.material.SetColor("_SpecColor", colors[0]);
            }
        }
        
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerBullet") || other.CompareTag("Player"))
        {
            //Debug.Log("Enemy hit!");
            hit = true;
            hitTimer = Time.time + flashTime;

            PlayerLaserBoltControl bolt = other.gameObject.GetComponent<PlayerLaserBoltControl>();
            Entity e = other.gameObject.GetComponent<Entity>();
            if(bolt)
            {
                if(e.Polarity == Polarity)
                {
                    health -= 1;
                }
                else
                {
                    health -= 2;
                }
                
            }else
            {
                if (e.Polarity == Polarity)
                {
                    health -= 10;
                }
                else
                {
                    health -= 20;
                }

            }

            if (health <= 0)
            {
                if (gibs)
                {
                    Instantiate(gibs, transform.position, gibs.transform.rotation);
                }
                if (e.Polarity == Polarity)
                {
                    //Debug.Log("Return Fire");
                    float angle = -90;
                    float randRange = 5;
                    for(int i = 0; i < 10; i++)
                    {
                        Vector3 dir = GetVectorInDirection(Vector3.zero, Random.Range(angle - randRange, angle + randRange));
                        EnemyBulletControl bullet = Instantiate(shot[(int)polarity], transform.position, Quaternion.LookRotation(dir, Vector3.back)) as EnemyBulletControl;

                        bullet.speed *= Random.Range(0.9f, 1.1f);
                    }

                }
                Destroy(gameObject);
            }
        }
    }

    private Vector3 GetVectorInDirection(Vector3 origin, float angle)
    {
        Vector3 dir = origin;
        angle *= Mathf.Deg2Rad;
        dir.y += Mathf.Sin(angle);
        dir.x += Mathf.Cos(angle);

        return dir;
    }
}
