# ProjectAN

## v 0.2

#####

> 개요

* 대본(Script)을 파싱하여 캐릭터가 대화를 진행하는 다이얼로그 씬을 구성합니다.
* 프로토타입 중점으로 개발합니다.
&nbsp;
> Sample

<img src="PNG_0.png" Width=277 Height=160>
<img src="JPEG_0.jpeg" Width=277 Height=160>
<img src="JPG_1.jpg" Width=277 Height=160>

&nbsp;
> 메커니즘

<img src="PNG_1.png">
&nbsp;

시나리오는 프로그래머가 작성하지 않기 때문에, 시나리오라이터가 이해할 수 있는 스크립트 양식을 구성합니다.

텍스트 파일을 파싱하고, 프로그램에서는 라인 한 줄 단위로 명령화 시킵니다. 명령을 차례대로 수행해야하기 때문에 이 파일을 파싱하는 Screenplay(대본) 클래스에서는 커맨드를 큐(Queue) 자료구조로 가지고 있습니다. 또한 이 클래스에서 커맨드 패턴이 사용 됩니다.

이를 테면 Background 커맨드는 해당되는 배경화면 파일로 배경화면을 지정합니다. Character는 해당되는 이미지 파일로 캐릭터를 지정합니다.

또한 커맨드는 자동으로 다음 커맨드를 진행할지 판단도 필요합니다. 예를 들면 Talk 커맨드(내부에서는 앞의 포맷이 [name=""] 으로 지정되면 Talk 커맨드로 판단됩니다)는 사용자가 '터치'를 진행해야만 다음으로 진행되기 때문이죠.

하지만 Character 커맨드나 Background 커맨드는 사용자의 수동적인 조작없이 다음 커맨드가 자동으로 진행이 되어야 합니다. 이에 따른 처리도 필요합니다.


<img src="GIF_1.gif">


대본(Script)을 파싱 → 명령화 → 명령 호출이 이뤄지면, 다음과 같은 화면이 연출됩니다.

Dialog(:Monobehaviour) 클래스는 단순히 명령 이벤트를 받아, 다이얼로그를 작동 시키는 역할만 합니다.

Dialog는 Screenplay(대본)을 로드해서 사용할 수 있도록, Screenplay를 하위 모듈화합니다. 또한 Screenplay에서 발생되는 커맨드들의 이벤트 응답을 받을 수 있게 Dialog는 IScreenplayEvent 인터페이스를 상속 받습니다.

대본과 관련된 이벤트가 발생한 경우, 관련된 데이터도 함께 전달 받고 Dialog는 그에 따른 UI 처리를 진행합니다.
&nbsp;

마지막으로 해당 대본(Script)이 프로그램 내에서는 어떻게 작동하는지 설명합니다.
&nbsp;
<div align="center">[Character(name="char_002_amiya_1", name2="char_249_muesys_1", focus=1)]</div>

&nbsp;

* char_002_amiya_1, char_249_muesys_2 두 캐릭터를 화면에 뿌린다
* focus는 첫번째 캐릭터로 한다 (나머지 캐릭터는 그을림 효과가 발생)




> 미비 건 (더 구현하고 싶은 것들)
* 각종 스크린 효과
* 선택지
* BGM/SE 재생
* Fade In / Out
* 다이얼로그 툴

&nbsp;
&nbsp;
&nbsp;


©2017 Hypergryph Co.,ltd. All Rights Reserved./©2018 Yostar Inc. All rights reserved.