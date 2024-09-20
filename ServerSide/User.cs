namespace ServerSide;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public short Age { get; set; }
    public override string ToString() => $"{Id}. {Name} {Surname} {Age}";
}
