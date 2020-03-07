using Godot;
using System;

public class Player : KinematicBody2D
{	
	[Export]
	int lifes = 3;
	int speed = 600;
	float fireRate = .3f;
	string fireType;
	bool isIvulnerable = false;
	float ivulnerableTime = 1f;
	int shakeOnDamage = 3;
	float shakeTimeout = .5f;

	bool isFiring = false;

	Vector2 velocity;
	
	Camera2D Camera;

	AnimatedSprite Turbo;
	AnimatedSprite Default;
	AnimatedSprite Blink;
	Sprite Damaged;

	public override void _Ready()
	{	
		Default = GetNode<AnimatedSprite>("Sprite");
		Turbo = GetNode<AnimatedSprite>("Turbo");
		Blink = GetNode<AnimatedSprite>("Blink");
		Damaged = GetNode<Sprite>("Damaged");
		Camera = GetNode<Camera2D>("../Camera2D");

		GetNode<Area2D>("ColisionArea").Connect("body_entered", this, "OnBodyEntered");
		Default.Play("idle");
		fireType = "Fire.tscn";
	}

	public override void _PhysicsProcess(float delta)
	{
		Controller("keyboard");

		if (isIvulnerable) {
			var rand = new Random();
			var vec = new Vector2(rand.Next(-1, 1) * shakeOnDamage, rand.Next(-1, 1) * shakeOnDamage);
			Camera.Offset = vec;
			GD.Print(Camera.Offset);
		}

		MoveAndCollide(velocity * delta);
	}

	public void Controller(string method = null)
	{
		if (Input.IsActionPressed("left")) {
			velocity.x = -speed;
		} else if (Input.IsActionPressed("right")) {
			velocity.x = speed;
		} else {
			velocity.x = 0;
		}

		if (Input.IsActionPressed("up")) {
			velocity.y = -speed;
			Turbo.Play("foward");
		} else if (Input.IsActionPressed("down")) {
			velocity.y = speed;
		} else {
			velocity.y = 0;
		}

		if (Input.IsActionJustReleased("up")) {
			Turbo.Play("hide");
		}

		if (Input.IsActionPressed("fire")) {
			Default.Play("firing");
			Fire();
		} else if (Input.IsActionJustReleased("fire")) { 
			Default.Play("idle");
		}
	}

	public void Timeout(string method, float seconds)
	{
		var timer = new Timer();
		timer.WaitTime = seconds;
		timer.OneShot = true;
		timer.Connect("timeout", this, method);
		AddChild(timer);
		timer.Start();

		if (timer.TimeLeft == 0) {
			timer.QueueFree();
		}
	}

	public void Fire()
	{ 
		if (!isFiring) {
			isFiring = true;

			var fireScene = (PackedScene) ResourceLoader.Load("res://" + fireType);
			var fire = (Node2D) fireScene.Instance();

			GetParent().AddChild(fire);

			fire.Position = new Vector2(Position.x, Position.y - 64);

			Timeout("OnFireTimeout", fireRate);
		}
	}

	public void OnFireTimeout()
	{
		isFiring = false;
	}

	public void OnBodyEntered(PhysicsBody2D body)
	{ 
		lifes -= 1;
		Damaged.Show();
		isIvulnerable = true;
		Timeout("OnIvulnerableTimeout", ivulnerableTime);
	}

	public void OnIvulnerableTimeout()
	{
		isIvulnerable = false;
		Camera.Offset = new Vector2(0, 0);
		Damaged.Hide();
	}

}
