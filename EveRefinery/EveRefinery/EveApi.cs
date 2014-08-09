using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace EveRefinery
{
	class EveApi
	{
		public static XmlDocument MakeRequest(String a_ApiUrl, Settings._ApiAccess.Key a_ApiKey, UInt32 a_ApiUser, String a_FailMessage)
		{
			try
			{
				XmlDocument xmlReply = Engine.LoadXmlWithUserAgent(MakeUrl(a_ApiUrl, a_ApiKey, a_ApiUser));

				XmlNodeList errorNodes = xmlReply.GetElementsByTagName("error");
				if (0 != errorNodes.Count)
				{
					Engine.ShowXmlRequestErrors(a_FailMessage + ":\n", errorNodes);
					return null;
				}

				return xmlReply;
			}
			catch (System.Net.WebException a_Exception)
			{
				String errorHint = "";

				if (a_Exception.Response is HttpWebResponse)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)a_Exception.Response;
					switch (httpResponse.StatusCode)
					{
						case HttpStatusCode.Forbidden:
							errorHint = "(Did you provide API key with insufficient access?)";
							break;
					}
				}

				String errorMessage = a_FailMessage + ":\n" + a_Exception.Message;
				if ("" != errorHint)
				{
					errorMessage += "\n";
					errorMessage += errorHint;
				}

				SpecialFNs.ErrorMessageBox.Show(errorMessage);
				return null;
			}
			catch (System.Exception a_Exception)
			{
				SpecialFNs.ErrorMessageBox.Show(a_FailMessage + ":\n" + a_Exception.Message);
				return null;
			}
		}

		private static String MakeUrl(String a_ApiUrl, Settings._ApiAccess.Key a_ApiKey, UInt32 a_ApiUser)
		{
			List<String> parameters = new List<String>();

			if (null != a_ApiKey)
			{
				parameters.Add("KeyID=" + a_ApiKey.KeyID);
				parameters.Add("vCode=" + a_ApiKey.Verification);
			}

			if (0 != a_ApiUser)
				parameters.Add("characterID=" + a_ApiUser);

			String parameterList = String.Join("&", parameters.ToArray());

			string requestUrl = "http://api.eve-online.com/" + a_ApiUrl;
			if ("" != parameterList)
			{
				requestUrl += "?";
				requestUrl += parameterList;
			}

			return requestUrl;
		}
	}
}