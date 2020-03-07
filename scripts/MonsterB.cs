using Godot;
using System;

public class MonsterB : KinematicBody2D
{	
	[Export]
	int lifes = 3;
	int wingLifes = 2;
	int speed = 80;

	Vector2 velocity;

	public override void _Ready()
	{
		GetNode<AnimatedSprite>("Sprite").Play("default");
		GetNode<Area2D>("Wings").Connect("area_entered", this, "OnWingsAreaEntered");
		GetNode<VisibilityNotifier2D>("VisibleArea").Connect("screen_exited", this, "OnScreenExited");
	}

	public override void _PhysicsProcess(float delta)
	{
		if (lifes == 0) {
			QueueFree();
		}

		if (wingLifes == 0) {
			RemoveChild(GetNode<Area2D>("Wings"));
			GetNode<AnimatedSprite>("Sprite").Hide();
			GetNode<AnimatedSprite>("Wingless").Show();
			GetNode<AnimatedSprite>("Wingless").Play("default");
			Rotation += 4 * delta;
			velocity.x = speed;
		}

		velocity.y = speed;

		velocity *= delta;

		MoveAndCollide(velocity);
	}

	public void OnWingsAreaEntered(PhysicsBody2D body)
	{
		wingLifes -= 1;
	}

	public void OnScreenExited()
	{
		QueueFree();
	}
}
