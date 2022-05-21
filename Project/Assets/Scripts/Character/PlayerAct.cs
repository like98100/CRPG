using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

public class PlayerAct : MonoBehaviour
{

    [System.Serializable]
    public class CharClass
    {
        public string CharTypeName;
        public string CharName;
        public float MaxHp;
        public float MaxMp;
        public float Hp;
        public float Mp;
        public string Weapon;
        public string Equipment;
        public List<int> CharDeckList = new List<int>();

        public float playerPosX_f, playerPosY_f;
        public int playerPosX_i, playerPosY_i;
        public float movementspeed;

        public CharClass()
        {
            CharTypeName = string.Empty;
            CharName = string.Empty;
            MaxHp = 10.0f;
            MaxMp = 10.0f;
            Hp = 10.0f;
            Mp = 10.0f;
            Weapon = "none";
            Equipment = "none";
            for (int i = 0; i < 9; i++)
            {
                int sum = 1;
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                            sum = 1;
                            break;
                        case 3:
                            sum = 2;
                            break;
                        case 4:
                        case 5:
                            sum = 3;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            sum = 4;
                            break;
                    }
                //CharDeckList.Add(sum.ToString());
                CharDeckList.Add(sum);
            }
            playerPosX_f = 0.0f; playerPosY_f = 0.0f;
            playerPosX_i = 0; playerPosY_i = 0;
            movementspeed = 5.0f;
        }

        public virtual void Log()
        {
            Debug.Log("CharTypeName : " + CharTypeName);
            Debug.Log("Charname : " + CharName);
            Debug.Log("Hp : " + Hp);
            Debug.Log("Mp : " + Mp);
            Debug.Log("Weapon : " + Weapon);
            Debug.Log("Equipment : " + Equipment);
            Debug.Log("PositionF : " + playerPosX_f + ", " + playerPosY_f);
            Debug.Log("PositionINT : " + playerPosX_i + ", " + playerPosY_i);
            Debug.Log("MovementSpeed : " + movementspeed);
            //foreach (string Deck in CharDeckList) Debug.Log("Deck : " + Deck);
            for (int i = 0; i < CharDeckList.Count; i++) Debug.Log("Deck : " + CharDeckList[i]);
        }

