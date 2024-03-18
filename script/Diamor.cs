using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diamor : MonoBehaviour
{
    public TextAsset dialogDataFile;//对话文本文件  //文本文件资源
    public SpriteRenderer spriteLeft;//左侧图像
    public SpriteRenderer spriteRight;//右侧图像
    public TMP_Text nameText;//名称文本
    public TMP_Text dialogText;//内容文本
    public int dialogIndex=0;//对话索引值
    public string[] dialogRows;//对话文本按行分割
    public Button NextButton;//关联按钮
    public GameObject optionButton;//选项按钮预制体
    public Transform buttonGroup;//选项按钮父节点用于自动排列
    public List<Person> people = new List<Person>();//基础类
    public List<Sprite> sprites = new List<Sprite>();//角色图片列表(单纯的图片)
    public GameObject ui;//画布
    Dictionary<string, Sprite> ImageDic = new Dictionary<string, Sprite>();//角色名字对应图片的字典
    private void Awake()
    {

        ImageDic["吉吉国王"] = sprites[0];//吉吉
        ImageDic["姜狗"] = sprites[1];
        Person person = new Person();
        person.name = "吉吉国王";
        people.Add(person);
        Person doctor = new Person();
        doctor.name = "吉吉国王";
        people.Add(doctor);
    }
    private void Start()
    {
        //UpdateText("博士","这里是罗德岛");
        ReadText(dialogDataFile);//读取文本文件
        ShowDialogRow();//显示行的脚本分割
    }
    

    public void UpdateText(string name, string Text)//更新文本内容
    {
        nameText.text = name;//更新名字
        dialogText.text = Text;//更新内容
    }
    public void UpdateImage(string name, string atLeft)//更新图片
    {
        if (atLeft=="左")
        {
            spriteLeft.sprite = ImageDic[name]; //要渲染的东西  吉吉
            
            spriteRight.sprite = null;
        }
        else if(atLeft=="右")
        {
            spriteRight.sprite = ImageDic[name];//跟腱
            spriteLeft.sprite = null;
        }
    }

    public void ReadText(TextAsset textAsset)//读取Excel文件
    {
        dialogRows = textAsset.text.Split('\n');//读取每行数据

        //Debug.Log("1");
    }
    public void ShowDialogRow()//显示行的脚本分割
    {
       
        for (int i = 0; i < dialogRows.Length; i++)//通过行数进行循环
        {
            string[] cells = dialogRows[i].Split(',');//将单行分割开


            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                Debug.Log("执行了");
                UpdateText(cells[2], cells[4]);//更新文本内容
                UpdateImage(cells[2], cells[3]);//用于显示图片的方向
                NextButton.gameObject.SetActive(true);//设置下一步按钮的显示
                dialogIndex = int.Parse(cells[5]);//设置新索引
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                NextButton.gameObject.SetActive(false);//设置下一步按钮的显示
                Generate0ption(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1])==dialogIndex)
            {
                Debug.Log("结束了");
                ui.gameObject.SetActive(false);
                spriteRight.sprite = null;
                spriteLeft.sprite = null;
            }
        }
    }
    public void OnClickNext()//单机按钮执行的方法
    {
        ShowDialogRow();
        
    }
    public void Generate0ption(int index)//生成选项
    {
        
        string[] cells = dialogRows[index].Split(',');
        if (cells[0] =="&")//&是选项的标识符，在这里递归调用生成
        {
            
            GameObject button = Instantiate(optionButton, buttonGroup.transform);//生成按钮
            button.GetComponentInChildren<Text>().text=cells[4];//查找到这个创建的组件，设置文本内容  //查找有没有text组件
            button.GetComponent<Button>().onClick.AddListener
                (
                delegate 
                {
                    On0ptionClick(int.Parse( cells[5])); 
                    if (cells[6]!=null)
                    {
                        string[] strings = cells[6].Split("@");
                        OptionEffect(strings[0], int.Parse(strings[1]), cells[7]);
                    }
                }//为每个生成的按钮添加要执行的事件  //绑定事件 //委托
                           
                );
            Generate0ption(index+1);//递归调用
        }
    }
    public void On0ptionClick(int id)//事件
    {
        Debug.Log(id);
        dialogIndex = id;//设置索引值
        ShowDialogRow();
        for (int i = 0; i < buttonGroup.childCount; i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);//执行完事件后，此循环用于销毁生成的按钮
        }
    }

    public void OptionEffect(string effect,int param,string target)//好感度   数量   目标
    {
        if (effect=="好感度加")
        {
            foreach (var cell in people)
            {
                if (cell.name==target)
                {
                    cell.likeValue += param;
                }
            }

        }
        else if(effect=="体力值加")
        {
            foreach (var cell in people)
            {
                if (cell.name == target)//创建的对象是否为传入的对象
                {
                    cell.strengthValue += param;
                }
            }
        }
    }
}
