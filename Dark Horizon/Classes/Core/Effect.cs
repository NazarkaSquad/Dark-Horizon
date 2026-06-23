namespace Dark_Horizon.Classes.Core;

public enum EffectType
{
    Bleeding,  
    Stun,    
    Shield     
}

public class Effect
{
    public EffectType Type { get; }
    public int Value { get; set; }
    public int TurnsLeft { get; set; }
    public string Name { get; }

    public Effect(EffectType type, int value, int turnsLeft, string name)
    {
        Type = type;
        Value = value;
        TurnsLeft = turnsLeft;
        Name = name;
    }

    public Effect Clone() => new(Type, Value, TurnsLeft, Name);
}