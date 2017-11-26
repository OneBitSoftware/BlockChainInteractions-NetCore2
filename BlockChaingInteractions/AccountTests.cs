namespace BlockChaingInteractions
{
    using Nethereum.Web3;
    using Xunit;

    public class AccountTests
    {
        Web3 _web3 = null;

        public AccountTests()
        {
            _web3 = new Web3("http://localhost:8545");
        }

        [Theory, 
            InlineData("0x6fab9ac64a2c86580f0c74478d8fcd175938d5a8", "password")]
        public async void UnlockAccount_ShouldReturnTrue(
            string accountAddress,
            string password)
        {
            var unlockResult = await _web3
                .Personal
                .UnlockAccount
                .SendRequestAsync(accountAddress, password, 60);

            Assert.True(unlockResult);
        }

        [Theory, 
            InlineData("0x6fab9ac64a2c86580f0c74478d8fcd175938d5a8")]
        public async void GetBalance_ShouldReturnGreaterThanZero(string accountAddress)
        {
            var balanceResult = await _web3.Eth.GetBalance.SendRequestAsync(accountAddress);

            Assert.True(balanceResult.Value > 0);
        }

        [Fact]
        public async void ListAccounts_ShouldReturnGreaterThanZero()
        {
            var accounts = await _web3.Eth.Accounts.SendRequestAsync();

            Assert.True(accounts.Length > 0);
        }
    }
}
