using System;
using System.Collections.Generic;
using System.Text;

namespace ControleEstoqueDao.DAO
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int EnderecoId { get; set; }
    }
    /*
     id_cliente INT IDENTITY(1,1) NOT NULL,
	nome VARCHAR(150)  NOT NULL,
	cpf CHAR(11)  NOT NULL,
	telefone CHAR(16) NULL,
	email VARCHAR(100) NULL,
	endereco_id INT NULL,
	PRIMARY KEY(id_cliente),
	CONSTRAINT FK_ClienteEndereco FOREIGN KEY (endereco_id) REFERENCES tb_endereco(id_endereco),
     */
    public class ClienteDAO
    {
        public ClienteDAO()
        {
        }
    }
}
