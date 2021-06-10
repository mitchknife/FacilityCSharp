// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core;

namespace Facility.ConformanceApi
{
	/// <summary>
	/// A delegating implementation of ConformanceApi.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public class DelegatingConformanceApi : IConformanceApi
	{
		/// <summary>
		/// Creates an instance with the specified delegator.
		/// </summary>
		public DelegatingConformanceApi(ServiceDelegator delegator) =>
			m_delegator = delegator ?? throw new ArgumentNullException(nameof(delegator));

		/// <summary>
		/// Gets API information.
		/// </summary>
		public virtual async Task<ServiceResult<GetApiInfoResponseDto>> GetApiInfoAsync(GetApiInfoRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.GetApiInfo, request, cancellationToken).ConfigureAwait(false)).Cast<GetApiInfoResponseDto>();

		/// <summary>
		/// Gets widgets.
		/// </summary>
		public virtual async Task<ServiceResult<GetWidgetsResponseDto>> GetWidgetsAsync(GetWidgetsRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.GetWidgets, request, cancellationToken).ConfigureAwait(false)).Cast<GetWidgetsResponseDto>();

		/// <summary>
		/// Creates a new widget.
		/// </summary>
		public virtual async Task<ServiceResult<CreateWidgetResponseDto>> CreateWidgetAsync(CreateWidgetRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.CreateWidget, request, cancellationToken).ConfigureAwait(false)).Cast<CreateWidgetResponseDto>();

		/// <summary>
		/// Gets the specified widget.
		/// </summary>
		public virtual async Task<ServiceResult<GetWidgetResponseDto>> GetWidgetAsync(GetWidgetRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.GetWidget, request, cancellationToken).ConfigureAwait(false)).Cast<GetWidgetResponseDto>();

		/// <summary>
		/// Deletes the specified widget.
		/// </summary>
		public virtual async Task<ServiceResult<DeleteWidgetResponseDto>> DeleteWidgetAsync(DeleteWidgetRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.DeleteWidget, request, cancellationToken).ConfigureAwait(false)).Cast<DeleteWidgetResponseDto>();

		/// <summary>
		/// Gets the specified widgets.
		/// </summary>
		public virtual async Task<ServiceResult<GetWidgetBatchResponseDto>> GetWidgetBatchAsync(GetWidgetBatchRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.GetWidgetBatch, request, cancellationToken).ConfigureAwait(false)).Cast<GetWidgetBatchResponseDto>();

		public virtual async Task<ServiceResult<MirrorFieldsResponseDto>> MirrorFieldsAsync(MirrorFieldsRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.MirrorFields, request, cancellationToken).ConfigureAwait(false)).Cast<MirrorFieldsResponseDto>();

		public virtual async Task<ServiceResult<CheckQueryResponseDto>> CheckQueryAsync(CheckQueryRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.CheckQuery, request, cancellationToken).ConfigureAwait(false)).Cast<CheckQueryResponseDto>();

		public virtual async Task<ServiceResult<CheckPathResponseDto>> CheckPathAsync(CheckPathRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.CheckPath, request, cancellationToken).ConfigureAwait(false)).Cast<CheckPathResponseDto>();

		public virtual async Task<ServiceResult<MirrorHeadersResponseDto>> MirrorHeadersAsync(MirrorHeadersRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.MirrorHeaders, request, cancellationToken).ConfigureAwait(false)).Cast<MirrorHeadersResponseDto>();

		public virtual async Task<ServiceResult<MixedResponseDto>> MixedAsync(MixedRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.Mixed, request, cancellationToken).ConfigureAwait(false)).Cast<MixedResponseDto>();

		public virtual async Task<ServiceResult<RequiredResponseDto>> RequiredAsync(RequiredRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.Required, request, cancellationToken).ConfigureAwait(false)).Cast<RequiredResponseDto>();

		public virtual async Task<ServiceResult<MirrorBytesResponseDto>> MirrorBytesAsync(MirrorBytesRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(ConformanceApiMethods.MirrorBytes, request, cancellationToken).ConfigureAwait(false)).Cast<MirrorBytesResponseDto>();

		private readonly ServiceDelegator m_delegator;
	}
}
