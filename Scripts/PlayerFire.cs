using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{

    public GameObject firePosition;

    public GameObject bombFactory;

    public GameObject bulletEffect;

    ParticleSystem ps;

    public float throwPower = 15f;

    float timer = 0;
    float mtimer = 8f;

    float btimer = 150f;

    float weaponPower = 1.5f;

    public int MN = 0;

    float RTimer = 2399f;
    int RUp = 1;

    Animator anim;

    public Text wModeText;

    public int zm = 0;

    public int bm = 0;

    public float attackUp = 1;
    public int hpUp = 0;
    public float speedUp = 1;
    public float jumpUp = 1;
    public float stealUp = 0;
    public float fireUp = 1;
    public int weakUp = 0;
    public int dieUp = 0;
    public int critical = 1;
    public int Crandom = 5;
    public float Cdamage = 1.25f;
    float SAtaackT = 0f;
    public int SafeZone = 0;
    public int kill = 0;
    public int killUp = 0;
    public float killAttackUp = 1;
    public float killHPUp = 1;
    public float killSpeedUp = 1;
   

    PlayerRotate PR;
    CamRotate CR;
    PlayerMove MS;
    EnemyFSM EF;

    enum WeaponMode
    {
        Normal,
        Sniper,
        Machine,
        Bomber
    }

    public enum SpecialMode
    {
        None,
        Attck,
        HP,
        Speed,
        Steal,
        Weak,
        Fire,
        Die,
        Critical,
        SAttack,
        RUp,
        SafeZone,
        KillUp
    }

    WeaponMode wMode;

    SpecialMode sMode;

    bool ZoomMode = false;

    public GameObject[] eff_Flash;

    public GameObject CriEffect;


    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();

        PR = GameObject.Find("Player").GetComponent<PlayerRotate>();
        CR = GameObject.Find("Main Camera").GetComponent<CamRotate>();
        MS = GameObject.Find("Player").GetComponent<PlayerMove>();
        EF = GameObject.Find("Player").GetComponent<EnemyFSM>();

        wMode = WeaponMode.Normal;

        sMode = SpecialMode.None;

        MS.moveSpeed = 7.5f * speedUp * killSpeedUp;
    }

   
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        if (Input.GetKey(KeyCode.E))
        {
            return;
        }

            if (Input.GetMouseButtonDown(1))
            {
                switch (wMode)
                {
                    case WeaponMode.Sniper:

                        if (!ZoomMode)
                        {
                            Camera.main.fieldOfView = 8f;
                            ZoomMode = true;                            
                            PR.rotSpeed = 20f;
                            CR.rotSpeed = 20f;
                            MS.moveSpeed = 2.5f * speedUp * killSpeedUp;
                            MS.jumpPower = 4f * jumpUp * (killSpeedUp );
                            zm = 1;
                            weaponPower = 35f + killAttackUp;
                        
                        }

                        else
                        {
                            Camera.main.fieldOfView = 70f;
                            ZoomMode = false;
                            PR.rotSpeed = 200f;
                            CR.rotSpeed = 200f;
                            MS.moveSpeed = 7f * speedUp * killSpeedUp;
                            MS.jumpPower = 7f * jumpUp * (killSpeedUp );
                            zm = 0;
                            weaponPower = 15f + killAttackUp;
                    }

                        break;

                    case WeaponMode.Normal:

                    if (btimer >= 180f)
                    {
                        GameObject bomb = Instantiate(bombFactory) as GameObject;

                        bomb.transform.position = firePosition.transform.position;

                        Rigidbody rb = bomb.GetComponent<Rigidbody>();

                        rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                        btimer = 0;
                        MS.hp += 15 * stealUp;

                    }

                        break;

                }
           

            }

        btimer++;
        if (timer >= mtimer && Input.GetMouseButton(0))
        {
            if (anim.GetFloat("MoveMotion") == 0) anim.SetTrigger("Attack");
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    float hitY = hitInfo.point.y;

                    if (hitY >= 1.66f)
                    {
                        eFSM.HitEnemy(weaponPower * killAttackUp * attackUp * Cdamage);
                        StartCoroutine(PlayCriEffect());
                    }
                    else
                    {
                        eFSM.HitEnemy(weaponPower * killAttackUp * attackUp);
                    }

                    if (sMode == SpecialMode.SAttack && SAtaackT >= 90f)
                    {
                        Collider[] cols = Physics.OverlapSphere(hitInfo.transform.position, 5.5f, 1 << 10);
                        for (int i = 1; i < cols.Length; i++)
                        {
                            cols[i].GetComponent<EnemyFSM>().HitEnemy((weaponPower * attackUp + killAttackUp) / 2.5f);
                        }
                        SAtaackT = 0f;
                    }
                    MS.hp += weaponPower * stealUp;
                }

                bulletEffect.transform.position = hitInfo.point;
                bulletEffect.transform.forward = hitInfo.normal;
                ps.Play();
            }
            timer = 0f;
            StartCoroutine(ShootEffectOn(0.05f));
        }
        else timer++;

        SAtaackT++;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (RTimer >= 3000f / RUp)
            {
                print("Use R");
                for (int i = 0; i <= 5; i++)
                {
                    GameObject bomb = Instantiate(bombFactory) as GameObject;

                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                }
                RTimer = 0f;
            }
            else
            {
                RTimer += 59f;
                print((int)((3000 / RUp - RTimer) / 60) + "  Sec left to use R");
            }
        }

        RTimer++;

        if(RTimer >= 2999f * fireUp && RTimer < 4000f * fireUp)
        {
            print("R Ready");
            RTimer += 1000f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            Camera.main.fieldOfView = 70f;

            wModeText.text = "Normal Mode";

            mtimer = 8f * fireUp;
            weaponPower = 1.5f;
            btimer = 160f * fireUp;
            MS.moveSpeed = 7f * speedUp * killSpeedUp;
            MN = 0;
            PR.rotSpeed = 200f;
            CR.rotSpeed = 200f;
            MS.jumpPower = 7f * jumpUp * (killSpeedUp );
            bm = 0;
        }
        
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            wMode = WeaponMode.Bomber;

            Camera.main.fieldOfView = 60f;

            wModeText.text = "Bomber Mode";

            mtimer = 80f * fireUp;
            weaponPower = 1.5f;
            btimer = 80000f;
            MS.moveSpeed = 4.5f * speedUp * killSpeedUp;
            MN = 0;
            PR.rotSpeed = 150f;
            CR.rotSpeed = 150f;
            MS.jumpPower = 5.5f * jumpUp * (killSpeedUp );
            bm = 1;
        }
        
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            wMode = WeaponMode.Machine;

            Camera.main.fieldOfView = 55f;

            wModeText.text = "Machine Gun Mode";

            mtimer = 0.8f * fireUp;
            weaponPower = 0.6f;
            btimer = -9999999f;
            MS.moveSpeed = 4.5f * speedUp * killSpeedUp;
            MN = 1;
            PR.rotSpeed = 100f;
            CR.rotSpeed = 100f;
            MS.jumpPower = 5.5f * jumpUp * (killSpeedUp );
            bm = 0;
        }

        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera.main.fieldOfView = 70f;
            wMode = WeaponMode.Sniper;
            mtimer = 100f * fireUp;
            timer = 80f;
            weaponPower = 11.5f;
            wModeText.text = "Sniper Mode";
            MS.moveSpeed = (7f * speedUp * killSpeedUp);
            MN = 0;
            PR.rotSpeed = 200f;
            CR.rotSpeed = 200f;
            MS.jumpPower = (7f * jumpUp * (killSpeedUp));
            bm = 0;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 10000, 1 << 10);

            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].GetComponent<EnemyFSM>().HitEnemy(1f);
            }
        }

        if (sMode == SpecialMode.None)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                sMode = SpecialMode.Attck;
                attackUp = 1.25f;
                print("Attack Power");
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                sMode = SpecialMode.HP;
                hpUp = 10;
                MS.hp += hpUp;
                print("HP/Heal");
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                sMode = SpecialMode.Speed;
                speedUp = 1.35f;
                jumpUp = 1.25f;
                print("Speed/Jump");
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                sMode = SpecialMode.Steal;
                stealUp = 0.2f;
                print("HP Absorb");
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                sMode = SpecialMode.Weak;
                weakUp = 1;
                print("Weakeness");
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                sMode = SpecialMode.Fire;
                fireUp = 0.8f;
                print("Fire Speed");
            }
            else if (Input.GetKeyDown(KeyCode.F7))
            {
                sMode = SpecialMode.Die;
                dieUp = 1;
                print("Kill Reward");
            }
            else if (Input.GetKeyDown(KeyCode.F8))
            {
                sMode = SpecialMode.Critical;
                critical = 2;
                Cdamage = 2f;
                print("Critical Up");
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                sMode = SpecialMode.SAttack;
                SAtaackT = 0f;
                print("Super Attack");
            }
            else if (Input.GetKeyDown(KeyCode.F10))
            {
                sMode = SpecialMode.RUp;
                RUp = 2;
                print("Ultimate Up");
            }
            else if (Input.GetKeyDown(KeyCode.F11))
            {
                sMode = SpecialMode.SafeZone;
                SafeZone = 1;
                print("SafeZone UP");
            }
            else if (Input.GetKeyDown(KeyCode.F12))
            {
                sMode = SpecialMode.KillUp;
                killUp = 1;
                print("Kill Upgrade");
            }
        }
        if(killUp == 1)
        {
            killAttackUp = 1 + (kill * 2f) / 100;
            killHPUp = 1+ (kill * 3f) / 100;
            killSpeedUp = 1 + (kill * 2.5f) / 100;
        }
    }

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0, eff_Flash.Length);

        eff_Flash[num].SetActive(true);

        yield return new WaitForSeconds(duration);

        eff_Flash[num].SetActive(false);
    }

    IEnumerator PlayCriEffect()
    {
        CriEffect.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        CriEffect.SetActive(false);
    }

    public void ReAttack(Collider Enemy)
    {
        Enemy.GetComponent<EnemyFSM>().HitEnemy(1f);
    }
}
