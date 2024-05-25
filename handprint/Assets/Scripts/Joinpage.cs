using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000003 RID: 3
public class Joinpage : MonoBehaviour
{
	// Token: 0x0400000A RID: 10
	private List<string> userlist;

	// Token: 0x0400000B RID: 11
	private float zpos = -0.3f;

	// Token: 0x0400000C RID: 12
	public Sprite pen1;

	// Token: 0x0400000D RID: 13
	public Sprite pen2;

	// Token: 0x0400000E RID: 14
	public Sprite eraser1;

	// Token: 0x0400000F RID: 15
	public Sprite eraser2;

	// Token: 0x04000010 RID: 16
	public Sprite trash1;

	// Token: 0x04000011 RID: 17
	public Sprite trash2;

	// Token: 0x04000012 RID: 18
	public Image pen;

	// Token: 0x04000013 RID: 19
	public Image eraser;

	// Token: 0x04000014 RID: 20
	public Image trash;

	// Token: 0x04000015 RID: 21
	private LineRenderer curLine;

	// Token: 0x04000016 RID: 22
	private int positionCount = 2;

	// Token: 0x04000017 RID: 23
	private Vector3 PrevPos = Vector3.zero;

	// Token: 0x04000018 RID: 24
	public Material m1;

	// Token: 0x04000019 RID: 25
	public Material m2;

	// Token: 0x0400001A RID: 26
	public Camera cam;

	// Token: 0x0400001B RID: 27
	private bool isPen;

	// Token: 0x0400001C RID: 28
	private bool isEraser;

	// Token: 0x0400001D RID: 29
	private bool isTrash;

	// Token: 0x0400001E RID: 30
	private Touch tempTouchs;

	// Token: 0x0400001F RID: 31
	private Vector3 touchedPos;

	// Token: 0x04000020 RID: 32
	public TMP_InputField mainInputField1;

	// Token: 0x04000021 RID: 33
	public static string id_d;
	public GameObject background;

	// Token: 0x04000022 RID: 34
	private string id;

	// Token: 0x04000023 RID: 35
	public TMP_Text text;

	// Token: 0x04000024 RID: 36
	private DatabaseReference reference;

	// Token: 0x04000025 RID: 37
	private FirebaseApp app;

	// Token: 0x0200000A RID: 10
	public class User
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000030B7 File Offset: 0x000012B7
		public User()
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000030BF File Offset: 0x000012BF
		public User(string name, string name2)
		{
			this.name = name;
			this.name2 = name2;
		}

		// Token: 0x04000056 RID: 86
		public string name;

