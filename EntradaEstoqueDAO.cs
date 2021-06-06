using System;
using System.Collections.Generic;
using System.Text;

namespace ControleEstoqueDao.DAO
{
    public class EntradaEstoque
    {	
		public int IdEntradaEstoque { get; set; }
		public int FornecedorId { get; set; }
		public string NumeroNf { get; set; }
		public string SerieNf { get; set; }
		public DateTime DataNf { get; set; }
		public string ChaveNf { get; set; }
		public int LojaId { get; set; }
		public DateTime DataEntrada { get; set; }
		public string Observacao { get; set; }
		public decimal ValorTotal { get; set; }
	}
	public class ItensEntrada
    {
		public int EntradasEstoqueId { get; set; }
		public int ProdutoId { get; set; }
		public int Quantidade { get; set; }
		public decimal ValorUnitario { get; set; }
		public int MarcaId { get; set; }
		public string Modelo { get; set; }
		public int Lote { get; set; }
		public DateTime DataValidade { get; set; }
	}
	/*entrada_estoque_id INT  NOT NULL,
	produto_id INT  NOT NULL,
	quantidade INT  NOT NULL,
	valor_unitario DECIMAL(11, 2)  NOT NULL,
	marca_id INT  NULL,
	modelo VARCHAR(100) NULL,
	lote INT NULL,
	data_validade DATE NULL,
	CONSTRAINT FK_ItensEntradaEstoque FOREIGN KEY (entrada_estoque_id) REFERENCES tb_entrada_estoque(id_entrada_estoque),
	CONSTRAINT FK_ItensEntradaProduto FOREIGN KEY (produto_id) REFERENCES tb_produto(id_produto),
	CONSTRAINT FK_ItensEntradaMarca   FOREIGN KEY (marca_id) REFERENCES tb_marca(id_marca),
	 */

	/*
   id_entrada_estoque INT IDENTITY(1,1) NOT NULL,
	fornecedor_id INT  NOT NULL,
	numero_nf VARCHAR(50)  NOT NULL,
	serie_nf VARCHAR(50)  NULL,
	data_nf DATETIME  NOT NULL,
	chave_nf VARCHAR(200) NULL,
	loja_id INT  NOT NULL,
	data_entrada DATETIME  NOT NULL,
	observacao VARCHAR(200) NULL,
	valor_total DECIMAL(11,2) NULL,
	PRIMARY KEY(id_entrada_estoque),
	CONSTRAINT FK_EntradaEstoqueFornecedor FOREIGN KEY (fornecedor_id) REFERENCES tb_fornecedor(id_fornecedor),
	CONSTRAINT FK_EntradaEstoqueLoja FOREIGN KEY (loja_id) REFERENCES tb_loja(id_loja),
     */
	public class EntradaEstoqueDAO
    {
        public EntradaEstoqueDAO()
        {
        }
    }
}
