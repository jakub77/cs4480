import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.util.ArrayList;

public class HttpCache
{
	final static int BUF_SIZE = 1024 * 8;
	final static int MAX_OBJECT_SIZE = 1024 * 1000;

	ArrayList<String> cacheList;

	public HttpCache()
	{
		cacheList = new ArrayList<String>();
		ClearCache();

	}

	private void ClearCache()
	{
		try
		{
			File f = new File("cache");
			if (!f.exists())
				return;
			if (!f.isDirectory())
				return;
			for (File c : f.listFiles())
				c.delete();
		}
		catch (Exception e)
		{
			System.out.println("Exception in ClearCache" + e);
			return;
		}
	}

	private String cleanFileName(String fileName)
	{
		char[] c = fileName.toCharArray();
		for (int i = 0; i < c.length; i++)
		{
			if (c[i] == '<' || c[i] == '>' || c[i] == '"' || c[i] == '\\' || c[i] == '/' || c[i] == '?' || c[i] == '*' || c[i] == '|' || c[i] == ':')
				c[i] = '`';
		}
		return new String(c);
	}

	public boolean InCache(String absoluteURL)
	{
		absoluteURL = cleanFileName(absoluteURL);
		synchronized (this)
		{
			if (cacheList.contains(absoluteURL))
				return true;
		}
		return false;
	}

	public void InsertPage(String absoluteURL, byte[] request)
	{
		if (absoluteURL.length() > 240)
			return;
		absoluteURL = cleanFileName(absoluteURL);
		try
		{
			synchronized (this)
			{
				File file = new File("cache/" + absoluteURL + ".txt");

				if (file.exists())
					return;
				file.createNewFile();

				FileOutputStream fos = new FileOutputStream(file);
				fos.write(request);
				fos.close();
				cacheList.add(absoluteURL);
			}
		}
		catch (Exception e)
		{
			System.out.println("Exception in InsertPage\nFile is: " + absoluteURL + "\n" + e);
		}

	}

	public byte[] GetPage(String absoluteURL)
	{
		byte[] totalBuffer = new byte[MAX_OBJECT_SIZE];

		absoluteURL = cleanFileName(absoluteURL);
		try
		{
			synchronized (this)
			{
				File file = new File("cache/" + absoluteURL + ".txt");

				if (!file.exists())
					return null;

				FileInputStream fis = new FileInputStream(file);
				int bytes = fis.read(totalBuffer);
				if(bytes == -1)
					return new byte[0];
				byte[] data = new byte[bytes];
				for (int i = 0; i < bytes; i++)
					data[i] = totalBuffer[i];

				return data;
			}
		}
		catch (Exception e)
		{
			System.out.println("Exception in GetPage " + e);
			return null;
		}
	}
}
