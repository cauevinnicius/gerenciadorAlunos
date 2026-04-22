public class Menu
{

    enum ListaOpcoes { CadastrarAluno = 1, ListarAlunos, BuscarAluno, EditarAluno, DeletarAluno, LancarMensalidade, RegistrarPagamentoMensalidade, ListarMensalidadeAluno, VerificaPendencias, EditarMensalidade, ExcluirMensalidade, Sair }

    private readonly AlunoSQL _aluno;
    private readonly MensalidadeSQL _mensalidade;


    // O internal foi sugerido pelo VS Code. Estava com erro na minha public Menu. Fui mais afundo pesquisar e vi que nas minhas classes de Aluno SQL e MensalidadeSQL eu não tinha posto "public" - subentende-se que seria internal.
    internal Menu(AlunoSQL aluno, MensalidadeSQL mensalidade)
    {
        _aluno = aluno;
        _mensalidade = mensalidade;
    }
    public void ExibirMenu()
    {
        bool escolheuSair = false;
        while (escolheuSair == false)
        {
            Console.Clear();
            Console.WriteLine("=== Gerenciador de Alunos ===\n");
            Console.WriteLine("(1) Cadastrar Aluno\n(2) Listar Alunos\n(3) Buscar Aluno\n(4) Editar Aluno\n(5) Excluir Aluno\n(6) Lançar Mensalidade\n(7) Registar Pagamento\n(8) Listar Mensalidades\n(9) Verificar pendências/atrasos\n(10) Editar Mensalidade\n(11) Deletar Mensalidade\n(12) Sair");
            Console.Write("\nPor gentileza, escolha a opção desejada: ");
            int.TryParse(Console.ReadLine(), out int opInt);

            if (opInt > 0 && opInt <= 12)
            {
                ListaOpcoes escolha = (ListaOpcoes)opInt;
                switch (escolha)
                {
                    // quis deixar o código o mais limpo possível. então inseri apenas a função de cadastrar em cada case
                    case ListaOpcoes.CadastrarAluno:
                        CadastrarAluno();
                        break;
                    case ListaOpcoes.ListarAlunos:
                        ListarAlunos();
                        break;
                    case ListaOpcoes.BuscarAluno:
                        BuscarAluno();
                        break;
                    case ListaOpcoes.EditarAluno:
                        EditarAluno();
                        break;
                    case ListaOpcoes.DeletarAluno:
                        DeletarAluno();
                        break;
                    case ListaOpcoes.LancarMensalidade:
                        LancarMensalidade();
                        break;
                    case ListaOpcoes.RegistrarPagamentoMensalidade:
                        RegistrarPagamentoMensalidade();
                        break;
                    case ListaOpcoes.ListarMensalidadeAluno:
                        ListarMensalidadeAluno();
                        break;
                    case ListaOpcoes.VerificaPendencias:
                        VerificaPendencias();
                        break;
                    case ListaOpcoes.EditarMensalidade:
                        EditarMensalidade();
                        break;
                    case ListaOpcoes.ExcluirMensalidade:
                        ExcluirMensalidade();
                        break;
                    case ListaOpcoes.Sair:
                        Console.WriteLine("Até uma próxima! :)");
                        escolheuSair = true;
                        break;
                }
            }
        }
    }

    private void CadastrarAluno() // a minha ideia era fazer uma classe Cadastro e uma função CadastrarALuno(). Pesquisando sobre, neste projeto seria over-engeneering 
    {
        Console.Clear();
        Console.WriteLine("=== Menu de Cadastro de Aluno ===\n");
        Console.Write("Insira o nome do aluno(a): ");
        string nomeAluno = Console.ReadLine();
        //Console.WriteLine("Insira o CPF do aluno(a) - 11 números: ");
        // primeira ideia
        // string? strCpfAluno = Console.ReadLine(1, 11);
        // int.TryParse(strCpfAluno, out int intCpfAlunoIdBusca);
        // segunda ideia - deixar simplesmente funcional
        //int.TryParse(Console.ReadLine(), out int cpfAluno); // queria limitar o número de caracteres para 11. Fui pesquisar como fazer.
        //terceira ideia - fazer um laço para ter o controle das situações que o usuário poderia se deparar e fazer.
        string cpfFormatado = ""; // uma string vazia p formatação. Nesse ponto eu busquei entender porquê usarmos uma string invés de um int. 
        bool cpfValido = false;

        while (cpfValido == false) // seria legal uma revisão aqui
        {
            Console.Write("Insira o CPF do aluno(a) - 11 números: ");
            string cpfAluno = Console.ReadLine();
            // a ideia agora seria prevenir que houvesse a digitação do usuário de pontos e traços. Depois pensei q não fazia tanto sentido pq eu logo abaixo faria a inserção novamente...
            string cpfLimpo = cpfAluno.Replace(".", "").Replace("-", "").Trim();
            // dai validar se deu certo mesmo, ou seja, tem 11 caracteres
            if (cpfLimpo.Length == 11)
            {
                cpfFormatado = cpfLimpo.Insert(3, ".").Insert(7, ".").Insert(11, "-");
                cpfValido = true;
            }
            else
            {
                Console.WriteLine("Hmm.. CPF inválido! Por gentileza, certifique-se que há onze números dígitados.");
            }
        }
        Console.Write("Insira o e-mail do aluno(a): ");
        string emailAluno = Console.ReadLine();
        Console.Write("Insira o celular do aluno(a) - apenas números: ");
        int.TryParse(Console.ReadLine(), out int celularAluno);

        while (true)
        {
            Console.WriteLine("\n=== Novo aluno a ser cadastrado ===");
            Console.WriteLine($"Nome do Aluno: {nomeAluno}\nCPF: {cpfValido}\nE-mail: {emailAluno}\nCelular: {celularAluno}\n\n");
            Console.Write("Prosseguir com o cadastramento? (S/N): ");
            string resposta = Console.ReadLine().ToUpper().Trim(); // ideia de prevenção de sempre passar para maiuscula e retirar espaços.
            if (resposta == "S" || resposta == "SIM" || resposta == "Sim")
            {
                _aluno.Cadastrar(nomeAluno, cpfFormatado, emailAluno, celularAluno);
                Console.WriteLine("=== Cadastro efetuado com sucesso! ===");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                break;
            }
            else if (resposta == "N" || resposta == "NÃO" || resposta == "NAO")
            {
                Console.WriteLine("Cadastramento cancelado!");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                //return;
            }
            else
            {
                Console.WriteLine("Hmm... parece que essa opção é inválida! Por gentileza, digite SIM ou NAO");
            }

        }
    }
    private void ListarAlunos() // a minha primeira ideia não foi bem sucedida. Busquei pesquisar algumas alternativas para funcionar. A ideia sugerida foi criar uma classe Aluno.cs e por get/sets nos campos (nome, cpf, etc)
    {
        Console.Clear();
        Console.WriteLine("=== Listagem de Alunos(as) ===\n");
        List<Aluno> alunosDoBanco = _aluno.Listar(); // eu busco a lista Aluno, coloco ela numa variável de nome alunosDoBanco e dou a ela o valor da função aluno.Listar.
        if (alunosDoBanco.Count == 0)
        {
            Console.WriteLine("Hmm.. parece que ainda não há alunos(as) cadastrados(as).");
            Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
            return;
        }
        foreach (Aluno a in alunosDoBanco) // para cada Aluno que guardei na variável a presente no banco, vou printar seu ID, nome, cpf, email, etc.
        {
            Console.WriteLine($"ID: {a.Id}\nNome: {a.Nome}\nCPF: {a.Cpf}\nE-mail: {a.Email}\nCelular: {a.Celular}");
            Console.WriteLine("===============================");
        }

        Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu principal.");
        Console.ReadLine(); // achei que eu tava com problemas na construção da minha listagem, mas, na verdade, a listagem aparecia tao rápido e voltava tão rápido ao menu q nao aparecia td. Dai coloquei um readline no fim.
    }

    private void BuscarAluno()
    {
        Console.Clear();
        Console.WriteLine("=== Pesquisa de Alunos(as) ===\n");
        Console.WriteLine("Por gentileza, digite o ID ou nome do(a) aluno(a): \n");
        List<Aluno> alunosDoBanco = _aluno.Selecionar(Console.ReadLine()); // eu busco a lista Aluno, coloco ela numa variável de nome alunosDoBanco e dou a ela o valor da função aluno.Listar.
        //uma validação, inicialmente. Se não encontrar nada, já avisar pro usuário
        if (alunosDoBanco.Count == 0)
        {
            Console.WriteLine("Hmm.. parece que ainda não há alunos(as) com esse ID ou nome.");
            Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
            Console.ReadLine();
            return;
        }
        foreach (Aluno a in alunosDoBanco) // para cada Aluno que guardei na variável "a" presente no banco, vou printar seu ID, nome, cpf, email, etc.
        {
            Console.WriteLine($"ID: {a.Id}\nNome: {a.Nome}\nCPF: {a.Cpf}\nE-mail: {a.Email}\nCelular: {a.Celular}");
            Console.WriteLine("===============================");
        }
        Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu principal.");
        Console.ReadLine();
    }
    private void EditarAluno()
    {
        Console.Clear();
        Console.WriteLine("=== Edição de Cadastros de Alunos(as) ===\n");
        Console.WriteLine("Digite o ID ou o nome do aluno(a) que deseja alterar: ");
        string termoBusca = Console.ReadLine(); // nessa daqui, diferente da BuscarAluno, quis criar uma variável pra guardar o que o usuário digitou.

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca); // aqui. Na Buscar eu coloquei o Console.ReadLine direto. Ver com o Sérgio é melhor (acredito q essa aqui)
         // pra nao precisaar refazer tudo, usei a função do selecionar


        if (alunosEncontrados.Count == 0)
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null; // criei um objeto da classe Aluno nulo - pesquisei na internet para auxílio

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0]; // ENTENDER!!!
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }
        }

        Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja editar: ");
        if (int.TryParse(Console.ReadLine(), out int idEscolhido)) // PRECISO ENTENDER ESSA SITUAÇÃO AQUI!
        {
            foreach (Aluno a in alunosEncontrados)
            {
                if (a.Id == idEscolhido)
                {
                    alunoAtual = a; // 
                    break;
                }
            }
        }
        // validacao para caso o haja a digitação de um ID q não exista ou algo aleatorio
        if (alunoAtual == null)
        {
            Console.WriteLine("\nHmm.. parece que esse ID é inválido! Tente novamente.");
            Console.ReadLine();
            return;
        }

        //PAREI AQUI. PRECISO ENTENDER ESSAS SITUAÇÕES ABAIXO!
        Console.WriteLine("\nDeixe em branco e pressione Enter para manter o dado atual.");

        Console.Write($"Nome atual ({alunoAtual.Nome}): ");
        string novoNome = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoNome)) novoNome = alunoAtual.Nome;

        Console.Write($"CPF atual ({alunoAtual.Cpf}): ");
        string novoCpf = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoCpf)) novoCpf = alunoAtual.Cpf;

        Console.Write($"E-mail atual ({alunoAtual.Email}): ");
        string novoEmail = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoEmail)) novoEmail = alunoAtual.Email;

        // O Truque do Inteiro (Celular)
        Console.Write($"Celular atual ({alunoAtual.Celular}): ");
        string strCelular = Console.ReadLine();
        int novoCelular = alunoAtual.Celular; // Por padrão, a variável já começa com o número antigo
        
        if (!string.IsNullOrWhiteSpace(strCelular))
        {
            // Se ele digitou algo, tentamos converter. Se der certo, substitui a variável
            int.TryParse(strCelular, out novoCelular); 
        }

        // 4. Salvando no Banco de Dados
        _aluno.Alterar(novoNome, novoCpf, novoEmail, novoCelular, alunoAtual.Id);
        
        Console.WriteLine("\nAlteração realizada com sucesso!");
        Console.ReadLine();
    }

    private void DeletarAluno() { Console.WriteLine("Teste"); }
    private void LancarMensalidade() { Console.WriteLine("Teste"); }
    private void RegistrarPagamentoMensalidade() { Console.WriteLine("Teste"); }
    private void ListarMensalidadeAluno() { Console.WriteLine("Teste"); }
    private void VerificaPendencias() { Console.WriteLine("Teste"); }
    private void EditarMensalidade() { Console.WriteLine("Teste"); }
    private void ExcluirMensalidade() { Console.WriteLine("Teste"); }
}