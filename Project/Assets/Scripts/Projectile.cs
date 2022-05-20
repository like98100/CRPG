using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rigid;
    public GameObject projectile, playerObject, enemyObject, particleObject, particleObject_Inst;
    public PlayerAct player;
    public EnemyAct enemy;
    public CharOrder charOrder;

    public bool isPlayer;
    public float rotate;
    public float speed;
    public float damage;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        //Debug.Log("오브젝트 이름 : "+playerObject.name);
        enemyObject = GameObject.FindWithTag("Enemy");
        charOrder = projectile.GetComponent<CharOrder>();
        charOrder.playerObject = playerObject;
        charOrder.enemyObject = enemyObject;

        player = playerObject.GetComponent<PlayerAct>();
        enemy = enemyObject.GetComponent<EnemyAct>();

        rigid = GetComponent<Rigidbody>();
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        //this.gameObject.GetComponent<BoxCollider>().size = new Vector3(20f, 20f, 20f);
        this.gameObject.transform.localEulerAngles = new Vector3(30.0f, 0f, 0f);

        direction = new Vector3(0, 0, 0);
        //speed = 0.0f;
        //damage = 0.0f;
        if(direction == new Vector3(0,0,0))
        {
            if (isPlayer)
            {
                //Vector2 direction = player.transform.position - transform.position; //플레이어 - 투사체 벡터(타겟팅 전용)
                if (player.transform.localScale.x == 1 && (direction.x == 0))
                {
                    direction = new Vector3(-1, 0, 0);
                    rotate = 360.0f;
                }
                else if (player.transform.localScale.x == -1 && (direction.x == 0))
                {
                    direction = new Vector3(1, 0, 0);
                    rotate = -360.0f;
                }
                if (rigid.velocity.x == 0 && rigid.velocity.y == 0) //rigid.velocity = direction.normalized * speed;
                    rigid.velocity = new Vector2(direction.normalized.x * speed, 0f);

                transform.Rotate(0, 0, rotate * Time.deltaTime * 2);
            }
            else
            {
                if (enemy.isLeft && (direction.x == 0))
                {
                    direction = new Vector3(-1, 0, 0);
                    rotate = 360.0f;
                }
                else if(!enemy.isLeft && (direction.x == 0))
                {
                    direction = new Vector3(1, 0, 0);
                    rotate = -360.0f;
                }
                if (rigid.velocity.x == 0 && rigid.velocity.y == 0) rigid.velocity = new Vector2(direction.normalized.x * speed, 0f);
                transform.Rotate(0, 0, rotate * Time.deltaTime * 2);
            }
        }

        //Debug.Log(this.direction + ", " + this.rotate + ", " + this.isPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayer)
        {
            //Vector2 direction = player.transform.position - transform.position; //플레이어 - 투사체 벡터(타겟팅 전용)
            if (player.transform.localScale.x == 1 && (direction.x == 0))
            {
                direction = new Vector3(-1, 0, 0);
                rotate = 360.0f;
            }
            else if (player.transform.localScale.x == -1 && (direction.x == 0))
            {
                direction = new Vector3(1, 0, 0);
                rotate = -360.0f;
            }
            //rigid.velocity = direction.normalized * speed;
            rigid.velocity = new Vector2(direction.normalized.x * speed, 0f);
            transform.Rotate(0, 0, rotate * Time.deltaTime * 2);
        }
        else
        {
            if (enemy.isLeft && (direction.x == 0))
            {
                direction = new Vector3(-1, 0, 0);
                rotate = 360.0f;
            }
            else if (!enemy.isLeft && (direction.x == 0))
            {
                direction = new Vector3(1, 0, 0);
                rotate = -360.0f;
            }
            if (rigid.velocity.x == 0 && rigid.velocity.y == 0) //rigid.velocity = direction.normalized * speed;
                rigid.velocity = new Vector2(direction.normalized.x * speed, 0f);
            transform.Rotate(0, 0, rotate * Time.deltaTime * 2);
        }

        if(this.transform.position.x < -200f || this.transform.position.x > 200f)
        {
            Destroy(this.gameObject, 0.0f);
            Destroy(this.particleObject_Inst, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isPlayer && (collision.gameObject.tag == "EnemyBody"))// || collision.gameObject.tag == "EnemyLeg" || collision.gameObject.tag == "EnemyHead")
        {
            //Debug.Log("단검 타격 성공");
            enemy.monster.mons[enemy.GetEnemyNum()].Hp -= Damage.SetDamage(damage, true);
            if (enemy.monster.mons[enemy.GetEnemyNum()].Hp < 0) enemy.monster.mons[enemy.GetEnemyNum()].Hp = 0;
            //Instantiate(particleObject_Inst, enemyObject.transform.position, enemyObject.transform.rotation);
            particleObject_Inst = Instantiate(particleObject, new Vector3(enemy.monster.mons[enemy.GetEnemyNum()].monsterPosX_f, enemy.monster.mons[enemy.GetEnemyNum()].monsterPosY_f, 0), Quaternion.identity);
            // 제거
            Destroy(this.gameObject, 0.0f);
            Destroy(this.particleObject_Inst, 1.0f);
            
        }
        else if(!isPlayer && (collision.gameObject.tag == "PlayerBody"))
        {
            //Debug.Log("적 단검 타격 성공");
            player.player.Hp -= Damage.SetDamage(damage, false);
            if (player.player.Hp < 0) player.player.Hp = 0;
            //Instantiate(particleObject_Inst, enemyObject.transform.position, enemyObject.transform.rotation);
            particleObject_Inst = Instantiate(particleObject, new Vector3(player.player.playerPosX_f, player.player.playerPosY_f, 0), Quaternion.identity);
            // 제거
            Destroy(this.gameObject, 0.0f);
            Destroy(this.particleObject_Inst, 1.0f);
        }
    }

    //public void SetProjectile(bool isPlayer, float speed, float damage)
    //{
    //    this.isPlayer = isPlayer; this.speed = speed; this.damage = damage;
    //    this.direction = new Vector2(0, 0);
    //    if (isPlayer)
    //    {
    //        //Vector2 direction = player.transform.position - transform.position; //플레이어 - 투사체 벡터(타겟팅 전용)
    //        if (player.isLeft && (direction.x == 0))
    //        {
    //            direction = new Vector2(-1, 0);
    //            rotate = 360.0f;
    //        }
    //        else if (!player.isLeft && (direction.x == 0))
    //        {
    //            direction = new Vector2(1, 0);
    //            rotate = -360.0f;
    //        }
    //        //if (rigid.velocity == new Vector2(0, 0)) rigid.velocity = direction.normalized * speed;
    //        //transform.Rotate(0, 0, rotate * Time.deltaTime * 2);
    //    }
    //    else
    //    {

    //    }

    //}
}
