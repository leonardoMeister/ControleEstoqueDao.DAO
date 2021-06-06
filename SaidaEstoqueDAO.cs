using System;
using System.Collections.Generic;
using System.Text;

namespace ControleEstoqueDao.DAO
{
    public class SaidaEstoque
    {
        public int IdSaida { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataSaida { get; set; }
        public string NumeroNf { get; set; }
        public string SerieNf { get; set; }
        public string LojaId { get; set; }
        public decimal PrecoTotal { get; set; }
    }
    /*id_saida_estoque INT IDENTITY(1,1)  NOT NULL,
	cliente_id INT  NOT NULL,
	data_saida DATETIME  NOT NULL,
	numero_nf VARCHAR(50) NULL,
	serie_nf VARCHAR(50)  NULL,
	loja_id INT  NOT NULL,
	preco_total DECIMAL(11,2) NULL,
	PRIMARY KEY(id_saida_estoque),
	CONSTRAINT FK_SaidaEstoqueCliente FOREIGN KEY (cliente_id) REFERENCES tb_cliente(id_cliente),
	CONSTRAINT FK_SaidaEstoqueLoja FOREIGN KEY (loja_id) REFERENCES tb_loja(id_loja),*/
    public class ItensSaida
    {
        public int SaidaEstoqueId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
    /*saida_estoque_id INT  NOT NULL,
	produto_id INT  NOT NULL,
	quantidade INT  NOT NULL,
	valor_unitario DECIMAL(11, 2) NULL,
	CONSTRAINT FK_ItensSaidaEstoque FOREIGN KEY (saida_estoque_id) REFERENCES tb_saida_estoque(id_saida_estoque),
	CONSTRAINT FK_ItensProdutoSaida FOREIGN KEY (produto_id) REFERENCES tb_produto(id_produto),*/
    public class SaidaEstoqueDAO
    {
        public SaidaEstoqueDAO()
        {
        }
    }
}
