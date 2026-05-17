namespace GerenciadorAlunos.UI;
using GerenciadorAlunos.Entities;
using GerenciadorAlunos.Repositories;

public class MenuAluno
{
    private readonly AlunoRepository _aluno;

    // Construtor
    internal MenuAluno(AlunoRepository aluno)
    {
        _aluno = aluno;
    }

    enum ListaOpcoes { CadastrarAluno = 1, ListarAlunos, BuscarAluno, EditarAluno, DeletarAluno, Voltar}
    // blz, agora que fiz a refatoração dos repositorys, preciso tornar meu menu e seus métodos assincronos (async/task/await)
    public async Task ExibirMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Menu de Alunos ===\n");
            Console.WriteLine("(1) Cadastrar Aluno(a)\n(2) Relatório de Alunos(as)\n(3) Pesquisar Aluno(a)\n(4) Editar Aluno(a)\n(5) Deletar Aluno(a)\n(6) Voltar");
            Console.Write("\nPor gentileza, escolha a opção desejada: ");
            int.TryParse(Console.ReadLine(), out int opInt);

            if (opInt > 0 && opInt <= 6)
            {
                ListaOpcoes escolha = (ListaOpcoes)opInt;
                switch (escolha)
                {
                    case ListaOpcoes.CadastrarAluno:
                    // tenho q adicionar os await antes de chamar qualquer método
                    await CadastrarAluno();
                    break;

                    case ListaOpcoes.ListarAlunos:
                    await ListarAlunos();
                    break;

                    case ListaOpcoes.BuscarAluno:
                    await BuscarAluno();
                    break;

                    case ListaOpcoes.EditarAluno:
                    await EditarAluno();
                    break;

                    case ListaOpcoes.DeletarAluno:
                    await DeletarAluno();
                    break;

                    case ListaOpcoes.Voltar:
                    return;
                }
            }
        }
    }

    public async Task <Aluno> CapturarAlunoSelecionado()
    {
        while (true)
        {
            Console.Write("Por gentileza, digite o ID ou o nome do(a) aluno(a): ");
            string termoBusca = Console.ReadLine();

            List<Aluno> alunosEncontrados = await _aluno.SelecionarAsync(termoBusca);

            if (alunosEncontrados.Count == 0)
            {
                Console.WriteLine("\nHmm.. Não encontrei nenhum aluno com esse nome ou ID.");
                Console.Write("Deseja fazer uma nova pesquisa? (S/N): ");
                if (Console.ReadLine().ToUpper().Trim() == "S") continue;
                else return null; // Retorna nulo se o usuário desistir
            }
            
            if (alunosEncontrados.Count == 1)
            {
                return alunosEncontrados[0]; // Retorna o aluno direto e encerra a função!
            }
            
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome:");
            // Usando foreach tradicional
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }
            // Usando LAMBDA
            //alunosEncontrados.ForEach(a => Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}"));

            Console.Write("\nPor gentileza, digite o ID exato do aluno: ");
            if (int.TryParse(Console.ReadLine(), out int idDigitado))
            {
                // Usando LAMBDA
                return alunosEncontrados.FirstOrDefault(a => a.Id == idDigitado);
            }

            Console.WriteLine("\nHmm.. parece que esse ID é inválido!");
            Console.Write("Deseja tentar pesquisar novamente? (S/N): ");
            if (Console.ReadLine().ToUpper().Trim() == "S") continue;
            else return null; // Desistiu de tentar, retorna nulo
        }
    }
    private async Task CadastrarAluno() 
    {
        Console.Clear();
        Console.WriteLine("=== Menu de Cadastro de Aluno ===\n");
        Console.Write("Insira o nome do aluno(a): ");
        string nomeAluno = Console.ReadLine();
        Console.Write("Insira o CPF do aluno(a) - 11 números: ");
        string cpfAluno = Console.ReadLine();
        Console.Write("Insira o e-mail do aluno(a): ");
        string emailAluno = Console.ReadLine();
        Console.Write("Insira o celular do aluno(a) - (DDD + 9 números): ");
        string celularAluno = Console.ReadLine();

        while (true)
        {
            Console.WriteLine("\n=== Novo aluno a ser cadastrado ===");
            Console.WriteLine($"Nome do Aluno: {nomeAluno}\nCPF: {cpfAluno}\nE-mail: {emailAluno}\nCelular: {celularAluno}\n\n");
            Console.Write("Prosseguir com o cadastramento? (S/N): ");

            string resposta = Console.ReadKey().KeyChar.ToString().ToUpper().Trim(); // Aplicada a ideia sugerida pelo Sergio: Console.ReadKey. Tive que passar pra um .ToString se não estava dando erro. Ideia de prevenção de sempre passar para maiuscula e retirar espaços
            Console.ReadLine(); // estava cadastrando direto, sem mostrar nenhuma mensagem.
            if (resposta == "S")
            {
                // agora eu instancio minha classe Aluno
                Aluno novoAluno = new Aluno
                {
                    Nome = nomeAluno,
                    Cpf = cpfAluno,
                    Email = emailAluno,
                    Celular = celularAluno
                };
                // e faço o método normalmente
                // Perguntar pro Sérgio: devo colocar os try/catch aqui ou lá no meu repository? Pelas minhas pesquisas, encontrei algumas divergências e fiquei em dúvida. 
                await _aluno.CadastrarAsync(novoAluno);

                Console.WriteLine("=== Cadastro efetuado com sucesso! ===");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                Console.ReadLine();
                break;
            }
            else if (resposta == "N")
            {
                Console.WriteLine("Cadastramento cancelado!");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                Console.ReadLine();
                break;
            }
            else
            {
                Console.WriteLine("Hmm... parece que essa opção é inválida! Por gentileza, digite S ou N");
            }

        }
    }
    private async Task ListarAlunos() // Criada uma classe Aluno.cs e posto os get/sets nos campos (nome, cpf, etc) para servirem como molde.
    {
        Console.Clear();
        Console.WriteLine("=== Relatório Geral de Alunos(as) ===\n");
        List<Aluno> alunosDoBanco = await _aluno.ListarAsync(); // eu busco a lista Aluno, coloco ela numa variável de nome alunosDoBanco e dou a ela o valor da função aluno.Listar.
        
        if (alunosDoBanco.Count == 0)
        {
            Console.WriteLine("Hmm.. parece que ainda não há alunos(as) cadastrados(as).");
            Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
            return;
        }
        // Foreach tradicional
        foreach (Aluno a in alunosDoBanco) // para cada Aluno que guardei na variável a presente no banco, vou printar seu ID, nome, cpf, email, etc.
        {
            Console.WriteLine($"ID: {a.Id}\nNome: {a.Nome}\nCPF: {a.Cpf}\nE-mail: {a.Email}\nCelular: {a.Celular}");
            Console.WriteLine("===============================");
        }
        // Usando LAMBDA
        // alunosDoBanco.ForEach(a => Console.WriteLine($"ID: {a.Id}\nNome: {a.Nome}\nCPF: {a.Cpf}\nE-mail: {a.Email}\nCelular: {a.Celular}"));

        Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu principal.");
        Console.ReadLine(); // achei que eu tava com problemas na construção da minha listagem, mas, na verdade, a listagem aparecia tao rápido e voltava tão rápido ao menu q nao aparecia td. Dai coloquei um readline no fim.
    }
    private async Task BuscarAluno() 
    // Hoje está programado para que busque/apareça todos os alunos com aquele nome ou ID.
    // Mas se eu quisesse deixar para que a pesquisa retorne apenas um único, poderia usufruir da CapturarAlunoSelecionado();
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Pesquisa de Alunos(as) ===\n");
            Console.Write("Por gentileza, digite o ID ou nome do(a) aluno(a): ");
            string termoBusca = Console.ReadLine();

            List<Aluno> alunosDoBanco = await _aluno.SelecionarAsync(termoBusca); // eu busco a lista Aluno, coloco ela numa variável de nome alunosDoBanco e dou a ela o valor da função aluno.SelecionarAsync, passando o termo de busca.
            // Se não encontrar nada, já avisar pro usuário e oportunizar uma nova pesquisa.
            if (alunosDoBanco.Count == 0)
            {
                Console.WriteLine("\nHmm.. parece que ainda não há alunos(as) com esse ID ou nome.");
                Console.WriteLine("Deseja realizar uma nova pesquisa? (S/N): "); // NOVIDADE
                string resposta = Console.ReadKey().KeyChar.ToString().ToUpper().Trim();
                Console.ReadLine();
                if (resposta == "S") continue;
                else return;
            }

            Console.WriteLine("\n=== Resultados da Pesquisa ===");
            // Tradicional
            foreach (Aluno a in alunosDoBanco) // para cada Aluno que guardei na variável "a" presente no banco, vou printar seu ID, nome, cpf, email, etc.
            {
                Console.WriteLine($"ID: {a.Id}\nNome: {a.Nome}\nCPF: {a.Cpf}\nE-mail: {a.Email}\nCelular: {a.Celular}");
                Console.WriteLine("===============================");
            }
            // Usando LAMBDA
            // alunosDoBanco.ForEach(a => Console.WriteLine($"ID: {a.Id}\nNome: {a.Nome}\nCPF: {a.Cpf}\nE-mail: {a.Email}\nCelular: {a.Celular}"));
            Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu principal.");
            Console.ReadLine();

            break;
        }

    }
    private async Task EditarAluno()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Edição de Cadastros de Alunos(as) ===\n");
            Aluno alunoAtual = await CapturarAlunoSelecionado();

            if (alunoAtual == null) return;

            Console.WriteLine("\nObservação: será possível deixar em branco e pressionar Enter para manter o dado atual.");

            Console.Write($"Nome atual ({alunoAtual.Nome}): ");
            string novoNome = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoNome)) novoNome = alunoAtual.Nome; 

            Console.Write($"CPF atual ({alunoAtual.Cpf}): ");
            string novoCpf = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoCpf)) novoCpf = alunoAtual.Cpf;

            Console.Write($"E-mail atual ({alunoAtual.Email}): ");
            string novoEmail = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoEmail)) novoEmail = alunoAtual.Email;


            Console.Write($"Celular atual ({alunoAtual.Celular}): ");
            string novoCelular = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoCelular)) novoCelular = alunoAtual.Celular;

            // Perguntar pro Sérgio: mesma coisa de antes - transformar aqui em try/catch ou manter lá no repository?
            await _aluno.AlterarAsync(alunoAtual);

            Console.WriteLine("\nAlteração realizada com sucesso!");
            Console.Write("Por gentileza, pressione Enter para voltar ao menu principal.");
            Console.ReadLine();

            break; // quebra do looping. Sem ele, sempre ficava retornando para editar o aluno novamente.
        }
    }
    private async Task DeletarAluno()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Deleção de Cadastros de Alunos(as) ===\n");
            Aluno alunoAtual = await CapturarAlunoSelecionado();

            if (alunoAtual == null) return;
            // NOVIDADE: validação extra para confirmação
            Console.Clear();
            Console.WriteLine("Dados do(a) aluno(a) que será deletado: ");
            Console.WriteLine($"ID: {alunoAtual.Id} | Nome: {alunoAtual.Nome} | CPF: {alunoAtual.Cpf} | E-mail: {alunoAtual.Email} | Celular: {alunoAtual.Celular}");
            Console.Write("Atenção: essa ação não poderá ser desfeita! Deseja seguir com a exclusão do cadastro? (S/N): ");
            string resposta = Console.ReadKey().KeyChar.ToString().ToUpper().Trim();
            Console.ReadLine();

            // Perguntar pro Sérgio: msm coisa das demais - try/catch?
            if (resposta == "S")
            {
                await _aluno.DeletarAsync(alunoAtual.Id);
                Console.WriteLine("\nDeleção realizada com sucesso!");
            }
            else
            {
                Console.WriteLine("Operação cancelada! Por gentileza, pressione Enter para retornar ao menu principal.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
            Console.ReadLine();

            break;
        }
    }
}