        public void SetDeckList(int idx, int sum)
        {
            CharDeckList[idx] = sum;
        }
    }

    [System.Serializable]
    public class CharClass_Playable : CharClass
    {
        public float nAttackDmg;
        public float nAttackSpd;

        public CharClass_Playable()
        {
            CharTypeName = typeof(CharClass_Playable).ToString();
            nAttackDmg = 1.0f;
            nAttackSpd = 3.5f;
        }

        public override void Log()
        {
            base.Log();
            Debug.Log("nAttackDmg : " + nAttackDmg);
            Debug.Log("nAttackSpd : " + nAttackSpd);
        }
    }
    
    [System.Serializable]
    public class CharClass_Monster : CharClass
    {
        public int MonNum;

        public CharClass_Monster()
        {
            MonNum = 0;
            CharTypeName = typeof(CharClass_Monster).ToString();
        }

        public override void Log()
        {
            Debug.Log(MonNum);
            base.Log();
        }
    }
    /*[Header("Playable Character")] */
    public CharClass_Playable player;

    public GameObject playObject, panelObject;
    public Animator animator;
    public bool isAttack = false;

    public float hAxis;
    public float vAxis;
    Vector3 moveVec;
    bool isBorder;
    bool isDead;
    bool defeat = false;
    [SerializeField] Panel panel;

    public enum STEP
    {
        NONE,
        MOVE,
        ATTACK,
        DEAD,
        NUM,
    };

    STEP step = STEP.NONE;
    STEP nextStep = STEP.NONE;
    float stepTimer;

    // Start is called before the first frame update
    void Start()
    {
        //최초 생성용
        player = new CharClass_Playable();
        //player.Log();
        CreateCharJSON();

        //BringCharJSON();
        //player.Log();
        //Debug.Log(player.Weapon);
        //player.Weapon = "Developer";
        //OverwriteCharJSON();
        //Debug.Log(player.Weapon);

        //player.Weapon = "nope";
        //BringCharJSON();
        //Debug.Log(player.Weapon);
        playObject = GameObject.FindWithTag("Player");
        BringCharJSON();
        playObject.transform.position = new Vector3(player.playerPosX_i, player.playerPosY_i, 0);
        animator = this.GetComponent<Animator>();
        animator.SetBool("moving", false);
        animator.SetFloat("attackSpeed", player.nAttackSpd);
        animator.SetBool("dead", false);


        panelObject = GameObject.FindWithTag("Panel");
        panel = panelObject.GetComponent<Panel>();
        isDead = false;

        this.step = STEP.NONE;
        this.nextStep = STEP.MOVE;
        stepTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        stepTimer += Time.deltaTime;

        // 상태 변화
        if (this.nextStep == STEP.NONE)
        {
            switch(this.step)
            {
                case STEP.MOVE: // 이동 중 변하는 상태(공격, 죽음)
                    if(Input.GetMouseButton(1))  //좌클릭 시
                        this.nextStep = STEP.ATTACK;

                    if(player.Hp == 0)  // 체력이 0이 됐을 때
                        this.nextStep = STEP.DEAD;
                    break;
                case STEP.ATTACK:   // 공격 중 변하는 상태(공격 완료 후 이동, 죽음)
                    //if (animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    if(Input.GetMouseButtonUp(1))   // 클릭 해제 시
                        this.nextStep = STEP.MOVE;

                    if (player.Hp == 0)  // 체력이 0이 됐을 때
                        this.nextStep = STEP.DEAD;
                    break;
                case STEP.DEAD: // 죽은 이후엔 변하지 않음
                    break;
            }
        }

        // 상태가 변했을 때
        while(this.nextStep != STEP.NONE)
        {
            this.step = nextStep;
            this.nextStep = STEP.NONE;
            switch(this.step)
            {
                case STEP.MOVE:
                    animator.SetBool("attack", false);
                    isAttack = false;
                    break;
                case STEP.ATTACK:
                    animator.SetBool("attack", true);
                    isAttack = true;
                    break;
                case STEP.DEAD:
                    animator.SetBool("dead", true);
                    animator.SetBool("moving", false);
                    animator.SetBool("attack", false);
                    isAttack = false;

                    break;
            }
        }
        // 반복 실행
        switch(this.step)
        {
            case STEP.MOVE:
                //이동(Axis ver)
                Move();
                break;
            case STEP.ATTACK:
                break;
            case STEP.DEAD:
                Invoke("Die", 1.0f);
                if (isDead && !defeat) Defeat();
                break;
        }


        // 체력, 마나 감소 및 회복 확인용
        if (Input.GetKeyDown(KeyCode.Z) && player.Hp != player.MaxHp) player.Hp += 1;
        else if(Input.GetKeyDown(KeyCode.X) && player.Hp != 0) player.Hp -= 1;
        else if (Input.GetKeyDown(KeyCode.C) && player.Mp != player.MaxMp) player.Mp += 1;
        else if (Input.GetKeyDown(KeyCode.V) && player.Mp != 0) player.Mp -= 1;

    }

    void Die()
    {
        isDead = true;
    }
    void Defeat()
    {
        panel.Show("패배");
        defeat = true;
        GameManager.IsVictory = false;
        Invoke("MoveScene", 1.0f);
    }

    void MoveScene()
    {
        GameManager.Inst.GoResultScene(false);
    }

    private void FixedUpdate()
    {
        CheckWall();
    }

    void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (hAxis == 0 && vAxis == 0) animator.SetBool("moving", false);
        else if (!isBorder && !animator.GetCurrentAnimatorStateInfo(0).IsName("nAttack"))
        {
            if (hAxis < 0) transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);

            transform.position += moveVec * player.movementspeed * Time.deltaTime;
            animator.SetBool("moving", true);
        }

        player.playerPosX_f = transform.position.x;
        player.playerPosY_f = transform.position.z;
    }

    void CheckWall()
    {
        Debug.DrawRay(transform.position, moveVec * 1, Color.blue);

        isBorder = Physics.Raycast(transform.position, moveVec, 1, LayerMask.GetMask("Wall"));
    }

    void CreateCharJSON()
    {
        string filePath = Application.dataPath + "/Data/Player.json";
        FileInfo fileInfo = new FileInfo(filePath);
        if(!fileInfo.Exists)
        {
            string playerToJsonData = playObject.GetComponent<ControlJSON>().ObjectToJson(player);      // player 클래스 내부 변수와 값들을 string으로 변환
            playObject.GetComponent<ControlJSON>().CreateJsonFile(Application.dataPath, "Player", playerToJsonData); // data 폴더에 json 생성
        }
    }

    void BringCharJSON()
    {
        string data = playObject.GetComponent<ControlJSON>().LoadJsonFile<CharClass_Playable>(Application.dataPath, "Player");
        //Debug.Log(data);
        player = JsonUtility.FromJson<CharClass_Playable>(data);
        //player = playObject.GetComponent<ControlJSON>().JsonToObjectOverwrite<CharClass_Playable>(data, player);
        //Debug.Log("Bring data from Player Json File.");
    }

    void OverwriteCharJSON()
    {
        string playerToJsonData = playObject.GetComponent<ControlJSON>().ObjectToJson(player);
        playObject.GetComponent<ControlJSON>().OverWriteJsonFile(Application.dataPath, "Player", playerToJsonData);
        //Debug.Log("Overwrite Player Json File.");
    }
}
