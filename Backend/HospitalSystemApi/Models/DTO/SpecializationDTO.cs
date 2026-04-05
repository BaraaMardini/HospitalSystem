using System.ComponentModel.DataAnnotations;

public class SpecializationDTO
{
    public int id { get; set; }
    public string name { get; set; }

    public SpecializationDTO() { }

    public SpecializationDTO(int id, string name)
    {
        id = id;
        name = name;
    }
}

