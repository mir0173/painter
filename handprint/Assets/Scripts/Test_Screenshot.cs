using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

// Token: 0x02000005 RID: 5
public class Test_Screenshot : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600001D RID: 29 RVA: 0x000028A0 File Offset: 0x00000AA0
	public static string RootPath
	{
		get
		{
			return "/storage/emulated/0/DCIM/" + Application.productName + "/";
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600001E RID: 30 RVA: 0x000028B6 File Offset: 0x00000AB6
	public static string FolderPath
	{
		get
		{
			return Test_Screenshot.RootPath + "/" + Test_Screenshot.folderName;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600001F RID: 31 RVA: 0x000028CC File Offset: 0x00000ACC
	public static string TotalPath
	{
		get
		{
			return string.Concat(new string[]
			{
				Test_Screenshot.FolderPath,
				"/",
				Test_Screenshot.fileName,
				"_",
				DateTime.Now.ToString("MMdd_HHmmss"),
				".",
				Test_Screenshot.extName
			});
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002929 File Offset: 0x00000B29
	private void Awake()
	{
		Test_Screenshot.textfield_text = this.textfield_text1;
		Test_Screenshot.textfield_text.text = "";
		Test_Screenshot.instance = this;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000294B File Offset: 0x00000B4B
	public static void TakeScreenShotWithoutUI()
	{
		Test_Screenshot.CheckAndroidPermissionAndDo("android.permission.WRITE_EXTERNAL_STORAGE", delegate
		{
			Test_Screenshot._willTakeScreenShot = true;
		});
		Test_Screenshot.instance.StartCoroutine(Test_Screenshot.CaptureScreenAndSave());
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002988 File Offset: 0x00000B88
	public static void CheckAndroidPermissionAndDo(string permission, Action actionIfPermissionGranted)
	{
		if (!Permission.HasUserAuthorizedPermission(permission))
		{
			PermissionCallbacks permissionCallbacks = new PermissionCallbacks();
			permissionCallbacks.PermissionGranted += delegate(string _)
			{
				actionIfPermissionGranted();
			};
			Permission.RequestUserPermission(permission, permissionCallbacks);
			return;
		}
		actionIfPermissionGranted();
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000029D5 File Offset: 0x00000BD5
	public static IEnumerator CaptureScreenAndSave()
	{
		yield return Test_Screenshot.frameEnd;
		string totalPath = Test_Screenshot.TotalPath;
		string rawJsonValueAsync = JsonUtility.ToJson(new Test_Screenshot.User("/storage/emulated/0/DCIM/Painter//HandWriting/Write_" + Mainpage.id + ".png", totalPath));
		FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(Mainpage.id).SetRawJsonValueAsync(rawJsonValueAsync);
		Texture2D texture2D = new Texture2D((int)((float)Screen.width * 0.9f), Screen.height, TextureFormat.RGB24, false);
		Rect source = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		texture2D.ReadPixels(source, 0, 0);
		bool flag = true;
		try
		{
			if (!Directory.Exists(Test_Screenshot.FolderPath))
			{
				Directory.CreateDirectory(Test_Screenshot.FolderPath);
			}
			File.WriteAllBytes(totalPath, texture2D.EncodeToPNG());
		}
		catch 
		{
			flag = false;
		}
		Destroy(texture2D);
		if (flag)
		{
			Test_Screenshot.textfield_text.text = "Screen Shot Saved : " + totalPath;
			yield return new WaitForSeconds(1f);
			Test_Screenshot.textfield_text.text = "";
			Test_Screenshot.lastSavedPath = totalPath;
		}
		Test_Screenshot.RefreshAndroidGallery(totalPath);
		SceneManager.LoadScene("LoginJoin");
		yield break;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000029E0 File Offset: 0x00000BE0
	[System.Diagnostics.Conditional("UNITY_ANDROID")]
    public static void RefreshAndroidGallery(string imageFilePath)
    {
#if !UNITY_EDITOR
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2]
        { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + imageFilePath) });
        objActivity.Call("sendBroadcast", objIntent);
#endif
    }

	// Token: 0x06000025 RID: 37 RVA: 0x00002A5D File Offset: 0x00000C5D
	public Test_Screenshot()
	{
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002A65 File Offset: 0x00000C65
	// Note: this type is marked as 'beforefieldinit'.
	static Test_Screenshot()
	{
	}

	// Token: 0x0400002D RID: 45
	[SerializeField]
	private TMP_Text textfield_text1;

	// Token: 0x0400002E RID: 46
	public static TMP_Text textfield_text;

	// Token: 0x0400002F RID: 47
	public static MonoBehaviour instance;

	// Token: 0x04000030 RID: 48
	public static string folderName = "HandWriting";

	// Token: 0x04000031 RID: 49
	public static string fileName = "Write";

	// Token: 0x04000032 RID: 50
	public static string extName = "png";

	// Token: 0x04000033 RID: 51
	public static bool _willTakeScreenShot = false;

	// Token: 0x04000034 RID: 52
	private Texture2D _imageTexture;

	// Token: 0x04000035 RID: 53
	public static string lastSavedPath;

	// Token: 0x04000036 RID: 54
	public static WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

	// Token: 0x02000013 RID: 19
	public class User
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00003746 File Offset: 0x00001946
		public User()
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000374E File Offset: 0x0000194E
		public User(string name, string name2)
		{
			this.name = name;
			this.name2 = name2;
		}

		// Token: 0x0400006E RID: 110
		public string name;

		// Token: 0x0400006F RID: 111
		public string name2;
	}
}
