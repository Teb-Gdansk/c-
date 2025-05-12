extern void object::Mission()
{
    float temp_treshold = 0.8;	    
	    while (radar(Target2) != null)
	    {
	                object item = radar(Target2);
		        float dir = direction(item.position);
		        // turn(direction(Target2.position));
		        jet((item.position.z - this.position.z)/15);
		    	motor(1 -dir/90, 1 + dir/90);
		    }
	goto(radar(SpaceShip).position);
}
