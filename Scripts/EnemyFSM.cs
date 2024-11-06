using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
   
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState m_State;

    public float fineDistance = 18f;
    public float attackDistance = 3f;
    public float attackDelay = 2f;
    public float moveSpeed = 10f;
    public float currentTime;

    public int attackPower = 5;

    Vector3 originPos;
    Quaternion originRot;

    GameObject avata;
    Vector3 avataOriginPos;
    Quaternion avataOriginRot;

    public float moveDistance = 20f;

    CharacterController cc;

    Transform player;

    float hp = 40f;
    float maxHp = 40f;

     Vector3 avataOPos;
     Quaternion avataORot;


    public Slider hpSlider;

    Animator anim;

    NavMeshAgent smith;

    PlayerFire PF;
    PlayerMove PM;

    int WeakC = 0;

    int rea = 0;

    void Start()
    {
        moveSpeed = Random.Range(3.5f, 10.5f);

        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        avata = transform.Find("Zombie1").gameObject;

        cc = GetComponent<CharacterController>();

        currentTime = attackDelay;

        originPos = transform.position;
        originRot = transform.rotation;

        avataOriginPos = avata.transform.position;
        avataOriginRot = avata.transform.rotation;

        anim = transform.GetComponentInChildren<Animator>();

        smith = GetComponent<NavMeshAgent>();
        int rx = Random.Range(-45, 45);
        int rz = Random.Range(-45, 45);
        while (rx < 20 && rx > -20 && rz < 20 && rz > -20)
        {
            rx = Random.Range(-45, 45);
            rz = Random.Range(-45, 45);
        }
        transform.position = new Vector3(rx, 1, rz);

        PF = GameObject.Find("Player").GetComponent<PlayerFire>();
        PM = GameObject.Find("Player").GetComponent<PlayerMove>();

    }

   
    void Update()
    {
        if (m_State != EnemyState.Damaged)
        {

            avataOPos = transform.position;
            avataORot = transform.rotation;
            avataOPos.y -= 1.05f;
        }

        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            
            case EnemyState.Move:
                Move();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Return:
                Return ();
                break;

            case EnemyState.Damaged:
                //Damaged();
                break;

            case EnemyState.Die:
                //Die();
                break;
        }


        /*
        if(m_State != EnemyState.Die)
        {
            if (PF.weakUp == 1)
            {
                if (hp < 20) hp += 2.5f / 60f;

                else hp += 1.25f / 60f;
            }
            else
            {
                if (hp < 20) hp += 5f / 60f;

                else hp += 2.5f / 60f;
            }
        }

        float positionx = transform.position.x;
        float positionz = transform.position.z;

        if (positionx <= PM.safezone && positionx >= -PM.safezone && positionz <= PM.safezone && positionz >= -PM.safezone)
        {
            if(PM.safezone == 7.5f) hp -= 6f / 60f;
            else hp -= 4f / 60f;
        }*/

        if (Input.GetKeyDown(KeyCode.Z))
        {
            HitEnemy(20);
        }


            if (hp > maxHp) hp = maxHp;

        hpSlider.value = (float)hp / (float)maxHp;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if(attackPower == 0)
            {
                attackPower = 5;
            }

            else
            {
                attackPower = 0;
            }
            
        }

        

    }

    void Idle()
    {
        if(Vector3.Distance(transform.position, player.position) < fineDistance)
        {
            m_State = EnemyState.Move;
            //print("상태 전환: Idle -> Move");

            rea = 1;

            anim.SetTrigger("IdleToMove");
        }

        
    }

    void Move()
    {
        
         //if(Vector3.Distance(transform.position, originPos) > moveDistance)
         //{
             /*
             m_State = EnemyState.Return;
             print("상태 전환: Move -> Return");


            hp += 4f / 60f;

             if (hp > maxHp) hp = maxHp;
             */

           // smith.destination = originPos;

            //smith.stoppingDistance = 0;
        //}
        

       if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            /*
            Vector3 dir = (player.position - transform.position).normalized;

            cc.Move(dir * moveSpeed * Time.deltaTime);

            transform.forward = dir;
            */

            smith.isStopped = true;

            smith.ResetPath();

            smith.stoppingDistance = attackDistance;

            smith.destination = player.position;

           smith.speed = moveSpeed;
        }
       

        else
        {
            smith.isStopped = true;

            smith.ResetPath();

            m_State = EnemyState.Attack;
            //print("상태 전환: Move -> Attack");

            //currentTime = attackDelay;

            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;

            if(currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                //print("공격");
                currentTime = 0;

                anim.SetTrigger("StartAttack");
            }
        }

        else
        {
            m_State = EnemyState.Move;
            //print("상태 전환: Attack -> Move");
            currentTime = attackDelay ;

            anim.SetTrigger("AttackToMove");
        }
    }

    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            transform.forward = dir;
        }

        else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State = EnemyState.Idle;
            //print("상태 전환:  Return -> Idle");

            anim.SetTrigger("MoveToIdle");
        }
    }

    public void HitEnemy(float hitPower)
    {

        
        if (hitPower == 1f) rea = 1;
        /*
        if (Vector3.Distance(transform.position, player.position) > 30f && PF.zm == 0)
        {
            hitPower = hitPower / 2;
        }
        else if(Vector3.Distance(transform.position, player.position) > 30f && PF.zm == 1)
        {
            hitPower = hitPower / 1.5f;
        }*/


        
        if (m_State == EnemyState.Die || m_State == EnemyState.Return)
        {

        }

        else
        {
            hp -= hitPower;

            smith.isStopped = true;

            smith.ResetPath();

            //print("E HP : " + hp);

            if (hp > 0)
            {


                avata.transform.position = avataOPos;
                avata.transform.rotation = avataORot;

                //avataOriginPos = avata.transform.position;
                // avataOriginRot = avata.transform.rotation;

                m_State = EnemyState.Damaged;
                //print("상태 전환:  Any State -> Damaged");

                anim.SetTrigger("Damaged");
                Damaged();
            }

            else
            {
                m_State = EnemyState.Die;
                //print("상태 전환:  Any State -> Die");

                anim.SetTrigger("Die");

                Die();
            }


        }
    }
        
    

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }
    
    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);

        if(PF.dieUp == 1)
        {
            PM.hp += 8;
        }

        //print("소멸!");

        Destroy(gameObject);
    }
    
    IEnumerator DamageProcess()
    {
        if (PF.weakUp == 1 && WeakC == 0)
        {
            attackPower = 4;
            moveSpeed = moveSpeed * 0.75f;
            WeakC = 1;
        }
        
        if(rea == 0)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 15f, 1 << 10);

            for (int i = 0; i < cols.Length; i++)
            {
                PF.ReAttack(cols[i]);
            }

            //print(cols.Length - 1);

            rea = 1;
        }
        

        yield return new WaitForSeconds(0.5f);

        //avata.transform.position = avataOriginPos;
        //avata.transform.rotation = avataOriginRot;

        m_State = EnemyState.Move;
        //print("상태 전환: Damaged -> Move");
    }

}
