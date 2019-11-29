using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Models.Blocks
{
	public class DividerBlock : BlockBase
	{
		public const string BlockName = "divider";
		public DividerBlock() : base(BlockName)
		{
		}
	}
}
