using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }

        [
            Required(ErrorMessage = "O campo {0} é obrigatório."),
            //MinLength(3, ErrorMessage = "O campo {0} deve ter no mínimo 3 caracteres."),
            //MaxLength(50, ErrorMessage = "O campo {0} deve ter no mmáximo 50 caracteres.")
            StringLength(50, MinimumLength = 3, ErrorMessage = "O campo {0} deve conter de 3 a 50 caracteres")
        ]
        public string Tema { get; set; }

        [Display(Name = "Quantidade pessoas")]
        [Range(1, 20000, ErrorMessage = "O campo {0} deve estar de 1 a 20000")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Imagem inválida. (gif, jpeg, bmp, png)")]
        public string ImagemUrl { get; set; }

        [Required(ErrorMessage = "O campo {} é obrigatório")]
        [Phone(ErrorMessage = "Informe um telefone válido")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um email válido")]
        public string Email { get; set; }

        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> PalestrantesEventos { get; set; }
    }
}
