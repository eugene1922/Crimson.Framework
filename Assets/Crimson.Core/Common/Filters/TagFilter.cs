using Crimson.Core.Enums;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Crimson.Core.Common.Filters
{
	[Serializable]
	public struct TagFilter
	{
		public bool useTagFilter;

		[ShowIf(nameof(useTagFilter))]
		[EnumToggleButtons]
		public TagFilterMode filterMode;

		[ShowIf(nameof(useTagFilter))]
		[ValueDropdown(nameof(Tags))]
		public List<string> filterTags;

		private static IEnumerable Tags()
		{
			return EditorUtils.GetEditorTags();
		}
	}
}