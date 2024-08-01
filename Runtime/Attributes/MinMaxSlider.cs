// Adapted from: https://gist.github.com/frarees/9791517
// Licence: https://frarees.github.io/default-gist-license

using System;
using UnityEngine;

namespace Runtime.Shared
{
	/// <summary>
	/// An attribute that simplifies defining bounded ranges (ranges with minimum and maximum limits) on the inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class MinMaxSliderAttribute : PropertyAttribute
	{
	#region Fields
		public readonly float Min;
		public readonly float Max;
		public readonly bool DataFields = true;
		public readonly bool FlexibleFields = true;
		public readonly  bool Bound = true;
		public readonly bool Round = true;
	#endregion 

		public MinMaxSliderAttribute (float min, float max)
		{
			Min = min;
			Max = max;
		}
	}
}
