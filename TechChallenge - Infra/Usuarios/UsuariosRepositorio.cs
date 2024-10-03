﻿using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TC_DataTransfer.Usuarios.Request;
using TC_Domain.Usuarios.Entidades;
using TC_Domain.Usuarios.Repositorios;
using TC_Domain.Utils;
using TC_IOC.Bibliotecas;
using TC_IOC.DBContext;

namespace TC_Infra.Usuarios
{
    public class UsuariosRepositorio(DapperContext dapperContext) : RepositorioDapper<Usuario>(dapperContext), IUsuariosRepositorio
    {
        public Usuario RecuperarUsuario(string email, string hash)
        {
            StringBuilder sql = new($@"
                           SELECT u.id as Id,
		                    u.nome as Nome,
		                    u.email as Email,
		                    u.hash as Hash,
		                    u.data_criacao as DataCriacao,
		                    u.permissao as Permissao
                    FROM techchallenge.usuarios u
	                    WHERE u.email = @email
	                    AND u.hash = @hash");
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("email", email);
            dynamicParameters.Add("hash", hash);
            
            return session.QueryFirstOrDefault<Usuario>(sql.ToString(), dynamicParameters);
        }
        public PaginacaoConsulta<Usuario> ListarUsuarios(UsuarioListarRequest request)
        {
            StringBuilder sql = new($@"
                           SELECT u.id as Id,
		                    u.nome as Nome,
		                    u.email as Email,
		                    u.hash as Hash,
		                    u.data_criacao as DataCriacao,
		                    u.permissao as Permissao
                    FROM techchallenge.usuarios u
	                    WHERE 1 = 1");
            
            if (!request.Email.IsNullOrEmpty())
            {
                sql.AppendLine($@" AND u.email = '{request.Email}' "); 
            }

            if (!request.NomeUsuario.IsNullOrEmpty())
            {
                sql.AppendLine($@" AND u.nome = '{request.NomeUsuario}' ");
            }
 
            return ListarPaginado(sql.ToString(), request.Pg, request.Qt, request.CpOrd, request.TpOrd.ToString());
        }
    }
}
