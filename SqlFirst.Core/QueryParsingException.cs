using System;
using System.Runtime.Serialization;

namespace SqlFirst.Core
{
	/// <summary>
	/// Возникает при ошибке построения запроса
	/// </summary>
	public class QueryEmitException : ApplicationException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class.</summary>
		public QueryEmitException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected QueryEmitException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class with a specified
		/// error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public QueryEmitException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class with a specified
		/// error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception. If the innerException parameter
		/// is not a null reference, the current exception is raised in a catch block that handles the inner exception.
		/// </param>
		public QueryEmitException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	/// <summary>
	/// Возникает при ошибке анализа запроса
	/// </summary>
	public class QueryParsingException : ApplicationException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class.</summary>
		public QueryParsingException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected QueryParsingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class with a specified
		/// error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public QueryParsingException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"></see> class with a specified
		/// error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception. If the innerException parameter
		/// is not a null reference, the current exception is raised in a catch block that handles the inner exception.
		/// </param>
		public QueryParsingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}