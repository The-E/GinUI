using System;
using System.Runtime.Serialization;

namespace GinClientApp.Dialogs.ProgressDialog
{
	[Serializable]
	internal class ProgressDialogCancellationException : Exception
	{
		public ProgressDialogCancellationException()
			: base()
		{
		}

		public ProgressDialogCancellationException(string message)
			: base(message)
		{
		}

		public ProgressDialogCancellationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ProgressDialogCancellationException(string format, params string[] arg)
			: base(string.Format(format, arg))
		{
		}

		protected ProgressDialogCancellationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}