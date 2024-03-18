using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diamor : MonoBehaviour
{
    public TextAsset dialogDataFile;//�Ի��ı��ļ�  //�ı��ļ���Դ
    public SpriteRenderer spriteLeft;//���ͼ��
    public SpriteRenderer spriteRight;//�Ҳ�ͼ��
    public TMP_Text nameText;//�����ı�
    public TMP_Text dialogText;//�����ı�
    public int dialogIndex=0;//�Ի�����ֵ
    public string[] dialogRows;//�Ի��ı����зָ�
    public Button NextButton;//������ť
    public GameObject optionButton;//ѡ�ťԤ����
    public Transform buttonGroup;//ѡ�ť���ڵ������Զ�����
    public List<Person> people = new List<Person>();//������
    public List<Sprite> sprites = new List<Sprite>();//��ɫͼƬ�б�(������ͼƬ)
    public GameObject ui;//����
    Dictionary<string, Sprite> ImageDic = new Dictionary<string, Sprite>();//��ɫ���ֶ�ӦͼƬ���ֵ�
    private void Awake()
    {

        ImageDic["��������"] = sprites[0];//����
        ImageDic["����"] = sprites[1];
        Person person = new Person();
        person.name = "��������";
        people.Add(person);
        Person doctor = new Person();
        doctor.name = "��������";
        people.Add(doctor);
    }
    private void Start()
    {
        //UpdateText("��ʿ","�������޵µ�");
        ReadText(dialogDataFile);//��ȡ�ı��ļ�
        ShowDialogRow();//��ʾ�еĽű��ָ�
    }
    

    public void UpdateText(string name, string Text)//�����ı�����
    {
        nameText.text = name;//��������
        dialogText.text = Text;//��������
    }
    public void UpdateImage(string name, string atLeft)//����ͼƬ
    {
        if (atLeft=="��")
        {
            spriteLeft.sprite = ImageDic[name]; //Ҫ��Ⱦ�Ķ���  ����
            
            spriteRight.sprite = null;
        }
        else if(atLeft=="��")
        {
            spriteRight.sprite = ImageDic[name];//����
            spriteLeft.sprite = null;
        }
    }

    public void ReadText(TextAsset textAsset)//��ȡExcel�ļ�
    {
        dialogRows = textAsset.text.Split('\n');//��ȡÿ������

        //Debug.Log("1");
    }
    public void ShowDialogRow()//��ʾ�еĽű��ָ�
    {
       
        for (int i = 0; i < dialogRows.Length; i++)//ͨ����������ѭ��
        {
            string[] cells = dialogRows[i].Split(',');//�����зָ


            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                Debug.Log("ִ����");
                UpdateText(cells[2], cells[4]);//�����ı�����
                UpdateImage(cells[2], cells[3]);//������ʾͼƬ�ķ���
                NextButton.gameObject.SetActive(true);//������һ����ť����ʾ
                dialogIndex = int.Parse(cells[5]);//����������
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                NextButton.gameObject.SetActive(false);//������һ����ť����ʾ
                Generate0ption(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1])==dialogIndex)
            {
                Debug.Log("������");
                ui.gameObject.SetActive(false);
                spriteRight.sprite = null;
                spriteLeft.sprite = null;
            }
        }
    }
    public void OnClickNext()//������ťִ�еķ���
    {
        ShowDialogRow();
        
    }
    public void Generate0ption(int index)//����ѡ��
    {
        
        string[] cells = dialogRows[index].Split(',');
        if (cells[0] =="&")//&��ѡ��ı�ʶ����������ݹ��������
        {
            
            GameObject button = Instantiate(optionButton, buttonGroup.transform);//���ɰ�ť
            button.GetComponentInChildren<Text>().text=cells[4];//���ҵ��������������������ı�����  //������û��text���
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
                }//Ϊÿ�����ɵİ�ť���Ҫִ�е��¼�  //���¼� //ί��
                           
                );
            Generate0ption(index+1);//�ݹ����
        }
    }
    public void On0ptionClick(int id)//�¼�
    {
        Debug.Log(id);
        dialogIndex = id;//��������ֵ
        ShowDialogRow();
        for (int i = 0; i < buttonGroup.childCount; i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);//ִ�����¼��󣬴�ѭ�������������ɵİ�ť
        }
    }

    public void OptionEffect(string effect,int param,string target)//�øж�   ����   Ŀ��
    {
        if (effect=="�øжȼ�")
        {
            foreach (var cell in people)
            {
                if (cell.name==target)
                {
                    cell.likeValue += param;
                }
            }

        }
        else if(effect=="����ֵ��")
        {
            foreach (var cell in people)
            {
                if (cell.name == target)//�����Ķ����Ƿ�Ϊ����Ķ���
                {
                    cell.strengthValue += param;
                }
            }
        }
    }
}
