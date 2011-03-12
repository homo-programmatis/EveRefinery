using System;
using System.Text;
using SpecialFNs;

namespace UserIDDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			string userID = ComputerID.GetComputerID();
			string message = String.Format("Your computer id is: {0:s}\n", userID);
			Console.WriteLine(message);
		}
	}
}
