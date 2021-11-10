using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Inventory
{
	List<IEquipObserverable> equipObserverables = new List<IEquipObserverable>();

	public void Equip( Item item )
	{
		// Notify
		foreach ( IEquipObserverable observerable in equipObserverables )
		{
			observerable.OnEquip( item );
		}
	}

	public void AddEquipObserverable( IEquipObserverable equipObserverable )
	{
		equipObserverables.Add( equipObserverable );
	}
}