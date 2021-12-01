// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using Facility.Core;

namespace Facility.ConformanceApi
{
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class AnyResultDto : ServiceDto<AnyResultDto>
	{
		/// <summary>
		/// Creates an instance.
		/// </summary>
		public AnyResultDto()
		{
		}

		public ServiceResult<string>? String { get; set; }

		public ServiceResult<bool>? Boolean { get; set; }

		public ServiceResult<double>? Double { get; set; }

		public ServiceResult<int>? Int32 { get; set; }

		public ServiceResult<long>? Int64 { get; set; }

		public ServiceResult<decimal>? Decimal { get; set; }

		public ServiceResult<byte[]>? Bytes { get; set; }

		public ServiceResult<ServiceObject>? Object { get; set; }

		public ServiceResult<ServiceErrorDto>? Error { get; set; }

		public ServiceResult<AnyDto>? Data { get; set; }

		public ServiceResult<Answer>? Enum { get; set; }

		public ServiceResult<IReadOnlyList<int>>? Array { get; set; }

		public ServiceResult<IReadOnlyDictionary<string, int>>? Map { get; set; }

		public ServiceResult<ServiceResult<int>>? Result { get; set; }

		/// <summary>
		/// Determines if two DTOs are equivalent.
		/// </summary>
		public override bool IsEquivalentTo(AnyResultDto? other)
		{
			return other != null &&
				ServiceDataUtility.AreEquivalentResults(String, other.String) &&
				ServiceDataUtility.AreEquivalentResults(Boolean, other.Boolean) &&
				ServiceDataUtility.AreEquivalentResults(Double, other.Double) &&
				ServiceDataUtility.AreEquivalentResults(Int32, other.Int32) &&
				ServiceDataUtility.AreEquivalentResults(Int64, other.Int64) &&
				ServiceDataUtility.AreEquivalentResults(Decimal, other.Decimal) &&
				ServiceDataUtility.AreEquivalentResults(Bytes, other.Bytes) &&
				ServiceDataUtility.AreEquivalentResults(Object, other.Object) &&
				ServiceDataUtility.AreEquivalentResults(Error, other.Error) &&
				ServiceDataUtility.AreEquivalentResults(Data, other.Data) &&
				ServiceDataUtility.AreEquivalentResults(Enum, other.Enum) &&
				ServiceDataUtility.AreEquivalentResults(Array, other.Array) &&
				ServiceDataUtility.AreEquivalentResults(Map, other.Map) &&
				ServiceDataUtility.AreEquivalentResults(Result, other.Result);
		}
	}
}
