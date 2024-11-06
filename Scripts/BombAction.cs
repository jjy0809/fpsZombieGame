using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    float attackPower = 15;

    public float explosionRadius = 7.5f;

    PlayerFire PF;

    //public GameObject CriEffect;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);

        if (PF.bm == 0)
        {
            attackPower = 15;
        }
        else if (PF.bm == 1)
        {
            attackPower = 13;
        }
        if(PF.killAttackUp != 0)
        {
            attackPower *= PF.killAttackUp;
        }

        PF.Crandom = Random.Range(1, 8);
        if (PF.Crandom <= PF.critical)
        {
            attackPower = attackPower * (PF.Cdamage / 1.2f);
            //StartCoroutine(PlayCriEffect());
        }

        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower * PF.attackUp);
        }

        GameObject eff = Instantiate(bombEffect) as GameObject;

        eff.transform.position = transform.position;

        Destroy(gameObject);

        if (PF.bm == 0)
        {
            attackPower = 15;
        }
        else if (PF.bm == 1)
        {
            attackPower = 13;
        }
    }

    void Start()
    {
        PF = GameObject.Find("Player").GetComponent<PlayerFire>();
        
    }

    
    void Update()
    {
        

    }
    /*
    IEnumerator PlayCriEffect()
    {
        CriEffect.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        CriEffect.SetActive(false);
    }
    */
}
