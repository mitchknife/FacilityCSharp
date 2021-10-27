// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Facility.Core;
using Newtonsoft.Json;

#pragma warning disable 612, 618 // member is obsolete

namespace EdgeCases
{
	/// <summary>
	/// Some old values.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	[JsonConverter(typeof(OldValuesJsonConverter))]
	public partial struct OldValues : IEquatable<OldValues>
	{
		/// <summary>
		/// An old value.
		/// </summary>
		[Obsolete]
		public static readonly OldValues Old = new OldValues(Strings.Old);

		[Obsolete]
		public static readonly OldValues Older = new OldValues(Strings.Older);

		/// <summary>
		/// Creates an instance.
		/// </summary>
		public OldValues(string value) => m_value = value;

		/// <summary>
		/// Converts the instance to a string.
		/// </summary>
		public override string ToString() => m_value != null && s_valueCache.TryGetValue(m_value, out var cachedValue) ? cachedValue : m_value ?? "";

		/// <summary>
		/// Checks for equality.
		/// </summary>
		public bool Equals(OldValues other) => StringComparer.OrdinalIgnoreCase.Equals(m_value ?? "", other.m_value ?? "");

		/// <summary>
		/// Checks for equality.
		/// </summary>
		public override bool Equals(object? obj) => obj is OldValues && Equals((OldValues) obj);

		/// <summary>
		/// Gets the hash code.
		/// </summary>
		public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(m_value ?? "");

		/// <summary>
		/// Checks for equality.
		/// </summary>
		public static bool operator ==(OldValues left, OldValues right) => left.Equals(right);

		/// <summary>
		/// Checks for inequality.
		/// </summary>
		public static bool operator !=(OldValues left, OldValues right) => !left.Equals(right);

		/// <summary>
		/// Returns true if the instance is equal to one of the defined values.
		/// </summary>
		public bool IsDefined() => m_value != null && s_valueCache.ContainsKey(m_value);

		/// <summary>
		/// Returns all of the defined values.
		/// </summary>
		public static IReadOnlyList<OldValues> GetValues() => s_values;

		/// <summary>
		/// Provides string constants for defined values.
		/// </summary>
		public static class Strings
		{
			/// <summary>
			/// An old value.
			/// </summary>
			[Obsolete]
			public const string Old = "old";

			[Obsolete]
			public const string Older = "older";
		}

		/// <summary>
		/// Used for JSON serialization.
		/// </summary>
		public sealed class OldValuesJsonConverter : ServiceEnumJsonConverter<OldValues>
		{
			/// <summary>
			/// Creates the value from a string.
			/// </summary>
			protected override OldValues CreateCore(string value) => new OldValues(value);
		}

		private static readonly ReadOnlyCollection<OldValues> s_values = new ReadOnlyCollection<OldValues>(
			new[]
			{
				Old,
				Older,
			});

		private static readonly IReadOnlyDictionary<string, string> s_valueCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ Strings.Old, Strings.Old },
			{ Strings.Older, Strings.Older },
		};

		private readonly string m_value;
	}
}
