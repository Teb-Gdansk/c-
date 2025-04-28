extern void object::Mission()
{
	    while (true)
	    {
		        object Cel = radar(Target1);
		
		        if (Cel != null)
		          {
			goto(Cel.position);
		}
		
	}
}
