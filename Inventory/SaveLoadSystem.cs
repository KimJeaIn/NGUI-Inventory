using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SaveLoadSystem : MonoBehaviour {

	static private SaveLoadSystem m_instance;
	static public SaveLoadSystem Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = FindObjectOfType<SaveLoadSystem>();
				
				if(m_instance == null)
				{
					Debug.LogError("Not Find InventoryEvent!!");
				}
			}
			
			return m_instance;
		}
	}

	static private string SaveFile = "NGUI_Save.sav";

	public void SaveStart()
	{
		StartCoroutine ("FileSave");
	}

	IEnumerator FileSave()
	{
		string strPath = string.Empty;

		strPath += (Application.dataPath + "/" + SaveFile);
		
		FileStream file = new FileStream (strPath, FileMode.Create, FileAccess.Write);
		
		StreamWriter sw = new StreamWriter( file );

		string savestr = "";

		// 인벤토리 세이브
		savestr = savestr + "i" + '\n';
		ItemIconInfo[] list = InventoryEvent.Instance.ItemIconList;

		for(int i = 0; i < list.Length; i++)
		{
			if(list[i].GetInfo() != null)
				savestr += list[i].GetInfo().ID;
			else
				savestr += '0';

			if(i != list.Length - 1)
				savestr += '/';
		}

		// 장비창 세이브
		savestr = savestr + '\n' + "e" + '\n';
		if (EquipmentManager.Instance.HeadEquip.GetInfo () != null)
			savestr = savestr + EquipmentManager.Instance.HeadEquip.GetInfo ().ID + '/';
		else
			savestr += '0';

		if(EquipmentManager.Instance.R_HandEquip.GetInfo() != null)
			savestr = savestr + EquipmentManager.Instance.R_HandEquip.GetInfo().ID + '/';
		else
			savestr += '0';

		if(EquipmentManager.Instance.L_HandEquip.GetInfo() != null)
			savestr = savestr + EquipmentManager.Instance.L_HandEquip.GetInfo().ID;
		else
			savestr += '0';

		// 골드 세이브
		savestr = savestr + '\n' + "g" + '\n' + PlayerInfomation.Instance.GOLD;

		sw.WriteLine(savestr);
		
		sw.Close();
		file.Close();

		yield return 0;
	}

	public void LoadStart()
	{
		StartCoroutine ("FileLoad");
	}

	IEnumerator FileLoad()
	{
		string strPath = string.Empty;

		strPath += (Application.dataPath + "/" + SaveFile);
		
		if (File.Exists(strPath))
		{
			string data = File.ReadAllText(strPath);
			StringReader rd = new StringReader(data);
			
			Debug.Log(data);

			yield return StartCoroutine("DataParse", rd);
			
			rd.Close();
		}
	}
	IEnumerator DataParse(StringReader rd)
	{
		string line;
		string[] data;
		int num;
		while((line = rd.ReadLine()) != null)
		{
			switch(line[0])
			{
			// 인벤토리 불러오기
			case 'i':
				data = rd.ReadLine().Split('/');

				for(int i = 0; i < data.Length; i++)
				{
					num = int.Parse(data[i]);
					if(num != 0)
						InventoryEvent.Instance.SlotAddItemInven(i, ItemManager.Instance.GetItem(num));	
					else
						InventoryEvent.Instance.DeleteItemInven(i);
				}

				break;
			// 장비 불러오기
			case 'e':
				data = rd.ReadLine().Split('/');
				
				for(int i = 0; i < data.Length; i++)
				{
					num = int.Parse(data[i]);
					switch(i)
					{
					case 0:
						if(num == 0)
							EquipmentManager.Instance.HeadEquip.EquipSetting(null);
						else
							EquipmentManager.Instance.HeadEquip.EquipSetting(ItemManager.Instance.GetItem(num));

						break;
					case 1:
						if(num == 0)
							EquipmentManager.Instance.R_HandEquip.EquipSetting(null);
						else
							EquipmentManager.Instance.R_HandEquip.EquipSetting(ItemManager.Instance.GetItem(num));

						break;
					case 2:
						if(num == 0)
							EquipmentManager.Instance.L_HandEquip.EquipSetting(null);
						else
							EquipmentManager.Instance.L_HandEquip.EquipSetting(ItemManager.Instance.GetItem(num));
						break;
					}
				}

				break;
			// 골드 불러오기
			case 'g':
				num = int.Parse(rd.ReadLine());
				PlayerInfomation.Instance.GOLD = num;
				PlayerInfomation.Instance.GoldUpdate(0);
				break;
			}
		}

		yield return 0;
	}
}
