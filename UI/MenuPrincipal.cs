namespace GerenciadorAlunos.UI;
using GerenciadorAlunos.Entities;
using GerenciadorAlunos.Repositories;
public class MenuPrincipal
{
    private readonly AlunoRepository _aluno;
    private readonly MensalidadeRepository _mensalidade;
    internal MenuPrincipal(AlunoRepository aluno, MensalidadeRepository mensalidade)
    {
        _aluno = aluno;
        _mensalidade = mensalidade;
    }

    enum ListaOpcoes { GerenciarAlunos = 1, GerenciarMensalidades, Sair };
    public void ExibirMenu()
    {
        bool escolheuSair = false;
        while(escolheuSair == false)
        {
            Console.Clear();
            Console.WriteLine("=== Gerenciador de Alunos ===\n");
            Console.WriteLine("(1) Gerenciar Alunos\n(2) Gerenciar Mensalidades\n(3) Sair");
            Console.Write("\nPor gentileza, escolha a opção desejada: ");
            int.TryParse(Console.ReadLine(), out int opInt);

            if (opInt > 0 && opInt <= 3)
            {
                ListaOpcoes escolha = (ListaOpcoes)opInt;
                switch (escolha)
                {
                    case ListaOpcoes.GerenciarAlunos:
                    MenuAluno menuAluno = new MenuAluno(_aluno);
                    menuAluno.ExibirMenu();
                    break;

                    case ListaOpcoes.GerenciarMensalidades:
                    // aqui vou precisar dos dois, pois busca-se os alunos e se salva as mensalidades
                    MenuMensalidades menuMensalidades = new MenuMensalidades(_aluno, _mensalidade);
                    menuMensalidades.ExibirMenu();
                    break;

                    case ListaOpcoes.Sair:
                    Console.WriteLine("Até uma próxima! :)");
                    escolheuSair = true;
                    break;
                }
            }
        }
    }
}