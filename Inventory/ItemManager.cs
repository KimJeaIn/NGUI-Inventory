using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour 
{
	static private ItemManager m_instance;
	static public ItemManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = FindObjectOfType<ItemManager>();

				if(m_instance == null)
				{
					GameObject obj = new GameObject();

					m_instance = obj.AddComponent<ItemManager>();

					obj.name = "(Singleton)" + typeof(ItemManager).ToString();

					DontDestroyOnLoad(obj);
				}
			}

			return m_instance;
		}
	}

	// 아이템 정보 저장
	Dictionary<int, ItemInfo> m_itemData = new Dictionary<int, ItemInfo> ();

	private bool m_isItemLoading = false;

	public bool IsItemLoading
	{
		set
		{
			m_isItemLoading = value;
		}
		get
		{
			return m_isItemLoading;
		}
	}

	// 외부에서 아이템 정보 넣기
	public void AddItem(ItemInfo info)
	{
		if (m_itemData.ContainsKey (info.ID))
			return;

		m_itemData.Add (info.ID, info);
	}

	// 외부에서 아이템 정보 가져오기
	public ItemInfo GetItem(int id)
	{
		if (m_itemData.ContainsKey (id))
			return m_itemData [id];
		
		return null;
	}

	// 전체 리스트 얻기
	public Dictionary<int, ItemInfo> GetAllItems()
	{
		return m_itemData;
	}

	// 전체 갯수 얻기
	public int GetItemsCount()
	{
		return m_itemData.Count;
	}
}

public class ItemInfo
{
	//sell_cost="100" buy_cost="200" icon="cake" name="Cake" id="1"/>

	public enum Equip {Null, Head, R_Hand, L_Hand};
	
	private int m_id = 0;
	private string m_itemName;
	private string m_iconName;
	private int m_buyCost;
	private int m_sellCost;
	private Equip m_equip = Equip.Null;

	public Equip EQUIP
	{
		get { return m_equip; }
		set { m_equip = value; }
	}	
	public int ID
	{
		get { return m_id; }
		set { m_id = value; }
	}
	public string NAME
	{
		get { return m_itemName; }
		set { m_itemName = value; }
	}
	public string ICON
	{
		get { return m_iconName; }
		set { m_iconName = value; }
	}
	public int BUY
	{
		get { return m_buyCost; }
		set { m_buyCost = value; }
	}
	public int SELL
	{
		get { return m_sellCost; }
		set { m_sellCost = value; }
	}
}