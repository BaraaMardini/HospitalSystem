using System.ComponentModel.DataAnnotations;

public class SpecializationViewDTO
{
    public int id { get; set; }
    public string name { get; set; }

    public SpecializationViewDTO() { }

    public SpecializationViewDTO(int id, string name)
    {
        id = id;
        name = name;
    }
}

