using System.ComponentModel.DataAnnotations;

namespace ccytet.Server.Models
{
    public class NoticiaImagen
    {
        [Key]
        public string IdNoticiaImagen           { get; set; }
        public string ImagenPath                { get; set; }

        //RELATIONS
        public virtual Noticia Noticia          { get; set; }
        public string IdNoticia                 { get; set; }
    }
}