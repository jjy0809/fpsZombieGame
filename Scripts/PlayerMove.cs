using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 7f;

    CharacterController cc;

    public float gravity = -20f;

    float yVelocity = 0;

    public float jumpPower = 7f;

    public bool isJump = false;

    public float hp = 20f;

    int dJump = 0;

    float maxHP = 20;

    public Slider hpSlider;

    public GameObject hitEffect;

    PlayerFire PF;

    Animator anim;

    public float safezone = 5f;

    public GameObject UpgradeSafeZone;

    int khpu = 0;
    int lk = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        UpgradeSafeZone.SetActive(false);
        PF = GameObject.Find("Player").GetComponent<PlayerFire>();
    }

    
    void Update()
    {
        if(GameManager.gm.gState != GameManager.GameState.Run){
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        anim.SetFloat("MoveMotion", dir.magnitude);

        dir = Camera.main.transform.TransformDirection(dir);
        /*
        if (transform.position.x < -50) transform.position = new Vector3(-50 , transform.position.y, transform.position.z);
        else if (transform.position.x > 50) transform.position = new Vector3(50 , transform.position.y, transform.position.z);
        else if (transform.position.z < -50) transform.position = new Vector3(transform.position.x , transform.position.y, -50);
        else if (transform.position.z > 50) transform.position = new Vector3(transform.position.x , transform.position.y, 50);
        */
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJump)
            {
                isJump = false;
                dJump = 0;
            }
            yVelocity = 0;
        }

        if (Input.GetButtonDown("Jump") && dJump == 1 && isJump == true)
        {
            yVelocity = jumpPower;
            isJump = true;
            dJump = 0;
        }
        if (Input.GetButtonDown("Jump") && !isJump){
            yVelocity = jumpPower;
            isJump = true;
            dJump = 1;
        }        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 200f * PF.killSpeedUp;
            dJump = 1;
        }

        else if(PF.zm == 1)
        {
            moveSpeed = 2.5f * PF.killSpeedUp;
        }
        else if(PF.MN == 1)
        {
            moveSpeed = 5f * PF.killSpeedUp;
        }
        else if(PF.speedUp != 1.2f)
        {
            moveSpeed = 7f * PF.killSpeedUp;
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            hp += 10;
        }

        if(PF.hpUp == 10)
        {
            hp += 0.5f / 60f;
            maxHP = 20 + PF.hpUp;
            hpSlider.value = (float)hp / (float)maxHP;
        }

        if (PF.SafeZone == 1)
        {
            safezone = 7.5f;
            UpgradeSafeZone.SetActive(true);
        }

        float positionx = transform.position.x;
        float positionz = transform.position.z;

        if (positionx <= safezone && positionx >= -safezone && positionz <= safezone && positionz >= -safezone)
        {
            if(safezone == 7.5f) hp += 1.5f / 60f;
            else hp += 1f / 60f;
        }

        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        if (Input.GetKey(KeyCode.E))
        {
            dir.x = dir.x * -1;
            dir.z = dir.z * -1;
        }

        cc.Move(dir * moveSpeed * Time.deltaTime);

        if (PF.kill > lk) khpu = 0;

        if(khpu == 0 && PF.killHPUp != 0)
        {
            for (int i = 0; i < PF.kill - lk; i++)
            {
                hp = hp * 1.03f;
                maxHP = maxHP * 1.03f;
                khpu = 1;
            }
            lk = PF.kill;
        }

        if (hp > maxHP) hp = maxHP;

        hpSlider.value = (float)hp / (float)maxHP;

        if (jumpPower > 8.5f) jumpPower = 8.5f;

        
    }

    public void DamageAction(int damage)
    {
        hp -= damage;

        if(hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }

    IEnumerator PlayHitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitEffect.SetActive(false);
    }

}
