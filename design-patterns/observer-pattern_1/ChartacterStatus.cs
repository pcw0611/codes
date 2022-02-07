using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ChartacterStatus : IEquipObserverable
{
	void IEquipObserverable.OnEquip( Item item )
	{
		Console.WriteLine( "아이템 장착 - 캐릭터 스탯 변화" );
	}
}