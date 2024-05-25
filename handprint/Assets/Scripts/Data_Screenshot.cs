using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

// Token: 0x02000002 RID: 2
public class Data_Screenshot : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static string RootPath
	{
		get
		{
			return "/storage/emulated/0/DCIM/" + Application.productName + "/";
		}
	}
	
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
	public static string FolderPath
	{
		get
		{
			return Data_Screenshot.RootPath + "/" + Data_Screenshot.folderName;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000003 RID: 3 RVA: 0x0000207C File Offset: 0x0000027C
	public static string TotalPath
	{
		get
		{
			return string.Concat(new string[]
			{
				Data_Screenshot.FolderPath,
				"/",
				Data_Screenshot.fileName,
				"_",
				Joinpage.id_d,
				".",
				Data_Screenshot.extName
			});
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020CC File Offset: 0x000002CC
	private void Awake()
	{
		Data_Screenshot.instance = this;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020D4 File Offset: 0x000002D4
	public static void TakeScreenShotWithoutUI()
	{
		Data_Screenshot.CheckAndroidPermissionAndDo("android.permission.WRITE_EXTERNAL_STORAGE", delegate
		{
			Data_Screenshot._willTakeScreenShot = true;
		});
		Data_Screenshot.instance.StartCoroutine(Data_Screenshot.CaptureScreenAndSave());
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002110 File Offset: 0x00000310
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

	// Token: 0x06000007 RID: 7 RVA: 0x0000215D File Offset: 0x0000035D
	public static IEnumerator CaptureScreenAndSave()
	{
		yield return Data_Screenshot.frameEnd;
		Data_Screenshot.totalPath = Data_Screenshot.TotalPath;
		Texture2D texture2D = new Texture2D((int)((float)Screen.width * 0.46f), (int)((float)Screen.height * 0.43f), TextureFormat.RGB24, false);
		Rect source = new Rect((float)Screen.width * 0.27f, (float)Screen.height * 0.17f, (float)Screen.width * 0.46f, (float)Screen.height * 0.43f);
		texture2D.ReadPixels(source, 0, 0);
		bool flag = true;
		try
		{
			if (!Directory.Exists(Data_Screenshot.FolderPath))
			{
				Directory.CreateDirectory(Data_Screenshot.FolderPath);
			}
			File.WriteAllBytes(Data_Screenshot.totalPath, texture2D.EncodeToPNG());
		}
		catch 
		{
			flag = false;
		}
		Destroy(texture2D);
		if (flag)
		{
			yield return new WaitForSeconds(1f);
			Data_Screenshot.lastSavedPath = Data_Screenshot.totalPath;
		}
		Data_Screenshot.RefreshAndroidGallery(Data_Screenshot.totalPath);
		yield break;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002168 File Offset: 0x00000368
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

	// Token: 0x06000009 RID: 9 RVA: 0x000021E5 File Offset: 0x000003E5
	public Data_Screenshot()
	{
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021ED File Offset: 0x000003ED
	// Note: this type is marked as 'beforefieldinit'.
	static Data_Screenshot()
	{
	}

	// Token: 0x04000001 RID: 1
	[SerializeField]
	public static MonoBehaviour instance;

	// Token: 0x04000002 RID: 2
	public static string folderName = "HandWriting";

	// Token: 0x04000003 RID: 3
	public static string fileName = "Write";

	// Token: 0x04000004 RID: 4
	public static string extName = "png";

	// Token: 0x04000005 RID: 5
	public static bool _willTakeScreenShot = false;

	// Token: 0x04000006 RID: 6
	private Texture2D _imageTexture;

	// Token: 0x04000007 RID: 7
	public static string totalPath;

	// Token: 0x04000008 RID: 8
	public static string lastSavedPath;

	// Token: 0x04000009 RID: 9
	public static WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();
}
