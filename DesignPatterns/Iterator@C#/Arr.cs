using System;

class Arr : IContainer
{
	public class ArrIterator : IIterator
	{
		Object[]		buffer;
		int				bSize;
		int				index;

		public ArrIterator( Object[] buffer, int bSize )
		{
			this.buffer = buffer;
			this.bSize	= bSize;
		}

		public Object Current {  get { return buffer[index]; } }

		public void Begin()
		{
			index = -1;
		}

		public bool MoveNext()
		{
			++index;

			return index < bSize;
		}
	}

	Object[]			buffer;
	readonly int		max_element;

	public Arr( int max_element )
	{
		this.max_element		= max_element;
		buffer					= new Object[max_element];
	}

	public int Count { get; private set; }
	public bool Add( Object obj )
	{
		if( Count < max_element )
		{
			buffer[ Count ] = obj;
			Count++;
			return true;
		}

		return false;
	}
	public void Clear()
	{
		Count = 0;
	}
	public void Remove( Object obj )
	{
		int index = Find( obj );
		if ( index == -1 )
			return;

		--Count;
		for( int i = index; i < Count; ++i  )
		{
			buffer[ i ] = buffer[ i + 1 ];
		}
	}
	private int Find( object obj )
	{
		int index = 0;
		for( int i = 0; i < Count; ++i )
		{
			if( buffer[ index ] == obj )
			{
				return index;
			}
		}
		return -1;
	}

	public IIterator MakerIterator()
	{
		return new ArrIterator( buffer, Count );
	}
}
