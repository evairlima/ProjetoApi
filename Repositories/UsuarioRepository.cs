using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exo.WebApi.Contexts;
using Exo.WebApi.Models;
 
namespace Exo.WebApi.Repositories
{
    public class UsuarioRepository
    {
        //Campo privado usado somente leitura que armazena a instancia do contexto
        private readonly ExoContext _context;

        public UsuarioRepository(ExoContext context)
        {
            _context = context;
        }

        public Usuario Login(string email, string senha)
        {
            //Buscando usuario no banco de dados cujo email e senha sejam iguais aos informados
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        }

        public List<Usuario> Listar()
        {
            return _context.Usuarios.ToList();
        }

        public void Cadastrar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario BuscarPorId(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public void Atualizar(int id, Usuario usuario)
        {
            Usuario usuarioBuscado = _context.Usuarios.Find(id);
            if (usuarioBuscado != null)
            {
                usuarioBuscado.Email = usuario.Email;
                usuarioBuscado.Senha = usuario.Senha;

                _context.Usuarios.Update(usuarioBuscado);
                _context.SaveChanges();

            }
        }
        public void Deletar(int id)
        {
            Usuario usuarioBuscado = _context.Usuarios.Find(id);
            if (usuarioBuscado != null)
            {
                _context.Usuarios.Remove(usuarioBuscado);
                _context.SaveChanges();
            }
        }
 
    }
}