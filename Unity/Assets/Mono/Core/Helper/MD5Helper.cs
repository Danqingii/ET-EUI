using System.IO;
using System.Security.Cryptography;

namespace ET
{
	public static class MD5Helper
	{
		public static string FileMD5(string filePath)
		{
			byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
	            MD5 md5 = MD5.Create();
				retVal = md5.ComputeHash(file);
			}
			return retVal.ToHex("x2");
		}
		
		public static string StringConversionMD5(string  str) 
		{
			MD5 md5 =  MD5.Create();

			byte[] c = System.Text.Encoding.Default.GetBytes(str);

			byte[] b = md5.ComputeHash(c);//用来计算指定数组的hash值
         
			//将每一个字节数组中的元素都tostring，在转成16进制
			string newStr = null;
			for (int i = 0; i < b.Length; i++)
			{
				newStr += b[i].ToString("x2") ;  //ToString(param);//传入不同的param可以转换成不同的效果
			}
			return newStr;
		}   
	}
}
