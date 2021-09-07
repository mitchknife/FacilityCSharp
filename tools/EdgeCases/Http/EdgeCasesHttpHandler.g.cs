// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core.Http;

#pragma warning disable 612, 618 // member is obsolete

namespace EdgeCases.Http
{
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class EdgeCasesHttpHandler : ServiceHttpHandler
	{
		/// <summary>
		/// Creates the handler.
		/// </summary>
		public EdgeCasesHttpHandler(IEdgeCases service, ServiceHttpHandlerSettings? settings = null)
			: base(settings)
		{
			m_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		/// <summary>
		/// Creates the handler.
		/// </summary>
		public EdgeCasesHttpHandler(Func<HttpRequestMessage, IEdgeCases> getService, ServiceHttpHandlerSettings? settings = null)
			: base(settings)
		{
			m_getService = getService ?? throw new ArgumentNullException(nameof(getService));
		}

		/// <summary>
		/// Attempts to handle the HTTP request.
		/// </summary>
		public override async Task<HttpResponseMessage?> TryHandleHttpRequestAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken = default)
		{
			return await AdaptTask(TryHandleOldMethodAsync(httpRequest, cancellationToken)).ConfigureAwait(true);
		}

		/// <summary>
		/// An old method.
		/// </summary>
		[Obsolete]
		public Task<HttpResponseMessage?> TryHandleOldMethodAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken = default) =>
			TryHandleServiceMethodAsync(EdgeCasesHttpMapping.OldMethodMapping, httpRequest, GetService(httpRequest).OldMethodAsync, cancellationToken);

		/// <summary>
		/// Returns the HTTP status code for a custom error code.
		/// </summary>
		protected override HttpStatusCode? TryGetCustomHttpStatusCode(string errorCode) =>
			HttpOldErrors.TryGetHttpStatusCode(errorCode);

		private IEdgeCases GetService(HttpRequestMessage httpRequest) => m_service ?? m_getService!(httpRequest);

		private readonly IEdgeCases? m_service;
		private readonly Func<HttpRequestMessage, IEdgeCases>? m_getService;
	}
}
