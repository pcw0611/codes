import Observer.Button;
import Observer.Button.OnClickListener;

public class Main 
{
	// Subject (Observerable)
	public static void main(String[] args) 
	{
		Button button = new Button();
		
		// Subscribe
		button.SetOnClickListener( new OnClickListener() {
			
			@Override
			public void OnClick( Button button )
			{
				System.out.println( button +" is Clicked");
			}
		});
		
		button.OnClick();
	}
}