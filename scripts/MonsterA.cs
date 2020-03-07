 using Godot;
using System;

public class MonsterA : KinematicBody2D
{	
	Vector2 velocity;

	[Export] 
	int lifes = 1;
	int speed = 70;

	public override void _Ready()
	{
		GetNode<VisibilityNotifier2D>("VisibleArea").Connect("screen_exited", this, "OnScreenExited");
		GetNode<AnimatedSprite>("AnimatedSprite").Play("default");
	}

	public override void _Process(float delta)
	{
		velocity.y = speed * delta;

		if (lifes == 0) {
			QueueFree();
		}

		MoveAndCollide(velocity);
	}
	
	public void OnScreenExited()
	{
		QueueFree();
	}
}
