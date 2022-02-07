using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Unity 에서는 옵저버 패턴을 -Observer가 아닌 -Listener 용어로 통일하고 있다 (주로 UGUI 이벤트에서 많이 본다)
// 이 인터페이스는 '장착의 상태 변화를 감지할 수 있는~' 기능(be able to)을 할 수 있게 한다
interface IEquipObserverable
{
	void OnEquip( Item item );
}