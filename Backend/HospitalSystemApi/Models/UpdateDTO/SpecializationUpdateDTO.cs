using System.ComponentModel.DataAnnotations;

public class SpecializationUpdateDTO
{
 [Required]
    public int  id { get; set; }
    public string?  name { get; set; }

    public SpecializationUpdateDTO() { }

    public SpecializationUpdateDTO(int id, string name)
    {
        id = id;
        name = name;
    }
}

