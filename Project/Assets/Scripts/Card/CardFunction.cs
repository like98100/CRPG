using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardFunction : MonoBehaviour
{
    public GameObject playerObject, enemyObject, manageObject, projectileObject, projectileObject_Inst;
    public PlayerAct player;
    public EnemyAct enemy;
    public Projectile projectile;
    public CardManager cardManager;
    public GameManager gameManager;

    public int drawCnt, drawFlag;

    List<bool> burfEntity;
    List<string> burfList;
    List<float> time;
    List<float> burfTime;

    static bool[] onHit;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        enemyObject = GameObject.FindWithTag("Enemy");
        player = playerObject.GetComponent<PlayerAct>();
        enemy = enemyObject.GetComponent<EnemyAct>();
        projectile = projectileObject.GetComponent<Projectile>();
        cardManager = this.GetComponent<CardManager>();
        gameManager = this.GetComponent<GameManager>();
        drawCnt = 0;
        drawFlag = 0;   //0 : 아무도 드로우 안함, 1 : 플레이어, 2 : 적

        burfEntity = new List<bool>();
        burfList = new List<string>();
        time = new List<float>();
        burfTime = new List<float>();
        onHit = new bool[] {false, false};
    }

    // Update is called once per frame
    void Update()
    {
        if (burfList.Count != 0)
        {
            for (int i = 0; i < time.Count; i++)
            {
                time[i] += Time.deltaTime;

                if(time[i] >= burfTime[i])
                {
                    DeleteBurf(i);
                }
            }
        }
    }

    void DeleteBurf(int idx)
    {
        switch (burfList[idx])
        {
            case "가벼운 발걸음":
                if (burfEntity[idx])
                {
                    player.player.movementspeed /= 1.35f;
                }
                else
                {
                    enemy.monster.mons[enemy.GetEnemyNum()].movementspeed /= 1.35f;

                }
                break;
            case "재빠른 공격":
                if (burfEntity[idx])
                {
                    player.player.nAttackSpd /= 1.5f;
                    player.animator.SetFloat("attackSpeed", player.player.nAttackSpd);
                }
                else
                {
                    enemy.monster.mons[enemy.GetEnemyNum()].nAttackSpd /= 1.5f;
                    enemy.animator.SetFloat("attackSpeed", enemy.monster.mons[enemy.GetEnemyNum()].nAttackSpd);
                }
                break;
            default:
                break;
        }


        burfEntity.RemoveAt(idx);
        burfList.RemoveAt(idx);
        burfTime.RemoveAt(idx);
        time.RemoveAt(idx);
    }

    void AddBurf(string burfName, bool isPlayer, float addTime)
    {
        burfEntity.Add(isPlayer);
        burfList.Add(burfName);
        time.Add(0f);
        burfTime.Add(addTime);
        int last = burfList.Count - 1;
        InitBurf(last);
    }

    void InitBurf(int idx)
    {
        switch(burfList[idx])
        {
            case "가벼운 발걸음":
                if(burfEntity[idx])
                {
                    player.player.movementspeed *= 1.35f;
                }
                else
                {
                    enemy.monster.mons[enemy.GetEnemyNum()].movementspeed *= 1.35f;
                }
                break;
            case "재빠른 공격":
                if (burfEntity[idx])
                {
                    player.player.nAttackSpd *= 1.5f;
                    player.animator.SetFloat("attackSpeed", player.player.nAttackSpd);
                }
                else
                {
                    enemy.monster.mons[enemy.GetEnemyNum()].nAttackSpd *= 1.5f;
                    enemy.animator.SetFloat("attackSpeed", enemy.monster.mons[enemy.GetEnemyNum()].nAttackSpd);
                }
                break;
            default:
                break;
        }
    }

    public static bool GetOnHit(bool isPlayer)
    {
        if (isPlayer) return onHit[0];
        else return onHit[1];
    }

    public static void SetOnHit(bool isPlayer, bool idx)
    {
        if (isPlayer) onHit[0] = idx;
        else onHit[1] = idx;
    }    

    public bool RunCardFunction(int cardNum, bool isPlayer)
    {
        int sum = cardNum - 1;  //   더미 카드 때문에 순번이 밀리는 문제가 발생하여 임시방편으로 해결
        //Debug.Log("현재 쓴 카드 넘버 : " + cardNum);
        switch (sum)
        {
            case 0: // 회복
                {
                    if(isPlayer)
                    {
                        if (player.player.Hp + 5.0f < player.player.MaxHp) player.player.Hp += 5.0f;
                        else player.player.Hp = player.player.MaxHp;
                        //Debug.Log("플레이어가 체력을 회복했습니다.");
                    }
                    else
                    {
                        if (enemy.monster.mons[enemy.GetEnemyNum()].Hp + 5.0f <  enemy.monster.mons[enemy.GetEnemyNum()].MaxHp) enemy.monster.mons[enemy.GetEnemyNum()].Hp += 5.0f;
                        else enemy.monster.mons[enemy.GetEnemyNum()].Hp = enemy.monster.mons[enemy.GetEnemyNum()].MaxHp;
                        //Debug.Log("적이 체력을 회복했습니다.");
                    }
                    break;
                }
            case 1: // 마나 회복
                {
                    if (isPlayer)
                    {
                        if (player.player.Mp + 5.0f < player.player.MaxMp) player.player.Mp += 5.0f;
                        else player.player.Mp = player.player.MaxMp;
                        //Debug.Log("플레이어가 마나를 회복했습니다.");
                    }
                    else
                    {
                        if (enemy.monster.mons[enemy.GetEnemyNum()].Mp + 5.0f < enemy.monster.mons[enemy.GetEnemyNum()].MaxMp) enemy.monster.mons[enemy.GetEnemyNum()].Mp += 5.0f;
                        else enemy.monster.mons[enemy.GetEnemyNum()].Mp = enemy.monster.mons[enemy.GetEnemyNum()].MaxMp;
                        //Debug.Log("적이 마나를 회복했습니다.");
                    }
                    break;
                }
            case 2: // 드로우
                {
                        if (cardManager.getCardCount(isPlayer) + 3 < 9)
                        {
                            drawCnt = 3;
                            if (isPlayer) drawFlag = 1;
                            else drawFlag = 2;
                            //cardManager.AddCard(isPlayer);
                            
                            Debug.Log("플레이어(" + isPlayer + ")가 스킬을 이용해 카드를 드로우했습니다.");
                        }
                        else
                        {
                            Debug.Log("플레이어(" + isPlayer + ")의 손패의 공간이 모자랍니다.");
                            return false;
                        }
                    break;
                }
            case 3: // 단검 투척
                {
                    projectile.speed = 5.0f;
                    projectile.damage = 1.0f;
                    if (isPlayer)
                    {
                        //projectile.speed = 5.0f;
                        //projectile.damage = 1.0f;
                        projectile.isPlayer = true;

                        projectileObject_Inst = Instantiate(projectileObject, new Vector3(player.player.playerPosX_f, 0, player.player.playerPosY_f), Quaternion.identity);
                        //projectileObject_Inst.GetComponent<Projectile>().SetProjectile(isPlayer, 5.0f, 1.0f);
                        //projectileObject_Inst.GetComponent<Projectile>().speed = 5.0f;
                        //Debug.Log("플레이어의 단검 투척");
                    }
                    else
                    {
                        //projectile.speed = 5.0f;
                        //projectile.damage = 1.0f;
                        projectile.isPlayer = false;

                        projectileObject_Inst = Instantiate(projectileObject, new Vector3(enemy.monster.mons[enemy.GetEnemyNum()].monsterPosX_f, 0, enemy.monster.mons[enemy.GetEnemyNum()].monsterPosY_f), Quaternion.identity);
                        //projectileObject_Inst.GetComponent<Projectile>().SetProjectile(isPlayer, 5.0f, 1.0f);
                        //Debug.Log("적 캐릭터의 단검 투척");
                    }
                    break;
                }
            case 4: // 이속 증가
                AddBurf("가벼운 발걸음", isPlayer, 3.0f);
                break;
            case 5: // 파멸의 일격(온힛 최대체력 20% 대미지)
                if (isPlayer) SetOnHit(true, true);
                else SetOnHit(false, true);
                break;
            case 6: // 공속 증가
                AddBurf("재빠른 공격", isPlayer, 5.0f);
                break;
            default:
                break;
        }

        return true;
    }
}
