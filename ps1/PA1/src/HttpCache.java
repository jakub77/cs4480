import java.io.File;
import java.io.IOException;
import java.util.ArrayList;

public class HttpCache
{
	ArrayList<String> cacheList;

	public HttpCache()
	{
		cacheList = new ArrayList<String>();
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
		absoluteURL = absoluteURL.replaceAll("/", ".");
		try
		{
			System.out.println("InsertPage");
			File file = new File("cache/" + absoluteURL + ".txt");
			
			if(file.exists())
				return;
			file.createNewFile();
			
			
		}
		catch (IOException e)
		{
			System.out.println("Exception in InsertPage " + e);
		}
		
	}
	
	public byte[] GetPage(String absoluteURL)
	{
		
		return null;		
	}
}
