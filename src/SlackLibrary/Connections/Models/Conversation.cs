using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Models
{
	public class ConversationChannel : Detail
	{
		public class ChannelPurpose
		{
			[JsonProperty("value")]
			public string Value { get; set; }

			[JsonProperty("creator")]
			public string Creator { get; set; }

			[JsonProperty("last_set")]
			public long LastSet { get; set; }
		}


		[JsonProperty("is_channel")]
		public bool IsChannel { get; set; }

		[JsonProperty("is_group")]
		public bool IsGroup { get; set; }

		[JsonProperty("is_im")]
		public bool IsIm { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("creator")]
		public string Creator { get; set; }

		[JsonProperty("is_archived")]
		public bool IsArchived { get; set; }

		[JsonProperty("is_general")]
		public bool IsGeneral { get; set; }

		[JsonProperty("unlinked")]
		public long Unlinked { get; set; }

		[JsonProperty("name_normalized")]
		public string NameNormalized { get; set; }

		[JsonProperty("is_read_only")]
		public bool IsReadOnly { get; set; }

		[JsonProperty("is_shared")]
		public bool IsShared { get; set; }

		[JsonProperty("is_ext_shared")]
		public bool IsExtShared { get; set; }

		[JsonProperty("is_org_shared")]
		public bool IsOrgShared { get; set; }

		[JsonProperty("pending_shared")]
		public object[] PendingShared { get; set; }

		[JsonProperty("is_pending_ext_shared")]
		public bool IsPendingExtShared { get; set; }

		[JsonProperty("is_member")]
		public bool IsMember { get; set; }

		[JsonProperty("is_private")]
		public bool IsPrivate { get; set; }

		[JsonProperty("is_mpim")]
		public bool IsMpim { get; set; }

		[JsonProperty("last_read")]
		public string LastRead { get; set; }

		[JsonProperty("topic")]
		public ChannelPurpose Topic { get; set; }

		[JsonProperty("purpose")]
		public ChannelPurpose Purpose { get; set; }

		[JsonProperty("previous_names")]
		public string[] PreviousNames { get; set; }

		[JsonProperty("num_members")]
		public long NumMembers { get; set; }

		[JsonProperty("locale")]
		public string Locale { get; set; }
	}
}
