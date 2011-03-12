using System.Threading;
using System;

namespace SpecialFNs
{
	class ThreadWithParam
	{
		public delegate void ThreadFn(Object a_Param);

		public ThreadFn			Function;
		public Object			Parameter;

		protected void ThreadFunction()
		{
			Function(Parameter);
		}
		
		// summary:
		// Creates a thread, but does not start it.
		public Thread CreateThread()
		{
			return new Thread(new ThreadStart(ThreadFunction));
		}
	}
}
