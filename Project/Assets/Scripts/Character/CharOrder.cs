using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharOrder : MonoBehaviour
{
    public GameObject playerObject, enemyObject, playerParts;
    public PlayerAct player;
    public EnemyAct enemy;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        enemyObject = GameObject.FindWithTag("Enemy");
        playerParts = this.gameObject;
        player = playerObject.GetComponent<PlayerAct>();
        enemy = enemyObject.GetComponent<EnemyAct>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.player.playerPosY_f > enemy.monster.mons[enemy.GetEnemyNum()].monsterPosY_f)
        {
            if(playerParts.gameObject.name == "Weapon-Sword")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 51;
            }
            else if (playerParts.gameObject.name == "Head")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 52;
            }
            else if(playerParts.gameObject.name == "Hat-Helmet")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 53;
            }
            else if(playerParts.gameObject.name == "Projectile(Clone)")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 54;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 50;
            }
            
        }

        else
        {
            if (playerParts.gameObject.name == "Weapon-Sword")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 201;
            }
            else if (playerParts.gameObject.name == "Head")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 202;
            }
            else if (playerParts.gameObject.name == "Hat-Helmet")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 203;
            }
            else if (playerParts.gameObject.name == "Projectile(Clone)")
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 204;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().sortingOrder = 200;
            }
        }

    }

}
