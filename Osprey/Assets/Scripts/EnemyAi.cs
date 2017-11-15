using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct MovementAction
{
    private float startTime;
    public float duration;
    public EaseMode easeMode;
    public float startDistance;
    public float endDistance;
    private bool endflag;

    public void init()
    {
        endflag = false;
        startTime = Time.time;
    }

    public void update(EnemyAi enemy)
    {
        float d = startDistance + Easing.Ease(easeMode, (Time.time - startTime) / duration) * (endDistance - startDistance);
        if ((Time.time - startTime) / duration < 1f && !endflag)
        {
            enemy.transform.position = enemy.path.GetDistancePoint(d);
        }
        else
        {
            endflag = true;
        }
    }

    public bool finished()
    {
        return endflag;
    }
}

public class EnemyAi : Entity {
    public float speed = 1.0f;
    public Entity[] shot;
    public GameObject[] gunBarrel;
    public float shotFireRate = 25.0f;
    protected float lastShot;
    private float health;
    public float maxHealth;
    private bool hit;
    public float flashTime;
    private float hitTimer;
    public Color[] colors;
    public MovementAction[] moveList;
    private int moveIndex;
    //public StateMachine sm = new StateMachine();


    public BezierSpline path;




    public GameObject playerTarget;
    void Start () {
        thisTransform = GetComponent<Transform>();
        thisRenderer.sharedMaterial = materials[(int)polarity];
        hitTimer = 0;
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        //startTime = Time.time;
    }

    private void OnEnable()
    {
        thisRenderer.sharedMaterial = materials[(int)polarity];
        hitTimer = 0;
        moveIndex = 0;
    }

    void Update () {

        if(moveIndex < moveList.Length)
        {
            moveList[moveIndex].update(this);
            if(moveList[moveIndex].finished() && moveIndex < moveList.Length -1)
            {
                moveIndex++;
                moveList[moveIndex].init();
            }
        }    
        //}
        //foreach(GameObject barrel in gunBarrel)
        //{
        //    if (Time.time - lastShot > 1 / shotFireRate)
        //    {
        //        if (playerTarget.activeSelf)
        //        {
        //            Vector3 diff = playerTarget.transform.position - barrel.transform.position;
        //            float ang = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //            barrel.transform.rotation = Quaternion.AngleAxis(ang -90, Vector3.forward);
        //        }

        //        Instantiate(shot[(int)polarity], barrel.transform.position, barrel.transform.rotation);
        //        lastShot = Time.time;
        //    }

        //}
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
        }
    }
}
