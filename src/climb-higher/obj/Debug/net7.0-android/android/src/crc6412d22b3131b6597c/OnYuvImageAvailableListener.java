package crc6412d22b3131b6597c;


public class OnYuvImageAvailableListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.ImageReader.OnImageAvailableListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onImageAvailable:(Landroid/media/ImageReader;)V:GetOnImageAvailable_Landroid_media_ImageReader_Handler:Android.Media.ImageReader/IOnImageAvailableListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Emgu.CV.Platform.Maui.UI.OnYuvImageAvailableListener, Emgu.CV.Platform.Maui.UI", OnYuvImageAvailableListener.class, __md_methods);
	}


	public OnYuvImageAvailableListener ()
	{
		super ();
		if (getClass () == OnYuvImageAvailableListener.class) {
			mono.android.TypeManager.Activate ("Emgu.CV.Platform.Maui.UI.OnYuvImageAvailableListener, Emgu.CV.Platform.Maui.UI", "", this, new java.lang.Object[] {  });
		}
	}


	public void onImageAvailable (android.media.ImageReader p0)
	{
		n_onImageAvailable (p0);
	}

	private native void n_onImageAvailable (android.media.ImageReader p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
