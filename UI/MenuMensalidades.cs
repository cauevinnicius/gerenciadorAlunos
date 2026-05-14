namespace GerenciadorAlunos.UI;
using GerenciadorAlunos.Entities;
using GerenciadorAlunos.Repositories;
public class MenuMensalidades
{
    private readonly AlunoSQL _aluno;
    private readonly MensalidadeSQL _mensalidade;
    private readonly MenuAluno _menuAlunoAux;

    internal MenuMensalidades(AlunoSQL aluno, MensalidadeSQL mensalidade)
    {
        _aluno = aluno;
        _mensalidade = mensalidade;
        // Instanciando o auxiliar, passando a conexão do db
        _menuAlunoAux = new MenuAluno(_aluno); 
    }

    enum ListaOpcoes { LancarMensalidade = 1, RegistrarPagamentoMensalidade, ListarMensalidades, VerificaPendencias, EditarMensalidade, ExcluirMensalidade, Voltar}

    public void ExibirMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Menu de Mensalidades ===\n");
            Console.WriteLine("(1) Cadastrar Mensalidade\n(2) Registrar Pagamento\n(3) Relatório de Mensalidades\n(4) Verificar pendências/atrasos\n(5) Editar Mensalidade\n(6) Excluir Mensalidade\n(7) Voltar");
            Console.Write("\nPor gentileza, escolha a opção desejada: ");
            int.TryParse(Console.ReadLine(), out int opInt);

            if (opInt > 0 && opInt <= 7)
            {
                ListaOpcoes escolha = (ListaOpcoes)opInt;
                switch (escolha)
                {
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

                    case ListaOpcoes.Voltar:
                        return;
                }
            }
        }
    }

    private void LancarMensalidade()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Lançamento de Mensalidades ===");
            // Utilizando o auxiliar para devolver o resultado
            Aluno alunoAtual = _menuAlunoAux.CapturarAlunoSelecionado();
            
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
            Aluno alunoAtual = _menuAlunoAux.CapturarAlunoSelecionado();
            
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

            // Usando foreach tradicional
            foreach (var m in faturas)
            {
                Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
            }
            // Usando LAMBDA
            // faturas.ForEach(m => Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}"));

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
                // Usando LAMBDA
                // faturas.FirstOrDefault(f => f.Id == idMensalidade);
                
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
            // Variável vazia para depois incluir nela a primeira posição da listagem
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
            Aluno alunoAtual = _menuAlunoAux.CapturarAlunoSelecionado();
            
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
                // Tradicional foreach
                foreach (var m in pendencias)
                {
                    Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}");
                }
                // Usando LAMBDA
                // pendencias.ForEach(m => Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento} | Status: {m.Status}"));
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
            Aluno alunoAtual = _menuAlunoAux.CapturarAlunoSelecionado();
            
            if (alunoAtual == null) return;

            Console.WriteLine($"\n=== Mensalidades de {alunoAtual.Nome} ===");
            // Buscando todas as faturas 
            List<Mensalidade> todasFaturas = _mensalidade.ListarMensalidades();
            // Criando uma variável faturasDoAluno vazia
            List<Mensalidade> faturasDoAluno = new List <Mensalidade>();
            
            foreach(var fatura in todasFaturas)
            {   
                // Se passar na validação dos IDs, então vamos incluir as faturas dentro da nossa lista faturasDoAluno
                if(fatura.AlunoId == alunoAtual.Id) faturasDoAluno.Add(fatura);
            }

            // Usando LAMBDA
            // Criar uma lista de mensalidades chamada faturasDoAluno, pegando todas as faturas onde cada fatura tenha o Id do Aluno igual ao Id do alunoAtual e transforme em uma lista.
            // List<Mensalidade> faturasDoAluno = todasFaturas.Where(f => f.AlunoId == alunoAtual.Id).ToList();

            if (faturasDoAluno.Count == 0)
            {
                Console.WriteLine("\nHmm.. parece que ainda não há mensalidades cadastradas!");
                Console.WriteLine("Por gentileza, pressione Enter para retornar ao menu principal.");
                Console.ReadLine();
                break;
            }
            // Foreach tradicional
            foreach (var m in faturasDoAluno)
            {
                Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento.ToShortDateString()} | Status: {m.Status}");
            }

            // Usando LAMBDA
            // faturasDoAluno.ForEach(m => Console.WriteLine($"ID da Mensalidade: {m.Id} | Valor: {m.ValorMensalidade} | Data de Vencimento: {m.DataVencimento.ToShortDateString()} | Status: {m.Status}"));

            Console.Write("\nPor gentileza, digite o ID da mensalidade que deseja editar: ");
            if (!int.TryParse(Console.ReadLine(), out int idMensalidade))
            {
                Console.WriteLine("Hmm.. parece que esse ID é inválido!");
                Console.ReadLine();
                break;
            }

            Mensalidade faturaAtual = null;
            // Tradicional foreach
            foreach (var f in faturasDoAluno)
            {
                if (f.Id == idMensalidade)
                {
                    faturaAtual = f;
                }
            }

            // Usando LAMBDA
            // faturasDoAluno.FirstOrDefault(f => f.Id == idMensalidade);

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

            // Como não sei se a faturaAtual tem ou não data de pagamento, vou dispor como padrão nula. 
            DateTime? novaDataPagamento = null;
            Console.Write("Data de pagamento (mantenha em branco caso não haja): ");
            string strDataPag = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(strDataPag))
            {
                // Mas se o usuário digitou uma data, vou capturá-la e salvar como novaDataPagamento.
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
            Aluno alunoAtual = _menuAlunoAux.CapturarAlunoSelecionado();
            
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

            // Usando LAMBDA (copiada da EditarMensalidade)
            // Criar uma lista de mensalidades chamada faturasDoAluno, pegando todas as faturas onde cada fatura tenha o Id do Aluno igual ao Id do alunoAtual e transforme em uma lista.
            // List<Mensalidade> faturasDoAluno = todasFaturas.Where(f => f.AlunoId == alunoAtual.Id).ToList();

            // Se não houver mensalidades disponíveis para excluir
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

            // Usando LAMBDA
            // faturasDoAluno.ForEach(m => Console.WriteLine("ID da Mensalidade: {m.Id} | Valor {m.ValorMensalidade} | Vencimento: {m.DataVencimento.ToShortDateString()} | Status: {m.Status}"));

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
                // faturasDoAluno.FirstOrDefault(f => f.Id == idMensalidade);

                // Agora sim, caso encontrada a mensalidade digitada (diferente de nula):
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