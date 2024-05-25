using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class UIclick : MonoBehaviour
{
		// Token: 0x04000037 RID: 55
	private Touch tempTouchs;

	// Token: 0x04000038 RID: 56
	private Vector3 touchedPos;

	// Token: 0x04000039 RID: 57
	private bool isPen;

	// Token: 0x0400003A RID: 58
	private bool isEraser;

	// Token: 0x0400003B RID: 59
	private bool isTrash;

	// Token: 0x0400003C RID: 60
	private bool isCheck;

	// Token: 0x0400003D RID: 61
	public Sprite pen1;

	// Token: 0x0400003E RID: 62
	private float zpos = -0.3f;

	// Token: 0x0400003F RID: 63
	public Sprite pen2;

	// Token: 0x04000040 RID: 64
	public Sprite eraser1;

	// Token: 0x04000041 RID: 65
	public Sprite eraser2;

	// Token: 0x04000042 RID: 66
	public Sprite trash1;

	// Token: 0x04000043 RID: 67
	public Sprite trash2;

	// Token: 0x04000044 RID: 68
	public Sprite check1;

	// Token: 0x04000045 RID: 69
	public Sprite check2;

	// Token: 0x04000046 RID: 70
	public Image pen;

	// Token: 0x04000047 RID: 71
	public Image eraser;

	// Token: 0x04000048 RID: 72
	public Image trash;

	// Token: 0x04000049 RID: 73
	public Image check;

	// Token: 0x0400004A RID: 74
	private LineRenderer curLine;

	// Token: 0x0400004B RID: 75
	private int positionCount = 2;

	// Token: 0x0400004C RID: 76
	private Vector3 PrevPos = Vector3.zero;

	// Token: 0x0400004D RID: 77
	public Material m1;

	// Token: 0x0400004E RID: 78
	public Material m2;

	// Token: 0x0400004F RID: 79
	public Camera cam;

	// Token: 0x04000050 RID: 80
	public GameObject background;
	// Token: 0x06000027 RID: 39 RVA: 0x00002A95 File Offset: 0x00000C95
	private void Awake()
	{
		isPen = true;
		isEraser = false;
		isTrash = false;
		isCheck = false;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002AB4 File Offset: 0x00000CB4
	private void Update()
	{
		if (Input.touchCount > 0)
		{
			tempTouchs = Input.GetTouch(0);
			if (tempTouchs.phase == TouchPhase.Began)
			{
				touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);
				touchedPos.z = zpos;
				zpos -= 0.001f;
				if (touchedPos.x > 7.00f && touchedPos.x < 8.00)
				{
					if (touchedPos.y > 3.64f && touchedPos.y < 4.54f)
					{
						if (!isPen)
						{
							isPen = true;
							isEraser = false;
						}
					}
					else if (touchedPos.y > 2.47f && touchedPos.y < 3.36f)
					{
						if (!isEraser)
						{
							isEraser = true;
							isPen = false;
						}
					}
					else if (touchedPos.y > 1.18f && touchedPos.y < 2.21f)
					{
						DeleteAll();
					}
					else if (touchedPos.y > -4.58f && touchedPos.y < -3.74f)
					{
						Enter();
					}
				}
				else if (touchedPos.x < 6.8f)
				{
					createLine(touchedPos);
				}
			}
			else if (tempTouchs.phase == TouchPhase.Moved || tempTouchs.phase == TouchPhase.Stationary)
			{
				touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);
				touchedPos.z = zpos;
				zpos -= 0.001f;
				if (touchedPos.x < 6.8f)
				{
					connectLine(touchedPos);
				}
			}
		}
		if (isPen)
		{
			pen.sprite = pen1;
		}
		else
		{
			pen.sprite = pen2;
		}
		if (isEraser)
		{
			eraser.sprite = eraser1;
		}
		else
		{
			eraser.sprite = eraser2;
		}
		if (isTrash)
		{
			trash.sprite = trash1;
		}
		else
		{
			trash.sprite = trash2;
		}
		if (isCheck)
		{
			check.sprite = check1;
			return;
		}
		check.sprite = check2;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002D8C File Offset: 0x00000F8C
	private void createLine(Vector3 Pos)
	{
		positionCount = 2;
		GameObject gameObject = new GameObject("Line");
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		gameObject.transform.parent = cam.transform;
		gameObject.transform.position = Pos;
		lineRenderer.numCornerVertices = 10;
		lineRenderer.numCapVertices = 10;
		if (isPen)
		{
			lineRenderer.material = m1;
			lineRenderer.startWidth = 0.075f;
			lineRenderer.endWidth = 0.075f;
		}
		else if (isEraser)
		{
			lineRenderer.material = m2;
			lineRenderer.startWidth = 0.5f;
			lineRenderer.endWidth = 0.5f;
		}
		lineRenderer.SetPosition(0, Pos);
		lineRenderer.SetPosition(1, Pos);
		curLine = lineRenderer;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002E38 File Offset: 0x00001038
	private void connectLine(Vector3 Pos)
	{
		Vector3 prevPos = PrevPos;
		if (Mathf.Abs(Vector3.Distance(PrevPos, Pos)) >= 0.001f)
		{
			PrevPos = Pos;
			positionCount++;
			curLine.positionCount = positionCount;
			curLine.SetPosition(positionCount - 1, Pos);
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002EA0 File Offset: 0x000010A0
	private void DeleteAll()
	{
		Vector3 position = background.transform.position;
		position.z = zpos;
		zpos -= 0.001f;
		background.transform.position = position;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002EEE File Offset: 0x000010EE
	private void Enter()
	{
		Test_Screenshot.TakeScreenShotWithoutUI();
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002EFF File Offset: 0x000010FF
	public UIclick()
	{
	}


}
