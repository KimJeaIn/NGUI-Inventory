using UnityEditor;
using UnityEngine;
using System.Collections;

public class ItemIconInfo : MonoBehaviour {
	
	public UISprite IconSprite;
	protected ItemInfo m_info;

	protected Color m_startColor;

	protected bool IsEquip = false;

	// 아이템이 안보이면 투명도를 0으로 만든다
	void Awake()
	{
		m_startColor = IconSprite.color;
		IconSprite.color = new Color(1f, 1f, 1f, 0f);
	}

	// 아이템 셋팅하는 함수
	public void SetItemIcon(ItemInfo info, float delay = 0f)
	{
		if(delay != 0)
		{
			StartCoroutine (SetItemEvent(info, delay));

			return;
		}

		m_info = info;
		
		if(info != null)
		{		
			IconSprite.color = m_startColor;
			
			IconSprite.spriteName = info.ICON;
		}
		else
			IconSprite.color = new Color(1f, 1f, 1f, 0f);
	}
	// 아이템 셋팅시 딜레이가 필요할때 쓰인다
	IEnumerator SetItemEvent(ItemInfo info, float delay = 0f)
	{
		m_info = info;

		yield return new WaitForSeconds (delay);
		
		if(info != null)
		{		
			IconSprite.color = m_startColor;
			
			IconSprite.spriteName = info.ICON;
		}
		else
			IconSprite.color = new Color(1f, 1f, 1f, 0f);

		yield return 0;
	}
	// 아이템끼리 드래그해서 교체시 쓰인다
	public void SwapItemIcon(ItemIconInfo icon)
	{
		ItemInfo temp_info = m_info;

		SetItemIcon(icon.GetInfo());

		icon.SetItemIcon (temp_info);
	}
	public void DeleteItemIcon()
	{
		IconSprite.color = new Color(1f, 1f, 1f, 0f);
		m_info = null;
	}
	public ItemInfo GetInfo(){ return m_info; }

	// 아이템 드래그 앤 드롭
	void OnPress(bool isDown)
	{
		if(isDown)
		{
			if(m_info != null)
			{
				InventoryEvent.Instance.InvenDrag.StartDrag(this, transform);
				// 현재 아이템이 장비품인지 확인
				InventoryEvent.Instance.InvenDrag.IsEquip = IsEquip;
			}
			else
				InventoryEvent.Instance.InvenDrag.DeleteIcon();
		}
		else
		{
			InventoryEvent.Instance.InvenDrag.EndDrag();
		}
	}
	// EquipEvent에서 ItemIconInfo을 상속받기때문에 virtual로 생성했다
	virtual protected void OnDrop (GameObject go)
	{
		if (InventoryEvent.Instance.InvenDrag.GetLastInfo () != null)
		{
			if(!InventoryEvent.Instance.InvenDrag.IsEquip)
			{
				SwapItemIcon (InventoryEvent.Instance.InvenDrag.GetLastInfo ());
				InventoryEvent.Instance.InvenDrag.DeleteIcon();
			}
			else
			{
				if(m_info != null)
				{
					if(InventoryEvent.Instance.InvenDrag.GetLastInfo ().GetInfo().EQUIP == m_info.EQUIP)
					{
						SwapItemIcon (InventoryEvent.Instance.InvenDrag.GetLastInfo ());
						InventoryEvent.Instance.InvenDrag.DeleteIcon();

						EquipmentManager.Instance.EquipModelSwap(m_info.ID, m_info.EQUIP);
					}
				}
				else
				{
					EquipmentManager.Instance.EquipModelSwap(0, InventoryEvent.Instance.InvenDrag.GetLastInfo ().GetInfo().EQUIP);
					SwapItemIcon (InventoryEvent.Instance.InvenDrag.GetLastInfo ());

					InventoryEvent.Instance.InvenDrag.DeleteIcon();
				}
			}
		}
	}
}