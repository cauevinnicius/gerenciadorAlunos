public class Menu
{

    enum ListaOpcoes { CadastrarAluno = 1, ListarAlunos, BuscarAluno, EditarAluno, DeletarAluno, LancarMensalidade, RegistrarPagamentoMensalidade, ListarMensalidades, VerificaPendencias, EditarMensalidade, ExcluirMensalidade, Sair }

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
                    case ListaOpcoes.ListarMensalidades:
                        ListarMensalidades();
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
    private void CadastrarAluno() // a minha ideia era fazer uma classe Cadastro e uma função CadastrarALuno(). *Final do Projeto - melhoria!
    {
        Console.Clear();
        Console.WriteLine("=== Menu de Cadastro de Aluno ===\n");
        Console.Write("Insira o nome do aluno(a): ");
        string nomeAluno = Console.ReadLine();
        string cpfFormatado = ""; // Se fosse um int, os zeros à esquerda seiram retirados. Além disso, é possível que o usuário digite pontos e traços
        bool cpfValido = false;

        while (cpfValido == false)
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
            Console.WriteLine($"Nome do Aluno: {nomeAluno}\nCPF: {cpfFormatado}\nE-mail: {emailAluno}\nCelular: {celularAluno}\n\n");
            Console.Write("Prosseguir com o cadastramento? (S/N): ");
            string resposta = Console.ReadKey().KeyChar.ToString().ToUpper().Trim(); // Aplicada a ideia sugerida pelo Sergio: Console.ReadKey. Tive que passar pra um .ToString se não estava dando erro. Ideia de prevenção de sempre passar para maiuscula e retirar espaços
            if (resposta == "S")
            {
                _aluno.Cadastrar(nomeAluno, cpfFormatado, emailAluno, celularAluno);
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
    private void BuscarAluno() //aqui me baseei na ideia anterior, mas incluindo mais validações
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
        //Console.WriteLine("Digite o ID ou o nome do aluno(a) que deseja alterar: "); // retirei pq já está vindo na minha _aluno.Selecionar.
        string termoBusca = Console.ReadLine(); // nessa daqui, diferente da BuscarAluno, quis criar uma variável pra guardar o que o usuário digitou.

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca); // aqui. Na Buscar eu coloquei o Console.ReadLine direto. Ver com o Sérgio é melhor (acredito q essa aqui)
                                                                       // pra nao precisaar refazer tudo, usei a função do selecionar

        if (alunosEncontrados.Count == 0) // validacao
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null; // criei um objeto da classe Aluno nulo - pesquisei na internet para auxílio

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0]; // a ideia aqui se baseia em encontrar 1 único aluno e retornar a posição 0 (ele mesmo) da nossa lista de alunosEncontrados
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados) // para cada aluno encontrado, vou mostrar suas informações na tela.
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }
        }

        Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja editar: ");
        if (int.TryParse(Console.ReadLine(), out int idDigitado)) // ta, basicamente eu to fazendo a validacao pra gerar um int do idDigitado
        {
            foreach (Aluno a in alunosEncontrados) // faz sentido buscar apenas o único.
            {
                if (a.Id == idDigitado) // o ID do aluno é o mesmo que o digitado?
                {
                    alunoAtual = a; // se sim, vou pegar meu objeto alunoAtual que disse que ele era nulo inicialmente e vou dar a ele o valor do objeto a 
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

        Console.WriteLine("\nObservação: será possível deixar em branco e pressionar Enter para manter o dado atual.");

        Console.Write($"Nome atual ({alunoAtual.Nome}): ");
        string novoNome = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoNome)) novoNome = alunoAtual.Nome; // entender melhor com o sérgio. Mas entendo que seria se o novoNome for nulo ou vazio, vou guardar o nome antigo. Se não, vou manter o nome novo.   

        Console.Write($"CPF atual ({alunoAtual.Cpf}): ");
        string novoCpf = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoCpf)) novoCpf = alunoAtual.Cpf;

        Console.Write($"E-mail atual ({alunoAtual.Email}): ");
        string novoEmail = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoEmail)) novoEmail = alunoAtual.Email;


        Console.Write($"Celular atual ({alunoAtual.Celular}): ");
        string strCelular = Console.ReadLine();
        int novoCelular = alunoAtual.Celular; // aqui variável já começa com o número antigo

        if (!string.IsNullOrWhiteSpace(strCelular))
        {
            // tentativa de conversão. Se der certo, substitui a variável
            int.TryParse(strCelular, out novoCelular);
        }

        _aluno.Alterar(novoNome, novoCpf, novoEmail, novoCelular, alunoAtual.Id);

        Console.WriteLine("\nAlteração realizada com sucesso!");
        Console.ReadLine();
    }
    private void DeletarAluno()
    {
        Console.Clear();
        Console.WriteLine("=== Edição de Cadastros de Alunos(as) ===\n");
        //Console.WriteLine("Digite o ID ou o nome do aluno(a) que deseja alterar: "); retirei pq já esta vindo na minha _aluno.Selecionar.
        string termoBusca = Console.ReadLine(); // nessa daqui, diferente da BuscarAluno, quis criar uma variável pra guardar o que o usuário digitou.

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca); // aqui. Na Buscar eu coloquei o Console.ReadLine direto. Ver com o Sérgio é melhor (acredito q essa aqui)
                                                                       // pra nao precisaar refazer tudo, usei a função do selecionar

        if (alunosEncontrados.Count == 0) // validacao
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null; // criei um objeto da classe Aluno nulo - pesquisei na internet para auxílio

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0]; // a ideia aqui se baseia em encontrar 1 único aluno e retornar a posição 0 (ele mesmo) da nossa lista de alunosEncontrados
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados) // para cada aluno encontrado, vou mostrar suas informações na tela.
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }
        }

        Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja deletar: ");
        if (int.TryParse(Console.ReadLine(), out int idDigitado)) // ta, basicamente eu to fazendo a validacao pra gerar um int do idDigitado
        {
            foreach (Aluno a in alunosEncontrados) // o unico, faz mais sentido.
            {
                if (a.Id == idDigitado) // o ID do aluno é o mesmo que o digitado?
                {
                    alunoAtual = a; // se sim, vou pegar meu objeto alunoAtual que disse que ele era nulo inicialmente e vou dar a ele o valor do objeto a 
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

        _aluno.Deletar(alunoAtual.Id);

        Console.WriteLine("\nDeleção realizada com sucesso!");
        Console.ReadLine();
    }
    private void LancarMensalidade() // basicamente reutilizei toda validação que construi antes, apenas implementando a funcionalidade de lançamento em si
    {
        Console.Clear();
        Console.WriteLine("=== Lançamento de Mensalidades ===");
        Console.WriteLine("Por gentileza, digite o ID ou o nome do(a) aluno(a)");
        string termoBusca = Console.ReadLine();

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca);

        if (alunosEncontrados.Count == 0) // validacao
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null; // criei um objeto da classe Aluno nulo - pesquisei na internet para auxílio

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0]; // a ideia aqui se baseia em encontrar 1 único aluno e retornar a posição 0 (ele mesmo) da nossa lista de alunosEncontrados
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados) // para cada aluno encontrado, vou mostrar suas informações na tela.
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }
        }

        Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja lançar a mensalidade: ");
        if (int.TryParse(Console.ReadLine(), out int idDigitado)) // ta, basicamente eu to fazendo a validacao pra gerar um int do idDigitado
        {
            foreach (Aluno a in alunosEncontrados)
            {
                if (a.Id == idDigitado)
                {
                    alunoAtual = a;
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

        Console.WriteLine($"Aluno(a): {alunoAtual.Nome}");
        Console.Write("Por gentileza, digite o valor da mensalidade (R$)");

        if (double.TryParse(Console.ReadLine(), out double valorMensalidade))
        {
            _mensalidade.LancarMensalidade(alunoAtual.Id, valorMensalidade);
            Console.WriteLine("\nMensalidade lançada com sucesso!\n Pressione Enter para retornar ao menu principal!");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Valor inválido!");
        }

        Console.ReadLine();

    }
    private void RegistrarPagamentoMensalidade()
    {
        Console.Clear();
        Console.WriteLine("=== Registro de Pagamento de Mensalidades ===");
        Console.WriteLine("Por gentileza, digite o ID ou o nome do(a) aluno(a)");
        string termoBusca = Console.ReadLine();

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca);

        if (alunosEncontrados.Count == 0)
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null;

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0];
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }

            Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja lançar a mensalidade: ");
            if (int.TryParse(Console.ReadLine(), out int idDigitado))
            {
                foreach (Aluno a in alunosEncontrados)
                {
                    if (a.Id == idDigitado)
                    {
                        alunoAtual = a;
                        break;
                    }
                }
            }
        }

        if (alunoAtual == null)
        {
            Console.WriteLine("\nHmm.. parece que esse ID é inválido! Tente novamente.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Aluno(a): {alunoAtual.Nome}");
        // aqui seria a melhoria de mostrar na tela todas as mensalidades (uma lista) do aluno e poder verificar com precisão o que está pendente.
        List<Mensalidade> faturas = _mensalidade.ListarMensalidades(alunoAtual.Id);

        if (faturas.Count == 0)
        {
            Console.WriteLine("Este(a) aluno(a) não possui nenhuma mensalidade lançada.");
            Console.ReadLine();
            return;
        }
        foreach (var m in faturas)
        {
            Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
        }

        Console.Write("Por gentileza, digite o ID da mensalidade que deseja pagar: ");
        if (int.TryParse(Console.ReadLine(), out int idMensalidade))
        {
            bool idExiste = false;
            foreach (var f in faturas)
            {
                if (f.Id == idMensalidade)
                {
                    idExiste = true;
                }
            }
            if (idExiste)
            {
                // tinha, inicialmente, só o bool sucesso. Mas quis fazer mais uma validação (acima) para caso o usuário digite um ID aleatório q nao exista
                bool sucesso = _mensalidade.RegistrarPagamento(idMensalidade);
                if (sucesso)
                {
                    Console.WriteLine("Pagamento registrado! O status foi alterado para 'pago'");
                }
            }
            else
            {
                Console.WriteLine("Hmm.. o ID da mensalidade parece incorreto. Tente novamente.");
            }
        }
        else
        {
            Console.WriteLine("Hmm.. parece que esse ID de mensalidade é inválido. Tente novamente.");
        }

        Console.ReadLine();
    }
    private void ListarMensalidades()
    {
        Console.Clear();
        Console.WriteLine("=== Registro de Pagamento de Mensalidades ===");
        Console.WriteLine("Por gentileza, digite o ID ou o nome do(a) aluno(a)");
        string termoBusca = Console.ReadLine();

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca);

        if (alunosEncontrados.Count == 0)
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null;

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0];
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }

            Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja lançar a mensalidade: ");
            if (int.TryParse(Console.ReadLine(), out int idDigitado))
            {
                foreach (Aluno a in alunosEncontrados)
                {
                    if (a.Id == idDigitado)
                    {
                        alunoAtual = a;
                        break;
                    }
                }
            }
        }

        if (alunoAtual == null)
        {
            Console.WriteLine("\nHmm.. parece que esse ID é inválido! Tente novamente.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Aluno(a): {alunoAtual.Nome}");
        List<Mensalidade> faturas = _mensalidade.ListarMensalidades(alunoAtual.Id);

        if (faturas.Count == 0)
        {
            Console.WriteLine("Este(a) aluno(a) não possui nenhuma mensalidade lançada.");
            Console.ReadLine();
            return;
        } // será q eu nao deveria ter posto um else aqui e o foreach dentro do else?
        foreach (var m in faturas)
        {
            Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
        }
    }
    private void VerificaPendencias()
    {
        Console.Clear();
        Console.WriteLine("=== Registro de Pagamento de Mensalidades ===");
        Console.WriteLine("Por gentileza, digite o ID ou o nome do(a) aluno(a)");
        string termoBusca = Console.ReadLine();

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca);

        if (alunosEncontrados.Count == 0)
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null;

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0];
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }

            Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja lançar a mensalidade: ");
            if (int.TryParse(Console.ReadLine(), out int idDigitado))
            {
                foreach (Aluno a in alunosEncontrados)
                {
                    if (a.Id == idDigitado)
                    {
                        alunoAtual = a;
                        break;
                    }
                }
            }
        }

        if (alunoAtual == null)
        {
            Console.WriteLine("\nHmm.. parece que esse ID é inválido! Tente novamente.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Hmm.. encontramos algumas pendências do(a) aluno(a): {alunoAtual.Nome}\n");
        List<Mensalidade> pendencias = _mensalidade.VerificaPendencias(alunoAtual.Id);

        if (pendencias.Count == 0)
        {
            Console.WriteLine("Maravilha! Este(a) aluno(a) não possui mensalidades pendentes!");
        }
        else
        {
            foreach (var m in pendencias)
            {
                Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
            }
        }

        Console.WriteLine("Por gentileza, pressione Enter para voltar ao menu principal");
        Console.ReadLine();
    }
    private void EditarMensalidade()
    {
        Console.Clear();
        Console.WriteLine("=== Registro de Pagamento de Mensalidades ===");
        Console.WriteLine("Por gentileza, digite o ID ou o nome do(a) aluno(a)");
        string termoBusca = Console.ReadLine();

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca);

        if (alunosEncontrados.Count == 0)
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null;

        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0];
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }

            Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja lançar a mensalidade: ");
            if (int.TryParse(Console.ReadLine(), out int idDigitado))
            {
                foreach (Aluno a in alunosEncontrados)
                {
                    if (a.Id == idDigitado)
                    {
                        alunoAtual = a;
                        break;
                    }
                }
            }
        }

        if (alunoAtual == null)
        {
            Console.WriteLine("\nHmm.. parece que esse ID é inválido! Tente novamente.");
            Console.ReadLine();
            return;
        }

        List<Mensalidade> faturas = _mensalidade.ListarMensalidades(alunoAtual.Id);

        if (faturas.Count == 0)
        {
            Console.WriteLine("\nEste aluno não possui nenhuma mensalidade lançada.");
            Console.ReadLine();
            return;
        }
        else
        {
            Console.WriteLine($"Mensalidades do(a) Aluno(a): {alunoAtual.Nome}");
            foreach (var m in faturas)
            {
                Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
            }
        }

        Console.WriteLine("\nPor gentileza, digite o ID da mensalidade que deseja editar: ");
        if (!int.TryParse(Console.ReadLine(), out int idMensalidade))
        {
            Console.WriteLine("Hmm.. parece que esse ID é inválido!");
            Console.ReadLine();
            return;
        }

        Mensalidade faturaAtual = null;
        foreach (var f in faturas)
        {
            if (f.Id == idMensalidade)
            {
                faturaAtual = f;
            }
        }

        if (faturaAtual == null)
        {
            Console.WriteLine("Hmm.. não encontrei essa mensalidade");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("\nPor gentileza, mantenha sem preenchimento e pressione Enter para manter os dados atuais");

        Console.WriteLine($"Mensalidade atual (R$): {faturaAtual.ValorMensalidade}");
        string strValor = Console.ReadLine();
        decimal novoValor = faturaAtual.ValorMensalidade;
        if (!string.IsNullOrWhiteSpace(strValor))
        {
            decimal.TryParse(strValor, out novoValor);
        }

        Console.WriteLine($"Data de vencimento atual: {faturaAtual.DataVencimento}");
        string strVenc = Console.ReadLine();
        DateTime novoVencimento = faturaAtual.DataVencimento;
        if (!string.IsNullOrWhiteSpace(strVenc))
        {
            DateTime.TryParse(strVenc, out novoVencimento);
        }

        Console.WriteLine($"Status atual: {faturaAtual.Status}");
        string novoStatus = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoStatus))
        {
            novoStatus = faturaAtual.Status;
        }

        DateTime? novaDataPagamento = faturaAtual.DataPagamento != DateTime.MinValue ? faturaAtual.DataPagamento : (DateTime?)null;
        if (faturaAtual.DataPagamento != DateTime.MinValue)
        {
            Console.WriteLine($"Data de pagamento atual: {faturaAtual.DataPagamento.ToShortDateString()}");
            string strDataPag = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(strDataPag))
            {
                if (DateTime.TryParse(strDataPag, out DateTime novaData))
                {
                    novaDataPagamento = novaData;
                }
            }
        }

        bool sucesso = _mensalidade.EditarMensalidade(novoValor, novoVencimento, novoStatus, novaDataPagamento, faturaAtual.Id);

        if (sucesso)
        {
            Console.WriteLine("\nMensalidade atualizada com sucesso!");
        }

        Console.WriteLine("\nPressione Enter para voltar ao menu.");
        Console.ReadLine();
    }
    private void ExcluirMensalidade()
    {
        Console.Clear();
        Console.WriteLine("=== Exclusão de Mensalidades ===\n");
        Console.WriteLine("Por gentileza, digite, o ID ou o nome da aluno(a):");
        string termoBusca = Console.ReadLine();

        List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca);

        if (alunosEncontrados.Count == 0)
        {
            Console.WriteLine("Hmm.. Não encontrei nenhum aluno com esse nome ou ID.");
            Console.ReadLine();
            return;
        }

        Aluno alunoAtual = null;


        if (alunosEncontrados.Count == 1)
        {
            alunoAtual = alunosEncontrados[0];
        }
        else
        {
            Console.WriteLine("\nHmm.. Encontramos mais de um aluno com esse nome.");
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }
            Console.WriteLine("\nPor gentileza, digite o ID do aluno que deseja editar: ");
            if (int.TryParse(Console.ReadLine(), out int idDigitado))
            {
                foreach (Aluno a in alunosEncontrados)
                {
                    if (a.Id == idDigitado)
                    {
                        alunoAtual = a;
                        break;
                    }
                }
            }
        }

        if (alunoAtual == null)
        {
            Console.WriteLine("\nHmm.. parece que esse ID é inválido! Tente novamente.");
            Console.ReadLine();
            return;
        }
        Console.WriteLine($"Aluno(a): {alunoAtual.Nome}"); // MELHORIA: no futuro, listar todas as mensaliaddes deste aluno antes de excluir, além de uma dupla validação
        Console.Write("Por gentileza, digite o ID da mensalidade que deseja excluir:");

        if (int.TryParse(Console.ReadLine(), out int idMensalidade))
        {
            _mensalidade.ExcluirMensalidade(idMensalidade);
            Console.WriteLine("\nMensalidade excluída com sucesso!\n Pressione Enter para retornar ao menu principal!");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("ID inválido! Por gentileza, tente novamente.");
        }

        Console.ReadLine();
    }
}