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
        drawFlag = 0;   //0 : �ƹ��� ��ο� ����, 1 : �÷��̾�, 2 : ��

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
            case "������ �߰���":
                if (burfEntity[idx])
                {
                    player.player.movementspeed /= 1.35f;
                }
                else
                {
                    enemy.monster.mons[enemy.GetEnemyNum()].movementspeed /= 1.35f;

                }
                break;
            case "����� ����":
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
            case "������ �߰���":
                if(burfEntity[idx])
                {
                    player.player.movementspeed *= 1.35f;
                }
                else
                {
                    enemy.monster.mons[enemy.GetEnemyNum()].movementspeed *= 1.35f;
                }
                break;
            case "����� ����":
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
        int sum = cardNum - 1;  //   ���� ī�� ������ ������ �и��� ������ �߻��Ͽ� �ӽù������� �ذ�
        //Debug.Log("���� �� ī�� �ѹ� : " + cardNum);
        switch (sum)
        {
            case 0: // ȸ��
                {
                    if(isPlayer)
                    {
                        if (player.player.Hp + 5.0f < player.player.MaxHp) player.player.Hp += 5.0f;
                        else player.player.Hp = player.player.MaxHp;
                        //Debug.Log("�÷��̾ ü���� ȸ���߽��ϴ�.");
                    }
                    else
                    {
                        if (enemy.monster.mons[enemy.GetEnemyNum()].Hp + 5.0f <  enemy.monster.mons[enemy.GetEnemyNum()].MaxHp) enemy.monster.mons[enemy.GetEnemyNum()].Hp += 5.0f;
                        else enemy.monster.mons[enemy.GetEnemyNum()].Hp = enemy.monster.mons[enemy.GetEnemyNum()].MaxHp;
                        //Debug.Log("���� ü���� ȸ���߽��ϴ�.");
                    }
                    break;
                }
            case 1: // ���� ȸ��
                {
                    if (isPlayer)
                    {
                        if (player.player.Mp + 5.0f < player.player.MaxMp) player.player.Mp += 5.0f;
                        else player.player.Mp = player.player.MaxMp;
                        //Debug.Log("�÷��̾ ������ ȸ���߽��ϴ�.");
                    }
                    else
                    {
                        if (enemy.monster.mons[enemy.GetEnemyNum()].Mp + 5.0f < enemy.monster.mons[enemy.GetEnemyNum()].MaxMp) enemy.monster.mons[enemy.GetEnemyNum()].Mp += 5.0f;
                        else enemy.monster.mons[enemy.GetEnemyNum()].Mp = enemy.monster.mons[enemy.GetEnemyNum()].MaxMp;
                        //Debug.Log("���� ������ ȸ���߽��ϴ�.");
                    }
                    break;
                }
            case 2: // ��ο�
                {
                        if (cardManager.getCardCount(isPlayer) + 3 < 9)
                        {
                            drawCnt = 3;
                            if (isPlayer) drawFlag = 1;
                            else drawFlag = 2;
                            //cardManager.AddCard(isPlayer);
                            
                            Debug.Log("�÷��̾�(" + isPlayer + ")�� ��ų�� �̿��� ī�带 ��ο��߽��ϴ�.");
                        }
                        else
                        {
                            Debug.Log("�÷��̾�(" + isPlayer + ")�� ������ ������ ���ڶ��ϴ�.");
                            return false;
                        }
                    break;
                }
            case 3: // �ܰ� ��ô
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
                        //Debug.Log("�÷��̾��� �ܰ� ��ô");
                    }
                    else
                    {
                        //projectile.speed = 5.0f;
                        //projectile.damage = 1.0f;
                        projectile.isPlayer = false;

                        projectileObject_Inst = Instantiate(projectileObject, new Vector3(enemy.monster.mons[enemy.GetEnemyNum()].monsterPosX_f, 0, enemy.monster.mons[enemy.GetEnemyNum()].monsterPosY_f), Quaternion.identity);
                        //projectileObject_Inst.GetComponent<Projectile>().SetProjectile(isPlayer, 5.0f, 1.0f);
                        //Debug.Log("�� ĳ������ �ܰ� ��ô");
                    }
                    break;
                }
            case 4: // �̼� ����
                AddBurf("������ �߰���", isPlayer, 3.0f);
                break;
            case 5: // �ĸ��� �ϰ�(���� �ִ�ü�� 20% �����)
                if (isPlayer) SetOnHit(true, true);
                else SetOnHit(false, true);
                break;
            case 6: // ���� ����
                AddBurf("����� ����", isPlayer, 5.0f);
                break;
            default:
                break;
        }

        return true;
    }
}
