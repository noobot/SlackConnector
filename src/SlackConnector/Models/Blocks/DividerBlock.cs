using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public class DividerBlock : BlockBase
	{
		public const string BlockName = "divider";
		public DividerBlock() : base(BlockName)
		{
		}
	}
}
