using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PolygonCollider2D))]
public class PlayerAttack : MonoBehaviour
{
    Rigidbody rigid;
    PlayerAct act;
    public GameObject enemyObject, particleObject, criticalParticleObject, particleObject_Inst;
    public EnemyAct enemy;

    [SerializeField] SpeechBubble speechBubble;
    [SerializeField] TutorialBubble tutorialBubble;
    private void Awake()
    {
        enemyObject = GameObject.FindWithTag("Enemy");
        rigid = GetComponent<Rigidbody>();
        act = GameObject.Find("Player").GetComponent<PlayerAct>();
        enemy = enemyObject.GetComponent<EnemyAct>();

        //this.gameObject.AddComponent<PolygonCollider2D>();
        //this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;

        speechBubble = GameObject.Find("SpeechBubble").GetComponent<SpeechBubble>();
        tutorialBubble = GameObject.Find("TutorialPanel").GetComponent<TutorialBubble>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    private void OnTriggerEnter(Collider collision)
    {
        //if((collision.gameObject.tag == "EnemyBody" || collision.gameObject.tag == "EnemyLeg" || collision.gameObject.tag == "EnemyHead")
        //    && act.animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack"))
        if ((collision.gameObject.tag == "EnemyBody")// || collision.gameObject.tag == "EnemyLeg" || collision.gameObject.tag == "EnemyHead")
    && act.animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack"))
        {
            //Debug.Log("타격 성공");
            if (CardFunction.GetOnHit(true))
            {
                Debug.Log("파멸의 일격!");
                
                float sum = enemy.monster.mons[enemy.GetEnemyNum()].MaxHp * 0.2f;
                enemy.monster.mons[enemy.GetEnemyNum()].Hp -= Damage.SetDamage(sum, true);
                CardFunction.SetOnHit(true, false);

                particleObject_Inst = Instantiate(criticalParticleObject, enemyObject.transform.position, enemyObject.transform.rotation);
            }
            else
            {
                enemy.monster.mons[enemy.GetEnemyNum()].Hp -= Damage.SetDamage(act.player.nAttackDmg, true);
                particleObject_Inst = Instantiate(particleObject, enemyObject.transform.position, enemyObject.transform.rotation);
            }

            if(enemy.monster.mons[enemy.GetEnemyNum()].Hp < 0) enemy.monster.mons[enemy.GetEnemyNum()].Hp = 0;

            Destroy(particleObject_Inst, 1.0f);

            if(speechBubble.PanelTMP.text == "마우스 우클릭으로" + System.Environment.NewLine + "기본 공격을 할 수 있습니다."
                || speechBubble.PanelTMP.text == "지금은" + System.Environment.NewLine + "기본 공격을 사용해 봅시다.")
            {
                speechBubble.Show("잘 하셨습니다.");
                Invoke("SetTutoFlag", 1.8f);
                
            }
        }
    }

    void SetTutoFlag()
    {
        tutorialBubble.tutoFlag = 3;

    }
}