		// Token: 0x04000057 RID: 87
		public string name2;
	}
	// Token: 0x0600000B RID: 11 RVA: 0x0000221D File Offset: 0x0000041D
	private void Awake()
	{
		this.isPen = true;
		this.isEraser = false;
		this.isTrash = false;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002234 File Offset: 0x00000434
	private void Start()
	{
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002238 File Offset: 0x00000438
	private void Update()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			if(Input.GetKey(KeyCode.Escape))
			{
				SceneManager.LoadScene("LoginJoin");
			}
		}
		if (Input.touchCount > 0)
		{
			this.tempTouchs = Input.GetTouch(0);
			if (this.tempTouchs.phase == TouchPhase.Began)
			{
				this.touchedPos = Camera.main.ScreenToWorldPoint(this.tempTouchs.position);
				this.touchedPos.z = this.zpos;
				this.zpos -= 0.001f;
				if (this.touchedPos.x > -3.85f && this.touchedPos.x < 3.75f && this.touchedPos.y < 0.95f && this.touchedPos.y > -3.15f)
				{
					this.createLine(this.touchedPos);
				}
				if (this.touchedPos.x > -3.4f && this.touchedPos.x < 3.4f && this.touchedPos.y < -3.8f && this.touchedPos.y > -4.5f && this.mainInputField1.text.Length == 4)
				{
					Joinpage.id_d = this.mainInputField1.text;
					Data_Screenshot.TakeScreenShotWithoutUI();
					this.writeNewUser(this.mainInputField1.text);
				}
				if (this.touchedPos.x > 4.3f && this.touchedPos.x < 4.925f && this.touchedPos.y < 1.1f && this.touchedPos.y > 0.55f)
				{
					this.isPen = true;
					this.isEraser = false;
				}
				if (this.touchedPos.x > 4.3f && this.touchedPos.x < 4.925f && this.touchedPos.y < 0.42f && this.touchedPos.y > -0.13f)
				{
					this.isPen = false;
					this.isEraser = true;
				}
				if (this.touchedPos.x > 4.3f && this.touchedPos.x < 4.925f && this.touchedPos.y < -2.54f && this.touchedPos.y > -3.09f)
				{
					Vector3 position = background.transform.position;
					position.z = zpos;
					zpos -= 0.001f;
					background.transform.position = position;
				}
			}
			else if (this.tempTouchs.phase == TouchPhase.Moved || this.tempTouchs.phase == TouchPhase.Stationary)
			{
				this.touchedPos = Camera.main.ScreenToWorldPoint(this.tempTouchs.position);
				this.touchedPos.z = this.zpos;
				this.zpos -= 0.001f;
				if (this.touchedPos.x > -3.85f && this.touchedPos.x < 3.75f && this.touchedPos.y < 0.95f && this.touchedPos.y > -3.15f)
				{
					this.connectLine(this.touchedPos);
				}
			}
		}
		if (this.isPen)
		{
			this.pen.sprite = this.pen1;
		}
		else
		{
			this.pen.sprite = this.pen2;
		}
		if (this.isEraser)
		{
			this.eraser.sprite = this.eraser1;
		}
		else
		{
			this.eraser.sprite = this.eraser2;
		}
		if (this.isTrash)
		{
			this.trash.sprite = this.trash1;
			return;
		}
		this.trash.sprite = this.trash2;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000024D4 File Offset: 0x000006D4
	private void writeNewUser(string id_val)
	{
		FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(delegate(Task<DataSnapshot> task)
		{
			if (!task.IsFaulted && task.IsCompleted)
			{
				using (IEnumerator<DataSnapshot> enumerator = task.Result.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((IDictionary)enumerator.Current.Value)["name"].ToString() == "/storage/emulated/0/DCIM/Painter//HandWriting/Write_" + id_val + ".png")
						{
							this.StartCoroutine("Noneuser");
							return;
						}
					}
				}
				string rawJsonValueAsync = JsonUtility.ToJson(new Joinpage.User("/storage/emulated/0/DCIM/Painter//HandWriting/Write_" + id_val + ".png", " "));
				FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(id_val).SetRawJsonValueAsync(rawJsonValueAsync);
				this.StartCoroutine("Plususer");
			}
		});
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000251C File Offset: 0x0000071C
	private void createLine(Vector3 Pos)
	{
		this.positionCount = 2;
		GameObject gameObject = new GameObject("Line");
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		gameObject.transform.parent = this.cam.transform;
		gameObject.transform.position = Pos;
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
		lineRenderer.numCornerVertices = 10;
		lineRenderer.numCapVertices = 10;
		lineRenderer.SetPosition(0, Pos);
		lineRenderer.SetPosition(1, Pos);
		this.curLine = lineRenderer;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000025C8 File Offset: 0x000007C8
	private void connectLine(Vector3 Pos)
	{
		Vector3 prevPos = this.PrevPos;
		if (Mathf.Abs(Vector3.Distance(this.PrevPos, Pos)) >= 0.001f)
		{
			this.PrevPos = Pos;
			this.positionCount++;
			this.curLine.positionCount = this.positionCount;
			this.curLine.SetPosition(this.positionCount - 1, Pos);
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x0000262E File Offset: 0x0000082E
	public IEnumerator nonepin()
	{
		this.text.text = "Please enter your ID";
		yield return null;
		yield return new WaitForSeconds(1.5f);
		this.text.text = "";
		yield break;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000263D File Offset: 0x0000083D
	public IEnumerator Noneuser()
	{
		this.text.text = "same ID already exist";
		yield return null;
		yield return new WaitForSeconds(1.5f);
		this.text.text = "";
		yield break;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000264C File Offset: 0x0000084C
	public IEnumerator Plususer()
	{
		this.text.text = "You have successfully registered as a member.";
		yield return null;
		yield return new WaitForSeconds(2f);
		this.text.text = "";
		yield return null;
		SceneManager.LoadScene("LoginJoin");
		yield break;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000265B File Offset: 0x0000085B
	public Joinpage()
	{
	}

	
}
