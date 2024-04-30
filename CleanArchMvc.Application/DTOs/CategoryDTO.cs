namespace CleanArchMvc.Application.DTOs;

// Esta classe pode atuar no projeto também como uma ViewModel para o asp.net mvc
// portanto poderá se valer de data annotations de validação
public class CategoryDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = " The Name is Required")]
    [MinLength(3)]
    [MaxLength(100)]
    [DisplayName("Name")]
    public string? Name { get; set; }
}
