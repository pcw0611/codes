using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Inventory
{
	List<IEquipObserver> equipObservers = new List<IEquipObserver>();

	public void Equip( Item item )
	{
		// Notify
		foreach ( IEquipObserver observer in equipObservers )
		{
			observer.OnEquip( item );
		}
	}

	public void AddEquipObserver( IEquipObserver equipObserver )
	{
		equipObservers.Add( equipObserver );
	}
}