using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using Api.services;


namespace Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularioCurser : ControllerBase
    {
        private readonly IConfiguration _config;

        public FormularioCurser(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("formulario")]
        public async Task<IActionResult> Formulario([FromBody] Formulario formulario)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = @"INSERT INTO classe (nome,sobrenome,email, telefone, curso, cpf) VALUES (@nome,@sobrenome, @email, @telefone, @curso, @cpf)";

                    var command = new NpgsqlCommand(query, connection);

                    command.Parameters.AddWithValue("@nome", formulario.Name);
                    command.Parameters.AddWithValue("@email", formulario.Email);
                    command.Parameters.AddWithValue("@telefone", formulario.NumberPhone);
                    command.Parameters.AddWithValue("@curso", formulario.Course);
                    command.Parameters.AddWithValue("@cpf", formulario.NumberIndentification);
                    command.Parameters.AddWithValue("@sobrenome", formulario.LastName);

                    await connection.OpenAsync();

                    var lineAfected = await command.ExecuteNonQueryAsync();

                    if (lineAfected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }



            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
        [HttpPost("BuscarAlunos")]
        public async Task<IActionResult> BuscarAlunos([FromBody] Buscar buscar)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var conectionStr = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(conectionStr))
                {
                    var query = @"SELECT * FROM classe WHERE
                    (@Nome IS NULL OR nome LIKE @Nome) AND
                    (@Curso IS NULL OR curso = @Curso)";

                    var command = new NpgsqlCommand(query, connection);

                    command.Parameters.Add("@Nome", NpgsqlTypes.NpgsqlDbType.Text).Value =
    string.IsNullOrWhiteSpace(buscar.NomeDoAluno) ? DBNull.Value : $"%{buscar.NomeDoAluno}%";

                    command.Parameters.Add("@Curso", NpgsqlTypes.NpgsqlDbType.Text).Value =
                        string.IsNullOrWhiteSpace(buscar.Curso) ? DBNull.Value : buscar.Curso;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var ListaDeAlunos = new List<object>();

                        while (await reader.ReadAsync())
                        {
                            ListaDeAlunos.Add(new
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["nome"].ToString(),
                                LastName = reader["sobrenome"].ToString(),
                                Email = reader["email"].ToString(),
                                NumberPhone = reader["telefone"].ToString(),
                                Course = reader["curso"].ToString(),
                                NumberIndentification = reader["cpf"].ToString()
                            });
                        }
                        if (ListaDeAlunos.Count > 0)
                        {
                            return Ok(ListaDeAlunos);
                        }
                        else
                        {
                            return BadRequest(
                                new
                                {
                                    error = "Nenhum aluno encontrado"
                                }



                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }


    }
}