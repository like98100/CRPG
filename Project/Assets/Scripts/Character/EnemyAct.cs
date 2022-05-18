using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

//using UnityEngine.UI;

using Panda;

public class EnemyAct : MonoBehaviour
{

    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    public class DataManager
    {
        public Dictionary<int, MonsterClass> CardDict { get; private set; } = new Dictionary<int, MonsterClass>();

    }

    [System.Serializable]
    public class MonsterClass
    {
        public int MonsterNum;
        public string CharTypeName;
        public string CharName;
        public float MaxHp;
        public float MaxMp;
        public float Hp;
        public float Mp;
        public string Weapon;
        public string Equipment;
        public List<int> MonDeckList = new List<int>();

        public float monsterPosX_f, monsterPosY_f;
        public int monsterPosX_i, monsterPosY_i;
        public float movementspeed;

        public float nAttackDmg;
        public float nAttackSpd;

        public MonsterClass()
        {
            MonsterNum = -999;
            CharTypeName = typeof(MonsterClass).ToString();
            CharName = "Monster";
            MaxHp = 20.0f;
            MaxMp = 20.0f;
            Hp = 5.0f;
            Mp = 5.0f;
            Weapon = "none";
            Equipment = "none";
            for (int i = 0; i < 9; i++)
            {
                int sum = i;
                //CharDeckList.Add(sum.ToString());
                MonDeckList.Add(sum + 5);
            }
            monsterPosX_f = 5.5f; monsterPosY_f = 0.0f;
            monsterPosX_i = 2; monsterPosY_i = 0;
            movementspeed = 5.0f;

            nAttackDmg = 1.0f;
            nAttackSpd = 5.5f;
        }

        public void Log()
        {
            Debug.Log("MonsterNum : " + MonsterNum);
            Debug.Log("CharTypeName : " + CharTypeName);
            Debug.Log("Charname : " + CharName);
            Debug.Log("MaxHp : " + MaxHp);
            Debug.Log("MaxMp : " + MaxMp);
            Debug.Log("Hp : " + Hp);
            Debug.Log("Mp : " + Mp);
            Debug.Log("Weapon : " + Weapon);
            Debug.Log("Equipment : " + Equipment);
            Debug.Log("PositionF : " + monsterPosX_f + ", " + monsterPosY_f);
            Debug.Log("PositionINT : " + monsterPosX_i + ", " + monsterPosY_i);
            Debug.Log("MovementSpeed : " + movementspeed);
            //foreach (string Deck in CharDeckList) Debug.Log("Deck : " + Deck);
            for (int i = 0; i < MonDeckList.Count; i++) Debug.Log("Deck : " + MonDeckList[i]);

            Debug.Log("nAttackDmg : " + nAttackDmg);
            Debug.Log("nAttackSpd : " + nAttackSpd);
        }
    }

    [System.Serializable]
    public class MonsterDataClass : ILoader<int, MonsterClass>
    {
        public List<MonsterClass> mons = new List<MonsterClass>();   //json 파일에서 이 리스트에 데이터가 옮겨질 예정

        public Dictionary<int, MonsterClass> MakeDict()        //오버라이딩
        {
            Dictionary<int, MonsterClass> dict = new Dictionary<int, MonsterClass>();
            foreach (MonsterClass monsterClass in mons)       // 리스트에서 dictionary로 옮기는 작업
                dict.Add(monsterClass.MonsterNum, monsterClass);      // cardnum을 키값으로 설정
            return dict;
        }

        public MonsterDataClass()
        {
            MonsterClass nullMonster = new MonsterClass();
            mons.Add(nullMonster);
        }
    }

    public GameObject monsterObject, particleObject, particleObject_Inst, panelObject, cardManager;
    public MonsterDataClass monster;
    public GameObject playerObject;
    public PlayerAct player;
    public CardManager manager;
    
    
    public Animator animator;

    int enemyNum;
    int particleCnt;
    public bool isLeft;
    bool victory;

    float skillCooldown;
    float time;

    [SerializeField] Panel panel;

    Vector3 destination;

    #region tasks

    [Task]
    bool isDanger = false;
    [Task]
    bool dead = false;
    [Task]
    bool isRange = false;
    //[Task]
    //List<int> cardNum = new List<int>();
    [Task]
    bool TagStatus()
    {
        SetDanger();
        return true;
    }

    [Task]
    bool GetDead()
    {
        //Debug.Log("Enemy's life is " + dead);
        return dead;
    }
    [Task]
    bool GetRange()
    {
        return isRange;
    }

    [Task]
    void Attack()
    {
        if(isRange)
        {
            animator.SetBool("attack", true);
        }
        else
        {
            animator.SetBool("attack", false);
        }
    }

