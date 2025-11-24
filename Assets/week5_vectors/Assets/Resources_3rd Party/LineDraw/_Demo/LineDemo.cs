using UnityEngine;

public class LineDemo : MonoBehaviour 
{
	public LineFactory lineFactory;

	private Line drawnLine;

    void Update ()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			var pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			// Start line drawing
			//
			drawnLine = lineFactory.GetLine (pos, pos, 0.02f, Color.black);

            drawnLine.EnableDrawing(true);	
		} 
		else if (Input.GetMouseButtonUp (0) && drawnLine != null) 
		{
            drawnLine.EnableDrawing(false);
        }

		if (drawnLine != null) {
			// Update line end
			//
			drawnLine.end = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Debug.Log(drawnLine.end);
		}
	}

}
