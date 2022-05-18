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
            Hp = 2.0f;
            Mp = 2.0f;
            Weapon = "none";
            Equipment = "none";
            for (int i = 0; i < 9; i++)
            {
                int sum = i;
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
            nAttackSpd = 6.5f;
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

    // Start is called before the first frame update
    void Start()
    {
        //최초 생성용
        //player = new CharClass_Playable();
        //player.Log();
        //CreateCharJSON();

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            //이동(Axis ver)
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");
            Move();
        }
        else
        {
            hAxis = 0f; vAxis = 0f;
        }

        //Pos_Int 변경
        if (player.playerPosX_f >= -6.5f && player.playerPosX_f < -3.75f) player.playerPosX_i = -2;
        else if(player.playerPosX_f >= -3.75f && player.playerPosX_f < -1.0f) player.playerPosX_i = -1;
        else if (player.playerPosX_f >= -1.1f && player.playerPosX_f < 1.6f) player.playerPosX_i = 0;
        else if (player.playerPosX_f >= 1.6f && player.playerPosX_f < 4.26f) player.playerPosX_i = 1;
        else if (player.playerPosX_f >= 4.26f && player.playerPosX_f <= 6.95f) player.playerPosX_i = 2;

        if (player.playerPosY_f >= 2.15f && player.playerPosY_f < 3.8f) player.playerPosY_i = 2;
        else if (player.playerPosY_f >= 0.45f && player.playerPosY_f < 2.15f) player.playerPosY_i = 1;
        else if (player.playerPosY_f >= -1.25f && player.playerPosY_f < 0.45f) player.playerPosY_i = 0;
        else if (player.playerPosY_f >= -3.0f && player.playerPosY_f < -1.25f) player.playerPosY_i = -1;

        //공격 애니메이션 실행
        if(Input.GetMouseButton(1))
        {
            animator.SetBool("attack", true);
            isAttack = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("attack", false);
            isAttack = false;
        }

        // 체력, 마나 감소 및 회복 확인용
        if (Input.GetKeyDown(KeyCode.Z) && player.Hp != player.MaxHp) player.Hp += 1;
        else if(Input.GetKeyDown(KeyCode.X) && player.Hp != 0) player.Hp -= 1;
        else if (Input.GetKeyDown(KeyCode.C) && player.Mp != player.MaxMp) player.Mp += 1;
        else if (Input.GetKeyDown(KeyCode.V) && player.Mp != 0) player.Mp -= 1;


        if (player.Hp == 0)
        {
            animator.SetBool("dead", true);
            animator.SetBool("moving", false);
            Invoke("Die", 1.0f);
        }
        if (isDead && !defeat) Defeat();
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
        string playerToJsonData = playObject.GetComponent<ControlJSON>().ObjectToJson(player);      // player 클래스 내부 변수와 값들을 string으로 변환
        playObject.GetComponent<ControlJSON>().CreateJsonFile(Application.dataPath, "Player", playerToJsonData); // data 폴더에 json 생성
        Debug.Log("Create Player Json File.");
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
