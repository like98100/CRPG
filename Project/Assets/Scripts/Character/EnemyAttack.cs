using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Rigidbody rigid;
    EnemyAct enemy;
    public GameObject playerObject, particleObject, criticalParticleObject, particleObject_Inst;
    public PlayerAct player;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyAct>();
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerAct>();
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    private void OnTriggerEnter(Collider collision)
    {
        //if((collision.gameObject.tag == "EnemyBody" || collision.gameObject.tag == "EnemyLeg" || collision.gameObject.tag == "EnemyHead")
        //    && act.animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack"))
        if ((collision.gameObject.tag == "PlayerBody")// || collision.gameObject.tag == "EnemyLeg" || collision.gameObject.tag == "EnemyHead")
    && enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack"))
        {
            //Debug.Log("피격");

            //player.player.Hp -= act.monster.mons[1].nAttackDmg;
            if (CardFunction.GetOnHit(false))
            {
                Debug.Log("적군의 파멸의 일격!");
                float sum = player.player.MaxHp * 0.2f;
                player.player.Hp -= sum;
                CardFunction.SetOnHit(false, false);

                particleObject_Inst = Instantiate(criticalParticleObject, playerObject.transform.position, playerObject.transform.rotation);
            }
            else
            {
                player.player.Hp -= enemy.monster.mons[enemy.GetEnemyNum()].nAttackDmg;
                particleObject_Inst = Instantiate(particleObject, playerObject.transform.position, playerObject.transform.rotation);
            }

            if (player.player.Hp < 0) player.player.Hp = 0;

            Destroy(particleObject_Inst, 1.0f);
        }
    }
}