    [Task]
    void MoveToDestination()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("dead")) return;
            var task = Task.current;
            var delta = destination - monsterObject.transform.position;
        //var delta = destination - new Vector3(monster.mons[1].monsterPosX_f, monster.mons[1].monsterPosY_f, 0.0f);

        //if (transform.position != destination)
        if (!isRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack"))
        {
            var velocity = delta.normalized * monster.mons[enemyNum].movementspeed;
            //transform.position += velocity * Time.deltaTime;
            monsterObject.transform.Translate(velocity * Time.deltaTime);

                //var newDelta = destination = -transform.position;
                //if (Vector3.Dot(newDelta, delta) < 0.0f)
                //{
                //    transform.position = destination;
                //}
            monster.mons[enemyNum].monsterPosX_f = monsterObject.transform.position.x;
            monster.mons[enemyNum].monsterPosY_f = monsterObject.transform.position.z; // 2D->3D 월드로 이전하면서 가독성 문제 발생(변수 수정할 것)

            if (!isDanger) destination = new Vector3(player.player.playerPosX_f, 0f, player.player.playerPosY_f);
            else
            {
                if ((monsterObject.transform.position.x - destination.x < 0.01f && monsterObject.transform.position.x - destination.x > -0.01f)
                    && (monsterObject.transform.position.z - destination.z < 0.01f && monsterObject.transform.position.z - destination.z > -0.01f))
                {
                    destination = Random.insideUnitSphere * 4.0f;

                    if (destination.x < -6.5f) destination.x = -6.5f;
                    else if (destination.x > 6.95f) destination.x = 6.95f;

                    if (destination.z < -3.0f) destination.z = -3.0f;
                    else if (destination.z > 3.8f) destination.z = 3.8f;
                }

                destination.y = 0.0f;
            }

        }

        //if (transform.position == destination) task.Succeed();
        if (isRange) task.Succeed();

    }

    [Task]
    bool SetDestination_Player()
    {
        //destination = player.transform.position;
        destination = new Vector3(player.player.playerPosX_f, 0f, player.player.playerPosY_f);
        //Debug.Log("destination.x : " + destination.x);
        return true;
    }

    [Task]
    bool SetDestination_Random()
    {
        if ((monsterObject.transform.position.x - destination.x < 0.01f || monsterObject.transform.position.x - destination.x > -0.01f)
            && (monsterObject.transform.position.z - destination.z < 0.01f || monsterObject.transform.position.z - destination.z > -0.01f))
        {
            destination = Random.insideUnitSphere * 4.0f;

            if (destination.x < -9.75f) destination.x = -9.75f;
            else if (destination.x > 9.75f) destination.x = 9.75f;

            if (destination.z < -2.5f) destination.z = -2.5f;
            else if (destination.z > 2.5f) destination.z = 2.5f;
        }
        destination.y = 0.0f;

        return true;
    }

    //[Task]
    //bool SetCardNum()
    //{
    //    cardNum.Clear();

    //    Debug.Log("현재 적 카드 카운트 : " + manager.EnemyCards.Count);
    //    for (int i = 0; i < manager.EnemyCards.Count; i++)
    //    {
    //        if (i >= manager.EnemyCards.Count) break;
    //        Debug.Log(i + "번째 : " + manager.EnemyCards[i].cardNum);
    //        cardNum.Add(manager.EnemyCards[i].cardNum);
    //    }
    //    return true;
    //}

    //[Task] bool UseEnemyCard()
    //{
    //    int maxIdx = -1;
    //    for(int i = 0; i < cardNum.Count; i++)
    //    {
    //        if (cardNum[i] == 1 && monster.mons[1].Mp != monster.mons[1].MaxMp)
    //        {
    //            maxIdx = cardNum[i];
    //            break;
    //        }
    //        else if (cardNum[i] == 2)
    //        {
    //            continue;
    //        }
    //        else if (maxIdx < cardNum[i])
    //        {
    //            maxIdx = cardNum[i];
    //        }
    //    }
    //    Debug.Log("쓰려는 카드 : " + manager.EnemyCards[maxIdx].name);
    //    if (maxIdx >= 0 && maxIdx < cardNum.Count) manager.UseEnemyCard(manager.EnemyCards[maxIdx]);

    //    //bool result = SetCardNum();
    //    //Debug.Log(result);

    //    //return result;

    //    return true;
    //}

    #endregion


    public int GetEnemyNum()
    {
        return enemyNum;
    }

    void SetDanger()
    {
        isDanger = !isDanger;
    }

    void SetRange()
    {
        isRange = !isRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        //monster = new MonsterDataClass();
        //CreateCharJSON();

        enemyNum = SelectStage.GetStageNum();

        monsterObject = GameObject.FindWithTag("Enemy");
        playerObject = GameObject.FindWithTag("Player");
        BringCharJSON();
        monsterObject.transform.position = new Vector3(monster.mons[enemyNum].monsterPosX_f, monster.mons[enemyNum].monsterPosY_f, 0);
        animator = this.GetComponent<Animator>();
        animator.SetFloat("attackSpeed", monster.mons[enemyNum].nAttackSpd);

        player = playerObject.GetComponent<PlayerAct>();
        animator.SetBool("moving", false);
        animator.SetBool("dead", false);
        particleCnt = 0;

        panelObject = GameObject.FindWithTag("Panel");
        panel = panelObject.GetComponent<Panel>();

        cardManager = GameObject.Find("CardManager");
        manager = cardManager.GetComponent<CardManager>();
        //cardNum = new List<int>();

        victory = false;
        dead = false;
        isLeft = true;

        destination = Vector3.zero;
        isRange = false;

        time = 0f;
        skillCooldown = 3.0f;

        if(monster.mons[enemyNum].Weapon != "none")
        {
            Debug.Log(monster.mons[enemyNum].Weapon + "으로 무기를 변환합니다.");
            SpriteRenderer weapon = monsterObject.transform.GetChild(3).GetComponent<SpriteRenderer>();
            weapon.sprite = Resources.Load<Sprite>("Sprites/WeaponImages/" + monster.mons[enemyNum].Weapon) as Sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (monster.mons[enemyNum].MaxHp / monster.mons[enemyNum].Hp >= 2.0f) isDanger = true;
        else isDanger = false;

        time += Time.deltaTime;
        if (time >= skillCooldown) UseCard();


        // 항상 player를 바라본다
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("dead") && !animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack"))
        {
            if (player.player.playerPosX_f >= monster.mons[enemyNum].monsterPosX_f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isLeft = false;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                isLeft = true;
            }
        }


        // player가 공격범위 안에 있는 경우 계산
        if ((player.player.playerPosX_f - 1.5f <= monster.mons[enemyNum].monsterPosX_f && player.player.playerPosX_f + 1.5f >= monster.mons[enemyNum].monsterPosX_f)
            && (player.player.playerPosY_f - 1.5f <= monster.mons[enemyNum].monsterPosY_f && player.player.playerPosY_f + 1.5f >= monster.mons[enemyNum].monsterPosY_f))
        {   //공격범위 안에 있을 때
            isRange = true;
            animator.SetBool("moving", false);
        }
        else
        {   //공격범위 밖에 있을 때
            isRange = false;
            animator.SetBool("moving", true);
        }


        // 사망
        if (monster.mons[enemyNum].Hp == 0)
        {
            animator.SetBool("dead", true);
            //if(monsterObject.transform.position.x < 800.0f) particleObject_Inst = Instantiate(particleObject, monsterObject.transform.position, monsterObject.transform.rotation);
            //Destroy(particleObject_Inst, 2.5f);
            Invoke("Die", 1.0f);
            if(!victory && dead) Victory();
        }
    }

    void UseCard()
    {
        int idx = Random.Range(0, manager.EnemyCards.Count - 1);

        manager.UseEnemyCard(manager.EnemyCards[idx]);

        skillCooldown = Random.Range(1.5f, 5.0f);
        time = 0f;
    }

    void Die()
    {
        //monsterObject.transform.position = new Vector3(900.0f, 900.0f, 0);
        dead = true;
    }
    void Victory()
    {
        panel.Show("승리");
        victory = true;
        GameManager.IsVictory = true;
        Invoke("MoveScene", 1.0f);
    }
    void MoveScene()
    {
        GameManager.Inst.GoResultScene(true);
    }

    void CreateCharJSON()
    {
        string playerToJsonData = monsterObject.GetComponent<ControlJSON>().ObjectToJson(monster);      // player 클래스 내부 변수와 값들을 string으로 변환
        monsterObject.GetComponent<ControlJSON>().CreateJsonFile(Application.dataPath, "Monster", playerToJsonData); // data 폴더에 json 생성
        Debug.Log("Create Player Json File.");
    }

    void BringCharJSON()
    {
        string data = monsterObject.GetComponent<ControlJSON>().LoadJsonFile<MonsterDataClass>(Application.dataPath, "Monster");
        //Debug.Log(data);
        monster = JsonUtility.FromJson<MonsterDataClass>(data);
        //player = playObject.GetComponent<ControlJSON>().JsonToObjectOverwrite<CharClass_Playable>(data, player);
        //Debug.Log("Bring data from Player Json File.");
    }

    void OverwriteCharJSON()
    {
        string playerToJsonData = monsterObject.GetComponent<ControlJSON>().ObjectToJson(monster);
        monsterObject.GetComponent<ControlJSON>().OverWriteJsonFile(Application.dataPath, "Monster", playerToJsonData);
        //Debug.Log("Overwrite Player Json File.");
    }
}
