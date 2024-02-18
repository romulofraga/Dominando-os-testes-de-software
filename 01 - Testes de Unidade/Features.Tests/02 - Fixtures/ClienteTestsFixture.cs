using System;
using Features.Clientes;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteTestsFixture>
    { }

    /// <summary>
    /// Represents a fixture for testing the <see cref="Cliente"/> class.
    /// </summary>
    public class ClienteTestsFixture : IDisposable
    {
        /// <summary>
        /// Generates a valid instance of the <see cref="Cliente"/> class.
        /// </summary>
        /// <returns>A valid <see cref="Cliente"/> instance.</returns>
        public Cliente GerarClienteValido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now);

            return cliente;
        }

        /// <summary>
        /// Generates an invalid instance of the <see cref="Cliente"/> class.
        /// </summary>
        /// <returns>An invalid <see cref="Cliente"/> instance.</returns>
        public Cliente GerarClienteInValido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "edu2edu.com",
                true,
                DateTime.Now);

            return cliente;
        }

        public void Dispose()
        {
        }
    }
}