using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.services
{
    public class Formulario
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string NumberPhone { get; set; }

        public string NumberIndentification { get; set; }

        public string Course { get; set; }

        public DateTime DateCreate { get; set; }
    }
}