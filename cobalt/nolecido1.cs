extern void object::Mission()
{
	    object Cel = radar(Target2);
	    
	    while (Cel != null)
	    {
		        turn(direction(Cel.position));
		        goto(Cel.position);
		        
		        Cel = radar(Target2);
		    }
}
