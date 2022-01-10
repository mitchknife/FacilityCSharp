// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core;

namespace EdgeCases
{
	/// <summary>
	/// A delegating implementation of EdgeCases.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public partial class DelegatingEdgeCases : IEdgeCases
	{
		/// <summary>
		/// Creates an instance with the specified delegator.
		/// </summary>
		public DelegatingEdgeCases(ServiceDelegator delegator) =>
			m_delegator = delegator ?? throw new ArgumentNullException(nameof(delegator));

		/// <summary>
		/// An old method.
		/// </summary>
		[Obsolete]
		public virtual async Task<ServiceResult<OldMethodResponseDto>> OldMethodAsync(OldMethodRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(EdgeCasesMethods.OldMethod, request, cancellationToken).ConfigureAwait(false)).Cast<OldMethodResponseDto>();

		private readonly ServiceDelegator m_delegator;
	}
}
