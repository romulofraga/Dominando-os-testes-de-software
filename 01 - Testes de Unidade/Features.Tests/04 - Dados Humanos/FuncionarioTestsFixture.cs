using System;
using System.Collections.Generic;
using Bogus;
using Bogus.Extensions.Brazil;
using Features.Clientes;

namespace Features.Tests;

public class FuncionarioTestsFixture: IDisposable
{
    public List<Funcionario> CriarFuncionario( bool isAtivo = true)
    {
        var funcionarioFactory = new Faker<Funcionario>("pt_BR")
            .CustomInstantiator(f => new Funcionario(
                Guid.NewGuid(),
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Internet.Email(),
                f.Date.Past(80, DateTime.Now.AddYears(-18)),
                DateTime.Now,
                isAtivo,
                f.PickRandom<Cargo>(),
                f.Random.Decimal(1000, 5000),
                f.Person.Cpf()
            ));
        return funcionarioFactory.Generate(20);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}