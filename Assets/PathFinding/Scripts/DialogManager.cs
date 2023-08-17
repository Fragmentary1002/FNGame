using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Security.Cryptography;

public class DialogManager : MonoBehaviour
{
    //对话文本文件，csv格式
    public TextAsset dialogDataFile;
    //NPC图片
    public Image NPC;
    //玩家图片
    public Image Player;
    //角色名字文本
    public TMP_Text nameText;
    //对话内容文本
    public TMP_Text dialogText;
    //角色图像列表
    public List<Image> Images = new List<Image>();
    //角色名字对应图片的字典
    Dictionary<string, Image> imageDic = new Dictionary<string, Image>();
    //当前对话索引值
    public int dialogIndex;
    //对话文本，按行分割
    public string[] dialogRows;
    //对话继续按钮
    public Button nextButton;
    //选项按钮预制体
    public GameObject OptionButton;
    //选项按钮父节点，用于自动排列
    public Transform buttonGroup;

    public GameObject player;
    public GameObject npc;
    public GameObject dialogPanel;
    public float interactionDistance = 3f;
    public bool canInteract = true;
    public bool isReturn = true;

    public int lastIndex;
    public GameObject victoryMenu;
    public GameObject failureMenu;
    //public bool isWin;

    // Start is called before the first frame update

    private void Awake()
    {
        imageDic["患者"] = Images[0];
        imageDic["中医医生"] = Images[1];
    }

    void Start()
    {
        dialogIndex = 0;
        ReadText(dialogDataFile);
        ShowDialogRow();
        /*UpdateText("npc", "gogo");
        UpdateImage("NPC", false);*/
        victoryMenu.SetActive(false);
        failureMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distans = Vector3.Distance(player.transform.position, npc.transform.position);
        if (distans <= interactionDistance && canInteract)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                dialogPanel.SetActive(true);

            }
            if (Input.GetKeyDown(KeyCode.Return) && isReturn)
            {
                // 触发 Next 按钮点击事件
                nextButton.onClick.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buttonGroup.GetChild(0).GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buttonGroup.GetChild(1).GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buttonGroup.GetChild(2).GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buttonGroup.GetChild(3).GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                buttonGroup.GetChild(4).GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                buttonGroup.GetChild(5).GetComponent<Button>().onClick.Invoke();
            }
        }
        if (!canInteract || distans > interactionDistance)
        {
            dialogPanel.SetActive(false);

        }
        if (dialogIndex == 23 || dialogIndex == 26)
        {
            lastIndex = dialogIndex;
        }
    }

    public void UpdateText(string _name, string _text)
    {
        nameText.text = _name;
        dialogText.text = _text;
    }

    public void UpdateImage(string _name, string _position)
    {
        if (_position == "右")
        {
            NPC.enabled = true;
            Player.enabled = false;
            NPC = imageDic[_name];
        }
        else if (_position == "左")
        {
            Player.enabled = true;
            NPC.enabled = false;
            Player = imageDic[_name];
        }
        else
        {
            Player.enabled = true;
            NPC.enabled = true;
            NPC = imageDic["患者"];
            Player = imageDic["中医医生"];
        }
    }
    public void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');
        /*foreach(var row in rows)
        {
            string[] cell=row.Split(',');
        }*/
        Debug.Log("ok");
    }

    public void ShowDialogRow()
    {
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {

                UpdateText(cells[2], cells[4]);
                UpdateImage(cells[2], cells[3]);

                dialogIndex = int.Parse(cells[5]);
                nextButton.gameObject.SetActive(true);
                isReturn = true;
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                nextButton.gameObject.SetActive(false);
                GenerateOption(i);
                isReturn = false;
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {

                Debug.Log("结束");
                canInteract = false;
                if (lastIndex == 23)
                {
                    failureMenu.SetActive(true);
                }
                else if (lastIndex == 26)
                {
                    victoryMenu.SetActive(true);
                }
            }
        }
    }

    public void OnClickNext()
    {
        ShowDialogRow();
    }

    public void GenerateOption(int _index)
    {

        string[] cells = dialogRows[_index].Split(',');
        if (cells[0] == "&")
        {
            GameObject button = Instantiate(OptionButton, buttonGroup);
            button.GetComponentInChildren<TMP_Text>().text = cells[4];
            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                OnOptionClick(int.Parse(cells[5]));
            });
            GenerateOption(_index + 1);
        }
    }

    public void OnOptionClick(int _id)
    {
        dialogIndex = _id;

        ShowDialogRow();
        for (int i = 0; i < buttonGroup.childCount; i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
        }
        GenerateOption(dialogIndex);
    }

    public void CloseResultPanel()
    {
        victoryMenu.SetActive(false);
        failureMenu.SetActive(false);
    }

}
