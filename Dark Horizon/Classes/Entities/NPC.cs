namespace Dark_Horizon.Classes.Entities;

public class NPC : Character
{
    public string Role { get; set; }

    public NPC(string name, string role) : base()
    {
        Name = name;
        Role = role;
        MaxHealth = 100;
        Health = 100;
    }
}