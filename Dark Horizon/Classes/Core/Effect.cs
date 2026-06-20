namespace Dark_Horizon.Classes.Core;

public enum EffectType
{
    Bleeding,    // Завдає шкоди кожен хід
    Stun,        // Пропуск ходу
    Shield       // Додатковий захист, який поглинає шкоду
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

    // Створює глибоку копію ефекту для накладання на персонажа
    public Effect Clone() => new(Type, Value, TurnsLeft, Name);
}