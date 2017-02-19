using UnityEngine;
using System.Collections;

public class ItemDragEvent : MonoBehaviour {
	
	public UISprite IconSprite;
	public bool IsEquip = false;
	private ItemIconInfo m_info;

	private Vector3 m_startPosition;
	private Transform m_myPos;
	private bool m_startDrag = false;

	// 초기 위치를 기억했다가 필요없을시 복귀한다
	void Awake()
	{
		m_myPos = transform;
		m_startPosition = m_myPos.position;
	}
	// 현재 드래그중인지 확인
	public bool IsDrag
	{
		get
		{
			return m_startDrag;
		}
	}
	// 드래그가 시작되면 드래그 아이콘이 바뀌며 마우스에 붙는다
	public void StartDrag(ItemIconInfo info, Transform pos)
	{
		m_info = info;
		IconSprite.spriteName = m_info.GetInfo().ICON;

		m_myPos.position = pos.position;

		m_startDrag = true;
	}
	public void EndDrag()
	{
		m_startDrag = false;
	}
	public void DeleteIcon()
	{
		m_info = null;
	}

	public ItemIconInfo GetLastInfo() { return m_info; }

	// 드래그중에 마우스에 붙게끔 업데이트
	void Update ()
	{
		if(m_startDrag)
			m_myPos.position = UICamera.currentCamera.ScreenToWorldPoint (Input.mousePosition);
		else
		{
			if(m_myPos.position != m_startPosition)
				m_myPos.position = m_startPosition;
		}
	}
}
