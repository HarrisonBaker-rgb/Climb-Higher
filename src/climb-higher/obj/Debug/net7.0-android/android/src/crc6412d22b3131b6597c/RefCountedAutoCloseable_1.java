package crc6412d22b3131b6597c;


public class RefCountedAutoCloseable_1
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.AutoCloseable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_close:()V:GetCloseHandler:Java.Lang.IAutoCloseableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Emgu.CV.Platform.Maui.UI.RefCountedAutoCloseable`1, Emgu.CV.Platform.Maui.UI", RefCountedAutoCloseable_1.class, __md_methods);
	}


	public RefCountedAutoCloseable_1 ()
	{
		super ();
		if (getClass () == RefCountedAutoCloseable_1.class) {
			mono.android.TypeManager.Activate ("Emgu.CV.Platform.Maui.UI.RefCountedAutoCloseable`1, Emgu.CV.Platform.Maui.UI", "", this, new java.lang.Object[] {  });
		}
	}


	public void close ()
	{
		n_close ();
	}

	private native void n_close ();

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
