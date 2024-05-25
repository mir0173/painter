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

// Token: 0x02000004 RID: 4
public class Mainpage : MonoBehaviour
{
		// Token: 0x04000026 RID: 38
	private Touch tempTouchs;

	// Token: 0x04000027 RID: 39
	private Vector3 touchedPos;

	// Token: 0x04000028 RID: 40
	public TMP_InputField mainInputField1;

	// Token: 0x04000029 RID: 41
	public static string id;

	// Token: 0x0400002A RID: 42
	public TMP_Text text;

	// Token: 0x0400002B RID: 43
	private FirebaseApp app;

	// Token: 0x0400002C RID: 44
	// Token: 0x06000015 RID: 21 RVA: 0x00002680 File Offset: 0x00000880
	private void Start()
	{
		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
  			var dependencyStatus = task.Result;
  			if (dependencyStatus == Firebase.DependencyStatus.Available) {
    			// Create and hold a reference to your FirebaseApp,
    			// where app is a Firebase.FirebaseApp property of your application class.
    			   app = Firebase.FirebaseApp.DefaultInstance;

    			// Set a flag here to indicate whether Firebase is ready to use by your app.
  			} else {
    			UnityEngine.Debug.LogError(System.String.Format(
      			"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
    			// Firebase Unity SDK is not safe to use here.
  			}
		});
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000026A4 File Offset: 0x000008A4
	private void Update()
	{
		if (Input.touchCount > 0)
		{
			this.tempTouchs = Input.GetTouch(0);
			if (this.tempTouchs.phase == TouchPhase.Ended)
			{
				this.touchedPos = Camera.main.ScreenToWorldPoint(this.tempTouchs.position);
				if (this.touchedPos.x > -3.4f && this.touchedPos.x < 3.4f && this.touchedPos.y < -1.19f && this.touchedPos.y > -1.86f)
				{
					if (this.mainInputField1.text.Length == 4)
					{
						Mainpage.id = this.mainInputField1.text.Trim();
						this.Rogin(Mainpage.id);
						return;
					}
					base.StartCoroutine(this.nonepin());
					return;
				}
				else if (this.touchedPos.x > -1.2f && this.touchedPos.x < 1.2f && this.touchedPos.y < -3.14f && this.touchedPos.y > -3.36f)
				{
					SceneManager.LoadScene("Joinnew");
				}
			}
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000027EC File Offset: 0x000009EC
	public void Rogin(string id)
	{
		FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(delegate(Task<DataSnapshot> task)
		{
			if (!task.IsFaulted && task.IsCompleted)
			{
				using (IEnumerator<DataSnapshot> enumerator = task.Result.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((IDictionary)enumerator.Current.Value)["name"].ToString() == "/storage/emulated/0/DCIM/Painter//HandWriting/Write_" + id + ".png")
						{
							this.StartCoroutine("roginin");
							return;
						}
					}
				}
				this.StartCoroutine("wrongpin");
			}
		});
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002833 File Offset: 0x00000A33
	public IEnumerator nonepin()
	{
		this.text.text = "Please enter your ID";
		yield return null;
		yield return new WaitForSeconds(1.5f);
		this.text.text = "";
		yield break;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002842 File Offset: 0x00000A42
	public IEnumerator wrongpin()
	{
		this.text.text = "This ID does not exist";
		yield return null;
		yield return new WaitForSeconds(1.5f);
		this.text.text = "";
		yield break;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002851 File Offset: 0x00000A51
	public IEnumerator roginin()
	{
		this.text.text = "Login was successful.";
		yield return null;
		yield return new WaitForSeconds(2f);
		this.text.text = "";
		yield return null;
		SceneManager.LoadScene("Handwriting");
		yield break;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002860 File Offset: 0x00000A60
	public Mainpage()
	{
	}


}
