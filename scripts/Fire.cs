using Godot;
using System;

public class Fire : KinematicBody2D
{	
	[Export]
	int speed = 1000;
	int fireDamage = 1;

	Vector2 velocity;

	public override void _Ready()
	{
		GetNode<Area2D>("ColisionArea").Connect("body_entered", this, "OnBodyEntered");
		GetNode<VisibilityNotifier2D>("VisibleArea").Connect("screen_exited", this, "OnScreenExited");
	} 

	public override void _PhysicsProcess(float delta)
	{
		velocity.y = -speed * delta;

		MoveAndCollide(velocity);
		
	}

	public void OnScreenExited()
	{
		GD.Print("screen exited");
	}

	public void OnBodyEntered(PhysicsBody2D body) 
	{
		body.Set("lifes", (int) body.Get("lifes") - fireDamage);
		QueueFree();
	}
}
