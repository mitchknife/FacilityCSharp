// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using Facility.Core;

namespace Facility.ConformanceApi
{
	/// <summary>
	/// Response for Required.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class RequiredResponseDto : ServiceDto<RequiredResponseDto>
	{
		/// <summary>
		/// Creates an instance.
		/// </summary>
		public RequiredResponseDto()
		{
		}

		public string? Normal { get; set; }

		/// <summary>
		/// Returns the DTO as JSON.
		/// </summary>
		public override string ToString() => SystemTextJsonServiceSerializer.Instance.ToJson(this);

		/// <summary>
		/// Determines if two DTOs are equivalent.
		/// </summary>
		public override bool IsEquivalentTo(RequiredResponseDto? other)
		{
			return other != null &&
				Normal == other.Normal;
		}

		/// <summary>
		/// Validates the DTO.
		/// </summary>
		public override bool Validate(out string? errorMessage)
		{
			errorMessage = GetValidationErrorMessage();
			return errorMessage == null;
		}

		private string? GetValidationErrorMessage()
		{
			if (Normal == null)
				return ServiceDataUtility.GetRequiredFieldErrorMessage("normal");

			return null;
		}
	}
}
