package Observer;

// 버튼은 옵저버의 관찰 대상이 된다
// 그리고 버튼이 눌릴 때 옵저버에 통보한다
public class Button 
{
	// Listener Interface가 옵저버가 된다. 클래스 명 그대로 OnClick 이벤트에 대한 청취를 실시한다
	public interface OnClickListener // Observer
	{
		void OnClick(Button button); // Update
	}
	
	private OnClickListener onClickListener;
	
	public void OnClick()
	{
		// 이벤트 처리
		if( null != onClickListener )
			onClickListener.OnClick( this );	// Update
	}
	public void SetOnClickListener( OnClickListener onClickListener )
	{
		this.onClickListener = onClickListener;
	}	
}
