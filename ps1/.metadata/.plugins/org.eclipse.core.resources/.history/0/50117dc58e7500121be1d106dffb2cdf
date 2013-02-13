import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;

public class HttpCache
{
	final static int BUF_SIZE = 1024*8;
    final static int MAX_OBJECT_SIZE = 1024*1000;
	
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
		if(!f.exists())
			return;
		if(!f.isDirectory())
			return;
		for(File c : f.listFiles())
			c.delete();
		}
		catch(Exception e)
		{
			System.out.println("Exception in ClearCache" + e);
			return;
		}
	}

	public boolean InCache(String absoluteURL)
	{
		absoluteURL = absoluteURL.replaceAll("/", ".");
		if(cacheList.contains(absoluteURL))
			return true;
		return false;
		
	}
	
	public void InsertPage(String absoluteURL, byte[] request)
	{
		//if(absoluteURL.length() > 240)
			//return;
		absoluteURL = absoluteURL.replaceAll("/", ".");
		try
		{
			File file = new File("cache/" + absoluteURL + ".txt");
			
			if(file.exists())
				return;
			file.createNewFile();
			
			FileOutputStream fos = new FileOutputStream(file);
			fos.write(request);
			fos.close();			
			cacheList.add(absoluteURL);
			System.out.println("Added new file to cache");			
		}
		catch (IOException e)
		{
			System.out.println("Exception in InsertPage\nFile is: " + absoluteURL + "\n" + e);
		}
		
	}
	
	public byte[] GetPage(String absoluteURL)
	{
		byte[] totalBuffer = new byte[MAX_OBJECT_SIZE];
		
		absoluteURL = absoluteURL.replaceAll("/", ".");
		try
		{
			File file = new File("cache/" + absoluteURL + ".txt");
			
			if(!file.exists())
				return null;

			FileInputStream fis = new FileInputStream(file);
			fis.read(totalBuffer);
			
			System.out.println("Read a file from cache");
			return totalBuffer;
		}
		catch (IOException e)
		{
			System.out.println("Exception in GetPage " + e);
			return null;
		}	
	}
}
