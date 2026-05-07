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
    
    // Retorna um objeto da classe Aluno (ou nulo)
    private Aluno CapturarAlunoSelecionado()
    {
        while (true)
        {
            Console.Write("Por gentileza, digite o ID ou o nome do(a) aluno(a): ");
            string termoBusca = Console.ReadLine();

            List<Aluno> alunosEncontrados = _aluno.Selecionar(termoBusca);

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
            foreach (Aluno a in alunosEncontrados)
            {
                Console.WriteLine($"ID: {a.Id} | Nome: {a.Nome} | CPF: {a.Cpf}");
            }

            Console.Write("\nPor gentileza, digite o ID exato do aluno: ");
            if (int.TryParse(Console.ReadLine(), out int idDigitado))
            {
                foreach (Aluno a in alunosEncontrados)
                {
                    if (a.Id == idDigitado) 
                    {
                        return a; 
                    }
                }
            }

            Console.WriteLine("\nHmm.. parece que esse ID é inválido!");
            Console.Write("Deseja tentar pesquisar novamente? (S/N): ");
            if (Console.ReadLine().ToUpper().Trim() == "S") continue;
            else return null; // Desistiu de tentar, retorna nulo
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
        Console.Write("Insira o celular do aluno(a): ");
        string celularAluno = Console.ReadLine();

        while (true)
        {
            Console.WriteLine("\n=== Novo aluno a ser cadastrado ===");
            Console.WriteLine($"Nome do Aluno: {nomeAluno}\nCPF: {cpfFormatado}\nE-mail: {emailAluno}\nCelular: {celularAluno}\n\n");
            Console.Write("Prosseguir com o cadastramento? (S/N): ");

            string resposta = Console.ReadKey().KeyChar.ToString().ToUpper().Trim(); // Aplicada a ideia sugerida pelo Sergio: Console.ReadKey. Tive que passar pra um .ToString se não estava dando erro. Ideia de prevenção de sempre passar para maiuscula e retirar espaços
            Console.ReadLine(); // estava cadastrando direto, sem mostrar nenhuma mensagem.
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
                break;
            }
            else
            {
                Console.WriteLine("Hmm... parece que essa opção é inválida! Por gentileza, digite S ou N");
            }

        }
    }
    private void ListarAlunos() // Criada uma classe Aluno.cs e posto os get/sets nos campos (nome, cpf, etc)
    {
        Console.Clear();
        Console.WriteLine("=== Relatório Geral de Alunos(as) ===\n");
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
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Pesquisa de Alunos(as) ===\n");
            Console.Write("Por gentileza, digite o ID ou nome do(a) aluno(a): ");
            string termoBusca = Console.ReadLine();
            List<Aluno> alunosDoBanco = _aluno.Selecionar(termoBusca); // eu busco a lista Aluno, coloco ela numa variável de nome alunosDoBanco e dou a ela o valor da função aluno.Selecionar, passando o termo de busca.
            //uma validação, inicialmente. Se não encontrar nada, já avisar pro usuário
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
            foreach (Aluno a in alunosDoBanco) // para cada Aluno que guardei na variável "a" presente no banco, vou printar seu ID, nome, cpf, email, etc.
            {
                Console.WriteLine($"ID: {a.Id}\nNome: {a.Nome}\nCPF: {a.Cpf}\nE-mail: {a.Email}\nCelular: {a.Celular}");
                Console.WriteLine("===============================");
            }
            Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu principal.");
            Console.ReadLine();

            break;
        }

    }
    private void EditarAluno()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Edição de Cadastros de Alunos(as) ===\n");
            Aluno alunoAtual = CapturarAlunoSelecionado();

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

            _aluno.Alterar(novoNome, novoCpf, novoEmail, novoCelular, alunoAtual.Id);

            Console.WriteLine("\nAlteração realizada com sucesso!");
            Console.Write("Por gentileza, pressione Enter para voltar ao menu principal.");
            Console.ReadLine();

            break; // quebra do looping. clearSem ele, sempre ficava retornando para editar o aluno novamente.
        }
    }
    private void DeletarAluno()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Deleção de Cadastros de Alunos(as) ===\n");
            Aluno alunoAtual = CapturarAlunoSelecionado();

            if (alunoAtual == null) return;
            // NOVIDADE: validação extra para confirmação
            Console.Clear();
            Console.WriteLine("Dados do(a) aluno(a) que será deletado: ");
            Console.WriteLine($"ID: {alunoAtual.Id} | Nome: {alunoAtual.Nome} | CPF: {alunoAtual.Cpf} | E-mail: {alunoAtual.Email} | Celular: {alunoAtual.Celular}");
            Console.Write("Atenção: essa ação não poderá ser desfeita! Deseja seguir com a exclusão do cadastro? (S/N): ");
            string resposta = Console.ReadKey().KeyChar.ToString().ToUpper().Trim();
            Console.ReadLine();
            if (resposta == "S")
            {
                _aluno.Deletar(alunoAtual.Id);
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
    private void LancarMensalidade()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Lançamento de Mensalidades ===");
            Aluno alunoAtual = CapturarAlunoSelecionado();
            
            if (alunoAtual == null) return;

            Console.WriteLine($"\nAluno(a): {alunoAtual.Nome}");
            Console.Write("Por gentileza, digite o valor da mensalidade (R$): ");

            if (double.TryParse(Console.ReadLine(), out double valorMensalidade))
            {
                Console.WriteLine("\n=== Confirmação de Lançamento de Mensalidade ===");
                Console.WriteLine($"ID: {alunoAtual.Id} | Nome: {alunoAtual.Nome} | CPF: {alunoAtual.Cpf} | Valor: {valorMensalidade}");
                Console.Write("Deseja seguir com a inserção da mensalidade? (S/N): ");
                string resposta = Console.ReadKey().KeyChar.ToString().ToUpper().Trim();
                Console.ReadLine();
                if (resposta == "S")
                {
                    _mensalidade.LancarMensalidade(alunoAtual.Id, valorMensalidade);
                    Console.WriteLine("\nMensalidade lançada com sucesso!");
                    Console.WriteLine("\nPressione Enter para retornar ao menu principal!");
                    Console.ReadLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Operação cancelada!");
                    Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                    Console.ReadLine();
                    break;
                }
            }
            else
            {
                Console.WriteLine("Valor inválido!");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                Console.ReadLine();
                break;
            }

        }
    }
    private void RegistrarPagamentoMensalidade()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Registro de Pagamento de Mensalidades ===");
            Aluno alunoAtual = CapturarAlunoSelecionado();
            
            if (alunoAtual == null) return;

            Console.WriteLine("=== Listagem de Faturas ===");
            Console.WriteLine($"ID: {alunoAtual.Id} | Aluno(a): {alunoAtual.Nome} | CPF: {alunoAtual.Cpf}");
            List<Mensalidade> faturas = _mensalidade.ListarMensalidades();

            if (faturas.Count == 0)
            {
                Console.WriteLine("Este(a) aluno(a) não possui nenhuma mensalidade lançada.");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao Menu principal");
                Console.ReadLine();
                break;
            }

            foreach (var m in faturas)
            {
                Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
            }

            Console.Write("Por gentileza, digite o ID da mensalidade que deseja pagar: ");
            if (int.TryParse(Console.ReadLine(), out int idMensalidade))
            {
                // antes a ideia era muito simples. Apenas inseria o id da mensalidade e o bool como true já era o suficente para o registro do pagamento.
                // quis incluir novas situações para melhorar a experiencia do programa, além de algumas validações de consistência
                Mensalidade faturaSelecionada = null;
                foreach (var f in faturas)
                {
                    if (f.Id == idMensalidade)
                    {
                        faturaSelecionada = f;
                    }
                }
                // novas inclusões
                if (faturaSelecionada != null)
                {
                    Console.Write("\nPor gentileza, caso queira deixar a data atual, mantenha sem preenchimento.\n Data do pagamento: ");
                    string dataInput = Console.ReadLine();
                    //basicamente, se o usuario deixar em branco, o aplicativo vai considerar que é hj. Mas, se não for nula ou em branco, vai dispor a data que o usuário digitou
                    DateTime dataPagamento = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(dataInput))
                    {
                        DateTime.TryParse(dataInput, out dataPagamento);
                    }

                    // assim como do cadastro de aluno, quis aplicar a mesma ideia de aparecerem as informações em tela e uma validação de confirmação.
                    Console.Clear();
                    Console.WriteLine("=== Confirmação de Pagamento ===");
                    Console.WriteLine($"\nID do Aluno: {alunoAtual.Id}\nAluno(a): {alunoAtual.Nome}\nValor da Mensalidade: {faturaSelecionada.ValorMensalidade}\nData do pagamento: {dataPagamento.ToShortDateString()}\n Status: {faturaSelecionada.Status}");
                    Console.Write("Por gentileza, confirmar o recebimento? (S/N): ");
                    if (Console.ReadLine().ToUpper().Trim() == "S")
                    {
                        bool sucesso = _mensalidade.RegistrarPagamento(idMensalidade, dataPagamento);
                        if (sucesso)
                        {
                            Console.WriteLine("\n=== Pagamento registrado com sucesso! ===");
                        }
                        else
                        {
                            Console.WriteLine("Hmm.. algo não deu certo. Tente novamente.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nHmm.. O ID da mensalidade inserido parece incorreto. Tente novamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Hmm.. o ID da mensalidade inserido parece incorreto. Tente novamente.");
                }

                Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu principal.");
                Console.ReadLine();
                break;
            }
        }
    }
    private void ListarMensalidades()
    {
        Console.Clear();
        Console.WriteLine("=== Relatório Geral de Mensalidades ===\n");

        List<Mensalidade> faturas = _mensalidade.ListarMensalidades();

        if (faturas.Count == 0)
        {
            Console.WriteLine("Hmm.. parece que não há nenhuma mensalidade cadastrada!");
            Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu principal.");
            Console.ReadLine();
            return;
        }
        // tentei estrutar como se fosse um relatório mesmo
        Console.WriteLine("ID MENSALIDADE | ID ALUNO | ALUNO(A)            | VALOR (R$)  | VENCIMENTO | STATUS");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        foreach (var m in faturas)
        {
            List<Aluno> buscaAluno = _aluno.Selecionar(m.AlunoId.ToString());
            string nomeAluno = "";

            if (buscaAluno.Count > 0)
            {
                nomeAluno = buscaAluno[0].Nome;
            }
            // li que os numeros negativos alinham à esquerda e os positivos à direita.
            Console.WriteLine($"{m.Id, -14} | {m.AlunoId, -8} | {nomeAluno, -20} | {m.ValorMensalidade, 11} | {m.DataVencimento.ToShortDateString(), -10} | {m.Status.ToUpper()}");
        }

        Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao menu Principal");
        Console.ReadLine();
    }
    private void VerificaPendencias()
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("=== Verificação de Pendências ===");
            Aluno alunoAtual = CapturarAlunoSelecionado();
            
            if (alunoAtual == null) return;

            Console.Clear();
            Console.WriteLine("=== Listagem de Pendências ===");
            Console.WriteLine($"ID: {alunoAtual.Id} | Aluno(a): {alunoAtual.Nome} | CPF: {alunoAtual.Cpf}\n");
            List<Mensalidade> pendencias = _mensalidade.VerificaPendencias(alunoAtual.Id);

            if (pendencias.Count == 0)
            {
                Console.WriteLine("Maravilha! Este(a) aluno(a) não possui mensalidades pendentes!");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao Menu principal.");
                Console.ReadLine();
                break;
            }
            else
            {
                Console.WriteLine("\nHmm.. encontramos as seguintes pendências\n");
                foreach (var m in pendencias)
                {
                    Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
                }
            }

            Console.WriteLine("\nPor gentileza, pressione Enter para voltar ao menu principal");
            Console.ReadLine();
            break;
        }
    }
    private void EditarMensalidade()
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("=== Edição de Mensalidades ===");
            Aluno alunoAtual = CapturarAlunoSelecionado();
            
            if (alunoAtual == null) return;

            Console.WriteLine($"\n=== Mensalidades de {alunoAtual.Nome} ===");
            List<Mensalidade> todasFaturas = _mensalidade.ListarMensalidades();
            List<Mensalidade> faturasDoAluno = new List <Mensalidade>();
            
            foreach(var fatura in todasFaturas)
            {
                if(fatura.AlunoId == alunoAtual.Id) faturasDoAluno.Add(fatura);
            }

            if (faturasDoAluno.Count == 0)
            {
                Console.WriteLine("\nHmm.. parece que ainda não há mensalidades cadastradas!");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                Console.ReadLine();
                break;
            }

            foreach (var m in faturasDoAluno)
            {
                Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento.ToShortDateString()} | Status: {m.Status}");
            }

            Console.Write("\nPor gentileza, digite o ID da mensalidade que deseja editar: ");
            if (!int.TryParse(Console.ReadLine(), out int idMensalidade))
            {
                Console.WriteLine("Hmm.. parece que esse ID é inválido!");
                Console.ReadLine();
                break;
            }

            Mensalidade faturaAtual = null;
            foreach (var f in faturasDoAluno)
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
                break;
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

            Console.Write($"Status atual (pendente/pago/atrasado): ");
            string novoStatus = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoStatus))
            {
                novoStatus = faturaAtual.Status;
            }

            // como não sei se a faturaAtual tem ou não data de pagamento, vou dispor como padrão nula. 
            DateTime? novaDataPagamento = null;
            Console.Write("Data de pagamento (mantenha em branco caso não haja): ");
            string strDataPag = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(strDataPag))
            {
                // mas se o usuário digitou uma data, vou capturá-la e salvar como novaDataPagamento.
                if (DateTime.TryParse(strDataPag, out DateTime novaData))
                {
                    novaDataPagamento = novaData;
                }
            }
          

            bool sucesso = _mensalidade.EditarMensalidade(novoValor, novoVencimento, novoStatus, faturaAtual.Id, novaDataPagamento);

            if (sucesso)
            {
                Console.WriteLine("\nMensalidade atualizada com sucesso!");
            }

            Console.WriteLine("\nPor gentileza, pressione Enter para voltar ao menu.");
            Console.ReadLine();
            break;
        }
    }
    private void ExcluirMensalidade()
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("=== Exclusão de Mensalidades ===\n");
            Aluno alunoAtual = CapturarAlunoSelecionado();
            
            if (alunoAtual == null) return;

            Console.WriteLine($"=== Mensalidades de {alunoAtual.Nome} ==="); // MELHORIA FEITA! - no futuro, listar todas as mensaliaddes deste aluno antes de excluir, além de uma dupla validação
            
            // aqui a ideia foi buscar do banco todas as mensalidades e criar uma lista nova e ainda vazia apenas para o aluno selecionado           
            List<Mensalidade> todasFaturas = _mensalidade.ListarMensalidades(); 
            List<Mensalidade> faturasDoAluno = new List<Mensalidade>();
            
            foreach(var fatura in todasFaturas)
            {
                if(fatura.AlunoId == alunoAtual.Id)
                {
                    faturasDoAluno.Add(fatura);
                }
            }

            // se não houver mensalidades disponíveis para excluir
            if (faturasDoAluno.Count == 0)
            {
                Console.WriteLine("Este(a) aluno(a) não possui nenhuma mensalidade registrada!");
                Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao Menu principal.");
                Console.ReadLine();
                break;
            }

            // exibição das faturas em tela
            foreach(var m in faturasDoAluno)
            {
                Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor {m.ValorMensalidade} | Vencimento: {m.DataVencimento.ToShortDateString()} | Status: {m.Status}");
            }

            Console.Write("\nPor gentileza, digite o ID da mensalidade que deseja excluir:");

            if (int.TryParse(Console.ReadLine(), out int idMensalidade))
            {
                Mensalidade faturaSelecionada = null;
                foreach(var f in faturasDoAluno)
                {
                    if (f.Id == idMensalidade)
                    {
                        faturaSelecionada = f;
                        break;
                    }
                }

                // agora sim, caso encontrada a mensalidade digitada:
                if (faturaSelecionada != null)
                {
                    // dupla validação
                    Console.Clear();
                    Console.WriteLine("=== Exclusão de Mensalidades ===");
                    Console.WriteLine("\nImportante: essa ação não poderá ser desfeita!\n");
                    Console.WriteLine($"\nAluno(a): {alunoAtual.Nome}\nID da Mensalidade: {faturaSelecionada.Id}\nValor: {faturaSelecionada.ValorMensalidade}\nVencimento: {faturaSelecionada.DataVencimento}\nStatus: {faturaSelecionada.Status})");
                    Console.Write($"\nPor gentileza, você deseja seguir com a exclusão da mensalidade {faturaSelecionada.Id}? (S/N): ");

                    if(Console.ReadLine().ToUpper().Trim() == "S")
                    {
                        _mensalidade.ExcluirMensalidade(idMensalidade);
                        Console.WriteLine("\nMensalidade excluída com sucesso!");     
                    }
                    else
                    {
                        Console.WriteLine("Operação cancelada!");
                    }
                }
                else
                {
                    Console.WriteLine("\nHmm.. parece que esse ID é inválido. Tente novamente.");
                }

                Console.WriteLine("\nPor gentileza, pressione Enter para retornar ao Menu principal!");
                Console.ReadLine();

                break;

            }
        }
    }
}