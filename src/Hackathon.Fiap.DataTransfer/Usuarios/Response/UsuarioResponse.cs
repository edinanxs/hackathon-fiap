﻿namespace Hackathon.Fiap.DataTransfer.Usuarios.Response
{
    public class UsuarioResponse
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public UsuarioResponse()
        {

        }
    }
}
