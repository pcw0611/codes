using System;

interface IContainer
{
	int Count { get; }

	bool Add( Object obj );
	void Remove( Object obj );
	void Clear();
	IIterator MakeIterator();
}

public interface IIterator
{
	Object Current { get; }

	void Begin();
	bool MoveNext();
}