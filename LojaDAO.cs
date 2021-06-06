using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ControleEstoqueDao.DAO
{
    public class Loja
    {
        public Loja()
        {
            IdLoja = 0;
            Cnpj = "";
            Ie = "";
            this.Tipo = 0;
            RazaoSocial = "";
            NomeFantasia = "";
            Telefone = "";
            Email = "";
            EnderecoId = 0;
        }
        public override string ToString()
        {
            return $"Loja: {NomeFantasia}";
        }

        public Loja(int id, string nome)
        {
            this.IdLoja = id;
            this.NomeFantasia = nome;
        }

        public int IdLoja { get; set; }
        public string Cnpj { get; set; }
        public string Ie { get; set; }
        public int Tipo { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int EnderecoId { get; set; }

        public static implicit operator Loja(Funcionario v)
        {
            throw new NotImplementedException();
        }
    }
    public class LojaDAO
    {
        private DbProviderFactory factory;

        public LojaDAO()
        {
        }

        /// <summary>
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="marca">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao, Loja loja)
        {
            factory = DbProviderFactories.GetFactory(provider);
            using (var conexao = factory.CreateConnection()) //Cria conexão
            {
                //Atribui a string de conexão
                conexao.ConnectionString = stringConexao;
                using (var comando = factory.CreateCommand()) //Cria comando
                {
                    comando.Connection = conexao; //Atribui conexão
                    conexao.Open();

                    if (loja.NomeFantasia != "" && loja.NomeFantasia != null)
                    {
                        var nome = comando.CreateParameter();
                        nome.ParameterName = "@nome";
                        nome.Value = "%" + loja.NomeFantasia + "%";
                        comando.Parameters.Add(nome);

                        comando.CommandText = @"SELECT id_loja 'Loja', cnpj 'CNPJ', ie 'IE', razao_social 'Razao Social',
                                            nome_fantasia 'Nome Fantasia', telefone 'Telefone',
                                            email 'Email', tipo 'Tipo'
                                            FROM tb_loja WHERE nome_fantasia like @nome";
                    }
                    else
                    {
                        string auxSql = "";
                        if (loja.IdLoja != 0)
                        {
                            auxSql = $"Where id_loja = '{loja.IdLoja}'";
                        }

                        //Criando query
                        comando.CommandText = @"select id_loja 'Loja', cnpj 'CNPJ', ie 'IE', razao_social 'Razao Social',
                                            nome_fantasia 'Nome Fantasia',tbj.telefone 'Telefone',
                                            tbj.email 'Email', tbe.cidade 'Cidade' , tbe.id_endereco 'Id Endereco', tipo 'Tipo'
                                            from tb_loja tbj 
                                            inner join tb_endereco tbe
                                            on tbj.endereco_id = tbe.id_endereco " + " " + auxSql;

                    }
                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            }
        }

        /// <summary>
        /// Removendo do banco
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="strConnection"></param>
        /// <param name="id"></param>
        public void RemoverDbProvider(string provider, string stringConexao, int id)
        {
            factory = DbProviderFactories.GetFactory(provider);
            using (var conexao = factory.CreateConnection())              //Cria conexão
            {
                //Atribui a string de conexão
                conexao.ConnectionString = stringConexao;
                using (var comando = factory.CreateCommand())            //Cria comando
                {
                    //Atribui conexão
                    comando.Connection = conexao;

                    //Abre conexão
                    conexao.Open();
                    //Script para inserir com os parâmetros adicionados
                    comando.CommandText = $"delete from tb_endereco where id_endereco = {id}";
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }
        }

        /// <summary>
        /// INserindo no banco
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="produto"></param>
        public void InserirDbProvider(string provider, string stringConexao, Loja loja)
        {
            factory = DbProviderFactories.GetFactory(provider);
            using (var conexao = factory.CreateConnection())              //Cria conexão
            {
                //Atribui a string de conexão
                conexao.ConnectionString = stringConexao;
                using (var comando = factory.CreateCommand())            //Cria comando
                {
                    //Atribui conexão
                    comando.Connection = conexao;

                    //Adiciona parâmetro (@campo e valor)
                    var cnpj = comando.CreateParameter();
                    cnpj.ParameterName = "@Cnpj";
                    cnpj.Value = loja.Cnpj;
                    comando.Parameters.Add(cnpj);

                    var email = comando.CreateParameter();
                    email.ParameterName = "@Email";
                    email.Value = loja.Email;
                    comando.Parameters.Add(email);

                    var enderecoId = comando.CreateParameter();
                    enderecoId.ParameterName = "@EnderecoId";
                    enderecoId.Value = loja.EnderecoId;
                    comando.Parameters.Add(enderecoId);

                    var ie = comando.CreateParameter();
                    ie.ParameterName = "@Ie";
                    ie.Value = loja.Ie;
                    comando.Parameters.Add(ie);

                    var nomeFantasia = comando.CreateParameter();
                    nomeFantasia.ParameterName = "@NomeFantasia";
                    nomeFantasia.Value = loja.NomeFantasia;
                    comando.Parameters.Add(nomeFantasia);

                    var razaoSocial = comando.CreateParameter();
                    razaoSocial.ParameterName = "@RazaoSocial";
                    razaoSocial.Value = loja.RazaoSocial;
                    comando.Parameters.Add(razaoSocial);

                    var telefone = comando.CreateParameter();
                    telefone.ParameterName = "@Telefone";
                    telefone.Value = loja.Telefone;
                    comando.Parameters.Add(telefone);

                    var tipo = comando.CreateParameter();
                    tipo.ParameterName = "@Tipo";
                    tipo.Value = loja.Tipo;
                    comando.Parameters.Add(tipo);

                    //Abre conexão
                    conexao.Open();

                    if (loja.IdLoja != 0)
                    {
                        var id = comando.CreateParameter();
                        id.ParameterName = "@id";
                        id.Value = loja.IdLoja;
                        comando.Parameters.Add(id);

                        comando.CommandText = @"UPDATE tb_loja SET cnpj= @Cnpj , ie = @Ie , tipo = @Tipo ,
                                                razao_social = @RazaoSocial , nome_fantasia = @NomeFantasia ,
                                                telefone = @Telefone , email = @Email
                                                WHERE id_loja = @Id; ";
                    }
                    else
                    {
                        //Criando query
                        comando.CommandText = @"insert into tb_loja (cnpj,ie,tipo,razao_social,nome_fantasia,
                                            telefone,email,endereco_id)
                                            values (@Cnpj,@Ie,@Tipo,@RazaoSocial,@NomeFantasia,
                                            @Telefone,@Email,@EnderecoId);";
                    }

                    //executa o comando no banco
                    comando.ExecuteNonQuery();
                }
            }
        }


    }
}
