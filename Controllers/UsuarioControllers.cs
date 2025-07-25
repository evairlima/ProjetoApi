using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Exo.WebApi.Controllers;


    [Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
 {
    private readonly UsuarioRepository _usuarioRepository;
    private object Expiração;
    private object signing;

    public UsuariosController(UsuarioRepository
    usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    // get -> /api/usuarios
    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_usuarioRepository.Listar());
    }
    // Novo código POST para auxiliar o método de Login.
    public IActionResult Post(Usuario usuario)
    {
        Usuario usuarioBuscado =
        _usuarioRepository.Login(usuario.Email, usuario.Senha);
        if (usuarioBuscado == null)
        {
            return NotFound("E-mail ou senha inválidos!");
        }
        var claims = new[]
        {
            // Armazena na claim o e-mail usuário autenticado.

            new Claim(JwtRegisteredClaimNames.Email,usuarioBuscado.Email),

            // Armazena na claim o id do usuário autenticado.
            new Claim(JwtRegisteredClaimNames.Jti,usuarioBuscado.Id.ToString()),
         };
        // Define a chave de acesso ao token.
        var key = new
        SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chaveautenticacao"));
        // Define as credenciais do token.
        var creds = new SigningCredentials(key,
        SecurityAlgorithms.HmacSha256);
        // Gera o token.
        var token = new JwtSecurityToken(
        issuer: "exoapi.webapi",
        audience: "exoapi.webapi", // Destinatário do token.
        claims: claims,
        expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);
       
        return Ok(
        new { token = new
        JwtSecurityTokenHandler().WriteToken(token) }
        );
    }
[HttpGet("{id}")] // Faz a busca pelo ID.
public IActionResult BuscarPorId(int id)

{

        Usuario usuario = _usuarioRepository.BuscarPorId(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(usuario);
}
// put -> /api/usuarios/{id}
// Atualiza.
[Authorize]
[HttpPut("{id}")]
public IActionResult Atualizar(int id, Usuario usuario)
{
_usuarioRepository.Atualizar(id, usuario);
return StatusCode(204);
}
// delete -> /api/usuarios/{id}
[Authorize]
[HttpDelete("{id}")]
public IActionResult Deletar(int id)

    {

        try
        {
            _usuarioRepository.Deletar(id);
            return StatusCode(204);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

  }

