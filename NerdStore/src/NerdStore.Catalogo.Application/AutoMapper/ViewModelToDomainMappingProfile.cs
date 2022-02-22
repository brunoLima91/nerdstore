using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
	public class ViewModelToDomainMappingProfile : Profile
	{
		public ViewModelToDomainMappingProfile()
		{
			CreateMap<CategoriaViewModel, Categoria>()
				.ConvertUsing(c => new Categoria(c.Nome, c.Codigo));

			CreateMap<ProdutoViewModel, Produto>()
				.ConvertUsing(p =>
				new Produto(p.Nome, p.Descricao, p.Ativo,
				p.Valor, p.CategoriaId, p.DataCadastro,
				p.Imagem, new Dimensoes(p.Altura, p.Largura, p.Profundidade)));
		}
	}
}
