using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Clients
{
    public class CursoredResponse<V> : IEnumerable<V>
    {
		public CursoredResponse(IEnumerable<V> items, string nextCursor)
		{
			Items = items;
			NextCursor = nextCursor;
		}

		public string NextCursor { get; set; }
		public IEnumerable<V> Items { get; }

		public IEnumerator<V> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}
	}
}
