using NerdStore.Core.DomainObjects;
using System.Collections;
using System.Collections.Generic;

namespace NerdStore.Catalogo.Domain
{
	public class Categoria : Entity
	{
		public string Nome { get; private set; }
		public int Codigo { get; private set; }

		//EF Relations
		public ICollection<Produto> Produtos { get; set; }

		protected Categoria() { }

		public Categoria(string nome, int codigo)
		{
			Nome = nome;
			Codigo = codigo;
			Validar();
		}

		public override string ToString()
		{
			return $"{Nome} - {Codigo}";
		}

		public void Validar()
		{
			Validacoes.ValidarSeVazio(Nome, "o campo Nome da catetogia não pode ser vazio");
			Validacoes.ValidarSeIgual(Codigo, 0, "o campo codigo da catetogia não pode ser 0");
		}
	}
}
