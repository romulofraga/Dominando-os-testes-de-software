using System;
using Features.Core;
using FluentValidation;

namespace Features.Clientes;

public class Funcionario: Entity
{
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
    public Cargo Cargo { get; set; }
    public decimal Salario { get; set; }
    public string Documento { get; set; }

    public Funcionario()
    {
        DataCadastro = DateTime.Now;
    }

    public Funcionario(Guid newGuid, string nome, string sobrenome, string email, DateTime dataNascimento,
        DateTime dataCadastro, bool ativo, Cargo cargo, decimal salario, string documento)
    {
        Nome = nome;
        Sobrenome = sobrenome;
        Email = email;
        DataNascimento = dataNascimento;
        DataCadastro = dataCadastro;
        Ativo = ativo;
        Cargo = cargo;
        Salario = salario;
        Documento = documento;
    }

    public override bool EhValido()
    {
        ValidationResult = new FuncionarioValidacao().Validate(this);
        return ValidationResult.IsValid;
    }
}

public enum Cargo
{
    Medico,
    Enfermeiro,
    TecnicoEnfermagem,
    Jornalista,
    SupervisorManutencao
}

public class FuncionarioValidacao: AbstractValidator<Funcionario>   
{
    public FuncionarioValidacao()
    {
        RuleFor(funcionario => funcionario.Nome)
            .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o nome")
            .Length(2, 150).WithMessage("O nome deve ter entre 2 e 150 caracteres");
            
        RuleFor(funcionario => funcionario.Sobrenome)
            .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o sobrenome")
            .Length(2, 150).WithMessage("O sobrenome deve ter entre 2 e 150 caracteres");
        
        RuleFor(funcionario => funcionario.Email)
            .NotEmpty().EmailAddress().WithMessage("O e-mail fornecido não é válido");
        
        RuleFor(funcionario => funcionario.Documento)
            .NotEmpty()
            .Must(CpfValidation.EhValido).WithMessage("Documento fornecido inválido");
        
        RuleFor(funcionario => funcionario.DataNascimento)
            .NotEmpty()
            .Must(HaveMinimumAge)
            .WithMessage("O funcionario deve ter 18 anos ou mais");
        
        RuleFor(funcionario => funcionario.Id)
            .NotEqual(Guid.Empty);
    }
    
    private static bool HaveMinimumAge(DateTime birthDate)
    {
        return birthDate <= DateTime.Now.AddYears(-18);
    }
}