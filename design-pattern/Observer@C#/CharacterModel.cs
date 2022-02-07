using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CharacterModel : IEquipObserver
{
	void IEquipObserver.OnEquip( Item item )
	{
		Console.WriteLine( "아이템 장착 - 캐릭터 모델 변화" );
	}
}